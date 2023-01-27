using Data.Map;

namespace Systems.Global.States
{
    public class GameState
    {
        public StarSceneState starState;
        public PlanetSceneState planetState;
        public ShipSceneState shipState;

        public WorldData loadedMapData;
    }
}