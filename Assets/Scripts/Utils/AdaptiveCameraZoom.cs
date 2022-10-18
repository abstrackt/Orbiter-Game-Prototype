using System;
using UnityEngine;

namespace Utils
{
    public class AdaptiveCameraZoom : MonoBehaviour
    {
        public FocusController focus;
        public float minZoom, maxZoom;
        public float lerpSpeed;
        public float slerpThreshold;
        public float followDist = 100f;
        [Range(0, 1)] public float tolerance;
        
        private Camera _camera;
        private float _targetZoom;
        private Vector3 _targetPos;

        public void Start()
        {
            if (!TryGetComponent(out _camera))
            {
                throw new MissingComponentException(
                    "Adaptive camera zoom needs a camera " +
                    "to be attached in order to work.");
            }

            _camera.transform.position = focus.Anchored + new Vector3(0, 0, -10);
        }

        public void Update()
        {
            CalculateZoom();
            AdjustCamera();
        }

        // This assumes camera is aligned with the axes of the scene
        private void CalculateZoom()
        {
            Vector2 fromCamera = focus.Followed - focus.Anchored;
            _targetZoom = fromCamera.magnitude * (1 + tolerance);
        }

        private void AdjustCamera()
        {
            var dist = Math.Min((focus.Followed - focus.Anchored).magnitude, followDist);
            _targetPos = (focus.Anchored * (followDist - dist) + focus.Followed * dist) / followDist;
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