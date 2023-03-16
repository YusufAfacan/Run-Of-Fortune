using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnEngageBossFight;
    public event EventHandler OnSlashAttack;
    public event EventHandler OnCritAttack;
    public event EventHandler OnVictory;
    public event EventHandler OnDie;
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

    public class OnHealthChangedEventArgs : EventArgs
    {
        public int amount;
    }


    private const string ENGAGED_BOSS_FIGHT = "EngagedBossFight";
    private const string TAUNT = "Taunt";
    private const string BATTLE_READY = "BattleReady";
    private const string IS_ENEMY_ALIVE = "IsEnemyAlive";
    private const string SLASH = "Slash";
    private const string CRIT = "Crit";

    [SerializeField] private PickerWheel wheel;
    
    [SerializeField] private GameObject deathEffect;


    private Animator animator;

    private Vector3 startingPos;

    [SerializeField] private int health;
    [SerializeField] private int critMultiplier;
    [SerializeField] private int slashDamage;


    private bool engagedBossFight = false;

    private bool isLastAttackSlash = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        startingPos = transform.position;
        animator = GetComponent<Animator>();
        health = 100 + UpgradeManager.Instance.GetHealthLevel() * 5;
        slashDamage = 15 + UpgradeManager.Instance.GetSlashLevel() * 5;
        critMultiplier = 2 + UpgradeManager.Instance.GetCritLevel();
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;
        Boss.Instance.OnBossVictory += Boss_OnBossVictory;
    }

    private void Boss_OnBossVictory(object sender, EventArgs e)
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void LevelLoader_OnRestartScene(object sender, EventArgs e)
    {
        transform.position = startingPos;
        transform.localScale = Vector3.one;
        engagedBossFight = false;
        health = 100 + UpgradeManager.Instance.GetHealthLevel() * 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<Boss>())
        {
            EngageBossFight();
        }
    }

   

    private void EngageBossFight()
    {
        engagedBossFight = true;
        OnEngageBossFight(this, EventArgs.Empty);
        SetWheelPieceAmounts();
        AttackToTheOpponent();
    }

    private void SetWheelPieceAmounts()
    {
        List<WheelPiece> ownedPieces = wheel.wheelPieces;

        foreach (WheelPiece ownedPiece in ownedPieces)
        {
            switch (ownedPiece.Label)
            {
                case "Crit":
                    ownedPiece.amount = (slashDamage + UnityEngine.Random.Range(1, 11)) * critMultiplier;
                    break;
                case "Slash":
                    ownedPiece.amount = slashDamage + UnityEngine.Random.Range(1, 11);
                    break;
            }
        }
    }

    private void AttackToTheOpponent()
    {
        wheel.gameObject.SetActive(true);
        animator.Play(TAUNT);
        wheel.Spin();

        wheel.OnSpinEnd(wheelPiece =>
        {
            int amount = wheelPiece.amount;

            if (wheelPiece.Label == "Slash")
            {
                OnSlashAttack?.Invoke(this, EventArgs.Empty);
                animator.Play(SLASH);
                isLastAttackSlash = true;
                Attack(amount);
                
            }
            else if (wheelPiece.Label == "Crit")
            {
                OnCritAttack?.Invoke(this, EventArgs.Empty);
                animator.Play(CRIT);
                isLastAttackSlash = false;
                Attack(amount);
                
            }
        });
    }

    private void Attack(int attackAmount)
    {
        wheel.gameObject.SetActive(false);
        Boss.Instance.TakeDamage(attackAmount);
        CheckOpponent();
    }

    private void CheckOpponent()
    {
        if (Boss.Instance.IsAlive())
        {
            animator.SetBool(IS_ENEMY_ALIVE, true);
            Invoke(nameof(PassTurn), 1.5f);
        }

        else
        {
            animator.SetBool(IS_ENEMY_ALIVE, false);
            float delayPriorToAnimation = isLastAttackSlash ? 0.8f : 1.3f;
            Invoke(nameof(PlayerVictory), delayPriorToAnimation);

        }
    }

    public bool IsAlive()
    {
        if (health > 0) return true;
        else return false;
    }

    private void PassTurn()
    {
        Boss.Instance.Turn();
    }

    public void Turn()
    {
        wheel.gameObject.SetActive(true);
        AttackToTheOpponent();
    }

    private void PlayerVictory()
    {
        OnVictory?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseAttack(int amount)
    {
        slashDamage += amount;
    }

    public bool IsEngagedBossFight()
    {
        return engagedBossFight;
    }

    public void RestoreHealth(int amount)
    {
        health += amount;
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs() { amount = amount });
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs() { amount = amount });
    }

    
    public void ModifySlashDamage(int amount)
    {
        slashDamage += amount;
    }

    public void ModifyCritMultiplier(int amount)
    {
        critMultiplier += amount;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

}
