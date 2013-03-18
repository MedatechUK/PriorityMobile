Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class Session

#Region "Initialisation"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal CurrentContext As HttpContext)
        With Me
            .mSessionID = CurrentContext.Session.SessionID
            .mInfo = New SessionInfo(CurrentContext)
        End With
    End Sub

#End Region

#Region "Public Subs"

    Public Sub Update(ByVal CurrentContext As HttpContext)
        With Me
            .mInfo.ViewingPage = CurrentContext.Request.RawUrl
        End With
    End Sub

#End Region

#Region "public properties"

    Private mSessionID As String = ""
    Public Property SessionID() As String
        Get
            Return mSessionID
        End Get
        Set(ByVal value As String)
            mSessionID = value
        End Set
    End Property

    Private _debug As Boolean = False
    Public Property Debug() As Boolean
        Get
            Return _debug
        End Get
        Set(ByVal value As Boolean)
            _debug = value
        End Set
    End Property

    Private mCart As New Cart()
    Public Property cart() As Cart
        Get
            Return mCart
        End Get
        Set(ByVal value As Cart)
            mCart = value
        End Set
    End Property

    Private mBasket As New BasketCls
    Public Property Basket() As BasketCls
        Get
            Return mBasket
        End Get
        Set(ByVal value As BasketCls)
            mBasket = value
        End Set
    End Property

    Private mInfo As New SessionInfo
    Public Property Info() As SessionInfo
        Get
            Return mInfo
        End Get
        Set(ByVal value As SessionInfo)
            mInfo = value
        End Set
    End Property

    Private fd As Dictionary(Of String, String)

#Region "Form Dictioanry"

    Public Function cfd(ByVal Key As String, Optional ByVal strDefault As String = "") As String
        For Each K As String In HttpContext.Current.Request.Params.Keys
            Try
                If String.Compare(Right(K, Len(Key)), Key, True) = 0 Then
                    Return HttpContext.Current.Request.Params(K)
                End If
            Catch
            End Try
        Next
        Return strDefault
    End Function

#End Region

#End Region

End Class