using UnityEngine;

public class SpottedCylinder : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Player.Instance.TakeDamage(-damageAmount);
        }
    }
}
