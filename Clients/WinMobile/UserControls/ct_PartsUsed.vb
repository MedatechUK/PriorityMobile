Imports cfpda.o

Public Class ct_PartsUsed

    Enum tCalcControl
        eUsedParts = 0
        eWarehouse = 1
    End Enum

    Enum tCalcAction
        eAdd = 0
        eUpdate = 1
    End Enum

    Dim ar As cfMyCls.MyArray

    Dim _CalcControl As tCalcControl
    Dim _CalcOp As tCalcAction

    Dim _ServiceCall As String = ""
    Dim CallerApp As cfOnBoardData.BaseForm

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            RefreshPartsUsed()
        End Set
    End Property

    Public Sub New(ByRef App As cfOnBoardData.BaseForm) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CallerApp = App

        With pnl_Parts
            .Top = 0
            .Left = 0
            .Width = Me.Width
            .Height = Me.Height
            .Visible = True
        End With

        RefreshWarehouse()
        RefreshPartsUsed()

        ar = New cfMyCls.MyArray

    End Sub

    Private Sub RefreshPartsUsed()

        Dim ch1 As New ColumnHeader
        Dim ch2 As New ColumnHeader
        Dim ch3 As New ColumnHeader
        With UsedParts
            .Items.Clear()
            .Columns.Clear()
            ch1.Text = "Part Number"
            .Columns.Add(ch1)
            ch2.Text = "Part Description"
            .Columns.Add(ch2)
            ch3.Text = "QTY"
            .Columns.Add(ch3)
            CallerApp.rss(o.Parts).currentIndex = _ServiceCall
            Dim sel() As Integer = CallerApp.rss(o.Parts).Selection
            For i As Integer = 0 To UBound(sel)
                Dim lvi As New ListViewItem
                .Items.Add(lvi)
                .Items(.Items.Count - 1).Text = CallerApp.rss(o.Parts).thisArray(2, sel(i))
                CallerApp.rss(o.Warehouse).currentIndex = CallerApp.rss(o.Parts).thisArray(2, sel(i))
                .Items(.Items.Count - 1).SubItems.Add(CallerApp.rss(o.Warehouse).GetField("PARTDES"))
                .Items(.Items.Count - 1).SubItems.Add(CallerApp.rss(o.Parts).thisArray(3, sel(i)))
            Next
            AutoSizeListView(UsedParts, CallerApp.Width - 25, 1)

        End With

    End Sub

    Public Sub RefreshWarehouse()

        Dim ch1 As New ColumnHeader
        Dim ch2 As New ColumnHeader
        Dim ch3 As New ColumnHeader

        With Warehouse
            .Items.Clear()
            .Columns.Clear()
            ch1.Text = "Part Number"
            .Columns.Add(ch1)
            ch2.Text = "Part Description"
            .Columns.Add(ch2)
            ch3.Text = "QTY"
            .Columns.Add(ch3)

            If Not IsNothing(CallerApp.rss(o.Warehouse).thisArray) Then
                For i As Integer = 0 To UBound(CallerApp.rss(o.Warehouse).thisArray, 2)
                    Dim lvi As New ListViewItem
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = CallerApp.rss(o.Warehouse).thisArray(0, i)
                    .Items(.Items.Count - 1).SubItems.Add(CallerApp.rss(o.Warehouse).thisArray(1, i))
                    .Items(.Items.Count - 1).SubItems.Add(CallerApp.rss(o.Warehouse).thisArray(2, i))
                Next
            End If

            AutoSizeListView(Warehouse, Me.Width - 25, 1)

        End With

    End Sub

    Private Sub ct_PartsUsed_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnl_Parts.Resize

        Dim fs As Single = GetFontSize(Me.Width)

        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        Me.UsedParts.Font = c
        Me.Warehouse.Font = c

        Me.Warehouse.Height = (pnl_Parts.Height) / 2
        Me.UsedParts.Height = (pnl_Parts.Height) / 2

        AutoSizeListView(Warehouse, Me.Width - 25, 1)
        AutoSizeListView(UsedParts, Me.Width - 25, 1)

    End Sub

    Private Sub warehouse_MouseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles Warehouse.SelectedIndexChanged

        Dim s() As Integer = SelectedItems(Warehouse) 'ListView.SelectedListViewItemCollection = Warehouse.SelectedItems
        If IsNothing(s) Then Exit Sub

        Dim seltext As String = Warehouse.Items(s(0)).Text
        Dim selindex As Integer = s(0)

        CallerApp.rss(o.Warehouse).currentIndex = seltext
        If Not IsNothing(s) > 0 Then

            Dim menu As New ContextMenu
            Dim mi As New MenuItem
            With menu
                .MenuItems.Add(mi)
                '.MenuItems(.MenuItems.Count - 1).Font = Me.Warehouse.Font
                .MenuItems(.MenuItems.Count - 1).Text = "Add part to call"
                '.MenuItems(.MenuItems.Count - 1).Name = "Add"
                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf hClickAdd

            End With
            Warehouse.ContextMenu = menu
            'Dim pos As System.Drawing.Point
            'pos.X = 0
            'pos.Y = 22 + s(0) * TextSize("Test", Warehouse.Font).Height
            'Warehouse.ContextMenu.Show(Warehouse, pos)

        End If

    End Sub

    Private Sub UsedParts_MouseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles UsedParts.SelectedIndexChanged

        Dim s() As Integer = SelectedItems(UsedParts) 'ListView.SelectedListViewItemCollection = UsedParts.SelectedItems

        If Not IsNothing(s) Then

            Dim menu As New ContextMenu

            With menu
                Dim mi1 As New MenuItem
                .MenuItems.Add(mi1)
                '.Items(.Items.Count - 1).Font = Me.Warehouse.Font
                .MenuItems(.MenuItems.Count - 1).Text = "Remove part from call"
                '.Items(.Items.Count - 1).Name = "Del"
                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf hClickDel
                Dim mi2 As New MenuItem
                .MenuItems.Add(mi2)
                '.Items(.Items.Count - 1).Font = Me.Warehouse.Font
                .MenuItems(.MenuItems.Count - 1).Text = "Update Quantity"
                '.Items(.Items.Count - 1).Name = "Upd"
                AddHandler .MenuItems(.MenuItems.Count - 1).Click, AddressOf hClickUpd

            End With
            UsedParts.ContextMenu = menu
            'Dim pos As System.Drawing.Point
            'pos.X = 0
            'pos.Y = 22 + s(0) * TextSize("Test", UsedParts.Font).Height
            'UsedParts.ContextMenu.Show(UsedParts, pos)
        End If

    End Sub

    Private Sub hClickAdd(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim s() As Integer = SelectedItems(Warehouse)
        If IsNothing(s) Then Exit Sub

        Me._CalcOp = tCalcAction.eAdd
        Me._CalcControl = tCalcControl.eWarehouse
        Me.Ct_number1.Max = CInt(Warehouse.Items(s(0)).SubItems(2).Text)

        ShowPanel(pnl_Parts, False)
        ShowPanel(pnl_Number, True)

    End Sub

    Private Sub hClickDel(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim sel As ListViewItem = Nothing
        For Each l As ListViewItem In Warehouse.Items
            If LCase(Trim(l.SubItems(0).Text)) = _
            LCase(Trim(UsedParts.Items(UsedParts.SelectedIndices(0)).SubItems(0).Text)) Then
                sel = l
                Exit For
            End If
        Next

        If Not IsNothing(sel) Then

            Dim total As Integer = CInt(sel.SubItems(2).Text) + CInt(UsedParts.Items(UsedParts.SelectedIndices(0)).SubItems(2).Text)
            sel.SubItems(2).Text = total

            Dim Part As String = ""
            Dim Qty As Integer = 0

            Dim s() As Integer = SelectedItems(UsedParts)
            If IsNothing(s) Then Exit Sub
            Dim seltext As String = UsedParts.Items(s(0)).Text
            Dim selindex As Integer = s(0)

            With CallerApp.rss(o.Parts)
                .currentIndex = _ServiceCall & "/" & seltext
                Dim i(0) As String
                i(0) = .currentOrdinal
                Part = .GetField("PARTNAME")
                Qty = 0 'CInt(.GetField("QTY"))
                ar.DeleteByIndex(.thisArray, i)
                .Save()
            End With
            Remove(UsedParts, selindex)

            With CallerApp.rss(o.Warehouse)
                .currentIndex = Part
                .SetField("QTY", CStr(total))
                .Save()
            End With

        End If
        'RefreshWarehouse()

    End Sub

    Private Sub hClickUpd(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim s() As Integer = SelectedItems(UsedParts) 'ListView.SelectedListViewItemCollection = 
        If IsNothing(s) Then Exit Sub
        Dim seltext As String = UsedParts.Items(s(0)).Text
        Dim selindex As Integer = s(0)

        Me._CalcControl = tCalcControl.eUsedParts
        Me._CalcOp = tCalcAction.eUpdate
        Me.Ct_number1.Max = CInt(UsedParts.Items(selindex).SubItems(2).Text)
        CallerApp.rss(o.Warehouse).currentIndex = UsedParts.Items(selindex).Text
        Me.Ct_number1.Max = Me.Ct_number1.Max + CInt(CallerApp.rss(o.Warehouse).GetField("QTY"))

        ShowPanel(pnl_Parts, False)
        ShowPanel(pnl_Number, True)

    End Sub

    Private Sub Ct_number1_SetNumber(ByVal MyValue As Integer) Handles Ct_number1.SetNumber

        Dim oldqty As Integer
        Dim sc As String = ""
        Dim s() As Integer 'ListView.SelectedListViewItemCollection = Nothing

        With CallerApp.rss(o.Parts)
            Select Case Me._CalcControl
                Case tCalcControl.eWarehouse
                    s = SelectedItems(Warehouse)
                    If IsNothing(s) Then Exit Sub
                    sc = Warehouse.Items(s(0)).Text
                    .currentIndex = _ServiceCall & "/" & sc
                Case tCalcControl.eUsedParts
                    s = SelectedItems(UsedParts)
                    If IsNothing(s) Then Exit Sub
                    sc = UsedParts.Items(s(0)).Text
                    .currentIndex = _ServiceCall & "/" & sc
            End Select

            If Not .Validate Then
                Me._CalcOp = tCalcAction.eAdd
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = _ServiceCall & "/" & sc
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

                        'UsedParts.Items(usedparts.Items.)).SubItems(2).Text = CStr(oldqty + MyValue)
                End Select
                .Save()
            End If
        End With

        With CallerApp.rss(o.Warehouse)

            Select Case Me._CalcControl
                Case tCalcControl.eWarehouse
                    s = SelectedItems(Warehouse)
                    .currentIndex = Warehouse.Items(s(0)).Text
                Case tCalcControl.eUsedParts
                    s = SelectedItems(UsedParts)
                    .currentIndex = UsedParts.Items(s(0)).Text
            End Select

            If IsNothing(s) Then Exit Sub

            Select Case Me._CalcOp
                Case tCalcAction.eAdd
                    .SetField("QTY", CStr(CInt(.GetField("QTY")) - MyValue))
                    Warehouse.Items(Warehouse.SelectedIndices(0)).SubItems(2).Text = CStr(CInt(Warehouse.Items(Warehouse.SelectedIndices(0)).SubItems(2).Text)) - MyValue

                Case tCalcAction.eUpdate
                    Dim sel As ListViewItem = Nothing
                    For Each l As ListViewItem In Warehouse.Items
                        If LCase(Trim(l.SubItems(0).Text)) = _
                        LCase(Trim(UsedParts.Items(UsedParts.SelectedIndices(0)).SubItems(0).Text)) Then
                            sel = l
                            Exit For
                        End If
                    Next

                    If Not IsNothing(sel) Then
                        Dim total As Integer = CInt(sel.SubItems(2).Text) + CInt(UsedParts.Items(UsedParts.SelectedIndices(0)).SubItems(2).Text)
                        sel.SubItems(2).Text = CStr(total - MyValue)
                        .SetField("QTY", sel.SubItems(2).Text)
                    End If

            End Select
            .Save()

        End With

        ShowPanel(pnl_Number, False)
        ShowPanel(pnl_Parts, True)

        Me.ServiceCall = _ServiceCall
        Ct_number1.Value = 0

        'RefreshWarehouse()

    End Sub

    Private Sub MovePart(ByVal Move As Integer)

        Dim s() As Integer
        Dim f As Boolean = False

        Select Case Me._CalcControl
            Case tCalcControl.eWarehouse
                s = SelectedItems(Warehouse)
                Dim partname As String = Warehouse.Items(s(0)).SubItems(0).Text

                For Each l As ListViewItem In Warehouse.Items
                    If LCase(Trim(l.SubItems(0).Text)) = LCase(Trim(partname)) Then
                        l.SubItems(2).Text = CStr(CInt(l.SubItems(2).Text) + Move - Move)
                        Exit For
                    End If
                Next

                For Each l As ListViewItem In UsedParts.Items
                    If LCase(Trim(l.SubItems(0).Text)) = LCase(Trim(partname)) Then
                        l.SubItems(2).Text = CStr(CInt(l.SubItems(2).Text) + Move)
                        If CInt(l.SubItems(2).Text) <= 0 Then
                            UsedParts.Items.RemoveAt(UsedParts.Items.IndexOf(l))
                        End If
                        f = True
                        Exit For
                    End If
                Next

                If Not f Then
                    Dim lvi As New ListViewItem
                    lvi.SubItems(0).Text = Warehouse.Items(s(0)).SubItems(0).Text
                    lvi.SubItems.Add(Warehouse.Items(s(0)).SubItems(1).Text)
                    lvi.SubItems.Add(CStr(Move))
                    UsedParts.Items.Add(lvi)
                End If

            Case tCalcControl.eUsedParts
                s = SelectedItems(UsedParts)
                Dim partname As String = Warehouse.Items(s(0)).SubItems(0).Text

                Select Case Me._CalcOp
                    Case tCalcAction.eAdd

                    Case tCalcAction.eUpdate

                End Select
        End Select

    End Sub

    Private Sub ShowPanel(ByRef pnl As Panel, ByVal Show As Boolean)
        With pnl
            If .Name = Me.pnl_Number.Name And Show Then
                Me.Ct_number1.isClosing = False
            End If
            Select Case Show
                Case False
                    .Dock = DockStyle.None
                    .Top = Me.Height
                    .Left = 0
                    '.Width = 0
                    '.Height = 0
                    .Visible = False
                Case True
                    .Dock = DockStyle.Fill
                    .Top = 0
                    .Left = 0
                    '.Width = Me.Width
                    '.Height = Me.Height
                    .Visible = True
            End Select
            If .Name = Me.pnl_Number.Name And Show Then
                Me.Ct_number1.isClosing = True
            End If
        End With

    End Sub

End Class
