using Systems.Global;
using Systems.Physics;
using Systems.StarsScene;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIShipStatusPanel : UIPanel
    {
        public Image fuelImage;
        public Text velocityText;
        public Text gravityText;

        private StarsSpaceshipController _controller;
        private PhysicsSystem _physics;
        private float _fuelSize;

        public override void Initialize(GameEventSystem events)
        {
            
        }

        public override void Deinitialize(GameEventSystem events)
        {
            
        }
        
        public void Start()
        {
            _controller = StarsSpaceshipController.Instance;
            _physics = PhysicsSystem.Instance;
            _fuelSize = fuelImage.rectTransform.rect.height;
        }
        
        public void Update()
        {
            var size = _fuelSize * _controller.FuelPercent;
            fuelImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            velocityText.text = $"{_controller.C:0.00}c";
            var shipPos = _controller.transform.position;
            gravityText.text = $"{_physics.Gravity(shipPos).magnitude / 9.81f:0.00}g";
        }
    }
}