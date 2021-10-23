using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Promote : MonoBehaviour
{
    public char type;
    Mono mono;
    Tutorial tutorial;
    PVP pvp;
    public int[] toReturn = new int[2];
    bool final = false;
    bool pvpBool = false;

    private void Start()
    {
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


        if (type == 'q')
        {
            toReturn[0] = 'q';
            toReturn[1] = (int)PiecePoints.queen;
        }
        else if (type == 'b')
        {
            toReturn[0] = 'b';
            toReturn[1] = (int)PiecePoints.bishop;
        }
        else if (type == 'n')
        {
            toReturn[0] = 'n';
            toReturn[1] = (int)PiecePoints.knight;
        }
        else if (type == 'r')
        {
            toReturn[0] = 'r';
            toReturn[1] = (int)PiecePoints.rook;
        }
    }

    public void Promotion()
    {
        if(final)
            mono.Promote(toReturn);
        else
        {
            if(pvpBool)
            {
                pvp.Promote(toReturn);
            }
            else
            {
                tutorial.Promote(toReturn);
            }
        }
    }
}
