using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour
{

    IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CleanUp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
