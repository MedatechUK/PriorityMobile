Imports cfpda.o

Public Class ct_DateSelect

    Public Event OpenCall(ByVal DOCNO As String)
    Public Event StatusChange(ByVal DOCNO As String)
    Public Event CloseCall(ByVal DOCNO As String)

    'Dim Statuses As sfPDA._Statuses = Nothing
    Dim CallerApp As cfOnBoardData.BaseForm

    Public Sub New(ByRef App As cfOnBoardData.BaseForm)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        CallerApp = App

    End Sub

    Private Sub ct_DateSelect_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim fs As Single = GetFontSize(Me.Width)

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        CallList.Font = c
        DatePick.Font = f

        CallList.Height = Me.Height - (DatePick.Height)
        AutoSizeListView(CallList, Me.Width, 1)

    End Sub

    Private Sub DatePick_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatePick.ValueChanged
        refreshCallList()
    End Sub

    Public Sub rDraw()
        refreshCallList("")
    End Sub

    Public Sub refreshCallList(Optional ByVal SelCall As String = "")

        Dim ServiceCall As cfServiceCall = CallerApp.rss(o.ServiceCall)
        'RaiseEvent DataRequest(o, Statuses)

        Dim startdate As Date = CDate(Split(CStr(DatePick.Value), " ")(0))
        Dim thisDateStart As Integer = DateDiff(DateInterval.Minute, #1/1/1988#, startdate)
        Dim thisDateEnd As Integer = DateDiff(DateInterval.Minute, #1/1/1988#, DateAdd(DateInterval.Day, 1, startdate))

        Dim ch1 As New ColumnHeader
        Dim ch2 As New ColumnHeader
        Dim ch3 As New ColumnHeader
        With CallList
            .Items.Clear()
            .Columns.Clear()
            ch1.Text = "Doc Num"
            .Columns.Add(ch1)
            ch2.Text = "Customer"
            .Columns.Add(ch2)
            ch3.Text = "Status"
            .Columns.Add(ch3)
        End With

        If Not IsNothing(ServiceCall.thisArray) Then
            For i As Integer = 0 To UBound(ServiceCall.thisArray, 2)
                ServiceCall.currentOrdinal = i
                Dim pdate As Integer = CInt(ServiceCall.GetField("PDATE"))
                Dim tdate As Date = DateAdd(DateInterval.Minute, CDbl(pdate), #1/1/1988#)
                If pdate >= thisDateStart And pdate < thisDateEnd Then
                    Dim lvi As New ListViewItem
                    lvi.Text = ServiceCall.GetField("DOCNO")
                    CallList.Items.Add(lvi)
                    For c As Integer = 0 To CallList.Items.Count - 1
                        If CallList.Items(c).Text = ServiceCall.GetField("DOCNO") Then
                            CallList.Items(c).SubItems.Add(ServiceCall.GetField("CUSTNAME"))
                            CallList.Items(c).SubItems.Add(ServiceCall.GetField("STATUS"))
                        End If
                    Next

                End If
            Next
        End If

        AutoSizeListView(CallList, Me.Width, 1)

        For c As Integer = 0 To CallList.Items.Count - 1
            If CallList.Items(c).Text = SelCall Then
                CallList.Items(c).Selected = True
                Exit For
            End If
        Next

    End Sub

    Public Sub FirstOpen(ByVal sc As String)

        With CallerApp.rss(o.Time)
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
                    With CallerApp.rss(o.Actions)
                        For i As Integer = 0 To UBound(.thisArray, 2)
                            If .thisArray(1, i) = txt_SetTimeEnRoute Then
                                act = .thisArray(2, i)
                                Exit For
                            End If
                        Next
                    End With

                    ' Post the status Change
                    With CallerApp.rss(o.ServiceCall)
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
                            .doLoading(CallerApp.rss(o.ServiceCall))
                        End If
                    End With

                End If
            End If

        End With

    End Sub

    Public Function IsOnsite(ByVal sc As String) As Boolean
        Dim act As String = ""
        With CallerApp.rss(o.Actions)
            For i As Integer = 0 To UBound(.thisArray, 2)
                If LCase(Trim(.thisArray(1, i))) = LCase(Trim(txt_SetTimeOnSite)) Then
                    act = .thisArray(2, i)
                    Exit For
                End If
            Next
        End With
        Return LCase(Trim(CallerApp.rss(o.ServiceCall).GetField("STATUS"))) = LCase(Trim(act))
    End Function

    Private Sub CallList_MouseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles CallList.SelectedIndexChanged

        Dim s() As Integer = SelectedItems(CallList) 'ListView.SelectedListViewItemCollection = CallList.SelectedItems
        If IsNothing(s) Then Exit Sub

        Dim selectedtext As String = CallList.Items(s(0)).Text

        CallerApp.rss(o.ServiceCall).currentIndex = selectedtext
        Dim CurrentState As String = Trim(CallerApp.rss(o.ServiceCall).GetField("STATUS"))

        If Not IsNothing(s) > 0 Then

            'RaiseEvent RightClicked(s(0).Text)
            Dim menu As New ContextMenu
            Dim mi As New MenuItem

            With menu

                .MenuItems.Add(mi)
                .MenuItems(.MenuItems.Count - 1).Text = "Open"
                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf State_Open

                For y As Integer = 0 To UBound(CallerApp.rss(o.Statuses).thisArray, 2)

                    CallerApp.rss(o.Statuses).currentOrdinal = y

                    Select Case StatusAct(CurrentState)
                        Case txt_IsReIssue
                            If StatusAct(CallerApp.rss(o.Statuses).thisArray(0, y)) = txt_SetTimeEnRoute Then
                                Dim tempmi As New MenuItem
                                .MenuItems.Add(tempmi)
                                '.MenuItems(.MenuItems.Count - 1).Font = Me.CallList.Font
                                .MenuItems(.MenuItems.Count - 1).Text = CallerApp.rss(o.Statuses).GetField("STATUS")
                                '.MenuItems(.MenuItems.Count - 1).Name = CallerApp.rss(o.Statuses).GetField("STATUS")
                                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf Change_State
                                'If .MenuItems(.MenuItems.Count - 1).Text = CurrentState Then
                                '    .MenuItems(.MenuItems.Count - 1).BackColor = Color.LightSkyBlue
                                'End If
                            End If

                        Case txt_SetTimeEnRoute
                            If StatusAct(CallerApp.rss(o.Statuses).thisArray(0, y)) = txt_SetTimeOnSite Then
                                Dim tempmi As New MenuItem
                                .MenuItems.Add(tempmi)
                                '.Items(.Items.Count - 1).Font = Me.CallList.Font
                                .MenuItems(.MenuItems.Count - 1).Text = CallerApp.rss(o.Statuses).GetField("STATUS")
                                '.Items(.Items.Count - 1).Name = CallerApp.rss(o.Statuses).GetField("STATUS")
                                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf Change_State
                                'If .Items(.Items.Count - 1).Name = CurrentState Then
                                '    .Items(.Items.Count - 1).BackColor = Color.LightSkyBlue
                                'End If
                            End If

                        Case txt_SetTimeOnSite
                            If StatusAct(CallerApp.rss(o.Statuses).thisArray(0, y)) = txt_SetTimeFinished Then
                                Dim tempmi As New MenuItem
                                .MenuItems.Add(tempmi)
                                '.Items(.Items.Count - 1).Font = Me.CallList.Font
                                .MenuItems(.MenuItems.Count - 1).Text = CallerApp.rss(o.Statuses).GetField("STATUS")
                                '.Items(.Items.Count - 1).Name = CallerApp.rss(o.Statuses).GetField("STATUS")
                                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf Change_State
                                'If .Items(.Items.Count - 1).Name = CurrentState Then
                                '    .Items(.Items.Count - 1).BackColor = Color.LightSkyBlue
                                'End If
                            End If
                    End Select

                Next
            End With
            Me.CallList.ContextMenu = menu
            'Dim pos As System.Drawing.Point
            'pos.X = 0
            'pos.Y = 22 + s(0) * TextSize("Test", CallList.Font).Height
            'CallList.ContextMenu.Show(CallList, pos)
        End If

    End Sub

    Sub State_Open(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim s() As Integer = SelectedItems(CallList) 'ListView.SelectedListViewItemCollection = CallList.SelectedItems
        If IsNothing(s) Then Exit Sub

        Dim selectedtext As String = CallList.Items(s(0)).Text
        RaiseEvent OpenCall(selectedtext)
    End Sub

    Sub Change_State(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim mi As MenuItem = sender
        Dim s() As Integer = SelectedItems(CallList) 'ListView.SelectedListViewItemCollection = 
        If IsNothing(s) Then Exit Sub
        Dim selectedIndex As Integer = s(0)
        Dim selectedtext As String = CallList.Items(s(0)).Text

        CallerApp.rss(o.ServiceCall).currentIndex = selectedtext
        Dim CurrentState As String = Trim(CallerApp.rss(o.ServiceCall).GetField("STATUS"))

        If mi.Text = CurrentState Then Exit Sub

        With CallerApp.rss(o.Actions)
            For i As Integer = 0 To UBound(.thisArray, 2)
                If .thisArray(2, i) = mi.Text Then
                    Select Case .thisArray(1, i)
                        Case txt_SetTimeEnRoute
                            With CallerApp.rss(o.Time)
                                .currentIndex = selectedtext
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
                            With CallerApp.rss(o.Time)
                                .currentIndex = selectedtext
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
                            With CallerApp.rss(o.Time)
                                .currentIndex = selectedtext
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
                            RaiseEvent CloseCall(CallerApp.rss(o.ServiceCall).currentIndex)

                    End Select
                End If
            Next
        End With

        ' Post the status Change
        With CallerApp.rss(o.ServiceCall)
            .currentIndex = selectedtext
            RaiseEvent StatusChange(.currentIndex)
            If .Validate Then
                Dim i As Integer = selectedIndex
                Me.CallList.Items(i).SubItems(2).Text = mi.Text
                .SetField("STATUS", mi.Text)
                .Save()
                .doLoading(CallerApp.rss(o.ServiceCall))
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
        With CallerApp.rss(o.Actions)
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
