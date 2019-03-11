using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedPlayerScript : NetworkBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;
    public Camera fpsCamera;
    public AudioListener audioListener;

    public override void OnStartLocalPlayer()
    {
        fpsController.enabled = true;
        fpsCamera.enabled = true;
        audioListener.enabled = true;

        gameObject.name = "LOCAL Player";

        base.OnStartLocalPlayer();
    }
}
