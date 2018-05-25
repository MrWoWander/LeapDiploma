using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alphabet : MonoBehaviour {

    public Text textUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void A ()
    {
        textUI.text = "А";
    }

    public void B ()
    {
        textUI.text = "Б";
    }
    public void C ()
    {
        textUI.text = "В";
    }

    public void D()
    {
        textUI.text = "Г";
    }
    public void I()
    {
        textUI.text = "И";
    }
    public void R()
    {
        textUI.text = "Р";
    }
    public void II()
    {
        textUI.text = "Я";
    }
}
