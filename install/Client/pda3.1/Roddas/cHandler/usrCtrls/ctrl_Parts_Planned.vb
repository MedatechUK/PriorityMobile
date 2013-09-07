Imports PriorityMobile
Imports System.Xml
Imports System.Data

Public Class ctrl_Parts_Planned
    Inherits iView

    Private ReadOnly Property scCurr() As xForm
        Get
            Return TopForm("Service Calls").CurrentForm
        End Get
    End Property

    Private ReadOnly Property scCurrView() As iView
        Get
            Return scCurr.Views(scCurr.CurrentView)
        End Get
    End Property

#Region "Initialisation and Finalisation"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With Me
            With ListSort1
                .Sort = "name"
                .AddColumn("name", "Part", 150)
                .AddColumn("des", "Description", 250)
                .AddColumn("serial", "Serial No", 150)
                .AddColumn("qty", "Qty", 50)
            End With
        End With

    End Sub

#End Region

#Region "Overides Base Properties"

    Public Overrides ReadOnly Property ButtomImage() As String
        Get
            Return "plan.bmp"
        End Get
    End Property

    Public Overrides ReadOnly Property Selected() As String
        Get
            Return ListSort1.Selected
        End Get
    End Property

#End Region

#Region "Overrides base Methods"

    Public Overrides Sub Bind() Handles ListSort1.Bind

        IsBinding = True
        Dim dr() As Data.DataRow = Nothing
        Dim query As String = String.Format( _
                "planned = 'Y'" _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                With .Items(.Items.Count - 1)
                    If String.Compare(r("name"), thisForm.CurrentRow("name"), True) = 0 And _
                        String.Compare(r("serial"), thisForm.CurrentRow("serial"), True) = 0 _
                    Then
                        .Selected = True
                        .Focused = True
                    Else
                        .Selected = False
                        .Focused = False
                    End If
                End With
            Next
            .Focus()
        End With
        thisForm.RefreshSubForms()
        IsBinding = False

    End Sub

    Public Overrides Sub SetFocus()
        Me.ListSort1.Focus()
    End Sub

    Public Overrides Sub ViewChanged()
        Bind()
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        If frmDialog.Result = DialogResult.OK Then

            With thisForm

                Dim tb As TextBox = frmDialog.FindControl("textbox1")

                Dim p As XmlNode = .FormData.SelectSingleNode( _
                String.Format( _
                    "{3}[name={0}{1}{0} and serial={0}{2}{0}]", _
                        Chr(34), _
                        .CurrentRow("name"), _
                        .CurrentRow("serial"), _
                        .boundxPath _
                    ) _
                )
                p.Attributes.Append(xmlForms.changedAttribute)

                .CurrentRow("planned") = "N"
                If .CurrentRow("fromstock") = "Y" Then
                    Dim whNode As XmlNode = .FormData.SelectSingleNode( _
                        String.Format("pdadata/warehouse/part[name={0}{1}{0} and serial={0}{2}{0}]", _
                                  Chr(34), _
                                .CurrentRow("name"), _
                                .CurrentRow("serial") _
                            ) _
                        )
                    If Not IsNothing(whNode) Then
                        whNode.SelectSingleNode("qty").InnerText = CStr(CInt(whNode.SelectSingleNode("qty").InnerText) - CInt(.CurrentRow("qty")))
                        .TableData.EndEdit()

                    End If
                End If

                .Save()
                .Bind()

                For Each V As iView In .Views
                    V.Bind()
                Next

                scCurrView.RefreshData()
                scCurr.RefreshForm()

            End With
        End If
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)

        With ToolBar
            .Add(AddressOf hCalc, "ADD.BMP", (ListSort1.SelectedIndex <> -1))
        End With

    End Sub

    Private Sub hCalc()

        With thisForm
            Dim dlg As New dlgAddPlanned
            Dim txtserial = dlg.FindControl("txtSerialNumber")
            txtserial.Text = .CurrentRow("serial")
            txtserial.Enabled = Not (.CurrentRow("serial").ToString.Length > 0)
            .Dialog(DLG)
        End With

    End Sub

#End Region

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value("name", row) & "~" & ListSort1.Value("serial", row)
            If Not (String.Compare(.CurrentRow("name"), ListSort1.Value("name", row)) = 0 And String.Compare(.CurrentRow("serial"), ListSort1.Value("serial", row)) = 0) Then
                .TableData.Position = .TableData.Find("id", cur)
                .RefreshSubForms()
                .RefreshDirectActivations()
            End If
        End With
    End Sub
End Class
