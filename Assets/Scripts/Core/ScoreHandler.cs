using System;
using Fusion;
using UnityEngine;

namespace Core
{
    public class ScoreHandler : NetworkBehaviour
    {
        private static ScoreHandler instance;
        public static ScoreHandler Instance => instance;
        public Action OnDriftStart;
        public Action<float> OnDriftEnd;
        public Action<float, float> OnDriftUpdate;
        public Action<float> OnTotalScoreUpdated;
        public float TotalScore { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void EndDrift(float totalScore)
        {
            Debug.Log("yab");
            OnTotalScoreUpdated?.Invoke(totalScore);
            TotalScore = totalScore;
        }
    }
}