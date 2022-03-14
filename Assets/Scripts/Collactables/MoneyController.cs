using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    private BoxCollider moneyCollider;
    private void Awake()
    {
        moneyCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        moneyCollider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.GetGameState() == GameState.Play)
        {
            moneyCollider.enabled = false;

            PlayerController.Instance.PlayMoneyParticle();

            ObjectPool.Instance.SpawnMoneyOnUI();
        }
    }

    private void OnDisable()
    {
        ObjectPool.Instance.AddPoolMoney(this);
    }
}
