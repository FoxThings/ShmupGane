using System;
using System.Collections.Generic;
using DG.Tweening;
using Interactions;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public EnemyEmitter Emitter;
    public HotText WaveInfo;
    public float WaveDelay = 3f;
    public List<WaveInfo> Waves;
    public NewDevicePanel deviceSelect;

    private int currentWave;

    private int enemiesCount;

    private int EnemiesCount
    {
        get => enemiesCount;
        set
        {
            enemiesCount = value;
            if (enemiesCount <= 0)
            {
                ManageWaves();
            }
        }
    }
    
    private void DoWave(int index)
    {
        enemiesCount = Waves[index].EnemiesCount;
        var sequence = DOTween.Sequence();
        sequence
            .AppendCallback(() =>
            {
                var list = Waves[index].Enemies;
                var enemy = Emitter.Emit(list[Random.Range(0, list.Count)]);
                enemy.GetComponent<Destroyable>().OnDestroyStart += () =>
                {
                    --EnemiesCount;
                };
            })
            .AppendInterval(Waves[index].Time / Waves[index].EnemiesCount)
            .SetLoops(Waves[index].EnemiesCount);
    }

    private void ManageWaves()
    {
        ++currentWave;
        if (currentWave == Waves.Count) return;
        
        
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => WaveInfo.ShowText("Wave " + (currentWave + 1), WaveDelay));
        sequence.AppendInterval(WaveDelay);
        sequence.AppendCallback(() =>
        {
            if(currentWave == 0)
            {
                DoWave(currentWave);
                return;
            }

            deviceSelect.ActivateWeaponChangeProcess(() => DoWave(currentWave), currentWave <= 2);
        });
    }

    private void Start()
    {
        currentWave = -1;
        ManageWaves();
    }
}

[Serializable]
public class WaveInfo
{
    public List<GameObject> Enemies;
    public float Time;
    public int EnemiesCount;
}