Public Class responseSales
    Inherits CumulocityResponse

    Sub New(ByVal Response As String)
        MyBase.New(Response)
    End Sub

    Public ReadOnly Property Machine() As String
        Get
            Return Data("value")("machine")
        End Get
    End Property

    Public ReadOnly Property Slot() As List(Of slot)
        Get
            Dim ret As New List(Of Slot)
            For Each el As jsonDictionary In Data("value")("slots")
                ret.Add( _
                    New Slot( _
                        el("slotId"), _
                        el("product"), _
                        CDbl(el("price")), _
                        CInt(el("itemsLeft")), _
                        CInt(el("salesSinceRefill")), _
                        CInt(el("capacity")), _
                        CInt(el("sales")), _
                        CDbl(el("cash")) _
                    ) _
                )
            Next
            Return ret
        End Get
    End Property

End Class

Public Class Slot

#Region "Initialisation and Finalisation"

    Public Sub New()

    End Sub

    Public Sub New( _
        ByVal slotId As String, _
        ByVal product As String, _
        ByVal Price As Double, _
        ByVal ItemsLeft As Integer, _
        ByVal salesSinceRefill As Integer, _
        ByVal capacity As Integer, _
        ByVal sales As Integer, _
        ByVal cash As Double)

        _slotId = slotId
        _product = product
        _Price = Price
        _ItemsLeft = ItemsLeft
        _salesSinceRefill = salesSinceRefill
        _capacity = capacity
        _sales = sales
        _cash = cash

    End Sub

#End Region

#Region "Public Properties"

    Private _slotId As String
    Public Property slotId() As String
        Get
            Return _slotId
        End Get
        Set(ByVal value As String)
            _slotId = value
        End Set
    End Property

    Private _product As String
    Public Property product() As String
        Get
            Return _product
        End Get
        Set(ByVal value As String)
            _product = value
        End Set
    End Property

    Private _Price As Double
    Public Property Price() As Double
        Get
            Return _Price
        End Get
        Set(ByVal value As Double)
            _Price = value
        End Set
    End Property

    Private _ItemsLeft As Integer
    Public Property ItemsLeft() As Integer
        Get
            Return _ItemsLeft
        End Get
        Set(ByVal value As Integer)
            _ItemsLeft = value
        End Set
    End Property

    Private _salesSinceRefill As Integer
    Public Property salesSinceRefill() As Integer
        Get
            Return _salesSinceRefill
        End Get
        Set(ByVal value As Integer)
            _salesSinceRefill = value
        End Set
    End Property

    Private _capacity As Integer
    Public Property capacity() As Integer
        Get
            Return _capacity
        End Get
        Set(ByVal value As Integer)
            _capacity = value
        End Set
    End Property

    Private _sales As Integer
    Public Property sales() As Integer
        Get
            Return _sales
        End Get
        Set(ByVal value As Integer)
            _sales = value
        End Set
    End Property

    Private _cash As Double
    Public Property cash() As Double
        Get
            Return _cash
        End Get
        Set(ByVal value As Double)
            _cash = value
        End Set
    End Property

#End Region

    Public Overloads Function toString()
        Return String.Format("slotId={1}{0} product={2}{0} Price={3}{0} ItemsLeft={4}{0} salesSinceRefill={5}{0} capacity={6}{0} sales={7}{0} cash={8}{0}{0}", _
            vbCrLf, _
            slotId, _
            product, _
            Price, _
            ItemsLeft, _
            salesSinceRefill, _
            capacity, _
            sales, _
            cash _
        )
    End Function

End Class
