using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaterialList : MonoBehaviour
{

    [SerializedDictionary("MaterialName", "Texture")]
    public SerializedDictionary<string, Texture> materialTextureLookup;

    public Texture ReturnTexture(string materialName)
    {
        return materialTextureLookup[materialName];
    }
}
