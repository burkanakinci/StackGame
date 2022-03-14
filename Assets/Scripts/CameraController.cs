using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offsetOnPlay, offsetOnFinish;
    [SerializeField] private Transform player;
    [SerializeField] float lerpOnPlay = 0.15f;

    private void Start()
    {
        GameManager.Instance.levelStart += CameraCutToPlayer;
    }
    private void CameraCutToPlayer()
    {
        transform.position = player.position + offsetOnPlay;

        transform.LookAt(player);
    }
    private void LateUpdate()
    {
        CameraMovement();
    }
    private void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position,
    player.position + offsetOnPlay,
    lerpOnPlay);

        transform.LookAt(player);
    }
}
