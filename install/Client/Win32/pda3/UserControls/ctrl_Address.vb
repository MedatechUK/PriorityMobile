Imports ViewControl

Public Class ctrl_Address
    Inherits ViewControl.iView

    Public Overrides Sub Bind()
        SetText()
    End Sub

    Public Overrides Sub CurrentChanged()
        SetText()
    End Sub

    Private Sub SetText()
        Dim build As String = ""
        With thisForm.Parent.TableData
            Gtext.AddNamePair("Customer", .Current("name"))
            Gtext.AddNamePair("Phone Number", .Current("phone"))
        End With
        With thisForm.TableData
            Gtext.AddNamePair("Contact Name", .Current("contact"))
            Gtext.AddNamePair("Address", _
                CRLFifData(.Current("address1")) & _
                CRLFifData(.Current("address2")) & _
                CRLFifData(.Current("address3")) & _
                CRLFifData(.Current("address4")) & _
                CRLFifData(.Current("postcode")) _
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
