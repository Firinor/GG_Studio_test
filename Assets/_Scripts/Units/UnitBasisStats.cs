using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "GameBalance/UnitStats")]
public class UnitBasisStats : ScriptableObject
{
    public float Attack = 15;

    public float HealthPoint = 100;
    public float MaxHealthPoint = 100;

    public float DefenceRate;
    public float MaxDefenceRate = 100;

    public float VampiricRate;
    public float MaxVampiricRate = 100;

    public int BuffLimit = 2;
}
