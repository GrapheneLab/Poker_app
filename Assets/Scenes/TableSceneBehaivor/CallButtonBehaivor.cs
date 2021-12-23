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

public class CallButtonBehaivor : MonoBehaviour
{

    public GameObject AlertMessage;
    public GameObject AlertWindow;
    public GameObject caller;
    public GameObject scene;

    //public Eos CLEOS.eos;

    public string transaction_call_id;

    [Serializable]
    public class Act
    {
        public byte act_;
        public string bet_; // only for a ACT_BET, SMALL and BIG BLINDs
    }

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
        send_call_action();
        caller.SetActive(false);
        scene.GetComponent<TableSceneBehaivor>().availible_turn = 1;
        scene.GetComponent<TableSceneBehaivor>().CheckTurnAbility(true);
    }

    async void send_call_action()
    {
        if (CLEOS.permission_to_make_turn == false)
            return;
        CLEOS.permission_to_make_turn = false;

        Debug.Log("send_call_action - " + CLEOS.my_table_index.ToString());
        Debug.Log("current bet = " + CLEOS.current_bet);
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
                        Data = new { name = CLEOS.account_name, table_id = CLEOS.table_index, act = new{ act_= 2, bet_ = CLEOS.current_bet }, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            transaction_call_id = result;
            Debug.Log(" send_call_action result - " + transaction_call_id);
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
