Imports priority

'The Following example generates the XML shown below and posts it to the service.

'<?xml version="1.0" encoding="UTF-16"?>
'<PriorityLoading PROCEDURE="ZSFDC_TEST" TABLE="ZSFDC_TABLE" ENVIRONMENT="company"> 
'    <ROW CQUANT="0" STATUS="''" PART="''" CURDATE="13060097" BIN="'0'" WARHS="'Main'" USERNAME="'user'" RECORDTYPE="1"/> 
'    <ROW CQUANT="1000" STATUS="'Goods'" PART="'PART123'" CURDATE="0" BIN="''" WARHS="''" USERNAME="''" RECORDTYPE="2"/> 
'    <ROW CQUANT="1000" STATUS="'Goods'" PART="'PART321'" CURDATE="0" BIN="''" WARHS="''" USERNAME="''" RECORDTYPE="2"/> 
'</PriorityLoading>

Module example

    Sub main()

        ' The Url of the service
        Dim ServiceURL As String = "http://localhost:8080"
        ' The Priority Environment to use
        Dim myEnvironment As String = "myCompany"

        ' Get data from the service
        Using getdata As New priority.GetData(ServiceURL)
            With getdata
                Try
                    ' Return a datatable object containing the results of a query
                    Dim reader As System.Data.DataTable = .ExecuteReader("Select * From PART", myEnvironment)
                    ' Return a scalar value
                    Dim result As Integer = .ExecuteScalar("Select 1+1", myEnvironment)
                    ' Execute non-query code
                    .ExecuteNonQuery("delete from GENERALLOAD where LINE > 0", myEnvironment)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End With
        End Using

        ' Load data to the service
        Using xl As New Loading
            With xl
                Try
                    .Table = "ZSFDC_TABLE"
                    .Procedure = "ZSFDC_TEST"
                    .Environment = myEnvironment

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

                    ' Send to Server
                    If Not .Post( _
                        ServiceURL, _
                        exp _
                    ) Then Throw exp

                    ' Send to file
                    'If Not .ToFile( _
                    '    String.Format( _
                    '        "c:\{0}.xml", _
                    '        System.Guid.NewGuid.ToString _
                    '    ), _
                    '    exp _
                    ') Then Throw exp

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End With
        End Using

    End Sub

End Module
