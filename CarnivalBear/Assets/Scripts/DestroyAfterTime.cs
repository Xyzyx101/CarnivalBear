using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField]
    float DestroyDelay = 2f;

	void Start () {
	
	}
	
	void Update () {
        DestroyDelay -= Time.deltaTime;
        if(DestroyDelay<0f)
        {
            Destroy(gameObject);
        }
	}
}
