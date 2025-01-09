using System;
using System.Linq;
using Core;
using Fusion;
using Multiplayer;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiRoundTimer : NetworkBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text roundText;
        [SerializeField] private float roundDuration;
        private static UiRoundTimer instance;
        [Networked] private float timeRemaining { get; set; }
        [Networked] private float startRoundTimer { get; set; }
        private bool isGameActive = false;
        private bool isRoundTimerActive = false;
        public event Action OnRoundEnded;

        public static UiRoundTimer Instance => instance;

        private void Awake()
        {
            instance = this;
        }

        public override void Spawned()
        {
            base.Spawned();
            timeRemaining = roundDuration;
            RestartHandler.OnRestart += RestartTimer;
            BasicSpawner.Instance.OnRoundStarted += StartRound;
        }

        private void StartRound()
        {
            roundText.gameObject.SetActive(true);
            startRoundTimer = 3.3f;
            isRoundTimerActive = true;
        }

        void Update()
        {
            if (isRoundTimerActive)
            {
                if (startRoundTimer > 0)
                {
                    startRoundTimer -= Time.deltaTime;
                    roundText.text = startRoundTimer.ToString("F0");
                }
                else
                {
                    isGameActive = true;

                    roundText.gameObject.SetActive(false);
                }
            }

            if (isGameActive && Runner != null && Runner.ActivePlayers.Count() == 2)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    UpdateTimerText();
                }
                else
                {
                    EndGame();
                }
            }
        }

        void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        void EndGame()
        {
            isGameActive = false;
            timerText.text = "Time's up!";
            OnRoundEnded?.Invoke();
        }

        private void RestartTimer()
        {
            timeRemaining = roundDuration;
            isGameActive = true;
        }
    }
}