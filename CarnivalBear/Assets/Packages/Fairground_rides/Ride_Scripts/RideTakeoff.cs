using UnityEngine;
using System.Collections;

public class RideTakeoff : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//										Andre "AEG" Buerger - VIS-Games 2012
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------

    GameObject arm;
    GameObject teller;
    GameObject[] gondeln;

    float arm_rot;
    float teller_rot;
    float gondel_rot;

    int arm_mode;
    float arm_timer;

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    arm = transform.Find("plattform/arm").gameObject;

    teller = arm.transform.Find("teller").gameObject;

    gondeln = new GameObject[4];
    gondeln[0] = teller.transform.Find("gondel1").gameObject;
    gondeln[1] = teller.transform.Find("gondel2").gameObject;
    gondeln[2] = teller.transform.Find("gondel3").gameObject;
    gondeln[3] = teller.transform.Find("gondel4").gameObject;

    arm_rot = 6.59f;
    arm_mode = 0;
    arm_timer = 0.0f;

    teller_rot = 0.0f;
    gondel_rot = 0.0f;
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
    //-- arm motion
    if(arm_mode == 0)
    {
        arm_timer += Time.deltaTime * 1.0f;
        if(arm_timer >= 4.0f)
        {
            arm_mode = 1;
        }
    }
    else if(arm_mode == 1)
    {
        arm_rot += Time.deltaTime * 4.0f;
        if(arm_rot >= 55.0f)
        {
            arm_timer = 0.0f;
            arm_mode = 2;
        }
    }
    else if(arm_mode == 2)
    {
        arm_timer += Time.deltaTime * 1.0f;
        if(arm_timer >= 4.0f)
        {
            arm_mode = 3;
        }
    }
    else
    {
        arm_rot -= Time.deltaTime * 4.0f;
        if(arm_rot <= 6.59f)
        {
            arm_timer = 0.0f;
            arm_mode = 0;
        }
    }
    arm.transform.localEulerAngles = new Vector3(arm_rot, 0.0f, 0.0f);

    //-- teller rotation
    teller_rot = (teller_rot + Time.deltaTime * 80.0f) % 360.0f;
    teller.transform.localEulerAngles = new Vector3(0.0f, teller_rot, 0.0f);

    //-- gondel rotations
    gondel_rot = (gondel_rot + Time.deltaTime * 150.0f) % 360.0f;
    gondeln[0].transform.localEulerAngles = new Vector3(0.0f,  gondel_rot, 0.0f);
    gondeln[1].transform.localEulerAngles = new Vector3(0.0f, -gondel_rot, 0.0f);
    gondeln[2].transform.localEulerAngles = new Vector3(0.0f,  gondel_rot, 0.0f);
    gondeln[3].transform.localEulerAngles = new Vector3(0.0f, -gondel_rot, 0.0f);


    
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
