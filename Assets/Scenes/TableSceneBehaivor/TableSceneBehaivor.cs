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
using System.Globalization;
using System.Linq;

public class TableSceneBehaivor : MonoBehaviour
{
    static int __LINE__([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
    {
        return lineNumber;
    }
    static string __FILE__([System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
    {
        return fileName;
    }

    public Table table = new Table();

    public GameObject AlertWindow;
    public GameObject AlertMessage;
    public GameObject AlertButton;
    public GameObject EndGameWindow;

    public GameObject Player0;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public GameObject Player5;
    public GameObject Player6;
    public GameObject Player7;
    public GameObject Player8;
    public GameObject Player9;

    public GameObject LeftPanel;
    public GameObject RightPanel;

    public GameObject TableCard0;
    public GameObject TableCard1;
    public GameObject TableCard2;
    public GameObject TableCard3;
    public GameObject TableCard4;

    public GameObject TableDeck0;
    public GameObject TableDeck1;
    public GameObject TableDeck2;
    public GameObject TableDeck3;
    public GameObject TableDeck4;
    public GameObject TableDeck5;

    public GameObject progressBar;
    public GameObject progressBarLabel;

    public GameObject bay_in_button;
    public GameObject raise_button;
    public GameObject fold_button;
    public GameObject call_button;
    public GameObject check_button;
    public GameObject show_cards_button;
    public GameObject menu_button;

    public GameObject exit_button;
    public GameObject continue_button;

    public GameObject current_bet_label;
    public GameObject current_bank_label;

    public GameObject shuffle_menu;
    public GameObject end_game_timer;
    public GameObject end_game_timer1;

    public Texture gc_background;
    public Texture gc_table;

    GameObject deallerCoin = null;
    int last_dealler_index = -1;

    public bool firstStart = true;
    public bool firstTimeStart = true;
    public bool gameWasEnded = false;
    public int next_player_index = 0;
    public bool shuffled = false;
    public bool grayscale = false;

    public GetInfoResponse last_block = new GetInfoResponse();

    public string transaction_reset_table_id = "";
    public string transaction_shuffle_cards = "";
    public string transaction_crypted_cards = "";
    public string transaction_setcardskeys = "";
    public string transaction_setcardskeys_for_showdown = "";
    public string transaction_transfer_id = "";
    public string transaction_transfer_wait_allin_keys = "";
    public string transaction_withdraw_id = "";
    public string transaction_send_new_game_id = "";
    public string transaction_wait_players = "";


    public Key shuffle_personal_key = new Key();
    public Key tmp_key = new Key();
    public Key[] personal_keys = new Key[52];
    public Key fake_personal_key = new Key();

    public Dictionary<int, string> players_indexes = new Dictionary<int, string>();
    public Dictionary<string, string> players_indexes_names = new Dictionary<string, string>();
    public List<string> avatar_names = new List<string>();

    public int last_status = -1;
    public int last_history_size = 0;
    public int availible_turn = 0;
    public int perv_game = 0;

    public bool force_reset = false;
    public bool run_main_loop = true;

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

        public bool isEmpty()
        {
            return !(name_.Length > 0);
        }
    }

    public account current_account = new account();

    [Serializable]
    public class Act
    {
        public byte act_;
        public string bet_; // only for a ACT_BET, SMALL and BIG BLINDs
    }

    [Serializable]
    public class TableKey
    {
        public byte[] data = new byte[32];
        public byte card_index;
        public byte[] s = new byte[8];
        public byte[] m = new byte[8];

        public void trace_b()
        {
          //  Debug.Log("KEY = " + BitConverter.ToString(data));
          //  Debug.Log("SYNCHRO = " + BitConverter.ToString(s));
        }
    }

    [Serializable]
    public class Key
    {
        public byte[] data = new byte[32];
        public uint[] data_i = new uint[8];

        public int card_index = -1;

        public byte[] s = new byte[8];
        public uint[] s_i = new uint[2];

        public byte[] m = new byte[8];
        public uint[] m_i = new uint[2];

        public void setup()
        {
            Buffer.BlockCopy(data, 0, data_i, 0, 32);
            Buffer.BlockCopy(s, 0, s_i, 0, 8);
            Buffer.BlockCopy(m, 0, m_i, 0, 8);
        }

        public void setup_from_table_key(TableKey tk)
        {
            Buffer.BlockCopy(tk.data, 0, data, 0, 32);
            Buffer.BlockCopy(tk.s, 0, s, 0, 8);
            Buffer.BlockCopy(tk.m, 0, m, 0, 8);
            card_index = (int)tk.card_index;
            setup();
        }

        public void trace()
        {
            string s = "";
            for (int i = 0; i < 8; i++)
                s += " " + data_i[i].ToString();

            //Debug.Log("KEY = " + s);

            s = "";
            for (int i = 0; i < 2; i++)
                s += " " + s_i[i].ToString();

            //Debug.Log("SYNCHRO = " + s);

            s = "";
            for (int i = 0; i < 2; i++)
                s += " " + m_i[i].ToString();

            //Debug.Log("MAC = " + s);
        }

        public void trace_ch()
        {
            string ss = "";
            for (int i = 0; i < 32; i++)
                ss += " " + data[i].ToString();

            //Debug.Log("KEY = " + ss);

            ss = "";
            for (int i = 0; i < 8; i++)
                ss += " " + s[i].ToString();

            //Debug.Log("SYNCHRO = " + ss);

            ss = "";
            for (int i = 0; i < 8; i++)
                ss += " " + m[i].ToString();

            //Debug.Log("MAC = " + ss);
        }
    }

    [Serializable]
    public class Card
    {
        public byte suit = 0;
        public byte value = 0;

        public Card()
        {
            suit = 0;
            value = 0;
        }

        public Card(Card card)
        {
            suit = card.suit;
            value = card.value;
        }

        public Card(byte s, byte v)
        {
            suit = s;
            value = v;
        }

        public void Trace()
        {
            Debug.Log("Card suit = " + suit.ToString() + " value = " + value.ToString());
        }
    }

    [Serializable]
    public class Combination
    {
        public byte type;
        public List<Card> cards;
    };

    [Serializable]
    public class PlayerHistoryInfo
    {
        public string name;
        public string winnings; // =0 if no winnings
        public List<Act> acts; // contains bets = commissions
        public List<byte> cards_indexes;
        public Combination combo;
    };

    [Serializable]
    public class PlayerAct
    {
        public byte player_index;
        public string name_;
        public Act act_;
    };

    [Serializable]
    public class PreviousGameResult
    {
        public byte result;
        public string bank;
        public List<PlayerHistoryInfo> players_info;
    };

    [Serializable]
    public class Player
    {
        public string name;
        public int status = 0;
        public int have_event = 0;
        public int was_reseted = 0;
        public int count_of_wins = 0;
        public int count_of_defeats = 0;
        public string stack = "";
        public List<int> cards_indexes = new List<int>();
        public List<Act> acts = new List<Act>();
        public List<Key> keys = new List<Key>();
    }

    [Serializable]
    public class Table
    {
        public int id = -1;
        public string small_blind;
        public int max_players = 10; // 5, 9, or 10
        public int players_count = 0;
        public List<string> raise_variants;
        public int current_game_players_count = 0;
        public int current_players_received_count = 0;
        public int current_game_round = 0;
        public string current_bet;
        public string current_bank;
        public int table_status = 0;
        public string bank;
        public int dealer_index = -1;
        public string last_act_time;
        public long timestamp;
        public int next_player_index;
        public List<byte> possible_moves;
        public List<byte> waiting_keys_indexes;
        public List<int> table_cards_indexes;
        public List<Card> table_cards; // 3, 4 or 5 max
        public List<Card> the_deck_of_cards; // 52 cards
        public List<Player> players;
        public List<PlayerAct> players_acts;
        public List<PreviousGameResult> history;
        public List<TableKey> all_keys;
        public byte table_was_reseted;

        public void Trace()
        {
            string buffer;
            buffer = " id = " + id.ToString();
            buffer += "; max_players = " + max_players.ToString();
            buffer += "; players_count = " + players_count.ToString();
            buffer += "; table_status = " + table_status.ToString();
            buffer += "; bank = " + bank;
            buffer += "; dealer_index = " + dealer_index.ToString();
            buffer += "; last_act_time = " + last_act_time;
            buffer += "; next_player_index = " + next_player_index.ToString();
            buffer += "; table_cards_indexes count = " + table_cards_indexes.Count.ToString();
            buffer += "; players count = " + players.Count.ToString();   

            Debug.Log(buffer);
        }
    }

    [Serializable]
    public class TransactionData
    {
        public string name;
        public int table_id; 
    }

    enum PlayerStatus
    {
        P_WAIT_NEW_GAME = 0,
        P_IN_GAME,
        P_ALL_IN,
        P_FOLD,
        P_TIMEOUT,
        P_DECRYPT_ERROR,
        P_OUT
    };

    public void enablePlayer( int index, bool enable )
    {
        switch ( index )
        {
            case 0:
                if(Player0.active == false && enable == true )
                    Player0.SetActive(true);
                if(Player0.active == true && enable == false)
                    Player0.SetActive(false);
                break;
            case 1:
                if (Player1.active == false && enable == true)
                    Player1.SetActive(true);
                if (Player1.active == true && enable == false)
                    Player1.SetActive(false);
                break;
            case 2:
                if (Player2.active == false && enable == true)
                    Player2.SetActive(true);
                if (Player2.active == true && enable == false)
                    Player2.SetActive(false);
                break;
            case 3:
                if (Player3.active == false && enable == true)
                    Player3.SetActive(true);
                if (Player3.active == true && enable == false)
                    Player3.SetActive(false);
                break;
            case 4:
                if (Player4.active == false && enable == true)
                    Player4.SetActive(true);
                if (Player4.active == true && enable == false)
                    Player4.SetActive(false);
                break;
            case 5:
                if (Player5.active == false && enable == true)
                    Player5.SetActive(true);
                if (Player5.active == true && enable == false)
                    Player5.SetActive(false);
                break;
            case 6:
                if (Player6.active == false && enable == true)
                    Player6.SetActive(true);
                if (Player6.active == true && enable == false)
                    Player6.SetActive(false);
                break;
            case 7:
                if (Player7.active == false && enable == true)
                    Player7.SetActive(true);
                if (Player7.active == true && enable == false)
                    Player7.SetActive(false);
                break;
            case 8:
                if (Player8.active == false && enable == true)
                    Player8.SetActive(true);
                if (Player8.active == true && enable == false)
                    Player8.SetActive(false);
                break;
            case 9:
                if (Player9.active == false && enable == true)
                    Player9.SetActive(true);
                if (Player9.active == true && enable == false)
                    Player9.SetActive(false);
                break;
        }
    }

    public void enableTableCard(int index, bool enable)
    {
        switch (index)
        {
            case 0:
                TableCard0.SetActive(enable);
                break;
            case 1:
                TableCard1.SetActive(enable);
                break;
            case 2:
                TableCard2.SetActive(enable);
                break;
            case 3:
                TableCard3.SetActive(enable);
                break;
            case 4:
                TableCard4.SetActive(enable);
                break;
        }
    }

    public void enableTableDeckCard(int index, bool enable)
    {
        switch (index)
        {
            case 0:
                if(TableDeck0.active == false && enable == true)
                    TableDeck0.SetActive(true);
                if (TableDeck0.active == true && enable == false)
                    TableDeck0.SetActive(false);
                break;
            case 1:
                if (TableDeck1.active == false && enable == true)
                    TableDeck1.SetActive(true);
                if (TableDeck1.active == true && enable == false)
                    TableDeck1.SetActive(false);
                break;
            case 2:
                if (TableDeck2.active == false && enable == true)
                    TableDeck2.SetActive(true);
                if (TableDeck2.active == true && enable == false)
                    TableDeck2.SetActive(false);
                break;
            case 3:
                if (TableDeck3.active == false && enable == true)
                    TableDeck3.SetActive(true);
                if (TableDeck3.active == true && enable == false)
                    TableDeck3.SetActive(false);
                break;
            case 4:
                if (TableDeck4.active == false && enable == true)
                    TableDeck4.SetActive(true);
                if (TableDeck4.active == true && enable == false)
                    TableDeck4.SetActive(false);
                break;
            case 5:
                if (TableDeck5.active == false && enable == true)
                    TableDeck5.SetActive(true);
                if (TableDeck5.active == true && enable == false)
                    TableDeck5.SetActive(false);
                break;
        }
    }

    public void setupPlayer( int index, string name, string balance )
    {
        string result = "";
        if (avatar_names.Count == 0)
            prepare_avatars();

        switch (index)
        {
            case 0:
                if (Player0.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player0.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player0.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player0.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player0.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player0.transform.GetChild(1).gameObject.SetActive(false);
                    Player0.transform.GetChild(2).gameObject.SetActive(false);
                    Player0.transform.GetChild(3).gameObject.SetActive(false);
                    Player0.transform.GetChild(4).gameObject.SetActive(false);
                    Player0.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player0.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player0");
                    players_indexes_names.Add(name, "Player0");
                }
                break;
            case 1:
                if (Player1.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player1.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player1.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player1.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player1.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player1.transform.GetChild(1).gameObject.SetActive(false);
                    Player1.transform.GetChild(2).gameObject.SetActive(false);
                    Player1.transform.GetChild(3).gameObject.SetActive(false);
                    Player1.transform.GetChild(4).gameObject.SetActive(false);
                    Player1.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player1.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player1");
                    players_indexes_names.Add(name, "Player1");
                }
                break;
            case 2:
                if (Player2.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player2.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player2.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player2.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player2.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player2.transform.GetChild(1).gameObject.SetActive(false);
                    Player2.transform.GetChild(2).gameObject.SetActive(false);
                    Player2.transform.GetChild(3).gameObject.SetActive(false);
                    Player2.transform.GetChild(4).gameObject.SetActive(false);
                    Player2.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player2.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player2");
                    players_indexes_names.Add(name, "Player2");
                }
                break;
            case 3:
                if (Player3.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player3.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player3.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player3.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player3.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player3.transform.GetChild(1).gameObject.SetActive(false);
                    Player3.transform.GetChild(2).gameObject.SetActive(false);
                    Player3.transform.GetChild(3).gameObject.SetActive(false);
                    Player3.transform.GetChild(4).gameObject.SetActive(false);
                    Player3.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player3.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player3");
                    players_indexes_names.Add(name, "Player3");
                }
                break;
            case 4:
                if (Player4.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player4.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player4.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player4.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player4.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player4.transform.GetChild(1).gameObject.SetActive(false);
                    Player4.transform.GetChild(2).gameObject.SetActive(false);
                    Player4.transform.GetChild(3).gameObject.SetActive(false);
                    Player4.transform.GetChild(4).gameObject.SetActive(false);
                    Player4.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player4.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player4");
                    players_indexes_names.Add(name, "Player4");
                }
                break;
            case 5:
                if (Player5.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player5.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player5.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player5.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player5.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player5.transform.GetChild(1).gameObject.SetActive(false);
                    Player5.transform.GetChild(2).gameObject.SetActive(false);
                    Player5.transform.GetChild(3).gameObject.SetActive(false);
                    Player5.transform.GetChild(4).gameObject.SetActive(false);
                    Player5.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player5.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player5");
                    players_indexes_names.Add(name, "Player5");
                }
                break;
            case 6:
                if (Player6.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player6.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player6.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player6.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player6.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player6.transform.GetChild(1).gameObject.SetActive(false);
                    Player6.transform.GetChild(2).gameObject.SetActive(false);
                    Player6.transform.GetChild(3).gameObject.SetActive(false);
                    Player6.transform.GetChild(4).gameObject.SetActive(false);
                    Player6.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player6.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player6");
                    players_indexes_names.Add(name, "Player6");
                }
                break;
            case 7:
                if (Player7.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player7.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player7.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player7.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player7.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player7.transform.GetChild(1).gameObject.SetActive(false);
                    Player7.transform.GetChild(2).gameObject.SetActive(false);
                    Player7.transform.GetChild(3).gameObject.SetActive(false);
                    Player7.transform.GetChild(4).gameObject.SetActive(false);
                    Player7.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player7.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player7");
                    players_indexes_names.Add(name, "Player7");
                }
                break;
            case 8:
                if (Player8.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player8.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player8.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player8.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player8.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player8.transform.GetChild(1).gameObject.SetActive(false);
                    Player8.transform.GetChild(2).gameObject.SetActive(false);
                    Player8.transform.GetChild(3).gameObject.SetActive(false);
                    Player8.transform.GetChild(4).gameObject.SetActive(false);
                    Player8.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player8.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player8");
                    players_indexes_names.Add(name, "Player8");
                }
                break;
            case 9:
                if (Player9.active == false)
                {
                    result = avatar_names.First();
                    avatar_names.Remove(avatar_names.First());

                    Player9.transform.GetChild(6).gameObject.GetComponent<UILabel>().text = name;
                    Player9.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    Player9.transform.GetChild(8).gameObject.GetComponent<UILabel>().text = "";
                    Player9.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = result;
                    Player9.transform.GetChild(1).gameObject.SetActive(false);
                    Player9.transform.GetChild(2).gameObject.SetActive(false);
                    Player9.transform.GetChild(3).gameObject.SetActive(false);
                    Player9.transform.GetChild(4).gameObject.SetActive(false);
                    Player9.transform.GetChild(5).gameObject.SetActive(false);
                }
                else
                {
                    Player9.transform.GetChild(7).gameObject.GetComponent<UILabel>().text = balance;
                    players_indexes.Add(index, "Player9");
                    players_indexes_names.Add(name, "Player9");
                }
                break;
        }
    }

    void Start ()
    {
        try
        {
            CLEOS.disconnect();
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
        }

        for ( int i = 0; i < 52; i++ )
           personal_keys[i] = new Key();

        prepare_table();
        StartCoroutine(gameMainLoop());

        {
            UITweener[] tweens = LeftPanel.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
                else
                    tw.Play(false);
            }

            show_cards_button.GetComponent<TweenScale>().enabled = false;
            show_cards_button.GetComponent<UICardsShowBehaivor>().enabled = false;
        }
    }

    void prepare_table()
    {
        CLEOS.skip_mainloop = false;
 
        for (int i = 0; i < 9; i++)
            enablePlayer(i, false);

        for (int i = 0; i < 5; i++)
            enableTableCard(i, false);

        for (int i = 0; i < 6; i++)
            enableTableDeckCard(i, false);

        CheckTurnAbility(true);
    }

    void prepare_avatars()
    {
        avatar_names.Clear();
        avatar_names.Add("F1");
        avatar_names.Add("F2");
        avatar_names.Add("F3");
        avatar_names.Add("F4");
        avatar_names.Add("F5");
        avatar_names.Add("M1");
        avatar_names.Add("M2");
        avatar_names.Add("M3");
        avatar_names.Add("M4");
        avatar_names.Add("M5");

        int[] indexes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int r = ShufflePlugin.shuffle(indexes, 10);

        List<string> t = new List<string>();
        for (int i = 0; i < 10; i++)
            t.Add(avatar_names[i]);

        avatar_names = t.GetRange(0, t.Count);
    }

    public void make_grayscale(bool make)
    {
        if (grayscale == true)
            return;

        if( progressBar != null )
             progressBar.SetActive(false);

        if (progressBarLabel != null)
            progressBarLabel.SetActive(false);

        grayscale = true;
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.name == "0")
                continue;

            if (go.GetComponent<UISprite>() != null)
            {
                string s = go.GetComponent<UISprite>().spriteName;
                if (make)
                    s += "_bw";
                else
                    s = s.Replace("_bw", "");
    
                go.GetComponent<UISprite>().spriteName = s;
            }
            if (go.GetComponent<UITexture>() != null)
            {
                string s = go.GetComponent<UITexture>().name;
                if( s.Contains("Background") == true )
                    go.GetComponent<UITexture>().mainTexture = gc_background;

                if (s.Contains("Table") == true)
                    go.GetComponent<UITexture>().mainTexture = gc_table;
            }
        }
    }

