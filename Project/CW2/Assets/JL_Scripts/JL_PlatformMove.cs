using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JL_PlatformMove : MonoBehaviour
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
        /*if (!BL_Active) DOTween.PauseAll();
        else DOTween.PlayAll();*/

        //if (FL_CurrentTarget - transform.position.x != 0)
        //{
        //    transform.Translate(1, 0, 0);
        //}
    }

    void Activate()
    {
        Debug.Log("ACTIVATE PLATFORM");
        BL_Active = false;
        DOTween.Pause(gameObject.name);
        Invoke("RestoreMovement", 3);

        GetComponent<Renderer>().material = Mat_Unusable;
    }

    void RestoreMovement()
    {
        BL_Active = true;
        DOTween.Play(gameObject.name);
        GetComponent<Renderer>().material = Mat_Original;
    }

}
