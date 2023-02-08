using Data.Ships;

namespace Systems.Global
{
    public class SpaceshipManager : SingletonMonoBehaviour<SpaceshipManager>
    {
        public ShipStats Stats => _stats ? _stats : GameDataManager.Instance.defaultShipStats;
        
        private ShipStats _stats;

        public void PickShip(ShipStats stats)
        { 
            _stats = stats;
        }
    }
}