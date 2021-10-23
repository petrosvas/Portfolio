package criptography;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.security.NoSuchAlgorithmException;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;

public class Criptography {

    public static void main(String[] args) {
        scan = new Scanner(System.in);
        
        
        /*
        System.out.print("Enter Key: ");
        String key = scan.nextLine();
        System.out.print("Enter Text: ");
        String text = scan.nextLine();
        int[] chars = new int[text.toCharArray().length];
        int[] encrypted = new int[chars.length];
        
        System.out.println("Encrypting....");
        for(int i = 0; i < text.toCharArray().length; i++)
        {
            chars[i] = (int) text.toCharArray()[i];
        }
        
        int keyLength = 0;
        for(char c : key.toCharArray())
        {
            keyLength += (int) c;
        }
        
        for(int i = 0; i < chars.length; i++)
        {
            encrypted[i] = chars[i] + keyLength;
        }
        
        System.out.print("Encrypted message: ");
        
        for(int i : encrypted)
        {
            System.out.print(i);
        }
        
        System.out.println("Decrypting....");
        
        for(int i = 0; i < chars.length; i++)
        {
            encrypted[i] -= keyLength;
        }
        
        System.out.print("Decrypted message: ");
        
        for(int i : encrypted)
        {
            System.out.print((char)i);
        }
        */
        
        System.out.println("1: Enter text manually. 2: Read from file");
        int choice = scan.nextInt();
        if(choice == 1)
        {
            enterText();
        }
        else if(choice == 2)
        {
            readFromFile();
        }
        else
        {
            System.out.println("No option selected. Exiting!");
            return;
        }
        
    }
    
    public static Scanner scan;
    public static String TXT = "";
    public static String EncryptionStandard = "";
    public static boolean encrypted = false;
    public static boolean decrypted = true;
    public static SecretKey EcncryptionKey = null;
    public static byte[] EcnryptedText;
    public static String DencyptedText;
    
