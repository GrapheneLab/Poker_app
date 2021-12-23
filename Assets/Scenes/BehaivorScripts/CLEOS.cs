using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EosSharp;

using EosSharp.Api.v1;

using System.Linq;
using System.Threading.Tasks;

using System;

using System.Runtime.InteropServices;
using Newtonsoft.Json;

public static class CLEOS {
    public static Eos eos = null;
    public static Eos eos_reader = null;

    public static string symbol = " EOS";
    public static string contract_name = "pokerchained";
    public static string chain_id = "7b7ebda7697b09f5ef9794c06b2c38d82e675ee0c8c700901ef017ebbc0b0e4b";

    public static string account_name;
    public static string quantity;
    public static int my_table_index;
    public static int table_index = -666;
    public static int next_player_index;
    public static int last_turn_player_index;

    public static int perv_step_players_count = Int32.MaxValue;

    public static List<string> raise_variants = new List<string>();

    public static string ip_addr;
    public static string key;

    public static string reader_ipaddr = "http://192.168.1.146:8000";

    public static double small_blind;
    public static double big_blind;
    public static double bay_in;
    public static int players_count;
    public static string current_bet;
    public static string raised_bet;

    public static int penalty_percent;
    public static string max_penalty_value;
    public static int player_pay_percent;
    public static int master_account_pay_percent;
    public static int warning_timeout_sec;
    public static int last_timeout_sec;

    public static int table_id;
    public static bool permission_to_make_turn = false;

    public static bool auto_re_buy;
    public static bool skip_mainloop = false;
    public static bool reload_data = true;
    public static int counter = 0;
    public static string version = "V 0.1.1";

    public static bool need_to_update_scene = false;

    public static string last_table_timestamp = "";
    public static long last_table_timestamp_abi;

    public static int game_loop_counter = 0;

    public static void setup(string ip, string private_key)
    {
        ip_addr = ip;
        key = private_key;
    }

    public static void setup(string ip)
    {
        ip_addr = ip;
    }

    public static void disconnect()
    {
        if (eos != null)
        {
            eos.cancel_execution();
            eos = null;
        }

        if (eos_reader != null)
        {
            eos_reader.cancel_execution();
            eos_reader = null;
        }

        GC.Collect();
    }

    public static void connect()
    {
        if (eos != null)
            return;

        if (key.Length == 0)
            return;

        if (ip_addr.Length == 0)
            return;
        try
        {
            eos = new Eos(new EosConfigurator()
            {
                
                HttpEndpoint = CLEOS.ip_addr,
                ChainId = CLEOS.chain_id,
                SignProvider = new DefaultSignProvider(CLEOS.key)
            });

            eos_reader = new Eos(new EosConfigurator()
            {
                HttpEndpoint = CLEOS.reader_ipaddr,
                ChainId = CLEOS.chain_id,
                SignProvider = new DefaultSignProvider(CLEOS.key)
            });
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            Debug.Log(JsonConvert.SerializeObject(e));
        }

        catch (EosSharp.Exceptions.ApiException e)
        {
            Debug.Log(JsonConvert.SerializeObject(e));
        }

        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
