using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform moveParent;
    [SerializeField] private float forwardSpeed = 12f;
    [SerializeField] private float horizontalSpeed = 8f;
    [SerializeField] private float playerXClampValue = 4.5f;
    [SerializeField] private Transform playerCanvasTransform;
    [SerializeField] private float playerCanvasMoveValue = 8f;
    [SerializeField] private float diamondMoveXValue = 8f;
    private Vector3 firstMousePos;
    private float changeOfMousePos, horizontalMovementChange;
    private void Start()
    {
        playerXClampValue = Mathf.Abs(playerXClampValue);
    }
    private void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.Play)
        {
            CheckControls();
        }
        else if (GameManager.Instance.GetGameState() == GameState.Start)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                GameManager.Instance.SetGameState(GameState.Play);
                UIController.Instance.UIOnPlay();
                CheckControls();
                PlayerController.Instance.PlayRun1Animation();
            }
        }

    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() == GameState.Play)
        {
            MoveForward();
            MoveCharacter();
            MoveStackAmount();
            MoveDiamond();
        }
        else if (GameManager.Instance.GetGameState() == GameState.Finish)
        {
            MoveCharacterOnFinish();
        }
    }
    private void CheckControls()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            firstMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            horizontalMovementChange = 0;

            changeOfMousePos = Input.mousePosition.x - firstMousePos.x;
            if (Mathf.Abs(changeOfMousePos) > 0.1f)
            {

                horizontalMovementChange = (changeOfMousePos * 1 / UIController.Instance.GetScreenWidth());
                firstMousePos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            horizontalMovementChange = 0;
            firstMousePos = Input.mousePosition;
        }
    }

    private void MoveForward()
    {
        moveParent.transform.position = new Vector3(moveParent.transform.position.x, moveParent.transform.position.y, Mathf.Lerp(moveParent.transform.position.z, moveParent.transform.position.z + 1, forwardSpeed * Time.fixedDeltaTime));
    }

    private void MoveCharacter()
    {
        transform.localPosition = new Vector3(
            Mathf.Clamp(
                Mathf.Lerp(transform.localPosition.x, transform.localPosition.x + (horizontalMovementChange * playerXClampValue * 2f), horizontalSpeed * Time.fixedDeltaTime),
                            (-1f * playerXClampValue),
                            playerXClampValue),
                transform.localPosition.y,
                transform.localPosition.z);
    }
    private void MoveCharacterOnFinish()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,
            PlayerController.Instance.transform.InverseTransformPoint(
            new Vector3(ObjectPool.Instance.GetFinishCollectionArea().transform.position.x,
                        ObjectPool.Instance.GetFinishCollectionArea().transform.position.y,
                        ObjectPool.Instance.GetFinishCollectionArea().transform.position.z + 5f)),
            0.28f);
    }
    private void MoveStackAmount()
    {
        playerCanvasTransform.localPosition = new Vector3(
    Mathf.Clamp(
        Mathf.Lerp(playerCanvasTransform.localPosition.x, transform.localPosition.x, playerCanvasMoveValue * Time.fixedDeltaTime),
                            (-1f * playerXClampValue),
                            playerXClampValue),
        playerCanvasTransform.localPosition.y,
        playerCanvasTransform.localPosition.z);
    }
    private void MoveDiamond()
    {
        for (int i = 1; i < PlayerController.Instance.GetCollectedDiamondCount(); i++)
        {
            if (!PlayerController.Instance.GetDiamondOnCollectedList(i).GetIsJumping())
            {
                PlayerController.Instance.GetDiamondOnCollectedList(i).transform.position =
                new Vector3(
            Mathf.Lerp(PlayerController.Instance.GetDiamondOnCollectedList(i).transform.position.x, PlayerController.Instance.GetDiamondOnCollectedList(i - 1).transform.position.x, diamondMoveXValue * Time.fixedDeltaTime),
            PlayerController.Instance.GetDiamondOnCollectedList(i).transform.position.y,
            PlayerController.Instance.GetDiamondOnCollectedList(i).transform.position.z);
            }
        }
    }
    public float MaxXMovement()
    {
        return (playerXClampValue * 2f);
    }
    public float GetXClampValue()
    {
        return playerXClampValue;
    }
}
