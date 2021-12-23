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

public class WithdrawBehaivor : MonoBehaviour
{

    public GameObject AlertMessage;
    public GameObject AlertWindow;
    public GameObject caller;
    public GameObject scene;

    //public Eos CLEOS.eos;
    public string transaction_fold_id;

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

    public void OnClick()
    {
        sned_withdraw_action();
        Application.Quit();
    }

    async void sned_withdraw_action()
    {
        Debug.Log("sned_withdraw action - " + CLEOS.my_table_index.ToString());
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
                        Name = "withdraw",
                        Data = new { name = CLEOS.account_name }
                    }
                }
            });

            transaction_fold_id = result;
            Debug.Log(" sned_withdraw result - " + transaction_fold_id);
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

