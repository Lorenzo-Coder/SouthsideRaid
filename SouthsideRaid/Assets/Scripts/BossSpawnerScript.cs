using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnerScript : MonoBehaviour
{
    [SerializeField] private float currentBossMaxHealth;
    public GameObject longArmPrefab;
    public GameObject apePrefab;
    private BossType previousBossType;
    public float hpMultiplier = 1.5f;

    private void Start()
    {
        currentBossMaxHealth = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>().maxHealth;
        previousBossType = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>().bossType;
    }
    // Update is called once per frame
    void Update()
    {
        // If there is no boss spawn a new one with more hp.
        if (GameObject.FindGameObjectWithTag("Boss") == null)
        {
            GameObject newBoss = apePrefab;

            //alternate between the two boss types
            //switch (previousBossType)
            //{
            //    case BossType.Ape:
                    newBoss = Instantiate(apePrefab, Vector3.zero, Quaternion.identity);
            //        previousBossType = BossType.LongArm;
            //        break;
            //    case BossType.LongArm:
            //        newBoss = Instantiate(apePrefab, Vector3.zero, Quaternion.identity);
            //        previousBossType = BossType.Ape;

            //        break;
            //    default: break;
            //}
            //newBoss = Instantiate(longArmPrefab, Vector3.zero, Quaternion.identity);
            newBoss.GetComponent<BossScript>().maxHealth = (currentBossMaxHealth * hpMultiplier);
            currentBossMaxHealth = newBoss.GetComponent<BossScript>().maxHealth;
        }
    }
}
