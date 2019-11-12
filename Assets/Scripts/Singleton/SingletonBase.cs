using UnityEngine;

namespace Shooter.Utility
{
    public class SingletonBase : MonoBehaviour
    {
        protected static bool ApplicationIsQuitting { get; set; } = false;
    }
}