using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SaveSystemInterface
{
    public void SaveData(ref SaveData data);
    public void LoadData(SaveData data);
}
