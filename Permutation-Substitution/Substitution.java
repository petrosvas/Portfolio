package itc4322;

import java.util.ArrayList;

public class Substitution {
    
    static char[] aTom = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm'};
    static char[] nToz = {'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
    static char[] zeroTo4 = {'0', '1', '2', '3', '4'};
    static char[] fiveTo9 = {'5', '6', '7', '8', '9'};
    static int increase = 0;
    
    public static String SubstitutionEncryption(String originalText, String key)
    {
        ArrayList<Character> chars = new ArrayList<>();
        int tmp = 0;
        for(char c : key.toCharArray())
        {
            tmp += (int)c;
        }
        increase = (int) ((tmp / key.length()) - (0.1 * key.toCharArray()[0]));
        char[] text = originalText.toCharArray();
        for(char c : text)
        {
            if(c >= 97 && c <= 122) // Lower case letter
            {
                if(c <= 109) // <= m
                {
                    for(int i=0; i<aTom.length; i++)
                    {
                        if(c == aTom[i])
                        {
                            chars.add((char) ((int)nToz[i] + increase));
                        }
                    }
                }
                else
                {
                    for(int i=0; i<nToz.length; i++)
                    {
                        if(c == nToz[i])
                        {
                            chars.add((char) ((int)aTom[i] + increase));
                        }
                    }
                }
            }
            else if(c >= 65 && c <= 90) // Uper case letter
            {
                if(c <= 77) // <= M
                {
                    for(int i=0; i<aTom.length; i++)
                    {
                        if(Character.toLowerCase(c) == aTom[i])
                        {
                            chars.add((char) ((int)Character.toUpperCase(nToz[i]) + increase));
                        }
                    }
                }
                else
                {
                    for(int i=0; i<nToz.length; i++)
                    {
                        if(Character.toLowerCase(c) == nToz[i])
                        {
                            chars.add((char) ((int)Character.toUpperCase(aTom[i]) + increase));
                        }
                    }
                }
            }
            else if(c >= 48 && c <= 57) // Number
            {
                if(c <= 52) // <= 4
                {
                    for(int i=0; i<zeroTo4.length; i++)
                    {
                        if(c == zeroTo4[i])
                        {
                            chars.add((char) ((int)fiveTo9[i] + increase));
                        }
                    }
                }
                else
                {
                    for(int i=0; i<zeroTo4.length; i++)
                    {
                        if(c == fiveTo9[i])
                        {
                            chars.add((char) ((int)zeroTo4[i] + increase));
                        }
                    }
                }
            }
            else if(c == ' ')
            {
                chars.add((char) ((int)' ' + increase));
            }
            else
            {
                chars.add((char) ((int)c + increase));
            }
        }
        
        String result = "";
        for(char c : chars)
        {
            result += c;
        }
        return result;
    }
    
    public static String SubstitutionDecryption(String originalText, String key)
    {
        ArrayList<Character> chars = new ArrayList<>();
        int tmp = 0;
        for(char c : key.toCharArray())
        {
            tmp += (int)c;
        }
        increase = (int) ((tmp / key.length()) - (0.1 * key.toCharArray()[0]));
        char[] text = originalText.toCharArray();
        for(char c : text)
        {
            c-=increase;
            if(c >= 97 && c <= 122) // Lower case letter
            {
                if(c <= 109) // <= m
                {
                    for(int i=0; i<aTom.length; i++)
                    {
                        if(c == aTom[i])
                        {
                            chars.add(nToz[i]);
                        }
                    }
                }
                else
                {
                    for(int i=0; i<nToz.length; i++)
                    {
                        if(c == nToz[i])
                        {
                            chars.add(aTom[i]);
                        }
                    }
                }
            }
            else if(c >= 65 && c <= 90) // Uper case letter
            {
                if(c <= 77) // <= m
                {
                    for(int i=0; i<aTom.length; i++)
                    {
                        if(Character.toLowerCase(c) == aTom[i])
                        {
                            chars.add(Character.toUpperCase(nToz[i]));
                        }
                    }
                }
                else
                {
                    for(int i=0; i<nToz.length; i++)
                    {
                        if(Character.toLowerCase(c) == nToz[i])
                        {
                            chars.add(Character.toUpperCase(aTom[i]));
                        }
                    }
                }
            }
            else if(c >= 48 && c <= 57) // Number
            {
                if(c <= 52) // <= 4
                {
                    for(int i=0; i<zeroTo4.length; i++)
                    {
                        if(c == zeroTo4[i])
                        {
                            chars.add(fiveTo9[i]);
                        }
                    }
                }
                else
                {
                    for(int i=0; i<zeroTo4.length; i++)
                    {
                        if(c == fiveTo9[i])
                        {
                            chars.add(zeroTo4[i]);
                        }
                    }
                }
            }
            else if(c == ' ')
            {
                chars.add(' ');
            }
            else
            {
                chars.add((char)(c));
            }
        }
        
        String result = "";
        for(char c : chars)
        {
            result += c;
        }
        return result;
    }
}
