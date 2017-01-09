using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameWonScript : MonoBehaviour {

	public void ToMainMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

}
