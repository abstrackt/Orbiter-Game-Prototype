using System;
using System.Collections;
using Data.Map;
using Systems.PlanetScene;
using Systems.StarsScene;
using UI;
using UnityEngine;
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
        public UILoadingScreen loadingScreen;
        
        public GameScope Current => _current;
        public bool IsLoading => _loaded != 0;
        
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
            _events.OnEnteredShipView += OnEnteredShipView;
            _events.OnLeftShipView += OnLeftShipView;
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            loadingScreen.Initialize(_events);
            
            EnterScope(GameScope.Space);
        }

        public void OnDisable()
        {
            _events.OnEnteredOrbit -= OnEnteredOrbit;
            _events.OnLeftOrbit -= OnLeftOrbit;
            _events.OnEnteredShipView -= OnEnteredShipView;
            _events.OnLeftShipView -= OnLeftShipView;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            loadingScreen.Deinitialize(_events);
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
                case GameScope.Ship:
                    if ((_current & _loaded) == 0)
                    {
                        _loaded = scope;
                        SceneManager.LoadSceneAsync("Scenes/ShipScene", LoadSceneMode.Additive);
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
                case GameScope.Ship:
                    s = SceneManager.GetSceneByName("ShipScene");
                    foreach (var go in s.GetRootGameObjects())
                    {
                        go.SetActive(false);
                    }
                    SceneManager.UnloadSceneAsync("Scenes/ShipScene");
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
                case GameScope.Planet:
                    PlanetUIManager.Instance.Initialize(events);
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
                    PlanetUIManager.Instance.Deinitialize(events);
                    break;
            }
            
            _current &= ~unloaded;
        }

        public void OnEnteredOrbit(PlanetData planet)
        {
            StartCoroutine(SwitchScenes(1f, GameScope.Space, GameScope.Planet));
        }
        
        public void OnLeftOrbit(PlanetData planet)
        {
            StartCoroutine(SwitchScenes(1f, GameScope.Planet, GameScope.Space));
        }

        public void OnEnteredShipView()
        {
            StartCoroutine(SwitchScenes(1f, GameScope.Planet, GameScope.Ship));
        }
        
        public void OnLeftShipView()
        {
            StartCoroutine(SwitchScenes(1f, GameScope.Ship, GameScope.Planet));
        }

        private IEnumerator SwitchScenes(float delay, GameScope leaving, GameScope entering)
        {
            yield return new WaitForSeconds(delay);
            
            LeaveScope(leaving);
            EnterScope(entering);
        }
    }
}