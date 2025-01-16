using Fusion;
using Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class SinglePlayerStart : MonoBehaviour
    {
        [SerializeField] private BasicSpawner basicSpawner;
        [SerializeField] private Button startButton;
        [SerializeField] private SceneRef sceneRef;
        private NetworkRunner networkRunner;

        private void Start()
        {
            startButton.onClick.AddListener(CreateLobby);
            networkRunner = basicSpawner.GetComponent<NetworkRunner>();
        }

        private void CreateLobby()
        {
            StartGameArgs startGameArgs =
                new StartGameArgs()
                {
                    GameMode = GameMode.Single,
                };
            basicSpawner.StartGame(GameMode.Single, startGameArgs);
        }
    }
}