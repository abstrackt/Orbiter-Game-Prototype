using Physics;
using Systems.Spaceship;
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
        public StarSceneSpaceshipController controller;
        public PhysicsSystem physicsSystem;

        public void Update()
        {
            var width = fuelWidth * controller.FuelPercent;
            fuelImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            velocityText.text = $"{controller.C:0.00}c";
            var shipPos = controller.transform.position;
            gravityText.text = $"{physicsSystem.Gravity(shipPos).magnitude / 9.81f:0.00}g";
        }
    }
}