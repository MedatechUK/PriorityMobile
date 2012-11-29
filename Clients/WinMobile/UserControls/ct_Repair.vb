Public Class ct_Repair
    Dim ar As cfMyCls.MyArray

    Dim _ServiceCall As String = ""
    Dim CallerApp As cfOnBoardData.BaseForm

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            NewData()
        End Set
    End Property

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not IsNothing(App) Then
            CallerApp = App
        End If

    End Sub

    Sub NewData()

        Dim MALFCODE As String = ""
        Dim RESCODE As String = ""

        With CallerApp.rss(o.Flags)
            .currentIndex = _ServiceCall
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = _ServiceCall
                .thisArray(1, nr) = ""
                .thisArray(2, nr) = ""
                .Save()
            Else
                MALFCODE = .GetField("MALFCODE")
                RESCODE = .GetField("RESCODE")
            End If
        End With

        With CallerApp.rss(o.ServiceCall)
            .currentIndex = _ServiceCall
            If Not .Validate Then
                Exit Sub
            Else
                BuildList(Me.lst_Malfunction, 1, CallerApp.rss(o.Malfunction), MALFCODE)
                BuildList(Me.lst_Resolution, 1, CallerApp.rss(o.Resolution), RESCODE)
            End If

        End With
        With CallerApp.rss(o.Repair)
            .currentIndex = _ServiceCall
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = _ServiceCall
                .thisArray(1, nr) = ""
                .Save()
            Else
                Me.txt_Repair.Text = .GetField("REPAIRTEXT")
            End If
        End With
    End Sub

    Private Sub BuildList(ByVal lst As ComboBox, ByVal Ordinal As Integer, ByVal thisRSS As cfOnBoardData.PDAData, ByVal strDefault As String)

        Dim f As Integer = 0
        With lst
            .Items.Clear()
            .Items.Add("")
        End With

        With thisRSS
            If Not IsNothing(.thisArray) Then
                For i As Integer = 0 To UBound(.thisArray, 2)
                    lst.Items.Add(.thisArray(Ordinal, i))
                    If Strings.StrComp(.thisArray(Ordinal, i), strDefault, CompareMethod.Text) = 0 Then
                        f = i + 1
                    End If
                Next
                lst.SelectedIndex = f
            End If
        End With

    End Sub

    Private Sub ct_Repair_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim os As Integer = 2
        Dim l As Point
        Dim s As Size

        Dim fs As Single = GetFontSize(Me.Width)

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)


        With Me

            With .lbl_Malfunction
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lbl_Resolution
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lst_Malfunction
                .Font = c
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
                .Top = os
            End With

            With .lst_Resolution
                .Font = c
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .txt_Repair
                .Font = c
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            Dim r As Integer = MaxOp(.lbl_Malfunction.Width, .lbl_Resolution.Width)

            .lbl_Malfunction.Left = os + (r - .lbl_Malfunction.Width)
            .lbl_Resolution.Left = os + (r - .lbl_Resolution.Width)

            .lst_Malfunction.Left = r + os
            .lst_Malfunction.Width = .Width - (r + (os * 2)) '.lst_Malfunction.Left

            .lst_Resolution.Left = r + os
            .lst_Resolution.Width = .Width - (r + (os * 2)) ' .lst_Resolution.Left

            .lst_Resolution.Top = .lst_Malfunction.Top + .lst_Malfunction.Height + os

            .lbl_Resolution.Top = .lst_Resolution.Top
            .lbl_Malfunction.Top = .lst_Malfunction.Top

            .txt_Repair.Height = .Height - (.lst_Resolution.Top + .lst_Resolution.Height + (os * 3))

        End With

    End Sub

    Private Function MaxOp(ByVal val1 As Integer, ByVal val2 As Integer)
        If val1 > val2 Then Return val1 Else Return val2
    End Function

    Private Sub lst_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Malfunction.SelectedValueChanged, lst_Resolution.SelectedValueChanged

        Dim temp As ComboBox = sender
        Select Case temp.Name
            Case "lst_Malfunction"
                With CallerApp.rss(o.Flags)
                    .SetField("MALFCODE", lst_Malfunction.Text)
                    .Save()
                End With
            Case "lst_Resolution"
                With CallerApp.rss(o.Flags)
                    .SetField("RESCODE", lst_Resolution.Text)
                    .Save()
                End With
        End Select

    End Sub

    Private Sub txt_Repair_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Repair.LostFocus
        With CallerApp.rss(7)
            .currentIndex = _ServiceCall
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = .currentIndex
                .thisArray(1, nr) = Me.txt_Repair.Text
                .Save()
            Else
                .SetField("REPAIRTEXT", Me.txt_Repair.Text)
                .Save()
            End If
        End With
    End Sub

End Class
