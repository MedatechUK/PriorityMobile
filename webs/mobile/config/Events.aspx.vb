Imports System.Diagnostics
Imports System.Data
Imports System.Globalization

Partial Class config_Events
    Inherits System.Web.UI.Page

#Region "Initialisiation and Finalisation"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlGrid.Visible = False
            pnlLoader.Visible = True
            GetEvents("0", "TimeGenerated")
        End If
    End Sub

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        pnlLoader.Visible = True
        pnlGrid.Visible = False
    End Sub

#End Region

#Region "Private Properties"

#Region "Log Properties"

    Private ReadOnly Property LogFile() As String
        Get
            Return "Application"
        End Get
    End Property

    Private ReadOnly Property SourceName() As String
        Get
            Return "PRIPROC4"
        End Get
    End Property

#End Region

#Region "WMI Query Filters"

#Region "WMI Date Filter"

    Private Enum eDateFilter
        Today = 0
        Last24 = 1
        ThreeDay = 2
        FiveDay = 3
        SevenDay = 4
    End Enum

    Private ReadOnly Property StartDate(ByVal Filter As eDateFilter) As String
        Get
            Dim dt As DateTime
            Select Case Filter
                Case eDateFilter.Today
                    dt = New DateTime(Now.Year, Now.Month, Now.Day)
                Case eDateFilter.Last24
                    dt = DateAdd(DateInterval.Day, -1, Now)
                Case eDateFilter.ThreeDay
                    dt = DateAdd(DateInterval.Day, -3, Now)
                Case eDateFilter.FiveDay
                    dt = DateAdd(DateInterval.Day, -5, Now)
                Case eDateFilter.SevenDay
                    dt = DateAdd(DateInterval.Day, -7, Now)
            End Select
            Return String.format("'{0}.000000-000'", dt.ToString("yyyyMMddHHmmss"))
        End Get
    End Property

#End Region

#Region "WMI Event Level Filter"

    Private ReadOnly Property EventLevel(ByVal chkList As CheckBoxList) As String
        Get
            Dim str As New System.Text.StringBuilder
            Dim evtLev As New System.Collections.Generic.List(Of String)
            For Each i As ListItem In chkList.Items
                With i
                    If .Selected Then
                        evtLev.Add(String.Format("EventType = {0}", .Value))
                    End If
                End With
            Next
            If evtLev.Count > 0 Then
                str.Append(" and (")
                For ev As Integer = 0 To evtLev.Count - 1
                    str.Append(evtLev(ev))
                    If ev < evtLev.Count - 1 Then
                        str.Append(" or ")
                    End If
                Next
                str.Append(")")
            End If
            Return str.ToString
        End Get
    End Property

#End Region

#Region "WMI Text Filter"

    Private ReadOnly Property TextFilter(ByVal SearchText As String) As String
        Get
            If SearchText.Length > 0 Then
                Return String.Format(" and Message like '%{0}%'", SearchText)
            Else
                Return ""
            End If
        End Get
    End Property

#End Region

#End Region

#Region "WMI Sort"

    Private Property GridViewSortDirection() As String
        Get
            Return If(TryCast(ViewState("SortDirection"), String), "ASC")
        End Get
        Set(ByVal value As String)
            ViewState("SortDirection") = value
        End Set
    End Property

    Private Function ConvertSortDirectionToSql(ByVal sortDirection As SortDirection) As String
        Select Case GridViewSortDirection
            Case "ASC"
                GridViewSortDirection = "DESC"
                Exit Select

            Case "DESC"
                GridViewSortDirection = "ASC"
                Exit Select
        End Select

        Return GridViewSortDirection
    End Function

#End Region

