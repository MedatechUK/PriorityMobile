Imports PDA.o

Public Class ct_Address

    Private _ServiceCall As String
    Private _App As PDAOnBoardData.BaseForm

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            ShowDetail()
        End Set
    End Property

    Public Sub New(ByRef App As PDAOnBoardData.BaseForm) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _App = App

    End Sub

    Private Sub ShowDetail()

        If IsNothing(_App.rss(o.ServiceCall).thisArray) Then
            Exit Sub
        End If

        With _App.rss(o.ServiceCall)

            .currentIndex = _ServiceCall

            If Not .Validate Then
                Exit Sub
            Else

                lbl_ADDRESS1.Text = _App.rss(o.ServiceCall).GetField("ADDRESS1")
                lbl_ADDRESS2.Text = _App.rss(o.ServiceCall).GetField("ADDRESS2")
                lbl_ADDRESS3.Text = _App.rss(o.ServiceCall).GetField("ADDRESS3")
                lbl_POSTCODE.Text = _App.rss(o.ServiceCall).GetField("POSTCODE")
                lbl_COUNTY.Text = _App.rss(o.ServiceCall).GetField("COUNTY")
                lbl_CONTACT.Text = _App.rss(o.ServiceCall).GetField("CONTACT")

                lbl_PHONENUM.Text = _App.rss(o.ServiceCall).GetField("PHONENUM")
                lbl_CUSTNAME.Text = _App.rss(o.ServiceCall).GetField("CUSTNAME")

            End If
        End With
    End Sub

    Private Sub ct_CallDetails_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim os As Integer = 2
        Dim l As Point
        Dim s As Size

        Dim fs As Single

        If Me.Width > 319 Then
            fs = 14
        ElseIf Me.Width > 268 Then
            fs = 12
        ElseIf Me.Width > 258 Then
            fs = 11
        ElseIf Me.Width > 241 Then
            fs = 10
        ElseIf Me.Width > 214 Then
            fs = 9
        ElseIf Me.Width > 199 Then
            fs = 8
        Else
            fs = 8
        End If

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        With Me
            .lbl_Address.Font = f
            .lbl_Customer.Font = f
            .lbl_Cont.Font = f
            .lbl_Phone.Font = f

            .lbl_CUSTNAME.Font = c
            .lbl_ADDRESS1.Font = c
            .lbl_ADDRESS2.Font = c
            .lbl_ADDRESS3.Font = c
            .lbl_COUNTY.Font = c
            .lbl_POSTCODE.Font = c
            .lbl_CONTACT.Font = c
            lbl_PHONENUM.Font = c
            lbl_CONTACT.Font = c

            Dim r As Integer = 10 + MaxOp(.lbl_Address.Width, .lbl_Customer.Width)

            .lbl_Customer.Left = r - .lbl_Customer.Width
            .lbl_Address.Left = r - .lbl_Address.Width
            .lbl_Address.Top = .lbl_Customer.Top + .lbl_Customer.Height + os
            .lbl_Phone.Left = r - lbl_Phone.Width
            .lbl_Cont.Left = r - lbl_Cont.Width

            .lbl_CUSTNAME.Top = .lbl_Customer.Top
            .lbl_CUSTNAME.Left = r + os
            .lbl_PHONENUM.Left = r + os
            .lbl_CONTACT.Left = r + os

            l.X = .lbl_CUSTNAME.Left
            l.Y = .lbl_Address.Top
            .pnl_Address.Location = l

            .lbl_ADDRESS2.Top = .lbl_ADDRESS1.Top + .lbl_ADDRESS1.Height + os
            .lbl_ADDRESS3.Top = .lbl_ADDRESS2.Top + .lbl_ADDRESS2.Height + os
            .lbl_COUNTY.Top = .lbl_ADDRESS3.Top + .lbl_ADDRESS3.Height + os
            .lbl_POSTCODE.Top = .lbl_COUNTY.Top + .lbl_COUNTY.Height + os

            s.Height = .lbl_POSTCODE.Top + .lbl_COUNTY.Height + os
            s.Width = .Width - .pnl_Address.Location.X
            .pnl_Address.Size = s

            .lbl_CONTACT.Top = .pnl_Address.Location.Y + .pnl_Address.Size.Height + os + os
            .lbl_Cont.Top = .lbl_CONTACT.Top

            .lbl_Phone.Top = .lbl_Cont.Top + .lbl_Cont.Height + os
            .lbl_PHONENUM.Top = .lbl_Phone.Top
        End With

    End Sub

    Private Function MaxOp(ByVal val1 As Integer, ByVal val2 As Integer)
        If val1 > val2 Then Return val1 Else Return val2
    End Function

End Class
