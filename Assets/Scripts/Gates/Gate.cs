using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gate : MonoBehaviour
{
    protected TextMeshProUGUI text;
    protected Gate neighbourGate;
    [SerializeField] protected int value;
    [SerializeField] protected int minValue = 20;
    [SerializeField] protected int maxValue = 31;
    [HideInInspector] public bool isGranted;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;
    }

    private void LevelLoader_OnRestartScene(object sender, System.EventArgs e)
    {
        isGranted = false;
    }
}
