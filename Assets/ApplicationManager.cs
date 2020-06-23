using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;

public class ApplicationManager : MonoBehaviour {

	static public int LABIRINTO_LIVRE = 0;
	static public int LABIRINTO_TESTE = 1;
	static public int LABIRINTO_FASE = 2;
	static public int HC_FASE1 = 3;
	static public int HC_FASE2 = 4;
	static public int HC_FASE3 = 5;
	static public int HC_FASE4 = 6;

	static public int currentLevel = HC_FASE2;

	static public long tempo = 0;

	static public string nome = "";

	static public PontuacaoFase[] pontuacao = new PontuacaoFase [10];

	static public int acertos = 0;

	static public int erros = 0;

	static public string errosLista = "";


	//HC3
	static public int acertoLivro = 0;
	static public int acertoEnvelope = 0;

	static public int errosLivro = 0;
	static public int errosEnvelope = 0;

	static public void start(){

		ApplicationManager.tempo = Environment.TickCount;
		ApplicationManager.acertos = 0;
		ApplicationManager.erros = 0;
		ApplicationManager.errosLista = "";
		ApplicationManager.acertoLivro = 0;
		ApplicationManager.acertoEnvelope = 0;
	}

	static public void salvarLabirinto(){
		
		string path = Application.persistentDataPath + "/Resultado_Labirinto_" + ApplicationManager.nome + "_" + DateTime.Now.ToString ("dd-MM-yyyy") + ".csv";
		double tempo = ( (double) Environment.TickCount - ApplicationManager.tempo) / 1000D;

		if (!File.Exists (path))			
			File.WriteAllText (path, "Hora,Tempo (seg),Acertos,Erros,Locais erros" + Environment.NewLine);

		File.AppendAllText(path, DateTime.Now.ToString("HH:mm")+","+tempo+","+(acertos-1)+","+erros+","+errosLista + Environment.NewLine);
	}

	static public void salvarHC2(string sequencia){

		string path = Application.persistentDataPath + "/Resultado_HC2_" + ApplicationManager.nome + "_" + DateTime.Now.ToString ("dd-MM-yyyy") + ".csv";
		double tempo = ( (double) Environment.TickCount - ApplicationManager.tempo) / 1000D;

		if (!File.Exists (path))			
			File.WriteAllText (path, "Hora,Tempo (seg),Sequencia" + Environment.NewLine);

		File.AppendAllText(path, DateTime.Now.ToString("HH:mm")+","+tempo+","+sequencia + Environment.NewLine);
	}

	static public void salvarHC4(string sequencia){

		string path = Application.persistentDataPath + "/Resultado_HC4_" + ApplicationManager.nome + "_" + DateTime.Now.ToString ("dd-MM-yyyy") + ".csv";
		double tempo = ( (double) Environment.TickCount - ApplicationManager.tempo) / 1000D;

		if (!File.Exists (path))			
			File.WriteAllText (path, "Hora,Tempo (seg),Sequencia" + Environment.NewLine);

		File.AppendAllText(path, DateTime.Now.ToString("HH:mm")+","+tempo+","+sequencia + Environment.NewLine);
	}

	static public void salvarHC3(){

		string path = Application.persistentDataPath + "/Resultado_HC3_" + ApplicationManager.nome + "_" + DateTime.Now.ToString ("dd-MM-yyyy") + ".csv";
		double tempo = ( (double) Environment.TickCount - ApplicationManager.tempo) / 1000D;

		if (!File.Exists (path))			
			File.WriteAllText (path, "Hora,Tempo (seg),Livro acerto,Envelope acerto,Erros livro, Erros envelope" + Environment.NewLine);

		File.AppendAllText(path, DateTime.Now.ToString("HH:mm")+","+tempo+","+acertoLivro+","+acertoEnvelope+","+errosLivro+","+errosEnvelope + Environment.NewLine);
	}

	static public IEnumerator CarregaMenu(float time = 5) {		
		yield return new WaitForSeconds(time);
		SceneManager.LoadScene("0.Menu", LoadSceneMode.Single);
	}



}
