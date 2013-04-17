Public Class LotS
    Private part As String
    Private lot As String
    Private balance As Integer
    Private warhs As String
    Private bin As String
    Private typ As String
    Private des As String
    Public Property lpart() As String
        Get
            Return part
        End Get
        Set(ByVal value As String)
            part = value
        End Set
    End Property
    Public Property LotRef() As String
        Get
            Return lot
        End Get
        Set(ByVal value As String)
            lot = value
        End Set
    End Property
    Public Property bal() As Integer
        Get
            Return balance
        End Get
        Set(ByVal value As Integer)
            balance = value
        End Set
    End Property
    Public Property whs() As String
        Get
            Return warhs
        End Get
        Set(ByVal value As String)
            warhs = value
        End Set
    End Property
    Public Property bi() As String
        Get
            Return bin
        End Get
        Set(ByVal value As String)
            bin = value
        End Set
    End Property
    Public Property type() As String
        Get
            Return typ
        End Get
        Set(ByVal value As String)
            typ = value
        End Set
    End Property
    Public Property desc() As String
        Get
            Return des
        End Get
        Set(ByVal value As String)
            des = value
        End Set
    End Property
    Public Sub New(ByVal p As String, ByVal l As String, ByVal b As Integer, ByVal w As String, ByVal bn As String, ByVal t As String, ByVal des As String)
        lpart = p
        LotRef = l
        bal = b
        whs = w
        bi = bn
        type = t
        desc = des
    End Sub
End Class
