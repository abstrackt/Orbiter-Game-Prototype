using System;

namespace Data.Ships
{
    [Serializable]
    public struct ShipData
    {
        public float thrust;
        public float dashRange;
        public float maxFuel;
        public float consumptionRate;
        public float refuelRange;
    }
}