using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public event EventHandler OnRestartScene;

    private AdManager adManager;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.OnVictory += Player_OnVictory;
        Player.Instance.OnDie += Player_OnDie;
        Boss.Instance.OnBossVictory += Boss_OnBossVictory;
        adManager = FindObjectOfType<AdManager>();


    }

    private void Player_OnDie(object sender, EventArgs e)
    {
        Invoke(nameof(ShowAd), 3f);
    }

    private void Boss_OnBossVictory(object sender, EventArgs e)
    {
        Invoke(nameof(ShowAd), 3f);
    }

    private void Player_OnVictory(object sender, System.EventArgs e)
    {
        Invoke(nameof(ShowAd), 3f);
    }

    public void RestartScene()
    {
        
        Player.Instance.gameObject.SetActive(true);
        OnRestartScene?.Invoke(this, EventArgs.Empty);
        

    }

    public void LoadNextScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }




    private void ShowAd()
    {
        adManager.ShowAd();
    }
}
