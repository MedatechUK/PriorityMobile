Imports System.Xml
Imports System.Text.RegularExpressions

Public Enum tExitDirection
    Up = -1
    Down = 1
    Selected = 2
End Enum

Public Class CtrlColumn

    Public Declare Sub keybd_event Lib "coredll.dll" (ByVal bVK As Byte, _
    ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo _
    As Integer)

#Region "Events"

    Public Event AcceptData()

#End Region

#Region "Private Variables"

    Private thisform As iForm
    Private EditControl As Control
    Private TextBuffer As String

#End Region

#Region "Enumerations"

    Enum tColumnType
        colText = 1
        colDate = 3
        colBoolean = 4
        colSign = 5
    End Enum

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef Sender As iForm, ByVal ColumnNode As XmlNode)

        InitializeComponent()
        thisform = Sender

        With ColumnNode

            If IsNothing(.SelectSingleNode("name")) Then Throw New Exception("Invalid Node.")
            If IsNothing(.SelectSingleNode("title")) Then Throw New Exception("Invalid Node.")
            If IsNothing(.SelectSingleNode("type")) Then Throw New Exception("Invalid Node.")

            _ColumnName = .SelectSingleNode("name").InnerText
            Me.ControlLabel.Text = .SelectSingleNode("title").InnerText

            Select Case .SelectSingleNode("type").InnerText.ToLower
                Case "date"
                    _ColumnType = tColumnType.colDate
                    EditControl = edit_Date
                Case "boolean"
                    _ColumnType = tColumnType.colBoolean
                    EditControl = Edit_Boolean
                Case "vector"
                    _ColumnType = tColumnType.colSign
                Case Else
                    _ColumnType = tColumnType.colText
                    EditControl = Edit_Text
            End Select

            For Each sqlNode As XmlNode In .SelectNodes("sql")
                If Not IsNothing(sqlNode.Attributes("name")) Then
                    If Not SQL.Keys.Contains(sqlNode.Attributes("name").Value) Then
                        SQL.Add(sqlNode.Attributes("name").Value, sqlNode.InnerText)
                    End If
                End If
            Next

            If SQL.Keys.Contains("list") Then ListParam(_Depends, _SQL("list"))
            If SQL.Keys.Contains("validation") Then ListParam(_Depends, _SQL("validation"))

            Dim ValidationNode As XmlNode = .SelectSingleNode("validexp")
            If Not IsNothing(ValidationNode) Then
                If String.Compare(ValidationNode.InnerText.Substring(1, 1), "#") = 0 Then
                    If ValidationExp.Keys.Contains(ValidationNode.InnerText.Substring(2)) Then
                        _ValidRegex = ValidationExp(ValidationNode.InnerText.Substring(2))
                    Else
                        Throw New Exception(String.Format("Referenced Expression {0} not found", ValidationNode.InnerText))
                    End If
                Else
                    _ValidRegex = ValidationNode.InnerText
                End If
            Else
                _ValidRegex = ".*"
            End If

            Dim ReadonlyNode As XmlNode = .SelectSingleNode("readonly")
            If Not IsNothing(ReadonlyNode) Then
                _ColumnReadOnly = String.Compare(ReadonlyNode.InnerText.ToLower, "true", True) = 0
                Me.ControlLabel.Enabled = False
            Else
                _ColumnReadOnly = False
            End If

            Dim titleNode As XmlNode = .SelectSingleNode("enabled")
            If Not IsNothing(titleNode) Then
                _ColumnEnabled = String.Compare(titleNode.InnerText.ToLower, "true", True) = 0
            Else
                _ColumnEnabled = True
            End If

            Dim visibleNode As XmlNode = .SelectSingleNode("visible")
            If Not IsNothing(visibleNode) Then
                _ColumnVisible = String.Compare(visibleNode.InnerText.ToLower, "true", True) = 0
            Else
                _ColumnVisible = True
            End If

            Dim widthNode As XmlNode = .SelectSingleNode("width")
            If Not IsNothing(widthNode) Then
                If IsNumeric(widthNode.InnerText) Then
                    _ColumnWidth = CInt(widthNode.InnerText)
                Else
                    _ColumnWidth = 40
                End If
            Else
                _ColumnWidth = 40
            End If

            Dim alignNode As XmlNode = .SelectSingleNode("align")
            If Not IsNothing(alignNode) Then
                Select Case alignNode.InnerText.ToLower
                    Case "left"
                        _ColumnAlignment = HorizontalAlignment.Left
                    Case "right"
                        _ColumnAlignment = HorizontalAlignment.Right
                    Case "center"
                        _ColumnAlignment = HorizontalAlignment.Center
                    Case Else
                End Select
            Else
                _ColumnAlignment = HorizontalAlignment.Left
            End If            

        End With

        Dim ControlLabelTextLength As Integer = ControlLabel.Text.Length + 0.5
        Dim thisformCharWidth As Integer = thisform.CharWidth

        For Each ctrl As Control In Me.Controls
            ctrl.Font = thisform.ControlFont
            With ctrl
                .Top = 1
                If String.Compare(ctrl.Name.ToLower, "controllabel") = 0 Then                    
                    .Left = 1
                    .Width = thisformCharWidth * (ControlLabelTextLength)
                Else
                    .Left = thisformCharWidth * (ControlLabelTextLength)
                    .Width = (Me.Width - 2) - .Left
                End If
            End With
            AddHandler ctrl.KeyPress, AddressOf hKeyPress
            AddHandler ctrl.KeyDown, AddressOf hKeyDown
        Next
        AddHandler Me.KeyPress, AddressOf hKeyPress
        AddHandler Me.KeyDown, AddressOf hKeyDown

    End Sub

