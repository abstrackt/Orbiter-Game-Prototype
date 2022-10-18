using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public class PhysicsSystem : MonoBehaviour
    {
        public List<Attractor> attractors = new List<Attractor>();
        public float physicsScale = 1;
        public bool defaultEnabled = true;

        private List<bool> _enabled;
        private List<Vector2> _positions;
        private List<Vector2> _velocities;
        private List<List<Vector2>> _forces;
        private PhysicsBody[] _physicsBodies;

        public const float G = 6.67f;

        public void Start()
        {
            _physicsBodies = FindObjectsOfType<PhysicsBody>();

            _positions = new List<Vector2>(_physicsBodies.Length);
            _velocities = new List<Vector2>(_physicsBodies.Length);
            _forces = new List<List<Vector2>>(_physicsBodies.Length);
            _enabled = new List<bool>(_physicsBodies.Length);

            for (int i = 0; i < _physicsBodies.Length; i++)
            {
                _physicsBodies[i].bodyIndex = i;
                _physicsBodies[i].physicsSystem = this;

                _positions.Add(_physicsBodies[i].transform.position);
                _velocities.Add(_physicsBodies[i].initialVelocity);
                _forces.Add(new List<Vector2>());
                _enabled.Add(defaultEnabled);
            }
        }

        // This gets called in a frame-independent update loop
        public void FixedUpdate()
        {
            Leapfrog();
            ResetForces();
        }

        // This just updates the display
        public void Update()
        {
            UpdateDisplay();
        }

        // Calculating velocities and positions with leapfrog integration. Ribbit.
        private void Leapfrog()
        {
            for (int i = 0; i < _physicsBodies.Length; i++)
            {
                if (!_enabled[i])
                    continue;

                var accel = GetNetAcceleration(i);
                var position = _positions[i];
                _positions[i] = position
                                + _velocities[i] * Time.fixedDeltaTime
                                + .5f * accel * Mathf.Pow(Time.fixedDeltaTime, 2);
                var nextAccel = GetNetAcceleration(i);
                _velocities[i] = _velocities[i]
                                 + .5f * (accel + nextAccel) * Time.fixedDeltaTime;
            }
        }

        private void UpdateDisplay()
        {
            for (int i = 0; i < _physicsBodies.Length; i++)
            {
                if (!_enabled[i])
                    continue;
                var position = _physicsBodies[i].transform.position;
                position.x = _positions[i].x;
                position.y = _positions[i].y;
                _physicsBodies[i].transform.position = position;
            }
        }

        public Vector2 Gravity(Vector2 point)
        {
            if (attractors.Count <= 0)
                return Vector2.zero;
            
            Attractor closest = attractors[0];
            var pos3 = closest.transform.position;
            var pos = new Vector2(pos3.x, pos3.y);
            var minDist = (point - pos).magnitude;
            foreach (var attractor in attractors)
            {
                pos3 = attractor.transform.position;
                pos = new Vector2(pos3.x, pos3.y);
                var dist = (point - pos).magnitude;
                if (dist >= minDist) continue;
                closest = attractor;
                minDist = dist;
            }
            
            var temp = closest.transform.position;
            var center = new Vector2(temp.x, temp.y);
            var accel = (center - point).normalized;
            var distance = (point - center).magnitude;
            var magnitude = G * closest.mass * physicsScale / Mathf.Pow(distance, 2);
            accel.Scale(new Vector2(magnitude, magnitude));

            return accel;
        }

        private Vector2 GetNetAcceleration(int index)
        {
            if (index < _forces.Count && index >= 0)
            {
                var point = _positions[index];
                var net = Gravity(point);
                var mass = _physicsBodies[index].mass;

                for (int j = 0; j < _forces[index].Count; j++)
                {
                    var accel = _forces[index][j];
                    accel.Scale(new Vector2(1 / mass, 1 / mass));
                    net += accel;
                }

                return net;
            }

            return new Vector2();
        }

        private void ResetForces()
        {
            for (int i = 0; i < _forces.Count; i++)
            {
                _forces[i].Clear();
            }
        }

        public void AddForce(int index, Vector2 force)
        {
            if (index < _forces.Count && index >= 0)
            {
                _forces[index].Add(force);
            }
        }

        public bool PhysicsEnabled(int index)
        {
            if (index < _forces.Count && index >= 0)
            {
                return _enabled[index];
            }

            return false;
        }

        public Vector2 GetVelocity(int index)
        {
            if (index < _velocities.Count && index >= 0)
            {
                return _velocities[index];
            }

            return default;
        }

        public void SetVelocity(int index, Vector2 v)
        {
            _velocities[index] = v;
        }

        public void SetPhysics(int index, bool enablePhysics, Vector2 overrideVelocity = default)
        {
            if (index < _enabled.Count && index >= 0)
            {
                if (!_enabled[index] && enablePhysics)
                {
                    // Update position that was changed due to non-physical movement
                    _positions[index] = _physicsBodies[index].transform.position;

                    // Change velocity vector too if needed
                    if (overrideVelocity != default)
                        _velocities[index] = overrideVelocity;
                }
                _enabled[index] = enablePhysics;
            }
        }
    }
}