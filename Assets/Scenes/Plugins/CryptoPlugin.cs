using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public static class CryptoPlugin
{
#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern void crypt_data(byte[] data, byte[] mac, int data_len, uint[] key,  uint[] synchro);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern void decrypt_data(byte[] data, byte[] mac, int data_len, uint[] key, uint[] synchro);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern IntPtr crypt_data1(byte[] data, uint[] key, uint[] synchro);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern IntPtr decrypt_data1(byte[] data, uint[] key, uint[] synchro);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern IntPtr get_mac1(byte[] data, uint[] key, uint[] synchro);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern void generate_random_range(byte[] data, int data_size);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern int freeMem(IntPtr ptr);
}
