using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractEnemyController : MonoBehaviour
{
    protected int hp;
    protected GameObject[] players = new GameObject[2];
    protected GameObject targetPlayer;
    public TMP_Text statsText;
    public abstract int HP
    {
        get;
        set;
    }

    protected GameObject GetClosestPlayer()
    {
        if (players.Length == 1)
        {
            return players[0];
        }
        float min_distance = 0;
        GameObject closest = null;
        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(p.transform.position, gameObject.transform.position);
            if (min_distance == 0)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
            if (dist < min_distance)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
        }
        return closest;
    }

    public void UpdateGUI()
    {
        if (statsText == null)
        {
            return;
        }
        if (hp <= 0)
        {
            statsText.text = "";
            Destroy(gameObject);
        }
        statsText.text = $"Enemy HP: {hp}";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
