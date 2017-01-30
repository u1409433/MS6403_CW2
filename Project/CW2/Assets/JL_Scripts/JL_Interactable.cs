using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Networking;

public class JL_Interactable : NetworkBehaviour
{
    public GameObject GO_Interactable;
    public GameObject GO_Lava;
    public Material Mat_Unusable;

    public GameObject GO_Parent;

    public GameObject GO_Cube1;
    public GameObject GO_Cube2;

    public bool BL_Activated = false;

    private JL_LevelManager SC_LevelManager; 

    // Use this for initialization
    void Start()
    {
        SC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.tag == "LockedDoor")
        {
            if (GO_Cube1.GetComponent<JL_Interactable>().BL_Activated && GO_Cube2.GetComponent<JL_Interactable>().BL_Activated && !BL_Activated)
            {
                gameObject.transform.DOMove(transform.position + new Vector3(0, 3, 0), 3);
                BL_Activated = true;
            }
        }
    }

    [Command]
    public void CmdInteract(GameObject vPlayer)
    {
        JL_PCControl mSC_PC = vPlayer.GetComponent<JL_PCControl>();
        if (gameObject.name == "CatapultPiece")
        {
            gameObject.transform.SetParent(vPlayer.transform);
            GO_Parent = vPlayer;
            mSC_PC.SetChild(gameObject);
        }
        else if (gameObject.name == "Open Door")
        {
            gameObject.transform.DOMove(transform.position + new Vector3(0, 3, 0), 3);
        }
        else if (gameObject.name == "Platform Switch")
        {
            GO_Interactable.SendMessage("CmdActivate");
        }
        else if (gameObject.name == "Catapult Switch")
        {
            GO_Interactable.GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find("Catapult").GetComponent<Rigidbody>().isKinematic = false;
        }
        else if (gameObject.name == "Door Switch")
        {
            GO_Interactable.SetActive(false);
            GO_Lava.SetActive(true);
        }
        else if (gameObject.name == "Strong Cube")
        {
            if (SC_LevelManager.StateCheck("Strong"))
            {
                GetComponent<Renderer>().material = Mat_Unusable;
                BL_Activated = true;
            }
        }
        else if (gameObject.name == "Light Cube")
        {
            if (SC_LevelManager.StateCheck("Light"))
            {
                GetComponent<Renderer>().material = Mat_Unusable;
                BL_Activated = true;
            }
        }
        else if (gameObject.name == "Fast Cube")
        {
            if (SC_LevelManager.StateCheck("Fast"))
            {
                GetComponent<Renderer>().material = Mat_Unusable;
                BL_Activated = true;
            }
        }
        else if (gameObject.name == "Techy Cube")
        {
            if (SC_LevelManager.StateCheck("Techy"))
            {
                GetComponent<Renderer>().material = Mat_Unusable;
                BL_Activated = true;
            }
        }
    }

    
    void OnTriggerEnter(Collider vCollided)
    {
        if (vCollided.transform.tag == "CatapultCollection")
        {
            if (transform.parent != null) GO_Parent.GetComponent<JL_PCControl>().CmdDrop();
            GameObject.Destroy(gameObject);
            GameObject.Find("LevelManager").GetComponent<JL_LevelManager>().IN_CatapultPieces++;
        }
    }
}
