Imports ViewControl

Public Class ctrl_Parts_Planned
    Inherits ViewControl.iView

#Region "Overides Base Properties"

    Public Overrides ReadOnly Property ButtomImage() As System.Drawing.Image
        Get
            Return Image.FromFile("icons\plan.bmp")
        End Get
    End Property

#End Region

    Public Overrides Sub Bind()

        Dim view As New DataView
        With view
            .Table = thisForm.TableData.DataSource
            .RowFilter = "planned = 'Y'"
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
