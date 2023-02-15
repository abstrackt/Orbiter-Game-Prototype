using System.Collections.Generic;
using Data.Map;
using Systems.StarsScene;
using UnityEngine;

namespace Systems.Global
{
    public class StarsTravelLogger : MonoBehaviour
    {
        private StarsMapManager _map;
        private List<StarData> _visitedStars = new ();
        private float _visitDistance;

        public void Start()
        {
            var data = GameDataManager.Instance;
            _visitDistance = data.stars.starVisitedDistance;
            _map = StarsMapManager.Instance;
        }
        
        public void Update()
        {
            var closest = _map.ClosestStarData;
            
            if (closest.dist < _visitDistance && !_visitedStars.Contains(closest.star))
            {
                _visitedStars.Add(closest.star);
                GameEventSystem.Instance.OnSystemDiscovered?.Invoke(closest.star.starName);
            }
        }
    }
}