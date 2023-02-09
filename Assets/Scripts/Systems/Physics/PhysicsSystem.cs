using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Global;
using UnityEngine;
using UnityEngine.Profiling;

namespace Systems.Physics
{
    public class PhysicsSystem : SingletonMonoBehaviour<PhysicsSystem>
    {
        public List<Attractor> attractors = new ();
        public bool defaultEnabled = true;
        public PhysicsBody playerSpaceship;

        private List<bool> _enabled;
        private List<Vector2> _positions;
        private List<Vector2> _velocities;
        private List<List<Vector2>> _forces;
        private List<PhysicsBody> _physicsBodies;
        private bool _cacheInitialized = false;

        public const float G = 6.67f;
        public const float PhysicsScale = 0.3f;

        public void Awake()
        {
            _physicsBodies = FindObjectsOfType<PhysicsBody>().ToList();

            _positions = new List<Vector2>(_physicsBodies.Count);
            _velocities = new List<Vector2>(_physicsBodies.Count);
            _forces = new List<List<Vector2>>(_physicsBodies.Count);
            _enabled = new List<bool>(_physicsBodies.Count);

            for (int i = 0; i < _physicsBodies.Count; i++)
            {
                _physicsBodies[i].bodyIndex = i;
                _physicsBodies[i].physicsSystem = this;

                _positions.Add(_physicsBodies[i].transform.position);
                _velocities.Add(_physicsBodies[i].initialVelocity);
                _forces.Add(new List<Vector2>());
                _enabled.Add(defaultEnabled);
            }
        }

        public void AddBody(PhysicsBody body)
        {
            var i = _physicsBodies.Count - 1;
            
            _physicsBodies.Add(body);
            
            body.bodyIndex = i;
            body.physicsSystem = this;

            _positions.Add(body.transform.position);
            _velocities.Add(body.initialVelocity);
            _forces.Add(new List<Vector2>());
            _enabled.Add(defaultEnabled);
        }

        // This gets called in a frame-independent update loop
        public void FixedUpdate()
        {
            if (!_cacheInitialized)
            {
                RefreshClosestAttractors();
            }
            
            Profiler.BeginSample("Physics loop");
            Leapfrog();
            ResetForces();
            Profiler.EndSample();
        }

        // This just updates the display
        public void Update()
        {
            UpdateDisplay();
        }
        
        public void RefreshClosestAttractors()
        {
            _cacheInitialized = true;
            for (int i = 0; i < _physicsBodies.Count; i++)
            {
                var closest = GetClosestAttractor(_positions[i]);
                _physicsBodies[i].cachedClosest = closest;
            }
        }

        // Calculating velocities and positions with leapfrog integration. Ribbit.
        private void Leapfrog()
        {
            var closestToShip = GetClosestAttractor(_positions[playerSpaceship.bodyIndex]);
            
            for (int i = 0; i < _physicsBodies.Count; i++)
            {
                if (!_enabled[i])
                    continue;

                var body = _physicsBodies[i];
                
                if (!body.bodyRenderer.isVisible && 
                    body.cachedClosest != null && 
                    body.cachedClosest != closestToShip && 
                    body != playerSpaceship)
                    continue;

                var closest = GetClosestAttractor(_positions[i]);
                
                var accel = GetNetAcceleration(i, closest);
                _positions[i] = GetPosition(_positions[i], _velocities[i], accel);
                var nextAccel = GetNetAcceleration(i, closest);
                _velocities[i] = GetVelocity(_velocities[i], accel, nextAccel);
            }
        }

        private Vector2 GetPosition(Vector2 position, Vector2 velocity, Vector2 accel)
        {
            return position + velocity * Time.fixedDeltaTime + accel * (.5f * Mathf.Pow(Time.fixedDeltaTime, 2));
        }

        private Vector2 GetVelocity(Vector2 velocity, Vector2 accel, Vector2 nextAccel)
        {
            return velocity + .5f * (accel + nextAccel) * Time.fixedDeltaTime;
        }

        // Gets trajectory of a body based on Kepler solution of 2-body problem
        public List<Vector2> GetTrajectory(int index, Vector2 center, float mass)
        {
            var positions = new List<Vector2>();

            var pos = _positions[index];
            var v = _velocities[index];

            pos -= center;
            
            var r = pos;
            var M = mass;

            var mu = G * M * PhysicsScale;

            var rMag = r.magnitude;
            var vSqMag = v.sqrMagnitude;
            
            var vt = (r.x * v.y - r.y * v.x) / rMag;
            var vr = (r.x * v.x + r.y * v.y) / rMag;
            
            var a = mu * rMag / (2 * mu - rMag * vSqMag);
            
            var e = Mathf.Sqrt(1 + rMag * vt * vt / mu * (rMag * vSqMag / mu - 2));

            var theta = Mathf.Sign(vt * vr) * Mathf.Acos((a * (1 - e * e) - rMag) / (e * rMag)) - Mathf.Atan2(r.y, r.x);

            Func<float, float> R = x => a * (1 - e * e) / (1 + e * Mathf.Cos(x));

            if (e < 1)
            {
                for (float i = -Mathf.PI; i <= Mathf.PI; i += 0.04f)
                {
                    var x = R(i) * Mathf.Cos(i + theta);
                    var y = -R(i) * Mathf.Sin(i + theta);
                
                    positions.Add(new Vector2(center.x + x, center.y + y));
                }
                positions.Add(positions[0]);
            }
            else
            {
                var thetaMax = Math.Acos((a * (1 - e * e) - 200f) / (e * 200f));
                for (double i = -thetaMax; i < thetaMax; i += 0.04f)
                {
                    var x = R((float)i) * Mathf.Cos((float)i + theta);
                    var y = -R((float)i) * Mathf.Sin((float)i + theta);
                
                    positions.Add(new Vector2(center.x + x, center.y + y));
                }
            }
            
            return positions;
        }
        

        private void UpdateDisplay()
        {
            for (int i = 0; i < _physicsBodies.Count; i++)
            {
                if (!_enabled[i])
                    continue;
                var position = _physicsBodies[i].transform.position;
                position.x = _positions[i].x;
                position.y = _positions[i].y;
                _physicsBodies[i].transform.position = position;
            }
        }

        public Attractor GetClosestAttractor(Vector2 point)
        {
            if (attractors.Count <= 0)
            {
                return null;
            }
            
            Attractor closest = attractors[0];
            var pos = (Vector2)closest.transform.position;
            var minDist = (point - pos).magnitude;
            foreach (var attractor in attractors)
            {
                pos = attractor.transform.position;
                var dist = (point - pos).magnitude;
                if (dist >= minDist) continue;
                closest = attractor;
                minDist = dist;
            }
            return closest;
        }

        public Vector2 Gravity(Vector2 point)
        {
            var closest = GetClosestAttractor(point);
            return Gravity(point, closest);
        }

        public Vector2 Gravity(Vector2 point, Attractor closest)
        {
            var temp = closest.transform.position;
            var center = new Vector2(temp.x, temp.y);
            var accel = (center - point).normalized;
            var distance = (point - center).magnitude;
            var magnitude = G * closest.mass * PhysicsScale / Mathf.Pow(distance, 2);
            accel.Scale(new Vector2(magnitude, magnitude));

            return accel;
        }

        private Vector2 GetNetAcceleration(int index, Attractor closest)
        {
            if (index < _forces.Count && index >= 0)
            {
                var point = _positions[index];
                var net = Gravity(point, closest);
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

        public void SetPosition(int index, Vector2 v)
        {
            _positions[index] = v;
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