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


    //GameManager ���� Animator ������ �����ϱ� ���� Animator �迭�� �Լ� �߰�
    public RuntimeAnimatorController[] level_ac;
    //������ ������ �����ϱ� ���� �迭 ����
    public int[] dough_goldlist;


    public void ChangeAc(Animator anim, int level)
    {
        //Dough ��ũ��Ʈ���� Animator ��ü�� level�� �޾ƿ�
        //runtimeAnimatorController�� ���� �ش������� ������ ���� Animator�� ����
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    //�̺�Ʈ �߻� �� CheckSell() �Լ��� �����ϵ��� ����
    //Pointer Enter�� ���콺 �����Ͱ� �ش� ��ư ������ ���� ��� �߻��ϴ� �̺�Ʈ
    //Pointer Exit�� ���콺 �����Ͱ� �ش� ��ư �������� ��� ��� �߻��ϴ� �̺�Ʈ
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
