using System;
using Systems.Physics;
using UnityEngine;

namespace Systems.StarsScene
{
    [RequireComponent(typeof(Camera))]
    public class StarsCameraManager : MonoBehaviour
    {
        public float minZoom, maxZoom;
        public float lerpSpeed;
        public float slerpThreshold;
        public float followDist = 100f;
        [Range(0, 1)] public float tolerance;
        
        private Camera _camera;
        private float _targetZoom;
        private Vector3 _targetPos;
        private Transform _anchored;
        private PhysicsSystem _physics;
        private StarsMapManager _map;

        public void Start()
        {
            _map = StarsMapManager.Instance;
            _anchored = StarsSpaceshipController.Instance.transform;
            _camera.transform.position = _anchored.position + new Vector3(0, 0, -10);
        }

        public void Update()
        {
            CalculateZoom();
            AdjustCamera();
        }

        // This assumes camera is aligned with the axes of the scene
        private void CalculateZoom()
        {
            var followed = (Vector2)_map.ClosestStarVisuals.star.transform.position;
            var anchored = (Vector2)_anchored.position;
            
            Vector2 fromCamera = followed - anchored;
            _targetZoom = fromCamera.magnitude * (1 + tolerance);
        }

        private void AdjustCamera()
        {
            var followed = (Vector2)_map.ClosestStarVisuals.star.transform.position;
            var anchored = (Vector2)_anchored.position;

            var dist = Math.Min((followed - anchored).magnitude, followDist);
            _targetPos = (anchored * (followDist - dist) + followed * dist) / followDist;
            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);

            var step = lerpSpeed;
            var distance = Mathf.Max(Mathf.Abs(_targetZoom - _camera.orthographicSize), .001f);

            if (distance < slerpThreshold)
            {
                step *= distance / slerpThreshold;
            }

            if (_targetZoom < _camera.orthographicSize)
                _camera.orthographicSize = Mathf.Max(_camera.orthographicSize - step, _targetZoom);
            else
                _camera.orthographicSize = Mathf.Min(_camera.orthographicSize + step, _targetZoom);

            var cameraPos = _camera.transform.position;
            var dir = new Vector2(_targetPos.x - cameraPos.x, _targetPos.y - cameraPos.y);

            if (dir.magnitude > step)
            {
                _camera.transform.position += new Vector3(dir.x, dir.y, 0) * 0.01f;
            }
        }
    }
}