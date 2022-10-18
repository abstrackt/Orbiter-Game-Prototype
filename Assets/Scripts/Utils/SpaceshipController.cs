using System.Collections;
using Physics;
using UnityEngine;

namespace Utils
{
    public class SpaceshipController : MonoBehaviour
    {
        public float turnSpeed;
        public float thrust;
        public float dashRange;
        [Range(0.05f, 1)] public float dashSpeed;

        private PhysicsBody _physicsBody;
        private (Vector2, Vector2) _dashData;

        public void Start()
        {
            _physicsBody = gameObject.GetComponent<PhysicsBody>();
        }

        public void Update()
        {
            var vert = Input.GetAxis("Vertical");
            var hor = Input.GetAxis("Horizontal");

            if (vert != 0 || hor != 0)
            {
                AddThrust(vert, hor);
            }

            Turn();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }

            var attractors = _physicsBody.physicsSystem;
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
