using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("PlayerController").AddComponent<PlayerController>();
            }
            return instance;
        }
    }

    [SerializeField] private Animator playerAnimator;
    private List<DiamondMovement> collectedDiamond = new List<DiamondMovement>();
    private DiamondMovement tempDecreaseDiamond;
    [SerializeField] private Transform diamondParentFirst, diamondParent;
    [SerializeField] private Transform spineTransform;

    [SerializeField] private PlayerMovement playerMovement;
    private float screenWidth, maxXMovement;
    private int neededStack = 1, currencyAmount = 0, totalCurrencyAmount = 0, upgradeIncrease = 0;
    [SerializeField] private Image stackFilledImage;
    private float stackFilledAmount;
    private bool diamondParentFirstIsMove;
    private Vector3 diamondParentStartPos;
    [SerializeField] private ParticleSystem moneyParticle, diamondParticle;
    private void Awake()
    {
        diamondParentStartPos = diamondParentFirst.localPosition;

        GameManager.Instance.levelStart += ResetPlayerValues;

        instance = this;
    }
    public void PlayMoneyParticle()
    {
        moneyParticle.Play();
    }
    public void PlayDiamondParticle()
    {
        diamondParticle.Play();
    }
    public void SetTotalCurrencyAmountData()
    {
        SaveSystem.SaveCurrencyAmount(totalCurrencyAmount);
    }

    public void SetFilledStack()
    {
        stackFilledAmount = (float)(upgradeIncrease + collectedDiamond.Count) / (float)neededStack;
        stackFilledImage.fillAmount = stackFilledAmount;
    }

    public void ResetAnimatorParameters()
    {
        playerAnimator.SetBool("IsIdle", false);
        playerAnimator.SetBool("IsRun", false);
        playerAnimator.SetInteger("Run", 0);
        playerAnimator.SetBool("IsDance", false);

        playerAnimator.Play("Idle", 0, 0.0f);
    }
    public void PlayIdleAnimation()
    {

        ResetAnimatorParameters();

        playerAnimator.Play("Idle", 0, 0.0f);
    }
    public void PlayRun1Animation()
    {
        playerAnimator.SetBool("IsRun", true);
    }
    public void PlayRun2Animation()
    {
        if (playerAnimator.GetInteger("Run") != 1)
        {
            playerAnimator.SetInteger("Run", 1);
        }
        if (!diamondParentFirstIsMove)
        {
            diamondParentFirstIsMove = true;

            StartCoroutine(DiamondFirstMove());
        }
    }
    public void PlayDanceAnimation()
    {
        playerAnimator.SetBool("IsDance", true);
    }

    private IEnumerator DiamondFirstMove()
    {
        yield return new WaitForSeconds(1f);
        diamondParentFirst.SetParent(spineTransform);
    }
    public void CollectDiamond(ref DiamondMovement _diamondMovement)
    {
        PlayRun2Animation();

        collectedDiamond.Add(_diamondMovement);

        if (collectedDiamond.Count == 1)
        {
            _diamondMovement.transform.SetParent(diamondParentFirst);
        }
        else
        {
            _diamondMovement.transform.SetParent(diamondParent);
        }


        _diamondMovement.CalculateTargetOnPlay();
        _diamondMovement.ResetJumpParameters();

        SetFilledStack();
    }

    public void IncreaseCurrencyAmount()
    {
        currencyAmount++;

        UIController.Instance.ShowCurrencyAmount(ref currencyAmount);
    }
    public void IncreaseTotalCurrencyAmount()
    {
        totalCurrencyAmount++;

        UIController.Instance.ShowTotalCurrencyAmount(ref totalCurrencyAmount);
    }
    public void DecreaseTotalCurrencyAmount()
    {
        totalCurrencyAmount--;

        UIController.Instance.ShowTotalCurrencyAmount(ref totalCurrencyAmount);
    }
    public void DecreaseDiamond()
    {
        if (collectedDiamond.Count > 0)
        {
            tempDecreaseDiamond = collectedDiamond[collectedDiamond.Count - 1];
            collectedDiamond.Remove(tempDecreaseDiamond);

            tempDecreaseDiamond.ActivateColliderOnMovement();
            tempDecreaseDiamond.transform.SetParent(ObjectPool.Instance.GetDiamondParent());
            tempDecreaseDiamond.CalculateTargetCollideObstacle();
            tempDecreaseDiamond.ResetJumpParameters();

            SetFilledStack();
        }
    }
    public void DecreaseCurrencyAmount()
    {
        currencyAmount--;

        UIController.Instance.ShowCurrencyAmount(ref currencyAmount);
    }

    public void ResetPlayerValues()
    {
        upgradeIncrease = 0;

        SetFilledStack();

        transform.position = Vector3.zero;
        playerMovement.transform.position = Vector3.zero;


        diamondParentFirstIsMove = false;
        diamondParentFirst.SetParent(playerMovement.transform);
        diamondParentFirst.localPosition = diamondParentStartPos;
        diamondParentFirst.eulerAngles = Vector3.zero;

        currencyAmount = 0;
        UIController.Instance.ShowCurrencyAmount(ref currencyAmount);
        totalCurrencyAmount = SaveSystem.LoadCurrencyAmount();
        UIController.Instance.ShowTotalCurrencyAmount(ref totalCurrencyAmount);

        ResetAnimatorParameters();
    }

    public float GetPositionOnUI()
    {
        return (playerMovement.transform.localPosition.x) *
            (UIController.Instance.GetScreenWidth() / playerMovement.MaxXMovement());
    }
    public float GetXMovementClamp()
    {
        return playerMovement.GetXClampValue();
    }
    public int GetNeededStack()
    {
        return neededStack;
    }
    public int GetTotalCurrencyAmount()
    {
        return totalCurrencyAmount;
    }
    public void SetTotalCurrency(int _totalCurrency)
    {
        totalCurrencyAmount = _totalCurrency;

        UIController.Instance.ShowTotalCurrencyAmount(ref totalCurrencyAmount);
    }
    public int GetUpgradeIncrease()
    {
        return upgradeIncrease;
    }
    public void SetUpgradeIncrease(int _upgradeIncrease)
    {
        upgradeIncrease = _upgradeIncrease;
    }
    public void SetNeededStack(int _neededStack)
    {
        neededStack = _neededStack;
    }
    public int GetCurrencyAmount()
    {
        return currencyAmount;
    }
    public int GetCollectedDiamondCount()
    {
        return collectedDiamond.Count;
    }
    public DiamondMovement GetDiamondOnCollectedList(int _index)
    {
        return collectedDiamond[_index];
    }

    public void CleanDiamondOnPlayer()
    {
        for (int i = collectedDiamond.Count - 1; i >= 0; i--)
        {
            collectedDiamond[i].transform.SetParent(ObjectPool.Instance.GetDiamondParent());
            collectedDiamond[i].CalculateTargetOnFinish();
            collectedDiamond[i].ResetJumpParameters();
            collectedDiamond.Remove(collectedDiamond[i]);
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.levelStart -= ResetPlayerValues;
    }
}
