using Data.Map;
using Data.Ships;
using Systems.Physics;
using UnityEngine;

namespace Systems.Global
{
    public class SpaceshipManager : SingletonMonoBehaviour<SpaceshipManager>
    {
        public ShipStats Stats => _stats ? _stats : GameDataManager.Instance.defaultShipStats;

        public Vector2 SavedPos => _savedPos;
        public Vector2 SavedVel => _savedVel;
        
        private ShipStats _stats;
        private Vector2 _savedPos;
        private Vector2 _savedVel;

        public void SaveState(PhysicsBody state)
        {
            _savedPos = state.transform.position;
            _savedVel = state.GetVelocity();
        }

        public void PickShip(ShipStats stats)
        { 
            _stats = stats;
        }
    }
}