    public static void enterText()
    {
        System.out.println("Select encryption standard: 1 = DES, 2 = AES.");
        String line = scan.nextLine();
        while(!line.equals("1") && !line.equals("2"))
        {
            System.out.println("Please enter a valid choice!");
            System.out.println("Select encryption standard: 1 = DES, 2 = AES.");
            line = scan.nextLine();
        }
        
        if(line.equals("1"))
        {
            EncryptionStandard = "DES";
        }
        else
        {
            EncryptionStandard = "AES";
        }
        
        Cipher cipher = null;
                
        try {
            cipher = Cipher.getInstance(EncryptionStandard);
        } catch (NoSuchAlgorithmException ex) {
            Logger.getLogger(Criptography.class.getName()).log(Level.SEVERE, null, ex);
        } catch (NoSuchPaddingException ex) {
            Logger.getLogger(Criptography.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        System.out.print("Enter the text to be encrypted: ");
        TXT = scan.nextLine();
        DencyptedText = TXT;
        
        while(true)
        {
            int choice = 0;
            while(true)
            {
                System.out.println("1: Encrypt Text. 2: Decrypt Text. 3: Show Text. 4: Exit");
                choice = 0;
                try
                {
                    choice = Integer.parseInt(scan.next());
                    if(choice >= 1 && choice <= 4)
                        break;
                    else
                    {
                        System.out.println("Please enter a valid choice!");
                    }
                }
                catch(Exception ex)
                {
                    System.out.println("Please enter a valid choice!");
                }
            }
            
            
            if(choice == 1)
            {
                if(!encrypted)
                {
                    try
                    {
                        EcncryptionKey = generateKey(EncryptionStandard);
                        EcnryptedText = encryptString(TXT, EcncryptionKey, cipher);
                        encrypted = true;
                        decrypted = false;
                    }
                    catch (Exception ex)
                    {
                        System.out.println(ex.getMessage());
                    }
                }
                else
                {
                    System.out.println("The text is already encrypted!");
                }
            }
            else if(choice == 2)
            {
                if(!decrypted)
                {
                    try
                    {
                        DencyptedText = decryptString(EcnryptedText, EcncryptionKey, cipher);
                        encrypted = false;
                        decrypted = true;
                    }
                    catch (Exception ex)
                    {
                        System.out.println(ex.getMessage());
                    }
                }
                else
                {
                    System.out.println("The text is already decrypted!");
                }
            }
            else if(choice == 3)
            {
                if(decrypted)
                {
                    System.out.println(DencyptedText);
                }
                else
                {
                    System.out.println(new String(EcnryptedText));
                }
            }
            else
            {
                break;
            }
        }
        
    }
    
    public static void readFromFile()
    {
        System.out.println("Select encryption standard: 1 = DES, 2 = AES.");
        String line = scan.nextLine();
            System.out.println("Please enter a valid choice!");
            System.out.println("Select encryption standard: 1 = DES, 2 = AES.");
            line = scan.nextLine();
        
        if(line.equals("1"))
        {
            EncryptionStandard = "DES";
        }
        else
        {
            EncryptionStandard = "AES";
        }
        
        Cipher cipher = null;
                
        try {
            cipher = Cipher.getInstance(EncryptionStandard);
        } catch (NoSuchAlgorithmException ex) {
            Logger.getLogger(Criptography.class.getName()).log(Level.SEVERE, null, ex);
        } catch (NoSuchPaddingException ex) {
            Logger.getLogger(Criptography.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        
        TXT = "";
        System.out.println("Read text directly from file encryptedText!");
        try {
            Scanner newScanner = new Scanner(new File("C:\\Users\\Furius\\Documents\\NetBeansProjects\\Criptography\\src\\criptography\\encryptedText.txt"));
            while (newScanner.hasNextLine()) {
            TXT += newScanner.nextLine();
          }
        } catch (FileNotFoundException ex) {
            System.out.println("Did not find file to read. Exiting!");
            return;
        }
        DencyptedText = TXT;
        
        BufferedWriter writer;
        try {
            writer = new BufferedWriter(new FileWriter("C:\\Users\\Furius\\Documents\\NetBeansProjects\\Criptography\\src\\criptography\\decreptedText.txt"));
        } catch (IOException ex) {
            System.out.println("Did not find file to write. Exiting!");
            return;
        }
	    //     writer.write(str);
	    
	    //     writer.close();
        
        while(true)
        {
            int choice = 0;
            while(true)
            {
                System.out.println("1: Encrypt Text. 2: Decrypt Text. 3: Show Text. 4: Exit");
                choice = 0;
                try
                {
                    choice = Integer.parseInt(scan.next());
                    if(choice >= 1 && choice <= 4)
                        break;
                    else
                    {
                        System.out.println("Please enter a valid choice!");
                    }
                }
                catch(Exception ex)
                {
                    System.out.println("Please enter a valid choice!");
                }
            }
            
            
            if(choice == 1)
            {
                if(!encrypted)
                {
                    try
                    {
                        EcncryptionKey = generateKey(EncryptionStandard);
                        EcnryptedText = encryptString(TXT, EcncryptionKey, cipher);
                        for(byte b : EcnryptedText)
                        {
                            writer.write(b);
                        }
                        writer.close();
                        encrypted = true;
                        decrypted = false;
                    }
                    catch (Exception ex)
                    {
                        System.out.println(ex.getMessage());
                    }
                }
                else
                {
                    System.out.println("The text is already encrypted!");
                }
            }
            else if(choice == 2)
            {
                if(!decrypted)
                {
                    try
                    {
                        DencyptedText = decryptString(EcnryptedText, EcncryptionKey, cipher);
                        encrypted = false;
                        decrypted = true;
                    }
                    catch (Exception ex)
                    {
                        System.out.println(ex.getMessage());
                    }
                }
                else
                {
                    System.out.println("The text is already decrypted!");
                }
            }
            else if(choice == 3)
            {
                if(decrypted)
                {
                    System.out.println(DencyptedText);
                }
                else
                {
                    System.out.println(new String(EcnryptedText));
                }
            }
            else
            {
                break;
            }
        }
    }
    
    public static SecretKey generateKey(String encryptionType)
    {
        try 
        {
            KeyGenerator keyGenerator = KeyGenerator.getInstance(encryptionType);
            SecretKey myKey = keyGenerator.generateKey();
            return myKey;
        } 
        catch (Exception ex) 
        {
            System.out.println(ex.getMessage());
            return null;
        }
    }
    
    public static byte[] encryptString(String toEncrypt, SecretKey myKey, Cipher cipher)
    {
        try
        {
            byte[] text = toEncrypt.getBytes("UTF-8");
            cipher.init(Cipher.ENCRYPT_MODE, myKey);
            byte[] encryptedText = cipher.doFinal(text);
            return encryptedText;
        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
            return null;
        }
    }
    
    public static String decryptString(byte[] toDecrypt, SecretKey myKey, Cipher cipher)
    {
        try
        {
            cipher.init(Cipher.DECRYPT_MODE, myKey);
            byte[] decreptedText = cipher.doFinal(toDecrypt);
            String result = new String(decreptedText);
            return result;
        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
            return null;
        }
    }
    
}
