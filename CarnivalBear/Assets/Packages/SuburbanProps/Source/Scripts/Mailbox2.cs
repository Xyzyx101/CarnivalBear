using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Mailbox2 : MonoBehaviour {

	public string houseNumber;

	void Start () {
		foreach ( Text text in GetComponentsInChildren<Text>()) {
			text.text = houseNumber;
		}
		//Destroy(this);
	}
}
