using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            return instance;
        }
    }

    private List<DiamondMovement> diamondPool = new List<DiamondMovement>();
    [SerializeField] private DiamondMovement diamondPrefab;
    [SerializeField] private Transform diamondParent;
    private List<MoneyController> moneyPool = new List<MoneyController>();
    [SerializeField] private MoneyController moneyPrefab;
    [SerializeField] private Transform moneyParent;
    private List<ObstacleController> obstaclePool = new List<ObstacleController>();
    [SerializeField] private ObstacleController obstaclePrefab;
    [SerializeField] private Transform obstacleParent;
    private List<MoneyMovement> moneyOnUIPool = new List<MoneyMovement>();
    [SerializeField] private MoneyMovement moneyOnUIPrefab;
    [SerializeField] private RectTransform moneyOnUIParent;
    private List<GameObject> platformPool = new List<GameObject>();
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Transform platformParentActive;
    [SerializeField] private Transform platformParentInactive;
    [SerializeField] private BoxCollider finishDiamondCollectionArea;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnMoneyOnUI()
    {
        if (moneyOnUIPool.Count > 0)
        {
            moneyOnUIPool[0].gameObject.SetActive(true);
            moneyOnUIPool.Remove(moneyOnUIPool[0]);
        }
        else
        {
            Instantiate(moneyOnUIPrefab, moneyOnUIParent);
        }
    }
    public void AddPoolMoneyOnUI(MoneyMovement _moneyMovement)
    {
        moneyOnUIPool.Add(_moneyMovement);
        _moneyMovement.gameObject.SetActive(false);
    }

    public void SpawnPlatform()
    {
        if (platformPool.Count > 0)
        {
            platformPool[0].SetActive(true);
            platformPool[0].transform.position = (Vector3.forward * 10f) * platformParentActive.childCount;
            platformPool[0].transform.SetParent(platformParentActive);
            platformPool.Remove(platformPool[0]);
        }
        else
        {
            Instantiate(platformPrefab, ((Vector3.forward * 10f) * platformParentActive.childCount), Quaternion.identity, platformParentActive);
        }
    }
    public void CleanPlatformOnScene()
    {
        for (int i = platformParentActive.childCount - 1; i >= 0; i--)
        {
            platformPool.Add(platformParentActive.GetChild(i).gameObject);
            platformParentActive.GetChild(i).gameObject.SetActive(false);
            platformParentActive.GetChild(i).transform.SetParent(platformParentInactive);
        }
    }
    public void SpawnMoney(Vector3 _moneyPosition)
    {
        if (moneyPool.Count > 0)
        {
            moneyPool[0].gameObject.SetActive(true);
            moneyPool[0].transform.position = _moneyPosition;
            moneyPool.Remove(moneyPool[0]);
        }
        else
        {
            Instantiate(moneyPrefab, _moneyPosition, Quaternion.identity, moneyParent);
        }
    }
    public void CleanMoneyOnScene()
    {
        for (int i = moneyParent.childCount - 1; i >= 0; i--)
        {
            moneyParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void AddPoolMoney(MoneyController _moneyController)
    {
        moneyPool.Add(_moneyController);
    }

    public void SpawnObstacle(Vector3 _obstaclePosition)
    {
        if (obstaclePool.Count > 0)
        {
            obstaclePool[0].gameObject.SetActive(true);
            obstaclePool[0].transform.position = _obstaclePosition;
            obstaclePool.Remove(obstaclePool[0]);
        }
        else
        {
            Instantiate(obstaclePrefab, _obstaclePosition, Quaternion.identity, obstacleParent);
        }
    }
    public void CleanObstacleOnScene()
    {
        for (int i = obstacleParent.childCount - 1; i >= 0; i--)
        {

            obstacleParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void AddPoolObstacle(ObstacleController _obstacleController)
    {
        obstaclePool.Add(_obstacleController);
    }
    public void SpawnDiamond(Vector3 _diamondPosition)
    {
        if (diamondPool.Count > 0)
        {
            diamondPool[0].gameObject.SetActive(true);
            diamondPool[0].transform.position = _diamondPosition;
            diamondPool.Remove(diamondPool[0]);
        }
        else
        {
            Instantiate(diamondPrefab, _diamondPosition, Quaternion.identity, diamondParent);
        }
    }
    public void CleanDiamondOnScene()
    {
        for (int i = diamondParent.childCount - 1; i >= 0; i--)
        {
            diamondParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void AddPoolDiamond(DiamondMovement _diamondMovement)
    {
        diamondPool.Add(_diamondMovement);
    }
    public Transform GetDiamondParent()
    {
        return diamondParent;
    }
    public BoxCollider GetFinishCollectionArea()
    {
        return finishDiamondCollectionArea;
    }
    public void SetFinishCollectionAreaPosition(Vector3 _finishPos)
    {
        finishDiamondCollectionArea.transform.position = _finishPos;
    }
}
