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
public class Worker {
    public String fullName;
    public String loginName;
    public String loginPass;
    public String position;
    public double salary;
    public double hours;
    public Worker(String fullName, String loginName, String loginPass, String position, double salary, double hours)
    {
        this.fullName = fullName;
        this.loginName = loginName;
        this.loginPass = loginPass;
        this.position = position;
        this.salary = salary;
        this.hours = hours;
    }
}
