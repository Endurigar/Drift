using Core;
using UnityEngine;

namespace Ui
{
    public class UiHandler : MonoBehaviour
    {
        [SerializeField] private UiRoundTimer uiRoundTimer;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject roundEndPanel;
        [SerializeField] private GameObject player;
        private Transform playerPosition;
        private Rigidbody playerRigidbody;
        private Vector3 startPosition;
        private Quaternion startRotation;

        private void Start()
        {
            RestartHandler.OnRestart += RoundStart;
            playerPosition = player.GetComponent<Transform>();
            playerRigidbody = player.GetComponent<Rigidbody>();
            startPosition = playerPosition.position;
            startRotation = playerPosition.rotation;
            uiRoundTimer.OnRoundEnded += RoundEnded;
        }

        private void RoundEnded()
        {
            Time.timeScale = 0f;
            inGamePanel.SetActive(false);
            roundEndPanel.SetActive(true);
        }

        private void RoundStart()
        {
            playerPosition.position = startPosition;
            playerPosition.rotation = startRotation;
            playerRigidbody.velocity = Vector3.zero;
            Time.timeScale = 1f;
            inGamePanel.SetActive(true);
            roundEndPanel.SetActive(false);
        }
    }
}