using Data.Ships;

namespace Systems.Global
{
    public class SpaceshipManager : SingletonMonoBehaviour<SpaceshipManager>
    {
        public ShipStats Stats => _stats;
        
        private ShipStats _stats;
    }
}