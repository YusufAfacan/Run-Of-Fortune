using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public event EventHandler OnLevelStarted;

    [SerializeField] private VariableJoystick joystick;
    private Rigidbody rb;
    private Animator animator;
    private bool canControl = true;
    private const string IS_RUNNING = "IsRunning";
    private bool levelStarted = false;

    [SerializeField] private float forwardSpeed;
    [SerializeField] private float thrustSpeed;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        LevelLoader.Instance.OnRestartScene += LevelLoader_OnRestartScene;
    }

    private void LevelLoader_OnRestartScene(object sender, EventArgs e)
    {
        canControl = true;
        levelStarted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            StartCoroutine(MoveToBoss(transform.position, Boss.Instance.transform.position - Vector3.forward, 2f));
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;

        float platformboundary = 2.5f;
        float xMove = joystick.Horizontal * thrustSpeed;
        float zMove = joystick.Vertical * forwardSpeed;
        Vector3 posToMove;

        if (rb.position.x > platformboundary)
        {
            rb.position = new Vector3(platformboundary, rb.position.y, rb.position.z);
        }

        if (rb.position.x < -platformboundary)
        {
            rb.position = new Vector3(-platformboundary, rb.position.y, rb.position.z);
        }

        if (canControl)
        {
            if (joystick.Vertical > 0)
            {
                

                if (transform.position.x >= platformboundary)
                {
                    if (joystick.Horizontal > 0)
                    {
                        posToMove = rb.position + new Vector3(0, 0, zMove) * Time.fixedDeltaTime;

                    }
                    else
                    {
                        posToMove = rb.position + new Vector3(xMove, 0, zMove) * Time.fixedDeltaTime;

                    }
                    
                }
                else if (transform.position.x <= -platformboundary)
                {
                    if (joystick.Horizontal > 0)
                    {
                        posToMove = rb.position + new Vector3(xMove, 0, zMove) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        posToMove = rb.position + new Vector3(0, 0, zMove) * Time.fixedDeltaTime;
                    }

                }
                else
                {
                    posToMove = rb.position + new Vector3(xMove, 0, zMove) * Time.fixedDeltaTime;
                }

                rb.MovePosition(posToMove);

                animator.SetBool(IS_RUNNING, true);

                SoundManager.Instance.PlayFootStepAudioClip();

                if (levelStarted == false)
                {
                    OnLevelStarted?.Invoke(this, EventArgs.Empty);
                    levelStarted = true;
                }
            }
            else
            {
                animator.SetBool(IS_RUNNING, false);
            }
        }
    }

    private IEnumerator MoveToBoss(Vector3 beginPos, Vector3 endPos, float time)
    {
        animator.SetBool(IS_RUNNING, true);
        canControl = false;

        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            SoundManager.Instance.PlayFootStepAudioClip();
            transform.position = Vector3.Lerp(beginPos, endPos, t);
            yield return null;
        }
        animator.SetBool(IS_RUNNING, false);
    }

}
