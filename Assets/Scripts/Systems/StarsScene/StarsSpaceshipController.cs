using System;
using System.Collections.Generic;
using Systems.Global;
using Systems.Physics;
using UnityEngine;

namespace Systems.StarsScene
{
    [RequireComponent(typeof(PhysicsBody))]
    public class StarsSpaceshipController : SingletonMonoBehaviour<StarsSpaceshipController>
    {
        public List<Vector2> Trajectory => _physicsBody.PredictedTrajectory();
        public Vector2 Velocity => _physicsBody.GetVelocity();
        public float C => (float)Math.Exp(Velocity.magnitude / 40f) - 1f;
        public bool Maneuvering => _maneuvering;
        public bool Refueling => _refueling;
        public bool CanOrbit => _canOrbit;
        public float FuelPercent => _fuel / _spaceship.Stats.maxFuel;

        private SpaceshipManager _spaceship;
        private LocationManager _location;
        private StarsMapManager _map;
        private StarsCameraManager _cameraManager;
        private PhysicsBody _physicsBody;
        private (Vector2, Vector2) _dashData;
        private float _fuel;
        private bool _maneuvering;
        private bool _refueling;
        private bool _canOrbit;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
            _cameraManager = StarsCameraManager.Instance;
            _physicsBody = gameObject.GetComponent<PhysicsBody>();
            _spaceship = SpaceshipManager.Instance;
            _location = LocationManager.Instance;
            _map = StarsMapManager.Instance;
            _fuel = _spaceship.Stats.maxFuel;

            _physicsBody.PositionOverride(_spaceship.SavedPos);
            _physicsBody.VelocityOverride(_spaceship.SavedVel);
            _cameraManager.JumpToPoint(_spaceship.SavedPos);
        }

        public void Refuel(float value)
        {
            _refueling = true;
            _fuel += value;
            _fuel = Mathf.Clamp(_fuel, 0, _spaceship.Stats.maxFuel);
        }

        public void Update()
        {
            HandleOrbit();
            HandleRefuel();
            HandleMovement();
        }

        private void HandleOrbit()
        {
            if (_map.ClosestPlanetPhysics.planet)
            {
                var body = _map.ClosestPlanetPhysics.planet;
                var dist = _map.ClosestPlanetPhysics.dist;
                if (dist < body.interactRadius)
                {
                    _canOrbit = true;

                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        _map.SaveWorldState();
                        _spaceship.SaveState(_physicsBody);
                        var planetBody = _map.ClosestPlanetPhysics.planet;
                        var planet = _map.ClosestPlanetData.planet;
                        var data = _map.GetClosestStarData(planetBody);
                        _location.EnterOrbit(planet, data.star, data.dist, data.phase);
                    }

                    return;
                }
            }

            _canOrbit = false;
        }

        private void HandleRefuel()
        {
            _refueling = false;
            var closest = _map.ClosestPlanetData;
            if (closest.planet.Inhabited && closest.dist < _spaceship.Stats.refuelRange)
            {
                Refuel(Time.deltaTime * 10f);
            }
        }
        
        private void HandleMovement()
        {
            var vert = Input.GetAxis("Vertical");
            var hor = Input.GetAxis("Horizontal");
        
            if ((vert != 0 || hor != 0) && _fuel > 0)
            {
                _maneuvering = true;
                _fuel -= _spaceship.Stats.consumptionRate * Time.deltaTime;
                AddThrust(vert, hor);
            }
            else
            {
                _maneuvering = false;
            }
            Turn();
        }

        private void Turn()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = _camera.WorldToScreenPoint(transform.position);
            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void AddThrust(float vert, float hor)
        {
            // Makes sense, right?
            var forward = transform.right;
            var right = -transform.up;
            var thrust = _spaceship.Stats.thrust;
            var thrustVector = new Vector2(
                forward.x * thrust * vert + right.x * thrust * hor,
                forward.y * thrust * vert + right.y * thrust * hor);

            _physicsBody.AddForce(thrustVector);
        }
    }
}
