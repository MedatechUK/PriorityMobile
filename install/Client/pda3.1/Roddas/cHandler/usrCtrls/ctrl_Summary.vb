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

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With Me
            With ListSort1
                .Sort = "deliveryorder"
                .AddColumn("deliveryid", "id", 0)
                .AddColumn("custname", "Customer", 280)
                .AddColumn("postcode", "Post Code", 150)
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
                "deliverydate >= {0} and deliverydate <= {1} and delivered <> 'Y'", _
                StartDate, _
                EndDate, _
                Chr(34) _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                With .Items(.Items.Count - 1)
                    If String.Compare(.Text, thisForm.CurrentRow("deliveryid"), True) = 0 Then
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
        Return ListSort1.SelectedIndex > -1
    End Function

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        With ToolBar        
            .Add(AddressOf hClearSort, "SORTASC.BMP")
        End With
    End Sub

    Private Sub hClearSort()
        ListSort1.Sort = "deliveryorder"
        Bind()
    End Sub

#End Region

#Region "Local control Handlers"

    Private Sub DateTimePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker.ValueChanged
        Bind()
    End Sub

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value("deliveryid", row)
            If Not String.Compare(Text, cur) = 0 Then
                .TableData.Position = .TableData.Find("deliveryid", cur)
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
