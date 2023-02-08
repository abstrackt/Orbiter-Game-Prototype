using Systems.Global;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISystemDiscoveredPanel : MonoBehaviour
    {
        public Animator animator;
        public Text systemName;
        public CanvasGroup canvasGroup;

        public void Start()
        {
            canvasGroup.alpha = 0;
            animator.enabled = false;
            var events = GameEventSystem.Instance;
            events.OnSystemDiscovered += Show;
        }

        public void Show(string name)
        {
            canvasGroup.alpha = 1;
            animator.enabled = true;
            animator.Play("LocationAnimation", 0, 0);
            systemName.text = name;
        }

        public void OnDestroy()
        {
            var events = GameEventSystem.Instance;
            events.OnSystemDiscovered -= Show;
        }
    }
}