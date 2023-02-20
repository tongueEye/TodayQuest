using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int flour;
    public int gold;

    public List<Dough> dough_list = new List<Dough>(); // 반죽을 사고 팜에 따라 현재 생성되어 있는 반죽을 저장하고 관리하기 위한 리스트
    public List<Data> dough_data_list = new List<Data>();

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
    public Image option_panel;

    public GameObject prefab;

    public GameObject data_manager_obj;

    DataManager data_manager;

    Animator dough_anim;
    Animator plant_anim;

    bool isDoughClick;
    bool isPlantClick;
    bool isOption;

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

    void Awake()
    {
        instance = this;

        dough_anim = dough_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();

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
        //runtimeAnimatorController를 통해 해당젤리의 레벨에 따라 Animator를 변경
        anim.runtimeAnimatorController = level_ac[level - 1];
    }


    public void GetGold(int id, int level, Dough dough)
    {
        gold += dough_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;

        dough_list.Remove(dough);
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


    //Esc 버튼이 눌리게 될 경우 3가지 경로로 나뉘게 되며 만약 이미 띄워져 있는 UI 창이 있다면 Option Panel을 활성화 시키는 대신 이미 띄워져 있는 창을 내림
    void Option()
    {
        isOption = !isOption;
        isLive = !isLive;

        option_panel.gameObject.SetActive(isOption);
        Time.timeScale = isOption == true ? 0 : 1;
    }
 

    //버튼이 클릭될 시 호출되며 page 변수의 값을 증가시키고, ChangePage() 함수를 호출
    public void PageUp()
    {
        if (page >= dough_ea-1) return;

        ++page;
        ChangePage();
    }

    //버튼이 클릭될 시 호출되며 page 변수의 값을 감소시키고, ChangePage() 함수를 호출
    public void PageDown()
    {
        if (page <= 0) return;

        --page;
        ChangePage();
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


    // 구입 버튼 클릭 시 호출되며, 현재 소지하고 있는 flour의 값에 따라 젤리를 해금시킬 수 있음.
    public void Unlock()
    {
        if (flour < dough_flourlist[page]) return;

        dough_unlock_list[page] = true;
        ChangePage();

        flour -= dough_flourlist[page];
    }


    // 반죽(빵) 구매 기능
    public void BuyDough()
    {
        if (gold < dough_goldlist[page] || dough_list.Count>=num_level*2) return; // 최대 젤리 수 제한

        gold -= dough_goldlist[page];

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Dough dough = obj.GetComponent<Dough>();
        obj.name = "Dough " + page;
        dough.id = page;
        dough.sprite_renderer.sprite = dough_spritelist[page];

        dough_list.Add(dough);
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


    void OnApplicationQuit()
    {
        data_manager.JsonSave();
    }

    // 업그레이드 버튼이 눌렸을 경우 호출
    public void NumUpgrade()
    {
        if (gold < num_gold_list[num_level]) return;

        gold -= num_gold_list[num_level++];

        num_sub_text.text = "반죽 수용량 " + num_level * 2;

        if (num_level >= 5) num_btn.gameObject.SetActive(false);
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);
    }

    public void ClickUpgrade()
    {
        if (gold < click_gold_list[click_level]) return;

        gold -= click_gold_list[click_level++];

        click_sub_text.text = "클릭 생산량 X " + click_level;

        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]);
    }

}