package card.application;

import java.io.InputStream;
import java.time.*;
import java.util.HashMap;
import java.util.Date;
import java.util.Map.Entry;
import java.util.Scanner;

public class CardApplication {

    private static int employeeIndex = 0;
    
    private static int employeesRemoved = 0;
    
    static int year = 2020;
    
    // Every employee has an id, which is used to search for their information. The first argument of the HashMaps is the id of the specific employee.
        
    // employeeName holds the information on employees' names
   static HashMap<Integer, String> employeeName = new HashMap<>();

    // employeeWork holds the information on employees' work station
    static HashMap<Integer, String> employeeWork = new HashMap<>();

    // employeeHireDate holds the information on employees' hire date
    static HashMap<Integer, LocalDate> employeeHireDate = new HashMap<>();

    // employeeRemovedDate holds the information regarding the date the employee left the company
    static HashMap<Integer, LocalDate> employeeRemovedDate = new HashMap<>();

    // employeeRemovedReason holds the information regarding the reason the employee left the comapny (Firing or Resignation)
    static HashMap<Integer, String> employeeRemovedReason = new HashMap<>();

    // employeePay holds the information on employees' hourly salary. 5 Euros per hour is 800 Euros per month, assuing the employee workks exactly 4 weeks a month and every week for 5 days (20 days a month)
    static HashMap<Integer, Integer> employeePay = new HashMap<>();

    // employeeShiftStart holds the informarion on employees' start of shifd using a 4 digit integer representing hours and employeeShiftEnd on employees' end of shift
    static HashMap<Integer, Integer> employeeShiftStart = new HashMap<>();
    static HashMap<Integer, Integer> employeeShiftEnd = new HashMap<>();
    
    public static void main(String[] args) {
        
        LogIn lg = new LogIn();
        
    }
    
    public static void AddEmployee(String name, String work, Date hire, Date remove, String removedReason, int pay, int shiftStart, int ShiftEnd)
    {
        employeeIndex++;
        
        employeeName.put(employeeIndex, name);
        
        employeeWork.put(employeeIndex, work);
        
        employeeHireDate.put(employeeIndex, hire.toInstant().atZone(ZoneId.systemDefault()).toLocalDate());
        
        if(remove != null)
            employeeRemovedDate.put(employeeIndex, remove.toInstant().atZone(ZoneId.systemDefault()).toLocalDate());
        else
            employeeRemovedDate.put(employeeIndex, null);
        
        if(removedReason != null)
            employeeRemovedReason.put(employeeIndex, removedReason);
        else
            employeeRemovedReason.put(employeeIndex, null);
        
        employeePay.put(employeeIndex, pay);
        
        employeeShiftStart.put(employeeIndex, shiftStart);
        
        employeeShiftEnd.put(employeeIndex, ShiftEnd);
    }
    
    public static void RemoveEmployee(int id, String reason)
    {
        employeeRemovedDate.put(id, new Date().toInstant().atZone(ZoneId.systemDefault()).toLocalDate());
        
        employeeRemovedReason.put(id, reason);
        
        employeesRemoved++;
    }
    
    public static int DateAtYear(int year)
    {
        // When the Date Class takes 3 integers as input, the first (the year) is added to 1900. So, this method subtracts 1900 to get the actual year.
        return year - 1900;
    }
    
    public static int DateAtMonth(int month)
    {
        // The second argument, the month, starts from 0. So, this method decreases it by one to match the number of the actual month.
        return month - 1;
    }
    
