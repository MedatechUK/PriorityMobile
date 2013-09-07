Public Class calcSetting

    Public Sub New()
        _permitDouble = False
        _permitNeg = False
        _DNUM = 0.0
    End Sub

    Public Sub New(ByVal Value As Double, Optional ByVal Min As Double = 0.0, Optional ByVal Max As Double = 999.99, Optional ByVal FormTitle As String = Nothing)
        _permitDouble = True
        _permitNeg = CBool(Min < 0)

        _DNUM = Value
        _Min = Min
        _Max = Max

        If Not IsNothing(FormTitle) Then
            _FormTitle = FormTitle
        Else
            _FormTitle = String.Empty
        End If

    End Sub

    Public Sub New(ByVal Value As Integer, Optional ByVal Min As Double = 0, Optional ByVal Max As Double = 999, Optional ByVal FormTitle As String = Nothing)
        _permitDouble = False
        _permitNeg = CBool(Min < 0)

        _DNUM = CDbl(Value)
        _Min = CDbl(CInt(Min))
        _Max = CDbl(CInt(Max))

        If Not IsNothing(FormTitle) Then
            _FormTitle = FormTitle
        Else
            _FormTitle = String.Empty
        End If

    End Sub

    Private _permitDouble As Boolean
    Public ReadOnly Property permitDouble() As Boolean
        Get
            Return _permitDouble
        End Get
    End Property

    Private _permitNeg As Boolean
    Public ReadOnly Property permitNeg() As Boolean
        Get
            Return _permitNeg
        End Get
    End Property

    Private _DNUM As Double = 0.0
    Public Property DNUM() As Double
        Get
            Return _DNUM
        End Get
        Set(ByVal value As Double)
            _DNUM = value
        End Set
    End Property

    Private _FormTitle As String = String.Empty
    Public ReadOnly Property FormTitle() As String
        Get
            Return _FormTitle
        End Get        
    End Property

    Private _Max As Double = 999
    Public Property Max() As Double
        Get
            Return _Max
        End Get
        Set(ByVal value As Double)
            _Max = value
        End Set
    End Property

    Private _Min As Double = 0
    Public Property Min() As Double
        Get
            Return _Min
        End Get
        Set(ByVal value As Double)
            _Min = value
        End Set
    End Property

    Private _Result As DialogResult = DialogResult.OK
    Public Property Result() As DialogResult
        Get
            Return _Result
        End Get
        Set(ByVal value As DialogResult)
            _Result = value
        End Set
    End Property

End Class
