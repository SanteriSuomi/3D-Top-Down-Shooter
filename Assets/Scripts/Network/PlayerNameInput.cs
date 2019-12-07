using Photon.Pun;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using TMPro;

namespace Shooter.Network
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInput : MonoBehaviour
    {
        //
        // PlayerNameInput handles setting the player name and saving it in playerprefs.
        //
        private const string playerNamePrefKey = "PlayerName";
        private TMP_InputField inputField;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            // Name is empty by default.
            string defaultName = string.Empty;
            if (inputField != null && PlayerPrefs.HasKey(playerNamePrefKey))
            {
                // Start the name field with a saved one from playerprefs.
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                // Update the input field with it.
                inputField.text = defaultName;
            }
            // Update the networked name for this player.
            PhotonNetwork.NickName = defaultName;
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", 
            Justification = "While member is not accessing member fields, it is being dynamically called from a unity event.")]
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                #if UNITY_EDITOR
                Debug.LogError("Player name is null or empty.");
                #endif
            }
            else
            {
                // Update networked name with the value from the input field.
                PhotonNetwork.NickName = value;
                // Store it in the memory with playerprefs.
                PlayerPrefs.SetString(playerNamePrefKey, value);
            }
        }
    }
}