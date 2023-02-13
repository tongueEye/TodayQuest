using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoving : MonoBehaviour
{
    bool isIdle = true;
    bool isMoveStart = false;

    float timer = 0;
    float randrange;
    float xspeed;
    float yspeed;

    Animator anim;

    SpriteRenderer spriteRenderer;

    public GameObject borderTopLeft;
    public GameObject borderBottomRight;

    private void Awake()
    {
        randrange = Random.Range(2f, 5f);
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isIdle)
        {
            Idle();
        }
        else
        {
            Move();
        }
        ChangeTimer();
    }

    void Idle()
    {
        anim.SetBool("isWalk", false);
        if (randrange > timer)
            return;

        Debug.Log("idle");

        Init();
        isMoveStart = true;
    }

    void Move()
    {
        if (isMoveStart)
        {
            MakeDirection();
            Debug.Log("move");
        }

        Vector3 curPos = transform.position;

        float xpos = curPos.x + xspeed * Time.deltaTime;
        float ypos = curPos.y + yspeed * Time.deltaTime;

        if (xspeed < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (xpos > borderBottomRight.transform.position.x || xpos < borderTopLeft.transform.position.x)
            xspeed = -xspeed;

        if (ypos > borderTopLeft.transform.position.y || ypos < borderBottomRight.transform.position.y)
            yspeed = -yspeed;

        transform.position = new Vector3(xpos, ypos, ypos);

        anim.SetBool("isWalk", true);

        if (randrange < timer)
        {
            Init();
        }
    }

    void ChangeTimer()
    {
        timer += Time.deltaTime;
    }

    void Init()
    {
        randrange = Random.Range(2f, 5f);
        isIdle = !isIdle;
        timer = 0;
    }

    void MakeDirection()
    {
        xspeed = Random.Range(-0.7f, 0.7f);
        yspeed = Random.Range(-0.7f, 0.7f);
        isMoveStart = false;
    }
}
