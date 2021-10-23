package itc4322;

import java.util.ArrayList;
import java.util.Arrays;

public class Permutation {

static public String PermutationEncryption(String text, String key)
{
    int cols = key.length();
    int rows;
    
    if(text.length() % cols == 0)
    {
        rows = text.length() / cols;
    }
    else
    {
        rows = (text.length() / cols) + 1;
    }
    
    int minIndex;
    int minChar;
    
    ArrayList<Character> list = new ArrayList<>();
    int[] order = new int[key.length()];
    
    for(char c : key.toCharArray())
    {
        list.add(c);
    }
    
    for(int i=0; i<key.length(); i++)
    {
        minIndex = 0;
        minChar = Integer.MAX_VALUE;
        for(int j=0; j<list.size(); j++)
        {
            if((int)list.get(j) < minChar)
            {
                minChar = list.get(j);
                minIndex = j;
            }
        }
        order[i] = minIndex;
        list.set(minIndex, Character.MAX_VALUE);
    }
    
    char[][] charArray = new char[rows][cols];
    char[] txt = text.toCharArray();
    
    int index = 0;
    for(int i=0; i<rows; i++)
    {
        for(int j=0; j< cols; j++)
        {
            if(index < txt.length)
            {
                charArray[i][j] = txt[index];
                index++;
            }
            else
            {
                charArray[i][j] = 7;
            }
        }
    }
    
    char[][] result = new char[rows][cols];
    
    for(int j=0; j<key.length(); j++)
    {
        for(int i=0; i < rows; i++)
        {
            result[i][j] = charArray[i][order[j]];
        }
    }
    
    char[] toReturn = new char[rows * cols];
    int charArrayIndex = 0;
    
    for(int i=0; i<rows; i++)
    {
        for(int j=0; j<cols; j++)
        {
            toReturn[charArrayIndex] = result[i][j];
            charArrayIndex++;
        }
    }
    
    return new String(toReturn);
}
    
static public String PermutationDecryption(String text, String key)
{
    int cols = key.length();
    int rows;
    
    rows = text.length() / cols;
    
    int minIndex;
    int minChar;
    
    ArrayList<Character> list = new ArrayList<>();
    int[] order = new int[key.length()];
    
    for(char c : key.toCharArray())
    {
        list.add(c);
    }
    
    for(int i=0; i<key.length(); i++)
    {
        minIndex = 0;
        minChar = Integer.MAX_VALUE;
        for(int j=0; j<list.size(); j++)
        {
            if((int)list.get(j) < minChar)
            {
                minChar = list.get(j);
                minIndex = j;
            }
        }
        order[i] = minIndex;
        list.set(minIndex, Character.MAX_VALUE);
    }
    
    char[][] charArray = new char[rows][cols];
    char[] txt = text.toCharArray();
    
    int index = 0;
    for(int i=0; i<rows; i++)
    {
        for(int j=0; j< cols; j++)
        {
            charArray[i][j] = txt[index];
            index++;
        }
    }
    
    char[][] result = new char[rows][cols];
    int orderIndex;
    
    for(int j=0; j<key.length(); j++)
    {
        orderIndex = 0;
        for(int in=0; in<order.length; in++)
        {
            if(order[in] == j)
            {
                orderIndex = in;
            }
        }
        
        for(int i=0; i < rows; i++)
        {
            result[i][j] = charArray[i][orderIndex];
        }
    }
    
    char[] toReturn = new char[rows * cols];
    int charArrayIndex = 0;
    
    for(int i=0; i<rows; i++)
    {
        for(int j=0; j<cols; j++)
        {
            toReturn[charArrayIndex] = result[i][j];
            charArrayIndex++;
        }
    }
    
    String ret = new String();
    for(int i=0; i<toReturn.length; i++)
    {
        if(toReturn[i] == 7)
            break;
        ret += toReturn[i];
    }
    
    return ret;
}

}
