using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cryptography.ECDSA;

using EosSharp;
using EosSharp.Api.v1;

using System;
using Newtonsoft.Json;

using UnityEngine.SceneManagement;
using System.Globalization;

public class LoadingSceneBehaivor : MonoBehaviour {

    //public Eos CLEOS.eos;

    public string public_key;
    public string current_balance;

    public GameObject AlertWindow;
    public GameObject AlertMessage;

    public string transaction_connect_id = "";
    public string transaction_transfer_id = "";

    [Serializable]
    public class account
    {
        public string name_ = "";
        public string quantity_ = "";
        public List<int> table_id_;
        public int count_of_wins = 0;
        public int count_of_defeats = 0;
        public string total_bank = "";
        public string out_reason = "";
        public string connection_time = "";

        public bool isEmpty()
        {
            return !( name_.Length > 0 );
        }

        public void trace()
        {
            string s = "";
            s += "name_ = " + name_ + "\n";
            s += "quantity_ = " + quantity_ + "\n";
            s += "table_id_ = " + table_id_.Count.ToString() + "\n";
            s += "total_bank = " + total_bank + "\n";

            Debug.Log(s);
        }
    }

    public account current_account = new account();
    public int errors_count = 0;

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    void Start ()
    {
        try
        {
            CLEOS.connect();
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
        
        make_transfer();
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    double quantityToDouble( string q )
    {
        q = q.Replace(CLEOS.symbol, string.Empty);
        q = q.Replace(" ", string.Empty);
        q = q.Replace(".", ",");
        return Convert.ToDouble(q);
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    async void get_account()
    {
        try
        {
            Debug.Log(public_key);

            var accounts = await CLEOS.eos_reader.GetKeyAccounts(public_key);
            if(accounts.Count == 0 )
            {
                throw new System.InvalidOperationException("Account for publicKey - " + public_key + " not founded!" );
            }

            CLEOS.account_name = accounts[0];

            int a = await get_accounts();
            double q = 0.0;
            if (!current_account.isEmpty())
                q = quantityToDouble(current_account.quantity_);

            Debug.Log("q = " + q.ToString());

            if (q < CLEOS.bay_in || current_account.isEmpty())
            {
                transfer(CLEOS.bay_in - q);
                StartCoroutine(proccedToTable());
            }
            else
            {
                transaction_transfer_id = "ok";
                StartCoroutine(proccedToTable());
            }
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

        catch (Exception e)
        {
            AlertMessage.GetComponent<UILabel>().text = e.ToString();
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));

        }
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    async void transfer( double amount )
    {
        Debug.Log("TRANSFERRING - " + amount.ToString());
        try
        {
            string q = amount_to_string(amount);
            Debug.Log("try to transfer - " + q);

            var result = await CLEOS.eos.CreateTransaction(new Transaction()
            {
                Actions = new List<EosSharp.Api.v1.Action>()
                {
                    new EosSharp.Api.v1.Action()
                    {
                        Account = "eosio.token",
                        Authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {Actor = CLEOS.account_name, Permission = "active" }
                        },
                        Name = "transfer",
                        Data = new { from = CLEOS.account_name, to = CLEOS.contract_name, quantity = q, memo = "" }
                    }
                }
            });

            transaction_transfer_id = result;
            Debug.Log("transfer result - " + result);
        }
        
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            Debug.Log(JsonConvert.SerializeObject(e));
            if (AlertMessage != null)
            {               
                if(AlertMessage != null)
                    AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;

                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
        }

        catch( EosSharp.Exceptions.ApiException e )
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

        catch ( Exception e )
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

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    void make_transfer()
    {
        var privKeyBytes = EosSharp.Helpers.CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(CLEOS.key);
        public_key = EosSharp.Helpers.CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privKeyBytes, true));
        Debug.Log(public_key);

        get_account();
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    async System.Threading.Tasks.Task<int> get_accounts()
    {
        try
        {
            GetTableRowsRequest request = new GetTableRowsRequest();
            request.Json = true;
            request.Code = CLEOS.contract_name;
            request.Scope = CLEOS.contract_name;
            request.Table = "accounts";
            request.TableKey = "";
            request.LowerBound = CLEOS.account_name;
            request.UpperBound = "";
            request.Limit = 1;
            request.KeyType = "name";
            request.IndexPosition = "1";

            var Table = await CLEOS.eos_reader.GetTableRows(request);
            foreach (object row in Table.Rows)
            {
                if (string.Equals(CLEOS.account_name, JsonUtility.FromJson<account>(row.ToString()).name_))
                {
                    Debug.Log("EQUALS = " + CLEOS.account_name + " : " + JsonUtility.FromJson<account>(row.ToString()).name_);
                    current_account = JsonUtility.FromJson<account>(row.ToString());

                    CLEOS.quantity = current_account.quantity_;

                    current_account.trace();
                }
            }       
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

        return 0;
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    IEnumerator proccedToTable()
    {
        while (transaction_transfer_id.Length == 0 || current_account.isEmpty() || quantityToDouble(current_account.quantity_) < CLEOS.bay_in)
        {
            get_accounts();           
            yield return new WaitForSeconds(1.0f);
        }

        string old_connection_time = current_account.connection_time;
        current_account.table_id_.Clear();

        Debug.Log("try to connect to table..." + transaction_transfer_id);        
        connect_to_table(CLEOS.small_blind, CLEOS.players_count, true);
        
        while (transaction_connect_id.Length == 0)
            yield return new WaitForSeconds(1.0f);

        transaction_connect_id = "";

        Debug.Log("connect to table ok");

        while (current_account.isEmpty() || String.Compare(current_account.connection_time, old_connection_time) == 0 || current_account.table_id_.Count == 0)
        {
            Debug.Log("OLD = " + old_connection_time + " NEW = " + current_account.connection_time);
            get_accounts();
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("proccedToTable - " + current_account.table_id_[0].ToString() );
        CLEOS.table_id = current_account.table_id_[0];

        SceneManager.LoadSceneAsync("TableScene");
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    void Update () {
		
	}

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    string amount_to_string( double data )
    {
        var cl = CultureInfo.CreateSpecificCulture("es-ES");
        cl.NumberFormat.NumberDecimalSeparator = ".";
        cl.NumberFormat.NumberGroupSeparator = "";
        return data.ToString("N4", cl) + CLEOS.symbol;
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    string small_blind_to_string(double data)
    {

        var cl = CultureInfo.CreateSpecificCulture("es-ES");
        cl.NumberFormat.NumberDecimalSeparator = ".";
        cl.NumberFormat.NumberGroupSeparator = ".";
        return data.ToString("N4", cl) + CLEOS.symbol;
    }

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    async void connect_to_table(double small_blind, int max_players, bool connect )
    {
        Debug.Log("account name - " + CLEOS.account_name + " small blind = " + small_blind.ToString());
        try
        {
            string q = small_blind_to_string(small_blind);
            Debug.Log(q);

            if (connect)
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
                            Name = "connecttable",
                            Data = new { name = CLEOS.account_name, small_blind = q, max_players = max_players }
                        }
                    }
                });
                transaction_connect_id = result;
                Debug.Log(" connecttable result - " + result );
            }
            else
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
                            Name = "outfromtable",
                            Data = new { name = CLEOS.account_name, table_id = CLEOS.table_id }
                        }
                    }
                });
                transaction_connect_id = result;
                Debug.Log(" outfromtable result - " + result);
            }
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

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    async void get_tables()
    {
        try
        {
            GetTableRowsRequest request = new GetTableRowsRequest();
            request.Json = true;
            request.Code = CLEOS.contract_name;
            request.Scope = CLEOS.contract_name;
            request.Table = "tables";
            request.TableKey = "";
            request.LowerBound = CLEOS.account_name;
            request.UpperBound = "";
            request.Limit = 1;
            request.KeyType = "";
            request.IndexPosition = "";

            var Table = await CLEOS.eos_reader.GetTableRows(request);

            foreach (object row in Table.Rows)
            {
                Debug.Log(row.ToString());
            }
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

/****************************************************************************************************************************************/
/****************************************************************************************************************************************/
/****************************************************************************************************************************************/

    void debug_remove_from_table()
    {
        Debug.Log("try to remove from table...");

        connect_to_table(CLEOS.small_blind, CLEOS.players_count, false);
        transaction_connect_id = "";

        Debug.Log("remove from table ok");
    }

    /****************************************************************************************************************************************/
    /****************************************************************************************************************************************/
    /****************************************************************************************************************************************/

    async void test_while()
    {
        try
        {
            var result = await CLEOS.eos.CreateTransaction(new Transaction()
            {
                Actions = new List<EosSharp.Api.v1.Action>()
                {
                    new EosSharp.Api.v1.Action()
                    {
                        Account = "pokerchained",
                        Authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {Actor = CLEOS.account_name, Permission = "active" }
                        },
                        Name = "testwhile"
                    }
                }
            });

            transaction_transfer_id = result;
            Debug.Log("test_while result - " + result);
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
          //  Debug.Log(JsonConvert.SerializeObject(e));
            Debug.Log(e.Content);

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
