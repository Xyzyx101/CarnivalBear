using UnityEngine;
using System.Collections;

public class ShockTrigger : MonoBehaviour {
    private Robot MyRobot;

	void Start () {
        MyRobot = transform.parent.GetComponent<Robot>();
	}

    void OnTriggerEnter(Collider other)
    {
        MyRobot.ShockTarget();
    }
}
