using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : MonoBehaviour
{
    private DiamondMovement diamondMovement;
    private BoxCollider diamondCollider;
    private void Awake()
    {
        diamondMovement = GetComponent<DiamondMovement>();
        diamondCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        ActivateCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.GetGameState() == GameState.Play
            && !diamondMovement.GetIsJumping())
        {
            diamondCollider.enabled = false;

            PlayerController.Instance.CollectDiamond(ref diamondMovement);
        }
    }
    public void ActivateCollider()
    {
        diamondCollider.enabled = true;
    }

}
