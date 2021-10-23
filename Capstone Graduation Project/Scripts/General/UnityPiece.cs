using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityPiece : MonoBehaviour
{
    public int I { get; set; }
    public int J { get; set; }
    public bool Movable { get; set; } = true;

    public void Move(int ori, int orj, int i, int j)
    {
        if(Movable)
        {
            /*
            GetComponent<RectTransform>().anchoredPosition += new Vector2((j - orj) * 30, (i - ori) * 30);
            I = i;
            J = j;
            */
            StartCoroutine(Moving(ori, orj, i, j));
        }
    }

    public IEnumerator Moving(int ori, int orj, int i, int j)
    {
        float time = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup - time < 0.22)
        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2((j - orj) * 40 * Time.deltaTime * 4, (i - ori) * 40 * Time.deltaTime * 4);
            yield return new WaitForEndOfFrame();
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(j * 40, i * 40);
        I = i;
        J = j;
    }
}
