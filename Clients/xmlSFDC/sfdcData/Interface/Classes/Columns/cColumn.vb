Imports System.Xml
Imports System.Text.RegularExpressions

Public Class cColumn
    Inherits cNode

    Private Validating As Boolean = False    

    Public Event SetData()

    Friend _strDepends As New List(Of String)

    Public Overloads ReadOnly Property Parent() As cContainer
        Get
            Return _Parent
        End Get
    End Property

    Private _NoPostField As Boolean = False
    Public Property NoPostField() As Boolean
        Get
            Return _NoPostField
        End Get
        Set(ByVal value As Boolean)
            _NoPostField = value
        End Set
    End Property

    Public Overrides ReadOnly Property NodeType() As String
        Get
            Return "column"
        End Get
    End Property

    Public Sub New(ByRef Parent As cContainer, ByRef Node As XmlNode)
        Try
            LoadNode(Node)
            _Triggers = New cTriggers(Me, thisNode)
            _Parent = Parent

            If Not IsNothing(thisNode.Attributes("regex")) Then
                _thisRegex = New Regex(thisNode.Attributes("regex").Value)                
            End If
            _isReadOnly = Not IsNothing(thisNode.Attributes("readonly"))

            For Each dt As String In _Triggers.Keys
                For Each colRef As String In _Triggers(dt).ColumnRefs
                    If Not (String.Compare(colRef, String.Format(":$.{0}", Me.Name)) = 0) Then
                        If Not _strDepends.Contains(colRef) Then
                            _strDepends.Add(colRef)
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

    Public Sub SetControl(ByRef uiCol As uiColumn)
        _uiCol = uiCol
    End Sub

    Private _uiCol As uiColumn
    Public ReadOnly Property uiCol() As uiColumn
        Get
            Return _uiCol
        End Get
    End Property

    Private _Depends As New List(Of cColumn)
    Public Property Depends() As List(Of cColumn)
        Get
            Return _Depends
        End Get
        Set(ByVal value As List(Of cColumn))
            _Depends = value
        End Set
    End Property

    Friend _Triggers As cTriggers
    Public ReadOnly Property Triggers() As Dictionary(Of String, cTrigger)
        Get
            Return _Triggers
        End Get
    End Property

    Public Sub CheckDependancy()
        If Not IsNothing(uiCol) Then
            uiCol.CheckDependancy()
        End If
    End Sub

#Region "Column Value"

    Public Function Validate(ByRef Value As String, ByRef ex As Exception) As Boolean

        Dim ret As Boolean = False
        ex = Nothing

        If Not IsNothing(rxPattern) Then
            If Not rxIsPattern(rxPattern, Value) Then
                ex = New cfmtException("Invalid format: '{0}'.", Value)
            End If
        End If

        If IsNothing(ex) Then

            Validating = True
            _Value = Value
            Validating = False

            If Triggers.Keys.Contains("CHECK-FIELD") Then
                Dim data As Data.DataTable = Triggers("CHECK-FIELD").Execute()
                If Not IsNothing(data) Then
                    ex = New Exception(data.Rows(0).Item(0))
                Else
                    ret = True
                End If
            Else
                ret = True
            End If

        End If

        If ret Then uiCol.ParentForm.thisHandler.CheckField(ret, uiCol)
        If Not ret Then
            Me.Value = ""
        Else
            Me.Value = Value
        End If

        Return ret

    End Function

    Private _Value As String = ""
    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value.Trim
            If Not IsNothing(uiCol) And Not Validating Then
                uiCol.lbl_Value.Text = _Value
                RaiseEvent SetData()
                If _Value.Length > 0 And Not _NoPostField Then
                    If Triggers.Keys.Contains("POST-FIELD") Then
                        _NoPostField = True
                        Triggers("POST-FIELD").Execute()
                        _NoPostField = False
                    End If
                    uiCol.ParentForm.thisHandler.PostField(uiCol)
                End If
            End If
        End Set
    End Property

#End Region

#Region "Column Properties"

    Public ReadOnly Property Name() As String
        Get
            Return thisNode.Attributes("name").Value
        End Get
    End Property

    Public ReadOnly Property Title() As String
        Get
            Return thisNode.Attributes("title").Value
        End Get
    End Property

    Public ReadOnly Property ColumnType() As String
        Get
            Return thisNode.Attributes("type").Value
        End Get
    End Property

    Public ReadOnly Property Visible() As Boolean
        Get
            Return IsNothing(thisNode.Attributes("hidden"))
        End Get
    End Property

    Public ReadOnly Property Mandatory() As Boolean
        Get
            Return Not IsNothing(thisNode.Attributes("mandatory"))
        End Get
    End Property

    Private _isReadOnly As Boolean = False
    Public Property isReadOnly() As Boolean
        Get
            Return _isReadOnly
        End Get
        Set(ByVal value As Boolean)
            With Me.uiCol
                _isReadOnly = value
                Select Case value
                    Case True                        
                        If .Enabled And .Selected Then
                            .Selected = False
                            .Enabled = False
                            .ColStyle = eColStyle.colReadOnly

                            If Not IsNothing(.Parent.FocusedControl) Then
                                .Parent.FocusedControl.ColStyle = eColStyle.colSelected
                            Else
                                .Parent.FirstControl()
                            End If
                        End If

                    Case Else
                        Select Case uiCol.Selected
                            Case True
                                .ColStyle = eColStyle.colSelected
                            Case Else
                                .ColStyle = eColStyle.colDeselected
                        End Select
                End Select
            End With
        End Set
    End Property

    Public ReadOnly Property Help() As String
        Get
            If Not IsNothing(thisNode.Attributes("help")) Then
                Return thisNode.Attributes("help").Value
            Else
                Return "There is no help for this field."
            End If
        End Get
    End Property

    Public ReadOnly Property Width() As Integer
        Get
            If Not IsNothing(thisNode.Attributes("width")) Then
                Return CInt(thisNode.Attributes("width").Value)
            Else
                Return 0
            End If
        End Get
    End Property

    Private _thisRegex As Regex = Nothing
    Public ReadOnly Property rxPattern() As Regex
        Get
            If Not IsNothing(_thisRegex) Then
                Return _thisRegex
            Else
                Select Case Me.ColumnType.ToLower
                    Case "int", "unsigned"
                        Return rxINT
                    Case "real"
                        Return rxREAL
                    Case Else
                        Return Nothing
                End Select
            End If
        End Get
    End Property

    Public ReadOnly Property BarcodeField() As String
        Get
            If Not IsNothing(thisNode.Attributes("barcode2d")) Then
                Return CInt(thisNode.Attributes("barcode2d").Value)
            Else
                Return ""
            End If
        End Get
    End Property

#End Region

#Region "Calculated Column Properties"

    Public ReadOnly Property SQLValue() As String
        Get
            Select Case ColumnType.ToUpper
                Case "INT", "REAL", "DATE", "TIME", "UNSIGNED"
                    Return String.Format("{0}", Value)
                Case Else
                    Return String.Format("'{0}'", Value)
            End Select
        End Get
    End Property

#End Region

End Class
