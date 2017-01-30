using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class JL_LevelManager : NetworkBehaviour
{
    private NetworkIdentity objNetId;
    public int IN_CatapultPieces;

    public GameObject GO_CatapultSwitch;

    public string ST_P1Form;
    public string ST_P2Form;

    //public Material Mat_Purple;
    //public Material Mat_Green;
    //public Material Mat_Orange;
    //public Material Mat_Blue;

    // Use this for initialization
    void Start()
    {
        ST_P1Form = "Techy";
        ST_P2Form = "Techy";

        GameObject.Find("UI").GetComponent<JL_UIManager>().mSC_LevelManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (IN_CatapultPieces >= 2)
        {
            RpcSpawnCatapultSwitch();            
        }
    }

    [ClientRpc]
    void RpcSpawnCatapultSwitch()
    {
        GO_CatapultSwitch.SetActive(true);
    }

    public bool StateCheck(string vForm)
    {
        if (ST_P1Form == vForm || ST_P2Form == vForm) return true;
        else return false;
    }

}
