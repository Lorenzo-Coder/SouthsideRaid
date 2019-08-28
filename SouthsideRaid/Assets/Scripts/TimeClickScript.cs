using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeClickScript : MonoBehaviour
{
    public GameObject outline;
    public GameObject centre;

    //public float decreaseRate = 2.0f;
    public float outlineScale = 0.1f;
    public float centreScale = 0.01f;

    public float durationMin = 1.5f;
    public float durationMax = 2.5f;
    //public float duration = 2.0f;

    public bool canHit = false;

    // Start is called before the first frame update
    void Start()
    {
        outline.transform.localScale = new Vector3(outlineScale, outlineScale, outlineScale);
        centre.transform.localScale = new Vector3(centreScale, centreScale, centreScale);
        centre.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        //outline.transform.DOScale(centreScale / 2.0f, 2.0f);
        outline.transform.DOScale(0.0f, Random.Range(durationMin,durationMax));

        //int numActivePlayers = 0;
        //foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        //{
        //    if (player.GetComponent<PlayerScript>().joined)
        //    {
        //        numActivePlayers++;
        //    }
        //}

        //canHit = new bool[numActivePlayers];

        //for (int i = 0; i < canHit.Length; i++)
        //{
        //    canHit[i] = true;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (outline.transform.localScale.x <= 0.0f)
        {
            Destroy(gameObject);
        }
        if (outline.transform.localScale.x <= (centre.transform.localScale.x*1.15))
        {
            centre.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
            canHit = true;
        }
    }
}
