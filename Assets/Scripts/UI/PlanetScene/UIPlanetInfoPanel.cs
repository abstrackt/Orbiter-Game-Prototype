using System;
using System.Collections;
using Data.Map;
using Systems.Global;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIPlanetInfoPanel : UIPanel
    {
        public Text objectName;
        public Text generalInfo;
        public Text detailedInfo;
        public CanvasGroup canvasGroup;

        private LocationManager _location;
        
        public override void Initialize(GameEventSystem events)
        {
            
        }

        public override void Deinitialize(GameEventSystem events)
        {
            
        }

        public void Start()
        {
            _location = LocationManager.Instance;

            if (_location.OrbitingPlanet.HasValue)
            {
                StartCoroutine(FadeIn());
                SetUp(_location.OrbitingPlanet.Value);
            }
        }

        public void SetUp(PlanetData data)
        {
            objectName.text = data.planetName;

            var general = string.Empty;

            general += $"Type: {Enum.GetName(typeof(PlanetType), data.planetType)}";

            var detailed = string.Empty;

            detailed += $"Pressure: {data.atmoData.pressure:F2}hPa\n";
            detailed += $"Temperature: {data.atmoData.temperature:F2}°C\n";
            detailed += $"Radiation: {data.surfaceRadiation:F2}mSv/h\n";
            detailed += $"Inhabited: {(data.Inhabited ? "YES" : "NO")}\n";
            detailed += $"Gravity: {data.SurfaceGravity:F2}G\n";
            detailed += $"Radius: {data.radius:F2} Earth\n";
            detailed += $"Sea coverage: {data.seaLevel:F2}%";

            generalInfo.text = general;
            detailedInfo.text = detailed;
        }

        public IEnumerator FadeIn()
        {
            canvasGroup.alpha = 0;
            yield return new WaitForSeconds(2f);

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;
        }
    }
}