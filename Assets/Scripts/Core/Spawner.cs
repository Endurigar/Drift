using Fusion;
using UnityEngine;

namespace Core
{
    public class Spawner: NetworkBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        public Transform[] SpawnPoints => spawnPoints;
        private static Spawner _instance;

        public static Spawner Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private void Awake()
        {
            Instance = this;
        }

        public override void Spawned()
        {
            base.Spawned();
            RestartHandler.OnRestart += RestartPositions;
        }

        private void RestartPositions()
        {
            if (Runner.IsServer)
            {
                foreach (var player in Runner.ActivePlayers)
                {
                    var networkObject = Runner.GetPlayerObject(player);

                    if (networkObject != null)
                    {
                        int index = player.PlayerId % spawnPoints.Length;
                        networkObject.transform.position = spawnPoints[index].transform.position;
                        networkObject.transform.rotation = spawnPoints[index].transform.rotation;
                    }
                }
               
            }
        }
       
    }
}
