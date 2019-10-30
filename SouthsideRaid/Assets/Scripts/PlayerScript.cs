using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum PlayerAnimState
{
    Idle,
    Attack,
    Block,
    HitStun,
    VictoryJump,
    Charging,
    Switch,
}

public enum PlayerLaneState
{
    Left,
    Middle, 
    Right
}

public class PlayerScript : MonoBehaviour
{
    public bool joined;
    public bool blocking;
    public bool stunned;
    public float timeToBlock = 20.0f;
    public float stunTime = 120.0f;
    public int damageMultiplierAmount = 25;
    public int damageAmount = 100;
    public KeyCode playersButton;
    public PlayerLaneState playersLane = PlayerLaneState.Middle;

    public string playerName;
    public int playerScore = 0;

    public Animator playerAnimator;
    public GameObject boss;
    public Canvas canvas;
    public GameObject DPSPopup;
    public GameObject VFX;
    public GameObject[] LaneCentres;

    public AudioClip[] audioClips;

    private GameObject modelAnimationThing;
    private float blockTimer;
    private float stunTimer;
    private int damageMultiplier;
    private int timesAttacked = 0;
    private bool foundBoss;
    [SerializeField] private PlayerAnimState CurrentAnimState;
    private bool finishedSpawning = true;
    private AudioSource audioSource;
    private bool laneDirection = true; //true is left, false is right

    public int superMeter = 0;
    public float timeToCharge = 0.2f;
    public float timeCharged = 0.0f;
    public SpriteRenderer superBG;
    public SpriteRenderer superFill;

    //public SkinnedMeshRenderer skinnedMeshRenderer;
    //private Vector3 myMaxBoundsCenter = Vector3.zero;
    //private Vector3 myMaxBoundsSize = new Vector3(999999.0f, 999999.0f, 999999.0f);

    // Start is called before the first frame update
    void Start()
    {
        joined = false;
        blocking = false;
        stunned = false;
        foundBoss = false;
        blockTimer = timeToBlock;
        stunTimer = stunTime;
        damageMultiplier = damageMultiplierAmount;
        modelAnimationThing = gameObject.transform.GetChild(0).gameObject;
        modelAnimationThing.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>();

        //// setting the new mesh bounds
        //// Get the SkinnedMeshRenderer component
        //skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();

        //// Create and set your new bounds
        //Bounds newBounds = new Bounds(myMaxBoundsCenter, myMaxBoundsSize);
        //skinnedMeshRenderer.localBounds = newBounds;

        UpdateSuperMeter();
    }

