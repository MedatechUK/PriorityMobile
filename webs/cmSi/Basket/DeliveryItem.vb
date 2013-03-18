Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class DeliveryItem

#Region "Initialisations"

    Public Sub New()
        MyBase.new()
        With Me
            .DELIVERYPART = ""
            .PARTDES = "No selected delivery Item"
            .PARTPRICE = "0.00"
            .SALESTAX = "0.00"
            .REFERER = ""
        End With
    End Sub

    Public Sub New(ByVal PARTNAME As String, ByVal PARTDES As String, ByVal PARTPRICE As String, ByVal SALESTAX As String)
        MyBase.new()
        With Me
            .DELIVERYPART = PARTNAME
            .PARTDES = PARTDES
            .PARTPRICE = PARTPRICE
            .SALESTAX = SALESTAX
            .REFERER = ""
        End With
    End Sub

#End Region

#Region "Public Properties"

    Private _PARTNAME As String = ""
    Public ReadOnly Property PARTNAME() As String
        Get
            Return "DELIVERY"
        End Get
    End Property

    Private _DELPART As String = ""
    Public Property DELIVERYPART() As String
        Get
            Return _DELPART
        End Get
        Set(ByVal value As String)
            _DELPART = value
        End Set
    End Property

    Private _PARTDES As String = ""
    Public Property PARTDES() As String
        Get
            Return _PARTDES
        End Get
        Set(ByVal value As String)
            _PARTDES = value
        End Set
    End Property

    Private _PARTPRICE As String = ""
    Public Property PARTPRICE() As String
        Get
            Return FormatDouble(_PARTPRICE)
        End Get
        Set(ByVal value As String)
            _PARTPRICE = value
        End Set
    End Property

    Private _QTY As String = ""
    Public ReadOnly Property QTY() As String
        Get
            Return "1"
        End Get
    End Property

    Private _SALESTAX As String = ""
    Public Property SALESTAX() As String
        Get
            Return FormatDouble(_SALESTAX)
        End Get
        Set(ByVal value As String)
            _SALESTAX = value
        End Set
    End Property

    Private _LINETOTAL As String = ""
    Public ReadOnly Property LINETOTAL() As String
        Get
            Return FormatDouble(CStr(CInt(_QTY) * (CDbl(_PARTPRICE) * (1 + CDbl(_SALESTAX) / 100))))
        End Get
    End Property

    Private _REFERER As String = ""
    Public Property REFERER() As String
        Get
            Return _REFERER
        End Get
        Set(ByVal value As String)
            _REFERER = value
        End Set
    End Property

#End Region

#Region "Private Functions"

    Private Function FormatDouble(ByVal str As String) As String
        If InStr(str, ".") Then
            Return Split(str, ".")(0) & "." & Left(Split(str, ".")(1) & "00", 2)
        Else
            Return str & ".00"
        End If
    End Function

#End Region

End Class