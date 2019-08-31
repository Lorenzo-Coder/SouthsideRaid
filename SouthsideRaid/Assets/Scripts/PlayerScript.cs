using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public string playerName;
    public int playerScore = 0;

    public GameObject boss;
    public GameObject DPSPopup;

    private SpriteRenderer spriteRenderer;
    private float blockTimer;
    private float stunTimer;
    private int damageMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        joined = false;
        blocking = false;
        stunned = false;
        blockTimer = timeToBlock;
        stunTimer = stunTime;
        damageMultiplier = damageMultiplierAmount;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(playersButton) && joined == false)
        {
            JoinGame();
        }

        if (stunned == false)
        {
            if (Input.GetKeyDown(playersButton) && joined == true)
            {
                attack();
            }
        }

        if(Input.GetKeyUp(playersButton) && joined == true)
        {
            blockTimer = timeToBlock;
            blocking = false;
        }

        if (Input.GetKey(playersButton) && joined == true)
        {
            blockTimer -= 1.0f;
            if (blockTimer <= 0.0f)
            {
                block();
            }
        }

        if ((boss.GetComponent<BossScript>().stance == Stances.Attack) && (blocking == false))
        {
            stunned = true;
        }

        if (stunned == true)
        {
            stunTimer -= 1.0f;
            if (stunTimer <= 0.0f)
            {
                stunned = false;
            }
        }
    }

    void JoinGame()
    {
        spriteRenderer.enabled = true;
        joined = true;
    }

    void attack()
    {
        Debug.Log("player " + playersButton + " is attacking");

        if(boss.GetComponent<BossScript>().stance == Stances.Down)
        {
            boss.GetComponent<BossScript>().currentHealth -= damageAmount + damageMultiplier;
            Debug.Log("player " + playersButton + " is attacking with " + (damageAmount + damageMultiplier));
            damageMultiplier = damageMultiplierAmount * 2;
            playerScore = playerScore + damageAmount + damageMultiplier;
        }
        else
        {
            boss.GetComponent<BossScript>().currentHealth -= damageAmount;
            damageMultiplier = damageMultiplierAmount;
            playerScore = playerScore + damageAmount;

            //Transform DPSTransform = Instantiate(DPSPopup, Can);
            //DPSScript DPSPopupScript = DPSTransform.GetComponent<DPSScript>();
            //DPSPopupScript.DPS(damageAmount);
        }

    }

    void block()
    {
        Debug.Log("player " + playersButton + " is blocking");
        blocking = true;
    }
}
