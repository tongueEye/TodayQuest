using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dough : MonoBehaviour
{
    public int id; //������ level�� ���� ��� ��ȭ�� ũ�⸦ �ٸ��� �ϱ� ���ؼ� ���� ������Ʈ�� ���ο� ���� id�� level�� �߰�
    public int level;
    public float exp; //level�� ������ ������ ����� ���� ����

    //�� ���
    public GameObject left_top;
    public GameObject right_bottom;

    //������ exp ��ġ�� �ִ��� �ξ� �ش� exp ��ġ���� �����ϰ� �Ǹ� ���̻� exp ���� �۾��� ���� �ʵ��� ��
    public float required_exp; //50
    public float max_exp; //100

    public GameManager game_manager; //GameManager���� �����ϴ� ��ȭ ������ Dough ��ũ��Ʈ ������ Ȱ���ϱ� ���� �߰�
    public GameObject game_manager_obj;

    public SpriteRenderer sprite_renderer;
    public Animator anim;

    float pick_time; // �ܼ� Ŭ���� �巡�׸� �����ϱ� ���� ����

    public int move_delay;	// ���� �̵������� ������ �ð�
    public int move_time;	// �̵� �ð�

    float speed_x;	// x�� ���� �̵� �ӵ�
    float speed_y;	// y�� ���� �̵� �ӵ�

    bool isWandering;
    bool isWalking;
    

    //���׿� ���� �׸����� ũ��� ��ġ�� �ٸ��� ��
    GameObject shadow;
    float shadow_pos_y;
    float shadow_scale_x;
    float shadow_scale_y;

    int flour_delay;
    bool isGetting;

    void Awake()
    {
        left_top = GameObject.Find("LeftTop").gameObject;
        right_bottom = GameObject.Find("RightBottom").gameObject;
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isWandering = false;
        isWalking = false;
        isGetting = false;

        shadow = transform.Find("Shadow").gameObject;
        switch (id)
        {
            case 8:
                {
                    shadow_pos_y = -0.75f;
                    shadow_scale_x = 0.9f;
                    shadow_scale_y = 1.0f;
                    break;
                }
            case 9:
                {
                    shadow_pos_y = -0.75f;
                    shadow_scale_x = 0.9f;
                    shadow_scale_y = 1.0f;
                    break;
                }
            case 12:
                {
                    shadow_pos_y = -0.72f;
                    shadow_scale_x = 1.2f;
                    shadow_scale_y = 1.2f;
                    break;
                }
            default:
                {
                    shadow_pos_y = -0.75f;
                    shadow_scale_x = 1.1f;
                    shadow_scale_y = 1.0f;
                    break;
                }

        }

        shadow.transform.localPosition = new Vector3(0, shadow_pos_y, 0);
        shadow.transform.localScale = new Vector3(shadow_scale_x, shadow_scale_y, 0);
    }


    void Update()
    {
        if (exp < max_exp)
            exp += Time.deltaTime; //exp������ �ʴ� 1�� ���ϸ鼭


        //GameManager �迭 ���� level_ac�� �ִ� ũ��� 3
        //level up�� �ʿ��� exp�� ����
        //�ð��� ������ ���� �ڿ������� ������ ���
        if (exp > required_exp * level && level < 3)
            game_manager.ChangeAc(anim, ++level);

        //�ڵ� ��ȭ ȹ�� ���
        if (!isGetting)
            StartCoroutine(GetFlour());
    }


    void FixedUpdate()
    {
        if (!isWandering)
            StartCoroutine(Wander());	// �ڷ�ƾ ����
        if (isWalking)
            Move();

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x)
            speed_x = -speed_x;
        if (pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            speed_y = -speed_y;
    }

    void Move()
    {
        if (speed_x != 0)
            sprite_renderer.flipX = speed_x < 0; // x�� �ӵ��� ���� Sprite �̹����� ������

        transform.Translate(speed_x, speed_y, speed_y);	// ���� �̵�
    }

    IEnumerator Wander()
    {
        move_delay = Random.Range(3,6);
        move_time = Random.Range(3,6);

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

    // �ڵ� ��ȭ ȹ�� ���
    IEnumerator GetFlour()
    {
        flour_delay = 3;

        isGetting = true;
        game_manager.GetFlour(id, level);

        yield return new WaitForSeconds(flour_delay);

        isGetting = false;
    }

    void OnMouseDown()
    {
        if (!game_manager.isLive) return;

        isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        if (exp < max_exp) ++exp; //�ð��� ������ ���� �ڵ������� exp�� ��� �ý��� ����

        game_manager.GetFlour(id, level);

        SoundManager.instance.PlaySound("Touch");

    }


    //������ �巡�� �� ��쿡�� ����Ǵ� �ڵ� (start)

    void OnMouseDrag()
    {
        if (!game_manager.isLive) return;

        pick_time += Time.deltaTime;

        if (pick_time < 0.1f) return; //0.1�� �̸� ������ ���� ��� �ܼ�Ŭ������ ����

        //0.1�� �̻� ������ ���� ��� �巡�׷� ����
        isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y)); //ScreenToWorldPoint�� ���� ���콺�� ��ġ�� ���� ��ǥ��� ����

        transform.position = point;
    }

    //������ ��ġ�� ���콺 �����Ϳ� ���� ���� ��踦 ���� �� ���, �� �߾����� ��ġ�� �ʱ�ȭ ��Ű�� ���
    void OnMouseUp()
    {
        if (!game_manager.isLive) return;

        pick_time = 0;

        //������ sell ��ư ���� �ø��� isSell ������ true�� �ٲ��
        if (game_manager.isSell)
        {
            game_manager.GetGold(id, level, this); //GetGold() �Լ��� ȣ���� ��带 ���, ���� ������Ʈ�� Destroy() �Լ��� ���� �����

            Destroy(gameObject);
        }

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x ||
            pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            transform.position = new Vector3(0, -1, 0);
    }
    //������ �巡�� �� ��쿡�� ����Ǵ� �ڵ� (end)

}