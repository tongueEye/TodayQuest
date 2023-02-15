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

    public Image dough_panel;
    public Image plant_panel;
    public Image option_panel;

    Animator dough_anim;
    Animator plant_anim;


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

    public bool isLive; // ������ Ȱ��ȭ/��Ȱ��ȭ ���¸� �����ϱ� ���� ����

    void Awake()
    {
        isSell = false;
        dough_anim = dough_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();

        isLive = true;
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


    bool isDoughClick;
    bool isPlantClick;

    //���� ��ư�� Ŭ�� �� �� �߻��ϸ� ������ ������� â�� ���� ������ ������ ����
    public void ClickDoughBtn()
    {
        if (isPlantClick)
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isDoughClick)
            dough_anim.SetTrigger("doHide");
        else
            dough_anim.SetTrigger("doShow");

        isDoughClick = !isDoughClick;
        isLive = !isLive;
    }

    public void ClickPlantBtn()
    {
        if (isDoughClick)
        {
            dough_anim.SetTrigger("doHide");
            isDoughClick = false;
            isLive = true;
        }

        if (isPlantClick)
            plant_anim.SetTrigger("doHide");
        else
            plant_anim.SetTrigger("doShow");

        isPlantClick = !isPlantClick;
        isLive = !isLive;
    }

    bool isOption;

    //Esc ��ư�� ������ �� ��� 3���� ��η� ������ �Ǹ� ���� �̹� ����� �ִ� UI â�� �ִٸ� Option Panel�� Ȱ��ȭ ��Ű�� ��� �̹� ����� �ִ� â�� ����
    void Option()
    {
        isOption = !isOption;
        isLive = !isLive;

        option_panel.gameObject.SetActive(isOption);
        Time.timeScale = isOption == true ? 0 : 1;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isDoughClick) ClickDoughBtn();
            else if (isPlantClick) ClickPlantBtn();
            else Option();
        }
    }

}
