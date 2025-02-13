using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCraftList : MonoBehaviour
{
    [SerializeField] public List<CraftRecipe> allRecipes = new List<CraftRecipe>();
    public List<CraftRecipe> accessibleRecipes = new List<CraftRecipe>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public List<CraftRecipe> getAllRecipes()
    {
        return allRecipes;
    }

    public List<CraftRecipe> getAccessibleRecipes()
    {
        return accessibleRecipes;
    }

    public void addToAccessibleRecipes(CraftRecipe recipeToAdd)
    {
        accessibleRecipes.Add(recipeToAdd);
    }

}
