Imports System.Xml
Imports PriorityMobile

Public Class ctrl_FamilyItems
    Inherits iView

    'Private ReadOnly Property HomeForm() As xForm
    '    Get
    '        Return TopForm("Home").CurrentForm
    '    End Get
    'End Property

    'Private ReadOnly Property HomeFormView() As iView
    '    Get
    '        Return HomeForm.Views(HomeForm.CurrentView)
    '    End Get
    'End Property

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
                .FormLabel = "Parts"
                .Sort = "name"
                .AddColumn("name", "Part", 130, True)
                .AddColumn("des", "Description", 260)
                '.AddColumn("barcode", "Barcode", 130)
            End With
        End With

    End Sub

#End Region

#Region "Overides Base Properties"

    'Public Overrides ReadOnly Property ButtomImage() As String
    '    Get
    '        Return "calendar.bmp"
    '    End Get
    'End Property

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
                "{0} <> '0'", _
                ListSort1.Keys(0) _
                )
        Dim family As String = thisForm.FormData.SelectSingleNode(thisForm.Parent.thisxPath).SelectSingleNode("familyname").InnerText
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            .FormLabel = String.Format("Parts in '{0}' family.", family)
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                If .RowSelected(r, thisForm.CurrentRow) Then
                    .Items(.Items.Count - 1).Selected = True
                    .Items(.Items.Count - 1).Focused = True
                Else
                    .Items(.Items.Count - 1).Selected = False
                    .Items(.Items.Count - 1).Focused = False
                End If
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
                Case Else
                    Return True
            End Select
        End With
    End Function

#End Region

#Region "Local control Handlers"

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value(ListSort1.Keys(0), row)
            If Not String.Compare(Text, cur) = 0 Then
                .TableData.Position = .TableData.Find(ListSort1.Keys(0), cur)
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

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        With ToolBar            
            .Add(AddressOf hCalc, "ADD.BMP", Not (Me.ListSort1.SelectedIndex) = -1) 'CBool(String.Compare(thisForm.FormData.SelectSingleNode(HomeFormView.thisForm.Parent.boundxPath).Name, "order", True) = 0) _
        End With
    End Sub

    Private Sub hCalc()
        thisForm.Calc( _
            New calcSetting( _
                CInt(0), , , thisForm.CurrentRow("des").ToString _
            ) _
        )
    End Sub

    Public Overrides Sub SetNumber(ByRef cSetting As calcSetting)
        With thisForm
            If cSetting.Result = DialogResult.OK Then

                Dim prepay As String = ""
                If .FormData.SelectSingleNode(.Parent.boundxPath).SelectSingleNode("familyname").InnerText = "Postal Cream" Then prepay = "Y"
                AddOrderItem(thisForm, .FormData.SelectSingleNode(thisForm.Parent.Parent.thisxPath).ParentNode.ParentNode, .CurrentRow("name"), .CurrentRow("barcode"), .CurrentRow("des"), cSetting.DNUM.ToString, prepay)
                '.SetTopForm(0)
                .TopForm("Home").CloseForm()
                '.TopForm("Home").CloseForm()
                '.TopForm("Home").CurrentForm.Views(0).RefreshData()
                .RefreshForm()

                'HomeFormView.RefreshData()
                'HomeForm.RefreshForm()

                '.SetTopForm(0)
            Else
                .TopForm("Home").CloseForm()                
                '.TopForm("Home").CurrentForm.Views(0).RefreshData()
                .RefreshForm()
            End If
        End With
    End Sub

#End Region

End Class
