using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

}


public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        JsonLoad();
    }

    public void JsonLoad()
    {
        SaveData save_data = new SaveData();

        if (!File.Exists(path))
        {
            GameManager.instance.flour = 0;
            GameManager.instance.gold = 0;

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

        string json = JsonUtility.ToJson(save_data, true);

        File.WriteAllText(path, json);
    }

}
