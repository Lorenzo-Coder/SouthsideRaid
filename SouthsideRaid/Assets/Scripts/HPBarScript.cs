using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarScript : MonoBehaviour
{
    public Image fill;
    public Image chunk;
    GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveChunk(int _damage)
    {
        // get the scale
        float scaleX = _damage / boss.GetComponent<BossScript>().maxHealth;
        Debug.Log("new chunk");
        Image newChunk = Instantiate(chunk, boss.GetComponent<Canvas>().transform);
        newChunk.transform.localScale = new Vector3(scaleX, newChunk.transform.localScale.y, newChunk.transform.localScale.z);
        newChunk.transform.position = new Vector3(fill.transform.position.x + (scaleX * fill.rectTransform.rect.width), fill.transform.position.y, fill.transform.position.z);
    }
}
