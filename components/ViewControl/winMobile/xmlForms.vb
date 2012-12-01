Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.IO

Public Enum eStatusRule
    prereport = 1
    beginreport = 2
    post = 3
    active = 4
    beginwork = 5
End Enum

Public Class xmlForms

#Region "Public Events"

    Public Event AddUserControl(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm)
    Public Event DrawForm()
    Public Event DrawSubMenu()
    Public Event DrawDirectActivations()
    Public Event StartCalc(ByVal Max As Integer)
    Public Event StartDialog(ByVal frmDialog As PriorityMobile.UserDialog)

#End Region

#Region "Public Shared Members"

    Public Shared UserEnv As UserEnv    
    Public Shared FormDesign As OfflineXML
    Public Shared FormData As OfflineXML
    Public Shared DataLookUp As lookupData
    Private Shared StatusRules As OfflineXML
    Public Shared TopForm As Dictionary(Of String, TopLevelForm)
    Public Shared changedAttribute As XmlAttribute
    Public Shared postAttribute As XmlAttribute
    Public Shared changedRegex As Regex
    
#End Region

#Region "initialisation and Finalisation"

    Public Sub New(ByRef UE As UserEnv, _
                   ByRef _FormDesign As OfflineXML, _
                   ByRef _FormData As OfflineXML, _
                   ByRef _lookup As OfflineXML, _
                   ByRef _StatusRules As OfflineXML _
                   )

        UserEnv = UE
        FormDesign = _FormDesign
        FormData = _FormData
        If Not IsNothing(_lookup) Then DataLookUp = New lookupData(_lookup)
        If Not IsNothing(_StatusRules) Then StatusRules = _StatusRules

        TopForm = New Dictionary(Of String, TopLevelForm)

        For Each f As XmlNode In FormDesign.Document.SelectNodes("forms/form")
            TopForm.Add(f.Attributes("name").Value, New TopLevelForm(New xForm(f, f.Attributes("xpath").Value, Nothing)))
        Next

        changedAttribute = FormData.Document.CreateAttribute("changed")
        changedAttribute.Value = "1"

        postAttribute = FormData.Document.CreateAttribute("post")
        postAttribute.Value = "1"

        changedRegex = New Regex(String.Format(" changed={0}.{0}| post={0}.{0}", Chr(34)))


    End Sub

    Private Sub hAddUserControl(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm)
        RaiseEvent AddUserControl(ControlName, view, thisForm)
    End Sub

#Region "Interface Event Handlers"

    Private Sub hDrawForm()
        RaiseEvent DrawForm()
    End Sub

    Private Sub hDrawSubMenu()
        RaiseEvent DrawSubMenu()
    End Sub

    Private Sub hDrawDirectActivations()
        RaiseEvent DrawDirectActivations()
    End Sub

    Private Sub hOpenCalc(ByVal Max As Integer)
        RaiseEvent StartCalc(Max)
    End Sub

    Private Sub hDialog(ByVal frmDialog As PriorityMobile.UserDialog)
        RaiseEvent StartDialog(frmDialog)
    End Sub

#End Region

#Region "Load view controls"

    Public Sub LoadViewControls()
        For Each f As TopLevelForm In TopForm.Values
            SetEventHandlers(f.TopForm)
            f.TopForm.LoadViewControls()
            For Each x As xForm In f.TopForm.SubForms.Values
                SetEventHandlers(x)
                x.LoadViewControls()
                RecurseView(x)
            Next
            f.TopForm.Bind()
        Next
    End Sub

    Private Sub RecurseView(ByVal x As xForm)
        For Each rx As xForm In x.SubForms.Values
            SetEventHandlers(rx)
            rx.LoadViewControls()
            RecurseView(rx)
        Next
    End Sub

    Private Sub SetEventHandlers(ByVal this As xForm)
        With this
            RemoveHandler .AddUserControl, AddressOf hAddUserControl
            RemoveHandler .DrawSubMenu, AddressOf hDrawSubMenu
            RemoveHandler .DrawDirectActivations, AddressOf hDrawDirectActivations
            RemoveHandler .DrawForm, AddressOf hDrawForm
            RemoveHandler .StartCalc, AddressOf hOpenCalc
            RemoveHandler .StartDialog, AddressOf hDialog

            AddHandler .AddUserControl, AddressOf hAddUserControl
            AddHandler .DrawSubMenu, AddressOf hDrawSubMenu
            AddHandler .DrawDirectActivations, AddressOf hDrawDirectActivations
            AddHandler .DrawForm, AddressOf hDrawForm
            AddHandler .StartCalc, AddressOf hOpenCalc
            AddHandler .StartDialog, AddressOf hDialog
        End With
    End Sub

#End Region

#End Region

#Region "Public Properties"

    Private _Activeform As Integer = 0
    Public Property ActiveForm() As Integer
        Get
            Return _Activeform
        End Get
        Set(ByVal value As Integer)
            If value > TopForm.Count - 1 Then
                _Activeform = 0
            Else
                _Activeform = value
            End If
        End Set
    End Property

    Public Shared ReadOnly Property StatusRule(ByVal Status As String, ByVal Rule As eStatusRule) As Boolean
        Get
            Dim st As XmlNode = StatusRules.Document.SelectSingleNode(String.Format("rules/status[@name={0}{1}{0}]", Chr(34), Status))
            If IsNothing(st) Then Return False
            Dim at As Xml.XmlAttribute = Nothing
            Select Case Rule
                Case eStatusRule.beginreport
                    At = st.Attributes("beginreport")
                Case eStatusRule.prereport
                    at = st.Attributes("prereport")
                Case eStatusRule.post
                    at = st.Attributes("post")
                Case eStatusRule.active
                    at = st.Attributes("active")
                Case eStatusRule.beginwork
                    at = st.Attributes("beginwork")
            End Select
            If IsNothing(at) Then Return False
            Return CBool(at.Value)
        End Get
    End Property

    Public Shared ReadOnly Property StatusList(rule As eStatusRule) As List(Of String)
        Get
            Dim ret As New List(Of String)
            Dim act As Xml.XmlNodeList = StatusRules.Document.SelectNodes("//status")
            For Each n As XmlNode In act
                If StatusRule(n.Attributes("name").Value, rule) Then
                    ret.Add(n.Attributes("name").Value)
                End If
            Next
            Return ret
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub Sync()
        FormData.Sync()
    End Sub

    Public Sub PostNode(ByVal Node As XmlNode, ByVal PostException As Exception)
        FormData.PostData(Node, PostException)
    End Sub

    Public Shared Sub Log(ByVal FormatString As String, ByVal ParamArray Values() As String)
        Using sw As New StreamWriter(UserEnv.LocalFolder & "\log.txt", True)
            sw.WriteLine(FormatString, Values)
        End Using
    End Sub

    Public Shared Sub Log(ByVal FormatString As String)
        Using sw As New StreamWriter(UserEnv.LocalFolder & "\log.txt", True)
            sw.WriteLine(FormatString)
        End Using
    End Sub

#End Region

End Class