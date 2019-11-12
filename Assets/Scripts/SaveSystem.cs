using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace Shooter.Utility
{
	public static class SaveSystem
	{
        private static readonly string savePath = $"{Application.persistentDataPath}/player.bin";

        public static void SavePlayer(PlayerSaveLoad player)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(savePath, FileMode.Create);
			PlayerData data = new PlayerData(player);
            try
            {
                binaryFormatter.Serialize(fileStream, data);
            }
            catch (Exception e)
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"{typeof(SaveSystem).Name} Failed to serialize. {e}");
                #endif
            }
            finally
            {
                fileStream.Close();
            }
        }

		public static PlayerData LoadPlayer()
		{
			if (File.Exists(savePath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = new FileStream(savePath, FileMode.Open);
                PlayerData data = null;
                try
                {
                    data = binaryFormatter.Deserialize(fileStream) as PlayerData;
                }
                catch (Exception e)
                {
                    #if UNITY_EDITOR
                    Debug.LogWarning($"{typeof(SaveSystem).Name} Failed to deserialize. {e}");
                    #endif
                }
                finally
                {
                    fileStream.Close();
                }

                return data;
			}
			else
			{
				Debug.LogError($"File was not found in {savePath}");
				return null;
			}
		}
	}
}