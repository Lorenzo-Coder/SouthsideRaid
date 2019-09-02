using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnerScript : MonoBehaviour
{
    [SerializeField] private int currentBossMaxHealth;
    public GameObject bossPrefab;
    public float hpMultiplier = 1.5f;

    private void Start()
    {
        currentBossMaxHealth = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>().maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        // If there is no boss spawn a new one with more hp.
        if (GameObject.FindGameObjectWithTag("Boss") == null)
        {
            GameObject newBoss = Instantiate(bossPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newBoss.GetComponent<BossScript>().maxHealth = (int)(currentBossMaxHealth * hpMultiplier);
            currentBossMaxHealth = newBoss.GetComponent<BossScript>().maxHealth;
        }
    }
}
