using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cryptography.ECDSA;

using EosSharp;
using EosSharp.Api.v1;

using System;
using Newtonsoft.Json;

using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class UIRaiseBehaivor : MonoBehaviour {

    public GameObject ControlWidget;
    public Vector3 HidedPosition;
    public Vector3 OriginalPosition;

    public bool int_enabled = true;
    public float speed = 1.0f;

    public GameObject AlertMessage;
    public GameObject AlertWindow;

    //public Eos CLEOS.eos;

    public GameObject caller;
    public GameObject scene;
    public GameObject scrollBar;

    public string transaction_raise_id;

    // Use this for initialization
    void Start ()
    {
        OriginalPosition = ControlWidget.transform.position;
        HidedPosition = OriginalPosition;
        HidedPosition.x += 10.0f;

        Debug.Log("Started!");
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    async void send_raise_action()
    {
        Debug.Log(scrollBar.GetComponent<UIScrollBar>().value.ToString());
        Debug.Log("send_raise_action - " + CLEOS.my_table_index.ToString());

        try
        {
            var result = await CLEOS.eos.CreateTransaction(new Transaction()
            {
                Actions = new List<EosSharp.Api.v1.Action>()
                {
                    new EosSharp.Api.v1.Action()
                    {
                        Account = CLEOS.contract_name,
                        Authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {Actor = CLEOS.account_name, Permission = "active" }
                        },
                        Name = "act",
                        Data = new { name = CLEOS.account_name, table_id = CLEOS.table_index, act = new{ act_= 2, bet_ = CLEOS.raised_bet }, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            transaction_raise_id = result;
            Debug.Log(" send_raise_action result - " + transaction_raise_id);
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

    public void OnClick()
    {
        Debug.Log("UIRaiseBehaivor OnClick( )" + enabled.ToString());

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
        /*
        if (enabled)
        {
            send_raise_action();
            caller.GetComponent<UIButton>().normalSprite = "B_Raise";
            caller.SetActive(false);
            scene.GetComponent<TableSceneBehaivor>().availible_turn = 1;
            scene.GetComponent<TableSceneBehaivor>().CheckTurnAbility(true);  
        }
        else
        {
            caller.GetComponent<UIButton>().normalSprite = "Menu";
            scrollBar.GetComponent<UIScrollBar>().value = 1.0f;
        }
        */
        int_enabled = !int_enabled;    
    }
}
