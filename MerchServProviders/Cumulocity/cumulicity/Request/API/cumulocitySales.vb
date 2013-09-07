Public Class cumulocitySales
    Inherits cumulocityRequest

    Sub New(ByRef Tennant As cumulocityCredentials, ByVal Machine As String, ByVal dateFrom As Integer, Optional ByVal dateTo As Integer = -1, Optional ByVal Slot As String = Nothing)
        MyBase.New(Tennant, "machine/sales")
        Parameters.Add("machine", Machine)
        Parameters.Add("date", cumulocityDate.PriorityToUTC(dateFrom))
        If dateTo > -1 Then Parameters.Add("dateTo", cumulocityDate.PriorityToUTC(dateTo))
        If Not (IsNothing(Slot)) Then Parameters.Add("slot", Slot)

    End Sub

    Public Overrides Function Result(ByRef excep As Exception) As CumulocityResponse

        excep = Nothing

        ' *** For Testing
        Return New responseSales("{'responseCode': 0,'value':{'machine': 'VND1039230-10','date': '2012-09-05T00:00:00Z','slots': [{'slotId':'11','product':'Gummibaerchen','price':0.05,'itemsLeft':16,'salesSinceRefill':0,'capacity':16,'sales':0,'cash':0.0},{'slotId':'12','product':'Small PostIt','price':0.05,'itemsLeft':4,'salesSinceRefill':12,'capacity':16,'sales':12,'cash':0.6}]}")

        Dim ex As New Exception
        Dim rd As System.IO.StreamReader = Response(ex)
        If IsNothing(ex) Then
            Try
                Return New responseSales(rd.ReadToEnd)

            Catch parseException As Exception
                excep = parseException
                Return Nothing
            End Try
        Else
            excep = ex
            Return Nothing
        End If

    End Function

End Class