    void make_end_game(List<string> winners)
    {
        readTableCardsEnd();

        progressBar.SetActive(false);
        progressBarLabel.SetActive(false);

        show_cards_button.SetActive(false);
        menu_button.SetActive(false);
        bay_in_button.SetActive(false);

        exit_button.GetComponent<ExitGame>().make_grayscale = false;
        continue_button.GetComponent<ErrorButton>().reloadTable = true;

        end_game_timer.GetComponent<UILabel>().text = "";
        end_game_timer1.GetComponent<UILabel>().text = "";

        for (int i = 0; i < table.current_game_players_count; i++)
        {
            GameObject p = GameObject.Find(players_indexes_names[table.players[i].name]);
            GameObject child_player_name = p.transform.GetChild(6).gameObject;
            child_player_name.GetComponent<UILabel>().color = Color.white;
            child_player_name.GetComponent<TweenScale>().enabled = false;

            GameObject child_avatar = p.transform.GetChild(0).gameObject;
            child_avatar.GetComponent<TweenScale>().enabled = false;
        }

        {
            string s = TableCard0.GetComponent<UISprite>().spriteName.Replace("_bw","");
            TableCard0.GetComponent<UISprite>().spriteName = s;
        }

        {
            string s = TableCard1.GetComponent<UISprite>().spriteName.Replace("_bw", "");
            TableCard1.GetComponent<UISprite>().spriteName = s;
        }

        {
            string s = TableCard2.GetComponent<UISprite>().spriteName.Replace("_bw", "");
            TableCard2.GetComponent<UISprite>().spriteName = s;
        }

        {
            string s = TableCard3.GetComponent<UISprite>().spriteName.Replace("_bw", "");
            TableCard3.GetComponent<UISprite>().spriteName = s;
        }

        {
            string s = TableCard4.GetComponent<UISprite>().spriteName.Replace("_bw", "");
            TableCard4.GetComponent<UISprite>().spriteName = s;
        }

        for( int i = 0; i < winners.Count; i += 3 )
        {
            GameObject winner_player = GameObject.Find(players_indexes_names[winners[i]]);
            {
                string s = winner_player.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName.Replace("_bw", "");
                winner_player.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = s;
            }
            {
                string s = winner_player.transform.GetChild(1).gameObject.GetComponent<UISprite>().spriteName.Replace("_bw", "");
                winner_player.transform.GetChild(1).gameObject.GetComponent<UISprite>().spriteName = s;
            }
            {
                string s = winner_player.transform.GetChild(2).gameObject.GetComponent<UISprite>().spriteName.Replace("_bw", "");
                winner_player.transform.GetChild(2).gameObject.GetComponent<UISprite>().spriteName = s;
            }
            {
                {
                    string s1 = winner_player.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName.Replace("_bw", "");
                    winner_player.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<UISprite>().spriteName = s1;
                }
                {
                    string s1 = winner_player.transform.GetChild(9).gameObject.GetComponent<UISprite>().spriteName.Replace("_bw", "");
                    winner_player.transform.GetChild(9).gameObject.GetComponent<UISprite>().spriteName = s1;
                }

                winner_player.transform.GetChild(10).gameObject.SetActive(true);
                winner_player.transform.GetChild(10).gameObject.GetComponent<UILabel>().color = Color.green;
                winner_player.transform.GetChild(10).gameObject.GetComponent<UILabel>().text = winners[i + 2] + "\n won " + winners[i + 1];
            }

            foreach (Player p in table.players)
            {
                if (p.name == winners[i])
                {
                    byte s1 = table.the_deck_of_cards[p.cards_indexes[0]].suit;
                    byte v1 = table.the_deck_of_cards[p.cards_indexes[0]].value;
                    string c1 = card_by_value_and_suit(s1, v1);

                    winner_player.transform.GetChild(1).gameObject.GetComponent<UISprite>().spriteName = c1;
                    UITweener[] tweens = winner_player.transform.GetChild(1).gameObject.GetComponents<UITweener>();
                    foreach (UITweener tw in tweens)
                    {
                        if (tw.tweenGroup == 0)
                            tw.Play(true);
                    }

                    byte s2 = table.the_deck_of_cards[p.cards_indexes[1]].suit;
                    byte v2 = table.the_deck_of_cards[p.cards_indexes[1]].value;
                    string c2 = card_by_value_and_suit(s2, v2);
                    winner_player.transform.GetChild(2).gameObject.GetComponent<UISprite>().spriteName = c2;
                    UITweener[] tweens1 = winner_player.transform.GetChild(2).gameObject.GetComponents<UITweener>();
                    foreach (UITweener tw in tweens1)
                    {
                        if (tw.tweenGroup == 0)
                            tw.Play(true);
                    }
                }
            }
        }
    }

