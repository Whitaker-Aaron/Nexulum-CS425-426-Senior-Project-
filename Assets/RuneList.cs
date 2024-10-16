using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneList : MonoBehaviour
{
    [SerializedDictionary("RuneName", "Rune")]
    public SerializedDictionary<string, Rune> runeLookup;

    public Rune ReturnRune(string runeName)
    {
        return runeLookup[runeName];
    }
}
