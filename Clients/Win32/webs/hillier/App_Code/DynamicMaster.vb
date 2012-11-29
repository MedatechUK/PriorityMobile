Imports Microsoft.VisualBasic

Public MustInherit Class DynamicMaster
    Inherits System.Web.UI.Page

    Public Event GetMPType()
    Public MustOverride Sub hGetMPType()
    Public MustOverride Function GetPH(Optional ByVal ph_Name As String = Nothing) As Object

    Public olu As New OnlineUsers

#Region "Public Properties"

    Private _thisSession As Session = Nothing
    Private _MP As String = ""
    Private _Title As String = ""

    Public ReadOnly Property thisSession() As Session
        Get
            If IsNothing(_thisSession) Then
                _thisSession = olu.CurrentSession(HttpContext.Current)
            End If
            Return _thisSession
        End Get
    End Property

    Public Property PageTitle() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property

    Public Property MasterPage() As String
        Get
            Return _MP
        End Get
        Set(ByVal value As String)
            _MP = value
        End Set
    End Property

#End Region

    Public Sub DisplayError(ByVal Message As String)
        ' Display error message
        Dim ph_Debug As PlaceHolder = Me.Master.FindControl("ph_Debug")
        If Not IsNothing(ph_Debug) Then
            Dim db As New Debug(Me)
            With db
                .DisplayError(Message, Page, ph_Debug)
            End With
        End If
    End Sub

    Public Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Dim start As String = thisSession.Info.SessionStart
        RaiseEvent GetMPType()
        With Page
            .MasterPageFile = "~/Masterpages/" & MasterPage & ".master"
            If PageTitle.Length > 0 Then
                .Title = PageTitle
            End If
        End With

    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Display any debug info
        Dim ph_Debug As PlaceHolder = Me.Master.FindControl("ph_Debug")
        If Not IsNothing(ph_Debug) Then
            _thisSession.NamePairs.Info(ph_Debug)
        End If

    End Sub

End Class
