Imports PriorityMobile
Imports System.Xml
Imports System.Data

Public Class ctrl_Warehouse
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
                "name <> 'dummy'" _
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

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)

        With ToolBar
            .Add(AddressOf hFind, "FIND.BMP")
            .Add(AddressOf hCalc, "ADD.BMP", CBool(String.Compare(scCurrView.XMLType, "part", True) = 0) And Not IsNothing(thisForm.TableData.Current))
        End With

    End Sub

    Public Overrides Sub SetNumber(ByVal MyValue As Integer)

        With thisForm
            .CurrentRow("qty") = CStr(CInt(thisForm.CurrentRow("qty")) - MyValue)
            ListSort1.Value("qty", ListSort1.SelectedIndex) = .CurrentRow("qty")
            .TableData.EndEdit()
        End With

        Dim pe As XmlNode
        pe = xmlForms.FormData.Document.SelectSingleNode( _
            String.Format( _
                "{1}[name={0}{2}{0} and serial={0}{3}{0}]", _
                Chr(34), _
                scCurr.boundxPath, _
                thisForm.CurrentRow("name"), _
                thisForm.CurrentRow("serial") _
            ) _
        )
        If IsNothing(pe) Then
            pe = xmlForms.FormData.Document.CreateElement("part")
            With pe
                .AppendChild(xmlForms.FormData.Document.CreateElement("id"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("name") & "~" & thisForm.CurrentRow("serial")
                .AppendChild(xmlForms.FormData.Document.CreateElement("name"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("name")
                .AppendChild(xmlForms.FormData.Document.CreateElement("des"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("des")
                .AppendChild(xmlForms.FormData.Document.CreateElement("serial"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("serial")
                .AppendChild(xmlForms.FormData.Document.CreateElement("qty"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = MyValue.ToString                
                .AppendChild(xmlForms.FormData.Document.CreateElement("fromstock"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = "Y"
                .AppendChild(xmlForms.FormData.Document.CreateElement("planned"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = "N"
                .AppendChild(xmlForms.FormData.Document.CreateElement("serialpart"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("serialpart")
                .AppendChild(xmlForms.FormData.Document.CreateElement("location"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = ""
                .AppendChild(xmlForms.FormData.Document.CreateElement("trans"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = "0"
            End With
            xmlForms.FormData.Document.SelectSingleNode(scCurr.boundxPath).ParentNode.AppendChild(pe)
        Else
            With pe
                .SelectSingleNode("qty").InnerText = CStr(CInt(pe.SelectSingleNode("qty").InnerText) + MyValue.ToString)                
            End With
        End If

        With xmlForms.FormData
            Dim partsnode As XmlNode = .Document.SelectSingleNode( _
                String.Format( _
                    "{0}", _
                    scCurr.boundxPath _
                ) _
            )
            partsnode.ParentNode.Attributes.Append( _
                xmlForms.changedAttribute _
            )
            thisForm.Save()
        End With

        scCurr.Save()
        scCurr.Bind()
        For Each V As iView In scCurr.Views
            V.Bind()
        Next
        scCurrView.RefreshData()
        scCurr.RefreshForm()



    End Sub

    Private Sub hCalc()
        thisForm.Calc(thisForm.CurrentRow("qty"))
    End Sub

    Private Sub hFind()
        Dim dlg As New dlgBarcode
        dlg.FocusContolName = "txtSerialNumber"
        thisForm.Dialog(dlg)
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

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

        thisForm.RefreshForm()

        If Not found Then
            MsgBox( _
                String.Format( _
                    "Could not find serial number {0}.", _
                    serial _
                ) _
            )
        End If

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
