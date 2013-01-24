Public Class FormattedColumn

    Private _ColumnWidth As Integer
    Public Property ColumnWidth() As Integer
        Get
            Return _ColumnWidth
        End Get
        Set(ByVal value As Integer)
            _ColumnWidth = value
        End Set
    End Property

    Private _StartChar As Integer
    Public Property StartChar() As Integer
        Get
            Return _StartChar
        End Get
        Set(ByVal value As Integer)
            _StartChar = value
        End Set
    End Property

    Private _align As eAlignment
    Public Property Alignment() As eAlignment
        Get
            Return _align
        End Get
        Set(ByVal value As eAlignment)
            _align = value
        End Set
    End Property

    Public Sub New(ByVal ColumnWidth As Integer, ByVal StartChar As Integer, Optional ByVal Alignment As eAlignment = eAlignment.Left)
        _StartChar = StartChar
        _ColumnWidth = ColumnWidth
        _align = Alignment
    End Sub

End Class

Public Class ReceiptFormatter

    Private myColumns As New List(Of FormattedColumn)
    Private myRows As New List(Of String())
    Private _CharWidth As Integer

    Public Sub New(ByVal CharWidth As Integer, ByVal ParamArray args() As FormattedColumn)
        _CharWidth = CharWidth
        For Each c As FormattedColumn In args
            If myColumns.Count > 0 Then
                If c.StartChar < myColumns(myColumns.Count - 1).StartChar + myColumns(myColumns.Count - 1).ColumnWidth Then
                    Throw New Exception(String.Format("Column {0} overlaps column {1}.", myColumns.Count, myColumns.Count - 1))
                End If
                If c.StartChar + c.ColumnWidth > CharWidth Then
                    Throw New Exception(String.Format("Column {0} is outside the defined Charwidth.", myColumns.Count))
                End If
            End If
            myColumns.Add(c)
        Next
    End Sub

    Public Sub AddRow(ByVal ParamArray args() As String)
        If Not UBound(args) = myColumns.Count - 1 Then
            Throw New Exception(String.Format("Column was defined with {0} columns, but this column has {1}.", myColumns.Count - 1, UBound(args)))
        End If
        Dim CroppedText(UBound(args)) As String
        For i As Integer = 0 To myColumns.Count - 1
            CroppedText(i) = Left(args(i), myColumns(i).ColumnWidth)
        Next
        myRows.Add(CroppedText)
    End Sub

    Public Function FormattedText() As List(Of String)
        Dim ln As New String(" ", _CharWidth)
        Dim ret As New List(Of String)
        For Each row As String() In myRows
            Dim col(UBound(row)) As String
            For i As Integer = 0 To myColumns.Count - 1
                Select Case myColumns(i).Alignment
                    Case eAlignment.Left
                        col(i) = Left(row(i) & New String(" ", myColumns(i).ColumnWidth), myColumns(i).ColumnWidth)
                    Case eAlignment.Right
                        col(i) = Right(New String(" ", myColumns(i).ColumnWidth) & row(i), myColumns(i).ColumnWidth)
                    Case eAlignment.Center
                        col(i) = Left(New String(" ", CInt((myColumns(i).ColumnWidth - row(i).ToString.Length) / 2)) & row(i) & New String(" ", myColumns(i).ColumnWidth), myColumns(i).ColumnWidth)
                End Select
            Next
            For i As Integer = 0 To myColumns.Count - 1
                addcol(ln, col(i), myColumns(i).StartChar)
            Next
            ret.Add(ln)
        Next
        Return ret
    End Function

    Private Sub addcol(ByRef Line As String, ByVal StrVal As String, ByVal Start As Integer)
        Line = Line.Substring(0, Start) & StrVal & Line.Substring(Start + StrVal.Length)
    End Sub

End Class
