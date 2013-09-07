Public Class LoadConst

    Private _Consts As Dictionary(Of String, String)

    Public Sub New()
        _Consts = New Dictionary(Of String, String)
        Constants("%DATE%") = DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString
    End Sub

    Public Property Constants(ByVal Parameter As String) As String
        Get
            If _Consts.ContainsKey(Parameter) Then
                Return _Consts(Parameter)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If _Consts.ContainsKey(Parameter) Then
                _Consts(Parameter) = value
            Else
                _Consts.Add(Parameter, value)
            End If
        End Set
    End Property

    Public Function Parse(ByVal StrVal As String)
        Dim ret As String = StrVal
        If IsNothing(StrVal) Then Return ""
        With Me._Consts
            If .ContainsKey(StrVal) Then
                ret = .Item(StrVal)
            End If
        End With
        Return ret
    End Function

End Class
