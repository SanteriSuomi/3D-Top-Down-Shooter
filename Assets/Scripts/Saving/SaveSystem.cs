using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Shooter.Player;

namespace Shooter.Utility
{
	public static class SaveSystem
	{
        private const string pathEnd = "player.bin";
        private static readonly string savePath = Path.Combine(Application.persistentDataPath, pathEnd);

        public static void SavePlayer(PlayerSettings player)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(savePath, FileMode.Create);
            PlayerSaveData data = new PlayerSaveData(player);
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

		public static PlayerSaveData LoadPlayer()
		{
			if (File.Exists(savePath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = new FileStream(savePath, FileMode.Open);
                PlayerSaveData data = null;
                try
                {
                    data = binaryFormatter.Deserialize(fileStream) as PlayerSaveData;
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