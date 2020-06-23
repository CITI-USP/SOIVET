using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour {

	public GameObject Menu;

	void Start () {
		
		//Debug.Log (ApplicationManager.nome);

		if (ApplicationManager.nome.Length >= 3) {
			GameObject.Find ("0.Keyboard").SetActive (false);
			Menu.SetActive (true);
		} else {
			GameObject.Find ("0.Keyboard").SetActive (true);
			Menu.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {


		if (ApplicationManager.nome.Length >= 3) {
			GameObject.Find ("0.Keyboard").SetActive (false);
			Menu.SetActive (true);
		} else {
			GameObject.Find ("0.Keyboard").SetActive (true);
			Menu.SetActive (false);
		}

	}
}
