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
                "planned = 'Y' and id <> 'dummy'" _
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
        Select Case frmDialog.Name.ToLower
            Case "add"
                CloseAddDialog(frmDialog)
            Case "find"
                CloseFindDialog(frmDialog)
        End Select
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)

        With ToolBar
            .Add(AddressOf hFind, "FIND.BMP")
            .Add(AddressOf hCalc, "ADD.BMP", (ListSort1.SelectedIndex <> -1))
        End With

    End Sub

    Private Sub hCalc()

        With thisForm
            If .CurrentRow("serialpart") = "Y" Then
                Dim dlg As New dlgAddPlanned
                dlg.Name = "add"
                Dim txtserial = dlg.FindControl("txtSerialNumber")
                txtserial.Text = ListSort1.Value("serial", ListSort1.SelectedIndex) '.CurrentRow("serial")
                If String.Compare(ListSort1.Value("serial", ListSort1.SelectedIndex).ToString, "0", True) = 0 Then
                    txtserial.Text = ""
                    txtserial.Enabled = True
                    dlg.FocusContolName = "txtSerialNumber"
                Else
                    txtserial.Text = ListSort1.Value("serial", ListSort1.SelectedIndex).ToString
                    txtserial.Enabled = False
                    dlg.FocusContolName = "txtLocation"
                End If
                .Dialog(dlg)
            Else
                updatePart()
                thisForm.Bind()
                thisForm.RefreshForm()
            End If
        End With        

    End Sub

    Private Sub CloseAddDialog(ByVal frmDialog As PriorityMobile.UserDialog)
        With thisForm
            If frmDialog.Result = DialogResult.OK Then
                Dim part As String = ListSort1.Value("name", ListSort1.SelectedIndex)
                Dim serial As TextBox = frmDialog.FindControl("txtSerialNumber")
                If serial.Enabled Then
                    If Not (String.Compare(serial.Text, .CurrentRow("serial").ToString, True) = 0) Then
                        Dim n As XmlNode = .FormData.SelectSingleNode( _
                            String.Format("{3}[name={0}{1}{0} and serial={0}{2}{0}]/serial", _
                                      Chr(34), _
                                    ListSort1.Value("name", ListSort1.SelectedIndex), _
                                    ListSort1.Value("serial", ListSort1.SelectedIndex), _
                                    .boundxPath _
                                ) _
                            )
                        n.InnerText = serial.Text
                    End If
                    .Bind()
                    .TableData.MoveFirst()
                    Do Until .CurrentRow("name") = part And .CurrentRow("serial") = serial.Text
                        .TableData.MoveNext()
                    Loop
                End If

                Dim DOA As CheckBox = frmDialog.FindControl("chkBroken")
                Dim p As XmlNode = .FormData.SelectSingleNode( _
                    String.Format( _
                        "{3}[name={0}{1}{0} and serial={0}{2}{0}]", _
                            Chr(34), _
                            ListSort1.Value("name", ListSort1.SelectedIndex), _
                            ListSort1.Value("serial", ListSort1.SelectedIndex), _
                            .boundxPath _
                        ) _
                    )

                p.SelectSingleNode("planned").InnerText = "N"
                If DOA.Checked Then
                    p.SelectSingleNode("qty").InnerText = "-1"                    
                Else
                    Dim location As TextBox = frmDialog.FindControl("txtLocation")                    
                    p.SelectSingleNode("location").InnerText = location.Text
                End If

                updatePart()

            End If            
            thisForm.Bind()
            thisForm.RefreshForm()

        End With
    End Sub

    Private Sub hFind()
        Dim dlg As New dlgBarcode
        dlg.Name = "find"
        dlg.FocusContolName = "txtSerialNumber"
        thisForm.Dialog(dlg)
    End Sub

    Private Sub CloseFindDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        If frmDialog.Result = DialogResult.OK Then

            Dim tbserial As TextBox = frmDialog.FindControl("txtSerialNumber")
            Dim serial As String = tbserial.Text
            Dim found As Boolean = False

            Dim dt As DataTable = thisForm.TableData.DataSource
            For Each row As DataRow In dt.Rows
                If String.Compare(row("serial"), serial, True) = 0 Or String.Compare(row("name"), serial, True) = 0 Then
                    found = True
                    With thisForm.TableData
                        .MoveFirst()
                        While Not (String.Compare(row("name"), thisForm.CurrentRow("name")) = 0 And String.Compare(row("serial"), thisForm.CurrentRow("serial")) = 0)
                            .MoveNext()
                        End While
                        Bind()
                    End With
                    Exit For
                End If
            Next

            If Not found Then
                thisForm.RefreshForm()
                MsgBox( _
                    String.Format( _
                        "Could not find serial number {0}.", _
                        serial _
                    ) _
                )
            Else
                thisForm.RefreshForm()
            End If

        Else
            thisForm.RefreshForm()
        End If

    End Sub

#End Region

#Region "Private Methods"

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

    Private Sub updatePart()

        With thisForm

            Dim p As XmlNode = .FormData.SelectSingleNode( _
            String.Format( _
                "{3}[name={0}{1}{0} and serial={0}{2}{0}]", _
                    Chr(34), _
                    ListSort1.Value("name", ListSort1.SelectedIndex), _
                    ListSort1.Value("serial", ListSort1.SelectedIndex), _
                    .boundxPath _
                ) _
            )
            p.Attributes.Append(xmlForms.changedAttribute)
            p.SelectSingleNode("planned").InnerText = "N"

            If p.SelectSingleNode("fromstock").InnerText = "Y" Then
                Dim whNode As XmlNode = .FormData.SelectSingleNode( _
                    String.Format("pdadata/warehouse/part[name={0}{1}{0} and serial={0}{2}{0}]", _
                              Chr(34), _
                            p.SelectSingleNode("name").InnerText, _
                            p.SelectSingleNode("serial").InnerText _
                        ) _
                    )
                If Not IsNothing(whNode) Then
                    whNode.SelectSingleNode("qty").InnerText = CStr(CInt(whNode.SelectSingleNode("qty").InnerText) - CInt(.CurrentRow("qty")))
                    .TableData.EndEdit()

                End If
            End If

            .Save()

            Bind()

        End With

    End Sub

#End Region

End Class
