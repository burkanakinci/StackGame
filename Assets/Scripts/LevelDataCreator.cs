using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;

public class LevelDataCreator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText, neededStackText, stackUpgradePrice;
    private LevelData tempLevelData;
    [SerializeField]
    private GameObject[] obstacles,
        golds,
        platforms,
        diamonds;
    private int tempNeededStack = 15, tempPlatformCount = 10, tempStackUpgradePrice = 0;
    private string savePath;

    public void SaveLevelDataButton()
    {
        tempLevelData = ScriptableObject.CreateInstance<LevelData>();

        savePath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/LevelScriptableObjects/Level" + int.Parse(levelNumberText.text.Trim(levelNumberText.text[levelNumberText.text.Length - 1])) + ".asset");

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        golds = GameObject.FindGameObjectsWithTag("Gold");
        diamonds = GameObject.FindGameObjectsWithTag("Diamond");

        for (int i = obstacles.Length - 1; i >= 0; i--)
        {
            tempLevelData.AddObstaclePosition(obstacles[i].transform.position);
        }
        for (int j = golds.Length - 1; j >= 0; j--)
        {
            tempLevelData.AddGoldPosition(golds[j].transform.position);
        }
        for (int k = diamonds.Length - 1; k >= 0; k--)
        {
            tempLevelData.AddDiamondPosition(diamonds[k].transform.position);
        }

        tempNeededStack = int.Parse(neededStackText.text.Trim(neededStackText.text[neededStackText.text.Length - 1]));
        tempPlatformCount = platforms.Length;
        tempStackUpgradePrice = int.Parse(stackUpgradePrice.text.Trim(stackUpgradePrice.text[stackUpgradePrice.text.Length - 1]));

        tempLevelData.SetNeededStackCount(tempNeededStack);
        tempLevelData.SetPlatformCount(tempPlatformCount);
        tempLevelData.SetStackUpgradePrice(tempStackUpgradePrice);

        AssetDatabase.CreateAsset(tempLevelData, savePath);
        AssetDatabase.SaveAssets();
    }
}
