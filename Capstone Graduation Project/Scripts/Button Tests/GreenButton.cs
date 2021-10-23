using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreenButton : MonoBehaviour
{
    public int i, j;

    public void SetI(int value)
    { i = value; }

    public void SetJ(int value)
    { j = value; }
    ShowMoves sh;
    Mono mono;
    Tutorial tutorial;
    PVP pvp;
    bool final = false;
    bool pvpBool = false;

    private void Start()
    {
        sh = FindObjectOfType<ShowMoves>();
        if(SceneManager.GetActiveScene().name == "Final")
        {
            mono = FindObjectOfType<Mono>();
            final = true;
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorial = FindObjectOfType<Tutorial>();
            final = false;
        }
        else
        {
            pvp = FindObjectOfType<PVP>();
            pvpBool = true;
        }
    }

    public void Move()
    {
        GameObject toMove = sh.TileToMove;

        if(final)
            mono.Move(toMove.GetComponent<UnityPiece>().I, toMove.GetComponent<UnityPiece>().J, i, j);
        else
        {
            if(pvpBool)
            {
                pvp.Move(toMove.GetComponent<UnityPiece>().I, toMove.GetComponent<UnityPiece>().J, i, j);
            }
            else
            {
                tutorial.Move(toMove.GetComponent<UnityPiece>().I, toMove.GetComponent<UnityPiece>().J, i, j);
            }
        }

        GameObject[] greens = GameObject.FindGameObjectsWithTag("Green");
        foreach (GameObject go in greens)
        {
            Destroy(go);
        }
        sh.IsPressed = false;
    }

}
