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
public enum WorkerPosition {
    
    Miner("Miner"),
    Supervisor("Supervisor"),
    TruckLoader("TruckLoader"),
    TruckDirver("TruckDirver")
    ;
    public final String position;
    private WorkerPosition(String position)
    {
        this.position = position;
    }
    
    public static String[] AllPositions()
    {
        String[] result = new String[WorkerPosition.values().length];
        int i = 0;
        
        for(WorkerPosition pos : WorkerPosition.values())
        {
            result[i] = pos.position;
            i++;
        }
        
        return result;
    }
}
