Imports System.Xml
Imports System.Text.RegularExpressions

Public Class CtrlColumn

#Region "Events"

    Public Event DependancyCheck()

#End Region

#Region "Private Variables"

    Private _ColumnNode As XmlNode    
    Private thisform As iForm

#End Region

#Region "Enumerations"

    Enum tColumnType
        colText = 1
        colNumber = 2
        colDate = 3
        colBoolean = 4
        colSign = 5
    End Enum

    Enum tSQLStatement
        sqlList = 1
        sqlValidation = 2
        sqlOnValid = 3
    End Enum

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef Sender As iForm, ByVal ColumnNode As XmlNode)
        InitializeComponent()
        thisform = Sender
        _ColumnNode = ColumnNode
        For Each sqlNode As XmlNode In _ColumnNode.SelectNodes("sql")
            Select Case sqlNode.Name.ToLower
                Case "list"
                    _SQL.Add(tSQLStatement.sqlList, sqlNode.InnerText)
                Case "validation"
                    _SQL.Add(tSQLStatement.sqlValidation, sqlNode.InnerText)
                Case "onvald"
                    _SQL.Add(tSQLStatement.sqlOnValid, sqlNode.InnerText)
                Case Else

            End Select
        Next
    End Sub

#End Region

#Region "Mandatory Fields"

    Public ReadOnly Property ColumnType() As tColumnType
        Get
            Static _ColumnType As tColumnType
            If IsNothing(_ColumnType) Then
                Select Case _ColumnNode.SelectSingleNode("type").InnerText.ToLower
                    Case "text"
                        _ColumnType = tColumnType.colText
                    Case "number"
                        _ColumnType = tColumnType.colNumber
                    Case "date"
                        _ColumnType = tColumnType.colDate
                    Case "boolean"
                        _ColumnType = tColumnType.colBoolean
                    Case "vector"
                        _ColumnType = tColumnType.colSign
                    Case Else
                End Select
            End If
            Return _ColumnType
        End Get
    End Property

    Public ReadOnly Property ColumnName() As String
        Get
            Static _ColumnName As String
            If IsNothing(_ColumnName) Then
                _ColumnName = _ColumnNode.SelectSingleNode("name").InnerText
            End If
            Return _ColumnName
        End Get
    End Property

    Public ReadOnly Property ColumnTitle() As String
        Get
            Static _ColumnTitle As String
            If IsNothing(_ColumnTitle) Then
                _ColumnTitle = _ColumnNode.SelectSingleNode("title").InnerText
            End If
            Return _ColumnTitle
        End Get
    End Property

#End Region

