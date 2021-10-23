package itc4322;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;
import java.util.TreeMap;

public class ITC4322 {
    
    // Testing TEXT =  "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 - abcdefghijklmnopqrstuvwxyz0123456789";
    private static String TEXT = "";
    private static String KEY = "";
    private static String SUBKEY1 = "";
    private static String SUBKEY2 = "";
    
    public static void main(String[] args) {
        
        
        Scanner scan = new Scanner(System.in);
        String key = "";
        boolean alpha = true;
        while(true)
        {
            alpha = true;
            System.out.println("Enter key! Key must be 16 alphanumeric characters only!");
            key = scan.nextLine();
            for(char c : key.toCharArray())
            {
                if(!Character.isLetterOrDigit(c))
                {
                    alpha = false;
                    break;
                }
            }
            if(key.length() < 16)
            {
                System.out.println("Key must be at least 16 characters!");
            }
            else if(!alpha)
            {
                System.out.println("Non-alphanumetic input inserted!");
            }
            else
            {
                KEY = key;
                break;
            }
        }
        
        GetSubkeys();
        
        try {
        File file = new File(System.getProperty("user.dir") + "\\Original Text.txt");
        Scanner myReader = new Scanner(file);
        while (myReader.hasNextLine()) {
          TEXT += myReader.nextLine();
        }
        myReader.close();
      } catch (FileNotFoundException e) {
        e.printStackTrace();
      }
        
        try 
        {
            FileWriter writer = new FileWriter(System.getProperty("user.dir") + "\\Encrypted Text.txt");
            FileWriter writer2 = new FileWriter(System.getProperty("user.dir") + "\\Decrypted Text.txt");
            
            String sub = Substitution.SubstitutionEncryption(TEXT, SUBKEY1);

            String per = Permutation.PermutationEncryption(sub, SUBKEY2);
            
            writer.write(per);

            String perdec = Permutation.PermutationDecryption(per, SUBKEY2);

            String dec = Substitution.SubstitutionDecryption(perdec, SUBKEY1);
            
            writer2.write(dec);
            
            writer.close();
            writer2.close();
          } catch (IOException e) {
            e.printStackTrace();
        }
        
    }
    
    public static void GetSubkeys()
    {
        int tmp = 0;
        
        for(char c : KEY.toCharArray())
        {
            tmp += (int)c;
        }
        int remainderInt = (int) ((tmp / KEY.length()) * 3);
        
        ArrayList<Character> sk1 = new ArrayList<>(), sk2 = new ArrayList<>(), mainkey = new ArrayList<>();
        for(char c : KEY.toCharArray())
        {
            mainkey.add(c);
        }
        
        int index = 0;
        int firstLetterNumber = KEY.toCharArray()[index];
        
        for(int i=0; i<KEY.length() / 2; i++)
        {
            int tmpSeperator = ((firstLetterNumber + remainderInt) % (mainkey.size() - 1));
            sk1.add(mainkey.get(tmpSeperator));
            index++;
            firstLetterNumber = KEY.toCharArray()[index];
            mainkey.remove(tmpSeperator);
        }
        
        int sizeRemaining = mainkey.size();
        
        for(int i=0; i<sizeRemaining - 1; i++)
        {
            int tmpSeperator = ((firstLetterNumber + remainderInt) % (mainkey.size() - 1));
            sk2.add(KEY.toCharArray()[tmpSeperator]);
            index++;
            firstLetterNumber = KEY.toCharArray()[index];
            mainkey.remove(tmpSeperator);
        }
        
        sk2.add(KEY.toCharArray()[((firstLetterNumber + remainderInt) % mainkey.size())]);
        
        for(char c : sk1)
        {
            SUBKEY1 += c;
        }
        
        for(char c : sk2)
        {
            SUBKEY2 += c;
        }
        
        System.out.println("Subkey1: " + SUBKEY1);
        System.out.println("Subkey2: " + SUBKEY2);
    }
    
}
