using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRotationCamera : MonoBehaviour {

	public GameObject MainCamera;

	private float rotateX;
	private float rotateY;
	private float rotateZ;

	public bool ligado;
	public float altura = -106663f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(ligado){
		transform.position = new Vector3 (transform.position.x, altura, transform.position.z);
		//rotateX = MainCamera.transform.rotation.eulerAngles.x;
		//rotateY = MainCamera.transform.rotation.eulerAngles.x;
		//rotateZ = MainCamera.transform.rotation.eulerAngles.y;

		//this.transform.eulerAngles = new Vector3 (rotateX, rotateY, rotateZ);
		}
	}
}
