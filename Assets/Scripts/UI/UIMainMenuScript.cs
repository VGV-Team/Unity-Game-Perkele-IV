using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenuScript : MonoBehaviour
{

	public GameObject VolumeSlider;
	public GameObject LoadingSlider;

	// Update is called once per frame
	void Update ()
	{
		AudioListener.volume = VolumeSlider.GetComponent<Slider>().value;
	}

	public void ExitApplication()
	{
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}

	private AsyncOperation async;

	IEnumerator LoadLevelWithBar ()
	{
		async = SceneManager.LoadSceneAsync("MainScene");
		while (!async.isDone)
		{
			LoadingSlider.GetComponent<Slider>().value = async.progress;
			yield return null;
		}
	}
	

	public void StartGame()
	{
		LoadingSlider.GetComponent<Slider>().value = 0;
		StartCoroutine(LoadLevelWithBar());
	}
}
