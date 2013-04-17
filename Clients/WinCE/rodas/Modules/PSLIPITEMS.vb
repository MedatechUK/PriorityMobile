Public Class PSLIPITEMS
    Private mORDI As Integer
    Private mROUTE As String
    Private mPSlipNo As String
    Private mPart As String
    Private mQuant As Integer
    Private mDesc As String
    Private mLot As String
    Private mWARHS As String
    Private mBin As String
    Private mType As String
    Public Property ORDI() As Integer
        Get
            Return mORDI
        End Get
        Set(ByVal value As Integer)
            mORDI = value
        End Set
    End Property
    Public Property ROUTE() As String
        Get
            Return mROUTE
        End Get
        Set(ByVal value As String)
            mROUTE = value
        End Set
    End Property
    Public Property PSlipNo() As String
        Get
            Return mPSlipNo
        End Get
        Set(ByVal value As String)
            mPSlipNo = value
        End Set
    End Property
    Public Property PART() As String
        Get
            Return mPart
        End Get
        Set(ByVal value As String)
            mPart = value
        End Set
    End Property
    Public Property Quant() As Integer
        Get
            Return mQuant
        End Get
        Set(ByVal value As Integer)
            mQuant = value
        End Set
    End Property
    Public Property Desc() As String
        Get
            Return mDesc
        End Get
        Set(ByVal value As String)
            mDesc = value
        End Set
    End Property
    Public Property Lot() As String
        Get
            Return mLot
        End Get
        Set(ByVal value As String)
            mLot = value
        End Set
    End Property
    Public Property WARHS() As String
        Get
            Return mWARHS
        End Get
        Set(ByVal value As String)
            mWARHS = value
        End Set
    End Property
    Public Property Bin() As String
        Get
            Return mBin
        End Get
        Set(ByVal value As String)
            mBin = value
        End Set
    End Property
    Public Property Type() As String
        Get
            Return mType
        End Get
        Set(ByVal value As String)
            mType = value
        End Set
    End Property
    Public Sub New(ByVal ord As Integer, ByVal ro As String, ByVal pslip As String, ByVal par As String, ByVal qua As Integer, ByVal des As String, ByVal lt As String, ByVal wrh As String, ByVal bi As String, ByVal ty As String)
        ORDI = ord
        ROUTE = ro
        PSlipNo = pslip
        PART = par
        Quant = qua
        Desc = des
        Lot = lt
        WARHS = wrh
        Bin = bi
        Type = ty
    End Sub
End Class