#Region "Initialisation functions"

    Private Sub ListParam(ByRef Parameters As List(Of String), ByVal Statement As String)
        For Each SQLParameter As Match In Regex.Matches(Statement, "\%[A-Za-z0-9]+\%")
            If Not String.Compare(SQLParameter.Value, "%ME%", True) = 0 Then
                If Not Parameters.Contains(TrimVar(SQLParameter.Value)) Then
                    Parameters.Add(TrimVar(SQLParameter.Value))
                End If
            End If
        Next
    End Sub

#End Region

#End Region

#Region "Activation and Deactivation"

    Public Function ActivateControl() As Boolean
        If Not CanActivate Then Return False
        For Each c As CtrlColumn In ThisContainer.formContainer.Controls
            If c.ControlActive Then
                thisform.BeginColumnAccept(c)
                If c.ControlActive Then Return False
            End If
        Next
        With Me
            .ControlActive = True
            .Display_Value.Visible = False
            With .EditControl
                .Text = Me.Value
                .Visible = True
            End With
            .ControlLabel.Focus()
        End With
        Return True
    End Function

    Public Sub DeactivateControl()
        With Me
            .ControlActive = False
            With .Display_Value
                .Text = Me.Value
                .Visible = True
            End With
            .EditControl.Visible = False
        End With
        ThisContainer.ControlLostFocus(Me)
    End Sub

#End Region

#Region "Public Properies"

    Public ReadOnly Property CanActivate()
        Get
            Return Not (_ColumnReadOnly) And _ColumnEnabled And _ColumnVisible
        End Get
    End Property

#Region "Mandatory Fields"

    Private _ColumnType As tColumnType
    Public ReadOnly Property ColumnType() As tColumnType
        Get
            Return _ColumnType
        End Get
    End Property

    Private _ColumnName As String
    Public ReadOnly Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
    End Property

    Private _ColumnTitle As String
    Public ReadOnly Property ColumnTitle() As String
        Get
            Return _ColumnTitle
        End Get
    End Property

#End Region

#Region "Optional Fields"

    Private _ValidRegex As String
    Public ReadOnly Property ColumnRegex() As String
        Get
            Return _ValidRegex
        End Get
    End Property

    Private _ColumnReadOnly As Boolean
    Public Property ColumnReadOnly() As Boolean
        Get
            Return _ColumnReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ColumnReadOnly = value
        End Set
    End Property

    Private _ColumnEnabled As Boolean = True
    Public Property ColumnEnabled() As Boolean
        Get
            Return _ColumnEnabled
        End Get
        Set(ByVal value As Boolean)
            _ColumnEnabled = value
            Me.ControlLabel.Enabled = value
        End Set
    End Property

    Private _ColumnVisible As Boolean
    Public Property ColumnVisible() As Boolean
        Get
            Return _ColumnVisible
        End Get
        Set(ByVal value As Boolean)
            _ColumnVisible = value
        End Set
    End Property

    Private _ColumnWidth As Integer
    Public Property ColumnWidth() As Integer
        Get
            Return _ColumnWidth
        End Get
        Set(ByVal value As Integer)
            _ColumnWidth = value
        End Set
    End Property

    Private _ColumnAlignment As Windows.Forms.HorizontalAlignment
    Public Property ColumnAlignment() As Windows.Forms.HorizontalAlignment
        Get
            Return _ColumnAlignment
        End Get
        Set(ByVal value As Windows.Forms.HorizontalAlignment)
            _ColumnAlignment = value
        End Set
    End Property

#End Region

