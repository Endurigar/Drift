using Core;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class ScoreUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text coefficientText;
        [SerializeField] private TMP_Text totalScoreText;
        [SerializeField] private ScoreHandler scoreHandler;
        [SerializeField] private GameObject scoreContainer;

        private void Start()
        {
            RestartHandler.OnRestart += ResetUI;
            scoreHandler.OnDriftStart += DriftStartHandle;
            scoreHandler.OnDriftUpdate += DriftUpdateHandle;
            scoreHandler.OnDriftEnd += DriftEndHandle;
        }

        private void DriftEndHandle(float value)
        {
            totalScoreText.text = value.ToString("F0");
            scoreContainer.SetActive(false);
        }

        private void DriftUpdateHandle(float coefficient, float score)
        {
            coefficientText.text = coefficient.ToString("F1");
            scoreText.text = score.ToString("F0");
        }

        private void DriftStartHandle()
        {
            scoreContainer.SetActive(true);
        }

        private void ResetUI()
        {
            scoreContainer.SetActive(false);
            totalScoreText.text = "0";
        }
    }
}


