using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Visuals;

namespace UI
{
    public class UIShipIndicatorPanel : MonoBehaviour
    {
        public Camera camera;
        public SpaceshipController controller;
        public MapDefinition map;
        public Image refuelIcon;
        public CanvasGroup canvasGroup;
        public Transform mousePivot;
        public Transform velocityPivot;
        public float visibilityThreshold = 40f;

        private float _fade = 0f;
        private bool _fading = false;

        public void Update()
        {
            var mouseTr = mousePivot.transform;
            var shipPos = controller.transform.position;
            Vector2 mousePos = Input.mousePosition;
            var screenPos = camera.WorldToScreenPoint(shipPos);
            Vector2 mouseDir = mousePos - (Vector2)screenPos;
            var mouseAngle = Vector2.SignedAngle(Vector2.up, mouseDir);
            var mouseRot = mouseTr.eulerAngles;
            mouseRot.z = mouseAngle;
            mouseTr.eulerAngles = mouseRot;

            var velocityTr = velocityPivot.transform;
            var velocityAngle = Vector2.SignedAngle(Vector2.up, controller.Velocity);
            var velocityRot = velocityTr.eulerAngles;
            velocityRot.z = velocityAngle;
            velocityTr.eulerAngles = velocityRot;

            if (controller.Refueling)
            {
                canvasGroup.alpha = Math.Min((controller.refuelRange - map.ClosestPlanet.dist) / controller.refuelRange * 10f, 1);
            }
            else if (camera.orthographicSize < visibilityThreshold)
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                canvasGroup.alpha = Math.Min((camera.orthographicSize - visibilityThreshold) / visibilityThreshold, 1);
            }

            refuelIcon.enabled = controller.Refueling;

            if (controller.Refueling)
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