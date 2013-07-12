Public Class WHSTRAN
    Private PART As String
    Private Amount As Integer
    Private Order As String
    Private Li As String
    Private ORDI As Integer
    Private CONV As Decimal
    Private SERIAL As String
    Public Property trPart() As String
        Get
            Return PART
        End Get
        Set(ByVal value As String)
            PART = value
        End Set

    End Property
    Public Property trAmount() As Integer
        Get
            Return Amount
        End Get
        Set(ByVal value As Integer)
            Amount = value
        End Set
    End Property
    Public Property trORD() As String
        Get
            Return Order
        End Get
        Set(ByVal value As String)
            Order = value
        End Set
    End Property
    Public Property trLine()
        Get
            Return Li
        End Get
        Set(ByVal value)
            Li = value
        End Set
    End Property
    Public Property trOrdi() As Integer
        Get
            Return ORDI
        End Get
        Set(ByVal value As Integer)
            ORDI = value
        End Set
    End Property
    Public Property trConv() As Decimal
        Get
            Return CONV
        End Get
        Set(ByVal value As Decimal)
            CONV = value
        End Set
    End Property
    Public Property trSerial() As String
        Get
            Return SERIAL
        End Get
        Set(ByVal value As String)
            SERIAL = value
        End Set
    End Property
    Public Sub New(ByVal p As String, ByVal a As Integer, ByVal l As String, ByVal li As String, ByVal oi As Integer, ByVal c As Decimal, ByVal s As String)
        trPart = p
        trAmount = a
        trORD = l
        trLine = li
        trOrdi = oi
        trConv = c
        trSerial = s
    End Sub
End Class
