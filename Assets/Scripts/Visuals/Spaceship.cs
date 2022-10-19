using UnityEngine;
using Utils;

namespace Visuals
{
    public class Spaceship : MonoBehaviour
    {
        public SpaceshipController controller;
        public ParticleSystem particles;

        public void Update()
        {
            if (controller.Maneuvering && !particles.isPlaying)
            {
                particles.Play();
            }
            else if (!controller.Maneuvering && particles.isPlaying)
            {
                particles.Pause();
            }
        }
    }
}