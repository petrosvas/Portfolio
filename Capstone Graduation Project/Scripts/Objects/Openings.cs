using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class Openings
{
    static Ops opening;
    static MainCat main;
    static SubCat sub;
    static bool firstMove = true;

    public static void Initialize()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/Openings.json");
        opening = JsonConvert.DeserializeObject<Ops>(json);
    }

    public static int[] NextMove(int[] whiteMove)
    {
        if(firstMove)
        {
            foreach(MainCat m in opening.openings)
            {
                if(EqualArrays(m.white, whiteMove))
                {
                    main = m;
                    sub = m.black;
                    firstMove = false;
                    return sub.answer;
                }
            }
        }
        else
        {
            if (sub.next == null)
                return new int[] { -1 };
            foreach (MainCat m in sub.next)
            {
                if (EqualArrays(whiteMove, m.white))
                {
                    main = m;
                    sub = m.black;
                    return sub.answer;
                }
            }
        }

        return new int[] { -1 };
    }

    static bool EqualArrays(int[] a, int[] b)
    {
        if (a[0] == b[0] && a[1] == b[1] && a[2] == b[2] && a[3] == b[3])
            return true;
        else
            return false;
    }

    static string PrintIntArray(int[] array)
    {
        if (array == null)
            return "Empty int array";
        string str = "";
        foreach(int i in array)
        {
            str += i + ",";
        }
        return str;
    }

}

[System.Serializable]
public class MainCat
{
    public int[] white;
    public SubCat black;
}

[System.Serializable]
public class SubCat
{
    public int[] answer;
    public List<MainCat> next = null;
}

[System.Serializable]
public class Ops
{
    public List<MainCat> openings;
}