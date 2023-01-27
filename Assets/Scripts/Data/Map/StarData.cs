using UnityEngine;

namespace Data.Map
{
    public struct StarData
    {
        public Vector3 position;
        public StarType starType;
        public float mass;
        public float age;
        public float radius;
        public float temperature; // effective temperature
        public float magneticField;
    }
}