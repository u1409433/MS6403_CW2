using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_CameraFollow : MonoBehaviour
{
    private GameObject PC;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PC = GameObject.Find("PC_P1(Clone)");
        if (PC != null) gameObject.transform.position = PC.transform.position + new Vector3(0, 15, 0);    
    }
}
