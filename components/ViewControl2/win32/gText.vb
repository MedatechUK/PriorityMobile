Public Class gtext

    Private g As Graphics = Me.CreateGraphics
    Private _mytext As String = ""
    Private np As New Dictionary(Of String, String)
    Private lastW As Integer = 0
    Private LastH As Integer = 0

    Public WriteOnly Property HTML() As String
        Set(ByVal value As String)

            Dim tags() As String = {"p", "/p", "br"}

            Dim tmp As String = value
            If InStr(value, "</style>") > 0 Then tmp = Split(value, "</style>", , CompareMethod.Text)(1)

            For i As Integer = 0 To UBound(tags)
                tmp = System.Text.RegularExpressions.Regex.Replace(tmp, "<[" & tags(i).ToLower & "|" & tags(i).ToUpper & "][^>]*>", vbCrLf)
            Next

            tmp = System.Text.RegularExpressions.Regex.Replace(tmp, "<[^>]*>", "")
            tmp = Replace(tmp, "&nbsp;", " ")

            While Strings.InStr(tmp, vbCrLf & " ", CompareMethod.Text) > 0
                tmp = Replace(tmp, vbCrLf & " ", vbCrLf)
            End While
            While Strings.InStr(tmp, " " & vbCrLf, CompareMethod.Text) > 0
                tmp = Replace(tmp, " " & vbCrLf, vbCrLf)
            End While
            While Strings.InStr(tmp, vbCrLf & vbCrLf & vbCrLf, CompareMethod.Text) > 0
                tmp = Replace(tmp, vbCrLf & vbCrLf & vbCrLf, vbCrLf & vbCrLf)
            End While
            While Strings.Left(tmp, 2) = vbCrLf
                tmp = Strings.Right(tmp, tmp.Length - 2)
            End While
            While Strings.Right(tmp, 2) = vbCrLf
                tmp = Strings.Left(tmp, tmp.Length - 2)
            End While

            _mytext = tmp
            'PARSE()
        End Set
    End Property

    Public Sub AddNamePair(ByVal Name As String, ByVal Value As String)
        If np.ContainsKey(Name) Then
            np(Name) = Value
        Else
            np.Add(Name, Value)
        End If
    End Sub

    Private Sub UserControl1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If Not IsNothing(_mytext) Then
            If _mytext.Length = 0 And np.Count = 0 Then Exit Sub
            If (LastH <> Me.Height Or lastW <> Me.Width) And (List.Width > 0 And List.Height > 0) Then
                PARSE()
                LastH = Me.Height
                lastW = Me.Width
            End If
        End If
    End Sub

    Public Sub PARSE()
        List.Items.Clear()
        If Not (List.Width > 0 And List.Height > 0) Then Exit Sub
        If Not IsNothing(_mytext) Then
            If _mytext.Length = 0 And np.Count = 0 Then Exit Sub

            With List

                .Items.Clear()
                .Columns.Clear()

                .Columns.Add("")

                .Width = Me.Width '- 10
                .Height = Me.Height ' - 10                
                .Columns(0).Width = .Width - 25

                For Each key As String In np.Keys
                    AddToList(Me.List, key & ":")
                    .Items(.Items.Count - 1).Font = New Font(.Font, FontStyle.Bold)
                    Dim l() As String = Split(np(key), vbCrLf)
                    For i As Integer = 0 To UBound(l)
                        AddToList(Me.List, l(i))
                    Next
                    AddToList(Me.List, "")
                Next

                If _mytext.Length = 0 Then Exit Sub

                Dim cr() As String = Split(_mytext, vbCrLf)
                Dim charWid As Integer = GetStringWidth("_")

                For i As Integer = 0 To UBound(cr)
                    Dim build As String = ""
                    If cr(i).Length > 0 Then 'And Strings.InStr(cr(i), " ") > 0
                        Dim sp() As String = Split(cr(i), " ")
                        Dim s As Integer = 0

                        While s <= UBound(sp)
                            While s <= UBound(sp)
                                Dim b As String = " " & build & sp(s)
                                If (charWid * Strings.Len(b)) < Me.List.Columns(0).Width Then
                                    build += " " & sp(s)
                                    s += 1
                                Else
                                    If Len(build) = 0 Then
                                        build += " " & sp(s)
                                        s += 1
                                    End If
                                    Exit While
                                End If
                            End While

                            While s <= UBound(sp)
                                If (GetStringWidth("  " & build & sp(s)) < Me.List.Columns(0).Width) Or _
                                    (build.Length = 0 And GetStringWidth(sp(s)) > Me.List.Columns(0).Width) Then
                                    build += " " & sp(s)
                                    s += 1
                                Else
                                    Exit While
                                End If
                            End While

                            AddToList(Me.List, build)
                            build = ""

                        End While

                    Else
                        AddToList(Me.List, build)
                    End If

                    If build.Length > 0 Then AddToList(Me.List, build)

                Next

            End With
        End If
    End Sub

    Private Sub AddToList(ByVal List As ListView, ByVal Str As String)
        If Len(Str) > 0 Then
            Dim lvi As New ListViewItem()
            lvi.Text = Trim(Str)
            With List
                .Items.Add(lvi)
            End With
        End If
    End Sub

    Public Function GetStringWidth(ByVal sStr As String) As Single
        Return g.MeasureString(sStr, Me.List.Font).Width()
    End Function

    Private Sub List_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles List.SelectedIndexChanged
        For Each i As Integer In List.SelectedIndices
            With List.Items(i)
                .Selected = False
                .Focused = False
            End With
        Next
    End Sub
End Class
