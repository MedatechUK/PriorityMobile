Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web
Imports System.Xml

Public Class Repl_OrderForm
    Inherits repl_Base

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "Repl_OrderForm"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As System.Collections.Generic.List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Table", "orderform", AddressOf hOrderForm))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Button", "addformtobasket", AddressOf hAddFormToBasket))
                .Add(New ReplaceControl("System.Web.UI.WebControls.LinkButton", "saveorderform", AddressOf hSaveOrderForm))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Private Methods"

    Private Function Plural(ByVal number As Integer) As String
        If number <> 1 Then
            Return "es"
        Else
            Return ""
        End If
    End Function

#End Region

#Region "Delegate Methods"

    Public Sub hOrderForm(ByVal sender As Object, ByVal e As repl_Argument)

        Dim t As System.Web.UI.WebControls.Table = sender

        Dim th As New System.Web.UI.WebControls.TableFooterRow
        With th
            For i As Integer = 0 To 3
                .Cells.Add(New TableHeaderCell)
            Next
            .Cells(0).Text = "Part"
            .Cells(1).Text = "Description"
            .Cells(2).Text = "Price"
            .Cells(3).Text = "QTY"
        End With
        t.Rows.Add(th)

        Dim cnt As Integer = 0
        For Each k As String In xmlFunc.AllParts(cmsData.part).Keys
            Dim P As XmlNode = xmlFunc.Part(cmsData.part, k)
            If Not IsNothing(P) Then
                Dim r As New System.Web.UI.WebControls.TableRow
                r.ID = String.Format("Row_{0}", cnt.ToString)

                Dim ce1 As New TableCell

                Dim pp As XmlNode = xmlFunc.PartPage(cmsData.doc, k)
                If Not IsNothing(pp) Then
                    Dim h As New LinkButton()
                    h.ID = "/" & pp.Attributes("id").Value
                    h.Text = P.SelectSingleNode("PARTNAME").InnerText
                    AddHandler h.Click, AddressOf SaveForm
                    ce1.Controls.Add(h)
                Else
                    ce1.Text = P.SelectSingleNode("PARTNAME").InnerText
                End If

                Dim ce2 As New TableCell
                ce2.Text = P.SelectSingleNode("PARTDES").InnerText

                Dim bCount As Integer = BoxCount(P.SelectSingleNode("PARTNAME").InnerText)

                Dim ce3 As New TableCell
                Dim cur As XmlNode = xmlFunc.PartCurrency(P, ts.Basket.CURRENCY, HttpContext.Current.Profile("CUSTNAME"))
                If Not t.Page.IsPostBack Then
                    ce3.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, 1)) * bCount)
                Else
                    If IsNumeric(t.Page.Request("ctl00$" & k)) Then
                        Dim lq As Integer = CInt(t.Page.Request("ctl00$" & k))
                        If lq = 0 Then lq = 1
                        ce3.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, lq)) * bCount)
                    Else
                        ce3.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, 1)) * bCount)
                    End If

                End If

                Dim ce4 As New TableCell


                Select Case bCount
                    Case 1
                        Dim q As New System.Web.UI.WebControls.TextBox
                        With q
                            .ID = P.SelectSingleNode("PARTNAME").InnerText
                            .Width = 30
                            .Attributes.Add("onfocus", "this.select();")
                            .MaxLength = 4

                            If Not t.Page.IsPostBack Then
                                .Text = "0"
                                Try
                                    If t.Page.Session("ordform." & q.ID).length > 0 Then
                                        If IsNumeric(t.Page.Session("ordform." & q.ID)) Then
                                            If Not .Text = t.Page.Session("ordform." & q.ID) Then
                                                .Text = t.Page.Session("ordform." & q.ID)
                                            End If
                                        End If
                                    End If
                                Catch
                                End Try

                            End If
                        End With
                        ce4.Controls.Add(q)

                    Case Else
                        Dim bx As New System.Web.UI.WebControls.DropDownList
                        With bx
                            .ID = P.SelectSingleNode("PARTNAME").InnerText
                            For bq As Integer = 0 To 10
                                bx.Items.Add(New ListItem(String.Format("{0} box{2} ({1})", bq.ToString, bq * bCount.ToString, Plural(CInt(bq.ToString))), bq * bCount))
                                Try
                                    .Items(.Items.Count - 1).Selected = CBool(CInt(bq * bCount) = CInt(t.Page.Session("ordform." & .ID)))
                                Catch
                                End Try
                            Next
                        End With
                        ce4.Controls.Add(bx)

                End Select

                Dim val As New RequiredFieldValidator
                With val
                    .ControlToValidate = P.SelectSingleNode("PARTNAME").InnerText
                    .Text = "*"
                    '.Type = ValidationDataType.Integer
                    '.MinimumValue = 0
                    '.MaximumValue = 9999
                End With
                ce4.Controls.Add(val)

                Dim val2 As New RegularExpressionValidator
                With val2
                    .ControlToValidate = P.SelectSingleNode("PARTNAME").InnerText
                    .Text = "*"
                    .ValidationExpression = "^[0-9]+$"
                    '.Type = ValidationDataType.Integer
                    '.MinimumValue = 0
                    '.MaximumValue = 9999
                End With
                ce4.Controls.Add(val2)

                r.Cells.Add(ce1)
                r.Cells.Add(ce2)
                r.Cells.Add(ce3)
                r.Cells.Add(ce4)
                If cnt Mod (2) = 1 Then r.CssClass = "odd"
                t.Rows.Add(r)
                cnt += 1
            End If
        Next

    End Sub

    Public Sub hAddFormToBasket(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As Button = sender
        AddHandler btn.Click, AddressOf AddFormToBasket
    End Sub

    Public Sub hSaveOrderForm(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As LinkButton = sender
        AddHandler btn.Click, AddressOf SaveForm
    End Sub

#End Region

#Region "Event Handlers"

    Public Sub SaveForm(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As LinkButton = sender
        With btn.Page
            Dim tab As Table = .Master.FindControl("orderform")
            For Each r As TableRow In tab.Rows
                For Each c As TableCell In r.Cells
                    For Each ct As Control In c.Controls
                        Select Case ct.ToString
                            Case "System.Web.UI.WebControls.TextBox"
                                Dim tb As TextBox = ct
                                If IsNumeric(tb.Text) Then
                                    If CInt(tb.Text) > 0 Then
                                        btn.Page.Session("ordform." & tb.ID) = tb.Text
                                    Else
                                        btn.Page.Session("ordform." & tb.ID) = Nothing
                                    End If
                                Else
                                    btn.Page.Session("ordform." & tb.ID) = Nothing
                                End If

                            Case "System.Web.UI.WebControls.DropDownList"
                                Dim tb As System.Web.UI.WebControls.DropDownList = ct
                                If IsNumeric(tb.SelectedValue) Then
                                    If CInt(tb.SelectedValue) > 0 Then
                                        .Session("ordform." & tb.ID) = tb.SelectedValue
                                    Else
                                        .Session("ordform." & tb.ID) = Nothing
                                    End If
                                Else
                                    .Session("ordform." & tb.ID) = Nothing
                                End If

                        End Select

                    Next
                Next
            Next
            If Left(btn.ID, 1) = "/" Then .Response.Redirect(btn.ID)

        End With
    End Sub

    Public Sub AddFormToBasket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = sender
        With btn
            Dim p As New cmsPage(.Page, HttpContext.Current, .Page.Server)
            With .Page
                For Each node As XmlNode In cmsData.doc.SelectNodes("/*[position()=1]/PARTS/PART[not(@DELIVERY)]")
                    Dim PartName As String = node.SelectSingleNode("PARTNAME").InnerText
                    Dim lq As Integer = CInt(p.FormDictionary(PartName))
                    If lq > 0 Then
                        Dim pp As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@part = '{0}']", p.FormDictionary(PartName)))
                        Dim h As String = ""
                        If Not IsNothing(pp) Then
                            h = "/" & pp.Attributes("id").Value
                        Else
                            h = "/" & p.PageNode.Attributes("id").Value
                        End If

                        ts.Basket.AddBasketItem(New BasketItem(PartName, lq, h, BoxCount(PartName).ToString))
                        .Session("ordform." & PartName) = Nothing
                    End If
                Next
                .Response.Redirect("basket.aspx")
            End With
        End With
    End Sub

#End Region

End Class
