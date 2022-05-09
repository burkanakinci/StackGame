using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private int decreaseDiamondCount = 4;
    private int tempDecreaseDiamondCount;
    [SerializeField] private float decreaseDuration = 0.1f;
    private BoxCollider obstacleCollider;
    private void Awake()
    {
        obstacleCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        tempDecreaseDiamondCount = decreaseDiamondCount;

        obstacleCollider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.GetGameState() == GameState.Play)
        {
            obstacleCollider.enabled = false;

            PlayerController.Instance.obstacleCollidedCounter++;

            if (PlayerController.Instance.obstacleCollidedCounter >= 3)
            {
                GameManager.Instance.SetGameState(GameState.Fail);

                PlayerController.Instance.PlayIdleAnimation();
                UIController.Instance.UIOnFail();

                PlayerController.Instance.CleanDiamondOnPlayer();
            }
            else
            {
                StartCoroutine(DecreasePlayerDuration());
            }
        }
    }
    private void OnDisable()
    {
        ObjectPool.Instance.AddPoolObstacle(this);
    }
    private IEnumerator DecreasePlayerDuration()
    {
        PlayerController.Instance.DecreaseDiamond();
        tempDecreaseDiamondCount--;
        yield return new WaitForSeconds(decreaseDuration);
        if (tempDecreaseDiamondCount > 0 && PlayerController.Instance.GetCollectedDiamondCount() > 0)
        {
            StartCoroutine(DecreasePlayerDuration());
        }

    }
}
