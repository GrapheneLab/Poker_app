using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class ShufflePlugin
{
#if UNITY_IOS
    [DllImport("__Internal")]
#else           
    [DllImport("Combination")]
#endif
    public static extern int shuffle(int[] cards, int cards_count);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern int get_random_from_range(int from, int to);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern uint get_random_from_range_uint(uint from, uint to);
}
