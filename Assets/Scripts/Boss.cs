using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    public PickerWheel pickerWheel;
    public bool isAlive;
    public float health;
    public Player player;
    private Animator animator;
    public float bombDamage;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            isAlive = true;
        }
        else
        {
            isAlive = false;
        }
    }

    public void Attack()
    {
        pickerWheel.gameObject.SetActive(true); 
        pickerWheel.Spin(); 

        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            pickerWheel.gameObject.SetActive(false);

            if (wheelPiece.Label == "Bomb")
            {
                ThrowBomb();
            }

        });
    }
    private void ThrowBomb()
    {
        animator.Play("ThrowBomb");
        player.health -= bombDamage;
        Invoke(nameof(PlayerTurn), animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void PlayerTurn()
    {
        if (player.isAlive)
        {
            player.ReadyForSpin();
           
        }
        else
        {
            Debug.Log("GAME OVER");
        }
    }
}
