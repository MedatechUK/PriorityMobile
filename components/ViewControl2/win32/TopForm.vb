Imports System.Xml

Public Class TopLevelForm

#Region "Private Variables"

    Private _openForms As New List(Of xForm)

#End Region

#Region "initialisation and finalisation"

    Public Sub New(ByRef ThisForm As xForm)
        _TopForm = ThisForm
        _openForms.Add(_TopForm)
    End Sub

#End Region

#Region "Public Properties"

    Private _TopForm As xForm
    Public Property TopForm() As xForm
        Get
            Return _TopForm
        End Get
        Set(ByVal value As xForm)
            _TopForm = value
        End Set
    End Property

    Public ReadOnly Property CurrentForm() As xForm
        Get
            Return _openForms(_openForms.Count - 1)
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub OpenForm(ByVal FormName As String)
        If CurrentForm.SubForms.Keys.Contains(FormName) Then
            _openForms.Add(CurrentForm.SubForms(FormName))
            With CurrentForm
                .Bind()
                .Views(.CurrentView).CurrentChanged()
                CurrentForm.RefreshDirectActivations()
            End With            
        Else
            MsgBox( _
                String.Format( _
                    "Form {0} does not contain a subform called {1}", _
                    CurrentForm.FormName, _
                    FormName _
                ), _
            MsgBoxStyle.OkOnly)
        End If
    End Sub

    Public Sub CloseForm()
        With CurrentForm
            If Not IsNothing(.Parent) Then
                .FormClosing()
                .Views(.CurrentView).thisForm.TableData.EndEdit()
                _openForms.RemoveAt(_openForms.Count - 1)
                .RefreshDirectActivations()
                .RefreshSubForms()
            End If
        End With
    End Sub

#End Region

End Class