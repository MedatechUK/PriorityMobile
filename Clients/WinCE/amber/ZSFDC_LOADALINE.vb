Module ZSFDC_LOADALINE
    '---------------------------------------------------------------------
    '  Copyright (C) eMerge-IT.  All rights reserved.
    ' 
    'THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
    'KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
    'IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
    'PARTICULAR PURPOSE.
    '---------------------------------------------------------------------

Module LoadData

        Sub Main()

            'Dim Records() As String = {"A", "B", "C"}
            'Dim Environment As String = "demo"

            Dim ws As New PriWebSVC.Service ' ws is the webservice - it holds a reference to the SOAP service
            Dim erl As New Loading.DataLoad ' This is the loading itself - that will hold load data etc

            With erl ' using the loading we declared

                .Clear() ' means loading dot clear
                .Table = "ZSFDC_LOADALINE" ' specifies the name of the Priority loading table
                .Procedure = "ZSFDC_LOADALINE" ' the name of the procedure
                .Environment = "demo" ' the Priority COMPANY we are loading into
                .RecordType1 = "SERIALNAME," & _
                                "PARTNAME," & _
                                "ACTNAME," & _
                                "WORKCNAME," & _
                                "USERID," & _
                                "QUANT," & _
                                "SQUANT," & _
                                "DEFECTCODE," & _
                                "MQUANT," & _
                                "TQUANT," & _
                                "TSQUANT," & _
                                "TMQUANT," & _
                                "NUMPACK," & _
                                "PACKCODE," & _
                                "STIME," & _
                                "ETIME," & _
                                "ASPAN," & _
                                "EMPSTIME," & _
                                "EMPETIME," & _
                                "EMPASPAN," & _
                                "SHIFTNAME," & _
                                "TOOLNAME," & _
                                "RTYPEBOOL," & _
                                "MODE," & _
                                "WARHSNAME," & _
                                "LOCNAME," & _
                                "NEWPALLET," & _
                                "TOPALLETNAME," & _
                                "ACTCANCEL," & _
                                "SERCANCEL,"

                .RecordType2 = "" ' and type 2 columns (ie will be loaded as type 2)
                .RecordTypes = "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "TEXT," & _                                                                                                                                           
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "TEXT," & _                                                                                                                                           
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "," & _                                                                                                                                               
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                           
                                    "TEXT," & _                                                                                                                                            'defines the type of data to be loaded ie with apostrophes (or without) around the data to define it as text or numeric. Values may be TEXT or blank (for numbers)."


                .Constants("user") = "tabula"

                Dim t1() As String = { _
                        String.Format(CtrlForm.ItemValue("_SERIALNAME"),"M,CHAR,22,Work Order"), _                                                                                                        
                                String.Format("","M,CHAR,15,Part Number"), _                                                                                                          
                                String.Format("","CHAR,16,Operation"), _                                                                                                              
                                String.Format("","CHAR,6,Work Cell"), _                                                                                                               
                                String.Format("0","INT,16,0,Employee ID"), _                                                                                                          
                                String.Format("0","INT,17,3,Qty Completed"), _                                                                                                        
                                String.Format("0","INT,17,3,Qty Rejected"), _                                                                                                         
                                String.Format("","CHAR,3,Defect Code"), _                                                                                                             
                                String.Format(CtrlForm.ItemValue("_MQUANT"),"INT,17,3,Qty for MRB"), _                                                                                                          
                                String.Format("0","INT,17,3,Completed (Buy/Sell)"), _                                                                                                 
                                String.Format("0","INT,17,3,Rejected (Buy/Sell)"), _                                                                                                  
                                String.Format("0"),"INT,17,3,MRB (Buy/Sell Units)"), _                                                                                                                         
                                String.Format("0","INT,6,Packing Crates (No.)"), _                                                                                                    
                                String.Format("","CHAR,2,Packing Crate Code"), _                                                                                                      
                                String.Format("0","TIME,5,Start Time"), _                                                                                                             
                                String.Format("0","TIME,5,End Time"), _                                                                                                               
                                String.Format("0","TIME,6,Span"), _         
                                String.Format("0","TIME,5,Start Labor"), _                                                                                                            
                                String.Format("0","TIME,5,End Labor"), _                                                                                                              
                                String.Format("0","TIME,6,Labor Span"), _                                                                                                             
                                String.Format("","CHAR,8,Shift"), _                                                                                                                   
                                String.Format("","CHAR,15,Tool"), _                                                                                                                   
                                String.Format("","CHAR,1,Rework?"), _                                                                                                                 
                                String.Format("","CHAR,1,Set-up/Break (S/B)"), _                                                                                                      
                                String.Format(CtrlForm.ItemValue("WARHSNAME"),"CHAR,4,To Warehouse"), _                                                                                                            
                                String.Format(CtrlForm.ItemValue("LOCNAME"),"CHAR,14,Bin"), _                                                                                                                    
                                String.Format("","CHAR,1,New Pallet?"), _                                                                                                             
                                String.Format(CtrlForm.ItemValue("_TOPALLET"),"CHAR,16,To Pallet"), _                                                                                                              
                                String.Format("","CHAR,1,Remove Oper. Number?"), _                                                                                                    
                                String.Format("","CHAR,1,Remove Wk Order No.?"), _     
                        } 'Type1 records to load'
                .AddRecord(1) = t1
                        Dim t2() As String = { _
                        
                        } 'Type2 records to load'
               
                .AddRecord(2) = t2

              



              

                ws.LoadData(.ToSerial) ' send the serialised data from the loading to the loaddata method of the webservice

            End With

        End Sub

    End Module

End Module
