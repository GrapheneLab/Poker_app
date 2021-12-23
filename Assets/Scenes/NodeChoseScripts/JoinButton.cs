using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinButton : MonoBehaviour {

    public GameObject ip_label;

    public GameObject RecoresMenu;
    public GameObject CoinLogo;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        string ip = ip_label.GetComponent<UILabel>().text;
        CLEOS.setup(ip);
 


        CoinLogo.SetActive(true);
        UITweener[] t1 = CoinLogo.GetComponents<UITweener>();
        foreach (UITweener tw in t1)
        {
            if (tw.tweenGroup == 0)
                tw.Play(true);
        }

        RecoresMenu.SetActive(false);
        gameObject.SetActive(false);

        SceneManager.LoadSceneAsync("ChoseTable");
    }
}
