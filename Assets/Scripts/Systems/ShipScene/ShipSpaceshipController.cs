using System.Collections.Generic;
using Systems.Global;
using Systems.Physics;
using UnityEngine;

namespace Systems.ShipScene
{
    public class ShipSpaceshipController : SingletonMonoBehaviour<ShipSpaceshipController>
    {
        public float rotationSpeed;
        
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
        }

        public void Update()
        {
            HandleMovement();
            HandleEscape();
        }

        private void HandleEscape()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _location.LeaveShipView();
            }
        }

        private void HandleMovement()
        {
            var vert = Input.GetAxis("Vertical");
            var hor = 0f;
            var turn = Input.GetAxis("Horizontal");

            // Rewrite for more fine control later
            if (Input.GetKey(KeyCode.Q))
            {
                hor = 1f;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                hor = -1f;
            }

            if (vert != 0 || hor != 0 || turn != 0)
            {
                _maneuvering = true;
                Turn(turn);
                AddThrust(vert, hor);
            }
            else
            {
                _maneuvering = false;
            }
        }

        private void Turn(float turn)
        {
            transform.Rotate(Vector3.back, turn * rotationSpeed);
        }

        private void AddThrust(float vert, float hor)
        {
            // Makes sense, right?
            var forward = -transform.up;
            var right = transform.right;
            var thrust = 20f;
            var thrustVector = new Vector2(
                forward.x * thrust * vert + right.x * thrust * hor,
                forward.y * thrust * vert + right.y * thrust * hor);

            _physicsBody.AddForce(thrustVector);
        }
    }
}