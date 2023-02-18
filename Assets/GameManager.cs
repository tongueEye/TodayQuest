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

    //������ Sprite, �̸�, ������ �����ϱ� ���� �迭 ����
    public Sprite[] dough_spritelist;
    public string[] dough_namelist;
    public int[] dough_flourlist;


    //��ư Ŭ���� ���� ������ �̵�
    public Text page_text;
    public Image unlock_group_dough_img;
    public Text unlock_group_gold_text;
    public Text unlock_group_name_text;

    int page;

    //��ư�� Ŭ���� �� ȣ��Ǹ� page ������ ���� ������Ű��, ChangePage() �Լ��� ȣ��
    public void PageUp()
    {
        if (page >= 13) return;

        ++page;
        ChangePage();
    }

    //��ư�� Ŭ���� �� ȣ��Ǹ� page ������ ���� ���ҽ�Ű��, ChangePage() �Լ��� ȣ��
    public void PageDown()
    {
        if (page <= 0) return;

        --page;
        ChangePage();
    }

    // �� ������Ʈ�� Sprite �Ǵ� Text�� �����Ͽ� ��ġ ���� �������� �Ѿ�� ��ó�� ����
    void ChangePage()
    {
        page_text.text = string.Format("#{0:00}", (page + 1));
        unlock_group_dough_img.sprite = dough_spritelist[page];
        unlock_group_name_text.text = dough_namelist[page];
        unlock_group_gold_text.text = string.Format("{0:n0}", dough_goldlist[page]);

        unlock_group_dough_img.SetNativeSize(); //Sprite Image�� ������ ���� ����
    }
}
