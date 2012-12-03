Public Class calc
    Dim btn(11) As Button
    Dim _Max As Integer
    Dim _Value As Integer

    Private mclosing As Boolean = True
    Private _Handler As Object
    Public Event SetNumber(ByVal MyValue As Integer)

    Public Property Max() As Integer
        Get
            Return _Max
        End Get
        Set(ByVal value As Integer)
            _Max = value
        End Set
    End Property

    Public Property isClosing() As Boolean
        Get
            Return mclosing
        End Get
        Set(ByVal value As Boolean)
            mclosing = value
        End Set
    End Property

    Public Property Value() As Integer
        Get
            Return CInt(Me.txt_Number.Text)
        End Get
        Set(ByVal value As Integer)
            Me.txt_Number.Text = CStr(value)
        End Set
    End Property


    Private Sub ct_number_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        'If mclosing Then Exit Sub
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

        Dim f As New Font("Microsoft Sans Serif", fs + 2, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs + 4, FontStyle.Regular)

        txt_Number.Font = c

        Dim i As Integer = 0
        For y As Integer = 0 To 3
            For x As Integer = 0 To 2
                If IsNothing(btn(i)) Then
                    btn(i) = New Button
                End If
                btn(i).Width = Me.Width / 3
                btn(i).Height = (Me.Height - txt_Number.Height) / 4
                btn(i).Font = f
                btn(i).Top = (txt_Number.Top + txt_Number.Height) + (y * btn(i).Height)
                btn(i).Left = x * btn(i).Width
                i = i + 1
            Next
        Next

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()        

        ' Add any initialization after the InitializeComponent() call.

        For i As Integer = 0 To 11
            btn(i) = New Button
            btn(i).Width = Me.Width / 3
            btn(i).Height = (Me.Height - txt_Number.Height) / 4
            Select Case i
                Case 0, 1, 2, 3, 4, 5, 6, 7, 8
                    btn(i).Text = CStr(i + 1)
                Case 9
                    btn(i).Text = "<"
                Case 10
                    btn(i).Text = "0"
                Case 11
                    btn(i).Text = "Ok"
            End Select
            AddHandler btn(i).Click, AddressOf Btn_Click
            Me.Controls.Add(btn(i))
        Next
        Me.txt_Number.Text = "0"

    End Sub

    Private Sub Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = sender
        Select Case Strings.Left(btn.Text, 1)
            Case "O"
                Me.isClosing = True
                RaiseEvent SetNumber(CInt(txt_Number.Text))
            Case "C"
                txt_Number.Text = ""
            Case "<"
                If Len(txt_Number.Text) > 0 Then
                    txt_Number.Text = Strings.Left(txt_Number.Text, Len(txt_Number.Text) - 1)
                End If
            Case Else
                Me.txt_Number.Text = Me.txt_Number.Text & btn.Text
        End Select
        If txt_Number.Text = "" Then txt_Number.Text = "0"
        If Not IsNumeric(txt_Number.Text) Then
            txt_Number.Text = "0"
        Else
            If CInt(txt_Number.Text) > Max Then txt_Number.Text = CStr(Max)
            txt_Number.Text = CStr(CInt(txt_Number.Text))
        End If

    End Sub
End Class
