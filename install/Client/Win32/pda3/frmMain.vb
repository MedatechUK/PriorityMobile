Imports System.IO
Imports ViewControl

Public Class frmMain

    Private ue As UserEnv
    Private WithEvents xf As xmlForms
    Private TopMenuSelectedColour As Color
    Private TopMenuBackgroundColour As Color

#Region "View Control References"

    Private ReadOnly Property Active() As TopLevelForm
        Get
            With xmlForms.TopForm
                Return .Item(.Keys(xf.ActiveForm))
            End With
        End Get
    End Property

    Private ReadOnly Property Current() As xForm
        Get
            Return Active.CurrentForm
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        TopMenuSelectedColour = Color.FromArgb(194, 224, 255)
        TopMenuBackgroundColour = Color.FromArgb(240, 240, 240)

        ue = New UserEnv("tabula", New Uri("http://mobile.emerge-it.co.uk:8080/"))

        File.Delete(ue.LocalFolder & "\forms.xml")
        File.Delete(ue.LocalFolder & "\calls.xml")
        File.Delete(ue.LocalFolder & "\lookup.xml")

        xf = New xmlForms( _
            New OfflineXML(ue, "forms.xml", "forms.ashx"), _
            New OfflineXML(ue, "calls.xml", "calls.ashx"), _
            New OfflineXML(ue, "lookup.xml", "lookup.ashx") _
        )

        AddHandler xf.AddUserControl, AddressOf xf_AddUserControl
        xf.LoadViewControls()

        With Me.mnu_TopForms.Items
            .Clear()
            For Each t As TopLevelForm In xmlForms.TopForm.Values
                .Add(t.TopForm.FormName)
                .Item(.Count - 1).Tag = .Count - 1
                .Item(.Count - 1).BackColor = TopMenuBackgroundColour
                AddHandler .Item(.Count - 1).Click, AddressOf hTopMenuClick
            Next
            .Item(0).BackColor = TopMenuSelectedColour
        End With

        With Active.CurrentForm
            .RefreshForm()
            .RefreshSubForms()
            .RefreshDirectActivations()
        End With

    End Sub

    Private Sub xf_AddUserControl(ByVal ControlName As String, ByRef view As ViewControl.iView, ByRef thisForm As xForm)
        Select Case ControlName
            Case "SUMMARY"
                view = New ctrl_Summary()                
            Case "STATUSPANE"
                view = New ctrl_StatusPane
            Case "REPAIR"
                view = New ctrl_Repair
            Case "SIGN"
                view = New ctrl_Sign
            Case "ADDRESS"
                view = New ctrl_Address
            Case "CALLDETAIL"
                view = New ctrl_CallDetail
            Case "PARTSPLANNED"
                view = New ctrl_Parts_Planned
            Case "PARTSACTUAL"
                view = New ctrl_Parts_Actual
            Case "WAREHOUSE"
                view = New ctrl_Warehouse
            Case Else
                view = New ctrl_DataGrid
        End Select
        view.SetForm(thisForm)
    End Sub

#End Region

#Region "Draw Interface elements"

    Private Sub DrawForm() Handles xf.drawform
        With Me
            With .ToolStripContainer1.ContentPanel
                With .Controls
                    .Clear()
                    .Add(Current.Views(Current.CurrentView))
                End With
                .Controls(.Controls.Count - 1).Dock = DockStyle.Fill
                Current.Views(Current.CurrentView).SetFocus()
            End With
        End With
    End Sub

    Private Sub DrawSubMenu() Handles xf.DrawSubMenu
        With Me
            With .mnu_SubForms.Items
                .Clear()
                For Each s As xForm In Current.VisibleSubForms.Values
                    .Add(s.FormName)
                    .Item(.Count - 1).Tag = s.FormName
                    RemoveHandler .Item(.Count - 1).Click, AddressOf hSubMenuClick
                    AddHandler .Item(.Count - 1).Click, AddressOf hSubMenuClick
                Next
            End With
        End With
    End Sub

    Private Sub DrawDirectActivations() Handles xf.DrawDirectActivations

        With ToolStrip1.Items
            .Clear()
            .Add("", Image.FromFile("icons\UP1LVL.BMP"), AddressOf btn_Up_Click)
            .Item(.Count - 1).Enabled = Not IsNothing(Current.Parent)
            .Add("", Active.CurrentForm.NextViewButton, AddressOf btn_View_Click)
            .Item(.Count - 1).Enabled = CBool(Current.Views.Count > 1)
            .Add(New ToolStripSeparator)
        End With
        Active.CurrentForm.Views(Active.CurrentForm.CurrentView).DirectActivations(ToolStrip1)

    End Sub

    Public Sub StartCalc(ByVal Max As Integer) Handles xf.StartCalc
        With Me
            With .ToolStripContainer1.ContentPanel
                With .Controls
                    .Clear()
                    Dim C As New calc
                    C.Max = Max
                    AddHandler C.SetNumber, AddressOf Active.CurrentForm.Views(Active.CurrentForm.CurrentView).SetNumber
                    .Add(C)
                End With
                .Controls(.Controls.Count - 1).Dock = DockStyle.Fill

            End With
        End With
    End Sub

#End Region

#Region "Interface Event Handlers"

    Private Sub hTopMenuClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim S As ToolStripItem = sender
        If Not (xf.ActiveForm = CInt(S.Tag)) Then
            xf.ActiveForm = CInt(S.Tag)
            With Active.CurrentForm
                .RefreshForm()
                .RefreshSubForms()
                .RefreshDirectActivations()
            End With
        End If
        For Each i As ToolStripItem In Me.mnu_TopForms.Items
            i.BackColor = TopMenuBackgroundColour
        Next
        S.BackColor = TopMenuSelectedColour
    End Sub

    Private Sub hSubMenuClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim S As ToolStripItem = sender
        Active.OpenForm(S.Tag)
        DrawSubMenu()
        DrawForm()
    End Sub

    Private Sub btn_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Active.CloseForm()
        DrawSubMenu()
        DrawForm()
    End Sub

    Private Sub btn_View_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Current.CurrentView += 1
        DrawForm()
    End Sub

#End Region

End Class