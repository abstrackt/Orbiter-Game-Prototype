using Systems.Global;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UISystemDiscoveredPanel : UIPanel
    {
        public Animator animator;
        public Text systemName;
        public CanvasGroup canvasGroup;

        public override void Initialize(GameEventSystem events)
        {
            events.OnSystemDiscovered += Show;
        }
        
        public override void Deinitialize(GameEventSystem events)
        {
            events.OnSystemDiscovered -= Show;
        }
        
        public void Start()
        {
            canvasGroup.alpha = 0;
            animator.enabled = false;
        }

        public void Show(string name)
        {
            canvasGroup.alpha = 1;
            animator.enabled = true;
            animator.Play("LocationAnimation", 0, 0);
            systemName.text = name;
        }
    }
}