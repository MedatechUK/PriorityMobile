
'---------------------------------------------------------------------
'  Copyright (C) eMerge-IT.  All rights reserved.
' 
'THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
'KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
'IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
'PARTICULAR PURPOSE.
'---------------------------------------------------------------------

Module WHSASSY

    Sub Main()

        Dim ws As New PriWebSVC.Service
        Dim erl As New Loading.DataLoad

        With erl

            .Clear()
            .Table = "ZSFDC_WHSASSY_LOAD"
            .Procedure = "ZSFDC_LOAD_WHSASSY"
            '.Environment = Environment

            .RecordType1 = "" & _
                "CUSTNAME," & _
                "CURDATE," & _
                "ASSCODESTATUS," & _
                "WARHSNAME," & _
                "LOCNAME," & _
                "BOOKNUM," & _
                "DETAILS," & _
                "ORDNAME," & _
                "BYUSERNAME," & _
                "BYUSERNAME1," & _
                "BYUSERNAME2"

            .RecordType2 = "" & _
                "PARTNAME," & _
                "TQUANT," & _
                "TUNITNAME," & _
                "SETFLAG," & _
                "CUSTNAME2," & _
                "REVNAME," & _
                "ORDNAME2," & _
                "PQUANT," & _
                "QUANT," & _
                "NUMPACK," & _
                "PACKCODE," & _
                "TOACTNAME"

            .RecordTypes = "" & _
                "TEXT," & _
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
                "TEXT," & _
                "," & _
                "TEXT," & _
                "TEXT," & _
                "TEXT," & _
                "TEXT," & _
                "TEXT," & _
                "," & _
                "," & _
                "," & _
                "TEXT," & _
                "TEXT"

            Dim t1() As String = { _
                String.Format("", "CHAR,16,Customer Number"), _
                String.Format("%DATE%", "M,DATE,8,Date"), _
                String.Format("", "M,CHAR,12,Assembly Status"), _
                String.Format("CEL1", "M,CHAR,4,Warehouse"), _
                String.Format("0", "M,CHAR,14,Bin"), _
                String.Format("LH01234", "CHAR,16,External Doc. Number"), _
                String.Format("", "CHAR,24,Details"), _
                String.Format("", "CHAR,16,Order"), _
                String.Format("sean.meeten", "M,CHAR,20,Assigned to"), _
                String.Format("", "CHAR,20,Additional Worker"), _
                String.Format("", "CHAR,20,Third Worker") _
                    }
            .AddRecord(1) = t1


            Dim t2() As String = { _
                String.Format("CSP-11-1102", "M,CHAR,15,Part Number"), _
                String.Format("10", "INT,17,Assembled Qty"), _
                String.Format("", "CHAR,3,Unit"), _
                String.Format("", "CHAR,1,Auto. Record Child?"), _
                String.Format("Goods", "M,CHAR,16,Status"), _
                String.Format("", "CHAR,10,Part Revision No."), _
                String.Format("", "CHAR,16,Order"), _
                String.Format("0", "INT,17,Required Qty (FactU)"), _
                String.Format("0", "INT,17,Assembled Qty(FactU)"), _
                String.Format("0", "INT,6,Packing Crates (No.)"), _
                String.Format("", "CHAR,2,Packing Crate Code"), _
                String.Format("", "CHAR,16,To Pallet") _
            }
            .AddRecord(2) = t2

            ws.LoadData(.ToSerial)

        End With

    End Sub

End Module
