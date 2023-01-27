using Physics;
using UnityEngine;


namespace Visuals
{
    [RequireComponent(typeof(Attractor))]
    public class StarVisuals : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public UnityEngine.Rendering.Universal.Light2D starLight;
        public string starName;
        public Attractor attractor;
    }
}