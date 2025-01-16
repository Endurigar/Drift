using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using Mods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Multiplayer
{
    public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        [FormerlySerializedAs("_playerPrefab")] [SerializeField] private NetworkPrefabRef playerPrefab;
        [Networked]private Dictionary<PlayerRef, NetworkObject> SpawnedCharacters { get; set; }
        private NetworkRunner runner;
        private RunnerSimulatePhysics3D simulate3DRunner;
        public event Action<byte[]> OnDataReceived;
        public event Action OnPlayerJoinedEvent;
        public event Action OnRoundStarted;
        public event Action OnSceneLoaded;
        public static BasicSpawner Instance { get; private set; }
    
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            OnPlayerJoinedEvent?.Invoke();   
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (SpawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                SpawnedCharacters.Remove(player);
            }
        }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        
            var data = new NetworkInputData();
            data.Horizontal = Input.GetAxis("Horizontal");
            data.Vertical = Input.GetAxis("Vertical");
            input.Set(data);
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            SceneManager.LoadScene(0);
        }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {}
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public async void OnSceneLoadDone(NetworkRunner runner)
        {
            OnSceneLoaded?.Invoke();
            if (runner.IsServer)
            {
                foreach (var player in runner.ActivePlayers)
                {
                    Vector3 spawnPosition = Spawner.Instance.SpawnPoints[player.PlayerId-1].transform.position;
                    NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Spawner.Instance.SpawnPoints[player.PlayerId-1].transform.rotation, player);
                    SpawnedCharacters.Add(player, networkPlayerObject);   
                }
            }
            if (runner.GameMode == GameMode.Single)
            {
                foreach (var character in SpawnedCharacters)
                {
                    character.Value.GetComponent<Car>().SpawnSavedCar();
                }
                OnRoundStarted?.Invoke();
                return;
            }
            if (runner.ActivePlayers.Count() == 2)
            {
                await Task.Delay(1000);
                OnRoundStarted?.Invoke();
                var cars = FindObjectsOfType<Car>();
                foreach (var character in cars)
                {
                    if (character.Object.HasInputAuthority)
                    {
                        Debug.Log("SPAWnSAVED CAR");
                        character.SpawnSavedCar();
                    }
                }
            }
        }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){Debug.Log("Data"); OnDataReceived?.Invoke(data.Array); }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }

        private void Start()
        {
            SpawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
            Instance = this;
        }

        public async void StartGame (GameMode mode, StartGameArgs startGameArgs)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
            runner.AddCallbacks(this);
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid) {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }
            await runner.StartGame(startGameArgs);
            if (mode == GameMode.Single)
            {
                runner.LoadScene(SceneRef.FromIndex(1));
            }
        }
    }
}
