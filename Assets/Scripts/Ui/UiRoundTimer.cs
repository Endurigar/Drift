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

        private void OnDestroy()
        {
            RestartHandler.OnRestart -= RestartTimer;
            BasicSpawner.Instance.OnRoundStarted -= StartRound;
        }

        private void StartRound()
        {
            roundText.gameObject.SetActive(true);
            startRoundTimer = 3.3f;
            isRoundTimerActive = true;
            StartCoroutine(HandleRoundCountdown());
        }

        private System.Collections.IEnumerator HandleRoundCountdown()
        {
            while (startRoundTimer > 0)
            {
                roundText.text = startRoundTimer.ToString("F0");
                yield return new WaitForSeconds(1f);
                startRoundTimer--;
            }

            roundText.gameObject.SetActive(false);
            isRoundTimerActive = false;
            isGameActive = true;
            StartCoroutine(HandleGameTimer());
        }

        private System.Collections.IEnumerator HandleGameTimer()
        {
            while (isGameActive && timeRemaining > 0 && Runner != null &&
                   (Runner.ActivePlayers.Count() == 2 || Runner.GameMode == GameMode.Single))
            {
                UpdateTimerText();
                yield return new WaitForSeconds(1f);
                timeRemaining--;
            }

            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void EndGame()
        {
            Debug.Log("timer");
            isGameActive = false;
            timerText.text = "Time's up!";
            OnRoundEnded?.Invoke();
        }

        private void RestartTimer()
        {
            StopAllCoroutines();
            timeRemaining = roundDuration;
            isGameActive = true;
            StartCoroutine(HandleGameTimer());
        }
    }
}
