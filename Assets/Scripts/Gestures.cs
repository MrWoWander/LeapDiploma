﻿using System.Collections;
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
    public float waitTimer;

    bool activeTimer = false;
    float timer;
    int gesturesValue;
    GameObject gestures;
    float testValue = 0.0f;

	void Start () {
        slider.value = 0;
        slider.gameObject.SetActive(false);
        textComplited.gameObject.SetActive(false);
        gestures = gesturesArr[gesturesValue].gestures;
        letter.text = gesturesArr[gesturesValue].name;

    }


    void Update () {
        if (slider.gameObject.activeSelf == true)
        {
            testValue += Time.deltaTime;
            slider.value = testValue;
        }

        if (slider.value == 1)
        {
            GesturesTrue();
        }

        if (activeTimer)
        {
            timer += Time.deltaTime;
            if (timer >= waitTimer)
            {
             
                activeTimer = false;
                textComplited.gameObject.SetActive(false);
                NextGestures();
                timer = 0;
            }
        }

    }

    void NextGestures()
    {
        gestures.SetActive(false);
        gesturesValue += 1;
        if (gesturesValue < gesturesArr.Length)
        {
            gestures = gesturesArr[gesturesValue].gestures;
            letter.text = gesturesArr[gesturesValue].name;
            gestures.SetActive(true);
        } else {
            letter.text = "WIN!";
        }
        testValue = 0;
    }

    public void GesturesFalse()
    {
        if (activeTimer)
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
        activeTimer = true;
        slider.value = 0;
        slider.gameObject.SetActive(false);
        textComplited.gameObject.SetActive(true);
        textComplited.color = Color.green;
        textComplited.text = "Complited";
        
    }

    public void ActiveGesture ()
    {
        if (!activeTimer)
        {
            slider.gameObject.SetActive(true);
            textComplited.gameObject.SetActive(false);
        }
    }
}
