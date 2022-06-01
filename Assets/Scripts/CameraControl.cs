using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private Player playerInventory;
    private GameObject boss;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = FindObjectOfType<Player>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        offset = transform.position - player.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInventory.EngagedFight)
        {
            transform.position = player.transform.position + offset;
        }

        else
        {
            float z = (player.transform.position.z + boss.transform.position.z) / 2;
            float y = 1.1f;
            float x = 1.5f;

            transform.position = new Vector3(x, y, z);
            transform.eulerAngles = new Vector3(22, -90, 0);
        }
    }

    
}
