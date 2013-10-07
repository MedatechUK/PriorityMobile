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

    Public ReadOnly Property Column() As Dictionary(Of String, String)
        Get
            Dim ret As New Dictionary(Of String, String)
            For Each c As cColumn In Columns.Values
                ret.Add(String.Format(":$.{0}", c.Name), c.Value)
            Next
            Return ret
        End Get
    End Property

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
        Dim ind As List(Of Integer)        
        ind = Indicies(True)
        If ind.Count = 0 Then            
            ind = Indicies(False)
        End If

        For Each c As uiColumn In Me.Controls
            If c.TabIndex = ind.Max Then
                c.Selected = True
                c.Focus()
                If c.thisColumn.Value.Length > 0 Then
                    c.ColStyle = eColStyle.colDeselected
                Else
                    c.ColStyle = eColStyle.colSelected
                End If
                Exit For
            End If
        Next
    End Sub

    Public Sub FirstControl()
        Dim ind As List(Of Integer)
        ind = Indicies(True)
        If ind.Count = 0 Then
            ind = Indicies(False)
        End If

        For Each c As uiColumn In Me.Controls
            If c.TabIndex = ind.Min Then
                c.Selected = True
                c.Focus()
                If c.thisColumn.Value.Length > 0 Then
                    c.ColStyle = eColStyle.colDeselected
                Else
                    c.ColStyle = eColStyle.colSelected
                End If
            Else
                c.Selected = False
                c.ColStyle = eColStyle.colDeselected
            End If
        Next
    End Sub

    Private Sub MoveFocus(ByVal Forward As Boolean, ByVal NextUnfilled As Boolean)

        'Dim repeat As Boolean = NextUnfilled
        Dim Controlindex As Integer
        Dim FirstIndex As Integer
        'Dim NoControl As Boolean = False

        Try
            If IsNothing(FocusedControl) Then
                Forward = True
                LastControl()
            End If
        Catch
        Finally
            With FocusedControl
                Controlindex = .TabIndex
                FirstIndex = .TabIndex
                .Selected = False
            End With
        End Try

        'SelectNextControl(FocusedControl, Forward, True, False, True)
        ShiftFocus(Controlindex, Forward, NextUnfilled)

        'While repeat
        '    With FocusedControl()
        '        If .thisColumn.Value.Length > 0 And NextUnfilled Then
        '            'SelectNextControl(FocusedControl, Forward, True, False, True)
        '            .Deselect()
        '            ShiftFocus(Controlindex, Forward)
        '        Else
        '            repeat = False
        '        End If
        '        If FirstIndex = .TabIndex Then
        '            NoControl = True
        '            repeat = False
        '        End If
        '    End With
        'End While

        If Not Controlindex = FirstIndex Then
            With FocusedControl
                .ColStyle = eColStyle.colSelected
                .Selected = True
            End With
        Else
            With FocusedControl
                .ColStyle = eColStyle.colDeselected
                .Selected = True
            End With

            If Not IsNothing(TryCast(_Parent, FormView)) Then
                ParentForm.ViewMain.TableView.Focus()
            ElseIf Not IsNothing(TryCast(_Parent, TableView)) Then
                'FocusedControl.Deselect()
            End If

        End If

    End Sub

    Public ReadOnly Property FocusedControl() As uiColumn
        Get
            For Each ctrl As uiColumn In Me.Controls
                If ctrl.Selected Then
                    Return ctrl
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public Sub Defocus(ByRef excep As Exception)
        excep = Nothing
        For Each ctrl As uiColumn In Me.Controls
            With ctrl
                If .Selected Then
                    If .HasScanBuffer Then
                        .ProcessBuffer(excep)
                        If Not IsNothing(excep) Then
                            .Focus()
                            Exit For
                        End If
                    End If
                    .Deselect()
                    .Selected = False
                End If
            End With
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
            Else
                uicol.Deselect()
                uicol.Selected = False
            End If
        Next
    End Sub

    Private Function Indicies(ByVal NextUnfilled As Boolean) As List(Of Integer)
        Dim l As New List(Of Integer)
        For Each c As uiColumn In Me.Controls

            Dim MissingDepend As Boolean = False
            For Each dep As cColumn In c.thisColumn.Depends
                If dep.Value.Length = 0 Then
                    MissingDepend = True
                    Exit For
                End If
            Next

            If Not MissingDepend And Not c.thisColumn.isReadOnly And c.Enabled Then
                If Not NextUnfilled Then
                    l.Add(c.TabIndex)
                Else
                    If c.thisColumn.Value.Length = 0 Then
                        l.Add(c.TabIndex)
                    End If
                End If
            End If

        Next

        l.Sort()
        Return l
    End Function

    Private Sub ShiftFocus(ByRef Controlindex As Integer, ByVal Forward As Boolean, Optional ByVal NextUnfilled As Boolean = False)

        Dim sel As Integer
        Dim ind As List(Of Integer) = Indicies(NextUnfilled)

        If ind.Count > 0 Then

            If Forward Then
                If ind.Max = Controlindex Then
                    sel = ind.Min
                Else
                    For Each i As Integer In ind
                        If i > Controlindex Then
                            sel = i
                            Exit For
                        End If
                    Next
                End If
            Else
                Dim last As Integer = ind.Min
                If ind.Min = Controlindex Then
                    sel = ind.Max
                Else
                    For Each i As Integer In ind
                        If i = Controlindex Then
                            sel = last
                            Exit For
                        Else
                            last = i
                        End If
                    Next
                End If
            End If

            Controlindex = sel

        Else
            sel = Controlindex
        End If

        For Each c As uiColumn In Me.Controls
            If c.TabIndex = sel Then
                c.Selected = True
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
                If Not IsNothing(.uiCol) Then
                    With .uiCol
                        .Deselect()
                        .Selected = False
                    End With
                End If
                .NoPostField = False
            End With
        Next
        'FirstControl()
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