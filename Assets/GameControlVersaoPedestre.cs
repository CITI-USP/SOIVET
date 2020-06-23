using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;
using System;
using System.IO;


public class GameControlVersaoPedestre : MonoBehaviour {

	public AudioClip successClip;
	public AudioClip failClip;
	public AudioClip finishClip;
	public AudioClip gameOverClip;

	public GameObject wrongWay;
	public GameObject gameOver;
	public GameObject win;

	public GameObject led1;
	public GameObject led2;
	public GameObject led3;

	private int lifeCount = 3;

	private int colliderCount = 0;


	private int pathReturnCount = 0;
	private int pathReturnSize = 150;

	private Stack<Vector3> path = new Stack<Vector3>();
	private Stack<Quaternion> pathRotation = new Stack<Quaternion>();

	private bool failed = false;
	private bool ignoreInput = false;
	private bool finish = false;

	//private float speed = 1f;

	private CarAudio carAudio;
	//private CarUserControl carUserControl;
	//private CarController carController;

	private Vector3 velocity = Vector3.zero;

	void OnTriggerEnter(Collider collider) {		

		//Fase 0 nao faz nada
		if (ApplicationManager.currentLevel == 0)
			return;

		//ledCollider
		if (collider.gameObject.name.StartsWith ("ledCollider")) {
			GameObject.Find ("ledPath" + collider.gameObject.name.Substring("ledCollider".Length)).GetComponent<Renderer> ().enabled = true;
		}

		//sucesso
		if (collider.gameObject.name.Equals (colliderCount.ToString())) 
			Success();
		

		//fracasso... perde vida e volta
		if (collider.gameObject.name.StartsWith ("fail")) 			
			Fail (collider.gameObject.name);
		
	}

	private void Success(){

		//SaveEncodedFile ();
		ApplicationManager.acertos++;

        
		try{			
            
			/*for(int i=1 ; i<=33 ;i++){
				if(GameObject.Find ("led" + i) != null)
					GameObject.Find ("led" + i).GetComponent<Renderer> ().enabled = false;
			}*/

			GameObject.Find ("led" + (colliderCount+1)).GetComponent<Renderer> ().enabled = true;

		}catch(System.Exception e){			
			Debug.Log ("ERROR led not found: " + "led" + colliderCount+1);
		}
        

		if( (SceneManager.GetActiveScene().name.Equals("1.Labirinto_fase1") && colliderCount == 4) ||
			(SceneManager.GetActiveScene().name.Equals("2.Labirinto_fase2") && colliderCount == 17) )
		{
			Finish ();
		} else {

			path.Clear ();
			pathRotation.Clear ();

			colliderCount++;

			AudioSource.PlayClipAtPoint (successClip, transform.position);
		}

	}

	//Tratando o erro de caminho
	private void Fail(string colliderName){

		//prevencao para nao entrar duas vezes seguidas
		if (failed)
			return;
		
		//flag da falha
		failed = true;

		//carAudio.StopSound ();
		//Ignorando novos comandos pro carro
		ignoreInput = true;
		//Parando o movimento atual do carro
		//carController.Move (0, 0, 0, 0f);

		AudioSource.PlayClipAtPoint (failClip, transform.position);

		//parando o carro
		//carController.CurrentSpeed = 0f;
		//GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);

		//habilitando o aviso
		wrongWay.SetActive(true);



		//So vamos subtrair vidas se for a fase 2 (que vale pra valer)
		if (SceneManager.GetActiveScene().name.Equals ("1.Labirinto_fase1"))
			return;

		ApplicationManager.erros++;
		ApplicationManager.errosLista += colliderName.Replace("fail ","") + "-";

		lifeCount--;

		if(lifeCount == 2)
			led3.SetActive (false);

		if(lifeCount == 1)
			led2.SetActive (false);

		//game over
		if (lifeCount == 0) {
			led1.SetActive (false);
			GameOver ();
		}

	}

	//Voltando..
	private void Restore(){

		failed = false;

		path.Clear();
		pathRotation.Clear ();

		wrongWay.SetActive(false);	

		//carAudio.StartSound ()

		//Restaurando novos comandos pro carro
		ignoreInput = false;

		//Parando o movimento atual do carro
		//carController.Move (0, 0, 0, 0f);
	}

	private void GameOver(){
		AudioSource.PlayClipAtPoint (gameOverClip, transform.position);
		//carAudio.StopSound ();
		finish = true;
		ignoreInput = true;
		//carController.Move (0, 0, 0, 0f);

		path.Clear ();
		pathRotation.Clear ();

		wrongWay.SetActive(false);
		gameOver.SetActive (true);

		//Salvando
		ApplicationManager.salvarLabirinto ();

		//Menu
		StartCoroutine (ApplicationManager.CarregaMenu ());
	}


