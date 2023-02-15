using System.Collections;
using Data.Map;
using Systems.Global;
using UnityEngine;
using Utils;

namespace UI
{
    public class UILoadingScreen : UIPanel
    {
        public CanvasGroup canvasGroup;

        public override void Initialize(GameEventSystem events)
        {
            events.OnEnteredOrbit += Show;
            events.OnLeftOrbit += Show;
            events.OnEnteredShipView += Show;
            events.OnLeftShipView += Show;
        }

        public override void Deinitialize(GameEventSystem events)
        {
            events.OnEnteredOrbit -= Show;
            events.OnLeftOrbit -= Show;
            events.OnEnteredShipView -= Show;
            events.OnLeftShipView -= Show;
        }

        public void Show(PlanetData planetData)
        {
            StartCoroutine(Animate());
        }
        
        public void Show()
        {
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            canvasGroup.alpha = 0;

            yield return null;

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime;
                yield return null;
            }
            
            canvasGroup.alpha = 1;

            while (GameScopeManager.Instance.IsLoading)
                yield return null;
            
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= 2 * Time.deltaTime;
                yield return null;
            }
            
            canvasGroup.alpha = 0;
        }
    }
}