Imports PriorityMobile
Imports System.Xml
Imports System.Data

Public Class ctrl_Warehouse
    Inherits iView

    Private ReadOnly Property scCurr() As xForm
        Get
            Return TopForm("Deliveries").CurrentForm
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
                .Sort = "partname"
                .AddColumn("partname", "Part", 150)
                .AddColumn("partdes", "Description", 250)                
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
                "" _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                With .Items(.Items.Count - 1)
                    If String.Compare(r("partname"), thisForm.CurrentRow("partname"), True) = 0 Then
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
            .Add(AddressOf hCalc, "PASTE.BMP", CBool(String.Compare(scCurrView.XMLType, "part", True) = 0) And Not IsNothing(thisForm.TableData.Current))
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
                "{1}[partname={0}{2}{0}]", _
                Chr(34), _
                scCurr.boundxPath, _
                thisForm.CurrentRow("partname") _
            ) _
        )
        If IsNothing(pe) Then
            pe = xmlForms.FormData.Document.CreateElement("part")
            With pe                
                .AppendChild(xmlForms.FormData.Document.CreateElement("partname"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("partname")
                .AppendChild(xmlForms.FormData.Document.CreateElement("partdes"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.CurrentRow("partdes")
                .AppendChild(xmlForms.FormData.Document.CreateElement("qty"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = MyValue.ToString
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
        Dim part As String = InputBox("Part Number")
        Dim found As Boolean = False
        If part.Length > 0 Then
            Dim dt As DataTable = thisForm.TableData.DataSource
            For Each row As DataRow In dt.Rows
                If String.Compare(row("partname"), part, True) = 0 Then
                    found = True
                    With thisForm.TableData
                        .MoveFirst()
                        While Not (String.Compare(row("partname"), thisForm.CurrentRow("partname")) = 0)
                            .MoveNext()
                        End While
                        Bind()
                    End With
                    Exit For
                End If
            Next
            If Not found Then
                MsgBox( _
                    String.Format( _
                        "Could not find part number {0}.", _
                        part _
                    ) _
                )
            End If
        End If
    End Sub

#End Region

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value("partname", row)
            If Not (String.Compare(.CurrentRow("partname"), ListSort1.Value("partname", row)) = 0) Then
                .TableData.Position = .TableData.Find("partname", cur)
                .RefreshSubForms()
                .RefreshDirectActivations()
            End If
        End With
    End Sub

End Class
