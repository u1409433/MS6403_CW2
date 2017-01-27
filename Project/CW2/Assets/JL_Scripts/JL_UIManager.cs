using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JL_UIManager : MonoBehaviour
{
    private JL_LevelManager mSC_LevelManager;
    private JL_PCControl mSC_PCControl;

    public GameObject UI_Shapeshift;


    // Use this for initialization
    void Start()
    {
        mSC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();

        mSC_PCControl = GameObject.Find("PC_P1").GetComponent<JL_PCControl>();
    }

    // Update is called once per frame
    void Update()
    {
        UI_Shapeshift.GetComponent<Toggle>().isOn = mSC_PCControl.GetShift();
    }
}
