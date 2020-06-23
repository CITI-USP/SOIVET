using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Input.gyro.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {

        Camera.main.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);

    }
}
