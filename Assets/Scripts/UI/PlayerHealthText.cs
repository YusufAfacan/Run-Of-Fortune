using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerHealthText : MonoBehaviour
{
    private TextMeshProUGUI healthText;


    private void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        healthText.text = (100 + UpgradeManager.Instance.GetHealthLevel() * 5).ToString();
        Player.Instance.OnHealthChanged += Player_OnHealthChanged;
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;
    }

    private void LevelLoader_OnRestartScene(object sender, System.EventArgs e)
    {
        healthText.text = (100 + UpgradeManager.Instance.GetHealthLevel() * 5).ToString();
    }

    private void Player_OnHealthChanged(object sender, Player.OnHealthChangedEventArgs e)
    {
        if (Player.Instance.GetHealth() > 0)
        {
            StartCoroutine(UpdateHealthText(e.amount, 1.5f));
        }
    }

    public IEnumerator UpdateHealthText(int changeValue, float changeTime)
    {
        int healthInText = int.Parse(healthText.text);

        int health = Player.Instance.GetHealth();

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
