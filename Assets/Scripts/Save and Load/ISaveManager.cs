using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager 
{
    public void saveData(ref GameData _data);
    public void loadData(GameData _data);
}
