Imports System.Drawing
Imports System.Xml

Public Class iView

#Region "Private variables"

    Private BoundList As New Dictionary(Of String, String)

#End Region

#Region "Public Properties"

    Private _thisform As xForm
    Public Property thisForm() As xForm
        Get
            Return _thisform
        End Get
        Set(ByVal value As xForm)
            _thisform = value
        End Set
    End Property

    Private _Bound As Boolean = False
    Public Property Bound() As Boolean
        Get
            Return _Bound
        End Get
        Set(ByVal value As Boolean)
            _Bound = value
        End Set
    End Property

    Private _IsBinding As Boolean = False
    Public Property IsBinding() As Boolean
        Get
            Return _IsBinding
        End Get
        Set(ByVal value As Boolean)
            _IsBinding = value
        End Set
    End Property

    Public ReadOnly Property TopForm() As Dictionary(Of String, TopLevelForm)
        Get
            Return xmlForms.TopForm
        End Get
    End Property

#Region "Overridable Properties"

    Public Overridable ReadOnly Property Selected() As String
        Get
            If IsNothing(thisForm.TableData.Current) Then Return Nothing
            If IsNothing(thisForm.KeyColumn) Then Return Nothing
            Return thisForm.TableData.Current(thisForm.KeyColumn)
        End Get
    End Property

    Public Overridable ReadOnly Property ButtomImage() As Image
        Get
            Try
                Return Image.FromFile("icons\COPY.BMP") 'sPath.Substring(0, nIndex) & "\
            Catch
                Return Nothing
            End Try
        End Get
    End Property

    Public Overridable Function ValidColumn(ByVal ColumnName As String, ByVal ProposedValue As String) As Boolean
        Return True
    End Function

#End Region

#End Region

#Region "public Methods"

    Public Sub SetForm(ByRef ThisForm As xForm)
        _thisform = ThisForm
    End Sub

    Public Function XMLType() As String
        Try
            Return xmlForms.FormData.Document.SelectSingleNode(thisForm.thisxPath).Name
        Catch
            Return ""
        End Try
    End Function

    Public Sub RefreshData()
        With thisForm
            .TableData.ResetBindings(True)
            .Bind()
        End With
    End Sub

#Region "List Binding"

    Public Sub ListBind(ByRef List As ComboBox, ByVal BoundColumn As String)
        Static recurs As Boolean
        If Not recurs Then
            recurs = True
            With List
                RemoveHandler .SelectedIndexChanged, AddressOf SelectedIndexChanged
                AddHandler .SelectedIndexChanged, AddressOf SelectedIndexChanged
                If Not IsNothing(thisForm.TableData.Current.item(.DataBindings(0).BindingMemberInfo.BindingField)) Then
                    If thisForm.TableData.Current.item(.DataBindings(0).BindingMemberInfo.BindingField).Length > 0 Then
                        .DataSource = thisForm.LookUp.BindSource(BoundColumn, thisForm.TableData.Current.item(List.DataBindings(0).BindingMemberInfo.BindingMember))
                    Else
                        .DataSource = thisForm.LookUp.BindSource(BoundColumn, "*")
                    End If
                    .DisplayMember = "value"
                    .ValueMember = "name"
                End If
                If Not BoundList.ContainsKey(.Name) Then
                    BoundList.Add(.Name, BoundColumn)
                Else
                    BoundList(.Name) = BoundColumn
                End If
            End With
            recurs = False
        End If
    End Sub

    Private Sub SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Static recurs As Boolean
        If Not recurs Then
            recurs = True
            Dim List As ComboBox = sender
            If BoundList.Keys.Contains(List.Name) Then
                thisForm.TableData.EndEdit()
                ListBind(sender, BoundList(List.Name))
            End If
            recurs = False
        End If
    End Sub

#End Region

#End Region

#Region "Overridable Subs"

    Public Overridable Sub Bind()
        MsgBox(String.Format("Form {0} has views that do not override the Bind Method.", thisForm.FormName))
    End Sub

    Public Overridable Sub CurrentChanged()

    End Sub

    Public Overridable Function SubFormVisible(ByVal Name As String) As Boolean
        Return True
    End Function

    Public Overridable Sub DirectActivations(ByRef ToolBar As ToolStrip)

    End Sub

    Public Overridable Sub ViewChanged()

    End Sub

    Public Overridable Sub SetFocus()

    End Sub

    Public Overridable Sub RowUpdated(ByVal Column As String, ByVal NewValue As String)

    End Sub

    Public Overridable Sub FormClosing()

    End Sub

    Public Overridable Sub SetNumber(ByVal MyValue As Integer)

    End Sub

#End Region

End Class
