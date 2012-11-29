Imports System.Windows.Forms

Public Class frmJoin

    Private mCaller As MDIChild

    Public Sub New(ByRef f As MDIChild)
        InitializeComponent()
        mCaller = f
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If SameType() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MsgBox("You have selected two different data types!", MsgBoxStyle.Exclamation)
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub lstTable1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTable1.SelectedIndexChanged
        With Me.lstColumn1.Items
            .Clear()
            For Each column As String In Columns(mCaller, lstTable1.Items(lstTable1.SelectedIndex))
                .Add(column)
            Next
        End With
        checkSubmit()
    End Sub

    Private Sub lstTable2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTable2.SelectedIndexChanged
        With Me.lstColumn2.Items
            .Clear()
            For Each column As String In Columns(mCaller, lstTable2.Items(lstTable2.SelectedIndex))
                .Add(column)
            Next
        End With
        checkSubmit()
    End Sub

    Private Sub lstColumn1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstColumn1.SelectedIndexChanged
        If lstTable2.SelectedIndex > -1 Then
            'For Each column As String In Columns(mCaller, lstTable2.Items(lstTable2.SelectedIndex))
            For i As Integer = 0 To lstColumn2.Items.Count - 1
                If lstColumn1.Items(lstColumn1.SelectedIndex) = lstColumn2.Items(i) Then
                    lstColumn2.SelectedIndex = i
                    Exit For
                End If
            Next
            'Next
        End If

        checkSubmit()

    End Sub

    Private Sub lstColumn2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstColumn2.SelectedIndexChanged
        checkSubmit()
    End Sub

    Private Sub checkSubmit()
        Me.OK_Button.Enabled = FilledIn()
    End Sub

    Private Function FilledIn() As Boolean
        Return lstTable1.SelectedIndex > -1 And _
            lstTable2.SelectedIndex > -1 And _
            lstColumn1.SelectedIndex > -1 And _
            lstColumn2.SelectedIndex > -1
    End Function

    Private Function SameType() As Boolean

        Dim t1 As String = ""
        Dim t2 As String = ""

        With mCaller
            For i As Integer = 0 To UBound(.Cols, 2)
                If .Cols(0, i) = lstColumn1.Items(lstColumn1.SelectedIndex) And .Cols(1, i) = lstTable1.Items(lstTable1.SelectedIndex) Then
                    t1 = .Cols(2, i)
                End If
                If .Cols(0, i) = lstColumn2.Items(lstColumn2.SelectedIndex) And .Cols(1, i) = lstTable2.Items(lstTable2.SelectedIndex) Then
                    t2 = .Cols(2, i)
                End If
            Next
        End With

        Return t1 = t2

    End Function

End Class
