using UnityEngine;
using System.Collections;

public class RideBooster : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//										Andre "AEG" Buerger - VIS-Games 2012
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------


    GameObject teller;
    GameObject[] kreuze;
    GameObject[] gondeln;

    float teller_rotation;
    float kreuz_rotation;
    float[] gondel_start_rot;
    float[] gondel_rotation;
    float[] gondel_sin1;
    float[] gondel_sin2;
    float[] gondel_sin3;
    float[] gondel_add1;
    float[] gondel_add2;
    float[] gondel_add3;

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    teller = transform.Find("plattform_typ_01b/teller_null/teller").gameObject;    

    kreuze  = new GameObject[4];
    gondeln = new GameObject[16];

    kreuze[0] = teller.transform.Find("kreuz1").gameObject;
    kreuze[1] = teller.transform.Find("kreuz2").gameObject;
    kreuze[2] = teller.transform.Find("kreuz3").gameObject;
    kreuze[3] = teller.transform.Find("kreuz4").gameObject;

    gondeln[ 0] = kreuze[0].transform.Find("arm1/gondel_achse/gondel").gameObject;
    gondeln[ 1] = kreuze[0].transform.Find("arm2/gondel_achse/gondel").gameObject;
    gondeln[ 2] = kreuze[0].transform.Find("arm3/gondel_achse/gondel").gameObject;
    gondeln[ 3] = kreuze[0].transform.Find("arm4/gondel_achse/gondel").gameObject;

    gondeln[ 4] = kreuze[1].transform.Find("arm1/gondel_achse/gondel").gameObject;
    gondeln[ 5] = kreuze[1].transform.Find("arm2/gondel_achse/gondel").gameObject;
    gondeln[ 6] = kreuze[1].transform.Find("arm3/gondel_achse/gondel").gameObject;
    gondeln[ 7] = kreuze[1].transform.Find("arm4/gondel_achse/gondel").gameObject;

    gondeln[ 8] = kreuze[2].transform.Find("arm1/gondel_achse/gondel").gameObject;
    gondeln[ 9] = kreuze[2].transform.Find("arm2/gondel_achse/gondel").gameObject;
    gondeln[10] = kreuze[2].transform.Find("arm3/gondel_achse/gondel").gameObject;
    gondeln[11] = kreuze[2].transform.Find("arm4/gondel_achse/gondel").gameObject;
    
    gondeln[12] = kreuze[3].transform.Find("arm1/gondel_achse/gondel").gameObject;
    gondeln[13] = kreuze[3].transform.Find("arm2/gondel_achse/gondel").gameObject;
    gondeln[14] = kreuze[3].transform.Find("arm3/gondel_achse/gondel").gameObject;
    gondeln[15] = kreuze[3].transform.Find("arm4/gondel_achse/gondel").gameObject;

    teller_rotation = 0.0f;
    kreuz_rotation = 0.0f;

    gondel_rotation = new float[16];
    gondel_start_rot = new float[16];
    gondel_sin1 = new float[16];
    gondel_sin2 = new float[16];
    gondel_sin3 = new float[16];
    gondel_add1 = new float[16];
    gondel_add2 = new float[16];
    gondel_add3 = new float[16];

    int i;
    for(i=0;i<16;i++)
    {
        gondel_start_rot[i] = gondeln[i].transform.localEulerAngles.y;
        gondel_rotation[i] = 0.0f;    
        gondel_sin1[i] = Random.Range(0.0f, 360.0f);
        gondel_sin2[i] = Random.Range(0.0f, 360.0f);
        gondel_sin3[i] = Random.Range(0.0f, 360.0f);

        gondel_add1[i] = Random.Range(0.2f, 3.0f);
        gondel_add2[i] = Random.Range(0.2f, 3.0f);
        gondel_add3[i] = Random.Range(0.2f, 3.0f);
    }
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
    int i;
    
    //-- teller rotation
    teller_rotation = (teller_rotation + Time.deltaTime * 60.0f) % 360.0f;
    teller.transform.localEulerAngles = new Vector3(0.0f, teller_rotation, 0.0f);
    
    //-- kreuz rotation
    kreuz_rotation = (kreuz_rotation - Time.deltaTime * 210.0f) % 360.0f;
    for(i=0;i<4;i++)
        kreuze[i].transform.localEulerAngles = new Vector3(0.0f, kreuz_rotation, 0.0f);

    //-- gondel rotations
    for(i=0;i<16;i++)
    {
        float sin1 = (Mathf.Sin(gondel_sin1[i])) * 360.0f;
        float sin2 = (Mathf.Sin(gondel_sin2[i])) * 210.0f;
        float sin3 = (Mathf.Sin(gondel_sin3[i])) * 450.0f;

        sin1 = (sin1 + sin2 + sin3) / 3.5f;
        
        gondeln[i].transform.localEulerAngles = new Vector3(0.0f, 0.0f, sin1 + gondel_start_rot[i]);

        gondel_sin1[i] += ((gondel_add1[i] * Time.deltaTime) % 360.0f);
        gondel_sin2[i] -= ((gondel_add2[i] * Time.deltaTime) % 360.0f);
        gondel_sin3[i] += ((gondel_add3[i] * Time.deltaTime) % 360.0f);
    }



}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