#Region "Parent Container property"

    Public Sub SetContainer(ByVal thisContainer As ctrlBase)
        _thisContainer = thisContainer
    End Sub

    Private _thisContainer As ctrlBase
    Public ReadOnly Property ThisContainer() As ctrlBase
        Get
            Return _thisContainer
        End Get
    End Property

#End Region

    Private _ControlActive As Boolean
    Public Property ControlActive() As Boolean
        Get
            Return _ControlActive
        End Get
        Set(ByVal value As Boolean)
            _ControlActive = value
        End Set
    End Property

    Private _value As String = ""
    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
            Me.DeactivateControl()
            RaiseEvent AcceptData()
        End Set
    End Property

    Private _SQL As New Dictionary(Of String, String)
    Public Property SQL() As Dictionary(Of String, String)
        Get
            Return _SQL
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _SQL = value
        End Set
    End Property

    Private _Depends As New List(Of String)
    Public ReadOnly Property Depends() As List(Of String)
        Get
            Return _Depends
        End Get
    End Property

    Private _ExitDirection As tExitDirection = tExitDirection.Down
    Public Property ExitDirection() As tExitDirection
        Get
            Return _ExitDirection
        End Get
        Set(ByVal value As tExitDirection)
            _ExitDirection = value
        End Set
    End Property

    Public ReadOnly Property ProposedValue() As String
        Get
            If Not EditControl.Visible Then Return Nothing
            Select Case Me.ColumnType
                Case tColumnType.colBoolean
                    Select Case Me.Edit_Boolean.Checked
                        Case True
                            If Value = "Y" Then Return Nothing
                            Return "Y"
                        Case Else
                            If Value = "" Then Return Nothing
                            Return ""
                    End Select

                Case tColumnType.colText
                    If String.Compare(Value, TextBuffer) = 0 Then Return Nothing
                    Return TextBuffer

                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

#End Region

#Region "Public methods"

    Public Sub hDependancyCheck()
        Dim myDepends As Boolean = True
        For Each Dependancy As String In Depends
            Dim ctrl As CtrlColumn = thisform.FindControl(Dependancy)
            If ctrl.Value.Length = 0 Then
                myDepends = False
                Exit For
            End If
        Next
        If myDepends Then
            If Not (ColumnReadOnly) And ColumnVisible Then
                Me.ColumnEnabled = True
            Else
                Me.ColumnEnabled = False
            End If
        Else
            Me.ColumnEnabled = False
        End If
    End Sub

#End Region

#Region "Form Event Handlers"

#Region "Keyboard event Handlers"

    Private Sub hKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        e.Handled = True
        Select Case e.KeyChar
            Case vbCrLf
                Select Case InStr(TextBuffer, "==")
                    Case 0
                        _ExitDirection = tExitDirection.Down
                        thisform.BeginColumnAccept(Me)
                    Case Else
                        Dim MultiValBarcode As New Dictionary(Of String, String)
                        For Each pair As String In TextBuffer.Split("&&")
                            If pair.Trim.Length > 0 And InStr(pair, "==") > 0 Then
                                Dim col As String = pair.Split("==")(0)
                                Dim val As String = pair.Split("==")(1)
                                If col.Length > 0 And val.Length > 0 Then
                                    MultiValBarcode.Add(col, val)
                                End If
                            End If
                        Next
                        If MultiValBarcode.Count > 0 Then
                            thisform.BeginColumnAccept(MultiValBarcode)
                        End If
                End Select

            Case Else
                TextBuffer += e.KeyChar
                If Edit_Text.Visible Then
                    Edit_Text.Text = TextBuffer
                End If

        End Select

    End Sub

    Private Sub hKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        e.Handled = True
        Select Case e.KeyValue
            Case 113
                keybd_event(&H73, 0, &H1, 0)
                keybd_event(&H73, 0, &H1, 0)
                Exit Sub
            Case 40
                ExitDirection = tExitDirection.Down
                thisform.BeginColumnAccept(Me)
            Case 39
                ExitDirection = tExitDirection.Down
                thisform.BeginColumnAccept(Me)
            Case 38
                ExitDirection = tExitDirection.Up
                thisform.BeginColumnAccept(Me)
            Case 37
                ExitDirection = tExitDirection.Up
                thisform.BeginColumnAccept(Me)
                'Case Else
                '    If e.KeyValue = 115 Then
                '        RaiseEvent ClickDataEntry(sender, e)
                '    End If
                '    Exit Sub
        End Select

    End Sub

#End Region

    Private Sub ControlLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ControlLabel.Click
        ExitDirection = tExitDirection.Selected
        Me.ActivateControl()
    End Sub

#End Region

End Class
