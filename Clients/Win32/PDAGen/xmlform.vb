Imports System
Imports System.xml
Imports System.io

#Region "Enumerative classes"

Public Enum xfView
    xfForm
    xfTable
    xfHtml
    xfSignature
End Enum

Public Enum xfFieldStyle
    xfText
    xfCombo
    xfDate
    xfBool
End Enum

Public Enum xmlColumnType
    tText
    tInt
    tReal
    tDate
End Enum

#End Region

Public Class xmlField

    Private _Name As String = ""
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _FieldStyle As xfFieldStyle = xfFieldStyle.xfText
    Public Property FieldStyle() As xfFieldStyle
        Get
            Return _FieldStyle
        End Get
        Set(ByVal value As xfFieldStyle)
            _FieldStyle = value
        End Set
    End Property

    Private _Mandatory As Boolean = False
    Public Property Mandatory() As Boolean
        Get
            Return _Mandatory
        End Get
        Set(ByVal value As Boolean)
            _Mandatory = value
        End Set
    End Property

    Private _ReadOnly As Boolean = False
    Public Property IsReadOnly() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
        End Set
    End Property

    Private _Hidden As Boolean = False
    Public Property Hidden() As Boolean
        Get
            Return _Hidden
        End Get
        Set(ByVal value As Boolean)
            _Hidden = value
        End Set
    End Property

    Private _ListSource As String = ""
    Public Property ListSource() As String
        Get
            Return _ListSource
        End Get
        Set(ByVal value As String)
            _ListSource = value
        End Set
    End Property

    Private _ListValueCol As String = ""
    Public Property ListValueCol() As String
        Get
            Return _ListValueCol
        End Get
        Set(ByVal value As String)
            _ListValueCol = value
        End Set
    End Property

    Private _ListTextCol As String
    Public Property ListTextCol() As String
        Get
            Return _ListTextCol
        End Get
        Set(ByVal value As String)
            _ListTextCol = value
        End Set
    End Property

    Private _ListFilter As String
    Public Property ListFilter() As String
        Get
            Return _ListFilter
        End Get
        Set(ByVal value As String)
            _ListFilter = value
        End Set
    End Property

    Private _Column As String = ""
    Public Property Column() As String
        Get
            Return _Column
        End Get
        Set(ByVal value As String)
            _Column = value
        End Set
    End Property

    'Private _label As Label
    'Public Sub SetReferenceLbl(ByRef ThisControl As Label)
    '    _label = ThisControl
    'End Sub
    'Public ReadOnly Property label() As Label
    '    Get
    '        Return _label
    '    End Get
    'End Property

    'Private _Control As Control
    'Public Sub SetReferenceCtl(ByRef ThisControl As Control)
    '    _Control = ThisControl
    'End Sub
    'Public ReadOnly Property Control() As Control
    '    Get
    '        Return _Control
    '    End Get
    'End Property

End Class

Public Class xmlTab

    Private _Name As String = ""
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _fields As New Dictionary(Of Integer, xmlField)
    Public Property fields() As Dictionary(Of Integer, xmlField)
        Get
            Return _fields
        End Get
        Set(ByVal value As Dictionary(Of Integer, xmlField))
            _fields = value
        End Set
    End Property

    Private _Panel As Windows.Forms.Panel
    Public Sub SetReference(ByRef ThisPanel As Windows.Forms.Panel)
        _Panel = ThisPanel
    End Sub
    Public ReadOnly Property Panel() As Windows.Forms.Panel
        Get
            Return _Panel
        End Get
    End Property

End Class

Public Class xmlForm

    Private _FormSQL As String = ""
    Public Property FormSQL() As String
        Get
            Return _FormSQL
        End Get
        Set(ByVal value As String)
            _FormSQL = value
        End Set
    End Property

    Private _xmlConfig As xmlConfiguration
    Private Property xmlConfig() As xmlConfiguration
        Get
            Return _xmlConfig
        End Get
        Set(ByVal value As xmlConfiguration)
            _xmlConfig = value
        End Set
    End Property

#Region "initialisation and finalisation"

    Public Sub New()

    End Sub

#End Region

