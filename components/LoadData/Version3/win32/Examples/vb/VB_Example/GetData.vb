'---------------------------------------------------------------------
'  Copyright (C) eMerge-IT.  All rights reserved.
' 
'THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
'KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
'IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
'PARTICULAR PURPOSE.
'---------------------------------------------------------------------

Module GetData

    Sub Main()
        Dim Environment As String = "demo"
        Dim ws As New PriWebSVC.Service
        Dim sd As New priority.SerialData
        Dim SQL As String = _
                "SELECT W1URL, W1USER, W1PASS, W1STKEY, " & _
                "ACCOUNTS.ACCNAME AS ACCNAME" & _
                "FROM COMPDATA, ACCOUNTS " & _
                "WHERE COMP <> 0" & _
                "AND COMPDATA.W1ACCOUNT = ACCOUNTS.ACCOUNT"
        Dim cd As New priority.ColumnDef(SQL)
        With sd
            .FromStr(ws.GetData _
                ( _
                    SQL, _
                    Environment _
                ) _
            )
            If IsNothing(.GetDataError) Then
                If Not IsNothing(.Data) Then
                    For i As Integer = 0 To .RowCount - 1
                        Console.Write( _
                            .Data(cd("W1URL"), i), _
                            .Data(cd("W1USER"), i), _
                            .Data(cd("W1PASS"), i), _
                            .Data(cd("W1STKEY"), i), _
                            .Data(cd("ACCNAME"), i) _
                        )
                    Next
                Else
                    Throw New Exception("No data.")
                End If
            Else
                Throw New Exception(.GetDataError.Message)
            End If
        End With
    End Sub

End Module
