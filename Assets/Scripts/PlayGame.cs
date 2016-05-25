using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour {

	public void Comecar()
	{
		SceneManager.LoadScene ("Jogo");
	}
}
