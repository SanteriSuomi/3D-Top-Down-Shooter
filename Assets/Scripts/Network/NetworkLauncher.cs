using Photon.Pun;
using Photon.Realtime;
using Shooter.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.Network
{
    public class NetworkLauncher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private string gameVersion = "1";
        [SerializeField]
        private byte maxPlayersPerRoom = 2;
        [SerializeField]
        private bool automaticSceneSync = true;
        private string clientNickName;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            PhotonNetwork.AutomaticallySyncScene = automaticSceneSync;
            clientNickName = string.Empty;
        }

        public void Connect()
        {
            if (string.IsNullOrEmpty(clientNickName))
            {
                clientNickName = PhotonNetwork.NickName;
            }

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }

            StartCoroutine(LoadMainSceneAsync());
        }

        private IEnumerator LoadMainSceneAsync()
        {
            // Asynchronously load the main scene.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LevelShared.LevelSceneString, LoadSceneMode.Single);
            // Freeze until scene is loaded.
            while (!asyncLoad.isDone && !PhotonNetwork.IsConnectedAndReady)
            {
                yield return null;
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"{clientNickName} was connected to master.");
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log($"{clientNickName} failed to join a random room. A room most likely does not exist. A new room creation will be attempted.");
                bool newRoomCreated = PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
                Debug.Log($"New room creation: {newRoomCreated.ToString().ToLower()}.");
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"{clientNickName} joined a room.");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"{clientNickName} was disconnected, cause: {cause}.");
        }
    }
}