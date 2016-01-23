using UnityEngine;
using System.Collections;

public class RideCondor : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//										Andre "AEG" Buerger - VIS-Games 2012
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------

    GameObject hubwerk;
    GameObject arme;
    GameObject[] ring;

    float hubwerk_ypos;
    float arm_rot;
    float ring_rot;

    int hubwerk_mode;
    float hubwerk_timer;

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    hubwerk = transform.Find("plattform_typ_02a/mast/hubwerk").gameObject;
    arme = hubwerk.transform.Find("arm_halter").gameObject;

    ring = new GameObject[4];
    ring[0] = arme.transform.Find("arm1/ring").gameObject;
    ring[1] = arme.transform.Find("arm2/ring").gameObject;
    ring[2] = arme.transform.Find("arm3/ring").gameObject;
    ring[3] = arme.transform.Find("arm4/ring").gameObject;

    hubwerk_ypos = 3.45f;
    arm_rot = 0.0f;
    ring_rot = 0.0f;

    hubwerk_mode = 0;
    hubwerk_timer = 0.0f;
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Start()
{
    
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Update()
{
    int i;
    
    //-- hubwerk
    if(hubwerk_mode == 0)
    {
        hubwerk_timer += Time.deltaTime * 1.0f;
        if(hubwerk_timer >= 5.0f)
        {
            hubwerk_mode = 1;
        }
    }
    else if(hubwerk_mode == 1)
    {
        hubwerk_ypos += Time.deltaTime * 2.0f;
        if(hubwerk_ypos >= 37.6f)
        {
            hubwerk_timer = 0.0f;
            hubwerk_mode = 2;    
        }
    }
    else if(hubwerk_mode == 2)
    {
        hubwerk_timer += Time.deltaTime * 1.0f;
        if(hubwerk_timer >= 5.0f)
        {
            hubwerk_mode = 3;
        }
    }
    else
    {
        hubwerk_ypos -= Time.deltaTime * 2.0f;
        if(hubwerk_ypos <= 3.45f)
        {
            hubwerk_timer = 0.0f;
            hubwerk_mode = 0;    
        }
    }
    hubwerk.transform.localPosition = new Vector3(0.0f, hubwerk_ypos, -1.758529f);


    //-- arme
    arm_rot = (arm_rot + Time.deltaTime * 50.0f) % 360.0f;
    arme.transform.localEulerAngles = new Vector3 (0.0f, arm_rot, 0.0f);

    //-- ringe
    ring_rot = (ring_rot - Time.deltaTime * 120.0f) % 360.0f;
    for(i=0;i<4;i++)
        ring[i].transform.localEulerAngles = new Vector3 (0.0f, ring_rot, 0.0f);


}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
