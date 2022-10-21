using Physics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Visuals
{
    [RequireComponent(typeof(Attractor))]
    public class Star : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public Light2D starLight;
        public string starName;
        public Attractor attractor;
    }
}