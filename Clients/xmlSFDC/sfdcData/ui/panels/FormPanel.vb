Imports System.Windows.Forms

Public Class FormPanel
    Inherits iFormPanel

    Private _ParentForm As iForm
    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _ParentForm
        End Get
    End Property

    Private _FormView As New FormView()
    Public Property FormView() As FormView
        Get
            Return _FormView
        End Get
        Set(ByVal value As FormView)
            _FormView = value
        End Set
    End Property

    Private _TableView As New TableView
    Public Property TableView() As TableView
        Get
            Return _TableView
        End Get
        Set(ByVal value As TableView)
            _TableView = value
        End Set
    End Property

    Private _FormButtons As New iFrmButtons()
    Public Property FormButtons() As iFrmButtons
        Get
            Return _FormButtons
        End Get
        Set(ByVal value As iFrmButtons)
            _FormButtons = value
        End Set
    End Property

    Sub New()
        With Me.Controls
            .Add(Me.FormView)
            .Item(.Count - 1).Dock = DockStyle.Top
            .Add(Me.TableView)
            .Item(.Count - 1).Dock = DockStyle.Bottom
            .Add(Me.FormButtons)
        End With

    End Sub

    Public Sub Load(ByRef thisForm As iForm, ByRef intf As PrioritySFDC.cInterface)
        _ParentForm = thisForm
        With Me
            With .FormView
                .Load(Me, intf.Form)
            End With
            With .TableView
                .Load(Me, intf.Table)
            End With
            With .FormButtons
                .Load(Me)
            End With
        End With
        iForm_Resize(Me, New System.EventArgs)

    End Sub

    Private Sub FormPanel_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        'MyBase.Focus()
        For Each uiCol As uiColumn In VisibleColumns()
            If uiCol.Selected Then
                uiCol.Parent.Parent.Focus()
                Exit Sub
            End If
        Next
        FormView.ViewForm.NextControl()
    End Sub

    Private Sub iForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Try
            With Me
                With .FormView
                    If .ViewForm.PanelHeight > (Me.Height / 2) Then
                        .Height = Me.Height / 2
                    Else
                        .Height = .ViewForm.PanelHeight
                    End If
                End With
                With .FormButtons
                    .Top = Me.FormView.Height
                    .Left = 0
                    .Width = Me.Width
                End With
                .TableView.Height = Me.Height - (.FormButtons.Top + .FormButtons.Height)
            End With
        Catch
        End Try
    End Sub

    Public Function VisibleColumns() As List(Of uiColumn)
        Dim ret As New List(Of uiColumn)
        Try
            For Each uiCol As uiColumn In ParentForm.ViewMain.FormView.ViewForm.Controls
                AddColRef(ret, uiCol)
            Next
            If ParentForm.ViewMain.TableView.TableView = TableView.eTableView.vForm Then
                For Each uiCol As uiColumn In ParentForm.ViewMain.TableView.ViewForm.Controls
                    AddColRef(ret, uiCol)
                Next
            End If
        Catch
        End Try
        Return ret
    End Function

    Private Sub AddColRef(ByRef List As List(Of uiColumn), ByRef uiCol As uiColumn)
        List.Add(uiCol)
    End Sub


End Class
