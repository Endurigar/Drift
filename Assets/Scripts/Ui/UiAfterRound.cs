using System;
using Core;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class UiAfterRound : NetworkBehaviour
    {
        [SerializeField] private UiRoundTimer roundTimer;
        [SerializeField] private ScoreHandler scoreHandler;
        [SerializeField] private TMP_Text scorePrefab;
        [SerializeField] private GameObject container;
        [SerializeField] private Transform content;
        [SerializeField] private TMP_Text totalScore;
        [SerializeField] private TMP_Text coinsEarned;
        [SerializeField] private TMP_Text playerCoins;
        [SerializeField] private Button restartButton;
        private int coins;

        private void Awake()
        {
            restartButton.onClick.AddListener(Restart);
            //roundTimer.OnRoundEnded += ScoreHandlerOnOnRoundEnd;
            scoreHandler.OnTotalScoreUpdated += ScoreHandlerOnRoundEnd;
            RestartHandler.OnRestart += () => container.SetActive(false);
        }

        public override void Spawned()
        {
            base.Spawned();
            if (!Object.HasStateAuthority)
            {
                restartButton.gameObject.SetActive(false);
            }
        }

        private void ScoreHandlerOnRoundEnd(float score)
        {
            Debug.Log(score);
            container.SetActive(true);
            coins = (int)Math.Round(score / 2f);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coins);
            coinsEarned.text = coins.ToString();
            playerCoins.text = PlayerPrefs.GetInt("Coins").ToString();
            totalScore.text = score.ToString("F0");
        }

        private void Restart()
        {
            container.SetActive(false);
            RPC_Restart();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_Restart()
        {
            RestartHandler.Restart();
        }
    }
}