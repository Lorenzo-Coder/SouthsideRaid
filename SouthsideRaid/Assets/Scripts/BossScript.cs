using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stances
{
    Idle,
    Attack,
    Down,
}

public class BossScript : MonoBehaviour
{
    public int maxHealth = 10000;
    public int currentHealth;
    public Slider healthBar;
    public Stances stance = Stances.Idle;
    public void dealDamage(int _damage)
    {
        currentHealth -= _damage;
    }

    public float idleDuration = 5.0f;
    public float attackDuration = 2.0f;
    public float downDuration = 5.0f;

    public GameObject TimeClick;

    private float idleTimer;
    private float attackTimer;
    private float downTimer;

    public bool isCritical = false;
    public float maxX = 0.25f;
    public float maxY = 0.25f;
    public float minX = -0.25f;
    public float minY = -0.25f;

    // Start is called before the first frame update
    void Start()
    {
        //stance = Stances.Idle;
        idleTimer = idleDuration;
        attackTimer = attackDuration;
        downTimer = downDuration;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = (float)currentHealth / (float)maxHealth;
        if (currentHealth >= 0)
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
                    if (downTimer <= 0.0f && (GameObject.FindGameObjectsWithTag("TimeClick").Length == 0))
                    {
                        stance = Stances.Idle;
                        downTimer = downDuration;
                    }
                    else if((GameObject.FindGameObjectsWithTag("TimeClick").Length == 0))
                    {
                        float posX = Random.Range(minX, maxX);
                        float posY = Random.Range(minY, maxY);
                        Instantiate(TimeClick, new Vector3(posX,posY,0.0f), Quaternion.identity);
                    }

                    if (GameObject.FindGameObjectWithTag("TimeClick")!= null)
                    {
                        isCritical = GameObject.FindGameObjectWithTag("TimeClick").GetComponent<TimeClickScript>().canHit;
                    }

                    break;
                default:
                    break;
            }
        }
        else
        {
            // Destroy self
            Debug.Log("Victory!");
            Destroy(gameObject);
        }
 
    }
}
