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
        private bool isAttemptingConnection;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            PhotonNetwork.AutomaticallySyncScene = automaticSceneSync;
        }

        public void Connect()
        {
            if (!isAttemptingConnection)
            {
                isAttemptingConnection = true;
                StartCoroutine(LoadMainSceneAsync());

                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                else
                {
                    PhotonNetwork.GameVersion = gameVersion;
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
        }

        private IEnumerator LoadMainSceneAsync()
        {
            // Asynchronously load the main scene.
            AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(LevelShared.LevelSceneString, LoadSceneMode.Single);
            asyncSceneLoad.allowSceneActivation = false;
            // Freeze until scene is loaded.
            yield return new WaitWhile(() => !asyncSceneLoad.isDone && !PhotonNetwork.IsConnectedAndReady);
            isAttemptingConnection = false;
            asyncSceneLoad.allowSceneActivation = true;
        }

        public override void OnConnectedToMaster()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log($"{PhotonNetwork.NickName} was connected to master.");
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log($"{PhotonNetwork.NickName} failed to join a random room. A room most likely does not exist. A new room creation will be attempted.");
                bool newRoomCreated = PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
                Debug.Log($"New room creation: {newRoomCreated.ToString().ToLower()}.");
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"{PhotonNetwork.NickName} joined the room {PhotonNetwork.CurrentRoom.Name}.");
        }
    }
}