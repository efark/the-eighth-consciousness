﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Straighter : AbstractEnemyController
{

    private TargetTypes targetType;
    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    private List<AttackPattern> attackPatterns = new List<AttackPattern>();
    private Vector2 lastDirection;
    private bool isAlive = true;
    private Camera cam;
    private Rect screenLimit;
    private bool canFire;

    // public TMP_Text statsText;
    private float time;
    public override int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp += value;
            UpdateGUI();
        }
    }

    private IEnumerator constantAttack(int index)
    {
        AttackPattern ap = attackPatterns[index];
        ap.UpdateNextFire(-Time.deltaTime);
        if (!canFire)
        {
            
        }
        if (ap.NextFire <= 0)
        {
            ap.UpdateNextFire(ap.cooldown);
            ap.UpdateIsRunning(true);

            yield return new WaitForSeconds(ap.burstOffset);
            for (int i = 0; i < ap.numberOfBursts; i++)
            {
                for (int j = 0; j < ap.burstSize; j++)
                {
                    if (j > 0)
                    {
                        yield return new WaitForSeconds(ap.burstSpacing);
                    }
                    players = GameObject.FindGameObjectsWithTag(targetType.ToString());
                    targetPlayer = GetClosestPlayer();
                    if (targetPlayer != null)
                    {
                        Vector3 targetDir = (targetPlayer.transform.position - this.transform.position).normalized;
                        Vector2 targetDirection = new Vector2(targetDir.x, targetDir.y);
                        if (ap.isOpposite)
                        {
                            targetDirection *= -1;
                        }
                        lastDirection = targetDirection;
                    }

                    ap.spread.Create(transform.position, transform.rotation, lastDirection);
                }
            }
            // yield return new WaitForSeconds(ap.cooldown);
            ap.UpdateIsRunning(false);
            attackPatterns[index] = ap;
        }
    }

    private void loopConstantAttacks()
    {
        for (int i = 0; i < attackPatterns.Count; i++)
        {
            StartCoroutine(constantAttack(i));
        }
    }

    void Start()
    {
        time = 0f;
        hp = 100;
        UpdateGUI();
        targetType = TargetTypes.Player;

        // https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
        cam = Camera.main;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));
        canFire = false;

        screenLimit = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);


        for (int i = 0; i < attackPatternsValues.Count; i++)
        {
            AttackPattern ap = attackPatternsValues[i];
            AttackPattern clone = Instantiate(ap);
            clone.Init(targetType);
            attackPatterns.Add(clone);
        }

    }

    void Update()
    {
        if (HP < 0)
        {
            Destroy(gameObject);
        }
        // Reached Vertical limit of screen.
        if (screenLimit.Contains(transform.position))
        {
            canFire = true;
        }
        else
        {
            canFire = false;
        }

        if (isAlive && canFire)
        {
            loopConstantAttacks();
            time += Time.fixedDeltaTime;
        }
    }

}
