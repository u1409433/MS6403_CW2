using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JL_TargetBox : NetworkBehaviour
{
    public GameObject GO_Target;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider vCollided)
    {
        if (vCollided.transform.name == "Projectile")
        {
            CmdActivate();
            GO_Target.SetActive(true);
        }
    }

    [Command]
    public void CmdActivate()
    {
        GO_Target.SetActive(true);

        //RpcActivate();
    }

    [ClientRpc]
    void RpcActivate()
    {
        GO_Target.SetActive(true);
    }
}
