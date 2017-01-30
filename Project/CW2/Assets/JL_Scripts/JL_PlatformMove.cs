using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;

public class JL_PlatformMove : NetworkBehaviour
{
    public float FL_Speed;
    public float FL_Target1;
    public float FL_Target2;

    public Material Mat_Unusable;
    public Material Mat_Original;

    private float FL_CurrentTarget;

    private bool BL_Active = true;

    // Use this for initialization
    void Start()
    {
        Sequence BackandForth = DOTween.Sequence().SetId(gameObject.name);
        BackandForth.Append(transform.DOMoveX(FL_Target1, FL_Speed).SetId("Back"));
        BackandForth.Append(transform.DOMoveX(FL_Target2, FL_Speed).SetId("Forth"));
        BackandForth.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdActivate()
    {
        Debug.Log("ACTIVATE PLATFORM CMD");
        BL_Active = false;
        DOTween.Pause(gameObject.name);
        Invoke("CmdRestoreMovement", 3);

        GetComponent<Renderer>().material = Mat_Unusable;

        RpcActivate();
    }

    [Command]
    void CmdRestoreMovement()
    {
        BL_Active = true;
        DOTween.Play(gameObject.name);
        GetComponent<Renderer>().material = Mat_Original;

        RpcRestoreMovement();
    }

    [ClientRpc]
    void RpcActivate()
    {
        Debug.Log("ACTIVATE PLATFORM RPC");
        BL_Active = false;
        DOTween.Pause(gameObject.name);
        Invoke("RpcRestoreMovement", 3);

        GetComponent<Renderer>().material = Mat_Unusable;
    }

    [ClientRpc]
    void RpcRestoreMovement()
    {
        BL_Active = true;
        DOTween.Play(gameObject.name);
        GetComponent<Renderer>().material = Mat_Original;
    }
}
