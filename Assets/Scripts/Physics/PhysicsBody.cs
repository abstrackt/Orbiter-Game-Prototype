using System;
using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public class PhysicsBody : MonoBehaviour
    {
        [NonSerialized] public int bodyIndex = -1;
        [NonSerialized] public PhysicsSystem physicsSystem;
        [NonSerialized] public Attractor currentAttractor;
        public float mass;
        public Vector2 initialVelocity;
        public int predictionSteps;
    
        public bool PhysicsEnabled
        {
            get => physicsSystem.PhysicsEnabled(bodyIndex);
            set => physicsSystem.SetPhysics(bodyIndex, value);
        }

        public Vector2 GetVelocity()
        {
            return physicsSystem.GetVelocity(bodyIndex);
        }

        public void VelocityOverride(Vector2 velocity)
        {
            physicsSystem.SetVelocity(bodyIndex, velocity);
        }

        public void AddForce(Vector2 force)
        {
            physicsSystem.AddForce(bodyIndex, force);
        }

        public List<Vector2> PredictedTrajectory()
        {
            return physicsSystem.GetTrajectory(bodyIndex, predictionSteps);
        }
    }
}