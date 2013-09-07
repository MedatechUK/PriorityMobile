Module example

    Private Sub TestLoading()

        ' ------ Test Loading ------
        Using xl As New Loading
            With xl
                Try
                    .Table = "ZSFDC_TABLE"
                    .Procedure = "ZSFDC_TEST"
                    .Environment = "wl"

                    .AddColumn(1) = New LoadColumn("USERNAME", tColumnType.typeCHAR)
                    .AddColumn(1) = New LoadColumn("WARHS", tColumnType.typeCHAR)
                    .AddColumn(1) = New LoadColumn("BIN", tColumnType.typeCHAR)
                    .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE)
                    .AddColumn(2) = New LoadColumn("PART", tColumnType.typeCHAR)
                    .AddColumn(2) = New LoadColumn("STATUS", tColumnType.typeCHAR)
                    .AddColumn(2) = New LoadColumn("CQUANT", tColumnType.typeINT)

                    .AddRecordType(1) = New LoadRow("user", "Main", "0", Now.ToString)
                    .AddRecordType(2) = New LoadRow("PART123", "Goods", "1")
                    .AddRecordType(2) = New LoadRow("PART321", "Goods", "1")

                    Dim exp As New Exception
                    If Not .Post("http://redknave:8080/loadHandler.ashx", exp) Then Throw exp
                    'If Not .Post("http://redknave:8080/loadHandler.ashx", exp) Then Throw exp
                    'If Not .Post("http://redknave:8080/loadHandler.ashx", exp) Then Throw exp

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End With
        End Using
    End Sub

End Module
