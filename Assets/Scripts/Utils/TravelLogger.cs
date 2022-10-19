using System.Collections.Generic;
using System.Net.WebSockets;
using UI;
using UnityEngine;
using Visuals;

namespace Utils
{
    public class TravelLogger : MonoBehaviour
    {
        public MapDefinition map;
        public UISystemDiscoveredPanel panel;
        public List<Star> visitedStars = new List<Star>();
        public float visitDistance;

        public void Update()
        {
            var closest = map.ClosestStar;

            if (closest.star != null)
            {
                if (closest.dist < visitDistance && !visitedStars.Contains(closest.star))
                {
                    visitedStars.Add(closest.star);
                    panel.Show(closest.star.name);
                }
            }
        }
    }
}