using System.Collections;
using Fusion;
using Multiplayer;
using Ui;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CarController))]
    public class ScoreCounter : NetworkBehaviour
    {
        private CarController carController;
        [SerializeField] private float coefficient;
        [SerializeField] private float driftDelay = 0.5f;

        private bool isDrifting;
        private float driftStartTime;
        private float lastDriftUpdateTime;
        private float totalScore;
        private float currentScore;
        public float TotalScore => totalScore;

        private bool isActive;
        private Coroutine driftCoroutine;

        private void Start()
        {
            carController = GetComponent<CarController>();

            carController.OnDrifting += DriftStart;
            UiRoundTimer.Instance.OnRoundEnded += End;
            BasicSpawner.Instance.OnRoundStarted += () => isActive = true;

            RestartHandler.OnRestart += () =>
            {
                RestartScore();
                isActive = true;
            };
        }

        private void DriftStart()
        {
            if (!isDrifting)
            {
                isDrifting = true;
                driftStartTime = Time.time;
                lastDriftUpdateTime = Time.time;
                ScoreHandler.Instance.OnDriftStart?.Invoke();

                if (driftCoroutine != null)
                {
                    StopCoroutine(driftCoroutine);
                }

                driftCoroutine = StartCoroutine(HandleDrift());
            }
            else
            {
                lastDriftUpdateTime = Time.time;
            }
        }

        private IEnumerator HandleDrift()
        {
            while (isDrifting)
            {
                float currentTime = Time.time;
                float driftDuration = currentTime - driftStartTime;

                if (currentTime - lastDriftUpdateTime >= driftDelay)
                {
                    EndDrift(driftDuration);
                }
                else
                {
                    currentScore = coefficient * driftDuration;
                    ScoreHandler.Instance.OnDriftUpdate?.Invoke(driftDuration, currentScore);
                }

                yield return null;
            }
        }

        private void EndDrift(float driftDuration)
        {
            isDrifting = false;

            float finalScore = coefficient * driftDuration;
            totalScore += finalScore;

            ScoreHandler.Instance.OnDriftEnd?.Invoke(totalScore);

            if (driftCoroutine != null)
            {
                StopCoroutine(driftCoroutine);
                driftCoroutine = null;
            }
        }

        private void End()
        {
            isActive = false;
            ScoreHandler.Instance.EndDrift(totalScore);
        }

        private void RestartScore()
        {
            totalScore = 0;
            currentScore = 0;
        }
    }
}
