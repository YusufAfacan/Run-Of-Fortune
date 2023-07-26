using System;
using System.Collections;
using UnityEngine;
public class HealthGate : Gate
{

    void Start()
    {
        neighbourGate = transform.parent.GetComponentInChildren<AttackGate>();
        value = UnityEngine.Random.Range(minValue, maxValue);
        text.text = value.ToString();
        LevelLoader.Instance.OnRestartScene += Instance_OnRestartScene;
    }

    private void Instance_OnRestartScene(object sender, EventArgs e)
    {
        isGranted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() == true && isGranted == false)
        {
            isGranted = true;
            neighbourGate.isGranted = true;
            Player.Instance.RestoreHealth(value);
            SoundManager.Instance.PlayHealthGateAudioClip();
            StartCoroutine(EnlargePlayerTransform());
        }
    }
    IEnumerator EnlargePlayerTransform()
    {
        Vector3 originalPlayerTransfromScale = Player.Instance.transform.localScale;
        Vector3 destinationPlayerTransfromScale = Player.Instance.transform.localScale + Vector3.one * 0.2f;
        float currentTime = 0.0f;
        float desiredTime = 0.5f;
        do
        {
            Player.Instance.transform.localScale = Vector3.Lerp(originalPlayerTransfromScale, destinationPlayerTransfromScale, currentTime / desiredTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= desiredTime);
    }
}
