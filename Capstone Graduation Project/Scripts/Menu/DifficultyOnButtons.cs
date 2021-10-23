using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyOnButtons : MonoBehaviour
{
    int diff;
    Music m;

    private void Start()
    {
        m = FindObjectOfType<Music>();
        if (gameObject.name == "Easy")
        {
            diff = (int)Difficulties.Easy;
        }
        else if(gameObject.name == "Medium")
        {
            diff = (int)Difficulties.Medium;
        }
        else if(gameObject.name == "Hard")
        {
            diff = (int)Difficulties.Hard;
        }

        GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(Action));
    }

    public void Action()
    {
        m.PlayMenuClick();
        FindObjectOfType<Diffs>().SetDif(diff);
        SceneManager.LoadScene("Final", LoadSceneMode.Single);
    }

    public int GetDifFromButton()
    {
        return diff;
    }
}
