using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public float maxHealth = 10000;
    public float currentHealth;
    public Slider healthBar;
    public Image fill;
    public Stances stance = Stances.Idle;
    public HPBarScript hpScript;
    public virtual void dealDamage(float _damage)
    {
        
        if (currentHealth > 0)
        {
            currentHealth -= _damage;
            hpScript.RemoveChunk(_damage);
            // set hit animation
            if (isCritical)
            {
                //bossAnimator.SetInteger("State", (int)AnimStates.Hit);
                bossAnimator.Play("Hit");
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().CameraShake();
            }
        }

    }

    public float idleDuration = 5.0f;
    public float attackDuration = 2.0f;
    public float downDuration = 5.0f;

    public GameObject TimeClick;

    protected float idleTimer;
    protected float attackTimer;
    protected float downTimer;

    public bool isCritical = false;

    // position ranges for the crit circle
    public float maxX = 0.25f;
    public float maxY = 0.25f;
    public float minX = -0.25f;
    public float minY = -0.25f;

    protected bool startingShakeHasPlayed = false;

    public AudioClip[] audioClips;
    protected AudioSource audioSource;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //stance = Stances.Idle;
        idleTimer = idleDuration;
        attackTimer = attackDuration;
        downTimer = downDuration;
        currentHealth = maxHealth;
        audioSource = gameObject.GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[0]);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Screen shake at the beginning of game
        if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Activate") && bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75 && !bossAnimator.IsInTransition(0) && !startingShakeHasPlayed)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().CameraShake();
            startingShakeHasPlayed = true;
        }

        fill.transform.localScale = new Vector3(Mathf.Max(0.0f,(float)currentHealth / (float)maxHealth), fill.transform.localScale.y, fill.transform.localScale.z);
        if (currentHealth > 0)
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
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClips[1]);
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

                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClips[5]);
                    }

                    break;
                default:
                    break;
            }
        }
        else
        {
            
            isCritical = false;
            bossAnimator.SetInteger("State", (int)AnimStates.Deactivate);
            gameObject.transform.DOLocalMoveY(-2, 3.33f, false);
            // Destroy self once the animation has finished playing
            if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Deactivate")&&bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !bossAnimator.IsInTransition(0))
            {
                Debug.Log("Finished animation");
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<LeaderboardScript>().IncBossLevel();
            }
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClips[6]);
                audioSource.PlayOneShot(audioClips[2]);
            }
            //Destroy(gameObject);
        }

    }
}
