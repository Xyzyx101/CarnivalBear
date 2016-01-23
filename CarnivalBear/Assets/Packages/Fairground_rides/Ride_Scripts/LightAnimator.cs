using UnityEngine;
using System.Collections;

public class LightAnimator : MonoBehaviour 
{
    public float     speed;
    public Material  material;
    public Texture[] textures;
    
    float frameTime;
    
	void Update() 
    {        
        int numTextures = textures.Length;
        
        if (numTextures == 0)
            return;
            
        frameTime += Time.deltaTime * speed;
        material.mainTexture = textures[(int)Mathf.Abs(frameTime) % numTextures];
        material.SetTexture("_Illum", textures[(int)Mathf.Abs(frameTime) % numTextures]);

	}
}

