Imports System.Xml
Imports PriorityMobile

Public Class ctrl_DeliveredItems
    Inherits iView

#Region "Initialisation and Finalisation"

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With Me
            With ListSort1
                .FormLabel = "Delivered Items"
                .Sort = "name"
                .AddColumn("ordi", "ordi", 0, True)
                .AddColumn("name", "Part", 130)
                .AddColumn("des", "Description", 280)
                .AddColumn("cquant", "qty", 65)
                .AddColumn("expirydate", "Expires", 150, , eColumnFormat.fmt_Date)
                .AddColumn("weight", "Weight/kg", 100)
            End With
        End With

    End Sub

#End Region

#Region "Overides Base Properties"

    'Public Overrides ReadOnly Property ButtomImage() As String
    '    Get
    '        Return "calendar.bmp"
    '    End Get
    'End Property

    Public Overrides ReadOnly Property Selected() As String
        Get
            Return ListSort1.Selected
        End Get
    End Property

#End Region

#Region "Overrides base Methods"

    Public Overrides Sub FormClosing()
        thisForm.CurrentView = 0
        thisForm.Save()
        MyBase.FormClosing()
    End Sub

    Public Overrides Sub Bind() Handles ListSort1.Bind

        IsBinding = True
        Dim dr() As Data.DataRow = Nothing
        Dim query As String = String.Format( _
                "{0} <> '0' and cquant <> 0", _
                ListSort1.Keys(0) _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                If .RowSelected(r, thisForm.CurrentRow) Then
                    .Items(.Items.Count - 1).Selected = True
                    .Items(.Items.Count - 1).Focused = True
                Else
                    .Items(.Items.Count - 1).Selected = False
                    .Items(.Items.Count - 1).Focused = False
                End If
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

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        With Me
            If IsNothing(.Selected) Then Return False
            If IsNothing(.thisForm.TableData.Current) Then Return False
            If IsNothing(ListSort1.Selected) Then Return False
            Select Case Name.ToUpper
                Case Else
                    Return True
            End Select
        End With
    End Function

#End Region

#Region "Local control Handlers"

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value(ListSort1.Keys(0), row)
            If Not String.Compare(Text, cur) = 0 Then
                .TableData.Position = .TableData.Find(ListSort1.Keys(0), cur)
                .RefreshSubForms()
                .RefreshDirectActivations()
            End If
        End With
    End Sub

    Private Sub hItemSelect() Handles ListSort1.ItemSelect
        With thisForm
            If Not IsNothing(.TableData.Current) Then
                .CurrentView += 1
                .RefreshForm()
            End If
        End With
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hUnDeliver, "delete.BMP", Not ListSort1.SelectedIndex = -1)
    End Sub

    Private Sub hUnDeliver()
        If MsgBox("This will remove the part from this delivery. Proceed?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Exit Sub
        With thisForm
            Dim ordi As String = .CurrentRow("ordi")
            Dim lot As XmlNode = .FormData.SelectSingleNode( _
                String.Format("pdadata/warehouse/parts/part[name='{0}']/lots/lot[name='{1}']", _
                    .CurrentRow("name"), _
                    ordi.Split("|")(1) _
                ) _
            )
            Dim DeliveryPart As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode( _
                String.Format( _
                    "part[ordi='{0}']", _
                    ordi.Split("|")(0) _
                ) _
            )
            Dim DeliveredPart As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode( _
                String.Format( _
                    "part[ordi='{0}|{1}']", _
                    ordi.Split("|")(0), _
                    ordi.Split("|")(1) _
                ) _
            )
            lot.SelectSingleNode("qty").InnerText = CDbl(lot.SelectSingleNode("qty").InnerText) + CDbl(.CurrentRow("cquant"))
            DeliveryPart.SelectSingleNode("tquant").InnerText = CDbl(DeliveryPart.SelectSingleNode("tquant").InnerText) + CDbl(.CurrentRow("cquant"))
            .FormData.SelectSingleNode(.boundxPath).ParentNode.RemoveChild(DeliveredPart)

            .Save()
            .Bind()
            For Each V As iView In thisForm.Views
                V.Bind()
            Next
            .RefreshForm()
        End With
    End Sub

#End Region

End Class
