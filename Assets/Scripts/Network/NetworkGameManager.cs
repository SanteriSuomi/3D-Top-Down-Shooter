using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Photon.Realtime;
using Shooter.Utility;

namespace Shooter.Network
{
    public class NetworkGameManager : GenericSingleton<NetworkGameManager>
    {
        [SerializeField]
        private GameObject playerPrefab = default;
        [SerializeField]
        private Vector3 playerPrefabSpawnPosition = new Vector3(0, 0, 2.5f);
        [SerializeField]
        private GameObject quitRoomButton = default;

        protected override void Awake()
        {
            base.Awake();
            PlayerInitialize();
        }

        private void PlayerInitialize()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, playerPrefabSpawnPosition, Quaternion.identity, 0);
                quitRoomButton.SetActive(true);
            }
            else
            {
                Instantiate(playerPrefab).transform.position = playerPrefabSpawnPosition;
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

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"{newPlayer.NickName} has joined the room.");
        }
    }
}