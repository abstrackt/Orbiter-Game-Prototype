using System;
using Data.Map;

namespace Systems.Global
{
    // Class for handling subscribing / unsubscribing from game events.
    public class GameEventSystem : SingletonMonoBehaviour<GameEventSystem>
    {
        public Action<string> OnSystemDiscovered;
        public Action<PlanetData> OnEnteredOrbit;
        public Action<PlanetData> OnLeftOrbit;
        public Action OnEnteredShipView;
        public Action OnLeftShipView;
    }
}