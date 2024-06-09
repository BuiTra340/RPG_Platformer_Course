using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance { get; private set; }
    public Player player;
    public int currency;
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;
    }
    public bool haveEnoughMoney(int _price)
    {
        if(currency < _price)
        {
            Debug.Log("not enought money");
            return false;
        }
        currency -= _price;
        return true;
    }
    public int currentCurrency() => currency;

    public void saveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }

    public void loadData(GameData _data)
    {
        this.currency = _data.currency;
    }
}
