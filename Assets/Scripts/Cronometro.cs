using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cronometro : MonoBehaviour
{
	public Text tempoText;
	float tempoInicio;
	float Timer_;

	// Use this for initialization
	void Start ()
	{
		
		tempoInicio = Time.time;
	}
		
	void Update ()
	{
		Tempo_Do_Jogo ();
	}

	void Tempo_Do_Jogo() // Função que calcula o tempo do jogo
	{
		Timer_ = Time.time - tempoInicio;

		float min = Timer_ / 120;
		float sec = Timer_ % 60;
		//float milisec = (Timer_ * 100) % 100;
		tempoText.text = "Tempo: " + min.ToString ("00") + ":" + sec.ToString ("00");
	}
}
