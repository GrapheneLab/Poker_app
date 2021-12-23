using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanelBehaivor : MonoBehaviour {

    public GameObject masterObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        masterObject.GetComponent<UICardsShowBehaivor>().ShowCombination();

    }
}
