Imports system.IO

Public Class Form1

    Dim sd As New Priority.SerialData
    Dim pcode As New pcode._default
    Dim data(,) As String
    Dim OUTPUTFILE As String = System.AppDomain.CurrentDomain.BaseDirectory() & "\OUTPUT.TXT"

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        Outcode.Text = UCase(Outcode.Text)
        Incode.Text = UCase(Incode.Text)
        Dim ok As Boolean = True
        Dim ret As String = ""
        Do
            ok = True
            Try
                ret = pcode.Postcode(Outcode.Text, Incode.Text)
            Catch ex As Exception
                ok = False
            End Try
            Application.DoEvents()
        Loop Until ok

        data = sd.DeSerialiseData(ret)
        With cbo_Address
            .Items.Clear()
            If Not IsNothing(data) Then
                For i As Integer = 0 To UBound(data, 2)
                    Dim str As String = ""
                    For x As Integer = 0 To UBound(data, 1) - 2
                        str = str & data(x, i)
                        If Len(data(x, i)) > 0 And x < UBound(data, 1) - 2 Then
                            str = str & ", "
                        End If
                    Next
                    .Items.Add(str)
                Next
                .SelectedIndex = 0
            Else
                MsgBox("Postcode not found.")
            End If
        End With


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim y As Integer = cbo_Address.SelectedIndex

        Dim str As String = data(0, y)
        If Len(data(0, y)) > 0 Then str = str & ", "
        str = str & data(1, y)
        If Len(data(1, y)) > 0 Then str = str & ", "
        str = str & data(2, y)
        If Len(data(2, y)) > 0 Then str = str & ", "

        Dim str1 As String = data(3, y)
        If Len(data(3, y)) > 0 Then str1 = str1 & ", "
        str1 = str1 & data(4, y)
        If Len(data(4, y)) > 0 Then str1 = str1 & " "
        str1 = str1 & data(5, y)

        If str <> "" Then
            STREET.Text = str
            ADDRESS1.Text = str1
            ADDRESS2.Text = data(6, y)
            CITY.Text = data(7, y)
            COUNTY.Text = data(8, y)
        Else
            STREET.Text = str1
            ADDRESS1.Text = data(6, y)
            CITY.Text = data(7, y)
            COUNTY.Text = data(8, y)
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Load the picture into a Bitmap.
        Dim bm As New Bitmap(System.AppDomain.CurrentDomain.BaseDirectory() & "\logo.BMP")

        ' Set the splash screen
        With SplashLogo
            .Image = bm
            .Width = bm.Width
            .Height = bm.Height
        End With

        Dim ret As String = pcode.Postcode("PR25", "3UN")


    End Sub

    Sub ftpFile()

        Dim currentfile As String = ""        
        Dim i As Integer = 1
        Do Until Not File.Exists(System.AppDomain.CurrentDomain.BaseDirectory() & dtstr() & "_" & CStr(i) & ".txt")
            i = i + 1
        Loop

        currentfile = dtstr() & "_" & My.Settings.pc & "_" & CStr(i) & ".txt"

        If File.Exists(OUTPUTFILE) Then
            File.Move(OUTPUTFILE, currentfile)

            Try
                Dim ftp = New EnterpriseDT.Net.Ftp.FTPConnection()
                With ftp
                    .ServerAddress = My.Settings.ftpServer
                    .UserName = My.Settings.ftpUser
                    .Password = My.Settings.ftpPass
                    .Connect()
                    .uploadfile(System.AppDomain.CurrentDomain.BaseDirectory() & currentfile, currentfile)
                    .Close()
                    MsgBox("File uploaded as " & currentfile & ".", MsgBoxStyle.Information)
                End With
            Catch er As Exception
                MsgBox(er.Message & vbCrLf & currentfile & " was not uploaded!", MsgBoxStyle.Critical)
            End Try

        Else

            MsgBox("No details to upload.", MsgBoxStyle.Information)

        End If

    End Sub

    Private Function dtstr() As String
        Return DateTime.Now.ToString("dd") & "-" & DateTime.Now.ToString("mm") & "-" & DateTime.Now.ToString("yy")
    End Function

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Dim en As Boolean = Not (CheckBox1.Checked)

        With STREET
            If en Then
                .BackColor = Color.White
            Else
                .BackColor = Color.LightGray
            End If
            .Enabled = en
        End With

        With ADDRESS1
            If en Then
                .BackColor = Color.White
            Else
                .BackColor = Color.LightGray
            End If
            .Enabled = en
        End With

        With ADDRESS2
            If en Then
                .BackColor = Color.White
            Else
                .BackColor = Color.LightGray
            End If
            .Enabled = en
        End With

        With CITY
            If en Then
                .BackColor = Color.White
            Else
                .BackColor = Color.LightGray
            End If
            .Enabled = en
        End With

        With COUNTY
            If en Then
                .BackColor = Color.White
            Else
                .BackColor = Color.LightGray
            End If
            .Enabled = en
        End With

    End Sub

    Sub ClearForm()
        Me.TITLE.Text = ""
        Me.TITLE.BackColor = Color.White
        Me.FIRSTNAME.Text = ""
        Me.FIRSTNAME.BackColor = Color.White
        Me.LASTNAME.Text = ""
        Me.LASTNAME.BackColor = Color.White
        Me.COMPANY.Text = ""
        Me.COMPANY.BackColor = Color.White
        Me.EMAIL.Text = ""
        Me.EMAIL.BackColor = Color.White
        Me.TELEPHONE.Text = ""
        Me.TELEPHONE.BackColor = Color.White
        Me.MOBILE.Text = ""
        Me.MOBILE.BackColor = Color.White
        Me.Incode.Text = ""
        Me.Incode.BackColor = Color.White
        Me.Outcode.Text = ""
        Me.Outcode.BackColor = Color.White
        Me.STREET.Text = ""
        Me.STREET.BackColor = Color.White
        Me.ADDRESS1.Text = ""
        Me.ADDRESS1.BackColor = Color.White
        Me.ADDRESS2.Text = ""
        Me.ADDRESS2.BackColor = Color.White
        Me.CITY.Text = ""
        Me.CITY.BackColor = Color.White
        Me.COUNTY.Text = ""
        Me.COUNTY.BackColor = Color.White
        Me.NOTES.Text = ""
        Me.NOTES.BackColor = Color.White
        With cbo_Address
            .BackColor = Color.White
            .Items.Clear()
            .Text = ""
        End With
        CheckBox1_CheckedChanged(Me, New System.EventArgs)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        If Not (IsValid()) Then
            MsgBox("Please fill in all the required fields.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim data() As String
        Dim sOut As New StreamWriter(OUTPUTFILE, True)
        With sOut
            .WriteLine( _
                "1" & Chr(9) & _
                Me.TITLE.Text & Chr(9) & _
                Me.FIRSTNAME.Text & Chr(9) & _
                Me.LASTNAME.Text & Chr(9) & _
                Me.COMPANY.Text & Chr(9) & _
                Me.EMAIL.Text & Chr(9) & _
                Me.TELEPHONE.Text & Chr(9) & _
                Me.MOBILE.Text & Chr(9) & _
                 Chr(9) & _
                 Chr(9) & _
                 Chr(9) & _
                 Chr(9) & _
                Chr(9) & _
                Me.Incode.Text & Chr(9) & _
                Me.Outcode.Text & Chr(9) _
            )
            .WriteLine( _
                "2" & Chr(9) & _
                 Chr(9) & _
                 Chr(9) & _
                 Chr(9) & _
                 Chr(9) & _
                Chr(9) & _
                Chr(9) & _
                 Chr(9) & _
                Me.STREET.Text & Chr(9) & _
                Me.ADDRESS1.Text & Chr(9) & _
                Me.ADDRESS2.Text & Chr(9) & _
                Me.CITY.Text & Chr(9) & _
                Me.COUNTY.Text & Chr(9) & _
                Me.Incode.Text & Chr(9) & _
                Me.Outcode.Text & Chr(9) _
            )

            '*************************************
            Dim Build As String = ""

            Newitem(data, "<style> p,div,li ")
            Newitem(data, "{margin:0cm;font-size:10.0pt;font-family:'Arial';}</style>")            

            Dim rText As String = Me.NOTES.Text
            Dim ln() As String = Split(Replace(rText, Chr(10), ""), Chr(13) & Chr(13))
            rText = ""
            For Each l As String In ln
                rText = rText & " <p> " & Replace(l, Chr(13), " <br> ") & " </p> "
            Next

            Dim words() = Split(rText, " ")
            For i As Integer = 0 To UBound(words)
                If Len(Build & " " & words(i)) > 68 Then
                    Newitem(data, Build)
                    Build = "" & words(i) & " "
                Else
                    Build = Build & words(i) & " "
                End If
            Next

            Newitem(data, Build)
            Build = ""

            '*******************************
            For y As Integer = 0 To UBound(data)
                .WriteLine( _
                    "3" & Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                    Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                    Chr(9) & _
                     Chr(9) & _
                     Chr(9) & _
                    data(y) _
                )
            Next
            .Close()
        End With


        ClearForm()
        MsgBox("Your details have been saved.", MsgBoxStyle.Information)
    End Sub

    Private Sub ClearFormToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearFormToolStripMenuItem.Click
        ClearForm()
    End Sub

    Private Sub UploadContactsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UploadContactsToolStripMenuItem.Click
        ftpFile()
    End Sub

    Private Sub Newitem(ByRef data() As String, ByVal item As String)
        Try
            ReDim Preserve data(UBound(data) + 1)
        Catch
            ReDim data(0)
        Finally
            data(UBound(data)) = item
        End Try
    End Sub

    Private Sub FTPSettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTPSettingsToolStripMenuItem.Click
        ftpDetails.ShowDialog()
    End Sub

    Private Function IsValid() As Boolean
        Return Not (isblank(Me.TITLE)) And _
                    Not (isblank(Me.FIRSTNAME)) And _
                    Not (isblank(Me.LASTNAME)) And _
                    Not (isblank(Me.TELEPHONE)) And _
                    Not (isblank(Me.STREET)) And _
                    Not (isblank(Me.Incode)) And _
                    Not (isblank(Me.Outcode))
    End Function

    Private Function isblank(ByVal obj As Object) As Boolean
        Dim ret As Boolean = True
        If obj.text <> "" Then
            obj.backcolor = Color.White
            ret = False
        Else
            obj.backcolor = Color.Red
            ret = True
        End If
        Return ret
    End Function

    Private Sub PCNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCNameToolStripMenuItem.Click
        My.Settings.pc = InputBox("This Machine Name", , My.Settings.pc)
        If My.Settings.pc <> "" Then My.Settings.Save()
    End Sub
End Class
