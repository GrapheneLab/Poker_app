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

public class ErrorButton : MonoBehaviour {

    public GameObject ControlWidget;

    public GameObject AlertMessage;
    public GameObject AlertWindow;

    public GameObject scene;

    //public Eos CLEOS.eos;

    public bool reloadTable = false;
    public bool quitFromTable = false;

    public string send_end_game_transaction = "";

    // Use this for initialization
    void Start()
    {
        try
        {
            CLEOS.connect();
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));
            ;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
        foreach (UITweener tw in tweens)
        {
            if (tw.tweenGroup == 1)
                tw.Play(true);
        }

        if (reloadTable)
        { 
            GameObject scene = GameObject.Find("SceneBehaivor");     
            scene.GetComponent<TableSceneBehaivor>().transaction_shuffle_cards = "";

            CombinationPlugin.current_combination = 0;

            send_end_game_action();
            return;
        }
        SceneManager.LoadScene("ChoseTable");
    }

    async void send_end_game_action()
    {
        Debug.Log("send_end_game_action - " + CLEOS.my_table_index.ToString());
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
                        Name = "sendendgame",
                        Data = new { name = CLEOS.account_name, table_id = CLEOS.table_index, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            send_end_game_transaction = result;
            Debug.Log(" send_end_game_transaction result - " + send_end_game_transaction);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));
            ;
        }
    }
}
