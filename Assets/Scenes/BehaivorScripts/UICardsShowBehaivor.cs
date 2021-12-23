using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardsShowBehaivor : MonoBehaviour {

    public GameObject ControlWidget;

    public GameObject Combination0;
    public GameObject Combination1;
    public GameObject Combination2;
    public GameObject Combination3;
    public GameObject Combination4;
    public GameObject Combination5;
    public GameObject Combination6;
    public GameObject Combination7;
    public GameObject Combination8;
    public GameObject Combination9;

    public GameObject ScrollView;

    public bool int_enabled = false;

    public int last_combitation = -1;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void animate( int index, bool animate )
    {
        bool reset = false;
        if(last_combitation != CombinationPlugin.current_combination)
        {
            last_combitation = CombinationPlugin.current_combination;
            reset = true;
        }
        switch (index )
        {           
            case 0:
                {
                    if (!reset)
                        break;

                    {
                        Combination0.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination0.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination0.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination0.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination0.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination0.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }
                    {
                        Combination1.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination1.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination1.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination1.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination1.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination1.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination2.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination2.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination2.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination2.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination2.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination2.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination3.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination3.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination3.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination3.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination3.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination3.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination4.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination4.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination4.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination4.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination4.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination4.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination5.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination5.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination5.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination5.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination5.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination5.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination6.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination6.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination6.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination6.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination6.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination6.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination7.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination7.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination7.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination7.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination7.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination7.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination8.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination8.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination8.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination8.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination8.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination8.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }

                    {
                        Combination9.GetComponentInChildren<UICenterOnChild>().enabled = false;

                        GameObject c0 = Combination9.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().color = c0.GetComponent<TweenColor>().from;
                        c0.GetComponent<TweenColor>().enabled = false;
                        GameObject c1 = Combination9.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().color = c1.GetComponent<TweenColor>().from;
                        c1.GetComponent<TweenColor>().enabled = false;
                        GameObject c2 = Combination9.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().color = c2.GetComponent<TweenColor>().from;
                        c2.GetComponent<TweenColor>().enabled = false;
                        GameObject c3 = Combination9.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().color = c3.GetComponent<TweenColor>().from;
                        c3.GetComponent<TweenColor>().enabled = false;
                        GameObject c4 = Combination9.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().color = c4.GetComponent<TweenColor>().from;
                        c4.GetComponent<TweenColor>().enabled = false;
                    }
                }
                break;
            case 1:
                {
                    if (Combination0.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination0.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination0.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination0.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination0.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination0.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination0.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination0.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination0.transform);
                    }
                }
                break;

            case 2:
                {
                    if (Combination1.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination1.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination1.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination1.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination1.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination1.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination1.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination1.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination1.transform);
                    }
                }
                break;

            case 3:
                {
                    if (Combination2.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination2.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination2.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination2.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination2.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination2.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination2.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination2.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination2.transform);
                    }
                }
                break;

            case 4:
                {
                    if (Combination3.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination3.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination3.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination3.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination3.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination3.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination3.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination3.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination3.transform);
                    }
                }
                break;

            case 5:
                {
                    if (Combination4.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination4.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination4.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination4.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination4.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination4.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination4.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination4.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination4.transform);
                    }
                }
                break;

            case 6:
                {
                    if (Combination5.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination5.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination5.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination5.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination5.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination5.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination5.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination5.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination5.transform);
                    }
                }
                break;

            case 7:
                {
                    if (Combination6.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination6.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination6.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination6.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination6.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination6.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination6.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination6.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination6.transform);
                    }
                }
                break;

            case 8:
                {
                    if (Combination7.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination7.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination7.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination7.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination7.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination7.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination7.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination7.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination7.transform);
                    }
                }
                break;

            case 9:
                {
                    if (Combination8.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination8.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination8.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination8.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination8.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination8.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination8.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination8.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination8.transform);
                    }
                }
                break;

            case 10:
                {
                    if (Combination9.GetComponentInChildren<UICenterOnChild>().enabled != animate)
                    {
                        GameObject c0 = Combination9.transform.GetChild(0).gameObject;
                        c0.GetComponent<TweenColor>().enabled = true;
                        GameObject c1 = Combination9.transform.GetChild(1).gameObject;
                        c1.GetComponent<TweenColor>().enabled = true;
                        GameObject c2 = Combination9.transform.GetChild(2).gameObject;
                        c2.GetComponent<TweenColor>().enabled = true;
                        GameObject c3 = Combination9.transform.GetChild(3).gameObject;
                        c3.GetComponent<TweenColor>().enabled = true;
                        GameObject c4 = Combination9.transform.GetChild(4).gameObject;
                        c4.GetComponent<TweenColor>().enabled = true;

                        Combination9.GetComponentInChildren<UICenterOnChild>().enabled = animate;
                        UICenterOnChild centerOnChild = Combination9.GetComponentInChildren<UICenterOnChild>();
                        centerOnChild.CenterOn(Combination9.transform);
                    }
                }
                break;
        }
    }

    enum CombinationTypes
    {
        C_NO_COMBINATION = 0,
        C_HIGH_CARD,       //9
        C_PAIR,            //8
        C_TWO_PAIRS,       //7
        C_THREE_OF_A_KIND, //6
        C_STRAIGHT,        //5
        C_FLUSH,           //4
        C_FULL_HOUSE,      //3
        C_FOUR_OF_A_KIND,  //2
        C_STRAIGHT_FLUSH,  //1
        C_ROYAL_FLUSH      //10
    };

    public void ShowCombination()
    {
        switch (CombinationPlugin.current_combination)
        {
            // case 0:
            //     animate(0, false);
            //     break;

            case 1:
                animate(0, false);
                animate(9, true);
                break;

            case 2:
                animate(0, false);
                animate(8, true);
                break;

            case 3:
                animate(0, false);
                animate(7, true);
                break;

            case 4:
                animate(0, false);
                animate(6, true);
                break;

            case 5:
                animate(0, false);
                animate(5, true);
                break;

            case 6:
                animate(0, false);
                animate(4, true);
                break;

            case 7:
                animate(0, false);
                animate(3, true);
                break;

            case 8:
                animate(0, false);
                animate(2, true);
                break;

            case 9:
                animate(0, false);
                animate(1, true);
                break;

            case 10:
                animate(0, false);
                animate(10, true);
                break;
        }
    }

    public void OnClick()
    {
        Debug.Log("UICardsShowBehaivor OnClick( )");

        ShowCombination();

        if (int_enabled)
        {
            UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
                else
                    tw.Play(false);
            }           
        }
        else
        {
            UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 1)
                    tw.Play(true);
               else
                    tw.Play(false);
            }
        }
        int_enabled = !int_enabled;

    }
}
