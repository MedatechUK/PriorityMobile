Imports System.Windows.Forms

Public Class FormView
    Inherits iFormChild

#Region "Inheritance"

    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _Parent.ParentForm
        End Get
    End Property

    Private _Parent As FormPanel
    Public Overloads ReadOnly Property Parent() As FormPanel
        Get
            Return _Parent
        End Get
    End Property

#End Region

#Region "Public properties"

    Private _ViewForm As ColumnPanel
    Public Property ViewForm() As ColumnPanel
        Get
            Return _ViewForm
        End Get
        Set(ByVal value As ColumnPanel)
            _ViewForm = value
        End Set
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub Load(ByRef Parent As FormPanel, ByRef thisForm As cForm)

        _Parent = Parent
        _Container = thisForm

        ViewForm = New ColumnPanel(Me, thisForm.Columns)
        With Me.Controls
            .Add(ViewForm)
            ViewForm.Dock = DockStyle.Fill
        End With

        If Container.Triggers.Keys.Contains("PRE-INSERT") Then
            Container.Triggers("PRE-INSERT").Execute()
        End If

        With ViewForm
            .FirstControl()
            '.LastControl()
            '.NextControl(True)
        End With

    End Sub

#End Region

#Region "Event handlers"

    Private Sub FormView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        'MyBase.Focus()
        With _Parent.TableView
            If .TableView = TableView.eTableView.vForm Then
                Dim ex As New Exception
                .ViewForm.Defocus(ex)
                If Not IsNothing(ex) Then
                    MsgBox(ex.Message)
                Else
                    ViewForm.Focus()
                End If
            Else
                ViewForm.Focus()
            End If
        End With

    End Sub

#End Region

End Class
