Public NotInheritable Class AboutBox1

    Private LoadMessage As System.Text.StringBuilder
    Private KeyTimer As System.Timers.Timer

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '' Set the title of the form.
        'Dim ApplicationTitle As String
        'If My.Application.Info.Title <> "" Then
        '    ApplicationTitle = My.Application.Info.Title
        'Else
        '    ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        'End If
        'Me.Text = String.Format("About {0}", ApplicationTitle)
        '' Initialize all of the text displayed on the About Box.
        '' TODO: Customize the application's assembly information in the "Application" pane of the project 
        ''    properties dialog (under the "Project" menu).
        'Me.LabelProductName.Text = My.Application.Info.ProductName
        'Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        'Me.LabelCopyright.Text = My.Application.Info.Copyright
        'Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        'Me.TextBoxDescription.Text = My.Application.Info.Description
        With My.Application.Info
            LoadMessage = New System.Text.StringBuilder
            LoadMessage.AppendFormat("{0}", .ProductName).AppendLine()
            LoadMessage.AppendFormat("Version {0}", My.Application.Info.Version.ToString).AppendLine()
            LoadMessage.AppendFormat("By {0}", "Simon Barnett.").AppendLine()
            LoadMessage.AppendFormat("{0}", My.Application.Info.Copyright).AppendLine()
            LoadMessage.AppendFormat("{0}", My.Application.Info.Trademark).AppendLine()            
            LoadMessage.AppendFormat("{0}", My.Application.Info.Description).AppendLine()
        End With

        KeyTimer = New System.Timers.Timer
        With KeyTimer
            .Interval = 50
            AddHandler .Elapsed, AddressOf hKeyTimer
            .Enabled = True
        End With

    End Sub

    Private Sub hKeyTimer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        If LoadMessage.Length > 0 Then
            If LoadMessage.Chars(0) = Chr(13) Then
                Threading.Thread.Sleep(850)
            End If
            Try
                Invoke(New Action(Of Char)(AddressOf threadSafeRTFAppend), LoadMessage.Chars(0))
                LoadMessage.Remove(0, 1)
            Catch
                With KeyTimer
                    .Enabled = False
                    .Dispose()
                End With
            End Try
        Else
            With KeyTimer
                .Enabled = False
                .Dispose()
            End With
        End If
    End Sub

    Private Sub threadSafeRTFAppend(ByVal Letter As Char)
        With rtfConsole
            .AppendText(Letter)
            .Select(.TextLength, 0)
        End With
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Protected Overrides Sub Finalize()
        If Not IsNothing(KeyTimer) Then
            With KeyTimer
                .Enabled = False
                .Dispose()
            End With
        End If
        MyBase.Finalize()
    End Sub

End Class
