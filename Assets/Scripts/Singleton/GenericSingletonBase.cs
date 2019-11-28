using UnityEngine;

namespace Shooter.Utility
{
    public class GenericSingletonBase : MonoBehaviour
    {
        // ApplicationIsQuitting as a base for the generic singleton, as static properties may not be inherited correctly for classes inheriting from that.
        protected static bool ApplicationIsQuitting { get; set; } = false;
    }
}