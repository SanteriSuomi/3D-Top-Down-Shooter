using UnityEngine;

namespace Shooter.Utility
{
    public class GenericSingletonBase : MonoBehaviour
    {
        protected static bool ApplicationIsQuitting { get; set; } = false;
    }
}