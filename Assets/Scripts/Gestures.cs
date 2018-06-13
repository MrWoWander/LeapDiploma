using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Gesture
{
    public GameObject gestures;
    public string name;
    
}

public class Gestures : MonoBehaviour {



    public Slider slider;
    public Text letter;
    public Text textComplited;
    public Gesture[] gesturesArr;
   

    int gesturesValue;
    GameObject gestures;
    float testValue = 0.0f;

	// Use this for initialization
	void Start () {
        slider.value = 0;
        slider.gameObject.SetActive(false);
        textComplited.gameObject.SetActive(false);
        gestures = gesturesArr[gesturesValue].gestures;
        letter.text = gesturesArr[gesturesValue].name;


    }
	
	// Update is called once per frame
	void Update () {
        if (slider.gameObject.activeSelf == true)
        {
            testValue += 1 * Time.deltaTime;
            slider.value = testValue;
        }

        if (slider.value == 1)
        {
            GesturesTrue();
        }

    }

    void NextGestures()
    {
        gestures.SetActive(false);
        gesturesValue += 1;
        gestures = gesturesArr[gesturesValue].gestures;
        letter.text = gesturesArr[gesturesValue].name;
        gestures.SetActive(true);
        slider.value = 0;
        testValue = 0;
    }

    public void GesturesFalse()
    {
        if (slider.value == 1)
        {
            return;
        } else {
            slider.value = 0;
            testValue = 0;
            slider.gameObject.SetActive(false);
            textComplited.gameObject.SetActive(true);
            textComplited.color = Color.red;
            textComplited.text = "False";
        }
    }

    public void GesturesTrue ()
    {
        slider.gameObject.SetActive(false);
        textComplited.gameObject.SetActive(true);
        textComplited.color = Color.green;
        textComplited.text = "Complited";
        NextGestures();
    }

    public void A ()
    {
        slider.gameObject.SetActive(true);
    }

    public void B ()
    {
        slider.gameObject.SetActive(true);
    }
    public void C ()
    {
        slider.gameObject.SetActive(true);
        textComplited.gameObject.SetActive(false);
    }

    public void D()
    {
        slider.gameObject.SetActive(true);
    }
    public void I()
    {
        slider.gameObject.SetActive(true);
    }
    public void R()
    {
        slider.gameObject.SetActive(true);
    }
    public void II()
    {
        slider.gameObject.SetActive(true);
    }
}
