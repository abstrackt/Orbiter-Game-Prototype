using Systems.Spaceship;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Visuals
{
    public class LightspeedEffect : MonoBehaviour
    {
        public SpaceshipController controller;
        public VolumeProfile profile;
        public float threshold;
        
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }
        
        public void Update()
        {
            var c = controller.C;

            var aberration = (ChromaticAberration)profile.components[0];
            var lensDistortion = (LensDistortion)profile.components[1];
            if (c >= threshold)
            {
                aberration.intensity.Override(Mathf.Clamp01((c - threshold)));
                var viewportPos = _camera.WorldToViewportPoint(controller.transform.position);
                lensDistortion.center.Override(viewportPos);
                lensDistortion.intensity.Override(-Mathf.Clamp01((c - threshold) * 1000f) / _camera.orthographicSize);
            }
            else
            {
                aberration.intensity.Override(0);
                lensDistortion.intensity.Override(0);
            }
        }
    }
}