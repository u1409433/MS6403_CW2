using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class JL_PCControl2 : NetworkBehaviour
{

    public float mFL_Speed = 6;
    public float mFL_Gravity = 10;
    private CharacterController mCC_PC;
    public Vector3 mV3_Direction;

    public GameObject GO_Capsule;
    private GameObject mGO_Child;

    public bool mBL_ShiftAvailable = true;
    public bool mBL_Holding = false;

    private JL_CameraScript SC_Camera;

    // Use this for initialization
    void Start()
    {
        mCC_PC = GetComponent<CharacterController>();
        SC_Camera = GameObject.Find("Camera").GetComponent<JL_CameraScript>();
        SC_Camera.P2 = gameObject;
        SC_Camera.GameStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Determine if you're the host or not
            if (isClient) CmdShapeshift();
            else RpcShapeshift();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CmdInteract(gameObject.transform.tag);
        }

        Move();
    }

    void Move()
    {

        int tIN_X = 0;
        int tIN_Z = 0;

        if (Input.GetKey(KeyCode.A))
        {
            tIN_X += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            tIN_X += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            tIN_Z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            tIN_Z += -1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            tIN_X *= 2;
            tIN_Z *= 2;
        }

        if (mCC_PC.isGrounded)
        {
            mV3_Direction = new Vector3(tIN_X, 0, tIN_Z);

            mV3_Direction = mFL_Speed * transform.TransformDirection(mV3_Direction);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mV3_Direction.y = 5;
            }
        }

        mV3_Direction.y -= mFL_Gravity * Time.deltaTime;

        mCC_PC.Move(mV3_Direction * Time.deltaTime);
    }

    [ClientRpc]
    void RpcShapeshift()
    {
        if (mBL_ShiftAvailable)
        {
            if (gameObject.transform.tag == "Tiny")
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.8f, 0);
                GO_Capsule.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                mCC_PC.radius = 0.5f;
                mCC_PC.height = 2;
                mFL_Speed = 3;
                gameObject.transform.tag = "Big";
                mBL_ShiftAvailable = false;
                Invoke("RestoreAbility", 3f);
            }
            else if (gameObject.transform.tag == "Big")
            {
                GO_Capsule.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                mCC_PC.radius = 0.25f;
                mCC_PC.height = 1;
                mFL_Speed = 6;
                gameObject.transform.tag = "Tiny";
                mBL_ShiftAvailable = false;
                Invoke("RestoreAbility", 3f);
            }
        }
    }

    [Command]
    void CmdShapeshift()
    {
        if (mBL_ShiftAvailable)
        {
            if (gameObject.transform.tag == "Tiny")
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.8f, 0);
                GO_Capsule.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                mCC_PC.radius = 0.5f;
                mCC_PC.height = 2;
                mFL_Speed = 3;
                gameObject.transform.tag = "Big";
                mBL_ShiftAvailable = false;
                Invoke("RestoreAbility", 3f);
            }
            else if (gameObject.transform.tag == "Big")
            {
                GO_Capsule.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                mCC_PC.radius = 0.25f;
                mCC_PC.height = 1;
                mFL_Speed = 6;
                gameObject.transform.tag = "Tiny";
                mBL_ShiftAvailable = false;
                Invoke("RestoreAbility", 3f);
            }
        }
    }

    [Command]
    void CmdInteract(string vTag)
    {
        if (mBL_Holding)
        {
            mGO_Child.transform.SetParent(null);
            mBL_Holding = false;
        }
        else
        {
            float tFL_Closest = 1000f;
            GameObject tGO_Closest = null;

            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Interactable"))
            {
                float dist = Vector3.Distance(gameObject.transform.position, item.transform.position);

                if (dist <= tFL_Closest) tFL_Closest = dist;
                tGO_Closest = item;
            }

            if (tFL_Closest < 2f) tGO_Closest.SendMessage("CmdInteract", gameObject);

            Debug.Log(tGO_Closest.transform.name.ToString() + " is " + tFL_Closest.ToString() + " units away");
        }
    }





    //-----------------------------THIS SECTION IS FOR AUTOMATICALLY HANDLED FUNCTIONS----------------------------------//
    public override void OnStartLocalPlayer()
    {
        GO_Capsule.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void RestoreAbility()
    {
        mBL_ShiftAvailable = true;
    }

    public bool GetShift()
    {
        return mBL_ShiftAvailable;
    }

    public void SetChild(GameObject vChild)
    {
        mGO_Child = vChild;
        mBL_Holding = true;
    }
}
