using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackPrefabScript : MonoBehaviour
{
    public float timeToDamage = 0.5f;
    public float blinkRate = 0.08f;
    public int howManyBlink = 10;

    private GameObject boss;
    private Renderer objectRenderer;
    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        //InvokeRepeating("blink", 0.0f, 1.0f);
        StartCoroutine(blink());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator blink()
    {
        for (int i = 0; i < howManyBlink; i++)
        {
            objectRenderer.enabled = false;
            yield return new WaitForSeconds(blinkRate);
            objectRenderer.enabled = true;
            Debug.Log("offnon");
        }
    }
}
