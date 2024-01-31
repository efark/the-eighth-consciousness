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

    private float iFrameDuration;
    public float IFrameDuration => iFrameDuration;
    private bool iFrameActive;
    public bool IFrameActive => iFrameActive;
    public void UpdateIFrameActive(bool value)
    {
        iFrameActive = value;
    }

    public static event Action<int> OnPlayerDeath;
    public static event Action<int> OnGameOver;
    public static event Action<int> OnPlayerGUIChange;
    public static event Action<int> OnPlayerHit;

    public void Init()
    {
        iFrameDuration = 0.5f;
        iFrameActive = false;
        isActive = true;
        currentHP = _maxHP;
        currentLives = _lives;
        currentBombs = _bombs;
        currentFirePower = _firePower;
    }

    public void UpdateHP(int summand)
    {
        if (currentHP < _minHP)
        {
            return;
        }
        if (summand < 0)
        {
            OnPlayerHit?.Invoke(playerId);
            if (iFrameActive)
            {
                return;
            }
            ActivateIFrame();
        }
        currentHP = Mathf.Clamp(currentHP + summand, _minHP -1, _maxHP);
        OnPlayerGUIChange?.Invoke(playerId);

        if (currentHP <= _minHP)
        {
            Debug.Log($"Lives: {currentLives}");
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

    public void ActivateIFrame()
    {
        iFrameActive = true;
    }

    public void UpdateECDStatus(string value)
    { 
        currentECDstatus = value;
        OnPlayerGUIChange?.Invoke(playerId);
    }

    public void UpdateIsActive(bool value)
    {
        isActive = value;
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
