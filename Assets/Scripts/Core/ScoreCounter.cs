using System;
using Core;
using Fusion;
using Multiplayer;
using Ui;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CarController))]
    public class ScoreCounter: NetworkBehaviour
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

         private void FixedUpdate()
            {
                if (isDrifting)
                {
                    UpdateDrift();
                }
            }
        
            private void DriftStart()
            {
                if (!isDrifting)
                {
                    ScoreHandler.Instance.OnDriftStart?.Invoke();
                    isDrifting = true;
                    driftStartTime = Time.time;
                    lastDriftUpdateTime = Time.time;
                }
                else
                {
                    lastDriftUpdateTime = Time.time;
                }
            }
            
        
            private void UpdateDrift()
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
                    ScoreHandler.Instance.OnDriftUpdate?.Invoke(driftDuration,currentScore);
                }
            }
        
            private void EndDrift(float driftDuration)
            {
                isDrifting = false;
                float finalScore = coefficient * driftDuration;
                totalScore = TotalScore + finalScore;
                ScoreHandler.Instance.OnDriftEnd?.Invoke(TotalScore);
             
            }

            private void End()
            {
                if(!isActive) return;
                isActive = false;
                //Debug.LogError("END_DRIFT");

                RPC_EndDrift(totalScore, Object.InputAuthority.PlayerId);
            }
            private void RestartScore()
            {
                totalScore = 0;
                currentScore = 0;
            }
            [Rpc(RpcSources.All, RpcTargets.All)]
            public void RPC_EndDrift(float totalScore, int id)
            {
                ScoreHandler.Instance.EndDrift(totalScore, id);
               
               // Debug.LogError("RPc_END_DRIFT");
             
            }
    }
}