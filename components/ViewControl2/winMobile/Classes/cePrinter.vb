Imports CPCL
Module cePrinter
    Private _prn As CPCL.LabelPrinter
    Public Property prn() As CPCL.LabelPrinter
        Get
            Return _prn
        End Get
        Set(ByVal value As CPCL.LabelPrinter)
            _prn = value
        End Set
    End Property
    Private _prnmac As String
    Public Property prnmac() As String
        Get
            Return _PRNMAC
        End Get
        Set(ByVal value As String)
            _prnmac = value
        End Set
    End Property
End Module
