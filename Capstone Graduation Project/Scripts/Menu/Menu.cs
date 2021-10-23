using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    Canvas diffs;
    Canvas menu;
    Canvas instr;
    Music m;

    private void Start()
    {
        diffs = GameObject.Find("Diffs").GetComponent<Canvas>();
        menu = GameObject.Find("Canvas").GetComponent<Canvas>();
        instr = GameObject.Find("Instructions").GetComponent<Canvas>();
        m = FindObjectOfType<Music>();
        m.PlayMenuTheme();
        diffs.enabled = false;
    }

    public void Instructions()
    {
        m.PlayMenuClick();
        menu.enabled = false;
        instr.enabled = true;
    }

    public void Tutorial()
    {
        // Tutorials uses Next Tutorial method on start, which plays the sound of the menu click
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    public void Play()
    {
        m.PlayMenuClick();
        menu.enabled = false;
        diffs.enabled = true;
    }

    public void PVP()
    {
        m.PlayMenuClick();
        SceneManager.LoadScene("PVP", LoadSceneMode.Single);
    }

    public void Return()
    {
        m.PlayMenuClick();
        menu.enabled = true;
        diffs.enabled = false;
    }

    public void Exit()
    {
        m.PlayMenuClick();
        Application.Quit();
    }
}
