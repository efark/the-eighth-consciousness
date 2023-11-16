using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Stats")]
    public int playerId;
    public int _maxHP = 100;
    public int _minHP = 0;
    public int _lives = 3;
    public int _bombs = 3;

    [Header("Fire")]
    public int _firePower = 1;
    public int _maxFirePower = 5;
    public int _minFirePower = 1;

    private int score;
    private bool isActive = false;
    private int currentHP;
    private int currentLives;
    private int currentBombs;
    private int currentFirePower;

    // 
    public bool IsActive => isActive;
    public int CurrentHP => currentHP;
    public int CurrentLives => currentLives;
    public int CurrentBombs => currentBombs;
    public int CurrentFirePower => currentFirePower;
    public int CurrentScore => score;

    public static event Action<int> OnPlayerDeath;
    public static event Action<int> OnGameOver;

    public void Init()
    {
        isActive = true;
        currentHP = _maxHP;
        currentLives = _lives;
        currentBombs = _bombs;
        currentFirePower = _firePower;

    }
    public void UpdateHP(int summand)
    {
        currentHP = Mathf.Clamp(currentHP + summand, _minHP, _maxHP);
        if (currentHP == _minHP)
        {
            if (currentLives == 0)
            {
                isActive = false;
                OnGameOver?.Invoke(playerId);
                return;
            }
            --currentLives;
            OnPlayerDeath?.Invoke(playerId);
        }
    }

    public void UpdateLives(int summand)
    {
        currentLives += summand;
    }

    public void UpdateBombs(int summand)
    {
        currentLives += summand;
    }

    public void UpdateFirePower(int summand)
    {
        currentFirePower += summand;
    }

    public void UpdateScore(int summand)
    {
        score += summand;
    }
}
