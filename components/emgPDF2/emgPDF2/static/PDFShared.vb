Imports HiQPdf

Public Class PDFShared

    ''' <summary>
    ''' Returns serial key for HiQPDF
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Shared ReadOnly Property Serial() As String
        Get
            Return "cjobIyIW-FD4bEAAT-AAtEXEJS-Q1JDUktG-QVJBQ1xD-QFxLS0tL"
        End Get
    End Property

    Private Shared _pdfparams As PdfParameters
    ''' <summary>
    ''' Static member to hold dictionary of parameters for PDF creation.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property PDFParams() As PdfParameters
        Get
            Return _pdfparams
        End Get
        Set(ByVal value As PdfParameters)
            _pdfparams = value
        End Set
    End Property

    ''' <summary>
    ''' This class is for static members only!
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        Return 'singleton
    End Sub
End Class



