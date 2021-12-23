using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleDeck : MonoBehaviour
{

    public GameObject deck0;
    public GameObject deck1;
    public GameObject deck2;
    public GameObject deck3;
    public GameObject deck4;
    public GameObject deck5;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(animationLoop());
    }

    // Update is called once per frame
    void Update()
    {



    }

    IEnumerator animationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }
}