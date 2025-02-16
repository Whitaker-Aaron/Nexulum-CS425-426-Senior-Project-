using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCraftRecipes : MonoBehaviour
{
    [SerializeField] List<CraftRecipe> recipesInStore = new List<CraftRecipe>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<CraftRecipe> getRecipes()
    {
        return recipesInStore;
    }

    public void removeRecipe(CraftRecipe recipeToRemove)
    {
        for(int i = 0; i < recipesInStore.Count; i++)
        {
            if (recipesInStore[i].recipeName == recipeToRemove.recipeName)
            {
                recipesInStore.RemoveAt(i);
                break;
            }
        }
    }
}
