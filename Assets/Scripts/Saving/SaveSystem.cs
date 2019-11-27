using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Shooter.Player;

namespace Shooter.Utility
{
	public static class SaveSystem
	{
        //
        // Class that is used for serializing/deserializing the required data.
        //
        private const string pathEnd = "save.bin";
        private static readonly string savePath = Path.Combine(Application.persistentDataPath, pathEnd);

        public static void SavePlayer(PlayerSettings player)
        {
            // Create a new binaryformatter isntance.
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            // Create a new filestream to the savePath.
            FileStream fileStream = new FileStream(savePath, FileMode.Create);
            // Create a new playersavedata from the player settings.
            PlayerSaveData data = new PlayerSaveData(player);
            Serialize(binaryFormatter, fileStream, data);
        }

        private static void Serialize(BinaryFormatter binaryFormatter, FileStream fileStream, PlayerSaveData data)
        {
            try
            {
                // Attempt to serialize the data using the instanced filestream to the disk.
                binaryFormatter.Serialize(fileStream, data);
            }
            catch (ArgumentNullException e)
            {
                Debug.LogError($"{typeof(SaveSystem).Name} failed to serialize. {e}");
            }
            finally
            {
                // Remember to close the filestream to prevent memory leaks etc.
                fileStream.Close();
            }
        }

        public static PlayerSaveData LoadPlayer()
		{
            // Make sure that the serialized data file exits on the drive.
			if (File.Exists(savePath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(savePath, FileMode.Open);
                PlayerSaveData data = null;
                data = Deserialize(binaryFormatter, fileStream, data);
                // Return the data to the methond requesting it.
                return data;
            }
            else
			{
				Debug.LogError($"File was not found in {savePath}");
				return null;
			}
		}

        private static PlayerSaveData Deserialize(BinaryFormatter binaryFormatter, FileStream fileStream, PlayerSaveData data)
        {
            try
            {
                // Deserialize the data as the PlayerSaveData using binaryformatter.
                data = binaryFormatter.Deserialize(fileStream) as PlayerSaveData;
            }
            catch (ArgumentNullException e)
            {
                Debug.LogError($"{typeof(SaveSystem).Name} failed to deserialize. {e}");
            }
            finally
            {
                // Remember to close the filestream to prevent memory leaks etc.
                fileStream.Close();
            }
            // Return the data to the methond requesting it.
            return data;
        }
    }
}