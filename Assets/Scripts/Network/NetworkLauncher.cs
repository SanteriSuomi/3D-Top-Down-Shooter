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
        //
        // NetworkLauncher handles launching the main level in multiplayer mode.
        //
        [SerializeField]
        private string gameVersion = "1";
        [SerializeField]
        private byte maxPlayersPerRoom = 2;
        [SerializeField]
        private bool automaticSceneSync = true;
        private bool isAttemptingConnection;

        private void Awake()
        {
            // Don't destroy this object on load as it's needed for the scene change.
            DontDestroyOnLoad(gameObject);
            // Set automatic scene sync using the serialized variable.
            PhotonNetwork.AutomaticallySyncScene = automaticSceneSync;
        }

        public void Connect()
        {
            // If player is not attempting connection already...
            if (!isAttemptingConnection)
            {
                // Make sure not to start many scene loads simultaneously.
                isAttemptingConnection = true;
                // Start loading the scene asynchronously.
                StartCoroutine(LoadMainSceneAsync());
                if (PhotonNetwork.IsConnected)
                {
                    // Join a random network room if network is already connected.
                    PhotonNetwork.JoinRandomRoom();
                }
                else
                {
                    // Otherwise connect to the network using the current version settings.
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
                #if UNITY_EDITOR
                Debug.Log($"{PhotonNetwork.NickName} was connected to master.");
                #endif
                // Once connected to network, attempt another room connection.
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                #if UNITY_EDITOR
                Debug.Log($"{PhotonNetwork.NickName} failed to join a random room. A room most likely does not exist. A new room creation will be attempted.");
                #endif
                // If there are no empty rooms to join, create a new room with default options (max players).
                bool newRoomCreated = PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
                #if UNITY_EDITOR
                Debug.Log($"New room has been created: {newRoomCreated.ToString().ToLower()}.");
                #endif
            }
        }
    }
}