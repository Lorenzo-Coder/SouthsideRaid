using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    public GameObject crit1;
    public GameObject crit2;
    public GameObject crit3;
    public GameObject punch1;
    public GameObject punch2;
    public GameObject punch3;

    public void SpawnCritFX()
    {
        int randVal = Random.Range(1, 3);
        float randXPos = Random.Range(-2.0f, 2.0f);
        float randYPos = Random.Range(-0.5f, 0.5f);

        switch (randVal)
        {
            case 1:
                GameObject VFX = Instantiate(crit1, Vector3.zero, Quaternion.identity, gameObject.transform);
                VFX.transform.position = new Vector3(gameObject.transform.position.x + randXPos, gameObject.transform.position.y - 2 + randYPos, gameObject.transform.position.z);
                StartCoroutine(waitToDestroy(VFX));
                break;
            case 2:
                GameObject VFX2 = Instantiate(crit2, Vector3.zero, Quaternion.identity, gameObject.transform);
                VFX2.transform.position = new Vector3(gameObject.transform.position.x + randXPos, gameObject.transform.position.y - 2 + randYPos, gameObject.transform.position.z);
                StartCoroutine(waitToDestroy(VFX2));
                break;
            case 3:
                GameObject VFX3 = Instantiate(crit3, Vector3.zero, Quaternion.identity, gameObject.transform);
                VFX3.transform.position = new Vector3(gameObject.transform.position.x + randXPos, gameObject.transform.position.y - 2 + randYPos, gameObject.transform.position.z);
                StartCoroutine(waitToDestroy(VFX3));
                break;
        }
    }

    public void SpawnPunchFX()
    {
        int randVal = Random.Range(1, 3);
        float randXPos = Random.Range(-2.0f, 2.0f);
        float randYPos = Random.Range(-0.5f, 0.5f);

        switch (randVal)
        {
            case 1:
                GameObject VFX = Instantiate(punch1, Vector3.zero, Quaternion.identity, gameObject.transform);
                VFX.transform.position = new Vector3(gameObject.transform.position.x + randXPos, gameObject.transform.position.y - 2 + randYPos, gameObject.transform.position.z);
                StartCoroutine(waitToDestroy(VFX));
                break;
            case 2:
                GameObject VFX2 = Instantiate(punch2, Vector3.zero, Quaternion.identity, gameObject.transform);
                VFX2.transform.position = new Vector3(gameObject.transform.position.x + randXPos, gameObject.transform.position.y - 2 + randYPos, gameObject.transform.position.z);
                StartCoroutine(waitToDestroy(VFX2));
                break;
            case 3:
                GameObject VFX3 = Instantiate(punch3, Vector3.zero, Quaternion.identity, gameObject.transform);
                VFX3.transform.position = new Vector3(gameObject.transform.position.x + randXPos, gameObject.transform.position.y - 2 + randYPos, gameObject.transform.position.z);
                StartCoroutine(waitToDestroy(VFX3));
                break;
        }
    }

    IEnumerator waitToDestroy(GameObject _object)
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(_object);
    }
}
