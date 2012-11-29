Imports System.Windows.Forms

Public Class frmOptions

    Dim mCaller As MDIChild

    Public Sub New(ByVal Caller As MDIChild)
        InitializeComponent()
        mCaller = Caller
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        With mCaller
            .CursorName = txt_mCursor.Text
            .StartLabel = txt_labelStart.Text
            .RecordType = txt_mRecordType.Text
            .VarPref = txt_mVarPref.Text
            .IsSub = boolOpt.GetItemChecked(0)
            .WriteLoad = boolOpt.GetItemChecked(1)
            .LinkGenLoad = boolOpt.GetItemChecked(2)
            .SelectVars = boolOpt.GetItemChecked(3)
            setHeader(mCaller)
            .doToolbar()
            .Changed = True
        End With
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With mCaller
            txt_mCursor.Text = .CursorName
            txt_labelStart.Text = .StartLabel
            txt_mRecordType.Text = .RecordType
            txt_mVarPref.Text = .VarPref

            boolOpt.Items.Clear()
            boolOpt.Items.Add("This procedure is a subroutine.", .IsSub)
            boolOpt.Items.Add("Generate code for a loading.", .WriteLoad)
            boolOpt.Items.Add("Link Load to GENERALLOAD table.", .LinkGenLoad)
            boolOpt.Items.Add("Generate Select statement.", .SelectVars)

        End With
    End Sub

End Class
