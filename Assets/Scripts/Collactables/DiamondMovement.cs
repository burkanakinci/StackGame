using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMovement : MonoBehaviour
{
    [SerializeField] private CollactableScriptableObject diamondMovementData;
    private DiamondController diamondController;
    private bool jumping;
    private Vector3 jumpTarget;
    private Vector3 jumpStartPoint;
    private float flightTimer;
    [SerializeField] private float minObstacleCollidedZDistance = 10f, maxObstacleCollidedZDistance = 15f;
    private void Awake()
    {
        diamondController = GetComponent<DiamondController>();
    }
    private void Update()
    {
        if (jumping)
        {
            LocalJumpToTarget();
        }
    }

    private void LocalJumpToTarget()
    {
        flightTimer += Time.deltaTime;
        //diamondMovementScriptable.flightTimer = 
        //diamondMovementScriptable.flightTimer % diamondMovementScriptable.FlightSpeed;

        transform.localPosition = MathParabola.Parabola
                (jumpStartPoint,
                jumpTarget,
                diamondMovementData.HeightMultiplier,
                flightTimer / diamondMovementData.FlightDuration);

        //diamondMovementScriptable.direction = transform.position - diamondMovementScriptable.lastPosition;
        //transform.rotation = Quaternion.LookRotation(diamondMovementScriptable.direction);

        //diamondMovementData.lastPosition = transform.position;
        if (flightTimer >= diamondMovementData.FlightDuration)
        {
            transform.localPosition = jumpTarget;
            jumping = false;
            return;
        }

    }

    public void CalculateTargetOnPlay()
    {
        jumpTarget = (Vector3.up * 0.85f) * (PlayerController.Instance.GetCollectedDiamondCount());
    }
    public void CalculateTargetOnFinish()
    {
        jumpTarget = (Vector3.up * 0.58f) + (RandomPointInBounds(ObjectPool.Instance.GetFinishCollectionArea().bounds));
    }
    public void CalculateTargetCollideObstacle()
    {
        jumpTarget = (Vector3.up * 1.5f) + (new Vector3(Random.Range((PlayerController.Instance.GetXMovementClamp() * (-1f)), PlayerController.Instance.GetXMovementClamp()), 0f, Random.Range((PlayerController.Instance.transform.position.z + minObstacleCollidedZDistance), (PlayerController.Instance.transform.position.z + maxObstacleCollidedZDistance))));
    }

    public void ResetJumpParameters()
    {
        flightTimer = 0.0f;
        jumpStartPoint = transform.localPosition;
        jumping = true;

        //diamondMovementData.lastPosition = transform.localPosition;

        // diamondMovementData.targetDistanceY =
        //     ((transform.localPosition.y * diamondMovementData.target.y) <= 0 ?
        //         (Mathf.Abs(transform.localPosition.y) - Mathf.Abs(diamondMovementData.target.y)) :
        //         Mathf.Abs(transform.localPosition.y - diamondMovementData.target.y));

    }
    private Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
    private void OnDisable()
    {
        ObjectPool.Instance.AddPoolDiamond(this);
    }
    public void ActivateColliderOnMovement()
    {
        diamondController.ActivateCollider();
    }
    public bool GetIsJumping()
    {
        return jumping;
    }
}
