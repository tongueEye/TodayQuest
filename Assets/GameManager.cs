using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int jelatin;
    public int gold;

    public Text jelatin_text;
    public Text gold_text;

    void LateUpdate()
    {
        jelatin_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }


    //GameManager 내에 Animator 변경을 관리하기 위한 Animator 배열과 함수 추가
    public RuntimeAnimatorController[] level_ac;


    public void ChangeAc(Animator anim, int level)
    {
        //Dough 스크립트에서 Animator 객체와 level을 받아와
        //runtimeAnimatorController를 통해 해당젤리의 레벨에 따라 Animator를 변경
        anim.runtimeAnimatorController = level_ac[level - 1];
    }
}
