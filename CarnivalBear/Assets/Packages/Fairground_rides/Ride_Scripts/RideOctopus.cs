using UnityEngine;
using System.Collections;

public class RideOctopus : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//										Andre "AEG" Buerger - VIS-Games 2012
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------

    GameObject dreharm;
    GameObject[] arm;
    GameObject[] kreuz;

    float dreharm_rot;
    float arm_rot;
    float kreuz_rot;
    float arm_sin;

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    dreharm = transform.Find("dreharm").gameObject;
    
    arm = new GameObject[4];
    arm[0] = dreharm.transform.Find("arm1").gameObject;
    arm[1] = dreharm.transform.Find("arm2").gameObject;
    arm[2] = dreharm.transform.Find("arm3").gameObject;
    arm[3] = dreharm.transform.Find("arm4").gameObject;

    kreuz = new GameObject[4];
    kreuz[0] = arm[0].transform.Find("kreuzcenter").gameObject;
    kreuz[1] = arm[1].transform.Find("kreuzcenter").gameObject;
    kreuz[2] = arm[2].transform.Find("kreuzcenter").gameObject;
    kreuz[3] = arm[3].transform.Find("kreuzcenter").gameObject;

    dreharm_rot = 0.0f;
    arm_rot = 0.0f;
    kreuz_rot = 0.0f;
    arm_sin = 0.0f;
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Start()
{
    Update();
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Update()
{   
    //-- dreharm rotation
    dreharm_rot = (dreharm_rot + Time.deltaTime * 65.0f) % 360.0f;
    dreharm.transform.localEulerAngles = new Vector3(0.0f, dreharm_rot, 0.0f);

    //-- arm up/down rotation
    arm_rot = (Mathf.Sin(arm_sin) * 11.0f) + 11.0f;
    arm_sin = (arm_sin + Time.deltaTime * 2.0f) % 360.0f;
    arm[0].transform.localEulerAngles = new Vector3(0.0f,   0.0f,         arm_rot);
    arm[1].transform.localEulerAngles = new Vector3(0.0f,  90.0f, 22.0f - arm_rot);
    arm[2].transform.localEulerAngles = new Vector3(0.0f, 180.0f,         arm_rot);
    arm[3].transform.localEulerAngles = new Vector3(0.0f, 270.0f, 22.0f - arm_rot);

    //-- kreuz rotation
    kreuz_rot = (dreharm_rot - Time.deltaTime * 180.0f) % 360.0f;
    kreuz[0].transform.localEulerAngles = new Vector3(0.0f, kreuz_rot, 0.0f);
    kreuz[1].transform.localEulerAngles = new Vector3(0.0f, kreuz_rot, 0.0f);
    kreuz[2].transform.localEulerAngles = new Vector3(0.0f, kreuz_rot, 0.0f);
    kreuz[3].transform.localEulerAngles = new Vector3(0.0f, kreuz_rot, 0.0f);

}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
