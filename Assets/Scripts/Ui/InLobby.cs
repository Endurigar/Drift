using System.Linq;
using Fusion;
using Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class InLobby : NetworkBehaviour
    {
        [SerializeField] private Button startGame;
        [SerializeField] private TMP_Text playerTwoText;
        [SerializeField] private TMP_Text waitingForHost;
        [SerializeField] private BasicSpawner basicSpawner;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject lobby;
        [SerializeField] private SceneRef sceneRef;
        [SerializeField] private Button back;
        [SerializeField] private GameObject lobbyListPage;
        private NetworkRunner networkRunner;

        private void Start()
        {
            back.onClick.AddListener(OnBack);
            networkRunner = basicSpawner.GetComponent<NetworkRunner>();
            backButton.onClick.AddListener(OnBackButton);
            startGame.onClick.AddListener(OnStart);
            basicSpawner.OnPlayerJoinedEvent += PlayerJoined;
        }

        private void OnBack()
        {
            lobbyListPage.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void PlayerJoined()
        {
            if (!networkRunner.IsClient)
            {
                startGame.gameObject.SetActive(true);
            }
            if (networkRunner.IsClient)
            {
                playerTwoText.text = "PLAYER 2";
                waitingForHost.gameObject.SetActive(true);
            }
            if (networkRunner.ActivePlayers.Count() >= 2)
            {
                playerTwoText.text = "PLAYER 2";   
            }
        }

        private void OnStart()
        {
            networkRunner.LoadScene(sceneRef);
        }
        private void OnBackButton()
        {
            gameObject.SetActive(false);
            lobby.SetActive(true);
        }
    }
}