    void draw_players()
    {
        players_indexes.Clear();
        players_indexes_names.Clear();

        int total_players = 0;
        int my_index = 0;
        foreach (Player p in table.players)
        {
            if (string.Compare(CLEOS.account_name, p.name) == 0)
                my_index = total_players;
            total_players++;
        }

        CLEOS.my_table_index = my_index;
        total_players = table.current_game_players_count;

        setupPlayer(0, table.players[CLEOS.my_table_index].name, table.players[CLEOS.my_table_index].stack);
        enablePlayer(0, true);

        // right  hand players
        int counter = 1;
        for (int i = 0; i < my_index; i++)
        {
            setupPlayer(counter, table.players[(my_index - 1 ) - i ].name, table.players[(my_index - 1) - i].stack);
            enablePlayer(counter, true);
            counter++;
        }
        // left hand players
        counter = 8;
        for (int i = my_index + 1; i < total_players; i++)
        {
            setupPlayer(counter, table.players[i].name, table.players[i].stack);
            enablePlayer(counter, true);
            counter--;
        }

        for (int i = 0; i < 6; i++)
            enableTableDeckCard(i, true);

        if ( players_indexes.Count > 0 && table.dealer_index != -1)
        {
            {
                if (deallerCoin != null)
                    deallerCoin.SetActive(false);

                last_dealler_index = table.dealer_index;
                if (players_indexes_names.ContainsKey(table.players[table.dealer_index].name))
                {
                    GameObject dealer = GameObject.Find(players_indexes_names[table.players[table.dealer_index].name]);
                    deallerCoin = dealer.transform.GetChild(4).gameObject;
                    deallerCoin.SetActive(true);
                }
            }
        }
    }

