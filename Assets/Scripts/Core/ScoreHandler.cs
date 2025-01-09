using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreHandler : NetworkBehaviour
{
    private static ScoreHandler _instance;
    public event Action<NetworkDictionary<int, float>> OnRoundEnd;
    public static ScoreHandler Instance => _instance;
    [Networked] private NetworkDictionary<int, float> scores { get; } = new NetworkDictionary<int, float>();
    public  Action OnDriftStart;
    public  Action<float> OnDriftEnd;
    public  Action<float,float> OnDriftUpdate;

    private void Awake()
    {
        _instance = this;
        RestartHandler.OnRestart += () => scores.Clear();
    }

    public override void Spawned()
    {
        base.Spawned();
       
    }

    public void EndDrift(float totalScore, int id)
    {
        if (Runner.IsServer)
        {
            scores.Add(id, totalScore);
        }

        if (scores.Count == Runner.ActivePlayers.Count()) RPC_CheckScores();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CheckScores()
    {
        OnRoundEnd?.Invoke(scores);
    }
}