
Friend Class cp_Country
    Inherits CPCommand
    Private _country As eCountry = eCountry.UK


    Public Sub New(Optional ByVal country As eCountry = eCountry.UK)
        _country = country
    End Sub

    Public Overrides ReadOnly Property tostring() As String
        Get
            Dim cmd As String = ""
            Select Case _country
                Case eCountry.USA
                    cmd = "USA"
                Case eCountry.Sweden
                    cmd = "SWEDEN"
                Case eCountry.Italy
                    cmd = "ITALY"
                Case eCountry.Korea
                    cmd = "KOREA"
                Case eCountry.Germany
                    cmd = "GERMANY"
                Case eCountry.Spain
                    cmd = "SPAIN"
                Case eCountry.CP850
                    cmd = "CP850"
                Case eCountry.Thai
                    cmd = "CP874"
                Case eCountry.TraditionalChinese
                    cmd = "BIG5"
                Case eCountry.France
                    cmd = "FRANCE"
                Case eCountry.Norway
                    cmd = "NORWAY"
                Case eCountry.UK
                    cmd = "UK"
                Case eCountry.China
                    cmd = "CHINA"
                Case eCountry.Japan
                    cmd = "JAPAN-S"
            End Select

            Return String.Format("COUNTRY {0}" & vbCrLf, cmd)
        End Get
    End Property
End Class
