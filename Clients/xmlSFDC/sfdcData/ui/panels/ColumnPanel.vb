Imports System.Windows.Forms

Public Class ColumnPanel
    Inherits iFormPanel

#Region "Initialisation and finalisation"

    Public Sub New(ByRef Parent As iFormChild, ByRef Columns As cColumns)
        _Parent = Parent
        _Columns = Columns

        With Me
            .BackColor = Parent.BackColor
            With .Controls
                For Each col As cColumn In _Columns.Values
                    If col.Visible Then
                        .Add(New uiColumn(Me, col))
                        With .Item(.Count - 1)
                            .Dock = DockStyle.Top
                            .TabStop = Not (col.isReadOnly)
                            .Enabled = Not (col.isReadOnly)
                        End With                        
                    End If
                    AddHandler col.SetData, AddressOf hSetData
                Next

                Dim i As Integer = Me.Controls.Count - 1
                For Each control In Me.Controls
                    .SetChildIndex(control, i)
                    i -= 1
                Next

                .Item(.Count - 1).Focus()

            End With
        End With
    End Sub

#End Region

#Region "Inheritance"

    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return Parent.ParentForm
        End Get
    End Property

    Private _Parent As iFormChild
    Public Overloads ReadOnly Property Parent() As iFormChild
        Get
            Return _Parent
        End Get
    End Property

#End Region

#Region "Public Properties"

    Private _Columns As cColumns
    Public Property Columns() As cColumns
        Get
            Return _Columns
        End Get
        Set(ByVal value As cColumns)
            _Columns = value
        End Set
    End Property

    Public ReadOnly Property PanelHeight() As Integer
        Get
            Dim i As Integer = 0
            For Each ctrl As Control In Me.Controls
                If ctrl.Top + ctrl.Height > i Then
                    i = ctrl.Top + ctrl.Height
                End If
            Next
            Return i
        End Get
    End Property

    Private _HasMandatory As Boolean = False
    Public ReadOnly Property HasMandatory() As Boolean
        Get
            Return _HasMandatory
        End Get
    End Property
#End Region

