using UnityEngine;
[System.Serializable]
public class DamageData
{
    [SerializeField] private DamageType type;
    [SerializeField] private int amount;
    public int Amount => amount;
    public DamageType GetDamageType => type;
}
