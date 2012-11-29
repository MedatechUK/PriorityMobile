Imports ViewControl
Imports System.Xml

Public Class ctrl_Warehouse
    Inherits ViewControl.iView

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

    Public Overrides Sub Bind()
        With Parts
            .DataSource = thisForm.TableData
        End With
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        Return MyBase.SubFormVisible(Name)
    End Function

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As System.Windows.Forms.ToolStrip)

        With ToolBar.Items
            .Add("", Image.FromFile("icons\FIND.BMP"), AddressOf hFind)
            .Add("", Image.FromFile("icons\PASTE.BMP"), AddressOf hCalc)
            .Item(.Count - 1).Enabled = CBool(String.Compare(scCurrView.XMLType, "part", True) = 0) And Not IsNothing(thisForm.TableData.Current)
        End With

    End Sub

    Public Overrides Sub SetNumber(ByVal MyValue As Integer)

        thisForm.TableData.Current("qty") = CStr(CInt(thisForm.TableData.Current("qty")) - MyValue)
        thisForm.TableData.EndEdit()

        Dim pe As XmlNode
        pe = xmlForms.FormData.Document.SelectSingleNode( _
            String.Format( _
                "{1}[name={0}{2}{0} and serial={0}{3}{0}]", _
                Chr(34), _
                scCurr.boundxPath, _
                thisForm.TableData.Current("name"), _
                thisForm.TableData.Current("serial") _
            ) _
        )
        If IsNothing(pe) Then
            pe = xmlForms.FormData.Document.CreateElement("part")
            With pe
                .AppendChild(xmlForms.FormData.Document.CreateElement("name"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.TableData.Current("name")
                .AppendChild(xmlForms.FormData.Document.CreateElement("des"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.TableData.Current("des")
                .AppendChild(xmlForms.FormData.Document.CreateElement("serial"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = thisForm.TableData.Current("serial")
                .AppendChild(xmlForms.FormData.Document.CreateElement("qty"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = MyValue.ToString
                .AppendChild(xmlForms.FormData.Document.CreateElement("fromstock"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = "Y"
                .AppendChild(xmlForms.FormData.Document.CreateElement("planned"))
                .ChildNodes(pe.ChildNodes.Count - 1).InnerText = "N"
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
        For Each V As iView In scCurr.Views
            V.Bind()
        Next
        scCurrView.RefreshData()
        scCurr.RefreshForm()

    End Sub

    Private Sub hCalc()
        thisForm.Calc(thisForm.TableData.Current("qty"))
    End Sub

    Private Sub hFind()
        Dim serial As String = InputBox("Part/Serial Number")
        Dim found As Boolean = False
        If serial.Length > 0 Then
            Dim dt As DataTable = thisForm.TableData.DataSource
            For Each row As DataRow In dt.Rows
                If String.Compare(row("serial"), serial, True) = 0 Or String.Compare(row("name"), serial, True) = 0 Then
                    found = True
                    With thisForm.TableData
                        .MoveFirst()
                        While Not (String.Compare(row("name"), thisForm.TableData.Current("name")) = 0 And String.Compare(row("serial"), thisForm.TableData.Current("serial")) = 0)
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
                        "Could not find serial number {0}.", _
                        serial _
                    ) _
                )
            End If
        End If
    End Sub

#End Region

End Class
