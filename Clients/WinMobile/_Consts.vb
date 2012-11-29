Imports System.Text.RegularExpressions

Module _Consts

    Public _Warnings(,) As String = Nothing
    Public _dispWarn As Boolean = False

    Public regex_WebService As Regex = New Regex("http://.*\..*\.asmx")
    Public regex_String As Regex = New Regex("^[a-zA-Z0-9]+$")
    Public regex_Time As Regex = New Regex("^[0-9][0-9]\:[0-9][0-9]$")

    Public Const num_Actions As Integer = 5
    Public Const txt_SetTimeEnRoute As String = "Set Time En-Route"
    Public Const txt_SetTimeOnSite As String = "Set Time On-Site"
    Public Const txt_SetTimeFinished As String = "Set Time Finished"
    Public Const txt_PostData As String = "Post Data"
    Public Const txt_IsReIssue As String = "Re-Issued"


    Public Enum o
        ServiceCall = 0
        Warehouse = 1
        Statuses = 2
        Details = 3
        Malfunction = 4
        Resolution = 5
        Survey = 6
        Repair = 7
        Time = 8
        Parts = 9
        Signature = 10
        Answers = 11
        Flags = 12
        Actions = 13
        Cancel = 14
        DayEnd = 15
    End Enum

    Public Function DatetoMin() As String
        Return CStr(DateDiff(DateInterval.Minute, CDate("01/01/1988"), Now()))
    End Function

    Public Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

    Public Function GetFontSize(ByVal Width As Integer) As Single
        If Width > 319 Then
            Return 14
        ElseIf Width > 268 Then
            Return 12
        ElseIf Width > 258 Then
            Return 11
        ElseIf Width > 241 Then
            Return 10
        ElseIf Width > 214 Then
            Return 9
        ElseIf Width > 199 Then
            Return 8
        Else
            Return 8
        End If
    End Function

#Region "Re-written Functions not supported by CF"
    Public Function TextSize(ByVal Str As String, _
          ByVal f As Font) As SizeF

        Dim b As Bitmap
        Dim g As Graphics

        ' Compute the string dimensions in the given font
        b = New Bitmap(1, 1)
        g = Graphics.FromImage(b)
        Return g.MeasureString(Str & "_", f)

        g.Dispose()
        b.Dispose()

    End Function

    Public Function SelectedItems(ByVal lv As ListView) As Integer()
        Dim ret() As Integer = Nothing
        For i As Integer = 0 To lv.SelectedIndices.Count - 1
            Try
                ReDim Preserve ret(UBound(ret) + 1)
            Catch ex As Exception
                ReDim ret(0)
            Finally
                ret(UBound(ret)) = lv.SelectedIndices(i)
            End Try
        Next
        Return ret
    End Function

    Public Sub Remove(ByRef lv As ListView, ByVal index As Integer)
        Dim tmp As New ListView
        For i As Integer = 0 To lv.Items.Count - 1
            If index <> i Then
                Dim lvi As New ListViewItem
                lvi.Text = lv.Items(i).Text
                For si As Integer = 0 To lv.Items(i).SubItems.Count - 1
                    lvi.SubItems.Add("")
                    lvi.SubItems(si).Text = lv.Items(i).SubItems(si).Text
                Next
                tmp.Items.Add(lvi)
            End If
        Next
        For i As Integer = 0 To lv.Columns.Count - 1
            Dim ch As New ColumnHeader
            tmp.Columns.Add(ch)
            tmp.Columns(i).Text = lv.Columns(i).Text
            tmp.Columns(i).Width = lv.Columns(i).Width
        Next
        lv.Clear()
        For i As Integer = 0 To tmp.Columns.Count - 1
            Dim ch As New ColumnHeader
            lv.Columns.Add(ch)
            lv.Columns(i).Text = tmp.Columns(i).Text
            lv.Columns(i).Width = tmp.Columns(i).Width
        Next
        For i As Integer = 0 To tmp.Items.Count - 1
            Dim lvi As New ListViewItem
            lvi.Text = tmp.Items(i).Text
            For si As Integer = 0 To tmp.Items(i).SubItems.Count - 1
                lvi.SubItems.Add("")
                lvi.SubItems(si).Text = tmp.Items(i).SubItems(si).Text
            Next
            lv.Items.Add(lvi)
        Next
    End Sub

    Public Sub AutoSizeListView(ByRef lv As ListView, ByVal MyWidth As Integer, ByVal ShrinkCol As Integer)

        If lv.Columns.Count > 0 Then

            Dim ins As Integer = lv.Columns.Count - 1
            If ins > 10 Then ins = 10
            Dim used As Integer = 0
            Dim wid(ins) As Integer
            Dim maxwidth(ins) As Integer
            Dim colwidth(ins) As Integer
            For col As Integer = 0 To ins
                colwidth(col) = TextSize(lv.Columns.Item(col).Text, lv.Font).Width
                For row As Integer = 0 To lv.Items.Count - 1
                    Dim ts As Integer = TextSize(lv.Items(row).SubItems(col).Text & "__", lv.Font).Width
                    If ts > maxwidth(col) Then
                        maxwidth(col) = ts
                    End If
                Next
            Next
            For col As Integer = 0 To ins
                If col <> ShrinkCol Then
                    If maxwidth(col) >= colwidth(col) Then
                        wid(col) = maxwidth(col)
                        used = used + maxwidth(col)
                    Else
                        wid(col) = colwidth(col)
                        used = used + colwidth(col)
                    End If
                End If
            Next
            wid(ShrinkCol) = MyWidth - used
            For col As Integer = 0 To ins
                lv.Columns(col).Width = wid(col)
            Next
        End If


    End Sub

    Public Sub AutoSizeLabel(ByRef lbl As Label, ByVal MaxWid As Integer)

        Dim word() As String = Nothing
        Dim lines() As String = Nothing
        Dim testtext As String = ""

        word = Split(lbl.Text, " ")
        testtext = testtext & word(0)
        For i As Integer = 1 To UBound(word)
            Select Case TextSize(testtext & " " & word(i), lbl.Font).Width
                Case Is > MaxWid
                    Try
                        ReDim Preserve lines(UBound(lines) + 1)
                    Catch
                        ReDim lines(0)
                    End Try
                    lines(UBound(lines)) = testtext
                    testtext = word(i)
                Case Else
                    testtext = testtext & " " & word(i)
            End Select
        Next

        Try
            ReDim Preserve lines(UBound(lines) + 1)
        Catch
            ReDim lines(0)
        End Try
        lines(UBound(lines)) = testtext

        testtext = ""
        For i As Integer = 0 To UBound(lines)
            testtext = testtext + lines(i) & vbCrLf
        Next

        With lbl
            .Text = testtext
            .Height = (TextSize("Test", lbl.Font).Height + 1) * (UBound(lines) + 1)
            .Width = maxwid
        End With

    End Sub

#End Region

End Module
