using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerScript : MonoBehaviour
{
    public bool joined;
    public bool stunned;
    public float timeToBlock = 20.0f;
    public int damageAmount = 100;
    public KeyCode playersButton;

    public int playerScore = 0;

    public GameObject boss;

    private SpriteRenderer spriteRenderer;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        joined = false;
        stunned = false;
        timer = timeToBlock;
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

        if (Input.GetKeyDown(playersButton) && joined == true)
        {
            attack();
        }

        if(Input.GetKeyUp(playersButton) && joined == true)
        {
            timer = timeToBlock;
        }

        if (Input.GetKey(playersButton) && joined == true)
        {
            timer -= 1.0f;
            if (timer <= 0.0f)
            {
                block();
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
        boss.GetComponent<BossScript>().currentHealth -= damageAmount;
    }

    void block()
    {
        Debug.Log("player " + playersButton + " is blocking");
    }
}
