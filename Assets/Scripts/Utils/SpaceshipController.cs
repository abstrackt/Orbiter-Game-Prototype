using System;
using System.Collections;
using Physics;
using UnityEngine;
using Visuals;

namespace Utils
{
    public class SpaceshipController : MonoBehaviour
    {
        public Vector2 Velocity => _physicsBody.GetVelocity();
        public float C => (float)Math.Exp(Velocity.magnitude / 40f) - 1f;
        public bool Maneuvering => _maneuvering;
        public bool Refueling => _refueling;
        public float FuelPercent => _fuel / maxFuel;
        
        public float thrust;
        public float dashRange;
        public float maxFuel;
        public float consumptionRate;
        public float refuelRange;
        public MapDefinition map;
        [Range(0.05f, 1)] public float dashSpeed;

        private PhysicsBody _physicsBody;
        private (Vector2, Vector2) _dashData;
        private float _fuel;
        private bool _maneuvering;
        private bool _refueling;

        public void Start()
        {
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
            _refueling = false;
            
            var vert = Input.GetAxis("Vertical");
            var hor = Input.GetAxis("Horizontal");

            var closest = map.ClosestPlanet;
            if (closest.planet != null && closest.planet.inhabited && closest.dist < refuelRange)
            {
                Refuel(Time.deltaTime * 10f);
            }
            
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }
        }

        private void Turn()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
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
