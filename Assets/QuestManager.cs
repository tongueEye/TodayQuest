using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager: MonoBehaviour
{
    public int id;
    public string task;
    public bool done;

    public InputField inputText;
    public InputField inputText2;
    public InputField inputText3;
    public InputField inputText4;
    public InputField inputText5;

    public static string txt1 = "";
    public static string txt2 = "";
    public static string txt3 = "";
    public static string txt4 = "";
    public static string txt5 = "";

    public Toggle checkBox1;
    public Toggle checkBox2;
    public Toggle checkBox3;
    public Toggle checkBox4;
    public Toggle checkBox5;

    public static bool check1 = false;
    public static bool check2 = false;
    public static bool check3 = false;
    public static bool check4 = false;
    public static bool check5 = false;


    public void setTask(Text task)
    {
        task.text = inputText.text;
        txt1 = inputText.text;
    }

    public void setTask2(Text task)
    {
        task.text = inputText2.text;
        txt2 = inputText2.text;
    }

    public void setTask3(Text task)
    {
        task.text = inputText3.text;
        txt3 = inputText3.text;
    }

    public void setTask4(Text task)
    {
        task.text = inputText4.text;
        txt4 = inputText4.text;
    }

    public void setTask5(Text task)
    {
        task.text = inputText5.text;
        txt5 = inputText5.text;
    }

    public void setDone1(Toggle checkBox)
    {
        checkBox.isOn = checkBox1.isOn;
        check1 = checkBox1.isOn;
        SoundManager.instance.PlaySound("Button");
    }

    public void setDone2(Toggle checkBox)
    {
        checkBox.isOn = checkBox2.isOn;
        check2 = checkBox2.isOn;
        SoundManager.instance.PlaySound("Button");
    }

    public void setDone3(Toggle checkBox)
    {
        checkBox.isOn = checkBox3.isOn;
        check3 = checkBox3.isOn;
        SoundManager.instance.PlaySound("Button");
    }

    public void setDone4(Toggle checkBox)
    {
        checkBox.isOn = checkBox4.isOn;
        check4 = checkBox4.isOn;
        SoundManager.instance.PlaySound("Button");
    }

    public void setDone5(Toggle checkBox)
    {
        checkBox.isOn = checkBox5.isOn;
        check5 = checkBox5.isOn;
        SoundManager.instance.PlaySound("Button");
    }


}
