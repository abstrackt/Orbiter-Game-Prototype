using System.Collections.Generic;
using Data.Map;
using Systems.StarsScene;
using UnityEngine;

namespace Systems.Global
{
    public class StarsTravelLogger : MonoBehaviour
    {
        public StarsMapManager map;
        public List<StarData> visitedStars = new ();
        public float visitDistance;

        public void Update()
        {
            var closest = map.ClosestStarData;
            
            if (closest.dist < visitDistance && !visitedStars.Contains(closest.star))
            {
                visitedStars.Add(closest.star);
            }
        }
    }
}