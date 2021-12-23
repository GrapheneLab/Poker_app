using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chose : MonoBehaviour {

    public GameObject scene;
    public GameObject auto_re_buy;

    public GameObject LoadingCoin;

    // Use this for initialization
    void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        double sb = scene.GetComponent<ChoseTableBehaivor>().small_blind;
        double bb = scene.GetComponent<ChoseTableBehaivor>().big_blind;
        int pc = scene.GetComponent<ChoseTableBehaivor>().players_count;

        CLEOS.auto_re_buy = auto_re_buy.GetComponent<UIToggle>().value;

        if (!(sb > 0.0) || !(bb > 0.0) || !(pc > 0))
        {
            GameObject Label = GameObject.Find("Message");
            Label.GetComponent<UILabel>().text = " You must chose small blind and players count before the game start!";
            GameObject ControlWidget = GameObject.Find("AlertWindow");
            UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
        }
        else
        {
         //   SceneManager.UnloadScene(SceneManager.GetActiveScene());
            

            LoadingCoin.SetActive(true);
            UITweener[] t1 = LoadingCoin.GetComponents<UITweener>();
            foreach (UITweener tw in t1)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }

            gameObject.SetActive(false);

            SceneManager.LoadSceneAsync("LoadingScene");
        }
        //Debug.Log("GO!");
    }
}
