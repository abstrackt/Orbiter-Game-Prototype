using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Physics
{
    public class PhysicsBody : MonoBehaviour
    {
        public float EscapeVelocity => Mathf.Sqrt(2 * PhysicsSystem.G * mass * PhysicsSystem.PhysicsScale / interactRadius);

        [NonSerialized] public int bodyIndex = -1;
        [NonSerialized] public PhysicsSystem physicsSystem;
        [NonSerialized] public Renderer bodyRenderer;
        public float mass;
        public float interactRadius;
        public Vector2 initialVelocity;
        public Attractor cachedClosest;

        public void Awake()
        {
            bodyRenderer = GetComponent<Renderer>();
        }
        
        public bool PhysicsEnabled
        {
            get => physicsSystem.PhysicsEnabled(bodyIndex);
            set => physicsSystem.SetPhysics(bodyIndex, value);
        }

        public Vector2 GetVelocity()
        {
            return physicsSystem.GetVelocity(bodyIndex);
        }
        
        public void PositionOverride(Vector2 position)
        {
            physicsSystem.SetPosition(bodyIndex, position);
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
            var pos = transform.position;
            var att = physicsSystem.GetClosestAttractor(pos);
            return physicsSystem.GetTrajectory(bodyIndex, att.transform.position, att.mass);
        }
    }
}