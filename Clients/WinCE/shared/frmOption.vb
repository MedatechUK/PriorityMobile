Public Class frmOption
    Dim itemCount As Integer
    Dim opt() As RadioButton
    Dim changing As Boolean

    Sub NewOption(ByVal OptName As String, ByVal OptValue As String, Optional ByVal Selected As Boolean = False)
        Try
            ReDim Preserve opt(UBound(opt) + 1)
        Catch ex As Exception
            ReDim opt(0)
        End Try

        itemCount = itemCount + 1

        opt(UBound(opt)) = New RadioButton
        AddHandler opt(UBound(opt)).CheckedChanged, AddressOf Me.handles_click
        With opt(UBound(opt))
            .Name = OptName
            .Text = OptValue
            .Checked = Selected
            .Left = 10
            .Width = Me.Width - (opt(UBound(opt)).Left * 2)
            .Height = 20
            .Top = opt(UBound(opt)).Height * itemCount
            .Visible = True
            .Show()
        End With

        Me.Controls.Add(opt(UBound(opt)))
        Me.Height = Me.Panel1.Height + (opt(UBound(opt)).Height * (itemCount + 3))
        Me.Panel1.Top = Me.Height - Me.Panel1.Height

        Panel1.Width = Me.Width - 5
        Panel2.Width = (Panel1.Width / 2)
        Panel3.Width = (Panel1.Width / 2)

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p

        enableButtons()

    End Sub

    Private Sub enableButtons()
        For i As Integer = 0 To UBound(opt)
            If opt(i).Checked Then
                btnOK.Enabled = True
                Exit Sub
            End If
        Next
        btnOK.Enabled = False
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        itemCount = 0

    End Sub

    Private Sub handles_click(ByVal sender As System.Object, ByVal e As System.EventArgs)        
        If Not (changing) Then
            Dim btn As RadioButton = sender
            changing = True
            For i As Integer = 0 To UBound(opt)
                opt(i).Checked = CBool(opt(i).Name = btn.Name)
            Next
            changing = False
        End If
        enableButtons()
    End Sub

    Public Function Selected() As String
        For i As Integer = 0 To UBound(opt)
            If opt(i).Checked Then
                Return opt(i).Name
            End If
        Next
        Return Nothing
    End Function

End Class