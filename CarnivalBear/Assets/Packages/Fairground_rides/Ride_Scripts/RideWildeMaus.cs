using UnityEngine;
using System.Collections;

public class RideWildeMaus : MonoBehaviour {
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------

    GameObject wagen_01;
    GameObject wagen_02;
    GameObject wagen_03;
    GameObject wagen_04;
    GameObject wagen_05;
    GameObject wagen_06;
    GameObject wagen_07;

    Animation wagen_01_anim;
    Animation wagen_02_anim;
    Animation wagen_03_anim;
    Animation wagen_04_anim;
    Animation wagen_05_anim;
    Animation wagen_06_anim;
    Animation wagen_07_anim;

//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
void Awake()
{
    wagen_01 = transform.Find("wilde_maus_wagen1").gameObject;        
    wagen_02 = transform.Find("wilde_maus_wagen2").gameObject;        
    wagen_03 = transform.Find("wilde_maus_wagen3").gameObject;        
    wagen_04 = transform.Find("wilde_maus_wagen4").gameObject;        
    wagen_05 = transform.Find("wilde_maus_wagen5").gameObject;        
    wagen_06 = transform.Find("wilde_maus_wagen6").gameObject;        
    wagen_07 = transform.Find("wilde_maus_wagen7").gameObject;        

    wagen_01_anim = wagen_01.GetComponent<Animation>();
    wagen_02_anim = wagen_02.GetComponent<Animation>();
    wagen_03_anim = wagen_03.GetComponent<Animation>();
    wagen_04_anim = wagen_04.GetComponent<Animation>();
    wagen_05_anim = wagen_05.GetComponent<Animation>();
    wagen_06_anim = wagen_06.GetComponent<Animation>();
    wagen_07_anim = wagen_07.GetComponent<Animation>();
}
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
void Start()
{
    wagen_01_anim.Play("fahren_init1");    
    wagen_02_anim.Play("fahren_init2");    
    wagen_03_anim.Play("fahren_init3");    
    wagen_04_anim.Play("fahren_init4");    
    wagen_05_anim.Play("fahren_init5");    
    wagen_06_anim.Play("fahren_init6");    
    wagen_07_anim.Play("fahren_init7");    

}
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
void Update()
{
    if( (!wagen_01_anim.IsPlaying("fahren_init1")) && (!wagen_01_anim.IsPlaying("fahren_loop")) )
        wagen_01_anim.Play("fahren_loop");        

    if( (!wagen_02_anim.IsPlaying("fahren_init2")) && (!wagen_02_anim.IsPlaying("fahren_loop")) )
        wagen_02_anim.Play("fahren_loop");        

    if( (!wagen_03_anim.IsPlaying("fahren_init3")) && (!wagen_03_anim.IsPlaying("fahren_loop")) )
        wagen_03_anim.Play("fahren_loop");        

    if( (!wagen_04_anim.IsPlaying("fahren_init4")) && (!wagen_04_anim.IsPlaying("fahren_loop")) )
        wagen_04_anim.Play("fahren_loop");        

    if( (!wagen_05_anim.IsPlaying("fahren_init5")) && (!wagen_05_anim.IsPlaying("fahren_loop")) )
        wagen_05_anim.Play("fahren_loop");        

    if( (!wagen_06_anim.IsPlaying("fahren_init6")) && (!wagen_06_anim.IsPlaying("fahren_loop")) )
        wagen_06_anim.Play("fahren_loop");        

    if( (!wagen_07_anim.IsPlaying("fahren_init7")) && (!wagen_07_anim.IsPlaying("fahren_loop")) )
        wagen_07_anim.Play("fahren_loop");        

}
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
}
