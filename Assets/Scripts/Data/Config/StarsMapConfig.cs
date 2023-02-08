using UnityEngine;

namespace Data.Config
{
    [CreateAssetMenu(fileName = "Stars Config", menuName = "Data/Scenes/Stars Config", order = 1)]
    public class StarsMapConfig : ScriptableObject
    {
        public float starVisitedDistance;
    }
}