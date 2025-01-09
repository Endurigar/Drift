using System;
using System.Linq;
using Core;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class UiAfterRound : NetworkBehaviour
    {
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
            scoreHandler.OnRoundEnd += ScoreHandlerOnOnRoundEnd;
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

        private void ScoreHandlerOnOnRoundEnd(NetworkDictionary<int, float> scores)
        {
            container.SetActive(true);
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (var score in scores.OrderByDescending(val => val.Value))
            {
                var newScore = Instantiate(scorePrefab, content);
                var formattedSCcore = score.Value.ToString("F0");
                newScore.text = $"Player{score.Key} - {formattedSCcore}";
            }

            coins = (int)Math.Round(scores[scoreHandler.Object.Runner.LocalPlayer.PlayerId] / 2f);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coins);
            coinsEarned.text = coins.ToString();
            playerCoins.text = PlayerPrefs.GetInt("Coins").ToString();
            totalScore.text = scores[scoreHandler.Object.Runner.LocalPlayer.PlayerId].ToString("F0");
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