using Systems.Global;
using UnityEngine;

namespace Utils
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract void Initialize(GameEventSystem events);
        public abstract void Deinitialize(GameEventSystem events);
    }
}