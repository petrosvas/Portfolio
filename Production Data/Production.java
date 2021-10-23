/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package production;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Iterator;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

/**
 *
 * @author Peter
 */
public class Production {

    /**
     * @param args the command line arguments
     * @throws java.io.FileNotFoundException
     */
    public static void main(String[] args) {
        // TODO code application logic here
        
        /*DataSet set = new DataSet();
        ProductionSite athens = new ProductionSite("Athens", "Dekeleias 25, Nea Filadelfeia", "A43jSu4Js93j2",ProductionType.Stone, 89.35);
        athens.workers.add(new Worker("Petros Antoniou", "PetAnt", "123456", WorkerPosition.TruckDirver.position, 4.25, 7.5));
        athens.workers.add(new Worker("Georgios Antreou", "GeoAnt", "g12ant", WorkerPosition.Miner.position, 2.25, 3.5));
        
        set.NewSite(athens);
        System.out.println(set.GetSingleSiteInfo(athens.ID));
        System.out.println(set.GetAllSiteInfo());
        
        set.NewSite(new ProductionSite("Poros", "Makedonias 12, Poros", "Po-H73hsUs87Sy2", ProductionType.Iron, 78.2));
        ProductionSite ps = set.GetSingleSiteInfoByNameAsProductionSite("Poros");
        if(ps != null)
        {
            ps.AddWorker(new Worker("Petros Antoniou", "PetAnt", "123456", WorkerPosition.TruckDirver.position, 4.25, 7.5));
            ps.AddWorker(new Worker("Georgios Antreou", "GeoAnt", "g12ant", WorkerPosition.Miner.position, 2.25, 3.5));
        }
         System.out.println(set.GetAllSiteInfo());
         
        
        String[] result = new String[ProductionType.values().length];
        int i = 0;
        
        for(ProductionType type : ProductionType.values())
        {
            result[i] = type.typeString;
            i++;
        }
        
        
        System.out.println(Arrays.toString(result));
        return;*/
        
        /*NewSite ns = new NewSite();
        ns.setAlwaysOnTop(true);
        NewSite ns2 = new NewSite();
        ns2.setAlwaysOnTop(true);*/
        
        String path = System.getProperty("user.dir");
        
        Scanner scan;
        
        System.out.println("Working Directory = " + path);
        
        /*DataSet.NewSite(new ProductionSite("Karistos", "Peloponisou 78, Karistos", "Ka-G8gkS3hj7S", ProductionType.Clay, 112.52));
        DataSet.NewSite(new ProductionSite("Poros", "Makedonias 12, Poros", "Po-H73hsUs87Sy2", ProductionType.Iron, 78.27));
        DataSet.GetSingleSiteInfoByNameAsProductionSite("Poros").AddWorker(new Worker("Georgios Bazos", "gvaz", "11gvas1", WorkerPosition.TruckDirver.position, 4.25, 7.5));
        DataSet.GetSingleSiteInfoByNameAsProductionSite("Poros").AddWorker(new Worker("Petros Antoniou", "PetAnt", "123456", WorkerPosition.TruckDirver.position, 4.25, 7.5));
        DataSet.GetSingleSiteInfoByNameAsProductionSite("Poros").AddWorker(new Worker("Georgios Antreou", "GeoAnt", "g12ant", WorkerPosition.Miner.position, 2.25, 3.5));

        JSONObject worker = new JSONObject();
        worker.put("fullName", "Georgios Bazos");
        worker.put("loginName", "gvaz");
        worker.put("loginPass", "11gvas1");
        worker.put("position", WorkerPosition.TruckDirver.position);
        worker.put("salary", 4.25);
        worker.put("hours", 7.5);
        
        JSONObject worker2 = new JSONObject();
        worker2.put("fullName", "Petros Antoniou");
        worker2.put("loginName", "PetAnt");
        worker2.put("loginPass", "123456");
        worker2.put("position", WorkerPosition.TruckDirver.position);
        worker2.put("salary", 4.25);
        worker2.put("hours", 7.5);
        
        JSONObject worker3 = new JSONObject();
        worker3.put("fullName", "Georgios Antreou");
        worker3.put("loginName", "GeoAnt");
        worker3.put("loginPass", "g12ant");
        worker3.put("position", WorkerPosition.Miner.position);
        worker3.put("salary", 2.25);
        worker3.put("hours", 3.5);
        
        ArrayList<JSONObject> workers = new ArrayList();
        workers.add(worker);
        workers.add(worker2);
        workers.add(worker3);
        
        JSONObject obj = new JSONObject();
        obj.put("name", "Karistos");
        obj.put("address", "Peloponisou 78, Karistos");
        obj.put("ID", "Ka-G8gkS3hj7S");
        obj.put("type", ProductionType.Clay.typeString);
        obj.put("avgQty", 112.52);
        obj.put("workers", null);
        
        JSONObject obj2 = new JSONObject();
        obj2.put("name", "Poros");
        obj2.put("address", "Makedonias 12, Poros");
        obj2.put("ID", "Po-H73hsUs87Sy2");
        obj2.put("type", ProductionType.Iron.typeString);
        obj2.put("avgQty", 78.27);
        obj2.put("workers", workers);

        ArrayList<JSONObject> allSites = new ArrayList();
        
        allSites.add(obj);
        allSites.add(obj2);
        
        JSONObject sites = new JSONObject();
        sites.put("sites", allSites);
        
        System.out.print(sites.toJSONString()); 
        try {
            scan = new Scanner(new File(DataSet.path));
            DataSet.Initialize();
            DataSet.Write(sites);
        } catch (FileNotFoundException ex) {
            Logger.getLogger(Production.class.getName()).log(Level.SEVERE, null, ex);
            return;
        }
        */
        
        try {
            scan = new Scanner(new File(DataSet.path));
        } catch (FileNotFoundException ex) {
            Logger.getLogger(Production.class.getName()).log(Level.SEVERE, null, ex);
            return;
        }
        
        JSONParser parser = new JSONParser();
        String json = "";
        
        while(scan.hasNextLine())
        {
            json += scan.nextLine();
        }
        DataSet.jsonString = json;
        
        JSONObject ob;
        
        try {
            ob = (JSONObject) parser.parse(json);
        } catch (ParseException ex) {
            Logger.getLogger(Production.class.getName()).log(Level.SEVERE, null, ex);
            return;
        }
        
        JSONArray array = (JSONArray) ob.get("sites");
        
        for (Iterator it = array.iterator(); it.hasNext();) {
            JSONObject o = (JSONObject) it.next();
            String name, address, id, type;
            double avg;
            JSONArray workersInSite;
            
            name = (String) o.get("name");
            address = (String) o.get("address");
            id = (String) o.get("ID");
            type = (String) o.get("type");
            avg = (double) o.get("avgQty");
            try
            {
                ProductionSite site = new ProductionSite(name, address, id, type, avg);
                JSONArray array2 = (JSONArray) o.get("workers");
                for (Iterator it2 = array2.iterator(); it2.hasNext();) {
                    JSONObject w = (JSONObject) it2.next();
                    String fullName, loginName, loginPass, position;
                    double salary, hours;
                    
                    fullName = (String) w.get("fullName");
                    loginName = (String) w.get("loginName");
                    loginPass = (String) w.get("loginPass");
                    position = (String) w.get("position");
                    salary = (double) w.get("salary");
                    hours = (double) w.get("hours");
                    site.AddWorker(new Worker(fullName, loginName, loginPass, position, salary, hours));
                }
                DataSet.NewSite(site);
            }
            catch(java.lang.ClassCastException ex)
            {
                ProductionSite site = new ProductionSite(name, address, id, type, avg);
                DataSet.NewSite(site);
                System.out.println("Class Exception Found! Message: " + ex.getMessage());
            }
            catch(NullPointerException e)
            {
                ProductionSite site = new ProductionSite(name, address, id, type, avg);
                DataSet.NewSite(site);
                System.out.println("Null Pointer Exception Found! Message: " + e.getMessage());
            }
        }
            
            
        
        Overview overview = new Overview();
        
    }
    
}
