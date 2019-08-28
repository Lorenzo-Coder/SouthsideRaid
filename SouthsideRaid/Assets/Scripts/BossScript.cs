using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stances
{
    Idle,
    Attack,
    Down,
}

public class BossScript : MonoBehaviour
{
    public int health = 10000;
    public Stances stance = Stances.Idle;
    public void dealDamage(int _damage)
    {
        health -= _damage;
    }

    public float idleDuration = 5.0f;
    public float attackDuration = 2.0f;
    public float downDuration = 5.0f;

    public GameObject outline;
    public GameObject centre;

    private float idleTimer;
    private float attackTimer;
    private float downTimer;

    // Start is called before the first frame update
    void Start()
    {
        //stance = Stances.Idle;
        idleTimer = idleDuration;
        attackTimer = attackDuration;
        downTimer = downDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (health >= 0)
        {
            switch (stance)
            {
                case Stances.Idle:
                    //Debug.Log("Idle");
                    idleTimer -= Time.deltaTime;
                    if (idleTimer <= 0.0f)
                    {
                        stance = Stances.Attack;
                        idleTimer = idleDuration;
                    }
                    break;
                case Stances.Attack:
                   // Debug.Log("Attack");
                    attackTimer -= Time.deltaTime;


                    if (attackTimer <= 0.0f)
                    {
                        stance = Stances.Down;
                        attackTimer = attackDuration;
                    }
                    break;
                case Stances.Down:
                    //Debug.Log("Down");
                    downTimer -= Time.deltaTime;
                    if (downTimer <= 0.0f)
                    {
                        stance = Stances.Idle;
                        downTimer = downDuration;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Victory!");
        }
 
    }
}
