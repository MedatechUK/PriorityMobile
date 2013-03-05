Imports System.Xml
Imports PriorityMobile

Public Class ctrl_FamilyItems
    Inherits iView

    Private ReadOnly Property HomeForm() As xForm
        Get
            Return TopForm("Home").CurrentForm
        End Get
    End Property

    Private ReadOnly Property HomeFormView() As iView
        Get
            Return HomeForm.Views(HomeForm.CurrentView)
        End Get
    End Property

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
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
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
            .Add(AddressOf hCalc, "ADD.BMP", CBool(String.Compare(thisForm.FormData.SelectSingleNode(HomeFormView.thisForm.boundxPath).ParentNode.ParentNode.Name, "order", True) = 0) And Not IsNothing(thisForm.TableData.Current))
        End With
    End Sub

    Private Sub hCalc()
        thisForm.Calc(999)
    End Sub

    Public Overrides Sub SetNumber(ByVal MyValue As Integer)

        With thisForm
            AddOrderItem(thisForm, .FormData.SelectSingleNode(HomeFormView.thisForm.boundxPath).ParentNode.ParentNode, .CurrentRow("name"), .CurrentRow("barcode"), .CurrentRow("des"), MyValue.ToString)
            .RefreshForm()

            'Dim parts As XmlNode = .FormData.SelectSingleNode(HomeFormView.thisForm.boundxPath).ParentNode
            'Dim OrderPart As XmlNode = parts.SelectSingleNode(String.Format("part[name='{0}']", .CurrentRow("name")))
            'Dim Price As Double = CDbl(.CurrentRow("price"))
            'If IsNothing(OrderPart) Then
            '    GetCustomerPrice(Price, parts.ParentNode.ParentNode.ParentNode.SelectSingleNode("customerpricelist"), MyValue)
            '    Dim part As XmlNode = .CreateNode(parts, "part")
            '    .CreateNode(part, "name", .CurrentRow("name"))
            '    .CreateNode(part, "barcode", .CurrentRow("barcode"))
            '    .CreateNode(part, "des", .CurrentRow("des"))
            '    .CreateNode(part, "qty", MyValue.ToString)
            '    .CreateNode(part, "unitprice", Price)
            'Else
            '    GetCustomerPrice(Price, parts.ParentNode.ParentNode.ParentNode.SelectSingleNode("customerpricelist"), CInt(OrderPart.SelectSingleNode("qty").InnerText) + MyValue)
            '    OrderPart.SelectSingleNode("qty").InnerText = CInt(OrderPart.SelectSingleNode("qty").InnerText) + MyValue
            '    OrderPart.SelectSingleNode("unitprice").InnerText = Price
            'End If
            '.Save()
            '.Bind()
        End With

        For Each V As iView In HomeForm.Views
            V.Bind()
        Next
        HomeFormView.RefreshData()
        HomeForm.RefreshForm()

    End Sub

    'Sub GetCustomerPrice(ByRef Price As Double, ByVal PriceList As XmlNode, ByVal qty As Integer)
    '    With thisForm
    '        For Each custPrice As XmlNode In PriceList.SelectNodes(String.Format("parts/part[name='{0}']", .CurrentRow("name")))
    '            If qty >= Integer.Parse(custPrice.SelectSingleNode("tquant").InnerText) Then
    '                If CDbl(custPrice.SelectSingleNode("price").InnerText) < Price Then
    '                    Price = CDbl(custPrice.SelectSingleNode("price").InnerText)
    '                End If
    '            End If
    '        Next
    '    End With
    'End Sub

#End Region

End Class
