using System;
using System.Collections;
using System.Collections.Generic;
using Physics;
using UnityEngine;
using Visuals;

namespace Systems.Spaceship
{
    [RequireComponent(typeof(PhysicsBody))]
    public class StarSceneSpaceshipController : MonoBehaviour
    {
        public List<Vector2> Trajectory => _physicsBody.PredictedTrajectory();
        public Vector2 Velocity => _physicsBody.GetVelocity();
        public float C => (float)Math.Exp(Velocity.magnitude / 40f) - 1f;
        public bool Maneuvering => _maneuvering;
        public bool Refueling => _refueling;
        public bool Orbiting => _orbiting != null;
        public float FuelPercent => _fuel / maxFuel;
        
        public float thrust;
        public float dashRange;
        public float maxFuel;
        public float consumptionRate;
        public float refuelRange;
        public MapUtils map;
        [Range(0.05f, 1)] public float dashSpeed;

        private PhysicsBody _physicsBody;
        private (Vector2, Vector2) _dashData;
        private float _fuel;
        private bool _maneuvering;
        private bool _refueling;
        private PhysicsBody _orbiting;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
            _orbiting = null;
            _physicsBody = gameObject.GetComponent<PhysicsBody>();
            _fuel = maxFuel;
        }

        public void Refuel(float value)
        {
            _refueling = true;
            _fuel += value;
            _fuel = Mathf.Clamp(_fuel, 0, maxFuel);
        }

        public void Update()
        {
            HandleRefuel();
            HandleMovement();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }
        }

        private bool TryEnterOrbit(PhysicsBody body)
        {
            var shipVelocity = _physicsBody.GetVelocity();
            var bodyVelocity = body.GetVelocity();
            var relativeVelocity = bodyVelocity - shipVelocity;
            var dist = (transform.position - body.transform.position).magnitude;
            if (dist < body.interactRadius && relativeVelocity.magnitude < body.EscapeVelocity)
            {
                _orbiting = body;
                _physicsBody.PhysicsEnabled = false;
            }

            return false;
        }

        private bool TryLeaveOrbit()
        {
            if (_orbiting == null)
            {
                var velocity = _orbiting.GetVelocity() ;
                _physicsBody.PhysicsEnabled = true;
            }

            return false;
        }

        private void HandleRefuel()
        {
            _refueling = false;
            var closest = map.ClosestPlanet;
            if (closest.planet != null && closest.planet.inhabited && closest.dist < refuelRange)
            {
                Refuel(Time.deltaTime * 10f);
            }
        }
        
        private void HandleMovement()
        {
            if (!_orbiting)
            {
                var vert = Input.GetAxis("Vertical");
                var hor = Input.GetAxis("Horizontal");
            
                if ((vert != 0 || hor != 0) && _fuel > 0)
                {
                    _maneuvering = true;
                    _fuel -= consumptionRate * Time.deltaTime;
                    AddThrust(vert, hor);
                }
                else
                {
                    _maneuvering = false;
                }
                Turn();
            }
        }

        private void Turn()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = _camera.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void AddThrust(float vert, float hor)
        {
            // Makes sense, right?
            var forward = transform.right;
            var right = -transform.up;
            var thrustVector = new Vector2(
                forward.x * thrust * vert + right.x * thrust * hor,
                forward.y * thrust * vert + right.y * thrust * hor);

            _physicsBody.AddForce(thrustVector);
        }

        private IEnumerator Dash()
        {
            // Don't dash when dashing already
            if (_physicsBody.PhysicsEnabled)
            {
                _dashData = (transform.position, transform.position + transform.right * dashRange);
                _physicsBody.PhysicsEnabled = false;

                var dashDirection = (_dashData.Item2 - _dashData.Item1).normalized;

                var t = 0f;

                while (t < 1)
                {
                    t += dashSpeed * Time.deltaTime * 30f;
                    transform.position = Vector2.Lerp(_dashData.Item1, _dashData.Item2, t);
                    yield return null;
                }

                var speed = _physicsBody.GetVelocity().magnitude;
                _physicsBody.VelocityOverride(speed * dashDirection);
                _physicsBody.PhysicsEnabled = true;
            }
        }
    }
}
