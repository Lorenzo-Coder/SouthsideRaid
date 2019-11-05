using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackPrefabScript : MonoBehaviour
{
    public float timeToDamage = 0.5f;
    public float blinkRate  = 1.0f;
    public int howManyBlink = 10;

    private GameObject boss;
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private bool visible = true;
    private bool countdownEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        boss = GameObject.FindGameObjectWithTag("Boss");
        //StartCoroutine(blink());
    }

    // Update is called once per frame
    void Update()
    {
        //if (countdownEnded == false)
        //{
            timeToDamage -= Time.deltaTime;
            Debug.Log(timeToDamage);
            if (timeToDamage <= 0.0f)
            {
                countdownEnded = true;
            }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("help me im crying");
        if ((other.tag == "Player") && (countdownEnded == true))
        {
            Debug.Log("player in trigger, countrdown ended");
            if (other.GetComponent<PlayerScript>().invincible == false)
            {
                other.GetComponent<PlayerScript>().GetHit();
                Debug.Log("called player getHit");
            }
        }
    }

    IEnumerator blink()
    {
        for (int i = 0; i < howManyBlink * 2; i++)
        {
            //objectRenderer.enabled = false;
            //objectRenderer.enabled = true;
            //if (gameObject.activeSelf)
            //{
            //    gameObject.SetActive(false);
            //}
            //else
            //{
            //    gameObject.SetActive(true);
            //}
            if(visible)
            {
                spriteRenderer.color = new Color (defaultColor.r, defaultColor.g, defaultColor.b, 0);
                Debug.Log("off");
                visible = false;
            }
            else
            {
                spriteRenderer.color = defaultColor;
                Debug.Log("on");
                visible = true;
            }
            yield return new WaitForSeconds(blinkRate);
        }
        spriteRenderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
    }
}
