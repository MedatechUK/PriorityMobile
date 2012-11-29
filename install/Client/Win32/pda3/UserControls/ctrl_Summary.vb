Imports System.Xml

Public Class ctrl_Summary
    Inherits ViewControl.iView

#Region "Date Functions"

    Private ReadOnly Property SelectedDate() As Date
        Get
            Dim dt As New Date( _
                DateTimePicker.Value.Year, _
                DateTimePicker.Value.Month, _
                DateTimePicker.Value.Day _
            )
            Return dt
        End Get
    End Property

    Private ReadOnly Property StartDate() As Integer
        Get
            Return DateDiff(DateInterval.Minute, #1/1/1988#, SelectedDate)
        End Get
    End Property

    Private ReadOnly Property EndDate() As Integer
        Get
            Return DateDiff(DateInterval.Minute, #1/1/1988#, DateAdd(DateInterval.Day, 1, SelectedDate))
        End Get
    End Property

    Private Function DateFromInt(ByVal IntDate As Integer) As Date
        Return DateAdd(DateInterval.Minute, IntDate, New Date(1988, 1, 1))
    End Function

    Private Function DateToMin()
        Return DateDiff(DateInterval.Minute, #1/1/1988#, Now)
    End Function

#End Region

#Region "Define List Columns"

    Private ReadOnly Property ListColumns() As Dictionary(Of String, String)
        Get
            Dim ret As New Dictionary(Of String, String)
            ret.Add("Call", "callnumber")
            ret.Add("Status", "callstatus")
            Return ret
        End Get
    End Property

    Private ReadOnly Property ColumnWidths() As Dictionary(Of String, Integer)
        Get
            Dim ret As New Dictionary(Of String, Integer)
            ret.Add("Call", 100)
            ret.Add("Status", 100)
            Return ret
        End Get
    End Property

    Private _Sort As String = "callnumber"
    Private Property Sort() As String
        Get
            Return _Sort
        End Get
        Set(ByVal value As String)
            _Sort = value
        End Set
    End Property

#End Region

#Region "Overides Base Properties"

    Public Overrides ReadOnly Property ButtomImage() As System.Drawing.Image
        Get
            Return Image.FromFile("icons\calendar.bmp")
        End Get
    End Property

    Public Overrides ReadOnly Property Selected() As String
        Get
            If ListView.SelectedItems.Count = 0 Then Return Nothing
            Return ListView.SelectedItems(0).Text
        End Get
    End Property

#End Region

#Region "Overrides base Methods"

    Public Overrides Sub Bind()

        IsBinding = True
        Dim dr() As DataRow = Nothing
        dr = thisForm.TableData.DataSource.Select( _
            String.Format( _
                "calldate >= {0} and calldate <= {1}", _
                StartDate, _
                EndDate _
                ) _
            , Sort)

        With ListView
            .Clear()
            For Each c As String In ListColumns.Keys
                .Columns.Add(c)
                With .Columns
                    With .Item(.Count - 1)
                        .Width = ColumnWidths(c)
                        .Tag = ListColumns(c)
                    End With
                End With
            Next
            For Each r As System.Data.DataRow In dr
                .Items.Add(New ListViewItem)
                With .Items(.Items.Count - 1)
                    Dim first As Boolean = True
                    For Each c As String In ListColumns.Values
                        If first Then
                            .Text = r.Item(c)
                            first = False
                        Else
                            With .SubItems
                                .Add(New System.Windows.Forms.ListViewItem.ListViewSubItem)
                                .Item(.Count - 1).Text = r.Item(c)
                            End With
                        End If
                    Next
                    If String.Compare(.Text, thisForm.TableData.Current("callnumber"), True) = 0 Then
                        .Selected = True
                        .Focused = True
                    Else
                        .Selected = False
                        .Focused = False
                    End If
                End With
            Next
            .Focus()
        End With
        thisForm.RefreshSubForms()
        IsBinding = False

    End Sub

    Public Overrides Sub SetFocus()
        ListView.Focus()
    End Sub

    Public Overrides Sub ViewChanged()
        Bind()
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        With Me
            If IsNothing(.Selected) Then Return False
            If IsNothing(.thisForm.TableData.Current) Then Return False
            Select Case Name.ToUpper
                Case "REPAIR", "PARTS", "SIGN"
                    Select Case .thisForm.TableData.Current("callstatus").ToString.ToUpper
                        Case "ISSUED", "EN-ROUTE"
                            Return False
                        Case Else
                            Return True
                    End Select
                Case Else
                    Return True
            End Select
        End With
    End Function

    Public Overrides Sub RowUpdated(ByVal Column As String, ByVal NewValue As String)
        Select Case Column.ToUpper
            Case "CALLSTATUS"
                Select Case NewValue.ToUpper
                    Case Else
                        With thisForm
                            With .FormData
                                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                                If IsNothing(n.SelectSingleNode("times")) Then
                                    n.AppendChild(.CreateElement("times"))
                                End If
                                n = n.SelectSingleNode("times")
                                n.AppendChild(.CreateElement(NewValue.ToUpper))
                                n.LastChild.InnerText = DateToMin
                            End With
                            .Save()
                        End With
                End Select
            Case Else

        End Select
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As System.Windows.Forms.ToolStrip)
        ToolBar.Items.Add("", Image.FromFile("icons\FIND.BMP"), AddressOf hFind)
        ToolBar.Items.Add("", Image.FromFile("icons\SORTASC.BMP"), AddressOf hClearSort)
    End Sub

    Private Sub hClearSort()
        Sort = "callnumber"
        Bind()
    End Sub

    Private Sub hFind()
        Dim serial As String = InputBox("Serial Number")
        Dim found As Boolean = False
        If serial.Length > 0 Then
            Dim dt As DataTable = thisForm.TableData.DataSource
            For Each row As DataRow In dt.Rows
                If String.Compare(row("serial"), serial, True) = 0 Then
                    found = True
                    With thisForm.TableData
                        .MoveFirst()
                        While Not String.Compare(row("callnumber"), thisForm.TableData.Current("callnumber")) = 0
                            .MoveNext()
                        End While
                        Me.DateTimePicker.Value = DateFromInt(CInt(row("calldate")))
                        Bind()
                    End With
                    Exit For
                End If
            Next
            If Not found Then
                MsgBox( _
                    String.Format( _
                        "Could not find serial number {0}.", _
                        serial _
                    ) _
                )
            End If
        End If
    End Sub

#End Region

#Region "Local control Handlers"

    Private Sub DateTimePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker.ValueChanged
        Bind()
    End Sub

    Private Sub ListView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView.ColumnClick
        If Not String.Compare(Sort, ListView.Columns(e.Column).Tag) = 0 Then
            Sort = ListView.Columns(e.Column).Tag
        Else
            Sort = ListView.Columns(e.Column).Tag & " DESC"
        End If
        Bind()
    End Sub

    Private Sub ListView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView.SelectedIndexChanged, ListView.LostFocus
        If ListView.SelectedItems.Count = 0 Then Exit Sub
        If IsBinding Then Exit Sub
        With thisForm
            If Not String.Compare(ListView.SelectedItems(0).Text, .TableData.Current("callnumber")) = 0 Then
                .TableData.MoveFirst()
                While String.Compare(ListView.SelectedItems(0).Text, .TableData.Current("callnumber")) <> 0
                    .TableData.MoveNext()
                End While
                .RefreshSubForms()
                .RefreshDirectActivations()
            End If
        End With
    End Sub

    Private Sub ListView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView.DoubleClick
        With thisForm
            If Not IsNothing(.TableData.Current) Then
                .CurrentView += 1
                .RefreshForm()
            End If
        End With
    End Sub

    Private Sub ListView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ListView.KeyPress
        Select Case e.KeyChar
            Case Chr(13)
                With thisForm
                    .CurrentView += 1
                    .RefreshForm()
                End With
        End Select
    End Sub

#End Region

End Class
