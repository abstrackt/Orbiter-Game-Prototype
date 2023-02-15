using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class LoadingIcon : MonoBehaviour
    {
        public Image dot;
        private float _t = 0;
        private float _r = 0;

        public void Start()
        {
            _r = dot.transform.localPosition.x;
        }
        
        public void Update()
        {
            dot.transform.localPosition = new Vector2(_r * Mathf.Cos(3 * _t), _r * Mathf.Sin(3 * _t));
            _t += Time.deltaTime;
        }
    }
}