using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dough : MonoBehaviour
{
    public int id; //반죽의 level에 따라 얻는 재화의 크기를 다르게 하기 위해서 반죽 오브젝트에 새로운 변수 id와 level을 추가
    public int level;
    public float exp; //level이 오르는 기준을 만들기 위한 변수

    //맵 경계
    public GameObject left_top;
    public GameObject right_bottom;

    //반죽의 exp 수치에 최댓값을 두어 해당 exp 수치까지 도달하게 되면 더이상 exp 증가 작업을 하지 않도록 함
    public float required_exp; //50
    public float max_exp; //100

    public GameManager game_manager; //GameManager에서 관리하는 재화 정보를 Dough 스크립트 내에서 활용하기 위해 추가
    public GameObject game_manager_obj;

    public SpriteRenderer sprite_renderer;
    public Animator anim;

    float pick_time; // 단순 클릭과 드래그를 구분하기 위한 변수

    public int move_delay;	// 다음 이동까지의 딜레이 시간
    public int move_time;	// 이동 시간

    float speed_x;	// x축 방향 이동 속도
    float speed_y;	// y축 방향 이동 속도

    bool isWandering;
    bool isWalking;
    

    //반죽에 따라 그림자의 크기와 위치를 다르게 함
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
            exp += Time.deltaTime; //exp변수에 초당 1씩 더하면서


        //GameManager 배열 변수 level_ac의 최대 크기는 3
        //level up에 필요한 exp를 설정
        //시간이 지남에 따라 자연스럽게 레벨이 상승
        if (exp > required_exp * level && level < 3)
            game_manager.ChangeAc(anim, ++level);

        //자동 재화 획득 기능
        if (!isGetting)
            StartCoroutine(GetFlour());
    }


    void FixedUpdate()
    {
        if (!isWandering)
            StartCoroutine(Wander());	// 코루틴 실행
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
            sprite_renderer.flipX = speed_x < 0; // x축 속도에 따라 Sprite 이미지를 뒤집음

        transform.Translate(speed_x, speed_y, speed_y);	// 반죽 이동
    }

    IEnumerator Wander()
    {
        move_delay = Random.Range(3,6);
        move_time = Random.Range(3,6);

        // Translate로 이동할 시 Object가 텔레포트 하는 것을 방지하기 위해 Time.deltaTime을 곱해줌
        speed_x = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        speed_y = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        isWandering = true;

        yield return new WaitForSeconds(move_delay);

        isWalking = true;
        anim.SetBool("isWalk", true);	// 이동 애니메이션 실행

        yield return new WaitForSeconds(move_time);

        isWalking = false;
        anim.SetBool("isWalk", false);	// 이동 애니메이션 종료

        isWandering = false;
    }

    // 자동 재화 획득 기능
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

        if (exp < max_exp) ++exp; //시간이 지남에 따라 자동적으로 exp를 얻는 시스템 구현

        game_manager.GetFlour(id, level);

        SoundManager.instance.PlaySound("Touch");

    }


    //반죽이 드래그 될 경우에만 실행되는 코드 (start)

    void OnMouseDrag()
    {
        if (!game_manager.isLive) return;

        pick_time += Time.deltaTime;

        if (pick_time < 0.1f) return; //0.1초 미만 누르고 있을 경우 단순클릭으로 인지

        //0.1초 이상 누르고 있을 경우 드래그로 인지
        isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y)); //ScreenToWorldPoint를 통해 마우스의 위치를 월드 좌표계로 변경

        transform.position = point;
    }

    //반죽의 위치가 마우스 포인터에 의해 맵의 경계를 벗어 날 경우, 맵 중앙으로 위치를 초기화 시키는 기능
    void OnMouseUp()
    {
        if (!game_manager.isLive) return;

        pick_time = 0;

        //반죽을 sell 버튼 위에 올리면 isSell 변수는 true로 바뀌고
        if (game_manager.isSell)
        {
            game_manager.GetGold(id, level, this); //GetGold() 함수를 호출해 골드를 얻고, 반죽 오브젝트는 Destroy() 함수에 의해 사라짐

            Destroy(gameObject);
        }

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x ||
            pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            transform.position = new Vector3(0, -1, 0);
    }
    //반죽이 드래그 될 경우에만 실행되는 코드 (end)

}