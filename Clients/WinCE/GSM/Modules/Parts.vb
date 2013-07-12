Public Class Parts
    Private PartID As Integer
    Private PartName As String
    Private PartDes As String
    Private Quant As Integer
    Private LOT As String
    Private Operation As String
    Private Checked As String
    Private Barcode As String
    Public Property pID() As Integer
        Get
            Return PartID
        End Get
        Set(ByVal value As Integer)
            PartID = value
        End Set
    End Property
    Public Property pName() As String
        Get
            Return PartName
        End Get
        Set(ByVal value As String)
            PartName = value
        End Set
    End Property
    Public Property Desc() As String
        Get
            Return PartDes
        End Get
        Set(ByVal value As String)
            PartDes = value
        End Set
    End Property
    Public Property qua() As Integer
        Get
            Return Quant
        End Get
        Set(ByVal value As Integer)
            Quant = value
        End Set
    End Property
    Public Property lt() As String
        Get
            Return lot
        End Get
        Set(ByVal value As String)
            LOT = value
        End Set
    End Property
    Public Property op() As String
        Get
            Return Operation
        End Get
        Set(ByVal value As String)
            Operation = value
        End Set
    End Property
    Public Property pChecked() As String
        Get
            Return Checked
        End Get
        Set(ByVal value As String)
            Checked = value
        End Set
    End Property
    Public Property pBarcode() As String
        Get
            Return Barcode
        End Get
        Set(ByVal value As String)
            Barcode = value
        End Set
    End Property
    Public Sub New(ByVal i As Integer, ByVal pn As String, ByVal d As String, ByVal q As Integer, ByVal l As String, ByVal o As String, ByVal c As String, ByVal b As String)
        pID = i
        pName = pn
        Desc = d
        qua = q
        lt = l
        op = o
        pChecked = c
        pBarcode = b
    End Sub
End Class