    async void update_table()
    {
        try
        {
            GetTableRowsRequest request = new GetTableRowsRequest();
            request.Json = true;
            request.Code = CLEOS.contract_name;
            request.Scope = CLEOS.contract_name;
            request.Table = "tables";
            request.TableKey = "";
            request.LowerBound = CLEOS.table_id.ToString();
            request.UpperBound = "";
            request.Limit = 1;
            request.KeyType = "";
            request.IndexPosition = "";

            var Table = await CLEOS.eos_reader.GetTableRows(request);

            if(Table.Rows.Count != 0 )
                table = JsonUtility.FromJson<Table>(Table.Rows.First().ToString());
        
            CLEOS.current_bet = table.current_bet;
            CLEOS.raise_variants = table.raise_variants;
            CLEOS.table_index = table.id;

            if (RightPanel != null && RightPanel.GetComponent<UIScrollBar>() != null )
                RightPanel.GetComponent<UIScrollBar>().numberOfSteps = table.raise_variants.Count - 1;

            if(current_bet_label != null && current_bet_label.GetComponent<UILabel>() != null )
                current_bet_label.GetComponent<UILabel>().text = table.current_bet;

            if(current_bank_label != null && current_bank_label.GetComponent<UILabel>() != null )
                current_bank_label.GetComponent<UILabel>().text = table.current_bank;
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
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
            CLEOS.disconnect();
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
            CLEOS.disconnect();
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

    void Update ()
    {
		
	}

    IEnumerator Example()
    {
        reset_table();
        yield return new WaitForSeconds(3);
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        StartCoroutine(Example());
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    enum TableStatus
    {
        T_WAIT_PLAYER = 0,
        T_INIT_GAME,
        T_WAIT_START_GAME,
        T_WAIT_SHUFFLE,
        T_WAIT_CRYPT,
        T_MASTER_BLIND,
        T_WAIT_KEYS_FOR_PLAYERS,
        T_WAIT_PLAYERS_ACT,
        T_WAIT_KEYS_FOR_SHOWDOWN,
        T_MASTER_SHOWDOWN,
        T_WAIT_ALL_KEYS,
        T_WAIT_ALLIN_KEYS,
        T_END_GAME,
        T_WAIT_END_GAME,
        T_END_ALL_IN_GAME,
        T_DELETE
    };

    enum ActTypes
    {
        ACT_SMALL_BLIND = 0,
        ACT_BIG_BLIND,
        ACT_BET, // includes CALL and RAISE
        ACT_FOLD,
        ACT_CHECK,
        ACT_NEW_ROUND,
        ACT_CALL // only for possible_moves 
    };

    string last_act_time = "";
    async void get_info()
    {
        if (progressBar == null)
            return;

        if (firstTimeStart)
        {
            last_block = await CLEOS.eos_reader.GetInfo();
            firstTimeStart = false;
            last_act_time = CLEOS.last_table_timestamp;
        }

        if (last_act_time == table.last_act_time)
        {
            //Debug.Log(" last_act_time the same - " + last_act_time + " : " + last_act_time == table.last_act_time);

            float timeout = (float)(CLEOS.warning_timeout_sec + CLEOS.last_timeout_sec);
            if (table.table_status == (int)TableStatus.T_WAIT_PLAYERS_ACT && table.next_player_index != CLEOS.my_table_index)
            {
                timeout = (float)(CLEOS.warning_timeout_sec + CLEOS.last_timeout_sec) + 5.0f;
            }
            else if (table.table_status == (int)TableStatus.T_WAIT_PLAYERS_ACT && table.next_player_index == CLEOS.my_table_index)
            {
                timeout = (float)(CLEOS.warning_timeout_sec + CLEOS.last_timeout_sec);
            }
            else
                timeout = (float)CLEOS.last_timeout_sec;

            GetInfoResponse current_block = await CLEOS.eos_reader.GetInfo();
            //int elapsedSeconds = System.Math.Abs((current_block.HeadBlockTime - last_block.HeadBlockTime).Value.Seconds);
            long elapsedSeconds = ((DateTimeOffset)current_block.HeadBlockTime).ToUnixTimeSeconds() - ((DateTimeOffset)last_block.HeadBlockTime).ToUnixTimeSeconds();
            float persent = (float)elapsedSeconds / timeout;

            //Debug.Log(" ProgressBar changed - CURRENT_BLOCK = " + current_block.HeadBlockTime + " LAST_BLOCK = " + last_block.HeadBlockTime
           //     + " ELAPSED SECONDS = " + elapsedSeconds.ToString() + " PERSENT = " + persent.ToString() + " TIMEOUT = " + timeout.ToString());

            if (progressBar != null)
                progressBar.GetComponent<UISlider>().value = 1.0f - persent;

            if (progressBarLabel != null)
                progressBarLabel.GetComponent<UILabel>().text = (timeout - elapsedSeconds).ToString() + " Seconds left";

            if (elapsedSeconds < (timeout / 4))
            {
                if (progressBarLabel != null)
                    progressBarLabel.GetComponent<UILabel>().color = Color.white;
            }
            else
            {
                if (progressBarLabel != null)
                    progressBarLabel.GetComponent<UILabel>().color = Color.red;
            }

            if(end_game_timer != null)
                end_game_timer.GetComponent<UILabel>().text = (timeout - elapsedSeconds).ToString();

            if (elapsedSeconds >= timeout)
            {
                firstTimeStart = true;
                if (table.table_status == (int)TableStatus.T_WAIT_PLAYERS_ACT && table.next_player_index == CLEOS.my_table_index)
                {
                    if (fold_button.active == false)
                        fold_button.active = true;

                    CheckTurnAbility(true);
                    fold_button.GetComponent<FoldButtonBehaivor>().OnClick();
                }
                else
                {
                    if (table.table_status != (int)TableStatus.T_WAIT_PLAYER)
                    {
                        outfrom_table();
                        transaction_send_new_game_id = "";
                        transaction_wait_players = "1";
                    }
                }
            }
        }
        else
        {
            Debug.Log(" last_act_time was changed - " + last_act_time + " : " + last_act_time == table.last_act_time);
            firstTimeStart = true;
        } 
    }

    public async void reset_table()
    {
        Debug.Log("reset_table - " + table.id.ToString());
        try
        {
            var tmp = new List<object>();
            int len = personal_keys.Length;

            if (table.players[CLEOS.my_table_index].status == (int)PlayerStatus.P_WAIT_NEW_GAME || table.table_status == (int)TableStatus.T_WAIT_END_GAME || table.table_status == (int)TableStatus.T_WAIT_START_GAME)
                len = 0;
            else
            {
                for (int i = 0; i < len; i++)
                {
                    if (table.players[CLEOS.my_table_index].cards_indexes.Count == 0)
                    {
                        if ((i == CLEOS.my_table_index * 2) || (i == (CLEOS.my_table_index * 2) + 1))
                            continue;
                    }
                    else
                    {
                        if (i == table.players[CLEOS.my_table_index].cards_indexes[0] || i == table.players[CLEOS.my_table_index].cards_indexes[1])
                            continue;
                    }

                    List<object> o = new List<object>();
                    for (int z = 0; z < 32; z++)
                        o.Add(personal_keys[i].data[z]);

                    List<object> s1 = new List<object>();
                    for (int z = 0; z < 8; z++)
                        s1.Add(personal_keys[i].s[z]);

                    List<object> m1 = new List<object>();
                    for (int z = 0; z < 8; z++)
                        m1.Add(personal_keys[i].m[z]);

                    tmp.Add(new { data = o, card_index = (byte)personal_keys[i].card_index, s = s1, m = m1 });
                }
            }

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
                        Data = new { name = CLEOS.account_name, table_id = table.id, keys = tmp }
                    }
                }
            });

            transaction_reset_table_id = result;
            Debug.Log(" outfromtable result - " + transaction_reset_table_id);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            if (AlertMessage != null)
            {
                AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
            Debug.Log(JsonConvert.SerializeObject(e));
        }
    }

    public async void outfrom_table()
    {
        Debug.Log("reset_table - " + table.id.ToString());
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
                        Name = "resettable",
                        Data = new { name = CLEOS.account_name, table_id = table.id, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            transaction_reset_table_id = result;
            Debug.Log(" resettable result - " + transaction_reset_table_id);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            if (AlertMessage != null)
            {
                AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
            Debug.Log(JsonConvert.SerializeObject(e));
        }
    }

    public async void send_new_game()
    {
        Debug.Log(" TRY TO SEND - send_new_game");
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
                        Name = "sendnewgame",
                        Data = new { name = CLEOS.account_name, table_id = table.id, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            transaction_send_new_game_id = result;
            Debug.Log("send_new_game - " + transaction_send_new_game_id + CLEOS.account_name);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));
        }