#Region "Optional Fields"

    Public ReadOnly Property ColumnRegex() As String
        Get
            Static _ValidRegex As String
            If IsNothing(_ValidRegex) Then
                Dim ValidationNode As XmlNode = _ColumnNode.SelectSingleNode("validexp")
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
                    _ValidRegex = ValidationNode.InnerText
                End If
            Else
                _ValidRegex = "^.*$"
            End If
            Return _ValidRegex
        End Get
    End Property

    Private _ColumnReadOnly As Boolean
    Public Property ColumnReadOnly() As Boolean
        Get
            Static _LD As Boolean
            If Not _LD Then
                _LD = True
                Dim ReadonlyNode As XmlNode = _ColumnNode.SelectSingleNode("readonly")
                If Not IsNothing(ReadonlyNode) Then
                    _ColumnReadOnly = String.Compare(ReadonlyNode.InnerText.ToLower, "true") = 0
                Else
                    _ColumnReadOnly = False
                End If
            End If
            Return _ColumnReadOnly
        End Get

        Set(ByVal value As Boolean)
            _ColumnReadOnly = value
        End Set
    End Property

    Private _ColumnEnabled As Boolean
    Public Property ColumnEnabled() As Boolean
        Get
            Static _LD As Boolean
            If Not _LD Then
                _LD = True
                Dim titleNode As XmlNode = _ColumnNode.SelectSingleNode("enabled")
                If Not IsNothing(titleNode) Then
                    _ColumnEnabled = String.Compare(titleNode.InnerText.ToLower, "true") = 0
                Else
                    _ColumnEnabled = True
                End If
            End If
            Return _ColumnEnabled
        End Get

        Set(ByVal value As Boolean)
            _ColumnEnabled = value
        End Set
    End Property

    Private _ColumnVisible As Boolean
    Public Property ColumnVisible() As Boolean
        Get
            Static _LD As Boolean
            If Not _LD Then
                _LD = True
                Dim visibleNode As XmlNode = _ColumnNode.SelectSingleNode("visible")
                If Not IsNothing(visibleNode) Then
                    _ColumnVisible = String.Compare(visibleNode.InnerText.ToLower, "true") = 0
                Else
                    _ColumnVisible = True
                End If
            End If
            Return _ColumnVisible
        End Get

        Set(ByVal value As Boolean)
            _ColumnVisible = value
        End Set
    End Property

    Private _ColumnWidth As Integer
    Public Property ColumnWidth() As Integer
        Get
            Static _LD As Boolean
            If Not _LD Then
                _LD = True
                Dim widthNode As XmlNode = _ColumnNode.SelectSingleNode("width")
                If Not IsNothing(widthNode) Then
                    _ColumnWidth = CInt(widthNode.InnerText)
                Else
                    _ColumnWidth = 40
                End If
            End If
            Return _ColumnWidth
        End Get
        Set(ByVal value As Integer)
            _ColumnWidth = value
        End Set
    End Property

    Private _ColumnAlignment As Windows.Forms.HorizontalAlignment
    Public Property ColumnAlignment() As Windows.Forms.HorizontalAlignment
        Get
            Static _align As Windows.Forms.HorizontalAlignment
            If IsNothing(_align) Then
                Dim alignNode As XmlNode = _ColumnNode.SelectSingleNode("align")
                If Not IsNothing(alignNode) Then
                    Select Case alignNode.InnerText.ToLower
                        Case "left"
                            _align = HorizontalAlignment.Left
                        Case "right"
                            _align = HorizontalAlignment.Right
                        Case "center"
                            _align = HorizontalAlignment.Center
                        Case Else
                    End Select
                Else
                    _align = HorizontalAlignment.Left
                End If
                _ColumnAlignment = _align
            End If
            Return _ColumnAlignment
        End Get
        Set(ByVal value As Windows.Forms.HorizontalAlignment)
            _ColumnAlignment = value
        End Set
    End Property

#End Region

#Region "Public Properies"

    Private _SQL As New Dictionary(Of tSQLStatement, String)
    Public ReadOnly Property SQL(ByVal Statement As tSQLStatement) As String
        Get
            If _SQL.Keys.Contains(Statement) Then
                Return _SQL(Statement)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property Depends() As List(Of String)
        Get
            Dim ret As New List(Of String)
            ParamList(ret, tSQLStatement.sqlList)
            ParamList(ret, tSQLStatement.sqlValidation)
            Return ret
        End Get
    End Property

    Private Sub ParamList(ByRef Parameters As List(Of String), ByVal Statement As tSQLStatement)
        If Not IsNothing(SQL(Statement)) Then
            For Each SQLParameter As String In Regex.Matches(SQL(tSQLStatement.sqlList), "\%.*\%")
                If Not String.Compare(SQLParameter.ToLower, "%me%") = 0 Then
                    If Not Parameters.Contains(SQLParameter) Then
                        Parameters.Add(SQLParameter.Substring(1, SQLParameter.Length - 2))
                    End If
                End If
            Next
        End If
    End Sub

#End Region

#Region "Public methods"

    Public Sub hDependancyCheck()
        Dim myDepends As Boolean = True
        For Each Dependancy As String In Depends
            Dim ctrl As CtrlColumn = thisform.FindControl(Dependancy)
            If ctrl.value.length = 0 Then
                myDepends = False
                Exit For
            End If
        Next
        If myDepends Then
            Me._ColumnEnabled = True
        End If
    End Sub

#End Region

End Class
