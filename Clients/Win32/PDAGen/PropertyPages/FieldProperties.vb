Imports System.ComponentModel
Imports System.Drawing.Design
Imports dataclasses
Imports bind

<DefaultPropertyAttribute("PRICOMPANY")> _
Public Class FieldProperties

    Private _it As InterfaceItem = Nothing

    Public Sub New(ByRef it As InterfaceItem, ByRef ds As Bind.oDataSet)
        _it = it
        staticRS.SetRS(ds)
        staticRS.SetFld(it)
    End Sub

    '''
    <CategoryAttribute("Design"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(FieldStylePropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The control that will be used to display this data.")> _
       Public Property FieldStyle() As String
        Get
            Select Case CInt(_it.FieldStyle)
                Case xfFieldStyle.xfText
                    clearList()
                    Return "Text"
                Case xfFieldStyle.xfCombo
                    Return "Combo"
                Case xfFieldStyle.xfDate
                    clearList()
                    Return "Date"
                Case xfFieldStyle.xfBool
                    clearList()
                    Return "Boolean"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal Value As String)
            Select Case Value
                Case "Text"
                    _it.FieldStyle = xfFieldStyle.xfText
                    clearList()
                Case "Combo"
                    _it.FieldStyle = xfFieldStyle.xfCombo
                Case "Date"
                    _it.FieldStyle = xfFieldStyle.xfDate
                    clearList()
                Case "Boolean"
                    _it.FieldStyle = xfFieldStyle.xfBool
                    clearList()
            End Select
        End Set
    End Property

    Private Sub clearList()

        With _it
            .ListFilter = ""
            .ListSource = ""
            .ListTextCol = ""
            .ListValueCol = ""
        End With
    End Sub
    '''
    <CategoryAttribute("Design"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(BooleanPropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("Determines if data held in ths field may be edited.")> _
    Public Property IsReadOnly() As Boolean
        Get
            Return CBool(_it.IsReadOnly)
        End Get
        Set(ByVal value As Boolean)
            _it.IsReadOnly = value
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
       DescriptionAttribute("Determines if this file is hidden or not.")> _
    Public Property Hidden() As Boolean
        Get
            Return CBool(_it.Hidden)
        End Get
        Set(ByVal value As Boolean)
            _it.Hidden = value
        End Set
    End Property

    '''
    <CategoryAttribute("Validation"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(BooleanPropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("Specifies if the field is mandatory.")> _
    Public Property Mandatory() As Boolean
        Get
            Return CBool(_it.Mandatory)
        End Get
        Set(ByVal value As Boolean)
            _it.Mandatory = value
        End Set
    End Property

    '''
    <CategoryAttribute("Validation"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The regular expression that will be used to validate data entry into this field.")> _
    Public Property REGEX() As String
        Get
            Return _it.REGEX
        End Get
        Set(ByVal value As String)
            _it.REGEX = value
        End Set
    End Property

    '''
    <CategoryAttribute("List"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(ListSourcePropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The table that provides data to this list.")> _
    Public Property ListSource() As String
        Get
            Return _it.ListSource
        End Get
        Set(ByVal value As String)
            _it.ListSource = value
        End Set
    End Property

    '''
    <CategoryAttribute("List"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(ListFieldPropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The value field of the list.")> _
    Public Property ListValueCol() As String
        Get
            Return _it.ListValueCol
        End Get
        Set(ByVal value As String)
            _it.ListValueCol = value
        End Set
    End Property

    '''
    <CategoryAttribute("List"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       EditorAttribute(GetType(ListFieldPropertyEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The data field of the list.")> _
    Public Property ListTextCol() As String
        Get
            Return _it.ListTextCol
        End Get
        Set(ByVal value As String)
            _it.ListTextCol = value
        End Set
    End Property

    '''
    <CategoryAttribute("List"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The filter for items displayed in this list.")> _
    Public Property ListFilter() As String
        Get
            Return _it.ListFilter
        End Get
        Set(ByVal value As String)
            _it.ListFilter = value
        End Set
    End Property
End Class

#Region "FieldStylePropertyEditor"
Public Class FieldStylePropertyEditor
    Inherits PropertyEditorBase

    Private WithEvents myListBox As New Windows.Forms.ListBox 'this is the control to be used in design time DropDown editor

    Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As System.Windows.Forms.Control
        With myListBox
            .BorderStyle = System.Windows.Forms.BorderStyle.None
            'Creating ListBox items... 
            'Note that as this is executed in design mode, performance is not important and there is no need to cache these items if they can change each time.
            .Items.Clear() 'clear previous items if any
            .Items.Add("Text")
            .Items.Add("Combo")
            .Items.Add("Date")
            .Items.Add("Boolean")
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
Public Class ListSourcePropertyEditor
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
            If staticRS.fld.FieldStyle = xfFieldStyle.xfCombo Then
                For Each si As SQLItem In sqlis.OriginalList
                    .Items.Add(si.TABLE)
                Next
                .SelectedIndex = myListBox.FindString(CurrentValue) 'Select current item based on CurrentValue of the property
                .Height = myListBox.PreferredHeight
            End If
        End With
        Return myListBox
    End Function

    Protected Overrides Function GetEditedValue(ByVal EditControl As System.Windows.Forms.Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
        If staticRS.fld.FieldStyle = xfFieldStyle.xfCombo Then
            Return myListBox.Text '.Substring(0, 2) 'return new value for property
        Else
            Return ""
        End If
    End Function

    Private Sub myTreeView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles myListBox.Click
        Me.CloseDropDownWindow() 'when user clicks on an item, the edit process is done.
    End Sub

End Class
#End Region

#Region "ListFieldPropertyEditor"
Public Class ListFieldPropertyEditor
    Inherits PropertyEditorBase

    Private WithEvents cols As oBind
    Public Sub New()
        cols = staticRS.rs.DataSet("ColumnItem")
    End Sub
    Private WithEvents myListBox As New Windows.Forms.ListBox 'this is the control to be used in design time DropDown editor

    Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As System.Windows.Forms.Control
        With myListBox
            .BorderStyle = System.Windows.Forms.BorderStyle.None
            'Creating ListBox items... 
            'Note that as this is executed in design mode, performance is not important and there is no need to cache these items if they can change each time.
            .Items.Clear() 'clear previous items if any
            If staticRS.fld.FieldStyle = xfFieldStyle.xfCombo Then
                For Each ci As ColumnItem In cols.OriginalList
                    If String.Compare(ci.TABLE, staticRS.fld.ListSource) = 0 Then
                        .Items.Add(ci.COLNAME)
                    End If
                Next
                .SelectedIndex = myListBox.FindString(CurrentValue) 'Select current item based on CurrentValue of the property
            End If
            .Height = myListBox.PreferredHeight
        End With
        Return myListBox
    End Function

    Protected Overrides Function GetEditedValue(ByVal EditControl As System.Windows.Forms.Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
        If staticRS.fld.FieldStyle = xfFieldStyle.xfCombo Then
            Return myListBox.Text '.Substring(0, 2) 'return new value for property
        Else
            Return ""
        End If
    End Function

    Private Sub myTreeView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles myListBox.Click
        Me.CloseDropDownWindow() 'when user clicks on an item, the edit process is done.
    End Sub

End Class
#End Region