        Debug.Log(" TRY TO SEND - send_new_game SENDED");
    }

    public byte[] src_buffer_fro_crypt_card = new byte[8];
    public byte[] dst_buffer_fro_crypt_card = new byte[8];
    public byte[] mac_buffer_fro_crypt_card = new byte[8];
    public void crypt_card(Card c, Key key_data, bool crypt)
    {
        src_buffer_fro_crypt_card[0] = c.suit;
        src_buffer_fro_crypt_card[1] = c.value;
        src_buffer_fro_crypt_card[2] = 0;
        src_buffer_fro_crypt_card[3] = 0;
        src_buffer_fro_crypt_card[4] = 0;
        src_buffer_fro_crypt_card[5] = 0;
        src_buffer_fro_crypt_card[6] = 0;
        src_buffer_fro_crypt_card[7] = 0;

        key_data.setup();

        dst_buffer_fro_crypt_card[0] = 0;
        dst_buffer_fro_crypt_card[1] = 0;
        dst_buffer_fro_crypt_card[2] = 0;
        dst_buffer_fro_crypt_card[3] = 0;
        dst_buffer_fro_crypt_card[4] = 0;
        dst_buffer_fro_crypt_card[5] = 0;
        dst_buffer_fro_crypt_card[6] = 0;
        dst_buffer_fro_crypt_card[7] = 0;

        mac_buffer_fro_crypt_card[0] = 0;
        mac_buffer_fro_crypt_card[1] = 0;
        mac_buffer_fro_crypt_card[2] = 0;
        mac_buffer_fro_crypt_card[3] = 0;
        mac_buffer_fro_crypt_card[4] = 0;
        mac_buffer_fro_crypt_card[5] = 0;
        mac_buffer_fro_crypt_card[6] = 0;
        mac_buffer_fro_crypt_card[7] = 0;

        if (crypt)
        {
            IntPtr returnedPtr = CryptoPlugin.get_mac1(src_buffer_fro_crypt_card, key_data.data_i, key_data.s_i);
            Marshal.Copy(returnedPtr, mac_buffer_fro_crypt_card, 0, 8);
            CryptoPlugin.freeMem(returnedPtr);
            Buffer.BlockCopy(mac_buffer_fro_crypt_card, 0, key_data.m, 0, 8);

            returnedPtr = CryptoPlugin.crypt_data1(src_buffer_fro_crypt_card, key_data.data_i, key_data.s_i);
            Marshal.Copy(returnedPtr, dst_buffer_fro_crypt_card, 0, 8);
            CryptoPlugin.freeMem(returnedPtr);
        }
        else
        {
            IntPtr returnedPtr = CryptoPlugin.decrypt_data1(src_buffer_fro_crypt_card, key_data.data_i, key_data.s_i);
            Marshal.Copy(returnedPtr, dst_buffer_fro_crypt_card, 0, 8);
            CryptoPlugin.freeMem(returnedPtr);       
        }

        c.suit = dst_buffer_fro_crypt_card[0];
        c.value = dst_buffer_fro_crypt_card[1];
    }

    public void init_key( Key in_k )
    {
        CryptoPlugin.generate_random_range(in_k.data, 32);
        CryptoPlugin.generate_random_range(in_k.s, 8);
        in_k.setup();
    }

    public void init_fake_key(Key in_k)
    {
        for (int i = 0; i < 32; i++)
            in_k.data[i] = 1;

        for (int i = 0; i < 8; i++)
            in_k.s[i] = 1;

        in_k.setup();
    }

    async void shuffle_cards()
    {
        List<int> indexes = new List<int>();
        List<Card> deck_of_cards = table.the_deck_of_cards.GetRange(0, table.the_deck_of_cards.Count);

        List<Card> shuffled_deck_of_cards = new List<Card>();
        init_key(shuffle_personal_key);
        
        int i = 0;
        foreach( Card c in deck_of_cards)
        {
            indexes.Add(i);
            i++;
        }
        
        int[] arr = indexes.ToArray();
        ShufflePlugin.shuffle(arr, indexes.Count);

        indexes = new List<int>(arr);

        string s = "";
        foreach (int c in indexes)
        {
            s += " " + c.ToString();        
            shuffled_deck_of_cards.Add(deck_of_cards[c]);
        }

        deck_of_cards = shuffled_deck_of_cards.GetRange(0, shuffled_deck_of_cards.Count); 

        foreach (Card c in deck_of_cards)
            crypt_card(c, shuffle_personal_key, true);
   
        Debug.Log("shuffle_cards - " + table.id.ToString());
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
                        Name = "shuffleddeck",
                        Data = new { name = CLEOS.account_name, table_id = table.id, cards = new List<object>() {
                            new { suit = deck_of_cards[0].suit, value = deck_of_cards[0].value },
                            new { suit = deck_of_cards[1].suit, value = deck_of_cards[1].value },
                            new { suit = deck_of_cards[2].suit, value = deck_of_cards[2].value },
                            new { suit = deck_of_cards[3].suit, value = deck_of_cards[3].value },
                            new { suit = deck_of_cards[4].suit, value = deck_of_cards[4].value },
                            new { suit = deck_of_cards[5].suit, value = deck_of_cards[5].value },
                            new { suit = deck_of_cards[6].suit, value = deck_of_cards[6].value },
                            new { suit = deck_of_cards[7].suit, value = deck_of_cards[7].value },
                            new { suit = deck_of_cards[8].suit, value = deck_of_cards[8].value },
                            new { suit = deck_of_cards[9].suit, value = deck_of_cards[9].value },
                            new { suit = deck_of_cards[10].suit, value = deck_of_cards[10].value },
                            new { suit = deck_of_cards[11].suit, value = deck_of_cards[11].value },
                            new { suit = deck_of_cards[12].suit, value = deck_of_cards[12].value },
                            new { suit = deck_of_cards[13].suit, value = deck_of_cards[13].value },
                            new { suit = deck_of_cards[14].suit, value = deck_of_cards[14].value },
                            new { suit = deck_of_cards[15].suit, value = deck_of_cards[15].value },
                            new { suit = deck_of_cards[16].suit, value = deck_of_cards[16].value },
                            new { suit = deck_of_cards[17].suit, value = deck_of_cards[17].value },
                            new { suit = deck_of_cards[18].suit, value = deck_of_cards[18].value },
                            new { suit = deck_of_cards[19].suit, value = deck_of_cards[19].value },
                            new { suit = deck_of_cards[20].suit, value = deck_of_cards[20].value },
                            new { suit = deck_of_cards[21].suit, value = deck_of_cards[21].value },
                            new { suit = deck_of_cards[22].suit, value = deck_of_cards[22].value },
                            new { suit = deck_of_cards[23].suit, value = deck_of_cards[23].value },
                            new { suit = deck_of_cards[24].suit, value = deck_of_cards[24].value },
                            new { suit = deck_of_cards[25].suit, value = deck_of_cards[25].value },
                            new { suit = deck_of_cards[26].suit, value = deck_of_cards[26].value },
                            new { suit = deck_of_cards[27].suit, value = deck_of_cards[27].value },
                            new { suit = deck_of_cards[28].suit, value = deck_of_cards[28].value },
                            new { suit = deck_of_cards[29].suit, value = deck_of_cards[29].value },
                            new { suit = deck_of_cards[30].suit, value = deck_of_cards[30].value },
                            new { suit = deck_of_cards[31].suit, value = deck_of_cards[31].value },
                            new { suit = deck_of_cards[32].suit, value = deck_of_cards[32].value },
                            new { suit = deck_of_cards[33].suit, value = deck_of_cards[33].value },
                            new { suit = deck_of_cards[34].suit, value = deck_of_cards[34].value },
                            new { suit = deck_of_cards[35].suit, value = deck_of_cards[35].value },
                            new { suit = deck_of_cards[36].suit, value = deck_of_cards[36].value },
                            new { suit = deck_of_cards[37].suit, value = deck_of_cards[37].value },
                            new { suit = deck_of_cards[38].suit, value = deck_of_cards[38].value },
                            new { suit = deck_of_cards[39].suit, value = deck_of_cards[39].value },
                            new { suit = deck_of_cards[40].suit, value = deck_of_cards[40].value },
                            new { suit = deck_of_cards[41].suit, value = deck_of_cards[41].value },
                            new { suit = deck_of_cards[42].suit, value = deck_of_cards[42].value },
                            new { suit = deck_of_cards[43].suit, value = deck_of_cards[43].value },
                            new { suit = deck_of_cards[44].suit, value = deck_of_cards[44].value },
                            new { suit = deck_of_cards[45].suit, value = deck_of_cards[45].value },
                            new { suit = deck_of_cards[46].suit, value = deck_of_cards[46].value },
                            new { suit = deck_of_cards[47].suit, value = deck_of_cards[47].value },
                            new { suit = deck_of_cards[48].suit, value = deck_of_cards[48].value },
                            new { suit = deck_of_cards[49].suit, value = deck_of_cards[49].value },
                            new { suit = deck_of_cards[50].suit, value = deck_of_cards[50].value },
                            new { suit = deck_of_cards[51].suit, value = deck_of_cards[51].value },
                            
                        }, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            transaction_shuffle_cards = result;
            Debug.Log(" shuffleddeck  result - " + transaction_shuffle_cards);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));
        }
    }

    async void crypted_cards()
    {
        List<Card> deck_of_cards = table.the_deck_of_cards.GetRange(0, table.the_deck_of_cards.Count);

        foreach (Card c in deck_of_cards)
            crypt_card(c, shuffle_personal_key, false);

        int i = 0;
        foreach (Card c in deck_of_cards)
        {
            init_key(personal_keys[i]);
            personal_keys[i].card_index = i;
            crypt_card(c, personal_keys[i], true);
            i++;
        }
 
        Debug.Log("crypted_cards - " + table.id.ToString());
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
                        Name = "crypteddeck",
                        Data = new { name = CLEOS.account_name, table_id = table.id, cards = new List<object>() {
                            new { suit = deck_of_cards[0].suit, value = deck_of_cards[0].value },
                            new { suit = deck_of_cards[1].suit, value = deck_of_cards[1].value },
                            new { suit = deck_of_cards[2].suit, value = deck_of_cards[2].value },
                            new { suit = deck_of_cards[3].suit, value = deck_of_cards[3].value },
                            new { suit = deck_of_cards[4].suit, value = deck_of_cards[4].value },
                            new { suit = deck_of_cards[5].suit, value = deck_of_cards[5].value },
                            new { suit = deck_of_cards[6].suit, value = deck_of_cards[6].value },
                            new { suit = deck_of_cards[7].suit, value = deck_of_cards[7].value },
                            new { suit = deck_of_cards[8].suit, value = deck_of_cards[8].value },
                            new { suit = deck_of_cards[9].suit, value = deck_of_cards[9].value },
                            new { suit = deck_of_cards[10].suit, value = deck_of_cards[10].value },
                            new { suit = deck_of_cards[11].suit, value = deck_of_cards[11].value },
                            new { suit = deck_of_cards[12].suit, value = deck_of_cards[12].value },
                            new { suit = deck_of_cards[13].suit, value = deck_of_cards[13].value },
                            new { suit = deck_of_cards[14].suit, value = deck_of_cards[14].value },
                            new { suit = deck_of_cards[15].suit, value = deck_of_cards[15].value },
                            new { suit = deck_of_cards[16].suit, value = deck_of_cards[16].value },
                            new { suit = deck_of_cards[17].suit, value = deck_of_cards[17].value },
                            new { suit = deck_of_cards[18].suit, value = deck_of_cards[18].value },
                            new { suit = deck_of_cards[19].suit, value = deck_of_cards[19].value },
                            new { suit = deck_of_cards[20].suit, value = deck_of_cards[20].value },
                            new { suit = deck_of_cards[21].suit, value = deck_of_cards[21].value },
                            new { suit = deck_of_cards[22].suit, value = deck_of_cards[22].value },
                            new { suit = deck_of_cards[23].suit, value = deck_of_cards[23].value },
                            new { suit = deck_of_cards[24].suit, value = deck_of_cards[24].value },
                            new { suit = deck_of_cards[25].suit, value = deck_of_cards[25].value },
                            new { suit = deck_of_cards[26].suit, value = deck_of_cards[26].value },
                            new { suit = deck_of_cards[27].suit, value = deck_of_cards[27].value },
                            new { suit = deck_of_cards[28].suit, value = deck_of_cards[28].value },
                            new { suit = deck_of_cards[29].suit, value = deck_of_cards[29].value },
                            new { suit = deck_of_cards[30].suit, value = deck_of_cards[30].value },
                            new { suit = deck_of_cards[31].suit, value = deck_of_cards[31].value },
                            new { suit = deck_of_cards[32].suit, value = deck_of_cards[32].value },
                            new { suit = deck_of_cards[33].suit, value = deck_of_cards[33].value },
                            new { suit = deck_of_cards[34].suit, value = deck_of_cards[34].value },
                            new { suit = deck_of_cards[35].suit, value = deck_of_cards[35].value },
                            new { suit = deck_of_cards[36].suit, value = deck_of_cards[36].value },
                            new { suit = deck_of_cards[37].suit, value = deck_of_cards[37].value },
                            new { suit = deck_of_cards[38].suit, value = deck_of_cards[38].value },
                            new { suit = deck_of_cards[39].suit, value = deck_of_cards[39].value },
                            new { suit = deck_of_cards[40].suit, value = deck_of_cards[40].value },
                            new { suit = deck_of_cards[41].suit, value = deck_of_cards[41].value },
                            new { suit = deck_of_cards[42].suit, value = deck_of_cards[42].value },
                            new { suit = deck_of_cards[43].suit, value = deck_of_cards[43].value },
                            new { suit = deck_of_cards[44].suit, value = deck_of_cards[44].value },
                            new { suit = deck_of_cards[45].suit, value = deck_of_cards[45].value },
                            new { suit = deck_of_cards[46].suit, value = deck_of_cards[46].value },
                            new { suit = deck_of_cards[47].suit, value = deck_of_cards[47].value },
                            new { suit = deck_of_cards[48].suit, value = deck_of_cards[48].value },
                            new { suit = deck_of_cards[49].suit, value = deck_of_cards[49].value },
                            new { suit = deck_of_cards[50].suit, value = deck_of_cards[50].value },
                            new { suit = deck_of_cards[51].suit, value = deck_of_cards[51].value },
                            
                        }, timestamp = CLEOS.last_table_timestamp_abi }
                    }
                }
            });

            transaction_crypted_cards = result;
            Debug.Log(" crypteddeck  result - " + transaction_crypted_cards);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            Debug.Log(JsonConvert.SerializeObject(e));
            if (AlertMessage != null)
            {
                AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
                UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                foreach (UITweener tw in tweens)
                {
                    if (tw.tweenGroup == 0)
                        tw.Play(true);
                }
            }
        }
    }

    async void send_keys_from_deck()
    {
        int count = table.current_game_players_count * 2 - 2;

        var tmp = new List<object>();
        foreach( byte k in table.waiting_keys_indexes )
        {
            if (k == table.players[CLEOS.my_table_index].cards_indexes[0])
                continue;

            if (k == table.players[CLEOS.my_table_index].cards_indexes[1])
                continue;

            tmp.Add(personal_keys[k]);
        }

        Debug.Log("send_keys_from_deck - " + table.id.ToString());
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
                        Name = "setcardskeys",
                        Data = new { name = CLEOS.account_name, table_id = table.id, keys = tmp, timestamp = CLEOS.last_table_timestamp_abi  }
                    }
                }
            });

            transaction_setcardskeys = result;
            Debug.Log(" setcardskeys  result - " + transaction_setcardskeys);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));
        }
    }

    async void send_personal_keys_for_showdown( List<Key> data )
    {
        try
        {
            var tmp = new List<object>();
            for (int i = 0; i < data.Count; i++)
            {
                List<object> o = new List<object>();
                for ( int z = 0; z < 32; z++ )
                    o.Add(data[i].data[z]);

                List<object> s1 = new List<object>();
                for (int z = 0; z < 8; z++)
                    s1.Add(data[i].s[z]);

                List<object> m1 = new List<object>();
                for (int z = 0; z < 8; z++)
                    m1.Add(data[i].m[z]);

               // Debug.Log("cards index = " + i.ToString());


                tmp.Add(new { data = o, card_index = (byte)data[i].card_index, s = s1, m = m1 });
            }

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
                        Name = "setcardskeys",
                        Data = new { name = CLEOS.account_name, table_id = table.id, keys = tmp, timestamp = CLEOS.last_table_timestamp_abi  }
                    }
                }
            });

            transaction_setcardskeys_for_showdown = result;
            Debug.Log(" transaction_setcardskeys_for_showdown  result - " + transaction_setcardskeys_for_showdown);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            Debug.Log(JsonConvert.SerializeObject(e));
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
        }
    }

    public void CheckTurnAbility( bool just_turn_off )
    {
        if (just_turn_off)
        {
            {
                if(raise_button.GetComponent<UIRaiseBehaivor>().int_enabled == true)
                {
                    raise_button.GetComponent<UIRaiseBehaivor>().OnClick();
                    raise_button.GetComponent<UIToggle>().Set(false, false);
                    raise_button.GetComponent<TweenScale>().Play(false);
                }
            }
        }

        raise_button.SetActive(false);
        fold_button.SetActive(false);
        call_button.SetActive(false);
        check_button.SetActive(false);

        if (table.id == -1 || just_turn_off)
            return;

        foreach( byte b in table.possible_moves)
        {
            switch(b)
            {
                case (byte)ActTypes.ACT_BET:
                    raise_button.SetActive(true);
                    break;

                case (byte)ActTypes.ACT_CALL:
                    call_button.SetActive(true);
                    break;

                case (byte)ActTypes.ACT_FOLD:
                    fold_button.SetActive(true);
                    break;

                case (byte)ActTypes.ACT_CHECK:
                    check_button.SetActive(true);
                    break;
            }
        }
        CLEOS.permission_to_make_turn = true;
    }

    string card_by_value_and_suit( int suit, int value )
    {
        string result = "";
        switch (suit)
           {
                case 0:
                    result += "P_";
                    break;

                case 1:
                    result += "H_";
                    break;

                case 2:
                    result += "B_";
                    break;

                case 3:
                    result += "T_";
                    break;
            }

            switch (value)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    result += value.ToString();
                    break;

                case 11:
                    result += "J";
                    break;

                case 12:
                    result += "Q";
                    break;

                case 13:
                    result += "K";
                    break;

                case 14:
                    result += "A";
                    break;
            }
        return result;
    }

    public byte[] cr_buffer = new byte[8];
    public Card cr_fro_test = new Card();

    void readTableCardsEnd()
    {
        if (table.id == -1)
            return;

        TableCard0.SetActive(false);
        TableCard1.SetActive(false);
        TableCard2.SetActive(false);
        TableCard3.SetActive(false);
        TableCard4.SetActive(false);

        List<int> combination_cards = new List<int>();

        int i = 0;
        foreach (Card c in table.table_cards)
        {
            combination_cards.Add(c.suit);
            combination_cards.Add(c.value);

            string s = card_by_value_and_suit(c.suit, c.value);
            switch (i)
            {
                case 0:
                    TableCard0.SetActive(true);
                    TableCard0.GetComponent<UISprite>().spriteName = s;
                    break;

                case 1:
                    TableCard1.SetActive(true);
                    TableCard1.GetComponent<UISprite>().spriteName = s;
                    break;

                case 2:
                    TableCard2.SetActive(true);
                    TableCard2.GetComponent<UISprite>().spriteName = s;
                    break;

                case 3:
                    TableCard3.SetActive(true);
                    TableCard3.GetComponent<UISprite>().spriteName = s;
                    break;

                case 4:
                    TableCard4.SetActive(true);
                    TableCard4.GetComponent<UISprite>().spriteName = s;
                    break;
            }
            i++;
        }

    }

    void readTableCards()
    {
        if (table.id == -1)
            return;

        TableCard0.SetActive(false);
        TableCard1.SetActive(false);
        TableCard2.SetActive(false);
        TableCard3.SetActive(false);
        TableCard4.SetActive(false);

        List<int> combination_cards = new List<int>();

        int i = 0;
        foreach( Card c in table.table_cards )
        {
            combination_cards.Add(c.suit);
            combination_cards.Add(c.value);

            string s = card_by_value_and_suit(c.suit, c.value);
            switch(i)
            {
                case 0:
                    TableCard0.SetActive(true);
                    TableCard0.GetComponent<UISprite>().spriteName = s;
                    break;

                case 1:
                    TableCard1.SetActive(true);
                    TableCard1.GetComponent<UISprite>().spriteName = s;
                    break;

                case 2:
                    TableCard2.SetActive(true);
                    TableCard2.GetComponent<UISprite>().spriteName = s;
                    break;

                case 3:
                    TableCard3.SetActive(true);
                    TableCard3.GetComponent<UISprite>().spriteName = s;
                    break;

                case 4:
                    TableCard4.SetActive(true);
                    TableCard4.GetComponent<UISprite>().spriteName = s;
                    break;
            }
            i++;
        }

        int counter = 0;
        foreach (int index in table.players[CLEOS.my_table_index].cards_indexes)
        {
            bool fold = table.players[CLEOS.my_table_index].status == (int)PlayerStatus.P_FOLD;
            bool bout = table.players[CLEOS.my_table_index].status == (int)PlayerStatus.P_OUT;
            if (!fold)
                fold = bout;

            cr_fro_test.suit = table.the_deck_of_cards[index].suit;
            cr_fro_test.value = table.the_deck_of_cards[index].value;

            foreach (TableKey k in table.all_keys )
            {
                if( k.card_index == index )
                {
                    tmp_key.setup_from_table_key(k);                    
                    crypt_card(cr_fro_test, tmp_key, false);                  
                }
            }

            crypt_card(cr_fro_test, personal_keys[index], false);

            combination_cards.Add(cr_fro_test.suit);
            combination_cards.Add(cr_fro_test.value);

            string r = card_by_value_and_suit(cr_fro_test.suit, cr_fro_test.value);
            if (counter == 0)
            {
                GameObject child_card0 = Player0.transform.GetChild(1).gameObject;
                child_card0.SetActive(!fold);
                child_card0.GetComponent<UISprite>().spriteName = r;
            }

            if (counter == 1)
            {
                GameObject child_card1 = Player0.transform.GetChild(2).gameObject;
                child_card1.SetActive(!fold);
                child_card1.GetComponent<UISprite>().spriteName = r;
            }
            counter++;
        }

        for (int z = 0; z < table.current_game_players_count; z++)
        {
            if (z == CLEOS.my_table_index)
                continue;

            bool fold = table.players[z].status == (int)PlayerStatus.P_FOLD;
            bool bout = table.players[z].status == (int)PlayerStatus.P_OUT;
            if (!fold)
                fold = bout;

            if (players_indexes_names.ContainsKey(table.players[z].name))
            {
                GameObject p = GameObject.Find(players_indexes_names[table.players[z].name]);
                p.transform.GetChild(1).gameObject.SetActive(!fold);
                p.transform.GetChild(1).gameObject.GetComponent<UISprite>().spriteName = "Back";

                p.transform.GetChild(2).gameObject.SetActive(!fold);
                p.transform.GetChild(2).gameObject.GetComponent<UISprite>().spriteName = "Back";
            }
        }

        //Draw combination
        if(combination_cards.Count >= 5 )
            CombinationPlugin.current_combination = CombinationPlugin.get_combination(combination_cards.ToArray(), combination_cards.Count / 2 );
    }

    enum CombinationTypes
    {
        C_NO_COMBINATION = 0,
        C_HIGH_CARD,
        C_PAIR,
        C_TWO_PAIRS,
        C_THREE_OF_A_KIND,
        C_STRAIGHT,
        C_FLUSH,
        C_FULL_HOUSE,
        C_FOUR_OF_A_KIND,
        C_STRAIGHT_FLUSH,
        C_ROYAL_FLUSH
    };

    void endGame()
    {
        make_grayscale(true);
       
        string result = "";
        List<string> winners = new List<string>();
        foreach ( PlayerHistoryInfo info in  table.history[table.history.Count - 1].players_info )
        {
            if (info.winnings.CompareTo("0.0000 EOS") == 0)
                continue;

            winners.Add(info.name);
            winners.Add(info.winnings);

            result += info.name + " won - " + info.winnings + " combo cards - ";
            string combo = "";
            switch (info.combo.type)
            {
                case 0:
                    combo = " no combination";
                    break;
                case 1:
                    combo = " high card";
                    break;
                case 2:
                    combo = " pair";
                    break;
                case 3:
                    combo = " two pairs";
                    break;
                case 4:
                    combo = " three of a kind";
                    break;
                case 5:
                    combo = " straight";
                    break;
                case 6:
                    combo = " flush";
                    break;
                case 7:
                    combo = " full house";
                    break;
                case 8:
                    combo = " four of a kind";
                    break;
                case 9:
                    combo = " straight flush";
                    break;
                case 10:
                    combo = " royal flush";
                    break;

            }
            winners.Add(combo);
            result += combo;

            string cards = " cards = ";
            foreach( Card c in info.combo.cards )
                cards += card_by_value_and_suit(c.suit, c.value) + " ";

            result += cards;
            result += "\n";
        }

        if (raise_button.GetComponent<UIRaiseBehaivor>().int_enabled == true)
        {
            raise_button.GetComponent<UIRaiseBehaivor>().OnClick();
            raise_button.GetComponent<UIToggle>().Set(false, false);
            raise_button.GetComponent<TweenScale>().Play(false);
        }

        if( show_cards_button.GetComponent<UICardsShowBehaivor>().int_enabled == true)
        {
            show_cards_button.GetComponent<UICardsShowBehaivor>().OnClick();
            show_cards_button.GetComponent<UIToggle>().Set(false, false);
            show_cards_button.GetComponent<TweenScale>().Play(false);
        }
        
        UITweener[] tweens = EndGameWindow.GetComponents<UITweener>();
        foreach (UITweener tw in tweens)
        {
            if (tw.tweenGroup == 0)
                tw.Play(true);
        }

        make_end_game(winners);
        Debug.Log(result);
    }

    void drawPlayersTurns()
    {
        for ( int i = 0; i < table.current_game_players_count; i++ )
        {
            if( i == table.next_player_index)
            {
                if (players_indexes_names.ContainsKey(table.players[i].name))
                {
                    GameObject p = GameObject.Find(players_indexes_names[table.players[i].name]);
                    GameObject child_player_name = p.transform.GetChild(6).gameObject;
                    child_player_name.GetComponent<UILabel>().color = Color.green;
                    child_player_name.GetComponent<TweenScale>().enabled = true;

                    GameObject child_avatar = p.transform.GetChild(0).gameObject;
                    child_player_name.GetComponent<TweenScale>().enabled = true;
                }
            }
            else
            {
                if (players_indexes_names.ContainsKey(table.players[i].name))
                {
                    GameObject p = GameObject.Find(players_indexes_names[table.players[i].name]);
                    GameObject child_player_name = p.transform.GetChild(6).gameObject;
                    child_player_name.GetComponent<UILabel>().color = Color.white;
                    child_player_name.GetComponent<TweenScale>().enabled = false;

                    GameObject child_avatar = p.transform.GetChild(0).gameObject;
                    child_avatar.GetComponent<TweenScale>().enabled = false;
                }
            }
        }

        List<PlayerAct> currentActs = new List<PlayerAct>();
        for (int i = 0; i < table.players_acts.Count; i++)
        {
            if (table.players_acts[i].name_.CompareTo(CLEOS.contract_name) == 0)
                currentActs = new List<PlayerAct>();
            
            currentActs.Add(table.players_acts[i]);
        }

        Dictionary<string, string> tmp = new Dictionary<string, string>();  
        foreach (PlayerAct act in currentActs)
        {
            string r = "";
            switch( act.act_.act_ )
            {
                case (int)ActTypes.ACT_SMALL_BLIND:
                    r = "Small Blind\n";
                    r += " " + act.act_.bet_ + "\n";
                    break;

                case (int)ActTypes.ACT_BIG_BLIND:
                    r = "Big Blind\n";
                    r += " " + act.act_.bet_ + "\n";
                    break;

                case (int)ActTypes.ACT_BET:
                    r = "Bet\n";
                    r += " " + act.act_.bet_ + "\n";
                    break;

                case (int)ActTypes.ACT_FOLD:
                    r = "Fold\n";
                    r += " " + act.act_.bet_ + "\n";
                    break;

                case (int)ActTypes.ACT_CHECK:
                    r = "Check\n";
                    r += " " + act.act_.bet_ + "\n";
                    break;

                case (int)ActTypes.ACT_NEW_ROUND:
                    r = "";
                    break;

            }

            if (tmp.ContainsKey(act.name_))
            {
                tmp.Remove(act.name_);
                tmp.Add(act.name_, r);

                if(act.name_.CompareTo(CLEOS.contract_name) != 0 )
                    tmp.Remove(CLEOS.contract_name);
            }
            else
                tmp.Add(act.name_, r);
        }

        foreach (KeyValuePair<string, string> item in tmp)
        {
            if (item.Key.CompareTo(CLEOS.contract_name) != 0)
            {
                if (players_indexes_names.ContainsKey(item.Key))
                {
                    GameObject o = GameObject.Find(players_indexes_names[item.Key]);
                    GameObject turn_history = o.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = item.Value;
                }
            }else
            {
                {
                    GameObject turn_history = Player0.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player1.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player2.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player3.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player4.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player5.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player6.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player7.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player8.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
                {
                    GameObject turn_history = Player9.transform.GetChild(8).gameObject;
                    turn_history.GetComponent<UILabel>().text = "";
                }
            }
        }
    }

    string amount_to_string(double data)
    {
        var cl = CultureInfo.CreateSpecificCulture("es-ES");
        cl.NumberFormat.NumberDecimalSeparator = ".";
        cl.NumberFormat.NumberGroupSeparator = "";
        return data.ToString("N4", cl) + CLEOS.symbol;
    }

    async void transfer(double amount)
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
            CLEOS.disconnect();
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
            CLEOS.disconnect();
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
            CLEOS.disconnect();
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

    public async void sned_withdraw_action()
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

            transaction_withdraw_id = result;
            Debug.Log(" sned_withdraw result - " + transaction_withdraw_id);
        }
        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
            AlertMessage.GetComponent<UILabel>().text = e.Error.Name + " : " + e.Error.What + e.Error.Details[0].Message;
            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
            foreach (UITweener tw in tweens)
            {
                if (tw.tweenGroup == 0)
                    tw.Play(true);
            }
            Debug.Log(JsonConvert.SerializeObject(e));
        }
    }

    public async void get_accounts()
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
                    current_account = JsonUtility.FromJson<account>(row.ToString());
                    if(current_account.table_id_.Count == 0 && CLEOS.skip_mainloop != true)
                    {
                        if (current_account.out_reason.Length != 0)
                        {
                            AlertMessage.GetComponent<UILabel>().text = current_account.out_reason;
                            UITweener[] tweens = AlertWindow.GetComponents<UITweener>();
                            foreach (UITweener tw in tweens)
                            {
                                if (tw.tweenGroup == 0)
                                    tw.Play(true);
                            }
                        }
                        else
                        {
                            run_main_loop = false;
                            SceneManager.LoadScene("ChoseTable");
                        }
                    }
                }
            }
        }

        catch (EosSharp.Exceptions.ApiErrorException e)
        {
            CLEOS.disconnect();
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
            CLEOS.disconnect();
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
            CLEOS.disconnect();
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

    IEnumerator gameMainLoop()
    {
        while (run_main_loop)
        {
            yield return new WaitForSeconds(0.5f);
            try
                {
                    CLEOS.game_loop_counter++;
                    CLEOS.reload_data = false;
                    CLEOS.counter = 0;
                    
                    get_accounts();
                    update_table();
                    get_info();

                    table.Trace();
                   
                    if (table.id == -1)
                    {
                        Debug.Log("table entity is null");
                        continue;
                    }

                    draw_players();

                    if (CLEOS.skip_mainloop == true )
                    {
                        Debug.Log("CLEOS.skip_mainloop == true");
                        continue;
                    }

                    if (table.last_act_time != CLEOS.last_table_timestamp)
                    {                   
                        CLEOS.need_to_update_scene = true;
                        CLEOS.last_table_timestamp = table.last_act_time;
                        CLEOS.last_table_timestamp_abi = table.timestamp;
                    }
                    else
                        CLEOS.need_to_update_scene = false;

                if (CLEOS.need_to_update_scene != true)
                    {
                        if (table.table_status == (int)TableStatus.T_WAIT_START_GAME && transaction_send_new_game_id.Length == 0)
                        {
                            if (table.players[CLEOS.my_table_index].status != 0)
                            {
                                send_new_game();
                                transaction_send_new_game_id = "1";
                                transaction_transfer_id = "";
                            }
                        }
                        continue;
                    }

                switch (table.table_status)
                    {
                        case (int)TableStatus.T_WAIT_START_GAME:
                        if (transaction_wait_players.Length != 0)
                            {
                                run_main_loop = false;
                                SceneManager.LoadScene("TableScene");
                                break;
                            }

                        if (transaction_send_new_game_id.Length == 0)
                            {
                                if (table.players[CLEOS.my_table_index].status != 0)
                                    send_new_game();

                                transaction_send_new_game_id = "1";
                                transaction_transfer_id = "";

                                for (int i = 0; i < 9; i++)
                                    enablePlayer(i, false);
                            }
                            break;

                        case (int)TableStatus.T_WAIT_PLAYER:
                        if (transaction_wait_players.Length != 0)
                            {
                                run_main_loop = false;
                                SceneManager.LoadScene("TableScene");
                                break;
                            }
                            else
                            {
                                transaction_send_new_game_id = "";
                            }
                            break;

                        case (int)TableStatus.T_WAIT_END_GAME:
                        if (CLEOS.auto_re_buy)
                            {
                                if (transaction_transfer_id.Length == 0)
                                {
                                    double stack;
                                    double quantity;
                                    {
                                        string[] arr = table.players[CLEOS.my_table_index].stack.Split(' ');
                                        arr[0] = arr[0].Replace(".", ",");
                                        String number = arr[0];
                                        stack = double.Parse(number, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("es-ES"));
                                    }

                                    {
                                        string[] arr = current_account.quantity_.Split(' ');
                                        arr[0] = arr[0].Replace(".", ",");
                                        String number = arr[0];
                                        quantity = double.Parse(number, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("es-ES"));
                                    }

                                    if ((stack + quantity) < CLEOS.bay_in)
                                    {
                                        transfer(CLEOS.bay_in - (stack + quantity));
                                    }
                                    transaction_transfer_id = "1";
                                }
                            }
                            else
                                transaction_transfer_id = "1";

                                if (transaction_shuffle_cards.Length != 0 && transaction_transfer_id.Length != 0)
                                {
                                    drawPlayersTurns();
                                    endGame();
                                }
                            break;

                        case (int)TableStatus.T_WAIT_SHUFFLE:
                        if (table.next_player_index == CLEOS.my_table_index)
                            {
                                transaction_wait_players = "1";
                                if (transaction_shuffle_cards.Length == 0)
                                {
                                    shuffle_cards();
                                    transaction_shuffle_cards = "1";
                                }

                                UITweener[] tweens = shuffle_menu.GetComponents<UITweener>();
                                foreach (UITweener tw in tweens)
                                {
                                    if (tw.tweenGroup == 0)
                                        tw.Play(true);
                                }

                                bay_in_button.SetActive(false);
                            }
                            break;

                        case (int)TableStatus.T_WAIT_CRYPT:
                        if (table.next_player_index == CLEOS.my_table_index)
                            {
                                if (transaction_crypted_cards.Length == 0)
                                {
                                    crypted_cards();
                                    transaction_crypted_cards = "1";
                                }
                            }
                            break;

                        case (int)TableStatus.T_WAIT_KEYS_FOR_PLAYERS:
                        if (table.players[CLEOS.my_table_index].status != (int)PlayerStatus.P_IN_GAME &&
                                table.players[CLEOS.my_table_index].status != (int)PlayerStatus.P_ALL_IN &&
                                table.players[CLEOS.my_table_index].status != (int)PlayerStatus.P_FOLD)
                                break;

                            if (transaction_setcardskeys.Length == 0)
                            {
                                List<Key> tmp1 = new List<Key>();
                                foreach (byte k in table.waiting_keys_indexes)
                                {
                                    if (k == table.players[CLEOS.my_table_index].cards_indexes[0])
                                        continue;

                                    if (k == table.players[CLEOS.my_table_index].cards_indexes[1])
                                        continue;

                                    if (k == 0 || k == 1 || k == 2 || k == 3)
                                    {
                                        personal_keys[k].trace_ch();
                                    }
                                    tmp1.Add(personal_keys[k]);
                                }

                                send_personal_keys_for_showdown(tmp1);
                                transaction_setcardskeys = "1";

                                UITweener[] tweens = shuffle_menu.GetComponents<UITweener>();
                                foreach (UITweener tw in tweens)
                                {
                                    if (tw.tweenGroup == 1)
                                        tw.Play(true);
                                }
                                bay_in_button.SetActive(true);
                            }
                            break;

                        case (int)TableStatus.T_WAIT_PLAYERS_ACT:
                            readTableCards();
                            drawPlayersTurns();

                            transaction_setcardskeys_for_showdown = "";

                            if (table.next_player_index == CLEOS.my_table_index)
                            {
                                if (availible_turn == 0)
                                    CheckTurnAbility(false);
                            }
                            else
                            {
                                if (availible_turn != 0)
                                {
                                    availible_turn = 0;
                                }
                            }
                            break;

                        case (int)TableStatus.T_WAIT_ALLIN_KEYS:
                        case (int)TableStatus.T_WAIT_ALL_KEYS:
                        case (int)TableStatus.T_WAIT_KEYS_FOR_SHOWDOWN:
                        if (table.players[CLEOS.my_table_index].status != (int)PlayerStatus.P_IN_GAME &&
                                table.players[CLEOS.my_table_index].status != (int)PlayerStatus.P_ALL_IN &&
                                table.players[CLEOS.my_table_index].status != (int)PlayerStatus.P_FOLD)
                                break;

                            List<Key> tmp = new List<Key>();
                            foreach (int i in table.waiting_keys_indexes)
                                tmp.Add(personal_keys[i]);

                            if (table.table_status == (int)TableStatus.T_WAIT_ALL_KEYS || table.table_status == (int)TableStatus.T_WAIT_ALLIN_KEYS)
                            {
                                foreach (int i in table.players[CLEOS.my_table_index].cards_indexes)
                                    tmp.Add(personal_keys[i]);
                            }

                            if (transaction_setcardskeys_for_showdown.Length == 0)
                            {
                                send_personal_keys_for_showdown(tmp);
                                transaction_setcardskeys_for_showdown = "1";
                            }

                            if (table.table_status == (int)TableStatus.T_WAIT_ALLIN_KEYS)
                            {
                                if (transaction_transfer_wait_allin_keys.Length == 0)
                                {
                                    send_personal_keys_for_showdown(tmp);
                                    transaction_transfer_wait_allin_keys = "1";
                                }
                            }

                            availible_turn = 0;
                            break;
                    }
                    last_status = (int)table.table_status;
                }

                catch (EosSharp.Exceptions.ApiErrorException e)
                {
                CLEOS.disconnect();
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
                CLEOS.disconnect();
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
                CLEOS.disconnect();
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
}
