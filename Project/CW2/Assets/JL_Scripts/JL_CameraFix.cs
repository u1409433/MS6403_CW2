using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JL_CameraFix : NetworkBehaviour
{
    [SerializeField]
    private GameObject myCamera;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        myCamera.SetActive(true);
    }
}
