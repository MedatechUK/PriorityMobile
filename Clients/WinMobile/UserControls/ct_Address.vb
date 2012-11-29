Imports cfpda.o

Public Class ct_Address

    Private _ServiceCall As String
    Private CallerApp As cfOnBoardData.BaseForm

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            ShowDetail()
        End Set
    End Property

    Public Sub New(ByRef App As cfOnBoardData.BaseForm) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CallerApp = App

    End Sub

    Private Sub ShowDetail()

        If IsNothing(CallerApp.rss(o.ServiceCall).thisArray) Then
            Exit Sub
        End If

        With CallerApp.rss(o.ServiceCall)

            .currentIndex = _ServiceCall

            If Not .Validate Then
                Exit Sub
            Else

                Address.Text = _
                CRLFifData(.GetField("ADDRESS1")) & _
                CRLFifData(.GetField("ADDRESS2")) & _
                CRLFifData(.GetField("ADDRESS3")) & _
                CRLFifData(.GetField("POSTCODE")) & _
                CRLFifData(.GetField("COUNTY"))

                lbl_CONTACT.Text = .GetField("CONTACT")
                lbl_PHONENUM.Text = .GetField("PHONENUM")
                lbl_CUSTNAME.Text = .GetField("CUSTNAME")

            End If
        End With
    End Sub

    Public Function CRLFifData(ByVal str As String) As String
        If Len(Trim(str)) > 0 Then
            Return str & vbCrLf
        Else
            Return ""
        End If
    End Function

    Private Sub ct_CallDetails_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim os As Integer = 2
        Dim l As Point
        Dim s As Size

        Dim fs As Single = GetFontSize(Me.Width)

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        With Me
            With .lbl_Address
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lbl_Customer
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lbl_Cont
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lbl_Phone
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lbl_CUSTNAME
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With


            'With .lbl_ADDRESS2
            '    .Font = f
            '    .Width = TextSize(.Text, .Font).Width
            '    .Height = TextSize(.Text, .Font).Height
            'End With

            'With .lbl_ADDRESS3
            '    .Font = f
            '    .Width = TextSize(.Text, .Font).Width
            '    .Height = TextSize(.Text, .Font).Height
            'End With

            'With .lbl_COUNTY
            '    .Font = f
            '    .Width = TextSize(.Text, .Font).Width
            '    .Height = TextSize(.Text, .Font).Height
            'End With

            'With .lbl_POSTCODE
            '    .Font = f
            '    .Width = TextSize(.Text, .Font).Width
            '    .Height = TextSize(.Text, .Font).Height
            'End With

            With .lbl_CONTACT
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With .lbl_PHONENUM
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With lbl_CONTACT
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            Dim r As Integer = 10 + MaxOp(.lbl_Address.Width, .lbl_Customer.Width)

            .lbl_Customer.Top = os
            .lbl_Customer.Left = r - .lbl_Customer.Width
            .lbl_Address.Left = r - .lbl_Address.Width
            .lbl_Address.Top = .lbl_Phone.Top + .lbl_Customer.Height + (os * 3)
            .lbl_Phone.Left = r - lbl_Phone.Width
            .lbl_Cont.Left = r - lbl_Cont.Width

            .lbl_CUSTNAME.Top = .lbl_Customer.Top
            .lbl_CUSTNAME.Left = r + os
            .lbl_PHONENUM.Left = r + os
            .lbl_CONTACT.Left = r + os

            .lbl_CONTACT.Top = .lbl_CUSTNAME.Top + .lbl_CUSTNAME.Height + os
            .lbl_Cont.Top = .lbl_CONTACT.Top

            .lbl_Phone.Top = .lbl_Cont.Top + .lbl_Cont.Height + os
            .lbl_PHONENUM.Top = .lbl_Phone.Top

            l.X = .lbl_CUSTNAME.Left
            l.Y = .lbl_Address.Top
                        


        End With

        With Me.Address
            .Font = f
            .Width = Me.Width - (os * 3)
            .Left = os
            .Top = lbl_Address.Top + lbl_Address.Height + (os * 3)
            .Height = Me.Height - (Me.lbl_Address.Top + lbl_Address.Height + os)
        End With

    End Sub

    Private Function MaxOp(ByVal val1 As Integer, ByVal val2 As Integer)
        If val1 > val2 Then Return val1 Else Return val2
    End Function

End Class
