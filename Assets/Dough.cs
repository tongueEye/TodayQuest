using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    public int id; //������ level�� ���� ��� ��ȭ�� ũ�⸦ �ٸ��� �ϱ� ���ؼ� Jelly ������Ʈ�� ���ο� ���� id�� level�� �߰�
    public int level;
    public float exp; //level�� ������ ������ ����� ���� ����

    //������ exp ��ġ�� �ִ��� �ξ� �ش� exp ��ġ���� �����ϰ� �Ǹ� ���̻� exp ���� �۾��� ���� �ʵ��� ��
    public float required_exp; //50
    public float max_exp; //100

    public GameManager game_manager; //GameManager���� �����ϴ� ��ȭ ������ Jelly ��ũ��Ʈ ������ Ȱ���ϱ� ���� �߰�

    public int move_delay;	// ���� �̵������� ������ �ð�
    public int move_time;	// �̵� �ð�

    float speed_x;	// x�� ���� �̵� �ӵ�
    float speed_y;	// y�� ���� �̵� �ӵ�
    bool isWandering;
    bool isWalking;

    SpriteRenderer sprite;
    Animator anim;

    

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isWandering = false;
        isWalking = false;
    }

    void FixedUpdate()
    {
        if (!isWandering)
            StartCoroutine(Wander());	// �ڷ�ƾ ����
        if (isWalking)
            Move();
    }

    void Move()
    {
        if (speed_x != 0)
            sprite.flipX = speed_x < 0; // x�� �ӵ��� ���� Spite �̹����� ������

        transform.Translate(speed_x, speed_y, speed_y);	// ���� �̵�
    }

    IEnumerator Wander()
    {
        move_delay = 6;
        move_time = 3;

        // Translate�� �̵��� �� Object�� �ڷ���Ʈ �ϴ� ���� �����ϱ� ���� Time.deltaTime�� ������
        speed_x = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        speed_y = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        isWandering = true;

        yield return new WaitForSeconds(move_delay);

        isWalking = true;
        anim.SetBool("isWalk", true);	// �̵� �ִϸ��̼� ����

        yield return new WaitForSeconds(move_time);

        isWalking = false;
        anim.SetBool("isWalk", false);	// �̵� �ִϸ��̼� ����

        isWandering = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bottom") || collision.gameObject.name.Contains("Top"))
            speed_y = -speed_y;
        else if (collision.gameObject.name.Contains("Left") || collision.gameObject.name.Contains("Right"))
            speed_x = -speed_x;
    }

    void OnMouseDown()
    {
        isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        if (exp < max_exp) ++exp; //�ð��� ������ ���� �ڵ������� exp�� ��� �ý��� ����

        if (game_manager.jelatin < 99999999)
            game_manager.jelatin += (id + 1) * level;

        //GameManager �迭 ���� level_ac�� �ִ� ũ��� 3
        //level up�� �ʿ��� exp�� ���� (50)
        /*
        if (++exp > 50 * level && level < 3)
            game_manager.ChangeAc(anim, ++level);*/
    }

    void Update()
    {
        if (exp < max_exp)
            exp += Time.deltaTime; //exp������ �ʴ� 1�� ���ϸ鼭


        //GameManager �迭 ���� level_ac�� �ִ� ũ��� 3
        //level up�� �ʿ��� exp�� ���� (50)
        //�ð��� ������ ���� �ڿ������� ������ ���
        if (exp > required_exp * level && level < 3)
            game_manager.ChangeAc(anim, ++level);
    }

}