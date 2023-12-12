using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    private PlayerActions playerActions;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    [SerializeField] CinemachineVirtualCamera TPSCam;

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
        aimCamera.gameObject.SetActive(false);
        aimCamera.Priority = 9;
        TPSCam.gameObject.SetActive(true);
        
    }

    private void SwitchToAim()
    {
        aimCamera.gameObject.SetActive(true);
        aimCamera.Priority = 11;
        TPSCam.gameObject.SetActive(false);
    }
}
