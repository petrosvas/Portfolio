/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package production;

import java.awt.Dimension;
import java.awt.Toolkit;
import java.awt.event.WindowEvent;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

/**
 *
 * @author Peter
 */
public class NewSite extends javax.swing.JFrame {

    ProductionSite[] sites;
    /**
     * Creates new form NewSite
     */
    public NewSite() {
        initComponents();
        Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();
        this.setLocation(dim.width/2-this.getSize().width/2, dim.height/2-this.getSize().height/2);
        this.setVisible(true);
        sites = DataSet.GetAllSitesAsProductionSite();
        jComboBox1.removeAllItems();
        for(String pos : ProductionType.GetAllTypes())
        {
            jComboBox1.addItem(pos);
        }
        jComboBox1.setSelectedIndex(0);
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();
        jLabel4 = new javax.swing.JLabel();
        jTextField2 = new javax.swing.JTextField();
        jTextField3 = new javax.swing.JTextField();
        jTextField5 = new javax.swing.JTextField();
        jComboBox1 = new javax.swing.JComboBox<>();
        jLabel5 = new javax.swing.JLabel();
        jLabel6 = new javax.swing.JLabel();
        jLabel7 = new javax.swing.JLabel();
        jButton1 = new javax.swing.JButton();
        jLabel8 = new javax.swing.JLabel();
        jTextField4 = new javax.swing.JTextField();
        jLabel9 = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
        setTitle("New Production Site");
        setMaximumSize(new java.awt.Dimension(800, 400));
        setMinimumSize(new java.awt.Dimension(800, 400));
        setPreferredSize(new java.awt.Dimension(800, 400));
        setResizable(false);

        jLabel1.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
        jLabel1.setText("Name: ");
        jLabel1.setMaximumSize(new java.awt.Dimension(53, 16));
        jLabel1.setMinimumSize(new java.awt.Dimension(53, 16));
        jLabel1.setPreferredSize(new java.awt.Dimension(53, 16));

        jLabel2.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
        jLabel2.setText("Address: ");
        jLabel2.setMaximumSize(new java.awt.Dimension(53, 16));
        jLabel2.setMinimumSize(new java.awt.Dimension(53, 16));
        jLabel2.setPreferredSize(new java.awt.Dimension(53, 16));

        jLabel3.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
        jLabel3.setText("Production Type: ");
        jLabel3.setMaximumSize(new java.awt.Dimension(53, 16));
        jLabel3.setMinimumSize(new java.awt.Dimension(53, 16));
        jLabel3.setPreferredSize(new java.awt.Dimension(53, 16));

        jLabel4.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
        jLabel4.setText("Average Quantity Per Year: ");
        jLabel4.setMaximumSize(new java.awt.Dimension(53, 16));
        jLabel4.setMinimumSize(new java.awt.Dimension(53, 16));
        jLabel4.setPreferredSize(new java.awt.Dimension(53, 16));

        jComboBox1.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Item 1", "Item 2", "Item 3", "Item 4" }));

        jLabel5.setForeground(new java.awt.Color(255, 0, 0));

        jLabel6.setForeground(new java.awt.Color(255, 0, 0));

        jLabel7.setForeground(new java.awt.Color(255, 0, 0));

        jButton1.setText("Add Site");
        jButton1.setMaximumSize(new java.awt.Dimension(130, 50));
        jButton1.setMinimumSize(new java.awt.Dimension(130, 50));
        jButton1.setPreferredSize(new java.awt.Dimension(130, 50));
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });

        jLabel8.setHorizontalAlignment(javax.swing.SwingConstants.RIGHT);
        jLabel8.setText("ID: ");
        jLabel8.setMaximumSize(new java.awt.Dimension(53, 16));
        jLabel8.setMinimumSize(new java.awt.Dimension(53, 16));
        jLabel8.setPreferredSize(new java.awt.Dimension(53, 16));

        jLabel9.setForeground(new java.awt.Color(255, 0, 0));

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addGap(38, 38, 38)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                    .addComponent(jButton1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(jLabel3, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel4, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.PREFERRED_SIZE, 223, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(jLabel8, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.PREFERRED_SIZE, 223, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGap(18, 18, 18)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(jTextField2)
                            .addComponent(jTextField3)
                            .addComponent(jTextField5)
                            .addComponent(jComboBox1, javax.swing.GroupLayout.PREFERRED_SIZE, 228, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(jTextField4, javax.swing.GroupLayout.PREFERRED_SIZE, 228, javax.swing.GroupLayout.PREFERRED_SIZE))))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel5, javax.swing.GroupLayout.PREFERRED_SIZE, 202, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel6, javax.swing.GroupLayout.PREFERRED_SIZE, 202, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel7, javax.swing.GroupLayout.PREFERRED_SIZE, 202, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel9, javax.swing.GroupLayout.PREFERRED_SIZE, 202, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addContainerGap(73, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                .addContainerGap(54, Short.MAX_VALUE)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel8, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addComponent(jTextField4, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(jLabel9, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextField2, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel5, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                        .addComponent(jLabel2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jTextField3, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(jLabel6, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(jLabel3, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(jComboBox1, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                        .addComponent(jLabel4, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jTextField5, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(jLabel7, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addComponent(jButton1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(36, 36, 36))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        
        // 2, 3, 5,        5, 6, 7
        boolean withoutErrors = true;
        double avgQ = 0;
        
        if(jTextField2.getText().equals(""))
        {
            jLabel5.setText("Empty!");
            withoutErrors = false;
        }
        else if(jTextField2.getText().length() <2)
        {
            jLabel5.setText("At least 3 letters needed!");
            withoutErrors = false;
        }
        else
        {
            String fullname = jTextField2.getText();
            boolean nonaplha = true;
            for(char c : fullname.toCharArray())
            {
                if(!Character.isAlphabetic(c) && c != ' ')
                {
                    withoutErrors = false;
                    nonaplha = false;
                    jLabel5.setText("Only letters allowed!");
                    break;
                }
            }
            if(nonaplha)
            {
                jLabel5.setText("");
            }
        }
        
        if(jTextField3.getText().equals(""))
        {
            jLabel6.setText("Empty");
            withoutErrors = false;
        }
        else
        {
            jLabel6.setText("");
        }
        
        if(jTextField5.getText().equals(""))
        {
            jLabel7.setText("Empty");
            withoutErrors = false;
        }
        else
        {
            try
            {
                avgQ = Double.parseDouble(jTextField5.getText());
                jLabel7.setText("");
            }
            catch(Exception e)
            {
                jLabel7.setText("Only decimals allowed!");
                withoutErrors = false;
            }
        }
        
        if(jTextField4.getText().equals(""))
        {
            jLabel9.setText("Empty");
            withoutErrors = false;
        }
        
        if(withoutErrors)
        {
            char[] arr = jTextField2.getText().toCharArray();
            for(ProductionSite site : DataSet.GetAllSitesAsProductionSite())
            {
                if(site.ID.equals((arr[0] + arr[1] + "-" + jTextField4.getText())))
                {
                    jLabel9.setText("ID already exists!");
                    return;
                }
            }
            
            if(SaveNewSite())
            {
                DataSet.NewSite(new ProductionSite(jTextField2.getText(), jTextField3.getText(), (arr[0] + arr[1] + "-" + jTextField4.getText()), ProductionType.valueOf((String) jComboBox1.getSelectedItem()), avgQ));
                JOptionPane.showMessageDialog(this, "New Worker Added!");
                ShowSites.IncrementMaxItems();
                this.dispatchEvent(new WindowEvent(this, WindowEvent.WINDOW_CLOSING));
            }
            else
            {
                JOptionPane.showMessageDialog(this, "Error with Json file!");
            }
        }
        
    }//GEN-LAST:event_jButton1ActionPerformed

    private boolean SaveNewSite()
    {
        JSONObject ob;
        JSONParser parser = new JSONParser();
        
        try {
            ob = (JSONObject) parser.parse(DataSet.jsonString);
        } catch (ParseException ex) {
            JOptionPane.showMessageDialog(this, "Error: " + ex);
            return false;
        }
        
        JSONArray array = (JSONArray) ob.get("sites");
        ArrayList<JSONObject> list = new ArrayList<>();
        
        for (Iterator it = array.iterator(); it.hasNext();) {
            JSONObject o = (JSONObject) it.next();
            list.add(o);
        }
        
        JSONObject obj = new JSONObject();
        obj.put("name", jTextField2.getText());
        obj.put("address", jTextField3.getText());
        obj.put("ID", (jTextField2.getText().toCharArray()[0] + jTextField2.getText().toCharArray()[1] + "-" + jTextField4.getText()));
        obj.put("type", (ProductionType.valueOf((String) jComboBox1.getSelectedItem())).typeString);
        obj.put("avgQty", Double.parseDouble(jTextField5.getText()));
        obj.put("workers", null);
        
        list.add(obj);
        
        JSONObject sites = new JSONObject();
        sites.put("sites", list);
        
        try {
            DataSet.Initialize();
            DataSet.Write(sites);
        } catch (FileNotFoundException ex) {
            Logger.getLogger(Production.class.getName()).log(Level.SEVERE, null, ex);
            return false;
        }
        
        return true;
    }
    
    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(NewSite.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(NewSite.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(NewSite.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(NewSite.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new NewSite().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButton1;
    private javax.swing.JComboBox<String> jComboBox1;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JLabel jLabel7;
    private javax.swing.JLabel jLabel8;
    private javax.swing.JLabel jLabel9;
    private javax.swing.JTextField jTextField2;
    private javax.swing.JTextField jTextField3;
    private javax.swing.JTextField jTextField4;
    private javax.swing.JTextField jTextField5;
    // End of variables declaration//GEN-END:variables
}
