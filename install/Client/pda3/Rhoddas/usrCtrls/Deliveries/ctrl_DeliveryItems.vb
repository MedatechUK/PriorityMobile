Imports System.Xml
Imports PriorityMobile

Public Class ctrl_DeliveryItems
    Inherits iView

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
                .FormLabel = "Items to deliver"
                .Sort = "name"
                .AddColumn("ordi", "ordi", 0, True)
                .AddColumn("name", "Part", 130)
                .AddColumn("des", "Description", 260)
                .AddColumn("tquant", "Qty", 65)
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

    Public Overrides Sub FormClosing()
        thisForm.CurrentView = 0
        thisForm.Save()
        MyBase.FormClosing()
    End Sub

    Public Overrides Sub Bind() Handles ListSort1.Bind

        IsBinding = True
        Dim dr() As Data.DataRow = Nothing
        Dim query As String = String.Format( _
                "{0} <> '0' and cquant = 0 and tquant > 0", _
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
                Select Case .CurrentRow("cheese")
                    Case "Y"
                        thisForm.Calc(999)

                    Case Else
                        If MsgBox(String.Format("Deliver {0} * {1}?", .CurrentRow("tquant"), .CurrentRow("name")), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                            Dim OrderItem As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format(".//part[ordi='{0}']", .CurrentRow("ordi")))
                            Dim remain As Double = CDbl(OrderItem.SelectSingleNode("tquant").InnerText)
                            While remain > 0
                                remain = DeliverOldestLot(OrderItem, remain)
                                If remain = -1 Then
                                    MsgBox("Insufficient stock.")
                                    Exit While
                                End If
                            End While
                        End If
                        'Dim dlg As New dlgDeliver
                        'dlg.Name = "Select"
                        'LotSelect(dlg.FindControl("LotView"), .CurrentRow("name"))
                        'thisForm.Dialog(dlg)
                End Select

            End If
        End With

    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        'ToolBar.Add(AddressOf hDeliver, "add.BMP", True)
    End Sub

    Private Sub hDeliver()

        Dim dlg As New dlgScanDeliver
        With dlg
            Dim lotnumber As TextBox = .FindControl("lotnumber")
            .FocusContolName = "lotnumber"
            .Name = "Scan"
        End With
        thisForm.Dialog(dlg)

    End Sub

    'Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

    '    With thisForm
    '        If frmDialog.Result = DialogResult.OK Then
    '            Select Case frmDialog.Name
    '                Case "Select"
    '                    Dim LotView As ListView = frmDialog.FindControl("LotView")
    '                    Dim lots As XmlNode = .FormData.SelectSingleNode( _
    '                        String.Format( _
    '                            "pdadata/warehouse/parts/part[name='{0}']", _
    '                            .CurrentRow("name") _
    '                        ) _
    '                    )
    '                    Dim lot As XmlNode = lots.SelectSingleNode(String.Format(".//lot[name='{0}']", LotView.Items(LotView.SelectedIndices(0)).SubItems(0).Text))
    '                    Dim OrderItem As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format(".//part[ordi='{0}']", .CurrentRow("ordi")))
    '                    DeliverLot(OrderItem, lot)

    '                Case "Scan"
    '                    Dim lotnumber As TextBox = frmDialog.FindControl("lotnumber")
    '                    Dim whsParts As XmlNode = .FormData.SelectSingleNode( _
    '                        String.Format( _
    '                            "pdadata/warehouse/parts" _
    '                        ) _
    '                    )
    '                    Dim lot As XmlNode = whsParts.SelectSingleNode(String.Format(".//lot[name='{0}']", lotnumber.Text))
    '                    Dim part As XmlNode = whsParts.SelectSingleNode(String.Format(".//part[barcode='{0}']", lotnumber.Text))

    '                    If Not IsNothing(lot) Then
    '                        Dim Orderitem As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format(".//part[name='{0}' and tquant > 0]", lot.ParentNode.ParentNode.SelectSingleNode("name").InnerText))
    '                        If IsNothing(Orderitem) Then
    '                            MsgBox("Part does not exist on this order or is already delivered.")
    '                        Else
    '                            If CDbl(lot.SelectSingleNode("qty").InnerText) = 0 Then
    '                                MsgBox("No items remaining in scanned lot.")
    '                            Else
    '                                DeliverLot(Orderitem, lot)
    '                            End If
    '                        End If

    '                    ElseIf Not IsNothing(part) Then
    '                        Dim Orderitem As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format(".//part[name='{0}' and tquant > 0]", part.SelectSingleNode("name").InnerText))
    '                        If IsNothing(Orderitem) Then
    '                            MsgBox("Part does not exist on this order or is already delivered.")
    '                        Else
    '                            If part.SelectNodes("lots/lot[qty > 0]").Count > 1 Then
    '                                Dim dlg As New dlgDeliver
    '                                dlg.Name = "Select"
    '                                LotSelect(dlg.FindControl("LotView"), part.SelectSingleNode("name").InnerText)
    '                                thisForm.Dialog(dlg)
    '                                Exit Sub
    '                            Else
    '                                If Not IsNothing(part.SelectSingleNode("lots/lot[qty > 0]")) Then
    '                                    DeliverLot(Orderitem, part.SelectSingleNode("lots/lot[qty > 0]"))
    '                                Else
    '                                    MsgBox("No items remaining of scanned part.")
    '                                End If
    '                            End If
    '                        End If

    '                    Else
    '                        MsgBox("Unknown Part / Lot barcode.")

    '                    End If

    '            End Select
    '        End If
    '        .Bind()
    '        For Each V As iView In thisForm.Views
    '            V.Bind()
    '        Next
    '        .RefreshForm()
    '    End With
    'End Sub

    Public Overrides Sub SetNumber(ByVal MyValue As Double)

        With thisForm
            If MsgBox(String.Format("Add {0} weighing {1}kg?", .CurrentRow("name"), MyValue.ToString), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim OrderItem As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format(".//part[ordi='{0}']", .CurrentRow("ordi")))
                Dim remain As Double = 1
                While remain > 0
                    remain = DeliverOldestLot(OrderItem, remain, MyValue)
                    If remain = -1 Then
                        MsgBox("Insufficient stock.")
                        Exit While
                    End If
                End While
            End If
        End With

    End Sub

    'Private Sub DeliverLot(ByRef OrderItem As XmlNode, ByRef lot As XmlNode)

    '    Dim qty As Double
    '    If Not CDbl(lot.SelectSingleNode("qty").InnerText) >= CDbl(OrderItem.SelectSingleNode("tquant").InnerText) Then
    '        qty = CDbl(lot.SelectSingleNode("qty").InnerText)
    '    Else
    '        qty = CDbl(OrderItem.SelectSingleNode("tquant").InnerText)
    '    End If

    '    With thisForm
    '        DeliverItem( _
    '            thisForm, _
    '            .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode, _
    '            OrderItem.SelectSingleNode("ordi").InnerText, _
    '            OrderItem.SelectSingleNode("name").InnerText, _
    '            OrderItem.SelectSingleNode("des").InnerText, _
    '            OrderItem.SelectSingleNode("parttype").InnerText, _
    '            OrderItem.SelectSingleNode("barcode").InnerText, _
    '            lot.SelectSingleNode("name").InnerText, _
    '            qty.ToString _
    '        )
    '    End With

    'End Sub

    Private Function DeliverOldestLot(ByRef OrderItem As XmlNode, ByVal Qty As Double, Optional ByVal weight As Double = 0) As Double
        Dim ret As Integer = 0
        With thisForm
            Dim warehouse As XmlNode = .FormData.SelectSingleNode("pdadata/warehouse")
            Dim lots As XmlNodeList = warehouse.SelectNodes(String.Format(".//part[name='{0}']/lots/lot[@qty != '0']", .CurrentRow("name")))
            If lots.Count = 0 Then
                ret = -1
            Else
                If Not CDbl(lots(0).Attributes("qty").Value) >= Qty Then
                    Qty = CDbl(lots(0).Attributes("qty").Value)
                    ret = CDbl(OrderItem.SelectSingleNode("tquant").InnerText) - CDbl(lots(0).Attributes("qty").Value)
                End If
                DeliverItem( _
                    thisForm, _
                    .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode, _
                    OrderItem.SelectSingleNode("ordi").InnerText, _
                    OrderItem.SelectSingleNode("name").InnerText, _
                    OrderItem.SelectSingleNode("des").InnerText, _
                    OrderItem.SelectSingleNode("parttype").InnerText, _
                    OrderItem.SelectSingleNode("cheese").InnerText, _
                    OrderItem.SelectSingleNode("barcode").InnerText, _
                    OrderItem.SelectSingleNode("price").InnerText, _
                    lots(0).Attributes("name").Value, _
                    Qty.ToString, _
                    weight.ToString _
                )
            End If

            .Bind()
            For Each V As iView In thisForm.Views
                V.Bind()
            Next
            .RefreshForm()
        End With

        Return ret
    End Function

    Private Sub CloseWeighDelivery()

    End Sub

    Private Sub LotSelect(ByRef LotView As ListView, ByVal Part As String, Optional ByVal selectedlot As String = Nothing)
        With thisForm
            Dim lots As XmlNode = .FormData.SelectSingleNode( _
                String.Format( _
                    "pdadata/warehouse/parts/part[name='{0}']", _
                    Part _
                ) _
            )

            If IsNothing(lots) Then
                MsgBox("No Stock")
                Exit Sub
            End If

            For Each lot As XmlNode In lots.SelectNodes(".//lot")
                If CDbl(lot.SelectSingleNode("qty").InnerText) > 0 Then
                    With LotView
                        .Items().Add(New ListViewItem)
                        With .Items(.Items.Count - 1)
                            .SubItems(0).Text = lot.SelectSingleNode("name").InnerText
                            .SubItems.Add(New ListViewItem.ListViewSubItem)
                            .SubItems(1).Text = lot.SelectSingleNode("qty").InnerText
                            If String.Compare(selectedlot, lot.SelectSingleNode("name").InnerText) = 0 Then
                                .Selected = True
                            Else
                                .Selected = False
                            End If
                        End With
                    End With
                End If
            Next
        End With
    End Sub



#End Region

End Class