#Region "Private subs"

    Public Sub NewForm(ByRef Form As xmlForm, ByVal node As XmlNode)

        With Form
            For Each at As XmlAttribute In node.Attributes
                Select Case LCase(at.Name)
                    Case "name"
                        .Name = at.Value
                    Case "defaultview"
                        Select Case LCase(at.Value)
                            Case "form"
                                .DefaultView = xfView.xfForm
                            Case "table"
                                .DefaultView = xfView.xfTable
                            Case "html"
                                .DefaultView = xfView.xfHtml
                            Case "signature"
                                .DefaultView = xfView.xfSignature
                        End Select
                        .CurrentView = .DefaultView
                    Case "readonly"
                        .IsReadOnly = CBool(at.Value)
                    Case "from"
                        .SQLFrom = at.Value
                    Case "where"
                        .SQLWhere = at.Value
                End Select
            Next
            For Each n As XmlNode In node.ChildNodes
                Select Case LCase(n.Name)
                    Case "form"
                        With .SubForm
                            .Add(.Count + 1, New xmlForm())
                            NewForm(.Item(.Count), n)
                        End With
                    Case "tab"
                        With .Tabs
                            .Add(.Count + 1, New xmlTab)
                            With .Item(.Count)
                                .Name = n.Attributes("name").Value
                                For Each f As XmlNode In n.ChildNodes
                                    With .fields
                                        .Add(.Count + 1, New xmlField)
                                        With .Item(.Count)
                                            .Column = f.InnerXml
                                            For Each at As XmlAttribute In f.Attributes
                                                Select Case LCase(at.Name)
                                                    Case "name"
                                                        .Name = at.Value
                                                    Case "fieldstyle"
                                                        Select Case LCase(at.Value)
                                                            Case "list", "combo"
                                                                .FieldStyle = xfFieldStyle.xfCombo
                                                            Case "date", "time", "datetime"
                                                                .FieldStyle = xfFieldStyle.xfDate
                                                            Case Else
                                                                .FieldStyle = xfFieldStyle.xfText
                                                        End Select
                                                    Case "listsource"
                                                        .ListSource = at.Value
                                                    Case "listvaluecol"
                                                        .ListValueCol = at.Value
                                                    Case "listtextcol"
                                                        .ListTextCol = at.Value
                                                    Case "listfilter"
                                                        .ListFilter = at.Value
                                                    Case "mandatory"
                                                        .Mandatory = CBool(at.Value)
                                                    Case "hidden"
                                                        .Hidden = CBool(at.Value)
                                                    Case "readonly"
                                                        .IsReadOnly = CBool(at.Value)
                                                End Select
                                            Next
                                        End With
                                    End With
                                Next
                            End With
                        End With
                End Select
            Next
        End With

    End Sub

#End Region

#Region "Public Properties"

    Private _Name As String = ""
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _DefaultView As xfView = xfView.xfForm
    Public Property DefaultView() As xfView
        Get
            Return _DefaultView
        End Get
        Set(ByVal value As xfView)
            _DefaultView = value
        End Set
    End Property

    Private _CurrentView As xfView
    Public Property CurrentView() As xfView
        Get
            Return _CurrentView
        End Get
        Set(ByVal value As xfView)
            _CurrentView = value
        End Set
    End Property

    Private _IsReadOnly As Boolean
    Public Property IsReadOnly() As Boolean
        Get
            Return _IsReadOnly
        End Get
        Set(ByVal value As Boolean)
            _IsReadOnly = value
        End Set
    End Property

    Private _SQLFrom As String = ""
    Public Property SQLFrom() As String
        Get
            Return _SQLFrom
        End Get
        Set(ByVal value As String)
            _SQLFrom = value
        End Set
    End Property

    Private _SQLWhere As String = ""
    Public Property SQLWhere() As String
        Get
            If _SQLWhere.Length = 0 Then
                Return "0=0"
            Else
                Return _SQLWhere
            End If
        End Get
        Set(ByVal value As String)
            _SQLWhere = value
        End Set
    End Property

    Private _Filter As String
    Public Property Filter() As String
        Get
            Return _Filter
        End Get
        Set(ByVal value As String)
            _Filter = value
        End Set
    End Property

    Private _Tabs As New Dictionary(Of Integer, xmlTab)
    Public Property Tabs() As Dictionary(Of Integer, xmlTab)
        Get
            Return _Tabs
        End Get
        Set(ByVal value As Dictionary(Of Integer, xmlTab))
            _Tabs = value
        End Set
    End Property

    Private _SubForm As New Dictionary(Of Integer, xmlForm)
    Public Property SubForm() As Dictionary(Of Integer, xmlForm)
        Get
            Return _SubForm
        End Get
        Set(ByVal value As Dictionary(Of Integer, xmlForm))
            _SubForm = value
        End Set
    End Property

#End Region

End Class

Public Class xmlConfiguration

    Public Sub New(ByVal filename As String)

        Dim xd As New XmlDocument

        Using sr As New StreamReader(filename)
            xd.LoadXml(sr.ReadToEnd)
            sr.Close()
        End Using

        For Each n As XmlNode In xd.ChildNodes
            Select Case LCase(n.Name)
                Case "configuration"
                    For Each no As XmlNode In n.ChildNodes
                        Select Case LCase(no.Name)
                            Case "form"
                                For Each at As XmlAttribute In no.Attributes
                                    Select Case LCase(at.Name)
                                        Case "name"
                                            Dim f As New xmlForm()
                                            f.NewForm(f, no)
                                            Forms.Add(at.Value, f)
                                            Exit For
                                    End Select
                                Next
                        End Select
                    Next
            End Select
        Next
    End Sub

    Private _forms As New Dictionary(Of String, xmlForm)
    Public Property Forms() As Dictionary(Of String, xmlForm)
        Get
            Return _forms
        End Get
        Set(ByVal value As Dictionary(Of String, xmlForm))
            _forms = value
        End Set
    End Property

    Public Function FormPath(ByVal Path As String) As xmlForm
        Dim str() As String = Split(Path, ".")
        Dim xf As xmlForm = Forms.Item(str(0))
        For i As Integer = 1 To UBound(str)
            For Each sf As xmlForm In xf.SubForm.Values
                If Strings.StrComp(sf.Name, str(i), CompareMethod.Text) = 0 Then
                    xf = sf
                End If
            Next
        Next
        Return xf
    End Function

End Class
