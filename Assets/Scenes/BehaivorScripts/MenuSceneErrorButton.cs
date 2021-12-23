using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneErrorButton : MonoBehaviour
{

    public GameObject ControlWidget;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        Debug.Log("UICardsShowBehaivor OnClick( )");

        UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
        foreach (UITweener tw in tweens)
        {
            if (tw.tweenGroup == 0)
                tw.Play(false);
        }
    }
}

