using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 startingPos;
    private Quaternion startingRot;
    void Start()
    {
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;
        Player.Instance.OnEngageBossFight += Player_OnEngageBossFight;
        startingRot = transform.rotation;
        startingPos = transform.position;
        offset = transform.position - Player.Instance.transform.position;
    }

    private void LevelLoader_OnRestartScene(object sender, System.EventArgs e)
    {
        transform.SetPositionAndRotation(startingPos, startingRot);
    }

    private void Player_OnEngageBossFight(object sender, System.EventArgs e)
    {
        StartCoroutine(AlignForBossFight());
    }
    private void FixedUpdate()
    {
        if (!Player.Instance.IsEngagedBossFight())
        {
            Vector3 desiredPosition = Player.Instance.transform.position + offset;
            Vector3 smoothFollow = Vector3.Lerp(transform.position, desiredPosition, 0.125f);
            transform.position = smoothFollow;
        }
    }
    private IEnumerator AlignForBossFight()
    {
        float elapsedTime = 0;
        float desiredTime = 1;
        float desiredXpos = -4f;
        float desiredYpos = 3.5F;
        float desiredZpos = 94.7f;
        float desiredYrot = 90f;
        Vector3 desiredPos = new(desiredXpos, desiredYpos, desiredZpos);
        Vector3 PivotPoint = new(0, 0, desiredZpos);

        while (elapsedTime < desiredTime)
        {
            transform.RotateAround(PivotPoint, Vector3.up, desiredYrot / desiredTime * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPos, elapsedTime / desiredTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
