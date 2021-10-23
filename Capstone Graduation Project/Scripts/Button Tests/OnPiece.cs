using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPiece : MonoBehaviour
{
    List<int> moves;
    public GameObject Green;
    int id;
    ShowMoves sm;
    Mono mono;
    Tutorial tutorial;
    PVP pvp;
    UnityPiece up;
    GameObject canvas;
    bool final = false;
    bool pvpBool = false;

    private void Start()
    {
        sm = FindObjectOfType<ShowMoves>();
        up = GetComponent<UnityPiece>();
        if (SceneManager.GetActiveScene().name == "Final")
        {
            mono = FindObjectOfType<Mono>();
            final = true;
            id = mono.getId(up.I, up.J);
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorial = FindObjectOfType<Tutorial>();
            final = false;
            id = tutorial.GetId(up.I, up.J);
        }
        else
        {
            pvp = FindObjectOfType<PVP>();
            id = pvp.GetId(up.I, up.J);
            pvpBool = true;
        }
        canvas = GameObject.Find("Canvas");
    }

    public void Moves()
    {
        if(final)
        {
            if (!mono.playerCanMove)
                return;
        }
        else if(pvpBool)
        {
            if (pvp.promotionInProgress)
                return;
        }

        if (!sm.IsPressed)
        {
            if(final)
            {
                moves = mono.getMoves(up.I, up.J);
            }
            else if(pvpBool)
            {
                moves = pvp.GetMoves(up.I, up.J);
            }
            else
            {
                moves = tutorial.GetMoves(up.I, up.J);
            }

            for (int i = 0; i < moves.Count; i += 2)
            {
                GameObject green = Instantiate(Green);
                green.transform.SetParent(canvas.transform);
                green.GetComponent<RectTransform>().anchoredPosition = new Vector2(40 * moves[i + 1], 40 * moves[i]);
                green.GetComponent<RectTransform>().localScale = Vector2.one;
                green.GetComponent<GreenButton>().SetI(moves[i]);
                green.GetComponent<GreenButton>().SetJ(moves[i + 1]);
            }
            sm.IsPressed = true;
            sm.PressedId = id;
            sm.TileToMove = gameObject;
        }
        else
        {
            if(sm.PressedId == id)
            {
                GameObject[] greens = GameObject.FindGameObjectsWithTag("Green");
                foreach (GameObject go in greens)
                {
                    Destroy(go);
                }
                sm.IsPressed = false;
            }
            else
            {
                GameObject[] greens = GameObject.FindGameObjectsWithTag("Green");
                foreach (GameObject go in greens)
                {
                    Destroy(go);
                }

                if (final)
                {
                    moves = mono.getMoves(up.I, up.J);
                }
                else if (pvpBool)
                {
                    moves = pvp.GetMoves(up.I, up.J);
                }
                else
                {
                    moves = tutorial.GetMoves(up.I, up.J);
                }

                for (int i = 0; i < moves.Count; i += 2)
                {
                    GameObject green = Instantiate(Green);
                    green.transform.SetParent(canvas.transform);
                    green.GetComponent<RectTransform>().anchoredPosition = new Vector2(40 * moves[i + 1], 40 * moves[i]);
                    green.GetComponent<RectTransform>().localScale = Vector2.one;
                    green.GetComponent<GreenButton>().SetI(moves[i]);
                    green.GetComponent<GreenButton>().SetJ(moves[i + 1]);
                }
                sm.IsPressed = true;
                sm.PressedId = id;
                sm.TileToMove = gameObject;
            }
        }
    }

}