    public static String CalculatePay(int index)
    {
        int yearsWorked;
        if (employeeRemovedDate.get(index) == null)
            yearsWorked = new Date().toInstant().atZone(ZoneId.systemDefault()).toLocalDate().getYear() - employeeHireDate.get(index).getYear();
        else
            yearsWorked = employeeRemovedDate.get(index).getYear() - employeeHireDate.get(index).getYear();
        
        int monthsWorked;
        if (employeeRemovedDate.get(index) == null)
            monthsWorked = new Date().toInstant().atZone(ZoneId.systemDefault()).toLocalDate().getMonthValue() - employeeHireDate.get(index).getMonthValue();
        else
            monthsWorked = employeeRemovedDate.get(index).getMonthValue() - employeeHireDate.get(index).getMonthValue();
        if(monthsWorked < 0)
        {
            yearsWorked--;
            monthsWorked += 12;
        }
        
        int hoursTotalWorked;
        int hoursPerDay;
        int daysWorked = 0;
        int sundaysWorked = 0;
        int nightHours = 0;
        int overtimeHours = 0;
        double initialPaymentPerDay = 0;
        double initialTotalPay = 0;
        double totalPay = 0;
        
        
        if(employeeShiftEnd.get(index) - employeeShiftStart.get(index) > 0)
        {
            hoursPerDay = employeeShiftEnd.get(index) - employeeShiftStart.get(index);
        }
        else
        {
            hoursPerDay = 24 - employeeShiftStart.get(index) + employeeShiftEnd.get(index);
        }
        
        if(employeeShiftStart.get(index) > 22)
        {
            nightHours += (24 - employeeShiftStart.get(index));
        }
        else if(employeeShiftStart.get(index) < 6)
        {
            nightHours += (6 - employeeShiftStart.get(index));
        }
        
        if(employeeShiftEnd.get(index) > 22)
        {
            nightHours += (employeeShiftEnd.get(index) - 22);
        }
        else if(employeeShiftEnd.get(index) < 6)
        {
            nightHours += (employeeShiftEnd.get(index) + 2); // If shift ends before 6, overtime is 6 - shift start + 2 between 22 and 24.
        }
        
        initialPaymentPerDay = ((hoursPerDay - nightHours) * employeePay.get(index)) + (nightHours * employeePay.get(index) * 1.25);
        
        if (employeeRemovedDate.get(index) == null)
        {
            for (LocalDate date = employeeHireDate.get(index); date.isBefore(new Date().toInstant().atZone(ZoneId.systemDefault()).toLocalDate()); date = date.plusDays(1))
            {
                daysWorked++;
                if(date.getDayOfWeek() == DayOfWeek.SUNDAY)
                {
                    sundaysWorked++;
                }
            }
        }
        else
        {
            for (LocalDate date = employeeHireDate.get(index); date.isBefore(employeeRemovedDate.get(index)); date = date.plusDays(1))
            {
                daysWorked++;
                if(date.getDayOfWeek() == DayOfWeek.SUNDAY)
                {
                    sundaysWorked++;
                }
            }
        }
        
        initialTotalPay = ((daysWorked - sundaysWorked) * initialPaymentPerDay) + (sundaysWorked * initialPaymentPerDay * 1.75);
        
        if(hoursPerDay > 8)
        {
            if(hoursPerDay == 9)
            {
                totalPay = initialTotalPay * 1.2;
            }
            else if(hoursPerDay == 10)
            {
                totalPay = initialTotalPay * (1.2 + 1.4);
            }
            else if(hoursPerDay == 11)
            {
                totalPay = initialTotalPay * (1.2 + 1.4 + 1.4);
            }
            else
            {
                totalPay = initialTotalPay * (1.2 + 1.4 + 1.4 + 1.8);
            }
        }
        else
        {
            totalPay = initialTotalPay;
        }
        
        hoursTotalWorked = daysWorked * (int) hoursPerDay;
        
        String ret = "";
        
        
        ret += "Years worked: " + yearsWorked + ". \nMonths worked: " + monthsWorked + ". \nTotal days worked: " + daysWorked;
        ret += ". \nHours per day: " + hoursPerDay + ". \nTotal hours worked: " + hoursTotalWorked;
        ret += ". \nSundays worked: " + sundaysWorked;
        ret += ". \nNight hours worked: " + nightHours * daysWorked;
        ret += ". \nNight hours per day: " + nightHours;
        ret += ". \nOvrtime per day worked: " + (hoursPerDay - 8);
        ret += ". \nTotal days worked: " + daysWorked;
        ret += ". \nInitial Payment per day (including night shifts): " + initialPaymentPerDay;
        ret += ". \nInitial total payment: " + (initialPaymentPerDay * daysWorked);
        ret += ". \nInitial total pay (including sundays): " + initialTotalPay;
        ret += ". \nTotal pay (including overtime): " + totalPay;
        return ret;
    }
    
