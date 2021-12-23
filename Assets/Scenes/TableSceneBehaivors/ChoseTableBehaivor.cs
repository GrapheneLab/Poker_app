using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using EosSharp;

using EosSharp.Api.v1;

using System.Linq;
using System.Threading.Tasks;

using System;

using System.Runtime.InteropServices;

using EosSharp.Helpers;
using EosSharp.Providers;
using System.Globalization;

public class ChoseTableBehaivor : MonoBehaviour {



    [Serializable]
    public struct global_table
    {
        /*
        std::vector<eosio::asset> small_blind_values;
        std::vector<uint8_t> max_players_count_values;
        uint32_t penalty_percent;
        eosio::asset max_penalty_value;
        uint32_t player_pay_percent;
        uint32_t master_account_pay_percent;
        uint32_t warning_timeout_sec;
        uint32_t last_timeout_sec;
        */

        public List<string> small_blind_values;
        public List<int> max_players_count_values;
        public int penalty_percent;
        public string max_penalty_value;
        public int player_pay_percent;
        public int master_account_pay_percent;
        public int warning_timeout_sec;
        public int last_timeout_sec;
        public int version;
    }

    //public Eos CLEOS.eos;

    public List<string> small_blinds = new List<string>();

    public global_table gt = new global_table();

    public double small_blind = -1.0;
    public double big_blind = -1.0;
    public double bay_in = -1.0;
    public int players_count = -1;

    // Use this for initialization
    void Start () {
        try
        {
            CLEOS.disconnect();
            CLEOS.connect();
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            Debug.Log("EXCEPTION ON CONNECT");
            Debug.Log(e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message);
        
            GameObject Label = GameObject.Find("Message");
            Label.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            GameObject ControlWidget = GameObject.Find("AlertWindow");
            UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            ;
        }

        get_globals();

        GameObject popup1 = GameObject.Find("SmallBlindValues1");
        EventDelegate.Add(popup1.GetComponent<UIPopupList>().onChange, onSmallBlindSelectionChange);

        GameObject popup2 = GameObject.Find("PlayersCountValues1");
        EventDelegate.Add(popup2.GetComponent<UIPopupList>().onChange, onPlayersCountChange);
    }

    public void onPlayersCountChange()
    {
        Debug.Log("onPlayersCountChange");

        if (UIPopupList.current.value.Length != 0)
        {
            players_count = Int32.Parse(UIPopupList.current.value);
            CLEOS.players_count = players_count;

            GameObject CurrentPlayersCount = GameObject.Find("CurrentPlayersCount");
            CurrentPlayersCount.GetComponent<UILabel>().text = players_count.ToString();
        }
    }

    public void onSmallBlindSelectionChange()
    {
        Debug.Log("onSmallBlindSelectionChange");
        GameObject popup = GameObject.Find("SmallBlindValues");
        if (UIPopupList.current.value.Length != 0)
        {
            string v = UIPopupList.current.value;

            string[] arr = v.Split(' ');
            arr[0] = arr[0].Replace(".", ",");

            String number = arr[0];
            double value = double.Parse(number, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("es-ES"));

            Debug.Log(value.ToString());

            CLEOS.small_blind = value;
            CLEOS.big_blind = value * 2.0f;
            CLEOS.bay_in = value * 20.0f;

            small_blind = CLEOS.small_blind;
            big_blind = CLEOS.big_blind;
            bay_in = CLEOS.bay_in;

            GameObject BuyInLabel = GameObject.Find("BuyInLabel");
            GameObject BigBlindLabel = GameObject.Find("BigBlindLabel");

            v = bay_in.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES")) + CLEOS.symbol;
            BuyInLabel.GetComponent<UILabel>().text = v;

            v = big_blind.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES")) + CLEOS.symbol;
            BigBlindLabel.GetComponent<UILabel>().text = v;

            GameObject CurrentSmallBlind = GameObject.Find("CurrentSmallBlind");
            CurrentSmallBlind.GetComponent<UILabel>().text = v;
            v = small_blind.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES")) + CLEOS.symbol;
            CurrentSmallBlind.GetComponent<UILabel>().text = v;
        }
    }

    /*
    public class GetTableRowsRequest
    {
        [JsonProperty("json")]
        public bool? Json { get; set; } = false;
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("table_key")]
        public string TableKey { get; set; }
        [JsonProperty("lower_bound")]
        public string LowerBound { get; set; } = "0";
        [JsonProperty("upper_bound")]
        public string UpperBound { get; set; } = "-1";
        [JsonProperty("limit")]
        public UInt32? Limit { get; set; } = 10;
        [JsonProperty("key_type")]
        public string KeyType { get; set; }
        [JsonProperty("index_position")]
        public string IndexPosition { get; set; }
    }
    */

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
            request.LowerBound = "";
            request.UpperBound = "";
            request.Limit = 10;
            request.KeyType = "";
            request.IndexPosition = "";

            var Table = await CLEOS.eos.GetTableRows(request);

            foreach( object row in Table.Rows )
            {
                Debug.Log(row.ToString());
            }
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            Debug.Log(e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message);

            GameObject Label = GameObject.Find("Message");
            Label.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            GameObject ControlWidget = GameObject.Find("AlertWindow");
            UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            ;
        }
    }

    async void get_globals()
    {
        try
        {
            GetTableRowsRequest request = new GetTableRowsRequest();
            request.Json = true;
            request.Code = CLEOS.contract_name;
            request.Scope = CLEOS.contract_name;
            request.Table = "globalstate";
            request.TableKey = "";
            request.LowerBound = "";
            request.UpperBound = "";
            request.Limit = 10;
            request.KeyType = "";
            request.IndexPosition = "";

            var Table = await CLEOS.eos.GetTableRows(request);

            Debug.Log("GOT INFO!!!!!!!!!!" + Table.ToString());

            GameObject smb = GameObject.Find("SmallBlindValues1");
            GameObject plr = GameObject.Find("PlayersCountValues1");

            foreach (object row in Table.Rows)
            {
                Debug.Log("INSIDE!!!!!!");
                Debug.Log(row.ToString());
                gt = JsonUtility.FromJson<global_table>(row.ToString());

                CLEOS.penalty_percent = gt.penalty_percent;
                CLEOS.max_penalty_value = gt.max_penalty_value;
                CLEOS.player_pay_percent = gt.player_pay_percent;
                CLEOS.master_account_pay_percent = gt.master_account_pay_percent;
                CLEOS.warning_timeout_sec = gt.warning_timeout_sec;
                CLEOS.last_timeout_sec = gt.last_timeout_sec;

                foreach (string s in gt.small_blind_values)
                {
                    smb.GetComponent<UIPopupList>().items.Add(s);
                }

                foreach (int s in gt.max_players_count_values)
                {
                    plr.GetComponent<UIPopupList>().items.Add(s.ToString());
                }
            }
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            Debug.Log("EXCEPTION ON get_globals");

            Debug.Log(e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message);

            GameObject Label = GameObject.Find("Message");
            Label.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            GameObject ControlWidget = GameObject.Find("AlertWindow");
            UITweener[] tweens = ControlWidget.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            ;
        }
    }
    /*
    *2 
    *20
    * */

    // Update is called once per frame
    void Update () {
    }
}
