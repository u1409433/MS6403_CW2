using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JL_CameraScript : NetworkBehaviour
{
    public GameObject P1;
    public GameObject P2;

    public bool GameStart = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart)
        {
            Vector3 Targetpos = new Vector3((P1.transform.position.x + P2.transform.position.x) / 2, (P1.transform.position.y + P2.transform.position.y) / 2, (P1.transform.position.z + P2.transform.position.z) / 2);
            transform.LookAt(Targetpos);
            transform.position = Targetpos + new Vector3(0, 20, -5);
        }
    }
}
