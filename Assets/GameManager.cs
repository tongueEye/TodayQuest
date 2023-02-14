using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int flour;
    public int gold;

    public Text flour_text;
    public Text gold_text;

    void LateUpdate()
    {
        flour_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(flour_text.text), flour, 0.5f));
        gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }


    //GameManager 내에 Animator 변경을 관리하기 위한 Animator 배열과 함수 추가
    public RuntimeAnimatorController[] level_ac;
    //반죽의 가격을 저장하기 위한 배열 변수
    public int[] dough_goldlist;


    public void ChangeAc(Animator anim, int level)
    {
        //Dough 스크립트에서 Animator 객체와 level을 받아와
        //runtimeAnimatorController를 통해 해당젤리의 레벨에 따라 Animator를 변경
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    //이벤트 발생 시 CheckSell() 함수를 실행하도록 설정
    //Pointer Enter는 마우스 포인터가 해당 버튼 영역에 들어올 경우 발생하는 이벤트
    //Pointer Exit는 마우스 포인터가 해당 버튼 영역에서 벗어날 경우 발생하는 이벤트
    public bool isSell;

    void Awake()
    {
        isSell = false;
    }

    public void CheckSell()
    {
        isSell = isSell == false;
    }

    public int max_gold;

    public void GetGold(int id, int level)
    {
        gold += dough_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;
    }

    public int max_flour;

    public void GetFlour(int id, int level)
    {
        flour += (id + 1) * level;

        if (flour > max_flour)
            flour = max_flour;
    }

}
