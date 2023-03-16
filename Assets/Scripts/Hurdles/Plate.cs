using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private int damageAmount = 5;
    private Animator animator;
    private bool dealtDamage;
    private const string DROP = "Drop";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() && !dealtDamage)
        {
            Player.Instance.TakeDamage(damageAmount);
            dealtDamage = true;
            animator.Play(DROP);

        }
    }
}
