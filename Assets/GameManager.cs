using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int flour;
    public int gold;

    public List<Dough> dough_list = new List<Dough>(); // ������ ��� �ʿ� ���� ���� �����Ǿ� �ִ� ������ �����ϰ� �����ϱ� ���� ����Ʈ
    public List<Data> dough_data_list = new List<Data>();
    //public List<Quest> quest_data_list = new List<Quest>(); // To-Do List(����Ʈ) �����͸� �����ϰ� �����ϱ� ���� ����Ʈ

    public bool[] dough_unlock_list; // ������ �ر� ���¸� Ȯ���ϱ� ���� �迭

    public int max_flour;
    public int max_gold;

    public bool isSell;
    public bool isLive; // ������ Ȱ��ȭ/��Ȱ��ȭ ���¸� �����ϱ� ���� ����

    //������ Sprite, �̸�, ������ �����ϱ� ���� �迭 ����
    public Sprite[] dough_spritelist;
    public string[] dough_namelist;
    public int[] dough_flourlist;
    public int[] dough_goldlist;

    //��ư Ŭ���� ���� ������ �̵�
    public Text page_text;
    public Image unlock_group_dough_img;
    public Text unlock_group_gold_text;
    public Text unlock_group_name_text;

    // Lock Group ������Ʈ�� ��Ʈ���ϱ� ���� ����
    public GameObject lock_group;
    public Image lock_group_dough_img;
    public Text lock_group_flour_text;

    //GameManager ���� Animator ������ �����ϱ� ���� Animator �迭�� �Լ� �߰�
    public RuntimeAnimatorController[] level_ac;

    public Text flour_text;
    public Text gold_text;

    public Image dough_panel;
    public Image plant_panel;
    public Image quest_panel;
    public Image quest_edit_panel;
    public Image option_panel;

    public Image alert_panel1;
    public Image alert_panel2;
    public Image alert_panel3;

    public GameObject prefab;

    public GameObject data_manager_obj;

    DataManager data_manager;

    Animator dough_anim;
    Animator plant_anim;
    Animator quest_anim;
    Animator quest_edit_anim;

    bool isDoughClick;
    bool isPlantClick;
    bool isQuestClick;
    bool isQuestEditClick;
    bool isOption;

    bool isReward;
    bool isntReward1;
    bool isntReward2;

    int page;
    int dough_ea;

    public Text num_sub_text;
    public Text num_btn_text;
    public Button num_btn;

    public Text click_sub_text;
    public Text click_btn_text;
    public Button click_btn;

    public int[] num_gold_list;
    public int[] click_gold_list;

    public int num_level;
    public int click_level;

    //����Ʈ ����Ʈ (��ũ�� ��)
    public GameObject list_contents;
    public GameObject edit_contents;
    public GameObject uiListItemPrefab;
    public GameObject uiListEditItemPrefab;

    // ����� ���� ����
    public bool possibleGet=true;
    public string getDay = "";

    void Awake()
    {
        instance = this;

        dough_anim = dough_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        quest_anim = quest_panel.GetComponent<Animator>();
        quest_edit_anim = quest_edit_panel.GetComponent<Animator>();

        isLive = true;

        flour_text.text = flour.ToString();
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = dough_goldlist[0].ToString();
        lock_group_flour_text.text = dough_flourlist[0].ToString();

        num_btn_text.text = num_gold_list[1].ToString();
        click_btn_text.text = click_gold_list[1].ToString();

        data_manager = data_manager_obj.GetComponent<DataManager>();

        page = 0;
        dough_ea = 14; //���� ������ ����
        dough_unlock_list = new bool[dough_ea];

    }


    void Start()
    {
        //DataManager�� ���� �����Ͱ� �ε�Ǳ� ���� GameManager�� Ȱ��ȭ �Ǿ� �� �����͸� �����ϴ� ������ �����ϱ� ����
        Invoke("LoadData", 0.1f);
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isDoughClick) ClickDoughBtn();
            else if (isPlantClick) ClickPlantBtn();
            else if (isQuestClick) ClickQuestBtn();
            else if (isQuestEditClick) ClickQuestEditBtn();
            else Option();
        }

    }


    void LateUpdate()
    {
        flour_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(flour_text.text), flour, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }


    public void ChangeAc(Animator anim, int level)
    {
        //Dough ��ũ��Ʈ���� Animator ��ü�� level�� �޾ƿ�
        //runtimeAnimatorController�� ���� �ش������ ������ ���� Animator�� ����
        anim.runtimeAnimatorController = level_ac[level - 1];
        SoundManager.instance.PlaySound("Grow");
    }


    public void GetGold(int id, int level, Dough dough)
    {
        gold += dough_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;

        dough_list.Remove(dough);

        SoundManager.instance.PlaySound("Sell");
    }



    public void GetFlour(int id, int level)
    {
        flour += (id + 1) * level * click_level; //click_level�� ���� Ŭ������ ��� flour�� ���� �޶����� ��

        if (flour > max_flour)
            flour = max_flour;
    }


    public void CheckSell()
    {
        isSell = isSell == false;
    }


    //���� ��ư�� Ŭ�� �� �� �߻��ϸ� ������ ������� â�� ���� ������ ������ ����
    public void ClickDoughBtn()
    {
        if (isPlantClick)
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isQuestClick)
        {
            quest_anim.SetTrigger("doHide");
            isQuestClick = false;
            isLive = true;
        }

        if (isQuestEditClick)
        {
            quest_edit_anim.SetTrigger("doHide");
            isQuestEditClick = false;
            isLive = true;
        }

        if (isDoughClick)
            dough_anim.SetTrigger("doHide");
        else
            dough_anim.SetTrigger("doShow");

        isDoughClick = !isDoughClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }

    public void ClickPlantBtn()
    {
        if (isDoughClick)
        {
            dough_anim.SetTrigger("doHide");
            isDoughClick = false;
            isLive = true;
        }

        if (isQuestClick)
        {
            quest_anim.SetTrigger("doHide");
            isQuestClick = false;
            isLive = true;
        }

        if (isQuestEditClick)
        {
            quest_edit_anim.SetTrigger("doHide");
            isQuestEditClick = false;
            isLive = true;
        }

        if (isPlantClick)
            plant_anim.SetTrigger("doHide");
        else
            plant_anim.SetTrigger("doShow");

        isPlantClick = !isPlantClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }

    public void ClickQuestBtn()
    {
        if (isDoughClick)
        {
            dough_anim.SetTrigger("doHide");
            isDoughClick = false;
            isLive = true;
        }

        if (isPlantClick)
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isQuestEditClick)
        {
            quest_edit_anim.SetTrigger("doHide");
            isQuestEditClick = false;
            isLive = true;
        }

        if (isQuestClick)
            quest_anim.SetTrigger("doHide");
        else
            quest_anim.SetTrigger("doShow");

        isQuestClick = !isQuestClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }

    public void ClickQuestEditBtn()
    {

        if (isDoughClick)
        {
            dough_anim.SetTrigger("doHide");
            isDoughClick = false;
            isLive = true;
        }

        if (isPlantClick)
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isQuestClick)
        {
            quest_anim.SetTrigger("doHide");
            isQuestClick = false;
            isLive = true;
        }


        if (isQuestEditClick)
            quest_edit_anim.SetTrigger("doHide");
        else
            quest_edit_anim.SetTrigger("doShow");

        isQuestEditClick = !isQuestEditClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }


    //Esc ��ư�� ������ �� ��� 3���� ��η� ������ �Ǹ� ���� �̹� ����� �ִ� UI â�� �ִٸ� Option Panel�� Ȱ��ȭ ��Ű�� ��� �̹� ����� �ִ� â�� ����
    public void Option()
    {
        isOption = !isOption;
        isLive = !isLive;

        option_panel.gameObject.SetActive(isOption);

        Time.timeScale = isOption == true ? 0 : 1;
        SoundManager.instance.PlaySound("Pause In");
    }

    public void RewardClick()
    {
        isReward = !isReward;
        isLive = !isLive;

        alert_panel1.gameObject.SetActive(isReward);
        SoundManager.instance.PlaySound("Button");
    }

    public void RewardClickNo1()
    {
        isntReward1 = !isntReward1;
        isLive = !isLive;

        alert_panel2.gameObject.SetActive(isntReward1);
        SoundManager.instance.PlaySound("Button");
    }

    public void RewardClickNo2()
    {
        isntReward2 = !isntReward2;
        isLive = !isLive;

        alert_panel3.gameObject.SetActive(isntReward2);
        SoundManager.instance.PlaySound("Button");
    }


    //��ư�� Ŭ���� �� ȣ��Ǹ� page ������ ���� ������Ű��, ChangePage() �Լ��� ȣ��
    public void PageUp()
    {
        if (page >= dough_ea-1) return;

        ++page;
        ChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    //��ư�� Ŭ���� �� ȣ��Ǹ� page ������ ���� ���ҽ�Ű��, ChangePage() �Լ��� ȣ��
    public void PageDown()
    {
        if (page <= 0) return;

        --page;
        ChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    // �� ������Ʈ�� Sprite �Ǵ� Text�� �����Ͽ� ��ġ ���� �������� �Ѿ�� ��ó�� ����
    void ChangePage()
    {
        lock_group.gameObject.SetActive(!dough_unlock_list[page]);

        page_text.text = string.Format("#{0:00}", (page + 1));

        // �ر� ���¿� ���� ��Ȳ�� �´� �������̽��� ������ �� �ֵ��� ��
        if (lock_group.activeSelf)
        {
            lock_group_dough_img.sprite = dough_spritelist[page];
            lock_group_flour_text.text = string.Format("{0:n0}", dough_flourlist[page]);

            lock_group_dough_img.SetNativeSize();
        }

        else
        {
            unlock_group_dough_img.sprite = dough_spritelist[page];
            unlock_group_name_text.text = dough_namelist[page];
            unlock_group_gold_text.text = string.Format("{0:n0}", dough_goldlist[page]);

            unlock_group_dough_img.SetNativeSize(); //Sprite Image�� ������ ���� ����
        }
    }


    // ���� ��ư Ŭ�� �� ȣ��Ǹ�, ���� �����ϰ� �ִ� flour�� ���� ���� ������ �رݽ�ų �� ����.
    public void Unlock()
    {
        if (flour < dough_flourlist[page])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }
        dough_unlock_list[page] = true;
        ChangePage();

        flour -= dough_flourlist[page];

        SoundManager.instance.PlaySound("Unlock");
    }


    // ����(��) ���� ���
    public void BuyDough()
    {
        if (gold < dough_goldlist[page] || dough_list.Count >= num_level * 2)
        {   // �ִ� ���� �� ����
            SoundManager.instance.PlaySound("Fail");
            return; 
        }
        gold -= dough_goldlist[page];

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Dough dough = obj.GetComponent<Dough>();
        obj.name = "Dough " + page;
        dough.id = page;
        dough.sprite_renderer.sprite = dough_spritelist[page];

        dough_list.Add(dough);

        SoundManager.instance.PlaySound("Buy");
    }


    void LoadData()
    {
        lock_group.gameObject.SetActive(!dough_unlock_list[page]);

        for (int i = 0; i < dough_data_list.Count; ++i)
        {
            GameObject obj = Instantiate(prefab, dough_data_list[i].pos, Quaternion.identity);
            Dough dough = obj.GetComponent<Dough>();
            dough.id = dough_data_list[i].id;
            dough.level = dough_data_list[i].level;
            dough.exp = dough_data_list[i].exp;
            dough.sprite_renderer.sprite = dough_spritelist[dough.id];
            dough.anim.runtimeAnimatorController = level_ac[dough.level - 1];
            obj.name = "Dough " + dough.id;

            dough_list.Add(dough);

            num_sub_text.text = "���� ���뷮 " + num_level * 2;
            if (num_level >= 5) num_btn.gameObject.SetActive(false);
            else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);

            click_sub_text.text = "Ŭ�� ���귮 X " + click_level;
            if (click_level >= 5) click_btn.gameObject.SetActive(false);
            else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]);
        }

    }


    public void Exit()
    {
        data_manager.JsonSave();
        SoundManager.instance.PlaySound("Pause Out");
        Application.Quit();
    }

    // ���׷��̵� ��ư�� ������ ��� ȣ��
    public void NumUpgrade()
    {
        if (gold < num_gold_list[num_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= num_gold_list[num_level++];

        num_sub_text.text = "���� ���뷮 " + num_level * 2;

        if (num_level >= 5) num_btn.gameObject.SetActive(false);
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);

        SoundManager.instance.PlaySound("Unlock");
    }

    public void ClickUpgrade()
    {
        if (gold < click_gold_list[click_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= click_gold_list[click_level++];

        click_sub_text.text = "Ŭ�� ���귮 X " + click_level;

        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]);

        SoundManager.instance.PlaySound("Unlock");
    }

    // to do list �� ��� �Ϸ����� ��, ����(���)�� �ִ� �ý��� - �Ϸ翡 �ѹ��� ���� �� �ְ�
    public void GetReward()
    {
        //�Ϸ簡 ������(���������� ������ ���� ��¥�� ������ ��¥�� �ٸ���) ������ ���� �� �ִ� ��ȸ ����
        if (getDay != DateTime.Now.ToString(("dd")))
        {
            possibleGet = true;
            Debug.Log("������� �߱޵Ǿ����ϴ�.");
        }

        GameObject check1 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem/checkBox");
        Toggle chk1 = check1.GetComponent<Toggle>();

        GameObject check2 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem2/checkBox");
        Toggle chk2 = check2.GetComponent<Toggle>();

        GameObject check3 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem3/checkBox");
        Toggle chk3 = check3.GetComponent<Toggle>();

        GameObject check4 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem4/checkBox");
        Toggle chk4 = check4.GetComponent<Toggle>();

        GameObject check5 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem5/checkBox");
        Toggle chk5 = check5.GetComponent<Toggle>();

        //���� ��ư Ŭ���� ��� to do list�� �Ϸ� �����̰�,
        if (chk1.isOn && chk2.isOn && chk3.isOn && chk4.isOn && chk5.isOn)
        {
            if (possibleGet)
            {
                RewardClick();
                gold += 500; // ������� �ִٸ�, ��带 ȹ��
                Debug.Log("������ ���޵Ǿ����ϴ�.");

                getDay = DateTime.Now.ToString(("dd")); //������ ���� ��¥ ����

                possibleGet = false; //����� ���ó��

                SoundManager.instance.PlaySound("Clear");
            }
            else
            {
                RewardClickNo1();
                // to do list�� ��� �Ϸ�� ���¿��� ������� ���ٸ� ������ ���� ����
                Debug.Log("������ ������ �̹� �޾ҽ��ϴ�. ���� �ٽ� ������ �ּ���!");

                SoundManager.instance.PlaySound("Fail");
            }
           
        }
        else
        {
            RewardClickNo2();
            //to do list �� �Ϸ���� ���� ���¸� ������ ���� ����
            Debug.Log("����Ʈ�� �Ϸ��ؾ� ������ ���� �� �ֽ��ϴ�.");

            SoundManager.instance.PlaySound("Fail");
        }
    }
    
}