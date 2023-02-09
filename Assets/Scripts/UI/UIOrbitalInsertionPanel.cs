using Systems.Global;
using Systems.StarsScene;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIOrbitalInsertionPanel : UIPanel
    {
        public Text insertionText;

        private StarsSpaceshipController _controller;

        public override void Initialize(GameEventSystem events)
        {
            
        }

        public override void Deinitialize(GameEventSystem events)
        {

        }

        public void Start()
        {
            _controller = StarsSpaceshipController.Instance;
        }

        public void Update()
        {
            insertionText.gameObject.SetActive(_controller.CanOrbit);
        }
    }
}