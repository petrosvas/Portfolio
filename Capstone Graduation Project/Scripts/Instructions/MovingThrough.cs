using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThrough : MonoBehaviour
{
    Canvas[] instrs;
    Music m;

    private void Start()
    {
        instrs = new Canvas[3];
        instrs[0] = GameObject.Find("Instructions").GetComponent<Canvas>();
        instrs[1] = GameObject.Find("Instructions 2").GetComponent<Canvas>();
        instrs[2] = GameObject.Find("Instructions 3").GetComponent<Canvas>();
        m = FindObjectOfType<Music>();
    }

    public void ChangeInstruction(int canvas)
    {
        m.PlayMenuClick();
        foreach(Canvas c in instrs)
        {
            c.enabled = false;
        }
        instrs[canvas].enabled = true;
    }

    public void Back()
    {
        m.PlayMenuClick();
        foreach (Canvas c in instrs)
        {
            c.enabled = false;
        }
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
    }

}
