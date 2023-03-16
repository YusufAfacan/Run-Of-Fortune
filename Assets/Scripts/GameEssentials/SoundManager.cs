using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private float footStepTimer;
    private float footStepTimerMax = 0.4f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        footStepTimer -= Time.deltaTime;
    }
    private void Start()
    {
        Player.Instance.OnSlashAttack += Player_OnSlashAttack;
        Player.Instance.OnCritAttack += Player_OnCritAttack;
        Boss.Instance.OnLightAttack += Boss_OnLightAttack;
        Boss.Instance.OnHeavyAttack += Boss_OnHeavyAttack;
        UpgradeManager.Instance.OnUpgrade += UpgradeManager_OnUpgrade;
        Player.Instance.OnVictory += Player_OnVictory;
    }

    private void Player_OnVictory(object sender, System.EventArgs e)
    {
        PlayVictoryAudioClip();
    }

    private void UpgradeManager_OnUpgrade(object sender, System.EventArgs e)
    {
        PlayUpgradeAudioClip();
    }

    private void Boss_OnHeavyAttack(object sender, System.EventArgs e)
    {
        float delayPriorToAnimation = 0.9f;

        Invoke(nameof(PlayCritAttackAudioClip), delayPriorToAnimation);
    }

    private void Boss_OnLightAttack(object sender, System.EventArgs e)
    {
        float delayPriorToAnimation = 0.5f;

        Invoke(nameof(PlaySlashAttackAudioClip), delayPriorToAnimation);
    }

    private void Player_OnCritAttack(object sender, System.EventArgs e)
    {
        float delayPriorToAnimation = 0.9f;

        Invoke(nameof(PlayCritAttackAudioClip), delayPriorToAnimation);
    }

    private void Player_OnSlashAttack(object sender, System.EventArgs e)
    {
        float delayPriorToAnimation = 0.5f;

        Invoke(nameof(PlaySlashAttackAudioClip), delayPriorToAnimation);
    }

    public void PlaySlashAttackAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.slashAttack, Player.Instance.transform.position, 1f);
    }

    public void PlayCritAttackAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.critAttack, Player.Instance.transform.position, 1f);
    }

    public void PlayFootStepAudioClip()
    {
        if (footStepTimer < 0)
        {
            PlayAudioClip(audioClipsRefsSO.footSteps, Player.Instance.transform.position, 1f);
            footStepTimer = footStepTimerMax;
        }
    }

    public void PlayRotatingSawAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.rotatingSaw, Player.Instance.transform.position, 1f);
    }

    public void PlayHealthGateAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.healthPortal, Player.Instance.transform.position, 1f);
    }

    public void PlayAttackGateAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.attackPortal, Player.Instance.transform.position, 1f);
    }

    public void PlayUpgradeAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.upgrade, Player.Instance.transform.position, 1f);
    }

    public void PlayVictoryAudioClip()
    {
        PlayAudioClip(audioClipsRefsSO.victory, Player.Instance.transform.position, 0.5f);
    }

    public void PlayAudioClip(AudioClip audioClip, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

}
