using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase1 : MonoBehaviour {

	public Transform character;
	public GameObject secretaria;

	public AudioClip audioInicial;
	public AudioClip primeiraParada;
	public AudioClip segundaParada;
	public AudioClip terceiraParada;
	public AudioClip quartaParada;
	public AudioClip ultimaParada;
	//public AudioClip audioVenhaComigo;

	public AudioClip finishClip;
	public AudioSource audioSource;

	public Transform[] path_0;
	public Transform[] path_1;
	public Transform[] path_2;
	public Transform[] path_3;
	public Transform[] path_4;


	private bool waiting = false;
	private int nextPath = 0;

	private Animator anim;

	void OnDrawGizmos(){
		iTween.DrawPath(path_0,Color.yellow);
		iTween.DrawPath(path_1,Color.blue);
		iTween.DrawPath(path_2,Color.green);	
		iTween.DrawPath(path_3,Color.red);
		iTween.DrawPath(path_4,Color.magenta);
	}	


	void Start(){
		anim = GetComponent<Animator> ();

		//Esse script só funcionara para a fase 1
		if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE1)) {			

			ApplicationManager.start ();

			audioSource.clip = audioInicial;
			audioSource.Play();

			waiting = false;
			nextPath = 0;

			StartCoroutine (DelayInicial (20));
		}

	}

	void Update(){
		DetectKeys();

		float distance = Vector3.Distance (secretaria.transform.position, character.transform.position);

		if (distance < 10f && waiting) {			
			next ();
		}
		
	}


	void OnTriggerEnter(Collider collider) {		
		
		if (waiting) {
			if (collider.gameObject.name.Equals ("FPSController")) {
				next ();
			}
		}
	}

	/*
	void OnCollisionStay(Collision collisionInfo) {
		Debug.Log ("OnCollisionStay");
		if (waiting) {
			if (collisionInfo.gameObject.name.Equals ("FPSController")) {
				next ();
			}
		}

	}*/


	void DetectKeys(){

		if(Input.GetKeyDown(KeyCode.P)){

			anim.SetBool("idle", true);
			anim.SetBool("idle", false);
			anim.SetBool("point", true);
			anim.SetBool("point", false);
			GameObject.Find ("envelope").GetComponent<Renderer> ().enabled = true;
			anim.SetBool("walk", true);
		}

	}

	IEnumerator Animacao(string value, float time) {		
		yield return new WaitForSeconds(time);
		anim.SetBool("idle", false);
		anim.SetBool("walk", false);
		anim.SetBool("point", false);

		anim.SetBool(value, true);
		//anim.SetTrigger (value);
	}


	IEnumerator gotoPath(Transform[] path, string complete, float time, float pathTime) {		
		yield return new WaitForSeconds(time);

		//iTween.PutOnPath (secretaria, path_2, 0f);
		iTween.MoveTo (secretaria, iTween.Hash (
			"path", path,
			"movetopath", false, //segredo foda - evita os problemas/trepidacao de entrar no path
			//"islocal",true,
			"time", pathTime, //ou speed
			"orienttopath", true,
			"lookahead", 0.05,
			"looktime", 0.05,
			"looptype", iTween.LoopType.none,
			"easetype","linear",
			"oncomplete", complete));
	}

	IEnumerator DelayInicial(float time) {		
		yield return new WaitForSeconds(time);
		waiting = true;
		next ();
	}

	private void next(){

		waiting = false;

		switch (nextPath) {

		case 0: //Inicio -> Mesa
			{	
				iTween.PutOnPath (secretaria, path_0, 0f);

				anim.SetBool("walk", true);

				//StartCoroutine (Animacao ("walk",0));
				StartCoroutine (gotoPath (path_0, "completePath0", 0.5f, 8));


				break;
			}

		case 1: //Mesa -> Jornaleiro
			{				
				audioSource.clip = primeiraParada;
				audioSource.Play();

				iTween.PutOnPath (secretaria, path_1, 0f);

				StartCoroutine (Animacao ("walk",38));

				StartCoroutine (gotoPath (path_1, "completePath1", 0.5f+38, 22));


				break;
			}
		case 2: //Jornaleiro -> Cantina
			{
				audioSource.clip = segundaParada;
				audioSource.Play();

				StartCoroutine (Animacao ("point",4));
				GameObject.Find ("envelope").GetComponent<Renderer> ().enabled = true;

				StartCoroutine (Animacao ("walk",30));
				StartCoroutine (gotoPath (path_2, "completePath2", 2+30, 40));
				break;
			}

		case 3: //Cantina -> Mesa
			{
				audioSource.clip = terceiraParada;
				audioSource.Play();

				StartCoroutine (Animacao ("walk",1.5f+22f));
				StartCoroutine (gotoPath (path_3, "completePath3", 2+22f, 25));

				break;
			}		
		case 4: //Mesa -> Centro de Estudos
			{
				audioSource.clip = quartaParada;
				audioSource.Play();
				
				StartCoroutine (Animacao ("point",4));
				GameObject.Find ("livro").GetComponent<Renderer> ().enabled = true;
				StartCoroutine (Animacao ("walk",2.4f+22));
				StartCoroutine (gotoPath (path_4, "completePath4", 3+22, 32));

				break;
			}
		}
	}


	// -> Mesa
	void completePath0(){	
		
		//iTween.RotateTo(secretaria, iTween.Hash("path" , path_2, "time" , 0, "easeType", iTween.EaseType.easeInOutSine));
		//iTween.PutOnPath (secretaria, path_1, 0f);
		StartCoroutine (Animacao ("idle",0));

		waiting = true;
		nextPath = 1;
	}

	//Mesa -> Jornaleiro
	void completePath1(){	
		//iTween.RotateTo(secretaria, iTween.Hash("path" , path_2, "time" , 0, "easeType", iTween.EaseType.easeInOutSine));
		iTween.PutOnPath (secretaria, path_2, 0f);
		StartCoroutine (Animacao ("idle",0));

		waiting = true;
		nextPath = 2;
	}

	//Jornaleiro -> Cantina
	void completePath2(){
		iTween.PutOnPath (secretaria, path_3, 0f);
		StartCoroutine (Animacao ("idle",0));
		waiting = true;
		nextPath = 3;
	}

	//Cantina -> Mesa
	void completePath3(){
		iTween.PutOnPath (secretaria, path_4, 0f);
		//iTween.RotateTo(secretaria, iTween.Hash("path" , path_4, "time" , 5, "easeType", iTween.EaseType.easeInOutSine));

		StartCoroutine (Animacao ("idle",0));
		waiting = true;
		nextPath = 4;
	}

	//Mesa -> Centro de Estudos
	void completePath4(){

		StartCoroutine (Animacao ("idle",0));

		audioSource.clip = ultimaParada;
		audioSource.Play();


		//AudioSource.PlayClipAtPoint (finishClip, character.position);

		//Menu
		StartCoroutine (ApplicationManager.CarregaMenu (25));
	}


}
