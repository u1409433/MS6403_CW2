using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_TargetBox : MonoBehaviour
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
            GO_Target.SetActive(true);
        }
    }
}
