Imports PriorityMobile

Public Class ctrl_Home
    Inherits iView

    Private ReadOnly Property VersionString()
        Get
            With System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
                Return String.Format("{0}.{1}.{2}.{3}", .Major, .Minor, .Build, .Revision)
            End With
        End Get
    End Property

    Public Overrides Sub Bind()
        With Me
            Try
                .curdate.DataBindings.Add("Text", thisForm.TableData, "curdate")
                .routenumber.DataBindings.Add("Text", thisForm.TableData, "routenumber")
                .vehiclereg.DataBindings.Add("Text", thisForm.TableData, "vehiclereg")
                .User.Text = thisForm.thisUserEnv.User
                .Version.Text = VersionString
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End With
    End Sub

    Public Overrides Sub CurrentChanged()

    End Sub

    Public Function CRLFifData(ByVal str As String) As String
        If Len(Trim(str)) > 0 Then
            Return str & vbCrLf
        Else
            Return ""
        End If
    End Function

#Region "Direct Activations"

    'Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
    '    ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
    'End Sub

    'Private Sub hPlaceCall()

    '    Dim ph As New Microsoft.WindowsMobile.Telephony.Phone
    '    Try
    '        ph.Talk(thisForm.CurrentRow("phone"))
    '    Catch ex As Exception
    '        MsgBox(String.Format("Call failed to: {0}.", thisForm.CurrentRow("phone")))
    '    End Try

    'End Sub

#End Region

End Class
