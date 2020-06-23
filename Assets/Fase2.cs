using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;

public class Fase2 : MonoBehaviour {

	public AudioClip introClip;
	public AudioClip successClip;
	public AudioClip failClip;
	public AudioClip finishClip;
	public AudioClip gameOverClip;

	public GameObject wrongWay;
	public GameObject gameOver;

	public GameObject led1;
	public GameObject led2;
	public GameObject led3;

	private int colliderCount = 1;
	private string lastCollider = "-1";
	private string currentCollider = "";

	private Stack<Vector3> path = new Stack<Vector3>();
	private Stack<Quaternion> pathRotation = new Stack<Quaternion>();

	private bool failed = false;
	private bool ignoreInput = false;
	private bool finish = false;

	private bool fase2 = false;

	private HashSet<int> jaFoi = new HashSet<int> ();
	private string sequencia = "";


	void Start () {	

		//Esse script só funcionara para a fase 2
		if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE2) || 
				ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE4)) {	

			ApplicationManager.start ();

			AudioSource.PlayClipAtPoint (introClip, transform.position);

			fase2 = true;
			GameObject.Find ("Secretaria").SetActive (false);
		}			
	}


	void OnTriggerEnter(Collider collider) {		
		
		//sucesso
		/*
		if (fase2 && collider.gameObject.name.Equals (colliderCount.ToString ()))
			Success ();		
		else if(fase2 && !collider.gameObject.name.Equals (lastCollider.ToString()))
			Fail ();		
			*/

		// FASE 1
		if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE1)){
			if( collider.gameObject.name.Equals("1") || collider.gameObject.name.Equals("2") || collider.gameObject.name.Equals("3") || 
				collider.gameObject.name.Equals("4") || collider.gameObject.name.Equals("5") )
				AudioSource.PlayClipAtPoint (successClip, transform.position);
		}


		// FASE 2 e 4
		if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE2) ||
		    ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE4)) {	

			try {
				int.Parse (collider.gameObject.name);
				currentCollider = collider.gameObject.name;
				Success ();	
			} catch (System.Exception e) {
			}
		}
		//SUCESS PARA OS 2 por causa:
		/**
		 * pediram pra nao diferenciar o som de erro
		 * pediram pra salvar a seguencia dos lugares.. e nao mais acertos e erros.
		 * dessa forma, é o mesmo para acerto ou erro
		 * */
	}

	void FixedUpdate () {

		if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE2) ||
		    ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE4)) {	

			if (Input.GetButtonDown ("Fire2") || Input.GetButtonDown ("Fire3") || Input.GetMouseButtonDown(1)) {

				if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE2))
					ApplicationManager.salvarHC2 (sequencia);
				else
					ApplicationManager.salvarHC4 (sequencia);


				AudioSource.PlayClipAtPoint (finishClip, transform.position);

				//Menu
				StartCoroutine (ApplicationManager.CarregaMenu ());

			}
		}
	}
	
	private void Success(){			
		
		ApplicationManager.acertos++;


		Debug.Log ("last "+ lastCollider);
		Debug.Log ("current "+ currentCollider);
		//So vamos contabilizar se o collider for outro, evitando que o cara volte
		if (lastCollider != currentCollider) {
			sequencia += currentCollider + ",";
			AudioSource.PlayClipAtPoint (successClip, transform.position);
		}


		Debug.Log (sequencia);


		// se acertou a sequencia - saimos
		if(sequencia.Equals("1,2,3,4,5,")){

			sequencia += currentCollider;

			if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE2))
				ApplicationManager.salvarHC2 (sequencia);
			else
				ApplicationManager.salvarHC4 (sequencia);
			

			AudioSource.PlayClipAtPoint (finishClip, transform.position);

			//Menu
			StartCoroutine (ApplicationManager.CarregaMenu ());
		} 

		lastCollider = currentCollider;

		colliderCount++;
	}

	//Tratando o erro de caminho
	private void Fail(){


		//sequencia += "," + colliderCount;
		ApplicationManager.erros++;

		AudioSource.PlayClipAtPoint (failClip, transform.position);
		//AudioSource.PlayClipAtPoint (successClip, transform.position);

		/*if (! jaFoi.Contains (lastCollider)) {
			jaFoi.Add (lastCollider);		
			colliderCount++;
		}*/
	}


	private void GameOver(){
		
		AudioSource.PlayClipAtPoint (gameOverClip, transform.position);
		finish = true;	
	}

}
