Imports System.Xml
Imports PriorityMobile
Imports PriorityMobile.funcDate
Imports System.Data

Public Class ctrl_Summary
    Inherits iView

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
            Return DateDiff(DateInterval.Minute, #1/1/1988#, DateAdd(DateInterval.Day, 1, SelectedDate)) - 1
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With Me
            With ListSort1
                .Sort = "callnumber"
                .AddColumn("callnumber", "Call", 130)
                .AddColumn("postcode", "Post Code", 130)
                .AddColumn("custname", "Customer", 200)
                .AddColumn("servtype", "Call Type", 150)
                .AddColumn("callstatus", "Status", 150)
            End With
        End With

    End Sub

#End Region

#Region "Overides Base Properties"

    Public Overrides ReadOnly Property ButtomImage() As String
        Get
            Return "calendar.bmp"
        End Get
    End Property

    Public Overrides ReadOnly Property Selected() As String
        Get
            Return ListSort1.Selected
        End Get
    End Property

#End Region

#Region "Overrides base Methods"

    Public Overrides Sub Bind() Handles ListSort1.Bind

        IsBinding = True
        Dim dr() As Data.DataRow = Nothing
        Dim query As String = String.Format( _
                "calldate >= {0} and calldate <= {1}", _
                StartDate, _
                EndDate _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                With .Items(.Items.Count - 1)
                    If String.Compare(.Text, thisForm.CurrentRow("callnumber"), True) = 0 Then
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
        Me.ListSort1.Focus()
    End Sub

    Public Overrides Sub ViewChanged()
        Bind()
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        With Me
            If IsNothing(.Selected) Then Return False
            If IsNothing(.thisForm.TableData.Current) Then Return False
            If IsNothing(ListSort1.Selected) Then Return False
            Select Case Name.ToUpper
                Case "REPAIR", "PARTS", "SIGN"
                    Return Not xmlForms.StatusRule(.thisForm.CurrentRow("callstatus").ToString, eStatusRule.prereport) And Not xmlForms.StatusRule(.thisForm.CurrentRow("callstatus").ToString, eStatusRule.post)
                Case Else
                    Return True
            End Select
        End With
    End Function

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        With ToolBar
            .Add(AddressOf hFind, "FIND.BMP")
            ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
            .Add(AddressOf hClearSort, "SORTASC.BMP")
        End With
    End Sub

    Private Sub hClearSort()
        ListSort1.Sort = "callnumber"
        Bind()
    End Sub

    Private Sub hFind()
        Dim dlg As New dlgBarcode
        Dim tbserial As TextBox = dlg.FindControl("txtSerialNumber")
        tbserial.Focus()
        thisForm.Dialog(dlg)
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        Dim tbserial As TextBox = frmDialog.FindControl("txtSerialNumber")
        Dim serial As String = tbserial.Text
        Dim found As Boolean = False
        If serial.Length > 0 Then
            Dim dt As DataTable = thisForm.TableData.DataSource
            For Each row As DataRow In dt.Rows
                If InStr(" " & row("serial") & " ", " " & serial & " ", CompareMethod.Text) > 0 Then
                    found = True
                    With thisForm.TableData
                        .MoveFirst()
                        While Not String.Compare(row("callnumber"), thisForm.CurrentRow("callnumber")) = 0
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
            Else
                thisForm.CurrentView += 1
            End If
        End If
        thisForm.RefreshForm()
    End Sub

    Private Sub hPlaceCall()
        With thisForm
            With .FormData
                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                If IsNothing(n.SelectSingleNode("report/times/called")) Then
                    n = n.SelectSingleNode("report/times")
                    n.AppendChild(.CreateElement("called"))
                    n.LastChild.InnerText = DateDiff(DateInterval.Minute, #1/1/1988#, Now)
                End If

            End With
            .Save()
        End With

        Dim ph As New Microsoft.WindowsMobile.Telephony.Phone
        Try
            ph.Talk(thisForm.CurrentRow("phone"))
        Catch ex As Exception
            MsgBox(String.Format("Call failed to: {0}.", thisForm.CurrentRow("phone")))
        End Try

    End Sub

#End Region

#Region "Local control Handlers"

    Private Sub DateTimePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker.ValueChanged
        Bind()
    End Sub

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value("callnumber", row)
            If Not String.Compare(Text, cur) = 0 Then
                .TableData.Position = .TableData.Find("callnumber", cur)
                .RefreshSubForms()
                .RefreshDirectActivations()
            End If
        End With
    End Sub

    Private Sub hItemSelect() Handles ListSort1.ItemSelect
        With thisForm
            If Not IsNothing(.TableData.Current) Then
                .CurrentView += 1
                .RefreshForm()
            End If
        End With
    End Sub

#End Region

End Class
