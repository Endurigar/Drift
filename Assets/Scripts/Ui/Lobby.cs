using Fusion;
using Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private BasicSpawner basicSpawner;
        [SerializeField] private Button hostButton;
        [SerializeField] private GameObject createLobbyPage;
        [SerializeField] private Button startLobby;
        [SerializeField] private TMP_InputField lobbyName;
        [SerializeField] private GameObject inLobbyPage;
        [SerializeField] private GameObject joinPage;
        [SerializeField] private TMP_InputField joinLobbyName;
        [SerializeField] private Button joinByName;
        [SerializeField] private Button join;
        [SerializeField] private Button back;
        [SerializeField] private GameObject mainMenu;
        private NetworkRunner networkRunner;

        private void Start()
        {
            back.onClick.AddListener(OnBack);
            hostButton.onClick.AddListener(HostRoom);
            startLobby.onClick.AddListener(CreateLobby);
            joinByName.onClick.AddListener(JoinRoom);
            join.onClick.AddListener(JoinLobby);
            networkRunner = basicSpawner.GetComponent<NetworkRunner>();
        }

        private void OnBack()
        {
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void HostRoom()
        {
            createLobbyPage.SetActive(true);
            if (joinPage.activeInHierarchy)
            {
                joinPage.SetActive(false);
            }
        }

        private void JoinRoom()
        {
            joinPage.SetActive(true);
            if (createLobbyPage.activeInHierarchy)
            {
                createLobbyPage.SetActive(false);
            }
        }

        private void CreateLobby()
        {
            string newLobbyName = lobbyName.text;
            if (newLobbyName.Length == 0)
            {
                newLobbyName = "No Name";
            }
            StartGameArgs startGameArgs = 
                new StartGameArgs()
                {
                    GameMode = GameMode.Host,
                    SessionName = newLobbyName,
                };
            basicSpawner.StartGame(GameMode.Host,startGameArgs);
            createLobbyPage.SetActive(false);
            this.gameObject.SetActive(false);
            inLobbyPage.SetActive(true);
        }

        private void JoinLobby()
        {
            string newLobbyName = lobbyName.text;
            StartGameArgs startGameArgs = 
                new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    SessionName = newLobbyName,
                };
            basicSpawner.StartGame(GameMode.Client,startGameArgs);
            createLobbyPage.SetActive(false);
            this.gameObject.SetActive(false);
            inLobbyPage.SetActive(true);
        }
    }
}
