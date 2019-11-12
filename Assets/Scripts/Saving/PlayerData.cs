using System;

namespace Shooter.Utility
{
	[Serializable]
	public class PlayerData
	{
		public int Health { get; set; }
        private readonly float[] position;
        public float[] GetPosition()
        {
            return position;
        }

        private readonly float[] rotation;
        public float[] GetRotation()
        {
            return rotation;
        }

		public PlayerData(PlayerSaveLoad player)
		{
			position = new float[3];
			position[0] = player.transform.position.x;
			position[1] = player.transform.position.y;
			position[2] = player.transform.position.z;

            rotation = new float[3];
            rotation[0] = player.transform.rotation.eulerAngles.x;
            rotation[1] = player.transform.rotation.eulerAngles.y;
            rotation[2] = player.transform.rotation.eulerAngles.z;
		}
	}
}
