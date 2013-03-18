Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class BasketItem

#Region "initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal PARTNAME As String, ByVal QTY As String, ByVal Referer As String, Optional ByVal BoxQuantity As String = "1")
        MyBase.New()
        _PARTNAME = PARTNAME
        _QTY = QTY
        _REFERER = Referer 'System.Web.HttpContext.Current.Request.Url.AbsolutePath
        _BoxQTY = BoxQuantity
    End Sub

#End Region

#Region "Public Properties"

    Private _PARTNAME As String = String.Empty
    Public Property PARTNAME() As String
        Get
            Return _PARTNAME
        End Get
        Set(ByVal value As String)
            _PARTNAME = value
        End Set
    End Property

    Private _QTY As String = String.Empty
    Public Property QTY() As String
        Get
            Return _QTY
        End Get
        Set(ByVal value As String)
            _QTY = value
        End Set
    End Property

    Private _BoxQTY As String = String.Empty
    Public Property BoxQTY() As String
        Get
            Return _BoxQTY
        End Get
        Set(ByVal value As String)
            _BoxQTY = value
        End Set
    End Property

    Private _REFERER As String = String.Empty
    Public Property REFERER() As String
        Get
            Return _REFERER
        End Get
        Set(ByVal value As String)
            _REFERER = value
        End Set
    End Property

#End Region

End Class
