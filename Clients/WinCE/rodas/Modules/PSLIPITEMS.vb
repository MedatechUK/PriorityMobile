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
    Private mOName As String
    Private mOLine As String
    Private mAmount As Integer
    Private mConv As Decimal
    Private pCust As String
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
    Public Property oname() As String
        Get
            Return moName
        End Get
        Set(ByVal value As String)
            moName = value
        End Set
    End Property
    Public Property oline() As String
        Get
            Return moline
        End Get
        Set(ByVal value As String)
            moline = value
        End Set
    End Property
    Public Property Amount() As Integer
        Get
            Return mAmount
        End Get
        Set(ByVal value As Integer)
            mAmount = value
        End Set
    End Property
    Public Property Con() As Decimal
        Get
            Return mConv
        End Get
        Set(ByVal value As Decimal)
            mConv = value
        End Set
    End Property
    Public Property Cust() As String
        Get
            Return pCust
        End Get
        Set(ByVal value As String)
            pCust = value
        End Set
    End Property
    Public Sub New(ByVal ord As Integer, ByVal ro As String, ByVal pslip As String, ByVal par As String, ByVal qua As Integer, ByVal des As String, ByVal lt As String, ByVal wrh As String, ByVal bi As String, ByVal ty As String, ByVal ona As String, ByVal ol As String, ByVal am As Integer, ByVal cnv As Decimal, ByVal cst As String)
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
        oname = ona
        oline = ol
        Amount = am
        Con = cnv
        Cust = cst
    End Sub
End Class
