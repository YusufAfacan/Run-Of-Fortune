using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] hurdleBlocks;
    public GameObject[] gates;
    

    public Transform[] hurdleBlockPoses;
    public Transform[] gatesPoses;
    

    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < hurdleBlockPoses.Length; i++)
        {
            int index = Random.Range(0, hurdleBlocks.Length);
            Instantiate(hurdleBlocks[index], hurdleBlockPoses[i].position, Quaternion.identity);
        }

        for (int i = 0; i < gatesPoses.Length; i++)
        {
            int index = Random.Range(0, gates.Length);
            Instantiate(gates[index], gatesPoses[i].position, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
