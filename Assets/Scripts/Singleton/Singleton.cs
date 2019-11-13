using UnityEngine;

namespace Shooter.Utility
{
    public abstract class Singleton<T> : SingletonBase where T : Component
    {
        public static T Instance { get; set; }

        public static T GetInstance()
        {
            if (ApplicationIsQuitting) { return null; }

            if (Instance == null)
            {
                Instance = FindObjectOfType<T>();

                if (Instance == null)
                {
                    GameObject gameObject = new GameObject { name = typeof(T).Name };
                    Instance = gameObject.AddComponent<T>();
                }
            }

            return Instance;
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject.transform.root);
            }
            else if (Instance != this as T)
            {
                Destroy(gameObject);
            }
            else { DontDestroyOnLoad(gameObject.transform.root); }
        }

        private void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
    }
}