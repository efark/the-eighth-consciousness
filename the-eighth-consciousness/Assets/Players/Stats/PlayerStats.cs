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
    private bool isAlive;
    private int currentHP;
    private int currentLives;
    private int currentBombs;
    private int currentFirePower;
    // 
    public bool IsActive => isActive;
    public bool IsAlive => isAlive;
    public int CurrentHP => currentHP;
    public int CurrentLives => currentLives;
    public int CurrentBombs => currentBombs;
    public int CurrentFirePower => currentFirePower;
    public int CurrentScore => score;

    private float iFrameDuration;
    public float IFrameDuration => iFrameDuration;
    private bool iFrameActive;
    public bool IFrameActive => iFrameActive;
    public void UpdateIFrameActive(bool value)
    {
        iFrameActive = value;
    }
    public void UpdateIsAlive(bool value)
    { 
        isAlive = value;
    }

    public static event Action<int> OnPlayerDeath;
    public static event Action<int> OnGameOver;
    public static event Action<int> OnPlayerGUIChange;
    public static event Action<int> OnPlayerHit;
    public static event Action<int> OnPlayerHealed;

    public void Init()
    {
        iFrameDuration = 0.5f;
        iFrameActive = false;
        isActive = true;
        currentHP = _maxHP;
        currentLives = _lives;
        currentBombs = _bombs;
        currentFirePower = _firePower;
        score = 0;
    }

    public void UpdateHP(int summand)
    {
        summand = iFrameActive ? 0 : summand;
        if (!isAlive || summand == 0)
        {
            return;
        }

        // Calculate new HP value.
        currentHP = Mathf.Clamp(currentHP + summand, _minHP, _maxHP);
        OnPlayerGUIChange?.Invoke(playerId);

        // Player gets killed.
        if (currentHP == _minHP)
        {
            isAlive = false;
            if (currentLives == 0)
            {
                isActive = false;
                OnGameOver?.Invoke(playerId);
                return;
            }
            OnPlayerDeath?.Invoke(playerId);
            OnPlayerGUIChange?.Invoke(playerId);
            return;
        }

        // Player gets healed.
        if (summand > 0)
        {
            // Call OnPlayerHealed.
            OnPlayerHealed?.Invoke(playerId);
            OnPlayerGUIChange?.Invoke(playerId);
            return;
        }
        // Player gets hit.
        if (summand < 0)
        {
            OnPlayerHit?.Invoke(playerId);
            OnPlayerGUIChange?.Invoke(playerId);
            ActivateIFrame();
            return;
        }

    }

    public void ActivateIFrame()
    {
        iFrameActive = true;
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
        OnPlayerGUIChange?.Invoke(playerId);
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
        OnPlayerGUIChange?.Invoke(playerId);
    }
}
