using UnityEngine;
using System.Collections;

public class RideWasserbahn : MonoBehaviour {

//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------

    GameObject boot_01;
    GameObject boot_02;
    GameObject boot_03;
    GameObject boot_04;
    GameObject boot_05;
    GameObject boot_06;

    Animation boot_01_anim;
    Animation boot_02_anim;
    Animation boot_03_anim;
    Animation boot_04_anim;
    Animation boot_05_anim;
    Animation boot_06_anim;

//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
void Awake()
{
    boot_01 = transform.Find("wasserbahn_boot1").gameObject;        
    boot_02 = transform.Find("wasserbahn_boot2").gameObject;        
    boot_03 = transform.Find("wasserbahn_boot3").gameObject;        
    boot_04 = transform.Find("wasserbahn_boot4").gameObject;        
    boot_05 = transform.Find("wasserbahn_boot5").gameObject;        
    boot_06 = transform.Find("wasserbahn_boot6").gameObject;        

    boot_01_anim = boot_01.GetComponent<Animation>();
    boot_02_anim = boot_02.GetComponent<Animation>();
    boot_03_anim = boot_03.GetComponent<Animation>();
    boot_04_anim = boot_04.GetComponent<Animation>();
    boot_05_anim = boot_05.GetComponent<Animation>();
    boot_06_anim = boot_06.GetComponent<Animation>();
}
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
void Start()
{
    boot_01_anim.Play("init1");    
    boot_02_anim.Play("init2");    
    boot_03_anim.Play("init3");    
    boot_04_anim.Play("init4");    
    boot_05_anim.Play("init5");    
    boot_06_anim.Play("fahren");    

}
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
void Update()
{
    if( (!boot_01_anim.IsPlaying("init1")) && (!boot_01_anim.IsPlaying("fahren")) )
        boot_01_anim.Play("fahren");        

    if( (!boot_02_anim.IsPlaying("init2")) && (!boot_02_anim.IsPlaying("fahren")) )
        boot_02_anim.Play("fahren");        

    if( (!boot_03_anim.IsPlaying("init3")) && (!boot_03_anim.IsPlaying("fahren")) )
        boot_03_anim.Play("fahren");        

    if( (!boot_04_anim.IsPlaying("init4")) && (!boot_04_anim.IsPlaying("fahren")) )
        boot_04_anim.Play("fahren");        

    if( (!boot_05_anim.IsPlaying("init5")) && (!boot_05_anim.IsPlaying("fahren")) )
        boot_05_anim.Play("fahren");        

    if( !boot_06_anim.IsPlaying("fahren") )
        boot_06_anim.Play("fahren");        

}
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------
}
