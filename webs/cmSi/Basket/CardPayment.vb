Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class CardPayment

#Region "Initialisation"
    Public Sub New(ByVal Trans As String, ByVal AuthCode As String, ByVal Amount As Double)
        _trans = Trans
        _authcode = AuthCode
        _amount = Amount
    End Sub
#End Region

#Region "Public Properties"
    Private _trans As String
    Public Property trans() As String
        Get
            Return _trans
        End Get
        Set(ByVal value As String)
            _trans = value
        End Set
    End Property
    Private _authcode As String
    Public Property authcode() As String
        Get
            Return _authcode
        End Get
        Set(ByVal value As String)
            _authcode = value
        End Set
    End Property
    Private _amount As Double
    Public Property amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property
#End Region

End Class