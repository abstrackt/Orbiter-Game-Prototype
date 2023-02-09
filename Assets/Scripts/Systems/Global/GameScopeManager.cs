using System;
using Data.Map;
using Systems.StarsScene;
using UnityEngine.SceneManagement;

namespace Systems.Global
{
    [Flags]
    public enum GameScope
    {
        Space = 2 << 0,
        Planet = 2 << 1,
        Ship = 2 << 2 // etc...
    }

    public class GameScopeManager : SingletonMonoBehaviour<GameScopeManager>
    {
        public GameScope Current => _current;
        
        private GameScope _current;
        private GameScope _loaded;

        private GameEventSystem _events;

        public void Start()
        {
            _current = 0;
            _loaded = 0;
            _events = GameEventSystem.Instance;
            _events.OnEnteredOrbit += OnEnteredOrbit;
            _events.OnLeftOrbit += OnLeftOrbit;
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnterScope(GameScope.Space);
        }

        public void OnDisable()
        {
            _events.OnEnteredOrbit -= OnEnteredOrbit;
            _events.OnLeftOrbit -= OnLeftOrbit;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            LeaveScope(GameScope.Space);
        }

        public void EnterScope(GameScope scope)
        {
            switch (scope)
            {
                case GameScope.Space:
                    if ((_current & _loaded) == 0)
                    {
                        _loaded = scope;
                        SceneManager.LoadSceneAsync("Scenes/SpaceScene", LoadSceneMode.Additive);
                    }
                    break;
                case GameScope.Planet:
                    if ((_current & _loaded) == 0)
                    {
                        _loaded = scope;
                        SceneManager.LoadSceneAsync("Scenes/PlanetScene", LoadSceneMode.Additive);
                    }
                    break;
            }
        }

        public void LeaveScope(GameScope scope)
        {
            OnSceneUnloaded(scope);
            Scene s;
            switch (scope)
            {
                case GameScope.Space:
                    s = SceneManager.GetSceneByName("SpaceScene");
                    foreach (var go in s.GetRootGameObjects())
                    {
                        go.SetActive(false);
                    }
                    SceneManager.UnloadSceneAsync("Scenes/SpaceScene");
                    break;
                case GameScope.Planet:
                    s = SceneManager.GetSceneByName("PlanetScene");
                    foreach (var go in s.GetRootGameObjects())
                    {
                        go.SetActive(false);
                    }
                    SceneManager.UnloadSceneAsync("Scenes/PlanetScene");
                    break;
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            var events = GameEventSystem.Instance;
            switch (_loaded)
            {
                case GameScope.Space:
                    StarsUIManager.Instance.Initialize(events);
                    break;
            }

            _current |= _loaded;
            _loaded = 0;
        }

        public void OnSceneUnloaded(GameScope unloaded)
        {
            var events = GameEventSystem.Instance;
            switch (unloaded)
            {
                case GameScope.Space:
                    StarsUIManager.Instance.Deinitialize(events);
                    break;
                case GameScope.Planet:
                    break;
            }
            
            _current &= ~unloaded;
        }

        public void OnEnteredOrbit(PlanetData planet)
        {
            LeaveScope(GameScope.Space);
            EnterScope(GameScope.Planet);
        }
        
        public void OnLeftOrbit(PlanetData planet)
        {
            LeaveScope(GameScope.Planet);
            EnterScope(GameScope.Space);
        }
    }
}