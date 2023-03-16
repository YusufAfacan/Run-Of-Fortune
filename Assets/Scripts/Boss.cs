using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Boss : MonoBehaviour
{
    public static Boss Instance { get; private set; }

    public event EventHandler OnLightAttack;
    public event EventHandler OnHeavyAttack;
    public event EventHandler OnBossVictory;
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public int amount;
    }

    private const string LIGHT_ATTACK = "LightAttack";
    private const string HEAVY_ATTACK = "HeavyAttack";
    private const string IS_ENEMY_ALIVE = "IsEnemyAlive";
    private const string BATTLE_READY = "BattleReady";

    [SerializeField] private PickerWheel wheel;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject deathEffect;

    private Animator animator;

    [SerializeField] private int health;
    [SerializeField] private int bossLevel;
    private bool isLastAttackHeavy = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Player.Instance.OnVictory += Player_OnVictory;
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;
        animator = GetComponent<Animator>();
        bossLevel = PlayerPrefs.GetInt("BossLevel");
        health = bossLevel * 10 + 100;
        healthText.text = health.ToString();
        SetWheelPieceAmounts();
    }

    private void LevelLoader_OnRestartScene(object sender, EventArgs e)
    {
        animator.Play(BATTLE_READY);
    }

    private void Player_OnVictory(object sender, EventArgs e)
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void SetWheelPieceAmounts()
    {
        List<WheelPiece> ownedPieces = wheel.wheelPieces;

        foreach (WheelPiece ownedPiece in ownedPieces)
        {
            if (ownedPiece.Label == "LightAttack")
            {
                ownedPiece.amount = 15 + bossLevel * 2;
            }

            else if (ownedPiece.Label == "NormalAttack")
            {
                ownedPiece.amount = 20 + bossLevel * 3;
            }

            else if (ownedPiece.Label == "HeavyAttack")
            {
                ownedPiece.amount = 30 + bossLevel * 4;
            }
        }
    }
    public void AttackToTheOpponent()
    {
        wheel.gameObject.SetActive(true);
        animator.Play("Taunt");
        wheel.Spin();

        wheel.OnSpinEnd(wheelPiece =>
        {
            int amount = wheelPiece.amount;

            if (wheelPiece.Label == "LightAttack" || wheelPiece.Label == "NormalAttack")
            {
                OnLightAttack?.Invoke(this, EventArgs.Empty);
                animator.Play(LIGHT_ATTACK);
                isLastAttackHeavy = false;
                Attack(amount);
            }
            else if (wheelPiece.Label == "HeavyAttack")
            {
                OnHeavyAttack?.Invoke(this, EventArgs.Empty);
                animator.Play(HEAVY_ATTACK);
                isLastAttackHeavy = true;
                Attack(amount);
            }

        });
    }
    private void Attack(int attackAmount)
    {
        wheel.gameObject.SetActive(false);
        Player.Instance.TakeDamage(attackAmount);
        CheckOpponent();
    }

    private void CheckOpponent()
    {
        if (Player.Instance.IsAlive())
        {
            animator.SetBool(IS_ENEMY_ALIVE, true);
            Invoke(nameof(PassTurn), 1.5f);
        }
        else
        {
            animator.SetBool(IS_ENEMY_ALIVE, false);
            float delayPriorToAnimation = isLastAttackHeavy ? 1.3f : 0.8f;
            Invoke(nameof(BossVictory), delayPriorToAnimation);
        }

    }

    public void PassTurn()
    {
        Player.Instance.Turn();
    }

    public void Turn()
    {
        wheel.gameObject.SetActive(true);
        AttackToTheOpponent();
    }

    private void BossVictory()
    {
        OnBossVictory?.Invoke(this, EventArgs.Empty);
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

    public bool IsAlive()
    {
        if (health > 0) return true;
        else return false;
    }

    public int GetHealth()
    {
        return health;
    }
}
