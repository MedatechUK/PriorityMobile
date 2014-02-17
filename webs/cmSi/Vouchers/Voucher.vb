Imports System.Xml
Imports System.Web
Imports System.Globalization

Public MustInherit Class Voucher
    ''' <summary>
    ''' Voucher base class
    ''' </summary>
    ''' <remarks>inherit voucher implementations from here. Apply must be overidden </remarks>
#Region "Public Properties"
    Private _des As String
    Public Property Des() As String
        Get
            Return _des
        End Get
        Set(ByVal value As String)
            _des = value
        End Set
    End Property

    Private _code As String
    Public Property Code() As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            _code = value
        End Set
    End Property

    Private _expiry As DateTime
    Public Property Expiry() As DateTime
        Get
            Return _expiry
        End Get
        Set(ByVal value As DateTime)
            _expiry = value
        End Set
    End Property

    Private _voucherData As XmlNode
    Public Property VoucherData() As XmlNode
        Get
            Return _voucherData
        End Get
        Set(ByVal value As XmlNode)
            _voucherData = value
        End Set
    End Property

    Private _discount As Double
    Public Overridable Property Discount() As Double
        Get
            Return _discount
        End Get
        Set(ByVal value As Double)
            _discount = value
        End Set
    End Property

    Public ReadOnly Property ts() As Session
        Get
            Return cmsSessions.CurrentSession(HttpContext.Current)
        End Get
    End Property
#End Region

#Region "Method Stubs"
    Protected MustOverride Sub Enact()
#End Region

#Region "Public Methods"

    Public Sub New(ByVal data As XmlNode)
        VoucherData = data
        Expiry = DateTime.ParseExact(VoucherData.Attributes("expiry").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        Code = VoucherData.Attributes("code").Value
        Des = VoucherData.Attributes("des").Value
    End Sub

    Public Function Apply() As String

        Try
            If Expiry < DateTime.Now And Not IsNothing(Expiry) Then
                Return "Voucher expired!"
            End If
            Enact()
            Return "Discount added."
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

#End Region

End Class

