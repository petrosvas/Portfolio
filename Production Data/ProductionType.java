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
public enum ProductionType {
    Coal(0, "Coal"), Stone(1, "Stone"), Clay(2, "Clay"), Iron(3, "Iron"), Wood(4, "Wood");
    public final int type;
    public final String typeString;
    private ProductionType(int type, String typeString)
    {
        this.type = type;
        this.typeString = typeString;
    }
    public static String[] GetAllTypes()
    {
        String[] result = new String[ProductionType.values().length];
        int i = 0;
        
        for(ProductionType type : ProductionType.values())
        {
            result[i] = type.typeString;
            i++;
        }
        
        return result;
    }
}
