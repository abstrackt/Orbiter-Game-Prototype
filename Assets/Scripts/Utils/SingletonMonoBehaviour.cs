using UnityEngine;

namespace Systems.Global
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        Debug.LogError(typeof(T) + "does not exist in this scene");
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (this != Instance)
            {
                Destroy(gameObject);
            }
        }
    }
}