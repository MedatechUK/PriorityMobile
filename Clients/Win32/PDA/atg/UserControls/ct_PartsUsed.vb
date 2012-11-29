Imports PDA.o

Public Class ct_PartsUsed

    Enum tCalcControl
        eUsedParts = 0
        eWarehouse = 1
    End Enum

    Enum tCalcAction
        eAdd = 0
        eUpdate = 1
    End Enum

    Dim ar As Priority.MyArray

    Dim _CalcControl As tCalcControl
    Dim _CalcOp As tCalcAction

    Dim _ServiceCall As String = ""
    Dim _App As PDAOnBoardData.BaseForm

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            RefreshPartsUsed()
        End Set
    End Property

    Public Sub New(ByRef App As PDAOnBoardData.BaseForm) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _App = App

        With pnl_Parts
            .Top = 0
            .Left = 0
            .Width = Me.Width
            .Height = Me.Height
            .Visible = True
        End With

        RefreshWarehouse()

        ar = New Priority.MyArray

    End Sub

    Private Sub RefreshPartsUsed()

        With UsedParts
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("Part Number")
            .Columns.Add("Part Description")
            .Columns.Add("QTY")
            _App.rss(o.Parts).currentIndex = _ServiceCall
            Dim sel() As Integer = _App.rss(o.Parts).Selection
            For i As Integer = 0 To UBound(sel)
                .Items.Add("")
                .Items(.Items.Count - 1).Text = _App.rss(o.Parts).thisArray(2, sel(i))
                _App.rss(o.Warehouse).currentIndex = _App.rss(o.Parts).thisArray(2, sel(i))
                .Items(.Items.Count - 1).SubItems.Add(_App.rss(o.Warehouse).GetField("PARTDES"))
                .Items(.Items.Count - 1).SubItems.Add(_App.rss(o.Parts).thisArray(3, sel(i)))
            Next
            If .Items.Count > 0 Then
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
            Else
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
            End If
        End With

    End Sub

    Public Sub RefreshWarehouse()

        With Warehouse
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("Part Number")
            .Columns.Add("Part Description")
            .Columns.Add("QTY")

            If Not IsNothing(_App.rss(o.Warehouse).thisArray) Then
                For i As Integer = 0 To UBound(_App.rss(o.Warehouse).thisArray, 2)
                    .Items.Add("")
                    .Items(.Items.Count - 1).Text = _App.rss(o.Warehouse).thisArray(0, i)
                    .Items(.Items.Count - 1).SubItems.Add(_App.rss(o.Warehouse).thisArray(1, i))
                    .Items(.Items.Count - 1).SubItems.Add(_App.rss(o.Warehouse).thisArray(2, i))
                Next
            End If

            If .Items.Count > 0 Then
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
            Else
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
            End If

        End With

    End Sub

    Private Sub ct_PartsUsed_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnl_Parts.Resize

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

        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        Me.UsedParts.Font = c
        Me.Warehouse.Font = c

        Me.Warehouse.Height = (pnl_Parts.Height - 5) / 2
        Me.UsedParts.Height = (pnl_Parts.Height - 5) / 2

        Try
            With Warehouse
                If .Items.Count > 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                End If
            End With

            With UsedParts
                If .Items.Count > 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                End If
            End With
        Catch
        End Try

    End Sub

    Private Sub warehouse_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Warehouse.MouseUp

        Dim s As ListView.SelectedListViewItemCollection = Warehouse.SelectedItems
        If s.Count = 0 Then Exit Sub

        _App.rss(o.Warehouse).currentIndex = s.Item(0).Text
        If s.Count > 0 Then
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Right
                    Dim menu As New ContextMenuStrip

                    With menu
                        .Items.Add("")
                        .Items(.Items.Count - 1).Font = Me.Warehouse.Font
                        .Items(.Items.Count - 1).Text = "Add part to call"
                        .Items(.Items.Count - 1).Name = "Add"
                        AddHandler .Items(.Items.Count - 1).Click, AddressOf hClickAdd

                    End With
                    Warehouse.ContextMenuStrip = menu

            End Select
        End If

    End Sub

    Private Sub UsedParts_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UsedParts.MouseUp

        Dim s As ListView.SelectedListViewItemCollection = UsedParts.SelectedItems
        If s.Count > 0 Then
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Right
                    Dim menu As New ContextMenuStrip

                    With menu
                        .Items.Add("")
                        .Items(.Items.Count - 1).Font = Me.Warehouse.Font
                        .Items(.Items.Count - 1).Text = "Remove part from call"
                        .Items(.Items.Count - 1).Name = "Del"
                        AddHandler .Items(.Items.Count - 1).Click, AddressOf hClickDel
                        .Items.Add("")
                        .Items(.Items.Count - 1).Font = Me.Warehouse.Font
                        .Items(.Items.Count - 1).Text = "Update Quantity"
                        .Items(.Items.Count - 1).Name = "Upd"
                        AddHandler .Items(.Items.Count - 1).Click, AddressOf hClickUpd

                    End With
                    UsedParts.ContextMenuStrip = menu

            End Select
        End If

    End Sub

    Private Sub hClickAdd(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim s As ListView.SelectedListViewItemCollection = Warehouse.SelectedItems
        If s.Count = 0 Then Exit Sub

        Me._CalcOp = tCalcAction.eAdd
        Me._CalcControl = tCalcControl.eWarehouse
        Me.Ct_number1.Max = CInt(Warehouse.Items(s(0).Index).SubItems(2).Text)

        ShowPanel(pnl_Parts, False)
        ShowPanel(pnl_Number, True)

    End Sub

    Private Sub hClickDel(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim Part As String = ""
        Dim Qty As Integer = 0

        Dim s As ListView.SelectedListViewItemCollection = UsedParts.SelectedItems
        If s.Count = 0 Then Exit Sub

        With _App.rss(o.Parts)
            .currentIndex = _ServiceCall & "/" & UsedParts.Items(s(0).Index).Text
            Dim i(0) As String
            i(0) = .currentOrdinal
            Part = .GetField("PARTNAME")
            Qty = CInt(.GetField("QTY"))
            ar.DeleteByIndex(.thisArray, i)
            .Save()
        End With
        UsedParts.Items(s(0).Index).Remove()

        With _App.rss(o.Warehouse)
            .currentIndex = Part
            .SetField("QTY", CStr(CInt(.GetField("QTY")) + Qty))
            .Save()
        End With

        RefreshWarehouse()

    End Sub

    Private Sub hClickUpd(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim s As ListView.SelectedListViewItemCollection = UsedParts.SelectedItems
        If s.Count = 0 Then Exit Sub

        Me._CalcControl = tCalcControl.eUsedParts
        Me._CalcOp = tCalcAction.eUpdate
        Me.Ct_number1.Max = CInt(UsedParts.Items(s(0).Index).SubItems(2).Text)
        _App.rss(o.Warehouse).currentIndex = UsedParts.Items(s(0).Index).Text
        Me.Ct_number1.Max = Me.Ct_number1.Max + CInt(_App.rss(o.Warehouse).GetField("QTY"))

        ShowPanel(pnl_Parts, False)
        ShowPanel(pnl_Number, True)

    End Sub

    Private Sub Ct_number1_SetNumber(ByVal MyValue As Integer) Handles Ct_number1.SetNumber

        Dim oldqty As Integer
        Dim s As ListView.SelectedListViewItemCollection = Nothing

        With _App.rss(o.Parts)
            Select Case Me._CalcControl
                Case tCalcControl.eWarehouse
                    s = Warehouse.SelectedItems
                    If s.Count = 0 Then Exit Sub
                    .currentIndex = _ServiceCall & "/" & s(0).Text
                Case tCalcControl.eUsedParts
                    s = UsedParts.SelectedItems
                    If s.Count = 0 Then Exit Sub
                    .currentIndex = _ServiceCall & "/" & s(0).Text
            End Select

            If Not .Validate Then
                Me._CalcOp = tCalcAction.eAdd
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = _ServiceCall & "/" & s(0).Text
                .thisArray(1, nr) = Split(.currentIndex, "/")(0)
                .thisArray(2, nr) = Split(.currentIndex, "/")(1)
                .thisArray(3, nr) = CStr(MyValue)
                .Save()
            Else
                oldqty = CInt(.GetField("QTY"))
                Select Case Me._CalcOp
                    Case tCalcAction.eAdd
                        .SetField("QTY", CStr(oldqty + MyValue))
                    Case tCalcAction.eUpdate
                        .SetField("QTY", CStr(MyValue))
                End Select
                .Save()
            End If
        End With

        With _App.rss(o.Warehouse)

            Select Case Me._CalcControl
                Case tCalcControl.eWarehouse
                    s = Warehouse.SelectedItems
                    .currentIndex = Warehouse.Items(s(0).Index).Text
                Case tCalcControl.eUsedParts
                    s = UsedParts.SelectedItems
                    .currentIndex = UsedParts.Items(s(0).Index).Text
            End Select

            If s.Count = 0 Then Exit Sub

            Select Case Me._CalcOp
                Case tCalcAction.eAdd
                    .SetField("QTY", CStr(CInt(.GetField("QTY")) - MyValue))
                Case tCalcAction.eUpdate
                    .SetField("QTY", CStr(CInt(.GetField("QTY")) + oldqty - MyValue))
            End Select
            .Save()

        End With

        ShowPanel(pnl_Number, False)
        ShowPanel(pnl_Parts, True)

        Me.ServiceCall = _ServiceCall
        Ct_number1.Value = 0

        RefreshWarehouse()

    End Sub

    Private Sub ShowPanel(ByRef pnl As Panel, ByVal Show As Boolean)
        With pnl
            Select Case Show
                Case False
                    .Dock = DockStyle.None
                    .Top = Me.Height
                    .Left = 0
                    .Width = 0
                    .Height = 0
                    .Visible = False
                Case True
                    .Dock = DockStyle.Fill
                    .Top = 0
                    .Left = 0
                    .Width = Me.Width
                    .Height = Me.Height
                    .Visible = True
            End Select
        End With

    End Sub
End Class
