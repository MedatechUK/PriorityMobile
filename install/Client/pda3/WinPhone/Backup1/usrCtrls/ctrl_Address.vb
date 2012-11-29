Imports PriorityMobile

Public Class ctrl_Address
    Inherits iView

    Public Overrides Sub Bind()

    End Sub

    Public Overrides Sub CurrentChanged()
        Dim build As String = ""
        With thisForm.Parent
            Gtext.AddNamePair("Customer", .CurrentRow("custname"))
            Gtext.AddNamePair("Phone Number", .CurrentRow("phone"))
        End With
        With thisForm
            Gtext.AddNamePair("Contact Name", .CurrentRow("contact"))
            Gtext.AddNamePair("Address", _
                CRLFifData(.CurrentRow("address1")) & _
                CRLFifData(.CurrentRow("address2")) & _
                CRLFifData(.CurrentRow("address3")) & _
                CRLFifData(.CurrentRow("address4")) & _
                CRLFifData(.CurrentRow("postcode")) _
            )
        End With
        Gtext.PARSE()
    End Sub

    Public Function CRLFifData(ByVal str As String) As String
        If Len(Trim(str)) > 0 Then
            Return str & vbCrLf
        Else
            Return ""
        End If
    End Function

End Class
