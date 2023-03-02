using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public int flour;
    public int gold;

    public bool[] dough_unlock_list = new bool[14]; //반죽의 개수 만큼 생성
    public List<Data> dough_list = new List<Data>();

    public int num_level;
    public int click_level;

    public float bgm_vol;
    public float sfx_vol;

    public string toDoList1;
    public string toDoList2;
    public string toDoList3;
    public string toDoList4;
    public string toDoList5;

    public bool done1;
    public bool done2;
    public bool done3;
    public bool done4;
    public bool done5;

    public bool possibleRwd;
    public string getRwdDay=" ";

}


public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
        JsonLoad();
    }

    public void JsonLoad()
    {
        SaveData save_data = new SaveData();

        if (!File.Exists(path))
        {
            GameManager.instance.flour = 11;
            GameManager.instance.gold = 51;

            GameManager.instance.num_level = 1;
            GameManager.instance.click_level = 1;

            JsonSave();
        }
        else
        {
            string load_json = File.ReadAllText(path);
            save_data = JsonUtility.FromJson<SaveData>(load_json);

            if (save_data != null)
            {
                for (int i = 0; i < save_data.dough_list.Count; ++i)
                    GameManager.instance.dough_data_list.Add(save_data.dough_list[i]);
                for (int i = 0; i < save_data.dough_unlock_list.Length; ++i)
                    GameManager.instance.dough_unlock_list[i] = save_data.dough_unlock_list[i];

                GameManager.instance.flour = save_data.flour;
                GameManager.instance.gold = save_data.gold;

                GameManager.instance.num_level = save_data.num_level;
                GameManager.instance.click_level = save_data.click_level;

                SoundManager.instance.bgm_slider.value = save_data.bgm_vol;
                SoundManager.instance.sfx_slider.value = save_data.sfx_vol;
                
                GameObject toDoText1 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem/Text");
                Text toDo1 = toDoText1.GetComponent<Text>();
                toDo1.text = save_data.toDoList1;

                GameObject toDoEditText1 = GameObject.Find("Quest Edit Panel/scrollView/edit_contents/UIListEditItem/InputText");
                InputField toDoEdit1 = toDoEditText1.GetComponent<InputField>();
                toDoEdit1.text = save_data.toDoList1;

                GameObject toDoText2 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem2/Text2");
                Text toDo2 = toDoText2.GetComponent<Text>();
                toDo2.text = save_data.toDoList2;

                GameObject toDoEditText2 = GameObject.Find("Quest Edit Panel/scrollView/edit_contents/UIListEditItem2/InputText2");
                InputField toDoEdit2 = toDoEditText2.GetComponent<InputField>();
                toDoEdit2.text = save_data.toDoList2;

                GameObject toDoText3 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem3/Text3");
                Text toDo3 = toDoText3.GetComponent<Text>();
                toDo3.text = save_data.toDoList3;

                GameObject toDoEditText3 = GameObject.Find("Quest Edit Panel/scrollView/edit_contents/UIListEditItem3/InputText3");
                InputField toDoEdit3 = toDoEditText3.GetComponent<InputField>();
                toDoEdit3.text = save_data.toDoList3;

                GameObject toDoText4 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem4/Text4");
                Text toDo4 = toDoText4.GetComponent<Text>();
                toDo4.text = save_data.toDoList4;

                GameObject toDoEditText4 = GameObject.Find("Quest Edit Panel/scrollView/edit_contents/UIListEditItem4/InputText4");
                InputField toDoEdit4 = toDoEditText4.GetComponent<InputField>();
                toDoEdit4.text = save_data.toDoList4;

                GameObject toDoText5 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem5/Text5");
                Text toDo5 = toDoText5.GetComponent<Text>();
                toDo5.text = save_data.toDoList5;

                GameObject toDoEditText5 = GameObject.Find("Quest Edit Panel/scrollView/edit_contents/UIListEditItem5/InputText5");
                InputField toDoEdit5 = toDoEditText5.GetComponent<InputField>();
                toDoEdit5.text = save_data.toDoList5;

                GameObject check1 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem/checkBox");
                Toggle chk1 = check1.GetComponent<Toggle>();
                chk1.isOn = save_data.done1;

                GameObject check2 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem2/checkBox");
                Toggle chk2 = check2.GetComponent<Toggle>();
                chk2.isOn = save_data.done2;

                GameObject check3 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem3/checkBox");
                Toggle chk3 = check3.GetComponent<Toggle>();
                chk3.isOn = save_data.done3;

                GameObject check4 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem4/checkBox");
                Toggle chk4 = check4.GetComponent<Toggle>();
                chk4.isOn = save_data.done4;

                GameObject check5 = GameObject.Find("Quest Panel/scrollView/list_contents/UIListItem5/checkBox");
                Toggle chk5 = check5.GetComponent<Toggle>();
                chk5.isOn = save_data.done5;

                GameManager.instance.possibleGet = save_data.possibleRwd;
                GameManager.instance.getDay = save_data.getRwdDay;

            }
        }
    }

    public void JsonSave()
    {
        SaveData save_data = new SaveData();

        for (int i = 0; i < GameManager.instance.dough_list.Count; ++i)
        {
            Dough dough = GameManager.instance.dough_list[i];
            save_data.dough_list.Add(new Data(dough.gameObject.transform.position, dough.id, dough.level, dough.exp));
        }
        for (int i = 0; i < GameManager.instance.dough_unlock_list.Length; ++i)
            save_data.dough_unlock_list[i] = GameManager.instance.dough_unlock_list[i];

        save_data.flour = GameManager.instance.flour;
        save_data.gold = GameManager.instance.gold;

        save_data.num_level = GameManager.instance.num_level;
        save_data.click_level = GameManager.instance.click_level;

        save_data.bgm_vol = SoundManager.instance.bgm_slider.value;
        save_data.sfx_vol = SoundManager.instance.sfx_slider.value;

        save_data.toDoList1 = QuestManager.txt1;
        save_data.toDoList2 = QuestManager.txt2;
        save_data.toDoList3 = QuestManager.txt3;
        save_data.toDoList4 = QuestManager.txt4;
        save_data.toDoList5 = QuestManager.txt5;

        save_data.done1 = QuestManager.check1;
        save_data.done2 = QuestManager.check2;
        save_data.done3 = QuestManager.check3;
        save_data.done4 = QuestManager.check4;
        save_data.done5 = QuestManager.check5;

        save_data.possibleRwd = GameManager.instance.possibleGet;
        save_data.getRwdDay = GameManager.instance.getDay;

        string json = JsonUtility.ToJson(save_data, true);

        File.WriteAllText(path, json);
    }

}
