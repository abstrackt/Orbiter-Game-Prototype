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