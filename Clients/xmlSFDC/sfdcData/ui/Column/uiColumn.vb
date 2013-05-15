Imports System
Imports System.Xml

Public Class uiColumn

    Private _ScanBuffer As String = ""
    Private _2d As Boolean = False
    Private _thisColumn As cColumn
    Private _Parent As ColumnPanel
    Private _mode As eMode = eMode.label
    Private _mandatory As Boolean = False
    Private _SwitchView As Boolean

    Private _Selected As Boolean = False
    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            _Selected = value
        End Set
    End Property

    Public Overloads ReadOnly Property Parent() As ColumnPanel
        Get
            Return _Parent
        End Get
    End Property

    Public Sub New(ByRef Parent As ColumnPanel, ByRef Col As cColumn)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _Parent = Parent
        _thisColumn = Col

        With _thisColumn
            .SetControl(Me)
            _mandatory = .Mandatory
            Label.Text = .Title
        End With

    End Sub

#Region "Column Style"

#Region "Enumerations"

    Public Enum eColStyle
        colReadOnly = 0
        colSelected = 1
        colDeselected = 2
        colList = 3
    End Enum

    Private Enum eMode
        label
        list
    End Enum

#End Region

    Private _ColStyle As eColStyle = eColStyle.colDeselected
    Public Property ColStyle() As eColStyle
        Get
            Return _ColStyle
        End Get
        Set(ByVal value As eColStyle)
            With Me
                _ColStyle = value

                Select Case _ColStyle
                    Case eColStyle.colDeselected
                        If Not _mode = eMode.label Then ShowCtrl(eMode.label)
                        With .lbl_Value
                            .ForeColor = Drawing.Color.Black
                            .BackColor = Me.BackColor
                        End With
                        With Label
                            '.Enabled = True
                            Select Case _mandatory
                                Case True
                                    .ForeColor = Drawing.Color.Red
                                Case Else
                                    .ForeColor = Drawing.Color.Blue
                            End Select
                        End With

                    Case eColStyle.colSelected
                        If Not _mode = eMode.label Then ShowCtrl(eMode.label)
                        With .lbl_Value
                            .Text = ""
                            .ForeColor = Drawing.Color.Black
                            .BackColor = Drawing.Color.Red
                        End With
                        With Label
                            '.Enabled = True
                            Select Case _mandatory
                                Case True
                                    .ForeColor = Drawing.Color.Blue
                                Case Else
                                    .ForeColor = Drawing.Color.Black
                            End Select
                        End With

                    Case eColStyle.colReadOnly
                        With .lbl_Value
                            .Visible = True
                            .ForeColor = Drawing.Color.DarkGray
                            .BackColor = Me.BackColor
                        End With
                        With .Label
                            '.Enabled = False
                            .ForeColor = Drawing.Color.DarkGray
                        End With

                    Case eColStyle.colList
                        If Not _mode = eMode.list Then
                            ShowCtrl(eMode.list)
                            If _thisColumn.Triggers.Keys.Contains("CHOOSE-FIELD") Then
                                _thisColumn.Triggers("CHOOSE-FIELD").Execute()
                            End If
                        End If
                        With Label
                            '.Enabled = True
                            Select Case _mandatory
                                Case True
                                    .ForeColor = Drawing.Color.Blue
                                Case Else
                                    .ForeColor = Drawing.Color.Black
                            End Select
                        End With

                End Select

            End With
        End Set
    End Property

    Private Sub ShowCtrl(ByVal show As eMode)

        _SwitchView = True
        Select Case show
            Case eMode.label

                With lbl_Value
                    .Dock = Windows.Forms.DockStyle.Right
                    .Width = Me.Width - Label.Width - 2
                    .Visible = True
                End With
                Label.Focus()

                With list
                    .Visible = False
                    .Dock = Windows.Forms.DockStyle.None
                End With

                _mode = eMode.label

            Case eMode.list
                With lbl_Value
                    .Visible = False
                    .Dock = Windows.Forms.DockStyle.None
                End With

                With list
                    .Dock = Windows.Forms.DockStyle.Right
                    .Width = Me.Width - Label.Width - 2
                    .Visible = True
                    .Focus()
                End With

                _mode = eMode.list

        End Select
        _SwitchView = False

    End Sub

#End Region