    // Update is called once per frame
    void Update()
    {
        //CurrentAnimState.Equals(playerAnimator.GetInteger("CurrentAnim"));

        if (foundBoss == false)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss != null)
            {
                foundBoss = true;
                animIdle();
            }
        }

        //// bandaid fix for a rendering bug. 
        //// Set the each player to be on ~-9.5 on the y axis so that Unity will render it and the animation "looks" right.
        //if ((playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SpawnPlayer") &&
        //    playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4 &&
        //    !playerAnimator.IsInTransition(0)))
        ////||(!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SpawnPlayer")))
        //{
        //    transform.position = new Vector3(transform.position.x, -3.5f, transform.position.z);
        //    playerAnimator.Play("Idle");
        //    finishedSpawning = true;
        //}

        if (boss == null)
        {
            foundBoss = false;
        }

        if (Input.GetKeyDown(playersButton) && joined == false)
        {
            JoinGame();
        }

        if (finishedSpawning)
        {
            if (stunned == false)
            {
                if (Input.GetKeyDown(playersButton) && joined == true)
                {
                    if (timesAttacked >= 1)
                    {
                        attack();
                    }
                    timesAttacked++;
                }
                else
                {
                    animIdle();
                }
            }

            if (Input.GetKeyUp(playersButton) && joined == true)
            {
                //blockTimer = timeToBlock;
                //blocking = false;
                if (timeCharged >= 1.0f)
                {
                    SuperMove();
                }
                timeCharged = 0.0f;

                animIdle();
            }

            if (Input.GetKey(playersButton) && joined == true)
            {
                //blockTimer -= 1.0f;
                //if (blockTimer <= 0.0f)
                //{
                //    block();
                //}

                // start charging SuperMove
                if (superMeter == 100)
                {
                    timeCharged += Time.deltaTime;
                    animCharge();
                }
            }

            if ((boss.GetComponent<BossScript>().stance == Stances.Attack) && (blocking == false))
            {
                animHitStun();
                stunned = true;
            }

            if (stunned == true)
            {
                stunTimer -= 1.0f;
                if (stunTimer <= 0.0f)
                {
                    animIdle();
                    stunned = false;
                    stunTimer = stunTime;
                }
            }

            if (boss.GetComponent<BossScript>().currentHealth <= 0)
            {
                animVictory();
            }

            //if (CurrentAnimState == PlayerAnimState.VictoryJump && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            //{
            //    animIdle();
            //    Debug.Log("It called it!");
            //}

            if ((boss.GetComponent<BossScript>().stance != Stances.Down))
            {
                damageMultiplier = damageMultiplierAmount;
            }
        }
    }

    void JoinGame()
    {
        modelAnimationThing.SetActive(true);
        joined = true;
        transform.DOLocalMoveY(-3.86f, 0.2f, false);
        animSwitch();
    }

    void attack()
    {
        Debug.Log("player " + playersButton + " is attacking");
        animAttack();

        if ((boss.GetComponent<BossScript>().stance == Stances.Down) && (boss.GetComponent<BossScript>().isCritical == true))
        {
            boss.GetComponent<BossScript>().dealDamage(damageAmount + damageMultiplier);
            Debug.Log("player " + playersButton + " is attacking with " + (damageAmount + damageMultiplier));

            PopUp(true);
            VFX.GetComponent<VFXScript>().SpawnCritFX();

            audioSource.PlayOneShot(audioClips[1]);

            playerScore = playerScore + damageAmount + damageMultiplier;
            damageMultiplier = damageMultiplier * 2;

            // increase superMeter by 10
            GainMeter(10);

        }
        else if ((boss.GetComponent<BossScript>().stance == Stances.Down) && (boss.GetComponent<BossScript>().isCritical == false))
        {
            damageMultiplier = damageMultiplierAmount;
            GainMeter();
        }
        else
        {
            boss.GetComponent<BossScript>().dealDamage(damageAmount);

            PopUp(false);
            VFX.GetComponent<VFXScript>().SpawnPunchFX();

            audioSource.PlayOneShot(audioClips[0]);

            playerScore = playerScore + damageAmount;
            damageMultiplier = damageMultiplierAmount;
            GainMeter();

        }
    }

    void block()
    {
        Debug.Log("player " + playersButton + " is blocking");
        animBlock();
        blocking = true;
    }

    void PopUp(bool withM)
    {
        if (withM == false)
        {
            GameObject popUpObject = Instantiate(DPSPopup, Vector3.zero, Quaternion.identity);
            popUpObject.transform.SetParent(canvas.transform, false);
            float xPopUpPos = Random.Range(-85.0f, 85.0f);
            popUpObject.transform.position = new Vector3(canvas.transform.position.x + xPopUpPos, canvas.transform.position.y + 100, canvas.transform.position.z);
            popUpObject.GetComponent<DPSScript>().DPS(damageAmount);
            StartCoroutine(animateDPS(popUpObject));
        }
        else
        {
            GameObject popUpObject = Instantiate(DPSPopup, Vector3.zero, Quaternion.identity);
            popUpObject.transform.SetParent(canvas.transform, false);
            float xPopUpPos = Random.Range(-85.0f, 85.0f);
            popUpObject.transform.position = new Vector3(canvas.transform.position.x + xPopUpPos, canvas.transform.position.y + 100, canvas.transform.position.z);
            popUpObject.GetComponent<DPSScript>().DPS(damageAmount + damageMultiplier);
            StartCoroutine(animateDPS(popUpObject));
        }
    }

    IEnumerator animateDPS(GameObject _object)
    {
        _object.transform.DOMoveY(_object.transform.position.y + 100.0f, 1.0f);
        _object.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 1.0f);

        StartCoroutine(waitToDestroy(_object));

        yield return null;
    }

    IEnumerator waitToDestroy(GameObject _object)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(_object);
    }

    void animAttack()
    {
        CurrentAnimState = PlayerAnimState.Attack;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
        playerAnimator.Play("Attack");
    }

    void animIdle()
    {
        CurrentAnimState = PlayerAnimState.Idle;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
    }

    void animHitStun()
    {
        CurrentAnimState = PlayerAnimState.HitStun;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
    }

    void animBlock()
    {
        CurrentAnimState = PlayerAnimState.Block;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
    }

    void animVictory()
    {
        CurrentAnimState = PlayerAnimState.VictoryJump;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
    }

    void animCharge()
    {
        CurrentAnimState = PlayerAnimState.Charging;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
    }

    void animSwitch()
    {
        CurrentAnimState = PlayerAnimState.Switch;
        playerAnimator.SetInteger("CurrentAnim", (int)CurrentAnimState);
    }

    void GainMeter()
    {
        superMeter++;
        superMeter = Mathf.Clamp(superMeter, 0, 100);
        Debug.Log("GainMeter++ ");
        UpdateSuperMeter();
    }

    void GainMeter(int _i)
    {
        superMeter += _i;
        superMeter = Mathf.Clamp(superMeter, 0, 100);
        Debug.Log("GainMeter + " + _i);
        UpdateSuperMeter();
    }

    void SuperMove()
    {
        BossScript bossScript = boss.GetComponent<BossScript>();
        timeCharged = Mathf.Clamp(timeCharged, 1.0f, 3.0f);
        float superDamage = bossScript.maxHealth * 0.25f * (timeCharged - 1.0f) / 2.0f;


        // deal up to 25% of the bosses maximum hp as damage depending on how long the player charged for
        //bossScript.dealDamage(bossScript.maxHealth * 0.25f * (timeCharged - 1.0f) / 2.0f);

        StartCoroutine(DamageDelay(0.6f, superDamage));

        // deplete the superMeter
        superMeter = 0;
        UpdateSuperMeter();
    }

    void UpdateSuperMeter()
    {
        // fill the meter
        superFill.transform.localScale = new Vector3(superBG.transform.localScale.x * superMeter / 100.0f, superFill.transform.localScale.y, superFill.transform.localScale.z);

        // if full change color to green
        if (superMeter == 100)
        {
            superFill.color = new Color(0, 255, 0);
        }
        // else its red
        else
            superFill.color = new Color(255, 0, 0);

    }

    void GetHit()
    {
        // lose 10 points of mana
        GainMeter(-10);
    }

    void SwitchLanes()
    {
        switch (playersLane)
        {
            case PlayerLaneState.Left:
                playersLane = PlayerLaneState.Middle;
                laneDirection = false;
                //move player to other position
                //make player invicible for abit
                break;
            case PlayerLaneState.Middle:
                if (laneDirection)
                {
                    playersLane = PlayerLaneState.Left;
                }
                else
                { 
                    playersLane = PlayerLaneState.Right;
                }
                //make player invicible for abit
                //move player to other position
                break;
            case PlayerLaneState.Right:
                playersLane = PlayerLaneState.Middle;
                laneDirection = true;
                //make player invicible for abit
                //move player to other position
                break;
        }
    }

    IEnumerator movePlayerLeft()
    {
        //gameObject.transform.DOMoveX()
        yield return null;
    }

    IEnumerator movePlayerMiddle()
    {

        yield return null;
    }
    IEnumerator movePlayerRight()
    {

        yield return null;
    }

    IEnumerator DamageDelay( float _delay, float _damage)
    {
        yield return new WaitForSeconds(_delay);
        boss.GetComponent<BossScript>().dealDamage(_damage);
        //StopCoroutine(DamageDelay);
    }
}
