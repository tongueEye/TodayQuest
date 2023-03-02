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

    public List<Dough> dough_list = new List<Dough>(); // 반죽을 사고 팜에 따라 현재 생성되어 있는 반죽을 저장하고 관리하기 위한 리스트
    public List<Data> dough_data_list = new List<Data>();
    //public List<Quest> quest_data_list = new List<Quest>(); // To-Do List(퀘스트) 데이터를 저장하고 관리하기 위한 리스트

    public bool[] dough_unlock_list; // 반죽의 해금 상태를 확인하기 위한 배열

    public int max_flour;
    public int max_gold;

    public bool isSell;
    public bool isLive; // 게임의 활성화/비활성화 상태를 구분하기 위한 변수

    //반죽의 Sprite, 이름, 가격을 저장하기 위한 배열 변수
    public Sprite[] dough_spritelist;
    public string[] dough_namelist;
    public int[] dough_flourlist;
    public int[] dough_goldlist;

    //버튼 클릭에 따른 페이지 이동
    public Text page_text;
    public Image unlock_group_dough_img;
    public Text unlock_group_gold_text;
    public Text unlock_group_name_text;

    // Lock Group 오브젝트를 컨트롤하기 위한 변수
    public GameObject lock_group;
    public Image lock_group_dough_img;
    public Text lock_group_flour_text;

    //GameManager 내에 Animator 변경을 관리하기 위한 Animator 배열과 함수 추가
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

    //퀘스트 리스트 (스크롤 뷰)
    public GameObject list_contents;
    public GameObject edit_contents;
    public GameObject uiListItemPrefab;
    public GameObject uiListEditItemPrefab;

    // 보상권 관리 변수
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
        dough_ea = 14; //반죽 종류의 개수
        dough_unlock_list = new bool[dough_ea];

    }


    void Start()
    {
        //DataManager에 의해 데이터가 로드되기 전에 GameManager가 활성화 되어 빈 데이터를 참조하는 현상을 방지하기 위함
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
        //Dough 스크립트에서 Animator 객체와 level을 받아와
        //runtimeAnimatorController를 통해 해당반죽의 레벨에 따라 Animator를 변경
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
        flour += (id + 1) * level * click_level; //click_level에 따라 클릭으로 얻는 flour의 양을 달라지게 함

        if (flour > max_flour)
            flour = max_flour;
    }


    public void CheckSell()
    {
        isSell = isSell == false;
    }


    //각각 버튼이 클릭 될 시 발생하며 이전에 만들었던 창을 오르 내리는 역할을 수행
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


    //Esc 버튼이 눌리게 될 경우 3가지 경로로 나뉘게 되며 만약 이미 띄워져 있는 UI 창이 있다면 Option Panel을 활성화 시키는 대신 이미 띄워져 있는 창을 내림
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


    //버튼이 클릭될 시 호출되며 page 변수의 값을 증가시키고, ChangePage() 함수를 호출
    public void PageUp()
    {
        if (page >= dough_ea-1) return;

        ++page;
        ChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    //버튼이 클릭될 시 호출되며 page 변수의 값을 감소시키고, ChangePage() 함수를 호출
    public void PageDown()
    {
        if (page <= 0) return;

        --page;
        ChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    // 각 오브젝트의 Sprite 또는 Text를 변경하여 마치 다음 페이지로 넘어가는 것처럼 구현
    void ChangePage()
    {
        lock_group.gameObject.SetActive(!dough_unlock_list[page]);

        page_text.text = string.Format("#{0:00}", (page + 1));

        // 해금 상태에 따라 상황에 맞는 인터페이스를 보여줄 수 있도록 함
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

            unlock_group_dough_img.SetNativeSize(); //Sprite Image가 깨지는 현상 방지
        }
    }


    // 구입 버튼 클릭 시 호출되며, 현재 소지하고 있는 flour의 값에 따라 반죽을 해금시킬 수 있음.
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


    // 반죽(빵) 구매 기능
    public void BuyDough()
    {
        if (gold < dough_goldlist[page] || dough_list.Count >= num_level * 2)
        {   // 최대 반죽 수 제한
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

            num_sub_text.text = "반죽 수용량 " + num_level * 2;
            if (num_level >= 5) num_btn.gameObject.SetActive(false);
            else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);

            click_sub_text.text = "클릭 생산량 X " + click_level;
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

    // 업그레이드 버튼이 눌렸을 경우 호출
    public void NumUpgrade()
    {
        if (gold < num_gold_list[num_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= num_gold_list[num_level++];

        num_sub_text.text = "반죽 수용량 " + num_level * 2;

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

        click_sub_text.text = "클릭 생산량 X " + click_level;

        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]);

        SoundManager.instance.PlaySound("Unlock");
    }

    // to do list 를 모두 완료했을 때, 보상(골드)을 주는 시스템 - 하루에 한번만 받을 수 있게
    public void GetReward()
    {
        //하루가 지나면(마지막으로 보상을 받은 날짜와 지금의 날짜가 다르면) 보상을 받을 수 있는 기회 생성
        if (getDay != DateTime.Now.ToString(("dd")))
        {
            possibleGet = true;
            Debug.Log("보상권이 발급되었습니다.");
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

        //보상 버튼 클릭시 모든 to do list가 완료 상태이고,
        if (chk1.isOn && chk2.isOn && chk3.isOn && chk4.isOn && chk5.isOn)
        {
            if (possibleGet)
            {
                RewardClick();
                gold += 500; // 보상권이 있다면, 골드를 획득
                Debug.Log("보상이 지급되었습니다.");

                getDay = DateTime.Now.ToString(("dd")); //보상을 받은 날짜 저장

                possibleGet = false; //보상권 사용처리

                SoundManager.instance.PlaySound("Clear");
            }
            else
            {
                RewardClickNo1();
                // to do list가 모두 완료된 상태여도 보상권이 없다면 보상을 받지 못함
                Debug.Log("오늘의 보상을 이미 받았습니다. 내일 다시 도전해 주세요!");

                SoundManager.instance.PlaySound("Fail");
            }
           
        }
        else
        {
            RewardClickNo2();
            //to do list 가 완료되지 않은 상태면 보상을 받지 못함
            Debug.Log("퀘스트를 완료해야 보상을 받을 수 있습니다.");

            SoundManager.instance.PlaySound("Fail");
        }
    }
    
}