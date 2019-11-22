using Shooter.Player;
using System;

namespace Shooter.Utility
{
	[Serializable]
	public class PlayerSaveData
	{
        public float Hitpoints { get; set; }
        public float Funds { get; set; }
        public float Score { get; set; }
        public float PlayerSensitivityMultiplier { get; set; }

        private float[] position;
        public void SetPosition(float[] position)
        {
            this.position = position;
        }
        public float[] GetPosition()
        {
            return position;
        }

        private float[] rotation;
        public void SetRotation(float[] rotation)
        {
            this.rotation = rotation;
        }
        public float[] GetRotation()
        {
            return rotation;
        }

        public PlayerSaveData(PlayerSettings player)
		{
            Hitpoints = player.HitPoints;
            Funds = player.Funds;
            Score = player.Score;
            PlayerSensitivityMultiplier = player.PlayerSensitivityMultiplier;

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
