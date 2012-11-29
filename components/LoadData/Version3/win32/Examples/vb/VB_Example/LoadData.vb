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

        Dim Records() As String = {"A", "B", "C"}
        Dim Environment As String = "demo"

        Dim ws As New PriWebSVC.Service
        Dim erl As New Loading.DataLoad

        With erl

            .Clear()
            .Table = "ZEMG_W1LOADERR"
            .Procedure = "ZEMG_W1LOADERR"
            .Environment = Environment
            .RecordType1 = "USER"
            .RecordType2 = "IVNUM,CURDATE,CODE,MESSAGE"            
            .RecordTypes = "TEXT,TEXT,,TEXT,TEXT"

            .Constants("user") = "tabula"

            Dim t1() As String = { _
                    "%user%" _
                    }
            .AddRecord(1) = t1

            For Each Record As String In Records
                Dim t2() As String = { _
                    Record, _
                    "%Date%", _
                    "404", _
                    "Page not found." _
                        }
                .AddRecord(2) = t2
            Next

            ws.LoadData(.ToSerial)

        End With

    End Sub

End Module
