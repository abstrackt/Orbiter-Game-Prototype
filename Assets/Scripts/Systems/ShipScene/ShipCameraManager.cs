using Systems.ShipScene;
using UnityEngine;

namespace Systems.ShipScene
{
    public class ShipCameraManager : MonoBehaviour
    {
        public Transform cameraPivot;
        public float minZ = 30;
        public float maxZ = 100;

        private Vector3 _lastPos;
        private ShipSpaceshipController _spaceship;
        private Camera _camera;

        public void Start()
        {
            _lastPos = Input.mousePosition;
            _spaceship = ShipSpaceshipController.Instance;
            _camera = Camera.main;
        }

        public void Update()
        {
            var currentPos = Input.mousePosition;

            var delta = currentPos - _lastPos;
            var scroll = Input.mouseScrollDelta;

            _lastPos = currentPos;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                var euler = cameraPivot.localEulerAngles;
                euler.x += delta.y;
                euler.y += -delta.x;
                cameraPivot.localEulerAngles = euler;
            }

            var zoom = Mathf.Abs(_camera.transform.localPosition.z);

            zoom += scroll.y;

            zoom = Mathf.Clamp(zoom, minZ, maxZ);

            _camera.transform.localPosition = Vector3.back * zoom;

            var cameraPos = cameraPivot.transform.position;
            var targetPos = _spaceship.transform.position;
            var dir = new Vector2(targetPos.x - cameraPos.x, targetPos.y - cameraPos.y);

            if (dir.magnitude > 0.01f)
            {
                cameraPivot.transform.position += new Vector3(dir.x, dir.y, 0) * 0.01f;
            }
        }
    }
}