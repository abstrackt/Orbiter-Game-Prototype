using Systems.Physics;
using Systems.StarsScene;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIShipStatusPanel : MonoBehaviour
    {
        public float fuelWidth = 1870;
        public Image fuelImage;
        public Text velocityText;
        public Text gravityText;

        private StarsSpaceshipController _controller;
        private PhysicsSystem _physics;

        public void Start()
        {
            _controller = StarsSpaceshipController.Instance;
            _physics = PhysicsSystem.Instance;
        }
        
        public void Update()
        {
            var width = fuelWidth * _controller.FuelPercent;
            fuelImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            velocityText.text = $"{_controller.C:0.00}c";
            var shipPos = _controller.transform.position;
            gravityText.text = $"{_physics.Gravity(shipPos).magnitude / 9.81f:0.00}g";
        }
    }
}