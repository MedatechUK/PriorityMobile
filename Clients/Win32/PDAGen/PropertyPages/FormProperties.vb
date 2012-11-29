Imports System.ComponentModel
Imports System.Drawing.Design
Imports dataclasses
Imports Bind

'''
<DefaultPropertyAttribute("PRICOMPANY")> _
Public Class FormProperties

    Private _it As FormItem = Nothing

    Public Sub New(ByRef it As FormItem, ByRef ds As Bind.oDataSet)
        _it = it
        staticRS.SetRS(ds)
        staticRS.SetFrm(it)
    End Sub
    <CategoryAttribute("Design"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(ViewStylePropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The style of this form.")> _
       Public Property ViewStyle() As String
        Get
            Select Case CInt(_it.DefaultView)
                Case xfView.xfForm
                    Return "Form"
                Case xfView.xfTable
                    Return "Table"
                Case xfView.xfHtml
                    Return "HTML"
                Case xfView.xfSignature
                    Return "Signature"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal Value As String)
            Select Case Value
                Case "Form"
                    _it.DefaultView = xfView.xfForm
                Case "Table"
                    _it.DefaultView = xfView.xfTable
                Case "HTML"
                    _it.DefaultView = xfView.xfHtml
                Case "Signature"
                    _it.DefaultView = xfView.xfSignature
            End Select

        End Set
    End Property

    <CategoryAttribute("Data"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   EditorAttribute(GetType(fListSourcePropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
   DescriptionAttribute("The table that provides data to this form.")> _
Public Property SQLFrom() As String
        Get
            Return _it.SQLFrom
        End Get
        Set(ByVal value As String)
            _it.SQLFrom = value
        End Set
    End Property

    '''
    <CategoryAttribute("Design"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(BooleanPropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("Specifies if the field is mandatory.")> _
    Public Property IsReadOnly() As Boolean
        Get
            Return CBool(_it.IsReadOnly)
        End Get
        Set(ByVal value As Boolean)
            _it.IsReadOnly = value
        End Set
    End Property

    <CategoryAttribute("List"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The filter for items displayed in this list.")> _
Public Property Filter() As String
        Get
            Return _it.Filter
        End Get
        Set(ByVal value As String)
            _it.Filter = value
        End Set
    End Property

End Class

#Region "FieldStylePropertyEditor"

Public Class ViewStylePropertyEditor
    Inherits PropertyEditorBase

    Private WithEvents myListBox As New Windows.Forms.ListBox 'this is the control to be used in design time DropDown editor

    Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As System.Windows.Forms.Control
        With myListBox
            .BorderStyle = System.Windows.Forms.BorderStyle.None
            'Creating ListBox items... 
            'Note that as this is executed in design mode, performance is not important and there is no need to cache these items if they can change each time.
            .Items.Clear() 'clear previous items if any
            .Items.Add("Form")
            .Items.Add("Table")
            .Items.Add("HTML")
            .Items.Add("Signature")
            .SelectedIndex = myListBox.FindString(CurrentValue) 'Select current item based on CurrentValue of the property
            .Height = myListBox.PreferredHeight
        End With
        Return myListBox
    End Function

    Protected Overrides Function GetEditedValue(ByVal EditControl As System.Windows.Forms.Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
        Return myListBox.Text '.Substring(0, 2) 'return new value for property
    End Function

    Private Sub myTreeView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles myListBox.Click
        Me.CloseDropDownWindow() 'when user clicks on an item, the edit process is done.
    End Sub

End Class
#End Region

#Region "ListSourcePropertyEditor"
Public Class fListSourcePropertyEditor
    Inherits PropertyEditorBase
    Private WithEvents sqlis As oBind

    Public Sub New()
        sqlis = staticRS.rs.DataSet("SQLItem")
    End Sub
    Private WithEvents myListBox As New Windows.Forms.ListBox 'this is the control to be used in design time DropDown editor

    Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As System.Windows.Forms.Control
        With myListBox
            .BorderStyle = System.Windows.Forms.BorderStyle.None
            'Creating ListBox items... 
            'Note that as this is executed in design mode, performance is not important and there is no need to cache these items if they can change each time.
            .Items.Clear() 'clear previous items if any

            For Each si As SQLItem In sqlis.OriginalList
                .Items.Add(si.TABLE)
            Next
            .SelectedIndex = myListBox.FindString(CurrentValue) 'Select current item based on CurrentValue of the property
            .Height = myListBox.PreferredHeight

        End With
        Return myListBox
    End Function

    Protected Overrides Function GetEditedValue(ByVal EditControl As System.Windows.Forms.Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
Return myListBox.Text
    End Function

    Private Sub myTreeView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles myListBox.Click
        Me.CloseDropDownWindow() 'when user clicks on an item, the edit process is done.
    End Sub

End Class
#End Region


