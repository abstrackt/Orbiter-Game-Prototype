using System.Collections.Generic;
using Systems.Global;
using Systems.Physics;
using UnityEngine;

namespace Systems.PlanetScene
{
    [RequireComponent(typeof(PhysicsBody))]
    public class PlanetSpaceshipController : SingletonMonoBehaviour<PlanetSpaceshipController>
    {
        public List<Vector2> Trajectory => _physicsBody.PredictedTrajectory();

        private PhysicsBody _physicsBody;
        private SpaceshipManager _spaceship;
        private LocationManager _location;
        private (Vector2, Vector2) _dashData;
        private bool _maneuvering;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
            _physicsBody = gameObject.GetComponent<PhysicsBody>();
            _spaceship = SpaceshipManager.Instance;
            _location = LocationManager.Instance;

            var dir = Random.insideUnitCircle.normalized;
            var perp = new Vector2(dir.y, -dir.x);
            
            _physicsBody.PositionOverride(dir * 80f);
            _physicsBody.VelocityOverride(-dir * 2f + perp * Random.Range(1f, 1.5f));
        }

        public void Update()
        {
            HandleMovement();
            HandleEscape();
            HandleShipView();
        }

        private void HandleEscape()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _location.LeaveOrbit();
            }
        }
        
        private void HandleShipView()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _location.EnterShipView();
            }
        }

        private void HandleMovement()
        {
            var vert = Input.GetAxis("Vertical");
            var hor = Input.GetAxis("Horizontal");
        
            if ((vert != 0 || hor != 0))
            {
                _maneuvering = true;
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
            var thrust = 0.1f;
            var thrustVector = new Vector2(
                forward.x * thrust * vert + right.x * thrust * hor,
                forward.y * thrust * vert + right.y * thrust * hor);

            _physicsBody.AddForce(thrustVector);
        }
    }
}
