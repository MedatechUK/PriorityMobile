Imports PDA.o

Public Class ct_DateSelect

    Public Event OpenCall(ByVal DOCNO As String)
    Public Event StatusChange(ByVal DOCNO As String)
    Public Event CloseCall(ByVal DOCNO As String)

    Dim Statuses As PDA._Statuses = Nothing
    Dim _App As PDAOnBoardData.BaseForm

    Public Sub New(ByRef App As PDAOnBoardData.BaseForm)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        _App = App

    End Sub

    Private Sub ct_DateSelect_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim fs As Single

        If Me.Width > 319 Then
            fs = 14
        ElseIf Me.Width > 268 Then
            fs = 12
        ElseIf Me.Width > 258 Then
            fs = 11
        ElseIf Me.Width > 241 Then
            fs = 10
        ElseIf Me.Width > 214 Then
            fs = 9
        ElseIf Me.Width > 199 Then
            fs = 8
        Else
            fs = 8
        End If

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        CallList.Font = c
        DatePick.Font = f

        CallList.Height = Me.Height - (DatePick.Height)

        Try
            With CallList
                If .Items.Count = 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                End If
            End With
        Catch
        End Try

    End Sub

    Private Sub DatePick_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatePick.ValueChanged
        refreshCallList()
    End Sub

    Public Sub rDraw()
        refreshCallList("")
    End Sub

    Public Sub refreshCallList(Optional ByVal SelCall As String = "")

        Dim ServiceCall As _ServiceCall = _App.rss(o.ServiceCall)
        'RaiseEvent DataRequest(o, Statuses)

        Dim startdate As Date = CDate(Split(CStr(DatePick.Value), " ")(0))
        Dim thisDateStart As Integer = DateDiff(DateInterval.Minute, #1/1/1988#, startdate)
        Dim thisDateEnd As Integer = DateDiff(DateInterval.Minute, #1/1/1988#, DateAdd(DateInterval.Day, 1, startdate))

        With CallList
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("Doc Num", 150)
            .Columns.Add("Customer", 150)
            .Columns.Add("Status", 150)

        End With

        If Not IsNothing(ServiceCall.thisArray) Then
            For i As Integer = 0 To UBound(ServiceCall.thisArray, 2)
                ServiceCall.currentOrdinal = i
                Dim pdate As Integer = CInt(ServiceCall.GetField("PDATE"))
                Dim tdate As Date = DateAdd(DateInterval.Minute, CDbl(pdate), #1/1/1988#)
                If pdate >= thisDateStart And pdate < thisDateEnd Then
                    CallList.Items.Add(ServiceCall.GetField("DOCNO"))
                    For c As Integer = 0 To CallList.Items.Count - 1
                        If CallList.Items(c).Text = ServiceCall.GetField("DOCNO") Then
                            CallList.Items(c).SubItems.Add(ServiceCall.GetField("CUSTNAME"))
                            CallList.Items(c).SubItems.Add(ServiceCall.GetField("STATUS"))
                        End If
                    Next

                End If
            Next
        End If

        Try
            With CallList
                If .Items.Count = 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                End If
            End With
        Catch
        End Try

        For c As Integer = 0 To CallList.Items.Count - 1
            If CallList.Items(c).Text = SelCall Then
                CallList.Items(c).Selected = True
                Exit For
            End If
        Next

    End Sub

    Public Sub FirstOpen(ByVal sc As String)

        With _App.rss(o.Time)
            .currentIndex = sc
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = .currentIndex
                .thisArray(1, nr) = ""
                .thisArray(2, nr) = ""
                .thisArray(3, nr) = ""                
                .Save()
            End If

            If Not regex_Time.IsMatch(.GetField("ONROUTE")) Then
                If MsgBox("Set en-route to this call?", _
                                MsgBoxStyle.YesNo, "En-Route") = MsgBoxResult.Yes Then
                    .SetField("ONROUTE", t)
                    .Save()

                    Dim act As String = ""
                    With _App.rss(o.Actions)
                        For i As Integer = 0 To UBound(.thisArray, 2)
                            If .thisArray(1, i) = txt_SetTimeEnRoute Then
                                act = .thisArray(2, i)
                                Exit For
                            End If
                        Next
                    End With

                    ' Post the status Change
                    With _App.rss(o.ServiceCall)
                        .currentIndex = sc                        
                        If .Validate Then
                            RaiseEvent StatusChange(.currentIndex)
                            For l As Integer = 0 To CallList.Items.Count - 1
                                If CallList.Items(l).SubItems(0).Text = sc Then
                                    Me.CallList.Items(l).SubItems(2).Text = act
                                End If
                            Next
                            .SetField("STATUS", act)
                            .Save()
                            .doLoading(_App.rss(o.ServiceCall))
                        End If
                    End With

                End If
            End If

        End With

    End Sub

    Public Function IsOnsite(ByVal sc As String) As Boolean
        Dim act As String = ""
        With _App.rss(o.Actions)
            For i As Integer = 0 To UBound(.thisArray, 2)
                If .thisArray(1, i) = txt_SetTimeOnSite Then
                    act = .thisArray(2, i)
                    Exit For
                End If
            Next
        End With
        Return _App.rss(o.ServiceCall).GetField("STATUS") = act
    End Function

    Private Sub CallList_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CallList.MouseUp

        Dim s As ListView.SelectedListViewItemCollection = CallList.SelectedItems
        If s.Count = 0 Then Exit Sub

        Dim selectedtext As String = s.Item(0).Text

        _App.rss(o.ServiceCall).currentIndex = selectedtext
        Dim CurrentState As String = Trim(_App.rss(o.ServiceCall).GetField("STATUS"))

        If s.Count > 0 Then
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Right
                    'RaiseEvent RightClicked(s(0).Text)
                    Dim menu As New ContextMenuStrip

                    With menu
                        For y As Integer = 0 To UBound(_App.rss(o.Statuses).thisArray, 2)
                            _App.rss(o.Statuses).currentOrdinal = y

                            Select Case StatusAct(CurrentState)
                                Case txt_IsReIssue
                                    If StatusAct(_App.rss(o.Statuses).thisArray(0, y)) = txt_SetTimeEnRoute Then
                                        .Items.Add("")
                                        .Items(.Items.Count - 1).Font = Me.CallList.Font
                                        .Items(.Items.Count - 1).Text = _App.rss(o.Statuses).GetField("STATUS")
                                        .Items(.Items.Count - 1).Name = _App.rss(o.Statuses).GetField("STATUS")
                                        AddHandler .Items(.Items.Count - 1).Click, AddressOf Change_State
                                        If .Items(.Items.Count - 1).Name = CurrentState Then
                                            .Items(.Items.Count - 1).BackColor = Color.LightSkyBlue
                                        End If
                                    End If

                                Case txt_SetTimeEnRoute
                                    If StatusAct(_App.rss(o.Statuses).thisArray(0, y)) = txt_SetTimeOnSite Then
                                        .Items.Add("")
                                        .Items(.Items.Count - 1).Font = Me.CallList.Font
                                        .Items(.Items.Count - 1).Text = _App.rss(o.Statuses).GetField("STATUS")
                                        .Items(.Items.Count - 1).Name = _App.rss(o.Statuses).GetField("STATUS")
                                        AddHandler .Items(.Items.Count - 1).Click, AddressOf Change_State
                                        If .Items(.Items.Count - 1).Name = CurrentState Then
                                            .Items(.Items.Count - 1).BackColor = Color.LightSkyBlue
                                        End If
                                    End If

                                Case txt_SetTimeOnSite
                                    If StatusAct(_App.rss(o.Statuses).thisArray(0, y)) = txt_SetTimeFinished Then
                                        .Items.Add("")
                                        .Items(.Items.Count - 1).Font = Me.CallList.Font
                                        .Items(.Items.Count - 1).Text = _App.rss(o.Statuses).GetField("STATUS")
                                        .Items(.Items.Count - 1).Name = _App.rss(o.Statuses).GetField("STATUS")
                                        AddHandler .Items(.Items.Count - 1).Click, AddressOf Change_State
                                        If .Items(.Items.Count - 1).Name = CurrentState Then
                                            .Items(.Items.Count - 1).BackColor = Color.LightSkyBlue
                                        End If
                                    End If
                            End Select



                        Next
                    End With
                    Me.CallList.ContextMenuStrip = menu

                Case Windows.Forms.MouseButtons.Left
                    RaiseEvent OpenCall(s(0).Text)
            End Select
        End If

    End Sub

    Sub Change_State(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim s As ListView.SelectedListViewItemCollection = CallList.SelectedItems
        If s.Count = 0 Then Exit Sub
        Dim selectedIndex As Integer = s(0).Index
        Dim selectedtext As String = s.Item(0).Text

        _App.rss(o.ServiceCall).currentIndex = s.Item(0).Text
        Dim CurrentState As String = Trim(_App.rss(o.ServiceCall).GetField("STATUS"))

        If sender.name = CurrentState Then Exit Sub

        With _App.rss(o.Actions)
            For i As Integer = 0 To UBound(.thisArray, 2)
                If .thisArray(2, i) = sender.name Then
                    Select Case .thisArray(1, i)
                        Case txt_SetTimeEnRoute
                            With _App.rss(o.Time)
                                .currentIndex = s.Item(0).Text
                                If Not .Validate Then
                                    Dim nr As Integer = .NewRecord
                                    .thisArray(0, nr) = .currentIndex
                                    .thisArray(1, nr) = ""
                                    .thisArray(2, nr) = ""
                                    .thisArray(3, nr) = ""
                                    .SetField("ONROUTE", t)
                                    .Save()
                                Else
                                    If Len(.GetField("ONROUTE")) = 0 Then
                                        .SetField("ONROUTE", t)
                                        .Save()
                                    Else
                                        If MsgBox("This Service Call already has an En-Route time" & _
                                        " of " & .GetField("ONROUTE") & _
                                        vbCrLf & "Do you wish to overwrite?", _
                                        MsgBoxStyle.YesNo, "En-Route") = MsgBoxResult.Yes Then
                                            .SetField("ONROUTE", t)
                                            .Save()
                                        End If
                                    End If
                                End If
                            End With

                        Case txt_SetTimeOnSite
                            With _App.rss(o.Time)
                                .currentIndex = s.Item(0).Text
                                If Not .Validate Then
                                    Dim nr As Integer = .NewRecord
                                    .thisArray(0, nr) = .currentIndex
                                    .thisArray(1, nr) = ""
                                    .thisArray(2, nr) = ""
                                    .thisArray(3, nr) = ""
                                    .SetField("ONSITE", t)
                                    .Save()
                                Else
                                    If Len(.GetField("ONSITE")) = 0 Then
                                        .SetField("ONSITE", t)
                                        .Save()
                                    Else
                                        If MsgBox("This Service Call already has an On-Site time" & _
                                        " of " & .GetField("ONSITE") & _
                                        vbCrLf & "Do you wish to overwrite?", _
                                        MsgBoxStyle.YesNo, "On-Site") = MsgBoxResult.Yes Then
                                            .SetField("ONSITE", t)
                                            .Save()
                                        End If
                                    End If
                                End If
                            End With

                        Case txt_SetTimeFinished
                            With _App.rss(o.Time)
                                .currentIndex = s.Item(0).Text
                                If Not .Validate Then
                                    Dim nr As Integer = .NewRecord
                                    .thisArray(0, nr) = .currentIndex
                                    .thisArray(1, nr) = ""
                                    .thisArray(2, nr) = ""
                                    .thisArray(3, nr) = ""
                                    .SetField("END", t)
                                    .Save()
                                Else
                                    If Len(.GetField("END")) = 0 Then
                                        .SetField("END", t)
                                        .Save()
                                    Else
                                        If MsgBox("This Service Call already has an End time" & _
                                        " of " & .GetField("END") & _
                                        vbCrLf & "Do you wish to overwrite?", _
                                        MsgBoxStyle.YesNo, "End Call") = MsgBoxResult.Yes Then
                                            .SetField("END", t)
                                            .Save()
                                        End If
                                    End If
                                End If
                            End With

                        Case txt_PostData
                            RaiseEvent CloseCall(_App.rss(o.ServiceCall).currentIndex)

                    End Select
                End If
            Next
        End With

        ' Post the status Change
        With _App.rss(o.ServiceCall)
            .currentIndex = selectedtext
            RaiseEvent StatusChange(.currentIndex)
            If .Validate Then
                Dim i As Integer = selectedIndex
                Me.CallList.Items(i).SubItems(2).Text = sender.name
                .SetField("STATUS", sender.NAME)
                .Save()
                .doLoading(_App.rss(o.ServiceCall))
            End If
        End With

    End Sub

    Private Function t() As String
        Return _
        Strings.Right("00" & CStr(DatePart(DateInterval.Hour, Now())), 2) & _
        ":" & _
        Strings.Right("00" & CStr(DatePart(DateInterval.Minute, Now())), 2)
    End Function

    Private Function StatusAct(ByVal Status As String) As String
        Dim ret As String = ""
        With _App.rss(o.Actions)
            For i As Integer = 0 To UBound(.thisArray, 2)
                If .thisArray(2, i) = Status Then
                    ret = .thisArray(1, i)
                    Exit For
                End If
            Next
        End With
        Return ret
    End Function

End Class
