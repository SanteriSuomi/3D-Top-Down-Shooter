using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace Shooter.Utility
{
    public abstract class GenericSingleton<T> : GenericSingletonBase where T : Component
    {
        private static T Instance { get; set; }

        public static T GetInstance()
        {
            // If application is quitting, make sure to not return singleton instance.
            if (ApplicationIsQuitting) { return null; }

            if (Instance == null)
            {
                // Make sure that instance is null.
                Instance = FindObjectOfType<T>();

                if (Instance == null)
                {
                    // If it's null, create a new GameObject and add the T component to it.
                    GameObject gameObject = new GameObject { name = typeof(T).Name };
                    Instance = gameObject.AddComponent<T>();
                }
            }

            return Instance;
        }

        [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "Intended, temporary")]
        protected virtual void Awake()
        {
            if (gameObject.transform.parent != null)
            {
                // Alert if this instance has a parent. (child objects cannot be DontDestroyOnLoad'd).
                #if UNITY_EDITOR
                Debug.Log($"{typeof(T).Name} has a parent. Is this intended?");
                #endif
            }

            if (Instance == null)
            {
                // Apply the inherited component to this instance.
                Instance = this as T;
                // Make the gameObject persist between scenes.
                //DontDestroyOnLoad(gameObject);
                if (gameObject.name == "MenuSceneLoad")
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            // If this is some other component, destroy it.
            else if (Instance != this as T)
            {
                Destroy(gameObject);
            }
            //else { DontDestroyOnLoad(gameObject); }
        }

        // All the below methods are to make sure singleton instance gets returned whilst game is quitting/going on pause.

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

        #if UNITY_STANDALONE
        private void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
        #endif
    }
}