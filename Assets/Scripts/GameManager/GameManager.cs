using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    public CheckPoint[] checkPoints;

    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private Transform player;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);

        checkPoints = FindObjectsOfType<CheckPoint>();
    }
    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }
    public void loadCurrentScene()
    {
        SaveManager.instance.saveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void saveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if(findClosestCheckPoint() != null)
            _data.closestCheckPointId = findClosestCheckPoint().checkPointId;
        _data.checkPoint.Clear();
        foreach (CheckPoint obj in checkPoints)
        {
            _data.checkPoint.Add(obj.checkPointId, obj.isActive);
        }
    }

    public void loadData(GameData _data)
    {
        //foreach (var pair in _data.checkPoint)
        //{
        //    foreach (CheckPoint obj in checkPoints)
        //    {
        //        if (obj.checkPointId == pair.Key && pair.Value == true)
        //        {
        //            obj.activateCheckPoint();
        //        }
        //    }
        //}

        foreach (CheckPoint obj in checkPoints)
        {
            if (obj.checkPointId == _data.closestCheckPointId)
                PlayerManager.instance.player.transform.position = obj.transform.position;
        }
        loadLostCurrencyAmount(_data);
    }

    private void loadLostCurrencyAmount(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;
        if (lostCurrencyAmount > 0)
        {
            GameObject newCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }
        lostCurrencyAmount = 0;
    }

    private CheckPoint findClosestCheckPoint()
    {
        CheckPoint checkPointClosest = null;
        float distanceToCheckPoint = Mathf.Infinity;
        Vector2 playerPos = player.position;
        foreach (CheckPoint obj in checkPoints)
        {
            float distanceClosest = Vector2.Distance(playerPos, obj.transform.position);
            if (distanceClosest < distanceToCheckPoint && obj.isActive == true)
            {
                checkPointClosest = obj;
                distanceToCheckPoint = distanceClosest;
            }
        }
        return checkPointClosest;
    }
    public void pauseGame(bool _value)
    {
        if(_value)
            Time.timeScale = 0;
        else Time.timeScale = 1;
    }
    public void exitAndSave()
    {
        SaveManager.instance.saveGame();
        Application.Quit();
    }
}
