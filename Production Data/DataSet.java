/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package production;

/**
 *
 * @author Peter
 */
import java.util.ArrayList;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.Scanner;
import org.json.simple.JSONObject;

public class DataSet {
    private static ArrayList<ProductionSite> sites = new ArrayList<>();
    
    public final static String path = System.getProperty("user.dir") + "\\json\\users.json";
    public static String jsonString = "";
    
    private static JSONObject obj = new JSONObject();

    private static PrintWriter writer;
    
    public static void Initialize() throws FileNotFoundException
    {
        writer = new PrintWriter(new File(path));
    }
    
    public static void Write(JSONObject json)
    {
        writer.write(json.toJSONString());
        writer.close();
    }
    
    public static void NewSite(ProductionSite site)
    {
        sites.add(site);
    }
    
    public static String GetSingleSiteInfoById(String siteId)
    {
        for(ProductionSite site : sites)
        {
            if(site.ID.equals(siteId))
                return site.toString();
        }
        return null;
    }
    
    public static String GetSingleSiteInfoByName(String siteName)
    {
        for(ProductionSite site : sites)
        {
            if(site.name.equals(siteName))
                return site.toString();
        }
        return null;
    }
    
    public static ProductionSite GetSingleSiteInfoByIdAsProductionSite(String siteId)
    {
        for(ProductionSite site : sites)
        {
            if(site.ID.equals(siteId))
                return site;
        }
        return null;
    }
    
    public static ProductionSite GetSingleSiteInfoByNameAsProductionSite(String siteName)
    {
        for(ProductionSite site : sites)
        {
            if(site.name.equals(siteName))
                return site;
        }
        return null;
    }
    
    public static ProductionSite[] GetAllSitesAsProductionSite()
    {
        ProductionSite[] s = new ProductionSite[sites.size()];
        for(int i=0; i<sites.size(); i++)
        {
            s[i] = sites.get(i);
        }
        return s;
    }
    
    public static String GetAllSiteInfo()
    {
        String result = "";
        for(ProductionSite site : sites)
        {
            result += site.toString();
            result += "---------------------------------------------------\n";
        }
        return result;
    }
    
    public static void ClearList()
    {
        sites.clear();
    }
}
