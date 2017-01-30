using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class JL_PCControl : NetworkBehaviour
{

    public float mFL_Speed = 6;
    public float mFL_Gravity = 10;
    private CharacterController mCC_PC;
    public Vector3 mV3_Direction;

    public GameObject GO_Capsule;
    private GameObject mGO_Child;

    public bool mBL_ShiftAvailable = true;
    public bool mBL_Holding = false;

    public Material Mat_Purple;
    public Material Mat_Green;
    public Material Mat_Orange;
    public Material Mat_Blue;

    private JL_CameraScript SC_Camera;

    private JL_LevelManager SC_LevelManager;

    private Vector3 SpawnPoint;

    private string ST_Name;

    // Use this for initialization
    void Start()
    {
        mCC_PC = GetComponent<CharacterController>();
        SC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();
        SpawnPoint = Vector3.zero;
        ST_Name = "Techy";

        SC_Camera = GameObject.Find("Camera").GetComponent<JL_CameraScript>();

        if (SC_Camera.P1 == null)
        {
            SC_Camera.P1 = gameObject;
        }
        else
        {
            SC_Camera.P2 = gameObject;
            SC_Camera.GameStart = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (mBL_ShiftAvailable)
            {
                NameChange(ST_Name);
                CmdShapeshift();
                if (isServer) SC_LevelManager.ST_P1Form = ST_Name;
                else SC_LevelManager.ST_P2Form = ST_Name;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CmdInteract(gameObject.transform.tag);
        }

        Move();

        HeightCheck();
    }

    [Command]
    void CmdShapeshift()
    {
        switch (ST_Name)
        {
            case "Techy":
               GO_Capsule.GetComponent<Renderer>().material = Mat_Purple;
                break;
            case "Strong":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Orange;
                break;
            case "Fast":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Green;
                break;
            case "Light":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Blue;
                break;
        }

        RpcShapeshift();
    }

    [ClientRpc]
    void RpcShapeshift()
    {
        switch (ST_Name)
        {
            case "Techy":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Purple;
                break;
            case "Strong":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Orange;
                break;
            case "Fast":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Green;
                break;
            case "Light":
                GO_Capsule.GetComponent<Renderer>().material = Mat_Blue;
                break;
        }
    }

    void HeightCheck()
    {
        if (gameObject.transform.position.y < -15)
        {
            gameObject.transform.position = SpawnPoint;
        }
    }

    void NameChange(string vName)
    {
        switch (vName)
        {
            case "Techy":
                ST_Name = "Strong";
                break;
            case "Strong":
                ST_Name = "Fast";
                break;
            case "Fast":
                ST_Name = "Light";
                break;
            case "Light":
                ST_Name = "Techy";
                break;
        }
    }

    void Move()
    {

        int tIN_X = 0;
        int tIN_Z = 0;

        int tIN_Rot = 0;

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
        if (Input.GetKey(KeyCode.E))
        {
            tIN_Rot += 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            tIN_Rot += -1;
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

        gameObject.transform.Rotate(0, tIN_Rot, 0);

        mCC_PC.Move(mV3_Direction * Time.deltaTime);
    }

    void OnTriggerEnter(Collider vCollided)
    {
        if (vCollided.transform.tag == "Deadly")
        {
            gameObject.transform.position = SpawnPoint;
            if (vCollided.transform.name == "Lava")
            {
                vCollided.transform.position = new Vector3(10, -9, 160);
                Debug.Log("I touched Lava");
            }
        }

        if (vCollided.transform.tag == "Spawnpoint")
        {
            SpawnPoint = vCollided.gameObject.transform.position;
        }

        if (vCollided.transform.name == "End Platform")
        {
            GameObject.Find("UI").GetComponent<JL_UIManager>().EndCollision();
        }
    }

    [Command]
    void CmdInteract(string vTag)
    {
        if (mBL_Holding)
        {
            CmdDrop();
        }
        else
        {
            float tFL_Closest = 1000f;
            GameObject tGO_Closest = null;

            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Interactable"))
            {
                float dist = Vector3.Distance(gameObject.transform.position, item.transform.position);

                if (dist <= tFL_Closest)
                {
                    tFL_Closest = dist;
                    tGO_Closest = item;
                }
            }

            if (tFL_Closest < 2f) tGO_Closest.SendMessage("CmdInteract", gameObject);

            Debug.Log(tGO_Closest.transform.name.ToString() + " is " + tFL_Closest.ToString() + " units away");
        }
    }

    [Command]
    public void CmdDrop()
    {
        mGO_Child.transform.SetParent(null);
        mBL_Holding = false;
    }



    //-----------------------------THIS SECTION IS FOR AUTOMATICALLY HANDLED FUNCTIONS----------------------------------//
    public override void OnStartLocalPlayer()
    {
        GO_Capsule.GetComponent<MeshRenderer>().material.color = Color.blue;
        SC_Camera = GameObject.Find("Camera").GetComponent<JL_CameraScript>();

        if (SC_Camera.P1 == null)
        {
            SC_Camera.P1 = gameObject;
        }
        else
        {
            SC_Camera.P2 = gameObject;
            SC_Camera.GameStart = true;
        }
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
