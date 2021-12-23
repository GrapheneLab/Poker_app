using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisMenuBehaivor : MonoBehaviour {

    public GameObject scrollBar;
    public GameObject currentBetLabel;

   // public bool visible = true;

    public float oldValue = 1.0f;
    public int currentStep = 0;
	// Use this for initialization
	void Start () {

        foreach( string s in CLEOS.raise_variants)
        {
            Debug.Log(s);
        }
    }
	
	// Update is called once per frame
	void Update () {
        valueChanged();       
    }
    /*
    void OnBecameInvisible()
    {
        visible = false;
    }

    void OnBecameVisible()
    {
        visible = true;
    }
    */
    public void valueChanged()
    {
        float fOffest = 1.0f / scrollBar.GetComponent<UIScrollBar>().numberOfSteps;
        float Value = ( 1.0f / fOffest ) * scrollBar.GetComponent<UIScrollBar>().value;
        int step = scrollBar.GetComponent<UIScrollBar>().numberOfSteps - (int)Math.Round(Value);

        if (CLEOS.raise_variants.Count != 0)
        {
            currentBetLabel.GetComponent<UILabel>().text = CLEOS.raise_variants[step];
            CLEOS.raised_bet = CLEOS.raise_variants[step];
        }              
    }
}
