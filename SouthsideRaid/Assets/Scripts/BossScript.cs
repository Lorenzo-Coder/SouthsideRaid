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

public enum BossType
{
    LongArm,
    Ape,
}

public enum AnimStates
{
    Idle = 0,
    Attack = 1,
    Opening = 2,
    Hit = 3,
    Berzerk = 4,
    Deactivate = 8,
}

public class BossScript : MonoBehaviour
{
    public Animator bossAnimator;
    public BossType bossType = BossType.LongArm;
    public int maxHealth = 10000;
    public int currentHealth;
    public Slider healthBar;
    public Image fill;
    public Stances stance = Stances.Idle;
    public HPBarScript hpScript;
    public void dealDamage(int _damage)
    {
        
        if (currentHealth > 0)
        {
            currentHealth -= _damage;
            hpScript.RemoveChunk(_damage);
            // set hit animation
            if (isCritical)
            {
                bossAnimator.SetInteger("State", (int)AnimStates.Hit);
            }
        }

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
        //healthBar.value = (float)currentHealth / (float)maxHealth;
        fill.transform.localScale = new Vector3(Mathf.Max(0.0f,(float)currentHealth / (float)maxHealth), fill.transform.localScale.y, fill.transform.localScale.z);
        if (currentHealth >= 0)
        {
            

            switch (stance)
            {
                case Stances.Idle:
                    bossAnimator.SetInteger("State", (int)AnimStates.Idle);
                    idleTimer -= Time.deltaTime;
                    if (idleTimer <= 0.0f)
                    {
                        stance = Stances.Attack;
                        idleTimer = idleDuration;
                        bossAnimator.SetInteger("State", (int)AnimStates.Attack);

                    }
                    break;
                case Stances.Attack:
                   // Debug.Log("Attack");
                    attackTimer -= Time.deltaTime;
                    bossAnimator.SetInteger("State", (int)AnimStates.Attack);

                    if (attackTimer <= 0.0f)
                    {
                        stance = Stances.Down;
                        attackTimer = attackDuration;
                        bossAnimator.SetInteger("State", (int)AnimStates.Opening);
                    }
                    break;
                case Stances.Down:
                    bossAnimator.SetInteger("State", (int)AnimStates.Opening);
                    downTimer -= Time.deltaTime;
                    if (downTimer <= 0.0f && (GameObject.FindGameObjectsWithTag("TimeClick").Length == 0))
                    {
                        bossAnimator.SetInteger("State", (int)AnimStates.Idle);
                        stance = Stances.Idle;
                        isCritical = false;
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
            bossAnimator.SetInteger("State", (int)AnimStates.Deactivate);
            //Destroy(gameObject);
        }

    }
}
