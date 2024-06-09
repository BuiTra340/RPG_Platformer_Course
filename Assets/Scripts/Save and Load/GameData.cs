using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string,bool> checkPoint;
    public string closestCheckPointId;
    public int lostCurrencyAmount;
    public float lostCurrencyX;
    public float lostCurrencyY;
    public SerializableDictionary<string, float> sliderVolume;
    public GameData()
    {
        this.currency = 0;
        this.inventory = new SerializableDictionary<string,int>();
        equipmentId = new List<string>();
        skillTree = new SerializableDictionary<string, bool>();
        checkPoint = new SerializableDictionary<string,bool>();
        closestCheckPointId = string.Empty;
        lostCurrencyAmount = 0;
        lostCurrencyX = 0;
        lostCurrencyY = 0;
        sliderVolume = new SerializableDictionary<string, float>();
    }
}