    public static String CalculatePay(int index, LocalDate start, LocalDate end)
    {
        int hoursTotalWorked;
        int hoursPerDay;
        int daysWorked = 0;
        int sundaysWorked = 0;
        int nightHours = 0;
        int overtimeHours = 0;
        double initialPaymentPerDay = 0;
        double initialTotalPay = 0;
        double totalPay = 0;
        
        
        if(employeeShiftEnd.get(index) - employeeShiftStart.get(index) > 0)
        {
            hoursPerDay = employeeShiftEnd.get(index) - employeeShiftStart.get(index);
        }
        else
        {
            hoursPerDay = 24 - employeeShiftStart.get(index) + employeeShiftEnd.get(index);
        }
        
        if(employeeShiftStart.get(index) > 22)
        {
            nightHours += (24 - employeeShiftStart.get(index));
        }
        else if(employeeShiftStart.get(index) < 6)
        {
            nightHours += (6 - employeeShiftStart.get(index));
        }
        
        if(employeeShiftEnd.get(index) > 22)
        {
            nightHours += (employeeShiftEnd.get(index) - 22);
        }
        else if(employeeShiftEnd.get(index) < 6)
        {
            nightHours += (employeeShiftEnd.get(index) + 2); // If shift ends before 6, overtime is 6 - shift start + 2 between 22 and 24.
        }
        
        initialPaymentPerDay = ((hoursPerDay - nightHours) * employeePay.get(index)) + (nightHours * employeePay.get(index) * 1.25);
        
        for (LocalDate date = start; date.isBefore(end); date = date.plusDays(1))
        {
            daysWorked++;
            if(date.getDayOfWeek() == DayOfWeek.SUNDAY)
            {
                sundaysWorked++;
            }
        }
        
        initialTotalPay = ((daysWorked - sundaysWorked) * initialPaymentPerDay) + (sundaysWorked * initialPaymentPerDay * 1.75);
        
        if(hoursPerDay > 8)
        {
            if(hoursPerDay == 9)
            {
                totalPay = initialTotalPay * 1.2;
            }
            else if(hoursPerDay == 10)
            {
                totalPay = initialTotalPay * (1.2 + 1.4);
            }
            else if(hoursPerDay == 11)
            {
                totalPay = initialTotalPay * (1.2 + 1.4 + 1.4);
            }
            else
            {
                totalPay = initialTotalPay * (1.2 + 1.4 + 1.4 + 1.8);
            }
        }
        else
        {
            totalPay = initialTotalPay;
        }
        
        hoursTotalWorked = daysWorked * (int) hoursPerDay;
        
        String ret = "";
        
        ret += "Hours per day: " + hoursPerDay + ". \nTotal hours worked: " + hoursTotalWorked;
        ret += ". \nSundays worked: " + sundaysWorked;
        ret += ". \nNight hours worked: " + nightHours * daysWorked;
        ret += ". \nNight hours per day: " + nightHours;
        ret += ". \nOvrtime per day worked: " + (hoursPerDay - 8);
        ret += ". \nTotal days worked: " + daysWorked;
        ret += ". \nInitial Payment per day (including night shifts): " + initialPaymentPerDay;
        ret += ". \nInitial total payment: " + (initialPaymentPerDay * daysWorked);
        ret += ". \nInitial total pay (including sundays): " + initialTotalPay;
        ret += ". \nTotal pay (including overtime): " + totalPay;
        return ret;
        
    }
    
    public static String PrintInfo(int i)
    {
        if(employeeRemovedDate.get(i) == null)
        {
            return "Employee name: " + employeeName.get(i) + ". \nWorke: " + employeeWork.get(i) + 
        ". \nHired on: " + employeeHireDate.get(i) + ". \nEmployee is still working here" + " .\nMonthly salary is " + employeePay.get(i) + " $, with a shift starting at " + employeeShiftStart.get(i) + ":00 and ending at " 
                    + employeeShiftEnd.get(i) + ":00.";
        }
        else
        {
            return "Employee name: " + employeeName.get(i) + ". \nWorke: " + employeeWork.get(i) + 
        ". \nHired on: " + employeeHireDate.get(i) + ". \nLeft on " + employeeRemovedDate.get(i) + " .\nReason of removal: " + 
        employeeRemovedReason.get(i) + ". \nMonthly salary was " + employeePay.get(i) + " $, with a shift starting at " + employeeShiftStart.get(i) + ":00 and ending at " + employeeShiftEnd.get(i) + ":00.";
        }
    }
    
    public static int indexCount()
    {
        return employeeIndex;
    }
    
    public static String indexCountString()
    {
        return String.valueOf(employeeIndex);
    }
    
    public static int RemovedCount()
    {
        return employeesRemoved;
    }
    
}
