using UnityEngine;

namespace Shooter.Utility
{
    public abstract class GenericSingleton<T> : GenericSingletonBase where T : Component
    {
        private static T Instance { get; set; }

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
            if (gameObject.transform.parent != null)
            {
                #if UNITY_EDITOR
                Debug.Log($"{typeof(T).Name} has a parent. Is this intended?");
                #endif
            }

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

        #if UNITY_STANDALONE
        private void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
        #endif

        #if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
        #endif

        #if UNITY_ANDROID
        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                ApplicationIsQuitting = true;
            }
            else
            {
                ApplicationIsQuitting = false;
            }
        }
        #endif
    }
}