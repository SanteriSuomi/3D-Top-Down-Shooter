using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Photon.Realtime;

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
        public void LeaveRoomAndDisconnect()
        {
            Debug.Log($"{PhotonNetwork.NickName} has left the room and disconnected.");
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"{PhotonNetwork.NickName} was disconnected, cause: {cause}.");
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
    }
}