#Region "Key events"

    Private Sub uiColumn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Label.KeyDown

        Select Case e.KeyValue
            Case 40, 39, 38, 37
                e.Handled = True
                With Me
                    .ColStyle = eColStyle.colDeselected
                    .Selected = False
                End With
                Select Case e.KeyValue
                    Case 40, 39
                        Parent.SelectNextControl(Me, True, True, False, True)
                    Case 38, 37
                        Parent.SelectNextControl(Me, False, True, False, True)
                End Select

                For Each ctrl As uiColumn In Parent.Controls
                    If ctrl.Label.Focused Then
                        With ctrl
                            ctrl.ColStyle = eColStyle.colSelected
                            .Selected = True
                            Exit For
                        End With
                    End If
                Next

            Case 32
                e.Handled = True
                If _ScanBuffer.Length = 0 Then
                    Me.ColStyle = eColStyle.colList
                Else
                    _ScanBuffer += Chr(e.KeyValue)
                    If Not _2d Then
                        lbl_Value.Text = _ScanBuffer
                    End If
                End If

            Case 60
                e.Handled = True
                If _ScanBuffer.Length = 0 Then
                    _2d = True
                End If
                _ScanBuffer += "<"

            Case 13
                e.Handled = True

                Select Case _2d
                    Case True
                        Dim doc As New Xml.XmlDocument
                        doc.LoadXml(_ScanBuffer)
                        For Each item As XmlNode In doc.SelectNodes("in/i")
                            If _thisColumn.Parent.Columns.Keys.Contains(item.Attributes("n").Value) Then
                                _thisColumn.Parent.Columns(item.Attributes("n").Value).Value = item.Attributes("v").Value
                            End If
                        Next

                    Case Else
                        _thisColumn.Value = _ScanBuffer

                End Select

                _2d = False
                _ScanBuffer = ""

            Case Else
                e.Handled = True
                _ScanBuffer += Chr(e.KeyValue)
                If Not _2d Then
                    lbl_Value.Text = _ScanBuffer
                End If

        End Select

    End Sub

    Private Sub list_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles list.KeyDown
        Select Case e.KeyValue
            Case 13
                e.Handled = True
                With Me
                    .lbl_Value.Text = list.SelectedText
                    .ColStyle = eColStyle.colSelected
                End With
                uiColumn_KeyPress(sender, e)
                'Me.Focus()
        End Select
    End Sub

#End Region

#Region "Select / Deselect"

    Private Sub Label_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label.Click        
        For Each uiCol As uiColumn In Parent.Controls
            With uiCol
                If .Selected Then
                    .Deselect()
                End If
            End With
        Next
        Me.Focus()
    End Sub

    Private Sub uiColumn_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        With Me            
            .ColStyle = eColStyle.colSelected
            .Selected = True            
        End With
    End Sub

    Public Sub Deselect()
        With Me
            If Not _mode = eMode.label Then ShowCtrl(eMode.label)
            .ColStyle = eColStyle.colDeselected
            .Selected = False
        End With
    End Sub

#End Region

    Private Sub uiColumn_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Label.Width = 150
        lbl_Value.Width = Me.Width - Label.Width - 2
        list.Width = Me.Width - Label.Width - 2
    End Sub

#Region "Dummy Scans"

    Private Sub ScanBarcode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scan.Click
        Dim DummyScan As String = InputBox("Scan Barcode")
        DummyScan += Chr(13)
        For i As Integer = 0 To DummyScan.Length
            Dim ea As New System.Windows.Forms.KeyEventArgs(DummyScan.Substring(i, 1))
            uiColumn_KeyPress(Me, ea)
        Next

    End Sub

    Private Sub Scan2dBarcode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scan2d.Click
        Dim DummyScan As String = "<in><i n='PART' v='PART123'/><i n='CUST' v='Goods'/><i n='SERIAL' v='123-456-789'/><i n='LOT' v='123-456-789'/><i n='ACT' v='123-456-789'/></in>".Replace("'", Chr(34))
        DummyScan += Chr(13)
        For i As Integer = 0 To DummyScan.Length - 1
            Dim ea As New System.Windows.Forms.KeyEventArgs(Asc(DummyScan.Substring(i, 1)))
            uiColumn_KeyPress(Me, ea)
        Next
    End Sub

#End Region

End Class
