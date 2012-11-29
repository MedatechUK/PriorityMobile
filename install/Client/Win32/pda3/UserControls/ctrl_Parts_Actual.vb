Imports ViewControl
Imports System.Xml

Public Class ctrl_Parts_Actual
    Inherits ViewControl.iView

    Private ReadOnly Property myxPath()
        Get
            Dim path As String
            If IsNothing(thisForm.TableData.Current) Then Return Nothing
            path = String.Format("/parts/part[name={0}{1}{0} and serial = {0}{2}{0}]", _
               Chr(34), _
               Parts.CurrentRow.Cells("name").Value, _
               Parts.CurrentRow.Cells("serial").Value _
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
                p.TableData.Current(keys(i)) _
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

#Region "Overides Base Properties"

    Public Overrides ReadOnly Property ButtomImage() As System.Drawing.Image
        Get
            Return Image.FromFile("icons\actual.BMP")
        End Get
    End Property

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As System.Windows.Forms.ToolStrip)
        With ToolBar.Items
            .Add("", Image.FromFile("icons\delete.BMP"), AddressOf hDeleteActual)
            .Item(.Count - 1).Enabled = Not IsNothing(Me.Parts.CurrentRow)
        End With
    End Sub

    Private Sub hDeleteActual()
        If String.Compare(thisForm.TableData.Current("fromstock"), "y", True) = 0 Then

        End If
        Dim removenode As xmlnode = xmlForms.FormData.Document.SelectSingleNode(myxPath)
        removenode.ParentNode.RemoveChild(removenode)
        thisForm.Save()
        RefreshData()
        Bind()
    End Sub

#End Region

    Public Overrides Sub Bind()

        Dim view As New DataView
        With view
            .Table = thisForm.TableData.DataSource
            .RowFilter = "planned <> 'Y'"
        End With        

        With Parts
            .DataSource = view
            With .Columns
                .Item(0).HeaderText = "Name"
                .Item(1).HeaderText = "Description"
                .Item(2).HeaderText = "Serial No"
                .Item(3).HeaderText = "QTY"
                .Item(4).Visible = False
                .Item(5).Visible = False
            End With
        End With

    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        Return MyBase.SubFormVisible(Name)
    End Function

    Private Sub ctrl_Parts_Actual_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Me.Parts.Height = Me.Height - (Label1.Height + 5)
    End Sub

End Class
