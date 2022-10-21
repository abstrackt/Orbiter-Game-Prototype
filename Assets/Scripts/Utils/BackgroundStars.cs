using UnityEngine;

namespace Utils
{
    public class BackgroundStars : MonoBehaviour
    {
        public ParticleSystem particles;
        public float showThreshold;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            if (_camera != null)
            {
                var size = _camera.orthographicSize * 2;
                var shape = particles.shape;
                shape.scale = new Vector3(1.2f * size * _camera.aspect, 1.2f * size , 1);
                particles.transform.position = _camera.transform.position;
                if (_camera.orthographicSize > showThreshold)
                {
                    if (!particles.isPlaying) particles.Play();
                }
                else
                {
                    if (particles.isPlaying) particles.Stop();;
                }
            }
        }
    }
}