using Data.Map;

namespace Systems.Global
{
    public class LocationManager : SingletonMonoBehaviour<LocationManager>
    {
        public PlanetData? OrbitingPlanet => _currentPlanet;
        public StarData? OrbitingStar => _currentStar;
        public float StarDistance => _starDistance;
        public float StarPhase => _starPhase;

        private PlanetData? _currentPlanet;
        private StarData? _currentStar;
        private float _starDistance;
        private float _starPhase;

        public void EnterOrbit(PlanetData planet, StarData? star, float dist, float phase)
        {
            _currentPlanet = planet;
            _currentStar = star;
            _starDistance = dist;
            _starPhase = phase;
            GameEventSystem.Instance.OnEnteredOrbit?.Invoke(_currentPlanet.Value);
        }
        
        public void LeaveOrbit()
        {
            if (_currentPlanet.HasValue)
            {
                GameEventSystem.Instance.OnLeftOrbit?.Invoke(_currentPlanet.Value);
                _currentPlanet = null;
                _currentStar = null;
                _starDistance = 0f;
            }
        }

        public void EnterShipView()
        {
            GameEventSystem.Instance.OnEnteredShipView.Invoke();
        }

        public void LeaveShipView()
        {
            GameEventSystem.Instance.OnLeftShipView.Invoke();
        }
    }
}