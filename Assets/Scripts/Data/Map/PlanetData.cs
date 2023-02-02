using System;
using Systems.Physics;
using UnityEngine;

namespace Data.Map
{
    [Serializable]
    public struct PlanetData
    {
        public bool Inhabited => population > 0;
        public float SurfaceGravity => PhysicsSystem.G * mass / Mathf.Pow(radius, 2);
        public float HoursInDay => 2 * Mathf.PI / angularVelocity;

        public string planetName;
        public Vector2 initPosition;
        public PlanetType planetType;
        public AtmosphereData atmoData;
        public float age;
        public float mass;
        public float radius;
        public float population;
        public float angularVelocity;
        public float surfaceRadiation;
        public float? seaLevel;
    }
}