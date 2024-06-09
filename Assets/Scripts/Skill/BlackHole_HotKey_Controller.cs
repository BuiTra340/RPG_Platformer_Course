using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    SpriteRenderer sprite;
    private KeyCode myHotKey;
    private Transform enemyTransform;
    private BlackHole_Skill_Controller BlackHole;
    private TextMeshProUGUI myText;
    
    void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            BlackHole.addEnemyToList(enemyTransform);
            sprite.color = Color.black;
            myText.color = Color.clear;
        }
    }
    public void setUpHotKey(KeyCode _myHotKey,Transform _enemyTransform,BlackHole_Skill_Controller _blackHole)
    {
        sprite = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        BlackHole = _blackHole;
        enemyTransform = _enemyTransform;
        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
    }
}
