using Systems.ShipScene;
using UnityEngine;

namespace Utils
{
    public class ShipCameraManager : MonoBehaviour
    {
        public Transform cameraPivot;
        public Transform shipPivot;

        private Vector3 _lastPos;
        private ShipSpaceshipController _spaceship;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
            _lastPos = Input.mousePosition;
            _spaceship = ShipSpaceshipController.Instance;
        }

        public void Update()
        {
            var currentPos = Input.mousePosition;

            var delta = currentPos - _lastPos;

            _lastPos = currentPos;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                cameraPivot.Rotate(delta.x, delta.y, 0);
            }
            
            var cameraPos = _camera.transform.position;
            var targetPos = _spaceship.transform.position;
            var dir = new Vector2(targetPos.x - cameraPos.x, targetPos.y - cameraPos.y);

            if (dir.magnitude > 0.01f)
            {
                cameraPivot.transform.position += new Vector3(dir.x, dir.y, 0) * 0.01f;
            }
        }
    }
}