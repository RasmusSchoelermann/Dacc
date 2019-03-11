using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject reticuleCanvas;

    public override void OnStartClient(NetworkClient client)
    {
        HideSceneCamera();
        ShowReticuleCanvas();
    }

    public override void OnStartHost()
    {
        HideSceneCamera();
        ShowReticuleCanvas();
    }

    public override void OnStopClient()
    {
        ShowSceneCamera();
        HideReticuleCanvas();
    }

    public override void OnStopHost()
    {
        ShowSceneCamera();
        HideReticuleCanvas();
    }

    private void HideSceneCamera()
    {
        if (sceneCamera)
            sceneCamera.SetActive(false);
    }

    private void ShowSceneCamera()
    {
        if (sceneCamera)
            sceneCamera.SetActive(true);
    }

    private void HideReticuleCanvas()
    {
        if (reticuleCanvas)
            reticuleCanvas.SetActive(false);
    }

    private void ShowReticuleCanvas()
    {
        if (reticuleCanvas)
            reticuleCanvas.SetActive(true);
    }
}