#Region "WMI Properties"

    Private ReadOnly Property ObjWMIEvt(Optional ByVal strComputer As String = ".") As Object
        Get
            Return GetObject("winmgmts:" & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
        End Get
    End Property

    Private ReadOnly Property WMIQuery() As String
        Get
            Return String.Format( _
                "Select * from Win32_NTLogEvent " & _
                "Where Logfile = '{0}' " & _
                "and SourceName = '{1}'  " & _
                "and TimeWritten >= {2} " & _
                "{3}" & _
                "{4}", _
                LogFile, _
                SourceName, _
                StartDate(DropDownList1.SelectedItem.Value), _
                EventLevel(CheckBoxList1), _
                TextFilter(txtSearch.Text) _
            )
        End Get
    End Property

    Private ReadOnly Property WMITable() As DataTable
        Get
            Dim dterr As New DataTable
            Try
                With dterr
                    .Rows.Clear()
                    .Clear()

                    With .Columns
                        .Add("Index", GetType(Integer))
                        .Add("EntryType", GetType(String))
                        .Add("Img", GetType(String))
                        .Add("TimeGenerated", GetType(String))
                        .Add("Message", GetType(String))
                    End With

                    For Each objEvent As Object In ObjWMIEvt.ExecQuery(WMIQuery)

                        Dim dtErrRow As DataRow = dterr.NewRow
                        dtErrRow("Index") = objEvent.EventIdentifier
                        Select Case objEvent.EventType.ToString
                            Case "1"
                                dtErrRow("EntryType") = "Error"
                                dtErrRow("Img") = "images/error.png"
                            Case "2"
                                dtErrRow("EntryType") = "Warning"
                                dtErrRow("Img") = "images/warning.png"
                            Case "3"
                                dtErrRow("EntryType") = "Information"
                                dtErrRow("Img") = "images/info.png"
                            Case "4"
                                dtErrRow("EntryType") = "SucessAudit"
                                dtErrRow("Img") = "images/info.png"
                            Case "5"
                                dtErrRow("EntryType") = "FailureAudit"
                                dtErrRow("Img") = "images/error.png"
                        End Select
                        dtErrRow("TimeGenerated") = DateTime.ParseExact( _
                            objEvent.TimeGenerated.ToString.Substring(0, 14), _
                            "yyyyMMddHHmmss", _
                            cultureinfo.InvariantCulture _
                        ).ToString("dd/MM/yy HH:mm")
                        dtErrRow("Message") = objEvent.Message
                        dterr.Rows.Add(dtErrRow)

                    Next

                End With

            Catch ex As Exception

            End Try

            Return dterr
        End Get
    End Property

#End Region

#End Region

#Region "Grid Methods / Handlers"

#Region "Grid Private Methods"

    Private Sub GetEvents(ByVal sort As String, ByVal item As String)
        With GridView1
            If IsPostBack Then

                ViewState("sort") = sort
                ViewState("item") = item

                .AutoGenerateColumns = False
                .DataKeyNames = New String() {"Index"}
                .BorderWidth = "1"
                .GridLines = GridLines.Both
                .CellPadding = "3"
                .CellSpacing = "0"
                .AllowSorting = True
                .DataSource = WMITable
                .DataBind()

            Else

                pnlGrid.Visible = True

                .Visible = True
                .DataSource = WMITable
                .DataBind()

            End If

        End With

    End Sub

#End Region

#Region "Protected Grid Event Handlers"

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) _
        Handles GridView1.RowCreated

        With e.Row
            If .RowType = DataControlRowType.DataRow Then
                Dim txtMessage As TextBox = TryCast(.FindControl("txtMessage"), TextBox)
                Try
                    txtMessage.Text = .DataItem("Message")
                Catch
                End Try
            End If
        End With
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) _
        Handles GridView1.PageIndexChanging

        GridView1.PageIndex = e.NewPageIndex
        GetEvents("", "")
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) _
        Handles GridView1.Sorting

        With e
            GetEvents(.SortDirection, .SortExpression)
        End With
    End Sub

#End Region

#End Region

#Region "Control Event Handlers"

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Button1.Click

        If IsPostBack = True Then
            GetEvents(ViewState("sort"), "")
        End If
    End Sub

#End Region

End Class
