using System;
using Systems.StarsScene;
using UnityEngine.SceneManagement;
using Utils;

namespace Systems.Global
{
    [Flags]
    public enum GameScope
    {
        Space = 2 << 0,
        Planet = 2 << 1,
        Ship = 2 << 2 // etc...
    }

    public enum GameScene
    {
        Space = 0,
        Planet = 0,
        Ship = 0
    }
    
    public class GameScopeManager : SingletonMonoBehaviour<GameScopeManager>
    {
        public GameScope Current => _current;
        
        private GameScope _current;
        private GameScope _loaded;

        public void Start()
        {
            _current = 0;
            _loaded = 0;
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnterScope(GameScope.Space);
        }

        public void OnDisable()
        {
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
            }
        }

        public void LeaveScope(GameScope scope)
        {
            OnSceneUnloaded(scope);
            switch (scope)
            {
                case GameScope.Space:
                    SceneManager.UnloadSceneAsync("Scenes/SpaceScene");
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
            _current &= ~unloaded;
        }
    }
}