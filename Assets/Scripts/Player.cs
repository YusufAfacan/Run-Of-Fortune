using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public float forwardSpeed;
    public float thrustSpeed;

    private Animator animator;

    private bool isRunning;
    public bool EngagedFight;
    public bool isAlive;

    public float health;

    public Button playerWheelSpinButton;
    public Text playerWheelSpinButtonText;
    public PickerWheel playerWheel;

    private Boss boss;

    private CameraControl cam;

    public GameManager gameManager;

    public int itemIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerWheel = gameManager.GeneratePickerWheel();

        itemIndex = 0;

        EngagedFight = false;
        
        isRunning = false;
        animator = GetComponent<Animator>();
        boss = GameObject.FindObjectOfType<Boss>();
  
        cam = FindObjectOfType<CameraControl>();
        

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckAlive();
    }

   

    private void CheckAlive()
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
    private void Movement()
    {
        if (Input.GetKey(KeyCode.W) && !EngagedFight)
        {
            transform.position += forwardSpeed * Time.deltaTime * Vector3.forward;
            isRunning = true;
            animator.SetBool("isRunning", true);
        }
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }

        if (isRunning && Input.GetKey(KeyCode.A))
        {
            transform.position += thrustSpeed * Time.deltaTime * Vector3.left;
        }

        if (isRunning && Input.GetKey(KeyCode.D))
        {
            transform.position += thrustSpeed * Time.deltaTime * Vector3.right;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Boss"))
        {
            EngageBossFight();
        }

        if (other.gameObject.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            
            WheelPiece wheelPiece = new WheelPiece
            {
                Icon = item.icon,
                Chance = 100f,
                Index = itemIndex,
                Label = item.label,
                _weight = 0f,

            };

            playerWheel.wheelPieces.Add(wheelPiece);

            itemIndex++;
        }
    }

    public void EngageBossFight()
    {
        EngagedFight = true;
        animator.SetBool("isDefending", true);
        
        playerWheel.gameObject.SetActive(true);
        playerWheelSpinButton.gameObject.SetActive(true);
    }


    public void ReadyForSpin()
    {

        playerWheel.gameObject.SetActive(true);
        playerWheelSpinButton.gameObject.SetActive(true);
        playerWheelSpinButton.interactable = true;
    }

    public void AttackToBoss()
    {
        playerWheelSpinButton.interactable = false;
        playerWheel.Spin();
        animator.Play("Taunt");
        playerWheelSpinButtonText.text = "Spinning";

        playerWheel.GetComponent<PickerWheel>().OnSpinEnd(wheelPiece =>
        {
            playerWheelSpinButtonText.text = wheelPiece.Label;
            
            playerWheelSpinButton.gameObject.SetActive(false);

            if (wheelPiece.Label == "Bomb")
            {
                ThrowBomb(wheelPiece);
            }

            if (wheelPiece.Label == "Bat")
            {
                MeleeAttack(wheelPiece);
            }

        });

    }

    private void ThrowBomb(WheelPiece wheelPiece)
    {
        animator.Play("ThrowBomb");
        playerWheel.wheelPieces.Remove(wheelPiece);
        playerWheel.gameObject.SetActive(false);

        PickerWheel newWheel = gameManager.GeneratePickerWheel();

        for (int i = 0; i < playerWheel.wheelPieces.Count; i++)
        {
            newWheel.wheelPieces[i] = playerWheel.wheelPieces[i];
        }

        playerWheel = newWheel;
        
       
        Invoke(nameof(BossTurn), animator.GetCurrentAnimatorStateInfo(0).length);

        
        
    }

    private void MeleeAttack(WheelPiece wheelPiece)
    {
        animator.Play("Punch");
        playerWheel.wheelPieces.Remove(wheelPiece);
        playerWheel.gameObject.SetActive(false);

        PickerWheel newWheel = gameManager.GeneratePickerWheel();

        for (int i = 0; i < playerWheel.wheelPieces.Count; i++)
        {
            newWheel.wheelPieces[i] = playerWheel.wheelPieces[i];
        }

        playerWheel = newWheel;

        Invoke(nameof(BossTurn), animator.GetCurrentAnimatorStateInfo(0).length);
        
    }

    private void BossTurn()
    {
        if (boss.isAlive)
        {
            animator.SetBool("isDefending", true);
            boss.Attack();
        }
        else
        {
            Debug.Log("LEVEL FÝNÝSHED");
        }
    } 
}
