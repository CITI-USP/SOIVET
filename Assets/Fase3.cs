using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Fase3 : MonoBehaviour {

	public Transform character;

	public GameObject GUIReticle;
	public GameObject MainCamera;
	public GameObject Secretaria;

	public FirstPersonController firstPersonController;

	public AudioClip audioLivro;
	public AudioClip audioEnvelope;

	public AudioClip successClip;
	public AudioClip failClip;
	public AudioClip finishClip;
	public AudioSource audio;

	private bool livroFound = false;
	private bool envelopeFound = false;

	[SerializeField] private float m_RayLength = 500f;

	// Use this for initialization
	void Start () {
		


		//Esse script só funcionara para a fase 2
		if (ApplicationManager.currentLevel.Equals (ApplicationManager.HC_FASE3)) {		
		
			ApplicationManager.start ();

			firstPersonController.canWalk = false;

			//desabilitando o collider da secretaria para o raycast pegar o collider do livro (que fica atras dela)
			Secretaria.GetComponent<SphereCollider> ().enabled = false;
			GameObject.Find("envelopeCollider").GetComponent<BoxCollider> ().enabled = true;
			GameObject.Find("livroCollider").GetComponent<BoxCollider> ().enabled = true;

			GUIReticle.SetActive (true);

			audio.clip = audioLivro;
			audio.Play ();

			GameObject.Find ("4-fase3").SetActive (true);
			//Pediram pra nao exibir mais o livro e o envelope
			//GameObject.Find ("livro").GetComponent<Renderer> ().enabled = true;
			//GameObject.Find ("envelope").GetComponent<Renderer> ().enabled = true;			
		} else {
			GameObject.Find ("4-fase3").SetActive (false);
		}

	}



	void FixedUpdate(){


		//GameObject.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = target;
		//Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.forward * m_RayLength, Color.blue, 1f);

		if (Input.GetButtonDown ("0") || Input.GetButtonDown ("1") || Input.GetMouseButtonDown (0)) {

			//AudioSource.PlayClipAtPoint (failClip, character.position);

			string target = "";
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hit;

			// Do the raycast forweards to see if we hit an interactive item
			if (Physics.Raycast (ray, out hit, m_RayLength)) {
				target = hit.collider.name;
			}

			//Debug.Log (target);

		
			if (!livroFound && target.Equals("livroCollider")) {			

				ApplicationManager.acertoLivro++;
				livroFound = true;
				Success ();
					
				GameObject.Find ("livro").GetComponent<Renderer> ().enabled = false;

				if (!envelopeFound) {
					audio.clip = audioEnvelope;
					audio.Play ();
				}
			}

			if (!envelopeFound && target.Equals("envelopeCollider")) {			

				//erro, o livro precisa ser primeiro!
				if (!livroFound) {
					ApplicationManager.errosLivro++;
					AudioSource.PlayClipAtPoint (failClip, character.position);
					return;
				}

				ApplicationManager.acertoEnvelope++;
				envelopeFound = true;
				Success ();

				GameObject.Find ("envelope").GetComponent<Renderer> ().enabled = false;

				if (!livroFound) {
					audio.clip = audioLivro;
					audio.Play ();
				}
			}
		

			if (envelopeFound == true && livroFound == true){
				finish ();
			}else if( livroFound && !target.Equals("envelopeCollider") && !target.Equals("livroCollider")){ // ERROU
				ApplicationManager.errosEnvelope++;
			}else if( !livroFound && !target.Equals("envelopeCollider") && !target.Equals("livroCollider")){ // ERROU
				ApplicationManager.errosLivro++;
			}

		}
	}

	void finish(){


		ApplicationManager.salvarHC3 ();

		AudioSource.PlayClipAtPoint (finishClip, character.position);

		//Menu
		StartCoroutine (ApplicationManager.CarregaMenu ());
	}

	void OnTriggerEnter(Collider collider) {				

		/*
		//Envelope
		if (waiting && collider.gameObject.name.Equals ("2")) {
			collider.gameObject.SetActive (false);
			Success ();
		}


		//Livro
		if (waiting && collider.gameObject.name.Equals ("4-fase3")) {
			collider.gameObject.SetActive (false);
			Success ();

			audio.clip = audioEnvelope;
			audio.Play ();
		}*/

	}

	private void Success(){					

		AudioSource.PlayClipAtPoint (successClip, character.position);
		/*
		colliderCount++;

		if (colliderCount == 2) {
			AudioSource.PlayClipAtPoint (finishClip, character.position);

			//Menu
			StartCoroutine (ApplicationManager.CarregaMenu ());
		} else {
			AudioSource.PlayClipAtPoint (successClip, character.position);
		}
		*/
	}

}
