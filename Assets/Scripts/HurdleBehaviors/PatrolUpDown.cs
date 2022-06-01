using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolUpDown : MonoBehaviour
{
    public float speed = 1f;
    public float delta = 2f;  //delta is the difference between min y to max y.
    public float startingY;
    private void Start()
    {
        startingY = transform.position.y;
    }

    void Update()
    {  
        float y = Mathf.Clamp(Mathf.PingPong(speed * Time.time, delta), startingY, delta);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = pos;
    }
}
