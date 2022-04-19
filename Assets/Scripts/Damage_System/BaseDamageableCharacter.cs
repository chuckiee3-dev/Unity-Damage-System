using System;
using UnityEngine;

[Serializable]
public class BaseDamageableCharacter: IDamageable
{
    public int health { get; set; }
   
    [SerializeField] private ResistanceData<DamageType>[] damageResistances;
    [SerializeField] private ResistanceData<StatusBuildUp>[] statusResistances;

    private DamageProcessor _damageProcessor;
    private StatusBuildUpProcessor _statusBuildUpProcessor;
    public BaseDamageableCharacter(){}
    public void Initialize()
    {
        if(damageResistances is { Length: > 0 }){
            _damageProcessor = new DamageProcessor(ref damageResistances);
        }

        if (statusResistances is { Length: > 0 })
        {
            _statusBuildUpProcessor = new StatusBuildUpProcessor(ref statusResistances);
        }
        
        _statusBuildUpProcessor.onStatusEffectTrigger += InflictStatusEffect;
    }

    private void InflictStatusEffect(StatusBuildUp triggeredStatus)
    {
        Debug.Log("Character inflicted by "+ triggeredStatus + " dealing " 
                  + triggeredStatus.Data.DamageOnTrigger + " extra damage.");
        
        //status effect damages are not affected by resistances
        health = health - triggeredStatus.Data.DamageOnTrigger;
    }

    public void Damage(DamageDealer damageDealer)
    {
       health = health - _damageProcessor.GetDamageDealtToHealth(damageDealer.damages);
       _statusBuildUpProcessor.ProcessDamage(damageDealer.statusBuildUps);
    }

    ~BaseDamageableCharacter()
    {
        _statusBuildUpProcessor.onStatusEffectTrigger -= InflictStatusEffect;
    }
}
