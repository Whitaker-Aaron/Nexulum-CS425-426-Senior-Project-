using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesCraftList : MonoBehaviour
{
    [SerializeField] public List<CraftRecipe> allRecipes = new List<CraftRecipe>();
    List<CraftRecipe> accessibleRecipes = new List<CraftRecipe>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}