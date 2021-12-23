using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonShowdown : MonoBehaviour
{
    public GameObject left_btn;
    public GameObject right_btn;

    public void Start()
    {
        
    }

    public void OnClick()
    {
        if (left_btn.GetComponent<UICardsShowBehaivor>().int_enabled == true)
        {
            left_btn.GetComponent<UICardsShowBehaivor>().OnClick();
            left_btn.GetComponent<TweenScale>().Play(false);
            left_btn.GetComponent<UICardsShowBehaivor>().int_enabled = false;
        }

        if (right_btn.GetComponent<UIRaiseBehaivor>().int_enabled == true)
        {
            right_btn.GetComponent<UIRaiseBehaivor>().OnClick();
            right_btn.GetComponent<TweenScale>().Play(false);
        }
    }
}
