Imports PriorityMobile
Imports System.Xml
Imports System.Data

Public Class ctrl_VanSales
    Inherits iView

    Private ReadOnly Property whCurr() As xForm
        Get
            Return TopForm("Warehouse").CurrentForm
        End Get
    End Property

    Private ReadOnly Property whCurrView() As iView
        Get
            Return whCurr.Views(whCurr.CurrentView)
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
                .AddColumn("partname", "Part", 90)
                .AddColumn("partdes", "Description", 250)
                .AddColumn("qty", "Qty", 80)                
            End With
        End With

    End Sub

#End Region

#Region "Overides Base Properties"

    Public Overrides ReadOnly Property ButtomImage() As String
        Get
            Return "actual.BMP"
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
                "partname <> 'dummy'" _
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
        ListSort1.Focus()

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
            .Add(AddressOf hCalc, "edit.BMP", (ListSort1.SelectedIndex <> -1))
            .Add(AddressOf hDeleteActual, "delete.BMP", (ListSort1.SelectedIndex <> -1))
        End With
    End Sub

    Private Sub hFind()
        Dim part As String = InputBox("Part Number")
        Dim found As Boolean = False
        If part.Length > 0 Then
            Dim dt As DataTable = thisForm.TableData.DataSource
            For Each row As DataRow In dt.Rows
                If String.Compare(row("partname"), part, True) = 0 And (CDbl(row("qty")) > CDbl(row("rcvdqty"))) Then
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

    Public Overrides Sub SetNumber(ByVal MyValue As Integer)

        With thisForm

            Dim whNode As XmlNode = .FormData.SelectSingleNode( _
                String.Format("pdadata/warehouse/part[partname={0}{1}{0}]", _
                        Chr(34), _
                        .CurrentRow("partname") _
                    ) _
                )
            If Not IsNothing(whNode) Then
                Dim DIF As Integer = MyValue - CInt(.CurrentRow("qty"))
                whNode.SelectSingleNode("qty").InnerText = CStr(CInt(whNode.SelectSingleNode("qty").InnerText) - DIF)
                whCurr.TableData.EndEdit()
                .Save()
            End If

            .CurrentRow("qty") = MyValue
            .TableData.EndEdit()
            .Bind()
            For Each V As iView In .Views
                V.Bind()
            Next

            .Views(.CurrentView).RefreshData()
            .RefreshForm()

            whCurr.Bind()
            With whCurrView
                .RefreshData()
                For Each V As iView In .thisForm.Views
                    V.Bind()
                Next
            End With

        End With

    End Sub

    Private Sub hCalc()

        With thisForm
            Dim whNode As XmlNode = .FormData.SelectSingleNode( _
                String.Format("pdadata/warehouse/part[partname={0}{1}{0}]", _
                          Chr(34), _
                        .CurrentRow("partname") _
                    ) _
                )
            If Not IsNothing(whNode) Then
                thisForm.Calc(CInt(thisForm.CurrentRow("qty")) + CInt(whNode.SelectSingleNode("qty").InnerText))
            Else
                thisForm.Calc(thisForm.CurrentRow("qty"))
            End If
        End With

    End Sub

    Private Sub hDeleteActual()

        With thisForm
            If MsgBox(String.Format("This will remove part {0} from the delivery. Are you sure?", thisForm.CurrentRow("partname")), MsgBoxStyle.OkCancel, "Confirm?") = MsgBoxResult.Cancel Then Exit Sub
            Dim whNode As XmlNode = .FormData.SelectSingleNode( _
                String.Format("pdadata/warehouse/part[partname={0}{1}{0}]", _
                        Chr(34), _
                        .CurrentRow("partname") _
                    ) _
                )
            If Not IsNothing(whNode) Then
                whNode.SelectSingleNode("qty").InnerText = CStr(CInt(whNode.SelectSingleNode("qty").InnerText) + CInt(.CurrentRow("qty")))
                .TableData.EndEdit()
                .Save()
            End If            
        End With

        Dim removenode As XmlNode = xmlForms.FormData.Document.SelectSingleNode(myxPath)
        removenode.ParentNode.RemoveChild(removenode)
        thisForm.Save()
        RefreshData()

        With Me
            .RefreshData()
            For Each V As iView In .thisForm.Views
                V.Bind()
            Next
        End With

        whCurr.Bind()
        With whCurrView
            .RefreshData()
            For Each V As iView In .thisForm.Views
                V.Bind()
            Next
        End With

    End Sub

#End Region

    Private ReadOnly Property myxPath()
        Get
            Dim path As String
            If IsNothing(thisForm.TableData.Current) Then Return Nothing
            path = String.Format("/vansales/part[partname={0}{1}{0}]", _
               Chr(34), _
               ListSort1.Value("partname", ListSort1.SelectedIndex) _
            )

            Dim p As xForm = thisForm.Parent
            Dim k As String
            Dim repeat As Boolean = True
            Do
                If Not IsNothing(p.thisNode.Attributes("key")) And Not IsNothing(p.TableData.Current) Then
                    k = parseKey(p)
                Else
                    k = ""
                End If
                path = String.Format("{0}{1}{2}", p.xPath, k, path)
                If Not IsNothing(p.Parent) Then
                    p = p.Parent
                Else
                    repeat = False
                End If
            Loop Until Not repeat
            Return path
        End Get
    End Property

    Private Function parseKey(ByVal p As xForm) As String

        Dim bstr As String = ""
        Dim keys() As String = Split(p.thisNode.Attributes("key").Value, ",")

        For i As Integer = 0 To keys.Count - 1
            bstr += String.Format( _
                "{1}={0}{2}{0}", _
                Chr(34), _
                keys(i), _
                p.CurrentRow(keys(i)) _
            )
            If i < keys.Count - 1 Then
                bstr += " and "
            End If
        Next

        Return String.Format( _
            "[{0}]", _
            bstr _
        )

    End Function

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
