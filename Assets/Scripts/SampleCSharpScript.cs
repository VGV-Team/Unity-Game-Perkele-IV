using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCSharpScript : MonoBehaviour {
    public Button yourButton;

    // Use this for initialization
    void Start ()
    {
        //yourButton.GetComponent<Button>().onClick.AddListener(OnClick);

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnClick()
    {
        print("qwe");
    }

}
