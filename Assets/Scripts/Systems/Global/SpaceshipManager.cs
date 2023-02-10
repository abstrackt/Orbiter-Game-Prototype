using Data.Map;
using Data.Ships;
using Systems.Physics;
using UnityEngine;

namespace Systems.Global
{
    public class SpaceshipManager : SingletonMonoBehaviour<SpaceshipManager>
    {
        public ShipStats Stats => _stats ? _stats : GameDataManager.Instance.defaultShipStats;
        public PlanetData? OrbitingPlanet => _currentPlanet;

        public Vector2 SavedPos => _savedPos;
        public Vector2 SavedVel => _savedVel;
        
        private ShipStats _stats;
        private PlanetData? _currentPlanet;
        private Vector2 _savedPos;
        private Vector2 _savedVel;

        public void EnterOrbit(PlanetData planet, PhysicsBody state)
        {
            _currentPlanet = planet;
            _savedPos = state.transform.position;
            _savedVel = state.GetVelocity();
            GameEventSystem.Instance.OnEnteredOrbit?.Invoke(planet);
        }

        public void LeaveOrbit()
        {
            if (_currentPlanet.HasValue)
            {
                GameEventSystem.Instance.OnLeftOrbit?.Invoke(_currentPlanet.Value);
            }
        }
        
        public void PickShip(ShipStats stats)
        { 
            _stats = stats;
        }
    }
}