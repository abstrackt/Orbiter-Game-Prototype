using Systems.Global;
using UI;

namespace Systems.PlanetScene
{
    public class PlanetUIManager : SingletonMonoBehaviour<PlanetUIManager>
    {
        public UIPlanetInfoPanel planetInfo;
        
        public void Initialize(GameEventSystem events)
        {
            planetInfo.Initialize(events);
        }

        public void Deinitialize(GameEventSystem events)
        {
            planetInfo.Deinitialize(events);
        }
    }
}