using Systems.Global;
using UI;

namespace Systems.StarsScene
{
    public class StarsUIManager : SingletonMonoBehaviour<StarsUIManager>
    {
        public UIPlanetIndicatorPanel planetIndicator;
        public UIShipIndicatorPanel shipIndicator;
        public UIShipStatusPanel shipStatus;
        public UIOrbitalInsertionPanel orbitalInsertion;
        public UISystemDiscoveredPanel systemDiscoveredPanel;
        
        public void Initialize(GameEventSystem events)
        {
            planetIndicator.Initialize(events);
            shipIndicator.Initialize(events);
            shipStatus.Initialize(events);
            orbitalInsertion.Initialize(events);
            systemDiscoveredPanel.Initialize(events);
        }

        public void Deinitialize(GameEventSystem events)
        {
            planetIndicator.Deinitialize(events);
            shipIndicator.Deinitialize(events);
            shipStatus.Deinitialize(events);
            orbitalInsertion.Deinitialize(events);
            systemDiscoveredPanel.Deinitialize(events);
        }
    }
}