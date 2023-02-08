using System;
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

        public void Start()
        {
            _current = 0;
            EnterScope(GameScope.Space);
        }

        public void EnterScope(GameScope scope)
        {
            switch (scope)
            {
                case GameScope.Space:
                    SceneManager.LoadScene("Scenes/SpaceScene", LoadSceneMode.Additive);
                    break;
            }
        }

        public void LeaveScope(GameScope scope)
        {
            switch (scope)
            {
                case GameScope.Space:
                    SceneManager.UnloadSceneAsync("Scenes/SpaceScene");
                    break;
            }
        }
    }
}