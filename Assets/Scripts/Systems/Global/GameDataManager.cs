using Data.Config;
using Data.Ships;

namespace Systems.Global
{
    public class GameDataManager : SingletonMonoBehaviour<GameDataManager>
    {
        public StarsMapConfig stars;
        public ShipStats defaultShipStats;
    }
}