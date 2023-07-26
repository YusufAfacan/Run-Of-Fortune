using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera followCam;
    public CinemachineVirtualCamera bossFightCam;
    private Vector3 followCamStartingPos;

    void Start()
    {
        Player.Instance.OnEngageBossFight += Player_OnEngageBossFight;
        LevelLoader.Instance.OnRestartScene += Instance_OnRestartScene;
        LevelLoader.Instance.OnLoadNextScene += Instance_OnLoadNextScene;

        followCamStartingPos = followCam.transform.position;
    }

    private void Instance_OnLoadNextScene(object sender, System.EventArgs e)
    {
        bossFightCam.gameObject.SetActive(false);
        followCam.gameObject.SetActive(true);
        
    }

    private void Instance_OnRestartScene(object sender, System.EventArgs e)
    {
        followCam.transform.position = followCamStartingPos;

        followCam.gameObject.SetActive(true);
        bossFightCam.gameObject.SetActive(false);
    }

    private void Player_OnEngageBossFight(object sender, System.EventArgs e)
    {
        followCam.gameObject.SetActive(false);
        bossFightCam.gameObject.SetActive(true);
    }

    
}
