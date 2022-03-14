using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMovement : MonoBehaviour
{
    private RectTransform currencyTransform;
    private Vector2 targetPos, startPos;
    [SerializeField] private float moveTimer;
    [SerializeField] private float moveDuration = 1f;

    private void Start()
    {
        currencyTransform = GetComponent<RectTransform>();

        targetPos = UIController.Instance.GetCurrencyTargetPosition();

        //startPos = new Vector3(PlayerController.Instance.GetPositionOnUI(), currencyTransform.anchoredPosition.y);
        startPos = currencyTransform.anchoredPosition;
    }
    private void OnEnable()
    {

        moveTimer = 0.0f;
    }
    private void Update()
    {
        moveTimer += Time.deltaTime;

        currencyTransform.anchoredPosition = ((targetPos - startPos) * (moveTimer / moveDuration)) + (startPos);

        if (moveDuration < moveTimer)
        {
            PlayerController.Instance.IncreaseCurrencyAmount();
            ObjectPool.Instance.AddPoolMoneyOnUI(this);
        }
    }
}
