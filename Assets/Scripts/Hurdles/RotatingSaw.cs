using UnityEngine;

public class RotatingSaw : MonoBehaviour
{
    private float timer;
    private float damageInterval = 0.1f;
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() && timer >= damageInterval)
        {
            Player.Instance.TakeDamage(1);
            if (Player.Instance.GetHealth() <= 0)
            {
                Player.Instance.Die();
            }
            SoundManager.Instance.PlayRotatingSawAudioClip();
            timer = 0;
        }
    }
}
