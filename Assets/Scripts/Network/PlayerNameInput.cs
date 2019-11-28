using Photon.Pun;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using TMPro;

namespace Shooter.Network
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInput : MonoBehaviour
    {
        const string playerNamePrefKey = "PlayerName";

        private void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            if (inputField != null && PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }

            PhotonNetwork.NickName = defaultName;
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", 
            Justification = "While member is not accessing member fields, it is being dynamically called from a unity event.")]
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player name is null or empty.");
            }
            else
            {
                PhotonNetwork.NickName = value;
                PlayerPrefs.SetString(playerNamePrefKey, value);
            }
        }
    }
}