Imports System.Diagnostics
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class _Default
    Inherits System.Web.UI.Page
#Region "Variable Decleration"
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
#Region "Initialisiation"
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
#Region "Log Reading"
    Private Sub getlogdata(ByVal days As Integer, ByVal query As String, ByVal sortby As String)
        'This sub reads all the events from the log using a WMI Query. I build the query up based on the values in the filters box.
        'it should be noted that when working with dates in WMI it uses a variant date type. As you can see in tjhe variable que  the wmi script is (almost) written is sql. The first thing we need to put in is which 
        'logfile to read from and then what the source of the events should be. Application and PriProc4. We also want to time limit the query (for speed).
        Dim que As String = "Select * from Win32_NTLogEvent Where Logfile = 'Application' and SourceName = 'PRIPROC4'  and TimeWritten >= '"
        'we have to be very careful when forming the date as it is in a structure dependant form  
        Dim yr, day, mo, hr, min, sec As String

        Dim g As Date = DateAdd(DateInterval.Minute, days, Now)
        Dim hol As Date = FormatDateTime("1/1/1988", DateFormat.GeneralDate)
        Dim mins As Integer = DateDiff(DateInterval.Minute, hol, g)
        Dim finaldate As String = ""
        day = DatePart(DateInterval.Day, g)
        mo = DatePart(DateInterval.Month, g)
        yr = DatePart(DateInterval.Year, g)
        hr = "00"
        min = "01"
        sec = "01"

        'we need the month to be 2 digits
        If Len(mo) = 1 Then
            mo = "0" & mo
        End If



        'and the day needs to be 2digits
        If Len(day) = 1 Then
            day = "0" & day
        End If

        'after the decimal point we have miliseconds and after the dash we have the gmt offset
        finaldate = yr & mo & day & hr & min & sec & ".000000-000"


        Dim strComputer As String = "."
        Dim ObjWMIEvt As Object = GetObject("winmgmts:" & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")

        que = que & finaldate & "' and " & query

        Dim LoggedEvents As Object = ObjWMIEvt.ExecQuery(que)
        Dim dterr As New DataTable
        dterr.Rows.Clear()
        dterr.Clear()

        dterr.Columns.Add("Index", GetType(Integer))
        dterr.Columns.Add("EntryType", GetType(String))
        dterr.Columns.Add("TimeGenerated", GetType(String))
        dterr.Columns.Add("Message", GetType(String))

        For Each objEvent As Object In LoggedEvents
            Dim dtErrRow As DataRow
            dtErrRow = dterr.NewRow
            dtErrRow("Index") = objEvent.EventIdentifier

            dtErrRow("EntryType") = objEvent.EventType
            dtErrRow("TimeGenerated") = objEvent.TimeGenerated
            dtErrRow("Message") = objEvent.Message
            dterr.Rows.Add(dtErrRow)
        Next
        Session("dterr") = dterr
    End Sub

#End Region

#Region "Set Grid & fill"
    Private Sub GetEvents(ByVal sort As String, ByVal item As String)
        If IsPostBack Then

            Dim ft As String = ViewState("FilterText")
            Dim filter, fitem As String

            Select Case ft
                Case ""
                    filter = "'%'"
                Case Else
                    filter = "'%" & ViewState("FilterText") & "%'"
            End Select

            If ViewState("FilterItem") Is Nothing = False Then
                fitem = ViewState("FilterItem")
            Else
                fitem = "NONE"
            End If

            Dim days As Integer

            If ViewState("FilterDate") Is Nothing = True Then
                days = -1
            Else
                Select Case ViewState("FilterDate").ToString
                    Case "Today"
                        days = 0 - ((DatePart(DateInterval.Hour, Now()) * 60) + DatePart(DateInterval.Minute, Now()))
                    Case "Past 24 Hrs"
                        days = -1 * (24 * 60)
                    Case "Past 7 Days"
                        days = -7 * (24 * 60)
                    Case "Past 30 Days"
                        days = -30 * (24 * 60)
                    Case "Past 90 Days"
                        days = -90 * (24 * 60)
                    Case Else
                        days = 999 * (24 * 60)
                End Select
            End If

            If sort = "" Then
                sort = 0
            End If

            ViewState("sort") = sort
            ViewState("item") = item

            With GridView1
                .AutoGenerateColumns = False
                .DataKeyNames = New String() {"Index", "Message"}
                .BorderWidth = "1"
                .GridLines = GridLines.Both
                .CellPadding = "3"
                .CellSpacing = "0"
                .AllowSorting = True

            End With
            Dim dterr As New DataTable



            Select Case fitem
                Case "NONE"
                    Select Case sort
                        Case ""

                        Case Else

                            If item = "" Then
                                item = "TGENERATED"
                            End If

                    End Select
                Case Else
                    If item = "" Then
                        item = "TGENERATED"
                    End If
                    If filter = "" Then
                    Else
                        If fitem = "" Then
                            fitem = "Message like " & filter
                        Else
                            fitem &= " and Message like " & filter
                        End If
                    End If
            End Select



            getlogdata(days, fitem, Convert.ToString(item & " " & ConvertSortDirectionToSql(sort)))

            dterr = DirectCast(Session("dterr"), DataTable)

            GridView1.DataSource = dterr
            GridView1.DataBind()

        Else
            Dim i As ListItem
            Dim h As String = "(EventType = "
            Dim j As Integer = 0
            For Each i In CheckBoxList1.Items
                If i.Selected = True Then
                    j = CheckBoxList1.Items.IndexOf(i)
                End If

            Next
            For Each i In CheckBoxList1.Items
                If i.Selected = True Then
                    If CheckBoxList1.Items.IndexOf(i) = j Then
                        h &= i.Value & ")"
                    Else
                        h &= i.Value & "" & " or EventType = "
                    End If


                End If
            Next

            ViewState("FilterItem") = h
            Dim filter, fitem As String

            filter = "'%'"
            fitem = h
            Dim days As Integer
            days = -1 * (24 * 60)
            sort = 0

            ViewState("sort") = sort
            ViewState("item") = item
            With GridView1
                .AutoGenerateColumns = False
                .DataKeyNames = New String() {"Index", "Message"}
                .BorderWidth = "1"
                .GridLines = GridLines.Both
                .CellPadding = "3"
                .CellSpacing = "0"
                .AllowSorting = True

            End With


            Dim dtErr As New DataTable
            Select Case fitem
                Case "NONE"
                    Select Case sort
                        Case ""

                        Case Else
                            If item = "" Then
                                item = "TGENERATED"
                            End If
                    End Select

                Case Else
                    If item = "" Then
                        item = "TGENERATED"
                    End If
                    If filter = "" Then
                    Else
                        If fitem = "" Then
                            fitem = "MESSAGE like " & filter

                        Else
                            'fitem &= " and MESSAGE like" & filter

                        End If
                    End If
            End Select

            getlogdata(days, fitem, Convert.ToString(item & " " & ConvertSortDirectionToSql(sort)))

            dtErr = DirectCast(Session("dterr"), DataTable)
            pnlGrid.Visible = True
            GridView1.Visible = True
            GridView1.DataSource = dtErr
            GridView1.DataBind()
            Dim hgh As Integer
            hgh = GridView1.Rows.Count
        End If


    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If IsPostBack = True Then
            txtSearch.Text = txtSearch.Text.Replace("'", "")
            txtSearch.Text = txtSearch.Text.Replace("""", "")
            txtSearch.Text = txtSearch.Text.Replace("[", "")
            txtSearch.Text = txtSearch.Text.Replace("]", "")
            ViewState("FilterText") = txtSearch.Text
            ViewState("FilterDate") = DropDownList1.Text

            Dim i As ListItem
            Dim h As String = "(EventType = "
            Dim j As Integer = 0
            For Each i In CheckBoxList1.Items
                If i.Selected = True Then
                    j = CheckBoxList1.Items.IndexOf(i)
                End If

            Next
            For Each i In CheckBoxList1.Items
                If i.Selected = True Then
                    If CheckBoxList1.Items.IndexOf(i) = j Then
                        h &= i.Value & ")"
                    Else
                        h &= i.Value & " or EventType = "
                    End If


                End If
            Next

            ViewState("FilterItem") = h
            GetEvents(ViewState("sort"), "")
        End If
    End Sub
#End Region

#Region "Grid Events"
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GetEvents("", "")
    End Sub
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Dim i, j As String
        i = e.SortDirection
        j = e.SortExpression
        GetEvents(i, j)

    End Sub


    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgLevImg"), Image)

            Dim messagetext As String = GridView1.DataKeys(e.Row.RowIndex).Values("Message").ToString()
            Dim txtMessage As TextBox = TryCast(e.Row.FindControl("txtMessage"), TextBox)
            txtMessage.Text = messagetext
            Select Case e.Row.Cells(2).Text
                Case "1"
                    e.Row.Cells(2).Text = "Error"
                Case "2"
                    e.Row.Cells(2).Text = "Warning"
                Case "3"
                    e.Row.Cells(2).Text = "Information"
                Case "4"
                    e.Row.Cells(2).Text = "SucessAudit"
                Case "5"
                    e.Row.Cells(2).Text = "FailureAudit"
            End Select
            Select Case e.Row.Cells(2).Text
                Case "Error", "FailureAudit"
                    img.ImageUrl = "images/error.png"
                Case "Warning"
                    img.ImageUrl = "images/warning.png"
                Case Else
                    img.ImageUrl = "images/info.png"
            End Select
            If e.Row.Cells(2).Text = "Error" Or e.Row.Cells(2).Text = "Failure" Then


            End If
            Dim yr, mo, dy, hr, min As String

            Dim gg As String = e.Row.Cells(3).Text
            yr = gg.Substring(0, 4)
            mo = gg.Substring(4, 2)
            dy = gg.Substring(6, 2)
            hr = gg.Substring(8, 2)
            min = gg.Substring(10, 2)
            e.Row.Cells(3).Text = dy & "/" & mo & "/" & yr & " " & hr & ":" & min

        End If
    End Sub
#End Region












End Class
