using Systems.ShipScene;
using UnityEngine;

namespace Visuals.ShipScene
{
    public class GridVisuals : MonoBehaviour
    {
        public MeshRenderer grid;
        public Material gridMaterial;

        private ShipSpaceshipController _spaceship;

        public void Start()
        {
            _spaceship = ShipSpaceshipController.Instance;

            grid.material = Instantiate(gridMaterial);
        }

        public void Update()
        {
            var position = _spaceship.transform.position;
            grid.material.SetVector("_TargetPos", position);
            grid.transform.position = position + Vector3.down * 0.5f;
        }
    }
}