using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cryptography.ECDSA;

using EosSharp;
using EosSharp.Api.v1;

using System;
using Newtonsoft.Json;

using UnityEngine.SceneManagement;

public class UIStartButton : MonoBehaviour {

    public GameObject ControlWidget;
    public GameObject PrivateKeyLabel;

    public GameObject AlertWindow;
    public GameObject AlertMessage;

    public GameObject loadingCoin;

    private bool enabled = false;
    private bool exeption = false;

    //public Eos CLEOS.eos;

    public string public_key;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        Debug.Log("ON CLICK ");

        CLEOS.key = PrivateKeyLabel.GetComponent<UILabel>().text;    
        CLEOS.key.Replace(" ", "");
        try
        {
            var privKeyBytes = EosSharp.Helpers.CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(CLEOS.key);
            public_key = EosSharp.Helpers.CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privKeyBytes, true));
            Debug.Log(public_key);

            if (!enabled)
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
            enabled = !enabled;


            loadingCoin.SetActive(true);
            UITweener[] t1 = loadingCoin.GetComponents<UITweener>();
            foreach (UITweener tw in t1)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }

            gameObject.SetActive(false);

            SceneManager.LoadSceneAsync("NodeChoseScene");
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            Debug.Log(JsonConvert.SerializeObject(e));
            if (AlertMessage != null)
            {
                if (AlertMessage != null)
                    AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;

                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
        }

        catch (EosSharp.Exceptions.ApiException e)
        {
            Debug.Log(JsonConvert.SerializeObject(e));
            if (AlertMessage != null)
            {
                if (AlertMessage != null)
                    AlertMessage.GetComponent<UILabel>().text = e.Content;

                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
        }

        catch (Exception e)
        {
            Debug.Log(e.ToString());
            if (AlertMessage != null)
            {
                if (AlertMessage != null)
                    AlertMessage.GetComponent<UILabel>().text = e.Message;

                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
        }
    }
}
