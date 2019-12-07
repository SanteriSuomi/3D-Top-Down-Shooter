using Photon.Pun;
using Photon.Realtime;
using Shooter.Utility;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private bool hasDestroyedSingleplayerPlayer;

        public override void OnJoinedRoom()
        {
            #if UNITY_EDITOR
            Debug.Log($"{PhotonNetwork.NickName} joined the room {PhotonNetwork.CurrentRoom.Name}.");
            #endif
            // Initialize a new player on server room join.
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (!hasDestroyedSingleplayerPlayer)
                {
                    // Destroy the singleplayer player prefab if this is the network version of the game.
                    hasDestroyedSingleplayerPlayer = true;
                    Destroy(FindObjectOfType<Player.Player>().gameObject);
                }
                // Instantiate a new networked player prefab from /resources folder.
                PhotonNetwork.Instantiate(playerPrefab.name, playerPrefabSpawnPosition, Quaternion.identity, 0, null);
                // Make sure quitRoomButton is visible for leaving the room.
                quitRoomButton.SetActive(true);
            }
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Member is accessed by unity events")]
        public void LeaveRoomAndDisconnect()
        {
            #if UNITY_EDITOR
            Debug.Log($"{PhotonNetwork.NickName} has left the room and disconnected.");
            #endif
            // Disconnect player and call OnDisconnected.
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            #if UNITY_EDITOR
            Debug.Log($"{PhotonNetwork.NickName} was disconnected, cause: {cause}.");
            #endif
            // Make sure cursor is visible for the main menu.
            Cursor.visible = true;
            // Load the main menu.
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            // Not empty.
            #if UNITY_EDIT
            Debug.Log($"{newPlayer.NickName} has joined the room.");
            #endif
        }
    }
}