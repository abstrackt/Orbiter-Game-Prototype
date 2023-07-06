using UnityEngine;

namespace Data.Map
{
    public struct StarData
    {
        public MapID id;
        public string starName;
        public Vector2 initPosition;
        public StarType starType;
        public float mass;
        public float age;
        public float radius;
        public float temperature; // effective temperature
        public float magneticField;
    }
}