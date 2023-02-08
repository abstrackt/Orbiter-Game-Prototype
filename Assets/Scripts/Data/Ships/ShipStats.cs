using UnityEngine;

namespace Data.Ships
{
    [CreateAssetMenu(fileName = "Ship Data", menuName = "Data/Stats/Ship Stats", order = 1)]
    public class ShipStats : ScriptableObject
    {
        public float thrust;
        public float dashRange;
        public float maxFuel;
        public float consumptionRate;
        public float refuelRange;
    }
}