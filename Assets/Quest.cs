using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public int index;
    public string task;
    public bool done;

    public Quest(int index, string task, bool done)
    {
        this.index = index;
        this.task = task;
        this.done = done;
    }
}
