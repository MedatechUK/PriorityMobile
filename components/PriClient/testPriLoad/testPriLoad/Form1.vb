Imports System.Data.SqlClient

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ev As New ntEvtlog.evt
        With ev
            .LogMode = ntEvtlog.EvtLogMode.EventLog
            .LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane
            .AppName = "Priority SOAP Service"
            .RegisterLog()
        End With
        Dim p As New Priority.Loading
        With p
            .SetEventLog(ev)
            .Procedure = "ZSFDC_TEST"
            .Table = "GENERALLOAD"
            .RecordType1 = "TEXT1, TEXT2"
            .RecordType2 = "TEXT3, INT1"
            .RecordTypes = "TEXT,TEXT,TEXT,"


            ' Type 1 records
            Dim t1() As String = { _
                                "Text One", _
                                "Text Two" _
                                }
            .AddRecord(1) = t1
            ' Type 1 records
            Dim t2() As String = { _
                                "Text three", _
                                "1" _
                                }
            .AddRecord(2) = t2
            Dim ts = .ToSerial
            .FromSerial(ts)
            Dim temp = .Data

            .Load("Data Source=DUTCHESS\PRI;Initial Catalog=demo;User ID=web;Password=Tabula!", "c:\")

        End With
    End Sub
End Class
