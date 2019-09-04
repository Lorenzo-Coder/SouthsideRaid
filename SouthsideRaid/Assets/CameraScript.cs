using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour
{
    private Vector3 initCamPos;

    // Start is called before the first frame update
    void Start()
    {
        initCamPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    CameraShake();
        //}
        transform.position = initCamPos;
    }

    public void CameraShake()
    {
        gameObject.transform.DOShakePosition(0.1f, 0.5f, 10, 90.0f, false, false);
    }
}
