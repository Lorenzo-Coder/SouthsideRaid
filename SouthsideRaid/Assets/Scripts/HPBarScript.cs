using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBarScript : MonoBehaviour
{
    public Image fill;
    public Image chunk;
    public Color endColor;
    GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    IEnumerator CleanUp(Image _chunk)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(_chunk);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // this is called whenever boss takes damage
    public void RemoveChunk(float _damage)
    {
        // get the scale
        float scaleX = (float)_damage / (float)boss.GetComponent<BossScript>().maxHealth;

        // get the scale for the position on the x axis
        float posScale = (float)boss.GetComponent<BossScript>().currentHealth /(float)boss.GetComponent<BossScript>().maxHealth;

        // create the new chunk with the canvas as parent
        Image newChunk = Instantiate(chunk, boss.GetComponentInChildren<Canvas>().transform);

        // set the new scale and positions
        float newPos = fill.rectTransform.position.x + (posScale * fill.rectTransform.rect.width);
        newChunk.rectTransform.anchoredPosition3D = new Vector3(newChunk.rectTransform.anchoredPosition3D.x + posScale * fill.rectTransform.rect.width, newChunk.rectTransform.anchoredPosition3D.y, fill.transform.position.z);
        newChunk.transform.localScale = new Vector3(scaleX, newChunk.transform.localScale.y, newChunk.transform.localScale.z);

        // fade the chunk
        newChunk.GetComponent<Image>().DOColor(endColor, 0.5f);

        // move the chunk slightly
        newChunk.transform.DOLocalMoveY(20, 1.0f, false);
        newChunk.transform.DOLocalMoveX(newChunk.rectTransform.anchoredPosition3D.x + 5, 0.5f, false);
    }
}
