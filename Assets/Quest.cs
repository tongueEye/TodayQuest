using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    /*
    public string task;

    public Quest(string task)
    {
        this.task = task;
    }
    */

    
    public int index;
    public string task;
    //public bool done;

    public Quest(int index, string task)
    {
        this.index = index;
        this.task = task;
        //this.done = done;
    }
    
}
