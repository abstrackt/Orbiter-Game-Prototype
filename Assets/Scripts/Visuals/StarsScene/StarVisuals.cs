using Systems.Physics;
using UnityEngine;

namespace Visuals.StarsScene
{
    [RequireComponent(typeof(Attractor))]
    public class StarVisuals : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public UnityEngine.Rendering.Universal.Light2D starLight;
        public Attractor attractor;
    }
}