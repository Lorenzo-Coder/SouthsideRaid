using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackPrefabScript : MonoBehaviour
{
    public float timeToDamage = 0.5f;
    public float blinkRate = 1.0f;
    public int howManyBlink = 10;
    public PlayerLaneState atkLane;

    private GameObject boss;
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private bool visible = true;
    private bool countdownEnded = false;
    private bool complete = false;

    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        boss = GameObject.FindGameObjectWithTag("Boss");
        players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(blink());
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownEnded == false)
        {
            timeToDamage -= Time.deltaTime;
            //Debug.Log(timeToDamage);
            if (timeToDamage <= 0.0f)
            {
                countdownEnded = true;
                attack();
            }
        }
        
        if (complete == true)
        {
            StartCoroutine(waitToDestroy());
        }
    }

    void attack()
    {
        for (int i = 0; i < players.Length; i++)
        {
            //Debug.Log(i);
            if (players[i].GetComponent<PlayerScript>().playersLane == atkLane && players[i].GetComponent<PlayerScript>().invincible == false)
            {
                Debug.Log("attacked player");
                players[i].GetComponent<PlayerScript>().GetHit();
            }
            else
            {
                Debug.Log("missed");
            }
        }
    }

    IEnumerator blink()
    {
        for (int i = 0; i < howManyBlink * 2; i++)
        {
            if(visible)
            {
                spriteRenderer.color = new Color (defaultColor.r, defaultColor.g, defaultColor.b, 0);
                //Debug.Log("off");
                visible = false;
            }
            else
            {
                spriteRenderer.color = defaultColor;
                //Debug.Log("on");
                visible = true;
            }
            yield return new WaitForSeconds(blinkRate);
        }
        spriteRenderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
