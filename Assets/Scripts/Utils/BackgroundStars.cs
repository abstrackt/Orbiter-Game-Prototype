using UnityEngine;

namespace Utils
{
    public class BackgroundStars : MonoBehaviour
    {
        public ParticleSystem particles;
        public float showThreshold;

        public void Update()
        {
            var mainCamera = Camera.main;
            if (mainCamera != null)
            {
                var size = mainCamera.orthographicSize * 2;
                var shape = particles.shape;
                shape.scale = new Vector3(1.2f * size * mainCamera.aspect, 1.2f * size , 1);
                particles.transform.position = mainCamera.transform.position;
                if (mainCamera.orthographicSize > showThreshold)
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