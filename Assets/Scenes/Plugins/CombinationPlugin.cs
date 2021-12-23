using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class CombinationPlugin
{

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern int get_combination( int[] cards, int cards_count );

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("Combination")]
#endif
    public static extern int test();
  
    public static int current_combination = 0;
}