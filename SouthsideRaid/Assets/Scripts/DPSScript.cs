using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DPSScript : MonoBehaviour
{
    private TextMeshPro textMesh;

    void Awake()
    {
        textMesh = gameObject.transform.GetComponent<TextMeshPro>();
    }
    
    public void DPS(int _damage)
    {
        textMesh.SetText(_damage.ToString());
    }
}
