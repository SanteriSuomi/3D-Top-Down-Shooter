using Shooter.AI;
using System;
using UnityEngine;

namespace Shooter.Utility
{
	[Serializable]
	public class PlayerData
	{
        //public int Health { get; set; }

        private float[] position;
        public void SetPosition(float[] pos)
        {
            position = pos;
        }
        public float[] GetPosition()
        {
            return position;
        }

        private float[] rotation;
        public void SetRotation(float[] rot)
        {
            rotation = rot;
        }
        public float[] GetRotation()
        {
            return rotation;
        }

        public PlayerData(Character player)
		{
            position = new float[3];
            position[0] = player.transform.position.x;
            position[1] = player.transform.position.y;
            position[2] = player.transform.position.z;
            Debug.Log($"Position: {position[0]}{position[1]}{position[2]}");

            rotation = new float[3];
            rotation[0] = player.transform.rotation.eulerAngles.x;
            rotation[1] = player.transform.rotation.eulerAngles.y;
            rotation[2] = player.transform.rotation.eulerAngles.z;
            Debug.Log($"Rotation: {rotation[0]}{rotation[1]}{rotation[2]}");
        }
	}
}
