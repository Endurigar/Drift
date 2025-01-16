using System;
using Core;
using Fusion;
using Multiplayer;
using Unity.VisualScripting;
using UnityEngine;

namespace Ui
{
    public class UiHandler : NetworkBehaviour
    {
        [SerializeField] private UiRoundTimer uiRoundTimer;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject roundEndPanel;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject pauseMenu;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            }
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