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
        flour_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(flour_text.text), flour, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }


    //GameManager 내에 Animator 변경을 관리하기 위한 Animator 배열과 함수 추가
    public RuntimeAnimatorController[] level_ac;
    //반죽의 가격을 저장하기 위한 배열 변수
    public int[] dough_goldlist;

    public Image dough_panel;
    public Image plant_panel;
    public Image option_panel;

    Animator dough_anim;
    Animator plant_anim;


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

    public bool isLive; // 게임의 활성화/비활성화 상태를 구분하기 위한 변수

    // Lock Group 오브젝트를 컨트롤하기 위한 변수
    public GameObject lock_group;
    public Image lock_group_dough_img;
    public Text lock_group_flour_text;

    bool[] dough_unlock_list; //반죽의 해금 상태를 확인하기 위한 배열

    public List<Dough> dough_list = new List<Dough>(); // 반죽을 사고 팜에 따라 현재 생성되어 있는 반죽을 저장하고 관리하기 위한 리스트

    void Awake()
    {
        isSell = false;

        dough_anim = dough_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();

        isLive = true;

        flour_text.text = flour.ToString();
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = dough_goldlist[0].ToString();
        lock_group_flour_text.text = dough_flourlist[0].ToString();

        dough_unlock_list = new bool[14];

        for (int i = 0; i < 14; ++i)
            if (PlayerPrefs.HasKey(i.ToString()))
                dough_unlock_list[i] = true;
        lock_group.gameObject.SetActive(!dough_unlock_list[page]);

        //PlayerPrefs.DeleteAll(); //save 초기화 코드 (테스트 용)

    }

    public void CheckSell()
    {
        isSell = isSell == false;
    }

    public int max_gold;

    public void GetGold(int id, int level, Dough dough)
    {
        gold += dough_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;

        dough_list.Remove(dough);
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

    bool isOption;

    //Esc 버튼이 눌리게 될 경우 3가지 경로로 나뉘게 되며 만약 이미 띄워져 있는 UI 창이 있다면 Option Panel을 활성화 시키는 대신 이미 띄워져 있는 창을 내림
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

    //반죽의 Sprite, 이름, 가격을 저장하기 위한 배열 변수
    public Sprite[] dough_spritelist;
    public string[] dough_namelist;
    public int[] dough_flourlist;


    //버튼 클릭에 따른 페이지 이동
    public Text page_text;
    public Image unlock_group_dough_img;
    public Text unlock_group_gold_text;
    public Text unlock_group_name_text;

    int page;

    //버튼이 클릭될 시 호출되며 page 변수의 값을 증가시키고, ChangePage() 함수를 호출
    public void PageUp()
    {
        if (page >= 13) return;

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

        PlayerPrefs.SetInt(page.ToString(), 1);
    }

    // 반죽(빵) 구매 기능
    public GameObject prefab;

    public void BuyDough()
    {
        if (gold < dough_goldlist[page]) return;

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Dough dough = obj.GetComponent<Dough>();
        obj.name = "Dough " + page;
        dough.id = page;
        dough.sprite_renderer.sprite = dough_spritelist[page];

        gold -= dough_goldlist[page];

        dough_list.Add(dough);
    }

}
