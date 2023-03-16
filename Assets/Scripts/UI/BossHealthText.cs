using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHealthText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        Boss.Instance.OnHealthChanged += Boss_OnHealthChanged;
    }

    private void Boss_OnHealthChanged(object sender, Boss.OnHealthChangedEventArgs e)
    {
        if (Player.Instance.GetHealth() > 0)
        {
            StartCoroutine(UpdateHealthText(e.amount, 1.5f));
        }
    }

    public IEnumerator UpdateHealthText(int changeValue, float changeTime)
    {
        int healthInText = int.Parse(healthText.text);
        int health = Boss.Instance.GetHealth();

        if (healthInText > health)
        {
            healthInText--;
            healthText.text = healthInText.ToString();

            if (Mathf.Abs(healthInText - health) != 0)
            {
                yield return new WaitForSeconds(changeTime / changeValue);
                StartCoroutine(UpdateHealthText(changeValue, changeTime));
            }
            else StopCoroutine(UpdateHealthText(changeValue, changeTime));
        }

        else if (healthInText < health)
        {
            healthInText++;
            healthText.text = healthInText.ToString();

            if (Mathf.Abs(healthInText - health) != 0)
            {
                yield return new WaitForSeconds(changeTime / changeValue);
                StartCoroutine(UpdateHealthText(changeValue, changeTime));
            }
            else StopCoroutine(UpdateHealthText(changeValue, changeTime));
        }

    }

}
