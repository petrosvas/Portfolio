using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diffs : MonoBehaviour
{
    int difficultySelected;

    // Start is called before the first frame update
    void Start()
    {
        Diffs[] toDelete = GameObject.FindObjectsOfType<Diffs>();

        foreach(Diffs d in toDelete)
        {
            if(d.gameObject != this.gameObject)
            {
                Destroy(d.transform.root.gameObject);
            }
        }

        DontDestroyOnLoad(this);
    }

    public void SetDif(int dif)
    {
        difficultySelected = dif;
    }

    public int GetDif()
    {
        return difficultySelected;
    }
}

public enum Difficulties : int
{
    Easy, Medium, Hard
}