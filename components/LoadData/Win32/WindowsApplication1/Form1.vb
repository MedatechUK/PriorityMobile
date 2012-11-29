Imports Priority.Load
Imports Priority.SerialData
Imports System.Array

Public Class Form1

    Public mCol() As Priority.tCol

    Private Sub PopulateTableToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PopulateTableToolStripMenuItem1.Click

        Dim hdr() = Split("PARTNAME,WARHS,LOC,TQUANT,TRANS", ",")
        For n As Integer = 0 To UBound(hdr)

            Dim c As New Priority.tCol
            With c
                .Name = hdr(n)
                .Title = hdr(n)
                .initWidth = 30
            End With
            AddCol(c)

        Next

        For n As Integer = 0 To 5
            Table.Items.Add("PART" & CStr(n))
            For i As Integer = 1 To UBound(hdr)
                Table.Items(n).SubItems.Add("PART" & CStr(n) & " " & hdr(i))
            Next
        Next

    End Sub

    Private Sub AddCol(ByVal Col As Priority.tCol)

        Try
            ReDim Preserve mCol(UBound(mCol) + 1)
        Catch ex As Exception
            ReDim mCol(0)
        End Try

        mCol(UBound(mCol)) = Col
        Dim pcentWid As Integer = ((Col.initWidth / 100) * Table.Width)
        Table.Columns.Add(Col.Title, pcentWid, HorizontalAlignment.Left)

    End Sub

    Private Sub MakeDatagramToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MakeDatagramToolStripMenuItem.Click

        Dim p As New Priority.Load
        Dim sd As New Priority.SerialData
        Dim ws As New priwebsvc.Service

        With p
            .DebugFlag = True
            .Procedure = "ZATG_DOTX_LOAD"
            .Table = "ZATG_LOAD_DOTX"
            .RecordType1 = "DOCNO,STATDES"
            .RecordType2 = "PARTNAME,WARHS,LOC,TQUANT,TRANS"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,,"
        End With

        ' Type 1 records
        Dim t1() As String = {"GRV1234", "FINAL"}
        p.AddRecord(1) = t1

        ' Type 2 records
        For y As Integer = 0 To Table.Items.Count - 1

            Dim t2() As String = { _
                Table.Items(y).SubItems(0).Text, _
                Table.Items(y).SubItems(1).Text, _
                Table.Items(y).SubItems(2).Text, _
                Table.Items(y).SubItems(3).Text, _
                Table.Items(y).SubItems(4).Text _
            }
            p.AddRecord(2) = t2

        Next

        Try
            ws.LoadDataAsync(sd.SerialiseDataArray(p.Data))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

End Class
