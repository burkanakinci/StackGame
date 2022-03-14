using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.GetGameState() == GameState.Play)
        {
            GameManager.Instance.SetGameState(GameState.Finish);
            if (((PlayerController.Instance.GetCollectedDiamondCount()) + (PlayerController.Instance.GetUpgradeIncrease()))
                >= PlayerController.Instance.GetNeededStack())
            {
                PlayerController.Instance.CleanDiamondOnPlayer();
                PlayerController.Instance.PlayDanceAnimation();
                UIController.Instance.UIOnLevelEnd();
            }
            else
            {
                PlayerController.Instance.PlayIdleAnimation();

                UIController.Instance.UIOnFail();
            }
        }
    }
}
