using System;

namespace Systems.Global
{
    // Class for handling subscribing / unsubscribing from game events.
    public class GameEventSystem : SingletonMonoBehaviour<GameEventSystem>
    {
        public Action<string> OnSystemDiscovered;
    }
}