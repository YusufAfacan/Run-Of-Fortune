using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLeftRight : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hurdle"))
        {
            speed = -speed;
        }

        if (other.gameObject.CompareTag("Set"))
        {
            speed = -speed;
        }

    }
}
