using UnityEngine;

namespace Utils
{
    public class VSync : MonoBehaviour
    {
        public int targetFrameRate = 60;

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }
    }
}