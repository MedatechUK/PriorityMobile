Public Class Address



    Private _First As String
    Public Property First() As String
        Get
            Return _First
        End Get
        Set(ByVal value As String)
            _First = value
        End Set
    End Property

    Private _Last As String
    Public Property Last() As String
        Get
            Return _Last
        End Get
        Set(ByVal value As String)
            _Last = value
        End Set
    End Property

    Private _Address1 As String
    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Private _Address2 As String
    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property

    Private _Address3 As String
    Public Property Address3() As String
        Get
            Return _Address3
        End Get
        Set(ByVal value As String)
            _Address3 = value
        End Set
    End Property

    Private _Address4 As String
    Public Property Address4() As String
        Get
            Return _Address4
        End Get
        Set(ByVal value As String)
            _Address4 = value
        End Set
    End Property

    Private _Address5 As String
    Public Property Address5() As String
        Get
            Return _Address5
        End Get
        Set(ByVal value As String)
            _Address5 = value
        End Set
    End Property

    Private _Address_Postcode As String
    Public Property Address_Postcode() As String
        Get
            Return _Address_Postcode
        End Get
        Set(ByVal value As String)
            _Address_Postcode = value
        End Set
    End Property

    Private _email As String
    Public Property eMail() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Private _phone As String
    Public Property Phone() As String
        Get
            Return _phone
        End Get
        Set(ByVal value As String)
            _phone = value
        End Set
    End Property

End Class
