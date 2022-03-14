using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UIController").AddComponent<UIController>();
            }
            return instance;
        }
    }
    private float screenWidth, screenHeight;
    [SerializeField] private RectTransform currencyTarget;
    [SerializeField] private GameObject failBackground;
    [SerializeField] private GameObject successBackground;
    [SerializeField] private GameObject currencyAmount;
    [SerializeField] private GameObject tapToPlayScreen, inGameScreen, levelEndScreen;
    [SerializeField] private GameObject nextLevelButtonArea;
    [SerializeField] private GameObject collectedCurrencyAmount;
    [SerializeField] private GameObject stackUpgradeButton;
    [SerializeField] private TextMeshProUGUI stackUpgradePriceText;
    [SerializeField] private TextMeshProUGUI currencyAmountText;
    [SerializeField] private TextMeshProUGUI collectedCurrencyAmountText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelCompletedText;
    [SerializeField] private TextMeshProUGUI levelFailedText;
    [SerializeField] private float decreaseCurrencyAmountDuration = 0.6f;

    private void Awake()
    {
        GameManager.Instance.levelStart += UIOnTapToPlay;
        GameManager.Instance.levelStart += ShowLevel;
        GameManager.Instance.levelStart += ShowStackUpgradePrice;

        instance = this;
    }
    private void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    public void ShowStackUpgradePrice()
    {
        stackUpgradePriceText.text = GameManager.Instance.GetUpgradePrice().ToString();
    }

    public void ShowCurrencyAmount(ref int _currencyAmount)
    {
        currencyAmountText.text = _currencyAmount.ToString();
    }
    public void ShowTotalCurrencyAmount(ref int _totalCurrencyAmount)
    {
        collectedCurrencyAmountText.text = _totalCurrencyAmount.ToString();
    }
    public void ShowLevel()
    {
        levelText.text = "Level. " + GameManager.Instance.GetLevelNumber();
    }

    public float GetScreenWidth()
    {
        return screenWidth;
    }
    public float GetScreenHeight()
    {
        return screenHeight;
    }
    public Vector3 GetCurrencyTargetPosition()
    {
        return currencyTarget.anchoredPosition;
    }
    public void UIOnPlay()
    {
        failBackground.SetActive(false);
        successBackground.SetActive(false);
        currencyAmount.SetActive(true);
        tapToPlayScreen.SetActive(false);
        inGameScreen.SetActive(true);
        levelEndScreen.SetActive(false);
        collectedCurrencyAmount.SetActive(false);
        nextLevelButtonArea.SetActive(false);
        levelText.gameObject.SetActive(true);

    }
    private void UIOnTapToPlay()
    {
        failBackground.SetActive(false);
        successBackground.SetActive(false);
        currencyAmount.SetActive(false);
        tapToPlayScreen.SetActive(true);
        stackUpgradeButton.SetActive(GameManager.Instance.GetLevelNumber() > 1);
        inGameScreen.SetActive(false);
        levelEndScreen.SetActive(false);
        collectedCurrencyAmount.SetActive(true);
        nextLevelButtonArea.SetActive(false);
        levelText.gameObject.SetActive(true);
    }
    public void UIOnLevelEnd()
    {
        failBackground.SetActive(false);


        levelCompletedText.text = "Level " + GameManager.Instance.GetLevelNumber() + " Completed";

        currencyAmount.SetActive(true);
        tapToPlayScreen.SetActive(false);
        inGameScreen.SetActive(false);
        levelEndScreen.SetActive(true);
        collectedCurrencyAmount.SetActive(true);
        nextLevelButtonArea.SetActive(false);
        levelText.gameObject.SetActive(false);

        StartCoroutine(SetTotalCurrencyAmountOnUI());
    }
    public void UIOnFail()
    {
        failBackground.SetActive(true);

        levelCompletedText.text = "Level " + GameManager.Instance.GetLevelNumber() + " Failed";

        successBackground.SetActive(false);
        currencyAmount.SetActive(false);
        tapToPlayScreen.SetActive(false);
        inGameScreen.SetActive(false);
        levelEndScreen.SetActive(false);
        collectedCurrencyAmount.SetActive(false);
        nextLevelButtonArea.SetActive(false);
        levelText.gameObject.SetActive(false);
    }
    private IEnumerator SetTotalCurrencyAmountOnUI()
    {
        PlayerController.Instance.DecreaseCurrencyAmount();
        yield return new WaitForSeconds(decreaseCurrencyAmountDuration);
        PlayerController.Instance.IncreaseTotalCurrencyAmount();
        if (PlayerController.Instance.GetCurrencyAmount() > 0)
        {
            StartCoroutine(SetTotalCurrencyAmountOnUI());
        }
        else
        {
            PlayerController.Instance.PlaySuccessParticle();
            nextLevelButtonArea.SetActive(true);
            successBackground.SetActive(true);
        }
    }
    public void NextLevel(bool _isRestart)
    {
        if (!_isRestart)
        {

            PlayerController.Instance.SetTotalCurrencyAmountData();
            GameManager.Instance.SetLevelData();
        }
        else
        {
            for (int i = (PlayerController.Instance.GetCollectedDiamondCount() - 1); i >= 0; i--)
            {
                PlayerController.Instance.DecreaseDiamond();
            }

            for (int j = (PlayerController.Instance.GetCurrencyAmount() - 1); j >= 0; j--)
            {
                PlayerController.Instance.DecreaseCurrencyAmount();
            }

        }

        GameManager.Instance.NextLevelOnGameManager();
    }

    public void UpgradeStack()
    {
        if (GameManager.Instance.GetUpgradePrice() <=
                PlayerController.Instance.GetTotalCurrencyAmount())
        {
            PlayerController.Instance.SetTotalCurrency(PlayerController.Instance.GetTotalCurrencyAmount() -
                GameManager.Instance.GetUpgradePrice());

            PlayerController.Instance.SetTotalCurrencyAmountData();

            PlayerController.Instance.SetUpgradeIncrease(GameManager.Instance.GetUpgradePrice());

            PlayerController.Instance.SetFilledStack();
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.levelStart -= UIOnTapToPlay;
        GameManager.Instance.levelStart -= ShowLevel;
    }
}
