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

        Dim ws As New PriWebSVC.Service
        Dim erl As New Loading.DataLoad

        With erl

            .Clear()
            .Table = "ZSFDC_PRODREP"
            .Procedure = "ZSFDC_PRODREP"
            '.Environment = Environment

            .RecordType1 = "" & _
                "CURDATE," & _
                "FORMNAME," & _
                "SHIFTNAME," & _
                "DETAILS," & _
                "FINAL"

            .RecordType2 = "" & _
                "SERIALNAME," & _
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
                "SHIFTNAME2," & _
                "TOOLNAME," & _
                "RTYPEBOOL," & _
                "MODE," & _
                "WARHSNAME," & _
                "LOCNAME," & _
                "NEWPALLET," & _
                "TOPALLETNAME," & _
                "ACTCANCEL," & _
                "SERCANCEL"

            .RecordTypes = "" & _
                "," & _
                "TEXT," & _
                "TEXT," & _
                "TEXT," & _
                "TEXT," & _
                "TEXT," & _
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
                "TEXT," & _
                "TEXT," & _
                "," & _
                "TEXT," & _
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
                "TEXT"

            Dim t1() As String = { _
                String.Format("%DATE%", "DATE,8,Date"), _
                String.Format("", "CHAR,16,Form Number"), _
                String.Format("", "CHAR,8,Shift"), _
                String.Format("", "CHAR,24,Details"), _
                String.Format("", "CHAR,1,Final") _
                    }
            .AddRecord(1) = t1


            Dim t2() As String = { _
                String.Format("WO-004702-11", "CHAR,22,Work Order"), _
                String.Format("", "CHAR,15,Part Number"), _
                String.Format("", "CHAR,16,Operation"), _
                String.Format("", "CHAR,6,Work Cell"), _
                String.Format("0", "INT,16,Employee ID"), _
                String.Format("1000", "INT,17,Qty Completed"), _
                String.Format("0", "INT,17,Qty Rejected"), _
                String.Format("", "CHAR,3,Defect Code"), _
                String.Format("0", "INT,17,Qty for MRB"), _
                String.Format("0", "INT,17,Completed,Buy/Sell"), _
                String.Format("0", "INT,17,Rejected,Buy/Sell"), _
                String.Format("0", "INT,17,MRB,Buy/Sell Units"), _
                String.Format("0", "INT,13,Packing Crates,No."), _
                String.Format("", "CHAR,2,Packing Crate Code"), _
                String.Format("", "CHAR,5,Start Time"), _
                String.Format("", "CHAR,5,End Time"), _
                String.Format("0", "TIME,6,Span"), _
                String.Format("", "CHAR,5,Start Labor"), _
                String.Format("", "CHAR,5,End Labor"), _
                String.Format("0", "TIME,6,Labor Span"), _
                String.Format("", "CHAR,8,Type 2 Shift"), _
                String.Format("", "CHAR,15,Tool"), _
                String.Format("", "CHAR,1,Rework?"), _
                String.Format("", "CHAR,1,Set-up/Break,S/B"), _
                String.Format("", "CHAR,4,To Warehouse"), _
                String.Format("", "CHAR,14,Bin"), _
                String.Format("", "CHAR,1,New Pallet?"), _
                String.Format("", "CHAR,16,To Pallet"), _
                String.Format("", "CHAR,1,Remove Oper. Number?"), _
                String.Format("", "CHAR,1,Remove Wk Order No.?") _
            }
            .AddRecord(2) = t2

            ws.LoadData(.ToSerial)

        End With

    End Sub

End Module