	private void Finish(){
		
		AudioSource.PlayClipAtPoint (finishClip, transform.position);

		finish = true;
		ignoreInput = true;
		//carController.Move (0, 0, 0, 0f);

		path.Clear ();
		pathRotation.Clear ();

		wrongWay.SetActive(false);
		gameOver.SetActive (false);
		win.SetActive (true);

		//Salvando - so se for a fase do labirinto de teste!
		if(SceneManager.GetActiveScene().name.Equals("2.Labirinto_fase2"))
			ApplicationManager.salvarLabirinto ();

		//Menu
		StartCoroutine (ApplicationManager.CarregaMenu ());
	}



	// Use this for initialization
	void Start () {	
		
		ApplicationManager.start ();

		//Fase 0 nao faz nada
		if (ApplicationManager.currentLevel == 0) {

			GameObject.Find ("Prancheta").SetActive (false);

		} //else
          //GameObject.Find ("mapaDurex").SetActive (true);


		//apagando os leds que valem pontos
        for (int i = 1; i <= 34; i++)
        {
            if (GameObject.Find("led" + i) != null)
                GameObject.Find("led" + i).GetComponent<Renderer>().enabled = false;
        }

		//apagando os leds do Path
		for (int i = 1; i <= 35; i++)
		{
			if (GameObject.Find("ledPath" + i) != null)
				GameObject.Find("ledPath" + i).GetComponent<Renderer>().enabled = false;
		}



        wrongWay.SetActive(false);
		gameOver.SetActive (false);
		win.SetActive (false);

		//carController = GetComponent<CarController> ();

		//carUserControl = GetComponent<CarUserControl> ();
		//carUserControl.ignoreInput = false;

		//carAudio = GetComponent<CarAudio> ();
		//carAudio.StartSound ();



	}
	
	// Update is called once per frame
	void FixedUpdate () {


		//transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
		//transform.localRotation = Camera.main.transform.localRotation;

		if( Input.GetKeyDown(KeyCode.Q))
			if (ApplicationManager.currentLevel != 0)
				ApplicationManager.salvarLabirinto ();


        //Menu
        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"))
           	StartCoroutine (ApplicationManager.CarregaMenu (0));


		if (finish) {
			//carController.Move (0, 0, 0, 0f);	
			return;
		}

		if (failed) {

			if (path.Count == 0) {
				Restore ();
				return;
			}

            //Debug.Log ("movendo.." + path.Count);
            
            transform.position = Vector3.SmoothDamp(transform.position, path.Pop(), ref velocity, 1f);
			transform.rotation = Quaternion.Slerp (transform.localRotation, pathRotation.Pop (), 1f);

			pathReturnCount++;
			if (pathReturnCount >= pathReturnSize) {
				pathReturnCount = 0;
				path.Clear ();
			}

		} else {			
			//Debug.Log ("add.." + path.Count);
			path.Push (transform.position);
			pathRotation.Push (transform.localRotation);
		}


		/*
		//VERSAO ORIGINAL FUNCIONANDO BEM - pediram pra mudar
		// pass the input to the car!
		//CrossPlatformInputManager.GetButton(string name)
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");

		float v0 = CrossPlatformInputManager.GetButton ("Fire1") ? 1 : 0;
		float v1 = CrossPlatformInputManager.GetButton ("Fire2") ? 1 : 0;
		float v2 = CrossPlatformInputManager.GetButton ("Jump") ? 1 : 0;
		//float v1 = CrossPlatformInputManager.GetButton ("fire1") ? 1 : 0;
		//float v2 = CrossPlatformInputManager.GetButton ("a") ? 1 : 0;
		//float v3 = CrossPlatformInputManager.GetButton ("b") ? 1 : 0;
		//float v4 = CrossPlatformInputManager.GetButton ("x") ? 1 : 0;
		//float v5 = CrossPlatformInputManager.GetButton ("1") ? 1 : 0;

		//botao de acelerador alterantivo
		if (v0 > 0 )//|| v3 > 0 || v4 > 0 || v5 > 0)
			v = 1f;

		//botao de freio alternativo (esse if eh diferente pq o footbrake eh igual ao acelearador
		if (v2 > 0 )//|| v3 > 0 || v4 > 0 || v5 > 0)
			carController.Move (h, 0, v2*-1, 0f);
		else
			if (!ignoreInput) {
				carController.Move (h, v, v, 0f);
			}
			*/

        /*float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");

		//ajuste para o carro virar andando mesmo se o usuario só apertar para o lado
		if (v == 0f)
			v = Math.Abs(h)/2;*/

	}

	public void SaveEncodedFile() {

		string path = Application.persistentDataPath;
		File.WriteAllText(path + "/neuro-labirinto.csv", "1997,Ford,E350,\"ac, abs, moon\",30100.00\n1999,Chevy,\"Venture Extended Edition\",49000.00\n1996,Jeep,Grand Cherokee,\"MUST SELL! air, moon roof, loaded\",479699.00");
	}

	public void LoadEncodedFile()
	{
		string path = Application.persistentDataPath;
		string result = File.ReadAllText(path + "/" + "TempName" + ".txt");
	}

}
