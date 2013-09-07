Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("PRICOMPANY")> _
Public Class SimpleProperties

    '''
    <CategoryAttribute("Priority"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The Priority environment that will be opened.")> _
       Public Property PRICOMPANY() As String
        Get
            Return My.Settings.PRICOMPANY
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.PRICOMPANY Then
                My.Settings.PRICOMPANY = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Priority"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The user name with which to open Priority.")> _
   Public Property PRIUSER() As String
        Get
            Return My.Settings.PRIUSER
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.PRIUSER Then
                My.Settings.PRIUSER = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Priority"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    TypeConverter(GetType(PasswordConverter)), _
    Editor(GetType(PasswordEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The password to use when opening Priority.")> _
    Public Property PRIPASSWORD() As String
        Get
            Return My.Settings.PRIPASSWORD
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.PRIPASSWORD Then
                My.Settings.PRIPASSWORD = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Priority"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    Editor(GetType(System.Windows.Forms.Design.FolderNameEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The path to the local Priority folder.")> _
    Public Property PRIORITYPATH() As String
        Get
            Return My.Settings.PRIORITYPATH
            Return Nothing
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.PRIORITYPATH Then
                My.Settings.PRIORITYPATH = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Database"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    DescriptionAttribute("The name or IP address of the SQL server/instance.")> _
    Public Property DATASOURCE() As String
        Get
            Return My.Settings.DATASOURCE
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.DATASOURCE Then
                My.Settings.DATASOURCE = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Database"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    DescriptionAttribute("The name of the Priority database in SQL.")> _
    Public Property INITIALCATALOG() As String
        Get
            Return My.Settings.INITIALCATALOG
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.INITIALCATALOG Then
                My.Settings.INITIALCATALOG = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Database"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    DescriptionAttribute("The username to use when opening the database.")> _
    Public Property USERID() As String
        Get
            Return My.Settings.USERID
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.USERID Then
                My.Settings.USERID = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Database"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    TypeConverter(GetType(PasswordConverter)), _
    Editor(GetType(DatabasePasswordEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The password to use when opening the database.")> _
    Public Property PASSWORD() As String
        Get
            Return My.Settings.PASSWORD
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.PASSWORD Then
                My.Settings.PASSWORD = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Defaults"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    DescriptionAttribute("The default form to open if no phone number data availible.")> _
    Public Property DEFAULTFORM() As String
        Get
            Return My.Settings.DEFAULTFORM
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.DEFAULTFORM Then
                My.Settings.DEFAULTFORM = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    '<CategoryAttribute("Priority"), _
    '   Browsable(True), _
    '   [ReadOnly](False), _
    '   BindableAttribute(False), _
    '   DefaultValueAttribute("True"), _
    '   DesignOnly(False), _
    '   DescriptionAttribute("Show option")> _
    '   Public Property Show() As Boolean
    '    Get
    '        Return _Show
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        _Show = Value
    '    End Set
    'End Property
    ' '''
    '<CategoryAttribute("Priority"), _
    '   Browsable(True), _
    '   [ReadOnly](False), _
    '   BindableAttribute(False), _
    '   DefaultValueAttribute("0"), _
    '   DesignOnly(False), _
    '   DescriptionAttribute("Enter a number")> _
    '   Public Property Number() As Short
    '    Get
    '        Return _Number
    '    End Get
    '    Set(ByVal Value As Short)
    '        _Number = Value
    '    End Set
    'End Property
    '''

End Class

Public Class PasswordEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Dim NEWPW As String = InputBox("Please enter your new Priority password.")
        If NEWPW.Length > 0 Then
            My.Settings.PRIPASSWORD = NEWPW
            My.Settings.Save()
            Return NEWPW
        Else
            Return value
        End If
    End Function

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class

Public Class DatabasePasswordEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Dim NEWPW As String = InputBox("Please enter your new Database password.")
        If NEWPW.Length > 0 Then
            My.Settings.PASSWORD = NEWPW
            My.Settings.Save()
            Return NEWPW
        Else
            Return value
        End If
    End Function

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class

Public Class PasswordConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destinationType As Type) As Boolean
        If destinationType Is Type.GetType("System.String", True, True) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function


    Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
        If destinationType Is Type.GetType("System.String", True, True) Then
            Dim password As String = value
            If Not IsNothing(password) And password.Length > 0 Then
                Return "********"
            End If
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class