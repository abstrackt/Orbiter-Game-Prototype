using System.Collections.Generic;
using Data.Map;
using Utils;

namespace Systems.Global
{
    public class SaveManager : SingletonMonoBehaviour<SaveManager>
    {
        private WorldData? _currentSave;
        
        private Dictionary<MapID, StarData> _starDict = new(); 
        private Dictionary<MapID, PlanetData> _planetDict = new(); 
        
        public WorldData GetCurrentSave()
        {
            if (_currentSave == null)
            {
                _currentSave = MapGenerator.GenerateWorldData(109123, 200, 4, 8000);
            }

            RefreshTempInfo();

            return _currentSave.Value;
        }

        public void OverwriteSave(WorldData data)
        {
            _currentSave = data;
        }

        private void RefreshTempInfo()
        {
            _starDict.Clear();
            _planetDict.Clear();
            
            if (!_currentSave.HasValue)
                return;

            foreach (var system in _currentSave.Value.systems)
            {
                _starDict.Add(system.star.id, system.star);
                foreach (var planet in system.planets)
                {
                    _planetDict.Add(planet.id, planet);
                }
            }
        }
    }
}