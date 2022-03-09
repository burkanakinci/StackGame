using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform moveParent;
    [SerializeField] private float forwardSpeed = 12f;
    [SerializeField] private float horizontalSpeed = 8f;
    [SerializeField] private float playerXClampValue = 4.5f;
    private Vector3 firstMousePos;
    private float changeOfMousePos, horizontalMovementChange, screenWidth;
    private void Start()
    {

        playerXClampValue = Mathf.Abs(playerXClampValue);
        screenWidth = Screen.width;
    }
    private void Update()
    {
        CheckControls();

    }
    private void FixedUpdate()
    {
        MoveForward();
        MoveCharacter();
    }
    private void CheckControls()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            horizontalMovementChange = 0;

            changeOfMousePos = Input.mousePosition.x - firstMousePos.x;
            if (Mathf.Abs(changeOfMousePos) > 0.1f)
            {

                horizontalMovementChange = (changeOfMousePos * 1 / screenWidth);
                firstMousePos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            horizontalMovementChange = 0;
            firstMousePos = Input.mousePosition;
        }
    }

    void MoveForward()
    {
        moveParent.transform.position = new Vector3(moveParent.transform.position.x, moveParent.transform.position.y, Mathf.Lerp(moveParent.transform.position.z, moveParent.transform.position.z + 1, forwardSpeed * Time.fixedDeltaTime));
    }

    void MoveCharacter()
    {
        transform.localPosition = new Vector3(Mathf.Clamp(Mathf.Lerp(transform.localPosition.x, transform.localPosition.x + (horizontalMovementChange * playerXClampValue * 2f), horizontalSpeed * Time.fixedDeltaTime), (-1f * playerXClampValue), playerXClampValue), transform.localPosition.y, transform.localPosition.z);
    }
}
