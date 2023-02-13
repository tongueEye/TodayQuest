using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    public int id; //젤리의 level에 따라 얻는 재화의 크기를 다르게 하기 위해서 Jelly 오브젝트에 새로운 변수 id와 level을 추가
    public int level;
    public float exp; //level이 오르는 기준을 만들기 위한 변수

    //젤리의 exp 수치에 최댓값을 두어 해당 exp 수치까지 도달하게 되면 더이상 exp 증가 작업을 하지 않도록 함
    public float required_exp; //50
    public float max_exp; //100

    public GameManager game_manager; //GameManager에서 관리하는 재화 정보를 Jelly 스크립트 내에서 활용하기 위해 추가

    public int move_delay;	// 다음 이동까지의 딜레이 시간
    public int move_time;	// 이동 시간

    float speed_x;	// x축 방향 이동 속도
    float speed_y;	// y축 방향 이동 속도
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
            StartCoroutine(Wander());	// 코루틴 실행
        if (isWalking)
            Move();
    }

    void Move()
    {
        if (speed_x != 0)
            sprite.flipX = speed_x < 0; // x축 속도에 따라 Spite 이미지를 뒤집음

        transform.Translate(speed_x, speed_y, speed_y);	// 젤리 이동
    }

    IEnumerator Wander()
    {
        move_delay = 6;
        move_time = 3;

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

        if (exp < max_exp) ++exp; //시간이 지남에 따라 자동적으로 exp를 얻는 시스템 구현

        if (game_manager.jelatin < 99999999)
            game_manager.jelatin += (id + 1) * level;

        //GameManager 배열 변수 level_ac의 최대 크기는 3
        //level up에 필요한 exp를 설정 (50)
        /*
        if (++exp > 50 * level && level < 3)
            game_manager.ChangeAc(anim, ++level);*/
    }

    void Update()
    {
        if (exp < max_exp)
            exp += Time.deltaTime; //exp변수에 초당 1씩 더하면서


        //GameManager 배열 변수 level_ac의 최대 크기는 3
        //level up에 필요한 exp를 설정 (50)
        //시간이 지남에 따라 자연스럽게 레벨이 상승
        if (exp > required_exp * level && level < 3)
            game_manager.ChangeAc(anim, ++level);
    }

}