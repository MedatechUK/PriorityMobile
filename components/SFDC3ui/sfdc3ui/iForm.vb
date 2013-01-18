Imports System.Xml
Imports System.IO

Public Class iForm

#Region "Handler Events"

    Private WaitStart As Timer
    Public Event test()

#End Region

#Region "Load Module XML"

    Private _ServerURL As String
    Public ReadOnly Property ServerURL() As String
        Get
            Return _ServerURL
        End Get
    End Property

    Private _ModuleName As String
    Public ReadOnly Property ModuleName() As String
        Get
            Return _ModuleName
        End Get
    End Property

    Private ReadOnly Property ModuleURL() As String
        Get
            Return String.Format("{0}/sfdc/{1}.xml", ServerURL, ModuleName)
        End Get
    End Property

    Private _ModuleXML As New XmlDocument
    Private ReadOnly Property ModuleXML() As XmlDocument
        Get
            Return _ModuleXML
        End Get
    End Property

#End Region

#Region "initialisation and finalisation"

    Public Sub New(ByVal ServerURL As String, ByVal ModuleName As String, ByRef mHandler As ModuleHandler, ByVal StartArgs As Dictionary(Of String, String))

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        With Me

            .Text = ModuleName
            .Width = Screen.PrimaryScreen.WorkingArea.Width
            .Height = Screen.PrimaryScreen.WorkingArea.Height

            _ModuleName = ModuleName
            _ServerURL = ServerURL
            _Arguments = StartArgs

            AddHandler .test, AddressOf mHandler.test

            .WaitStart = New Timer
            With .WaitStart
                .Interval = 1
                AddHandler WaitStart.Tick, AddressOf hInitialise
                .Enabled = True
            End With

        End With
    End Sub

    Private Sub hInitialise(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Me.WaitStart.Dispose()

            Try
                _ModuleXML.Load(ModuleURL)
            Catch ex As Exception
                Throw New Exception(String.Format("Could not load module information from [{0}]. {1}", ModuleURL, ex.Message))
            End Try

            Dim StartNode As XmlNode
            If _Arguments.Keys.Contains("startarg") Then
                ' Select the specified start Node
                StartNode = ModuleXML.SelectSingleNode( _
                    String.Format( _
                        "module/start[@argument='{0}']", _
                        _Arguments("startarg") _
                    ) _
                )
                If IsNothing(StartNode) Then
                    Throw New Exception( _
                        String.Format( _
                            "Module [{0}] does not contain a start tag for the argument '{1}'.", _
                            ModuleURL, _
                            _Arguments("startarg") _
                        ) _
                    )
                End If
            Else
                ' Select the default StartNode
                StartNode = ModuleXML.SelectSingleNode( _
                                "module/start[@argument='*']" _
                            )
                If IsNothing(StartNode) Then
                    Throw New Exception( _
                        String.Format( _
                            "Module [{0}] does not contain a start tag for the default argument '*'.", _
                            ModuleURL _
                        ) _
                    )
                End If
            End If

            Dim FormNode As XmlNode = StartNode.SelectSingleNode("form")
            Dim TableNode As XmlNode = StartNode.SelectSingleNode("table")
            If Not IsNothing(FormNode) And Not IsNothing(TableNode) Then
                SetFontSize()
                With thisForm
                    .Load(Me, FormNode)
                    .Draw()
                End With
                With thisTable
                    .Load(Me, TableNode)
                    .Draw()
                End With
            Else
                Throw New Exception(String.Format("Invalid Module XML file [{0}]. Start node does not contain form and table elements.", ModuleURL))
            End If

            For Each col As CtrlColumn In thisForm.Columns
                For Each dependacy As String In col.Depends
                    Dim ctrl As CtrlColumn = FindControl(dependacy)
                    If Not IsNothing(ctrl) Then
                        AddHandler ctrl.AcceptData, AddressOf col.hDependancyCheck
                    Else
                        Throw New Exception(String.Format("Column [{0}] depends on missing control [{1}].", col.ColumnName, dependacy))
                    End If
                Next
                col.hDependancyCheck()
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
            Close()
        End Try

    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property FindControl(ByVal ControlName As String) As CtrlColumn
        Get
            For Each col As CtrlColumn In thisForm.Columns
                If String.Compare(col.ColumnName, ControlName, True) = 0 Then
                    Return col
                End If
            Next
            For Each col As CtrlColumn In thisTable.Columns
                If String.Compare(col.ColumnName, ControlName, True) = 0 Then
                    Return col
                End If
            Next
            Return Nothing
        End Get
    End Property

    Private _Arguments As New Dictionary(Of String, String)
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

    Private _ControlHeight As Integer = 24
    Public Property ControlHeight() As Integer
        Get
            Return _ControlHeight
        End Get
        Set(ByVal value As Integer)
            _ControlHeight = value
        End Set
    End Property

    Private _ControlFont As Font = New Font("Arial", 13, FontStyle.Regular)
    Public Property ControlFont() As Font
        Get
            Return _ControlFont
        End Get
        Set(ByVal value As Font)
            _ControlFont = value
        End Set
    End Property

    Private Sub SetFontSize()

        Dim FontSize = 1
        Dim fs As Font
        Dim stringSize As New SizeF
        Using g As Graphics = CreateGraphics()
            With _ControlFont
                fs = New Font(.Name, FontSize, .Style)
                stringSize = g.MeasureString("A", fs)
                Do Until stringSize.Height > Me.ControlHeight - 2
                    FontSize += 1
                    fs = New Font(.Name, FontSize, .Style)
                    stringSize = g.MeasureString("A", fs)
                Loop
                If FontSize > 0 Then FontSize = FontSize - 1
                _ControlFont = New Font(.Name, FontSize, .Style)
                _CharWidth = g.MeasureString("A", _ControlFont).Width
            End With
        End Using

    End Sub

    Private _CharWidth As Integer
    Public ReadOnly Property CharWidth() As Integer
        Get
            Return _CharWidth
        End Get
    End Property

#End Region

#Region "Form Resize Handlers"

    Private Sub iForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        With Me
            .Top = 0
            .Left = 0
        End With
    End Sub

    Private Sub iForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me
            With .thisForm
                .Top = 0
                .Width = Me.Width
                .Height = Me.Height / 2
            End With
            With .thisTable
                .Top = thisForm.Height
                .Width = Me.Width
                .Height = Me.Height / 2
            End With
        End With
    End Sub

#End Region

    Public Sub BeginColumnAccept(ByRef sender As CtrlColumn)
        Try
            With sender
                If Not IsNothing(.ProposedValue) Then
                    .Value = .ProposedValue
                Else
                    .DeactivateControl()
                End If
            End With
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub

    Public Sub BeginColumnAccept(ByVal Columns As Dictionary(Of String, String))

    End Sub

End Class