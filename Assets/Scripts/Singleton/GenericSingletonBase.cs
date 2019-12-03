using Photon.Pun;

namespace Shooter.Utility
{
    public class GenericSingletonBase : MonoBehaviourPunCallbacks 
        // Implement PunCallbacks instead of normal MonoBehaviour as this singleton will be used by script needing networking callbacks.
    {
        // ApplicationIsQuitting as a base for the generic singleton, as static properties may not be inherited correctly for classes inheriting from that.
        protected static bool ApplicationIsQuitting { get; set; } = false;
    }
}