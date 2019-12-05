using Shooter.Enemy;
using UnityEngine;

namespace Shooter.Factory
{
    public class NpcFactory : MonoBehaviour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0010:MonoBehavior instance creation is not recommended", Justification = "Not used currently")]
        public static INpc GetNewNPC(NPCType npcType)
        {
            switch (npcType)
            {
                case NPCType.Ghost:
                    INpc npc = new EnemyGhost() as INpc;
                    return npc;
                default:
                    break;
            }

            return null;
        }
    }
}