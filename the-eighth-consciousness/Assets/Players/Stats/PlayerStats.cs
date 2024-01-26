using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ScriptableObject
{
    [Header("Stats")]
    public int playerId;
    public int _maxHP = 100;
    public int _minHP = 0;
    public int _lives = 3;
    public int _bombs = 3;

    [Header("Fire")]
    public int _firePower = 0;
    public int _maxFirePower = 5;
    public int _minFirePower = 0;

    private int score;
    private bool isActive = false;
    private int currentHP;
    private int currentLives;
    private int currentBombs;
    private int currentFirePower;
    private string currentECDstatus;
    // 
    public bool IsActive => isActive;
    public int CurrentHP => currentHP;
    public int CurrentLives => currentLives;
    public int CurrentBombs => currentBombs;
    public int CurrentFirePower => currentFirePower;
    public int CurrentScore => score;
    public string CurrentECDstatus => currentECDstatus;

    public static event Action<int> OnPlayerDeath;
    public static event Action<int> OnGameOver;
    public static event Action<int> OnPlayerGUIChange;
    public static event Action OnPlayerHit;

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
        OnPlayerGUIChange?.Invoke(playerId);
        if (summand < 0)
        {
            OnPlayerHit?.Invoke();
        }
        if (currentHP == _minHP)
        {
            if (currentLives == 0)
            {
                isActive = false;
                OnPlayerDeath?.Invoke(playerId);
                OnGameOver?.Invoke(playerId);
                return;
            }
            --currentLives;
            OnPlayerDeath?.Invoke(playerId);
        }
    }

    public void UpdateECDStatus(string value)
    { 
        currentECDstatus = value;
        OnPlayerGUIChange?.Invoke(playerId);
    }

    public void UpdateIsActive(bool value)
    {
        isActive = false;
    }

    public void SetFullHP()
    {
        currentHP = _maxHP;
        OnPlayerGUIChange?.Invoke(playerId);
    }

    public void UpdateLives(int summand)
    {
        currentLives += summand;
    }

    public void UpdateBombs(int summand)
    {
        currentBombs += summand;
        OnPlayerGUIChange?.Invoke(playerId);
    }

    public void UpdateFirePower(int summand)
    {
        if (currentFirePower == _maxFirePower)
        {
            // Add points and return.
            return;
        }
        currentFirePower = Mathf.Clamp(currentFirePower + summand, _minFirePower, _maxFirePower);
    }

    public void UpdateScore(int summand)
    {
        score += summand;
    }
}
