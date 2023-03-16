using UnityEngine;

public class AttackGate : Gate
{

    // Start is called before the first frame update
    void Start()
    {
        neighbourGate = transform.parent.GetComponentInChildren<HealthGate>();
        value = Random.Range(minValue, maxValue);
        text.text = value.ToString();


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() == true && isGranted == false)
        {
            isGranted = true;
            neighbourGate.isGranted = true;
            Player.Instance.IncreaseAttack(value);
            SoundManager.Instance.PlayAttackGateAudioClip();

        }
    }
}
