using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Shooter.Network
{
    public class NetworkGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject quitRoomButton = default;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                quitRoomButton.SetActive(true);
            }
            else
            {
                quitRoomButton.SetActive(false);
            }
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Member is accessed by unity events")]
        public void LeaveRoom()
        {
            Debug.Log($"{PhotonNetwork.NickName} has left the room.");
            PhotonNetwork.Disconnect();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
    }
}