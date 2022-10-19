using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Utils
{
    public class LightspeedEffect : MonoBehaviour
    {
        public Camera camera;
        public SpaceshipController controller;
        public VolumeProfile profile;
        public float threshold;

        public void Update()
        {
            var c = controller.C;

            var aberration = (ChromaticAberration)profile.components[0];
            var lensDistortion = (LensDistortion)profile.components[1];
            if (c >= threshold)
            {
                aberration.intensity.Override(Mathf.Clamp01((c - threshold)));
                var viewportPos = camera.WorldToViewportPoint(controller.transform.position);
                lensDistortion.center.Override(viewportPos);
                lensDistortion.intensity.Override(-Mathf.Clamp01((c - threshold) * 1000f) / camera.orthographicSize);
            }
            else
            {
                aberration.intensity.Override(0);
                lensDistortion.intensity.Override(0);
            }
        }
    }
}