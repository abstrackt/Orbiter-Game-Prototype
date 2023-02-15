using System;
using Systems.Global;
using Systems.StarsScene;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIShipIndicatorPanel : UIPanel
    {
        public Image refuelIcon;
        public CanvasGroup canvasGroup;
        public Transform mousePivot;
        public Transform velocityPivot;
        public float visibilityThreshold = 40f;

        private float _fade = 0f;
        private bool _fading = false;
        private Camera _camera;
        private StarsSpaceshipController _controller;
        private StarsMapManager _map;
        private SpaceshipManager _spaceship;

        public override void Initialize(GameEventSystem events)
        {
            
        }

        public override void Deinitialize(GameEventSystem events)
        {
            
        }
        
        public void Start()
        {
            _camera = Camera.main;
            _map = StarsMapManager.Instance;
            _controller = StarsSpaceshipController.Instance;
            _spaceship = SpaceshipManager.Instance;
        }
        
        public void Update()
        {
            var mouseTr = mousePivot.transform;
            var shipPos = _controller.transform.position;
            Vector2 mousePos = Input.mousePosition;
            var screenPos = _camera.WorldToScreenPoint(shipPos);
            Vector2 mouseDir = mousePos - (Vector2)screenPos;
            var mouseAngle = Vector2.SignedAngle(Vector2.up, mouseDir);
            var mouseRot = mouseTr.eulerAngles;
            mouseRot.z = mouseAngle;
            mouseTr.eulerAngles = mouseRot;

            var velocityTr = velocityPivot.transform;
            var velocityAngle = Vector2.SignedAngle(Vector2.up, _controller.Velocity);
            var velocityRot = velocityTr.eulerAngles;
            velocityRot.z = velocityAngle;
            velocityTr.eulerAngles = velocityRot;

            if (_controller.Refueling)
            {
                var range = _spaceship.Stats.refuelRange;
                canvasGroup.alpha = Math.Min((range - _map.ClosestPlanetVisuals.dist) / range * 10f, 1);
            }
            else if (_camera.orthographicSize < visibilityThreshold)
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                canvasGroup.alpha = Math.Min((_camera.orthographicSize - visibilityThreshold) / visibilityThreshold, 1);
            }

            refuelIcon.enabled = _controller.Refueling;

            if (_controller.Refueling)
            {
                _fade += _fading ? -Time.deltaTime : Time.deltaTime;
                
                if (_fading && _fade < 0)
                {
                    _fade = 0;
                    _fading = false;
                }

                if (!_fading && _fade > 1)
                {
                    _fade = 1;
                    _fading = true;
                }

                var c = refuelIcon.color;
                c.a = _fade;
                refuelIcon.color = c;
            }

            transform.position = screenPos;
        }
    }
}