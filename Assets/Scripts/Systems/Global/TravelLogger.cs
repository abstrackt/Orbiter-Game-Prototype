using System.Collections.Generic;
using Data.Map;
using Systems.StarsScene;
using UI;
using UnityEngine;

namespace Systems.Global
{
    public class TravelLogger : MonoBehaviour
    {
        public StarsMapManager map;
        public UISystemDiscoveredPanel panel;
        public List<StarData> visitedStars = new ();
        public float visitDistance;

        public void Update()
        {
            var closest = map.ClosestStarData;
            
            if (closest.dist < visitDistance && !visitedStars.Contains(closest.star))
            {
                visitedStars.Add(closest.star);
                panel.Show(closest.star.starName);
            }
        }
    }
}