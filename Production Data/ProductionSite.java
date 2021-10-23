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

public class ProductionSite {
    public String name;
    public String address;
    public String ID;
    public ProductionType type;
    public double avrgQtyPerYear;
    ArrayList<Worker> workers = new ArrayList<>();
    
    public ProductionSite(ProductionSite site)
    {
        this.address = site.address;
        this.ID = site.ID;
        this.type = site.type;
        this.avrgQtyPerYear = site.avrgQtyPerYear;
    }
    
    public ProductionSite(String name, String address, String ID, ProductionType type, double avrgQtyPerYear)
    {
        this.name = name;
        this.address = address;
        this.ID = ID;
        this.type = type;
        this.avrgQtyPerYear = avrgQtyPerYear;
    }
    
    public ProductionSite(String name, String address, String ID, String type, double avrgQtyPerYear)
    {
        this.name = name;
        this.address = address;
        this.ID = ID;
        this.type = ProductionType.valueOf(type);
        this.avrgQtyPerYear = avrgQtyPerYear;
    }
    
    public void AddWorker(Worker w)
    {
        workers.add(w);
    }
    
    public void ClearWorkers()
    {
        workers.clear();
    }
    
    @Override
    public String toString()
    {
        String result = "Name: " + this.name + "\n";
        result += "Address: " + this.address + "\n";
        result += "Site ID: " + this.ID + "\n";
        result += "Type of product: " + type.typeString + "\n";
        result += "Average Quantity of product per year: " + new java.text.DecimalFormat("#.##").format(avrgQtyPerYear) + " tons.\n";
        result += "Workers:\n";
        for(Worker w : workers)
        {
            result += "\tName: " + w.fullName + "\n";
            result += "\tPosition: " + w.position + "\n";
            result += "\tSalary: " + w.salary + "\n";
            result += "\tHours: " + w.hours + "\n\n";
        }
        return result;
    }
}