#Region "Control Focus"

    Public Sub NextControl(Optional ByVal NextUnfilled As Boolean = True)
        MoveFocus(True, NextUnfilled)
    End Sub

    Public Sub PreviousControl(Optional ByVal NextUnfilled As Boolean = True)
        MoveFocus(False, NextUnfilled)
    End Sub

    Public Sub LastControl()
        With Me.Controls
            For i As Integer = 0 To .Count - 1
                If Not TryCast(.Item(i), uiColumn).thisColumn.isReadOnly Then
                    TryCast(.Item(i), uiColumn).Focus()
                    Exit For
                End If
            Next            
        End With
    End Sub

    Public Sub FirstControl()
        With Me.Controls
            For i As Integer = .Count - 1 To 0 Step -1
                If Not TryCast(.Item(i), uiColumn).thisColumn.isReadOnly Then
                    TryCast(.Item(i), uiColumn).Focus()
                    Exit For
                End If
            Next
        End With
        'Select Case _Parent.Container.NodeType.ToLower
        '    Case "form"
        '        _Parent.ParentForm.ViewMain.TableView.ViewTable.thisTable.Focus()
        '    Case "table"
        'End Select
    End Sub

    Private Sub MoveFocus(ByVal Forward As Boolean, ByVal NextUnfilled As Boolean)

        Dim repeat As Boolean = NextUnfilled
        Dim Controlindex As Integer
        Dim NoControl As Boolean = False

        Try
            If IsNothing(FocusedControl) Then
                Forward = True
                LastControl()
            End If
        Catch
        Finally
            With FocusedControl
                Controlindex = .TabIndex
                .Deselect()
            End With
        End Try

        'SelectNextControl(FocusedControl, Forward, True, False, True)
        ShiftFocus(FocusedControl, Forward)

        While repeat
            With FocusedControl()
                Select Case .ColStyle
                    Case eColStyle.colReadOnly
                        'SelectNextControl(FocusedControl, Forward, True, False, True)
                        ShiftFocus(FocusedControl, Forward)
                    Case Else
                        If .thisColumn.Value.Length > 0 And NextUnfilled Then
                            'SelectNextControl(FocusedControl, Forward, True, False, True)
                            .Deselect()
                            ShiftFocus(FocusedControl, Forward)
                        Else
                            repeat = False
                        End If
                End Select
                If Controlindex = .TabIndex Then
                    NoControl = True
                    repeat = False
                End If
            End With
        End While

        If Not (NoControl) Then
            With FocusedControl
                .ColStyle = eColStyle.colSelected
                .Selected = True
            End With
        Else
            If Not IsNothing(TryCast(_Parent, FormView)) Then
                ParentForm.ViewMain.TableView.Focus()
            ElseIf Not IsNothing(TryCast(_Parent, TableView)) Then
                FocusedControl.Deselect()                
            End If
        End If

    End Sub

    Public ReadOnly Property FocusedControl() As uiColumn
        Get
            For Each ctrl As uiColumn In Me.Controls
                If ctrl.Label.Focused Then
                    Return ctrl
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public Sub Defocus(ByRef excep As Exception)
        For Each ctrl As uiColumn In Me.Controls
            If ctrl.Selected Then
                ctrl.ProcessBuffer(excep)
                If Not IsNothing(excep) Then
                    ctrl.Focus()
                    Exit For
                Else
                    ctrl.Deselect()
                End If
            End If
        Next
    End Sub

    Private Sub ColumnPanel_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        'MyBase.Focus()
        For Each uiCol As uiColumn In Me.Controls
            If uiCol.Selected Then
                uiCol.Focus()
                Exit Sub
            End If
        Next

        'LastControl()
        'NextControl(True)

    End Sub

    Public Sub SelectColumn(ByVal Name As String)
        For Each uicol As uiColumn In Controls
            If String.Compare(uicol.thisColumn.Name, Name, True) = 0 Then
                uicol.Focus()
            End If
        Next
    End Sub

    Private Function Indicies() As List(Of Integer)
        Dim l As New List(Of Integer)
        For Each c As uiColumn In Me.Controls
            If Not c.thisColumn.isReadOnly And c.Enabled Then
                l.Add(c.TabIndex)
            End If
        Next
        l.Sort()
        Return l
    End Function

    Private Sub ShiftFocus(ByRef ctrl As Control, ByVal Forward As Boolean)

        Dim sel As Integer
        Dim ind As List(Of Integer) = Indicies()

        If Forward Then
            If ind.Max = ctrl.TabIndex Then
                sel = ind.Min
            Else
                For Each i As Integer In ind
                    If i > ctrl.TabIndex Then
                        sel = i
                        Exit For
                    End If
                Next
            End If
        Else
            Dim last As Integer = ind.Min
            If ind.Min = ctrl.TabIndex Then
                sel = ind.Max
            Else
                For Each i As Integer In ind
                    If i = ctrl.TabIndex Then
                        sel = last
                        Exit For
                    Else
                        last = i
                    End If
                Next
            End If
        End If

        For Each c As Control In Me.Controls
            If c.TabIndex = sel Then
                c.Focus()
                Exit For
            End If
        Next

    End Sub

#End Region

#Region "public Methods"

    Public Sub Clear()
        For Each col As cColumn In Me.Columns.Values
            With col
                .NoPostField = True
                .Value = ""
                If Not IsNothing(.uiCol) Then .uiCol.Deselect()
                .NoPostField = False
            End With
        Next
    End Sub

#End Region

    Private Sub hSetData()
        For Each col As cColumn In _Columns.Values
            If col.Mandatory And col.Value.Length = 0 Then
                _HasMandatory = False
                Exit Sub
            End If
            _HasMandatory = True
        Next
        ParentForm.ViewMain.FormButtons.RefreshButtons()
    End Sub

End Class