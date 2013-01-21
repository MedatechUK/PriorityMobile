Imports System.Xml

Public Class ctrlBase

    Private _thisForm As iForm
    Public ReadOnly Property thisForm() As iForm
        Get
            Return _thisForm
        End Get
    End Property

    Private _Columns As New List(Of CtrlColumn)
    Public Property Columns() As List(Of CtrlColumn)
        Get
            Return _Columns
        End Get
        Set(ByVal value As List(Of CtrlColumn))
            _Columns = value
        End Set
    End Property

    Public Sub Load(ByRef Sender As iForm, ByRef ContainerNode As XmlNode)
        _thisForm = Sender
        For Each ColumnNode As XmlNode In ContainerNode.SelectNodes("column")
            Columns.Add(New CtrlColumn(Sender, ColumnNode))
        Next        
    End Sub

    Public Sub DrawForm()
        Dim h As Integer = 0
        Dim f As CtrlColumn = Nothing
        With formContainer.Controls
            .Clear()
            For Each ctl As CtrlColumn In Columns
                If ctl.ColumnVisible Then
                    With ctl
                        .Height = thisForm.ControlHeight
                        .Width = Me.Width
                        .Top = h
                        .SetContainer(formContainer)
                        If IsNothing(f) And ctl.ColumnEnabled Then
                            f = ctl
                        End If
                    End With
                    h += thisForm.ControlHeight
                    .Add(ctl)
                End If
            Next
        End With
        If Not IsNothing(f) Then f.ActivateControl()
    End Sub

    Public Sub ControlLostFocus(ByRef sender As CtrlColumn)
        With Me.formContainer.Controls
            Dim current As Integer = .GetChildIndex(sender)
            Select Case sender.ExitDirection
                Case tExitDirection.Up
                    For i As Integer = current - 1 To 0 Step -1                        
                        If ColumnIndex(.Item(i)).ActivateControl Then Exit Select
                    Next
                    For i As Integer = .Count - 1 To current Step -1
                        If ColumnIndex(.Item(i)).ActivateControl Then Exit Select
                    Next
                Case tExitDirection.Down
                    For i As Integer = current + 1 To Me.Controls.Count - 1
                        If ColumnIndex(.Item(i)).ActivateControl Then Exit Select
                    Next
                    For i As Integer = 0 To current
                        If ColumnIndex(.Item(i)).ActivateControl Then Exit Select
                    Next
                Case tExitDirection.Selected

            End Select
        End With
    End Sub

    Private Function ColumnIndex(ByRef Ctrl As Control) As CtrlColumn
        Return Ctrl
    End Function

#Region "Overridable Methods"

    Public Overridable Sub Draw()

    End Sub

    Public Overridable Function formContainer() As Control
        Throw New Exception("Form container is not overridden in the derived class.")
    End Function

#End Region

End Class