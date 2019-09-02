using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
    public Canvas canvas;
    public GameObject DPSPopup;

    private SpriteRenderer spriteRenderer;
    private float blockTimer;
    private float stunTimer;
    private int damageMultiplier;
    private int timesAttacked = 0;
    public bool foundBoss;

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
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (foundBoss == false)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss != null)
            {
                foundBoss = true;
            }
        }

        if (boss.GetComponent<BossScript>().currentHealth <= 0)
        {
            foundBoss = false;
        }

        if (Input.GetKeyDown(playersButton) && joined == false)
        {
            JoinGame();
        }

        if (stunned == false)
        {
            if (Input.GetKeyDown(playersButton) && joined == true)
            {
                if (timesAttacked >=1)
                {
                 attack();
                }
                timesAttacked++;
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

        if((boss.GetComponent<BossScript>().stance == Stances.Down) && (boss.GetComponent<BossScript>().isCritical == true))
        {
            boss.GetComponent<BossScript>().dealDamage(damageAmount + damageMultiplier);
            Debug.Log("player " + playersButton + " is attacking with " + (damageAmount + damageMultiplier));

            PopUp(true);

            playerScore = playerScore + damageAmount + damageMultiplier;
            damageMultiplier = damageMultiplier * 2;
        }
        else if ((boss.GetComponent<BossScript>().stance == Stances.Down) && (boss.GetComponent<BossScript>().isCritical == false))
        {
            damageMultiplier = damageMultiplierAmount;
        }
        else
        {
            boss.GetComponent<BossScript>().dealDamage(damageAmount);

            PopUp(false);

            playerScore = playerScore + damageAmount;
            damageMultiplier = damageMultiplierAmount;
        }
    }

    void block()
    {
        Debug.Log("player " + playersButton + " is blocking");
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
}
