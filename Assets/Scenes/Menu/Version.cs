using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class Version : MonoBehaviour {

    public GameObject label;

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

    public uint[] key0 = new uint[8];
    public uint[] synchro0 = new uint[2];
    public uint[] key1 = new uint[8];
    public uint[] synchro1 = new uint[2];

    public byte[] src_bytes = new byte[8];
    public byte[] result1 = new byte[8];
    public byte[] result2 = new byte[8];
    public byte[] result3 = new byte[8];

    public void CorrectCryption()
    {
        for (uint i = 0; i < 8; i++)
            key0[i] = ShufflePlugin.get_random_from_range_uint(uint.MinValue, uint.MaxValue);


        for (uint i = 0; i < 2; i++)
            synchro0[i] = ShufflePlugin.get_random_from_range_uint(uint.MinValue, uint.MaxValue);


        for (uint i = 0; i < 8; i++)
            key1[i] = ShufflePlugin.get_random_from_range_uint(uint.MinValue, uint.MaxValue);


        for (uint i = 0; i < 2; i++)
            synchro1[i] = ShufflePlugin.get_random_from_range_uint(uint.MinValue, uint.MaxValue);

        src_bytes[0] = 1;
        src_bytes[1] = 2;
        src_bytes[2] = 3;
        src_bytes[3] = 4;
        src_bytes[4] = 5;
        src_bytes[5] = 6;
        src_bytes[6] = 7;
        src_bytes[7] = 8;

       // Debug.Log("r0 = " + BitConverter.ToString(src_bytes.ToArray()));        
        CryptoPlugin.crypt_data(src_bytes, result1, src_bytes.Length, key0,  synchro0);
        //Debug.Log("r1 = " + BitConverter.ToString(src_bytes.ToArray()));

        //**********************************************************************************************************//

        CryptoPlugin.crypt_data(src_bytes, result3,  src_bytes.Length, key1,  synchro1);
       // Debug.Log("r2 = " + BitConverter.ToString(src_bytes.ToArray()));

        //**********************************************************************************************************//
        CryptoPlugin.decrypt_data(src_bytes, result3,  src_bytes.Length, key1,  synchro1);
       // Debug.Log("r3 = " + BitConverter.ToString(src_bytes.ToArray()));

        //**********************************************************************************************************//
        CryptoPlugin.decrypt_data(src_bytes, result2,  src_bytes.Length, key0,  synchro0);
        // Debug.Log("r4 = " + BitConverter.ToString(src_bytes.ToArray()));

        bool b = false;
        for( int i = 0; i < 8; i++ )
        {
            b = result1[i] == result2[i];

            if (b == false)
                break;
        }

        Debug.Log(b.ToString());

        //**********************************************************************************************************//
    }

    public Key k1 = new Key();
    public Key k2 = new Key();

    public Key[] personal_keys = new Key[52];
    public Card[] personal_card = new Card[52];

    public void init_key(Key in_k)
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

    public Card c = new Card(1, 1);
    void test_2()
    {
        for (uint i = 0; i < 32; i++)
            k1.data[i] = (byte)i;//( byte )ShufflePlugin.get_random_from_range_uint(0, 255);


        for (uint i = 0; i < 8; i++)
            k1.s[i] = (byte)i;//(byte)ShufflePlugin.get_random_from_range_uint(0, 255);

        k1.setup();


        for (uint i = 0; i < 32; i++)
            k2.data[i] = (byte)i;//(byte)ShufflePlugin.get_random_from_range_uint(0, 255);


        for (uint i = 0; i < 8; i++)
            k2.s[i] = (byte)i;//(byte)ShufflePlugin.get_random_from_range_uint(0, 255);

        k2.setup();

        c.Trace();

        crypt_card(c, k1, true);
        crypt_card(c, k2, true);
        crypt_card(c, k1, true);
        crypt_card(c, k2, true);

        c.Trace();

        crypt_card(c, k1, false);
        crypt_card(c, k2, false);
        crypt_card(c, k1, false);
        crypt_card(c, k2, false);

        c.Trace();
    }

    public byte[] src_buffer_fro_crypt_card = new byte[8];
    public byte[] dst_buffer_fro_crypt_card = new byte[8];
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



        if (crypt)
        {


            //Create new Variable to Store the result
           // byte[] returnedResult = new byte[2];

            //Copy from result pointer to the C# variable
            
            IntPtr returnedPtr = CryptoPlugin.crypt_data1(src_buffer_fro_crypt_card, key_data.data_i, key_data.s_i);
            Marshal.Copy(returnedPtr, dst_buffer_fro_crypt_card, 0, 8);
            CryptoPlugin.freeMem(returnedPtr);
            // CryptoPlugin.crypt_data(src_buffer_fro_crypt_card, key_data.m, 8, key_data.data_i, key_data.s_i);
        }
        else
        {
            IntPtr returnedPtr = CryptoPlugin.decrypt_data1(src_buffer_fro_crypt_card, key_data.data_i, key_data.s_i);
            Marshal.Copy(returnedPtr, dst_buffer_fro_crypt_card, 0, 8);
            CryptoPlugin.freeMem(returnedPtr);
            //dst_buffer_fro_crypt_card = CryptoPlugin.decrypt_data1(src_buffer_fro_crypt_card, key_data.data_i, key_data.s_i);
            // CryptoPlugin.decrypt_data(src_buffer_fro_crypt_card, key_data.m, 8, key_data.data_i, key_data.s_i);
        }

        c.suit = dst_buffer_fro_crypt_card[0];
        c.value = dst_buffer_fro_crypt_card[1];
    }

    void test3()
    {
        for (int i = 0; i < 52; i++)
        {
            personal_keys[i] = new Key();
            personal_keys[i].setup();

            personal_card[i] = new Card();
            personal_card[i].suit = (byte)i;
            personal_card[i].value = (byte)i;
        }

        Debug.Log(" original suit = " + personal_card[51].suit.ToString() + " original value = " + personal_card[51].value.ToString());

        for (int i = 0; i < 52; i++)
        {
            crypt_card(personal_card[i], personal_keys[i], true);
        }
/*
        for (int i = 0; i < 52; i++)
        {
            crypt_card(personal_card[i], personal_keys[i], false);
        }
        */
        //Debug.Log(" final suit = " + personal_card[51].suit.ToString() + " final value = " + personal_card[51].value.ToString());
    }

    void test4()
    {
       // Debug.Log(" start suit = " + personal_card[51].suit.ToString() + " start value = " + personal_card[51].value.ToString());


        
                for (int i = 0; i < 52; i++)
                {
                    crypt_card(personal_card[i], personal_keys[i], false);
                }

        Debug.Log(" final suit = " + personal_card[51].suit.ToString() + " final value = " + personal_card[51].value.ToString());
    }

    // Update is called once per frame

    void Update () {
		
	}

    void test_5()
    {
        init_fake_key(k1);

        c.suit = 0;
        c.value = 2;
        c.Trace();

        crypt_card(c, k1, true);
      //  crypt_card(c, k1, true);
        // crypt_card(c, k1, false);

        c.Trace();

    }

    // Use this for initialization
    void Start()
    {
        QualitySettings.vSyncCount = 2;
        // test3();
        // CorrectCryption();
        //test_5();
        // test4();

        label.GetComponent<UILabel>().text = CLEOS.version;
    }
}
