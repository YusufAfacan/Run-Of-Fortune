using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    public event EventHandler OnUpgrade;

    private const string HIDE = "Hide";
    private const string SHOW = "Show";
    private const string LEVEL = "LEVEL ";
    private const string DIAMONDAMOUNT = "DiamondAmount";
    private const string HEALTHLEVEL = "HealthLevel";
    private const string SLASHLEVEL = "SlashLevel";
    private const string CRITLEVEL = "CritLevel";
    private const string BOSSLEVEL = "BossLevel";

    [SerializeField] private int diamondAmount;
    [SerializeField] private TextMeshProUGUI diamondAmountText;

    [SerializeField] private int healthLevel;
    [SerializeField] private int healthIncreaseCost;
    [SerializeField] private Button healthIncreaseButton;
    [SerializeField] private TextMeshProUGUI healthLevelText;
    [SerializeField] private TextMeshProUGUI healthIncreaseCostText;

    [SerializeField] private int slashLevel;
    [SerializeField] private int slashIncreaseCost;
    [SerializeField] private Button slashIncreaseButton;
    [SerializeField] private TextMeshProUGUI slashLevelText;
    [SerializeField] private TextMeshProUGUI slashIncreaseCostText;

    [SerializeField] private int critLevel;
    [SerializeField] private int critIncreaseCost;
    [SerializeField] private Button critIncreaseButton;
    [SerializeField] private TextMeshProUGUI critLevelText;
    [SerializeField] private TextMeshProUGUI critIncreaseCostText;

    [SerializeField] private GameObject[] UIToHide;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PlayerMovement.Instance.OnLevelStarted += Player_OnLevelStarted;
        Player.Instance.OnVictory += Player_OnVictory;
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;

        diamondAmount = PlayerPrefs.GetInt(DIAMONDAMOUNT);
        diamondAmountText.text = diamondAmount.ToString();

        healthLevel = PlayerPrefs.GetInt(HEALTHLEVEL);
        healthIncreaseCost = (healthLevel + 1) * 50;
        healthLevelText.text = LEVEL + healthLevel;
        healthIncreaseCostText.text = healthIncreaseCost.ToString();

        slashLevel = PlayerPrefs.GetInt(SLASHLEVEL);
        slashIncreaseCost = (slashLevel + 1) * 50;
        slashLevelText.text = LEVEL + slashLevel;
        slashIncreaseCostText.text = slashIncreaseCost.ToString();

        critLevel = PlayerPrefs.GetInt(CRITLEVEL);
        critIncreaseCost = (critLevel + 1) * 500;
        critLevelText.text = LEVEL + critLevel;
        critIncreaseCostText.text = critIncreaseCost.ToString();

        healthIncreaseButton.onClick.AddListener(UpgradeHealth);
        slashIncreaseButton.onClick.AddListener(UpgradeSlash);
        critIncreaseButton.onClick.AddListener(UpgradeCrit);

    }

    private void LevelLoader_OnRestartScene(object sender, EventArgs e)
    {
        foreach (GameObject gameObject in UIToHide)
        {
            gameObject.GetComponent<Animator>().Play(SHOW);
        }
    }

    private void Player_OnVictory(object sender, EventArgs e)
    {
        PlayerPrefs.SetInt(DIAMONDAMOUNT, PlayerPrefs.GetInt(DIAMONDAMOUNT) + UnityEngine.Random.Range(50, 100));
        PlayerPrefs.SetInt(BOSSLEVEL, PlayerPrefs.GetInt(BOSSLEVEL) + 1);
    }

    private void Player_OnLevelStarted(object sender, System.EventArgs e)
    {
        foreach (GameObject gameObject in UIToHide)
        {
            gameObject.GetComponent<Animator>().Play(HIDE);
        }
    }

    public void UpgradeHealth()
    {
        if (PayDiamond(healthIncreaseCost))
        {
            healthLevel++;
            healthLevelText.text = LEVEL + healthLevel;
            PlayerPrefs.SetInt(HEALTHLEVEL, healthLevel);
            healthIncreaseCost = (healthLevel + 1) * 50;
            healthIncreaseCostText.text = healthIncreaseCost.ToString();

            Player.Instance.RestoreHealth(5);
        }
    }

    public void UpgradeSlash()
    {
        if (PayDiamond(slashIncreaseCost))
        {
            slashLevel++;
            slashLevelText.text = LEVEL + slashLevel;
            PlayerPrefs.SetInt(SLASHLEVEL, slashLevel);
            slashIncreaseCost = (slashLevel + 1) * 50;
            slashIncreaseCostText.text = slashIncreaseCost.ToString();

            Player.Instance.ModifySlashDamage(5);
        }
    }

    public void UpgradeCrit()
    {
        if (PayDiamond(critIncreaseCost))
        {
            critLevel++;
            critLevelText.text = LEVEL + slashLevel;
            PlayerPrefs.SetInt(CRITLEVEL, critLevel);
            critIncreaseCost = (slashLevel + 1) * 50;
            critIncreaseCostText.text = critIncreaseCost.ToString();

            Player.Instance.ModifyCritMultiplier(1);
        }
    }

    private bool PayDiamond(int statIncreaseCost)
    {
        if (diamondAmount >= statIncreaseCost)
        {
            diamondAmount -= statIncreaseCost;
            diamondAmountText.text = diamondAmount.ToString();
            PlayerPrefs.SetInt(DIAMONDAMOUNT, diamondAmount);
            OnUpgrade(this, EventArgs.Empty);
            return true;
        }
        return false;
    }


    public int GetHealthLevel()
    {
        return healthLevel;
    }

    public int GetSlashLevel()
    {
        return slashLevel;
    }
    public int GetCritLevel()
    {
        return critLevel;
    }
}
