using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    private PlayerActions playerActions;
    [SerializeField] CinemachineVirtualCamera aimCamera;

    private void Awake()
    {
        playerActions = GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (playerActions != null)
        {
            if (playerActions.isAiming)
                SwitchToAim();
            else
                SwitchToTPS();
        }
    }

    private void SwitchToTPS()
    {
        aimCamera.Priority = 9;
    }

    private void SwitchToAim()
    {
        aimCamera.Priority = 11;
    }
}
