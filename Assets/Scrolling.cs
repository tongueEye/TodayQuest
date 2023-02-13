using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{

    private float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Move();
        Scroll();
    }

    void Move()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.right * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scroll()
    {
        if (transform.position.x > 8)
        {
            float curY = transform.position.y;
            transform.position = new Vector3(-8, curY, 0);
        }
    }
}
