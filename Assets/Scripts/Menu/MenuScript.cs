using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void ExitProject ()
    {
        print("Bye!");
        Application.Quit();
    }

    public void LoadLevel(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel, LoadSceneMode.Single);
    }
}
