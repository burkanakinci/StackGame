using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int platformCount = 10, neededStackCount = 1, stackUpgradePrice = 0;
    [SerializeField] private List<Vector3> obstaclePositions = new List<Vector3>();
    [SerializeField] private List<Vector3> diamondPositions = new List<Vector3>();
    [SerializeField] private List<Vector3> goldPositions = new List<Vector3>();

    public void AddObstaclePosition(Vector3 _obstaclePositions)
    {
        obstaclePositions.Add(_obstaclePositions);
    }
    public void AddDiamondPosition(Vector3 _diamondPosition)
    {
        diamondPositions.Add(_diamondPosition);
    }
    public void AddGoldPosition(Vector3 _goldPosition)
    {
        goldPositions.Add(_goldPosition);
    }
    public void SetPlatformCount(int _platformCount)
    {
        platformCount = _platformCount;
    }
    public void SetNeededStackCount(int _neededStack)
    {
        neededStackCount = _neededStack;
    }
    public void SetStackUpgradePrice(int _stackUpgradePrice)
    {
        stackUpgradePrice = _stackUpgradePrice;
    }
    public int PlatformCount
    {
        get { return platformCount; }
    }
    public int NeededStackCount
    {
        get { return neededStackCount; }
    }
    public int StackUpgradePrice
    {
        get { return stackUpgradePrice; }
    }
    public int GetObstaclePositionsCount
    {
        get { return obstaclePositions.Count; }
    }
    public int GetDiamondPositionsCount
    {
        get { return diamondPositions.Count; }
    }
    public int GetGoldPositionsCount
    {
        get { return goldPositions.Count; }
    }
    public Vector3 ObstaclePosition(int _obstacleIndex)
    {
        return obstaclePositions[_obstacleIndex];
    }
    public Vector3 GoldPosition(int _goldIndex)
    {
        return goldPositions[_goldIndex];
    }
    public Vector3 DiamondPosition(int _diamondIndex)
    {
        return diamondPositions[_diamondIndex];
    }
}
