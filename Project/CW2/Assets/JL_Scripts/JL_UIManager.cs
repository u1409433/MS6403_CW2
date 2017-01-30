using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class JL_UIManager : NetworkBehaviour
{
    public JL_LevelManager mSC_LevelManager;

    public GameObject UI_Shapeshift;

    public GameObject UI_P1Form;
    public GameObject UI_P2Form;


    // Use this for initialization
    void Start()
    {
        mSC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UI_P1Form.GetComponent<Text>().text = mSC_LevelManager.ST_P1Form;
        UI_P2Form.GetComponent<Text>().text = mSC_LevelManager.ST_P2Form;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Level",LoadSceneMode.Single);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

    public void EndCollision()
    {
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
    }

}
