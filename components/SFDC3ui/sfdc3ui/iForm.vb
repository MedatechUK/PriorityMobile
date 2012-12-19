Imports System.Xml
Imports System.IO

Public Class iForm

#Region "Handler Events"

    Public Event test()

#End Region

#Region "Local Variables"

    Private myForm As ctrlForm
    Private myTable As ctrlTable

#End Region

#Region "Load Module XML"

    Private _ModuleName As String
    Public ReadOnly Property ModuleName() As String
        Get
            Return _ModuleName
        End Get
    End Property

    Private ReadOnly Property ModuleXMLFile() As String
        Get
            If File.Exists(String.Format("{0}.xml", ModuleName)) Then
                Return String.Format("{0}.xml", ModuleName)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ModuleXML As XmlDocument = Nothing
    Private ReadOnly Property ModuleXML() As XmlDocument
        Get
            Try
                If IsNothing(_ModuleXML) Then
                    _ModuleXML = New XmlDocument()
                    _ModuleXML.Load(ModuleXMLFile)
                End If
                Return _ModuleXML
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Private ReadOnly Property StartNode() As XmlNode
        Get
            Static sNode As XmlNode
            If IsNothing(sNode) Then
                If Arguments.Keys.Contains("startArg") Then
                    sNode = ModuleXML.SelectSingleNode( _
                            String.Format( _
                                "module/start[@argument='{0}']", _
                                Arguments("startArg" _
                            ) _
                        ) _
                    )
                End If
                If IsNothing(sNode) Then
                    StartNode = ModuleXML.SelectSingleNode( _
                        String.Format( _
                            "module/start[@argument='*']", _
                            Arguments("startArg" _
                        ) _
                    ) _
                )
                End If
            End If
            Return sNode
        End Get
    End Property

#End Region

#Region "initialisation and finalisation"

    Public Sub New(ByVal ModuleName As String, ByRef mHandler As ModuleHandler, Optional ByVal StartArguments As Dictionary(Of String, String) = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler Me.test, AddressOf mHandler.test

        _ModuleName = ModuleName
        _Arguments = StartArguments

        If IsNothing(ModuleXMLFile) Then
            Throw New Exception(String.Format("Missing XML file [{0}.xml].", ModuleName))
        End If

        If IsNothing(StartNode) Then
            Throw New Exception(String.Format("Invalid XML file [{0}.xml]. Missing Start node.", ModuleName))
        End If

        Dim FormNode As XmlNode = StartNode.SelectSingleNode("form")
        Dim TableNode As XmlNode = StartNode.SelectSingleNode("table")
        If Not IsNothing(FormNode) And Not IsNothing(TableNode) Then
            myForm = New ctrlForm(Me, FormNode)
            myTable = New ctrlTable(Me, TableNode)
        Else
            Throw New Exception(String.Format("Invalid XML file [{0}.xml]. Start node must contain form and table elements.", ModuleName))
        End If

        For Each col As CtrlColumn In myForm.Columns
            For Each dependacy As String In col.Depends
                Dim ctrl As CtrlColumn = FindControl(dependacy)
                If Not IsNothing(ctrl) Then
                    AddHandler ctrl.DependancyCheck, AddressOf col.hDependancyCheck
                Else
                    Throw New Exception(String.Format("Column [{0}] depends on missing control [{1}].", col.ColumnName, dependacy))
                End If
            Next
        Next

    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property FindControl(ByVal ControlName As String) As CtrlColumn
        Get
            For Each col As CtrlColumn In myForm.Columns
                If String.Compare(col.ColumnName, ControlName, True) = 0 Then
                    Return col
                End If
            Next
            For Each col As CtrlColumn In myTable.Columns
                If String.Compare(col.ColumnName, ControlName, True) = 0 Then
                    Return col
                End If
            Next
            Return Nothing
        End Get
    End Property

    Private _Arguments As Dictionary(Of String, String)
    Public Property Arguments(ByVal ArgName As String) As String
        Get
            If _Arguments.Keys.Contains("argname") Then
                Return _Arguments(ArgName)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If _Arguments.Keys.Contains("argname") Then
                _Arguments(ArgName) = value
            Else
                _Arguments.Add(ArgName, value)
            End If
        End Set
    End Property

#End Region

End Class