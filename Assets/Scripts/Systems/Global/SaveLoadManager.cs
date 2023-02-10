using Data.Map;
using Utils;

namespace Systems.Global
{
    public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
    {
        // This save data will include more info later on
        private WorldData? _currentSave;
        
        public WorldData GetCurrentSave()
        {
            if (_currentSave == null)
            {
                _currentSave = MapGenerator.GenerateWorldData(109123, 200, 4, 8000);
            }

            return _currentSave.Value;
        }

        public void OverwriteSave(WorldData data)
        {
            
        }
    }
}