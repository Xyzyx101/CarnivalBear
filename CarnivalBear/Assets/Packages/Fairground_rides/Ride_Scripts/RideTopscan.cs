using UnityEngine;
using System.Collections;

public class RideTopscan : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//										Andre "AEG" Buerger - VIS-Games 2012
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------

    GameObject arm;
    GameObject kreuz;
    GameObject[] gondeln;

    float arm_rot;
    float kreuz_rot;
    float gondel_sin;

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    arm = transform.Find("arm1/arm2_null/arm2").gameObject;
    kreuz = arm.transform.Find("drehkreuz_null/drehkreuz").gameObject;
    
    gondeln = new GameObject[6];
    gondeln[0] = kreuz.transform.Find("gondel1").gameObject;
    gondeln[1] = kreuz.transform.Find("gondel2").gameObject;
    gondeln[2] = kreuz.transform.Find("gondel3").gameObject;
    gondeln[3] = kreuz.transform.Find("gondel4").gameObject;
    gondeln[4] = kreuz.transform.Find("gondel5").gameObject;
    gondeln[5] = kreuz.transform.Find("gondel6").gameObject;

    arm_rot = 0.0f;
    kreuz_rot = 0.0f;
    gondel_sin = 0.0f;
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
    //-- arm rotation
    arm_rot = (arm_rot + Time.deltaTime * 50.0f) % 360.0f;
    arm.transform.localEulerAngles = new Vector3(0.0f, arm_rot, 0.0f);

    //-- kreuz rotation
    kreuz_rot = (kreuz_rot + Time.deltaTime * 90.0f) % 360.0f;
    kreuz.transform.localEulerAngles = new Vector3(0.0f, kreuz_rot, 0.0f);

    //-- gondel swinging
    gondeln[0].transform.localEulerAngles = new Vector3(0.0f,   0.0f, (Mathf.Sin(gondel_sin +   0.0f) * 60.0f));
    gondeln[1].transform.localEulerAngles = new Vector3(0.0f,  60.0f, (Mathf.Sin(gondel_sin +  60.0f) * 60.0f));
    gondeln[2].transform.localEulerAngles = new Vector3(0.0f, 120.0f, (Mathf.Sin(gondel_sin + 120.0f) * 60.0f));
    gondeln[3].transform.localEulerAngles = new Vector3(0.0f, 180.0f, (Mathf.Sin(gondel_sin + 180.0f) * 60.0f));
    gondeln[4].transform.localEulerAngles = new Vector3(0.0f, 240.0f, (Mathf.Sin(gondel_sin + 240.0f) * 60.0f));
    gondeln[5].transform.localEulerAngles = new Vector3(0.0f, 300.0f, (Mathf.Sin(gondel_sin + 300.0f) * 60.0f));
    
    gondel_sin = (gondel_sin + Time.deltaTime * 2.0f) % 360.0f;
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
