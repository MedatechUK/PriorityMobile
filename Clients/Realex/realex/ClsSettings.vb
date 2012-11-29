Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("PRICOMPANY")> _
Public Class SimpleProperties

    <CategoryAttribute("Realex"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    DescriptionAttribute("The Realex merchant name.")> _
    Public Property MerchantName() As String
        Get
            Return My.Settings.MerchantName
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.MerchantName Then
                My.Settings.MerchantName = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Realex"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    DescriptionAttribute("The Realex transaction account.")> _
    Public Property transAccount() As String
        Get
            Return My.Settings.transAccount
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.transAccount Then
                My.Settings.transAccount = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Realex"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    TypeConverter(GetType(PasswordConverter)), _
    Editor(GetType(normalPasswordEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The normal Realex password.")> _
    Public Property normalPassword() As String
        Get
            Return My.Settings.normalPassword
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.normalPassword Then
                My.Settings.normalPassword = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Realex"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    TypeConverter(GetType(PasswordConverter)), _
    Editor(GetType(rebatePasswordEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The rebate Realex password.")> _
    Public Property rebatePassword() As String
        Get
            Return My.Settings.rebatePassword
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.rebatePassword Then
                My.Settings.rebatePassword = Value
                My.Settings.Save()
            End If
        End Set
    End Property

    <CategoryAttribute("Realex"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    TypeConverter(GetType(PasswordConverter)), _
    Editor(GetType(refundPasswordEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The normal Realex password.")> _
    Public Property refundPassword() As String
        Get
            Return My.Settings.refundPassword
        End Get
        Set(ByVal Value As String)
            If Not Value = My.Settings.refundPassword Then
                My.Settings.refundPassword = Value
                My.Settings.Save()
            End If
        End Set
    End Property

End Class

Public Class normalPasswordEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Dim NEWPW As String = InputBox("Please enter your new password.")
        If NEWPW.Length > 0 Then
            My.Settings.normalPassword = NEWPW
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

Public Class rebatePasswordEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Dim NEWPW As String = InputBox("Please enter your new password.")
        If NEWPW.Length > 0 Then
            My.Settings.rebatePassword = NEWPW
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

Public Class refundPasswordEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Dim NEWPW As String = InputBox("Please enter your new password.")
        If NEWPW.Length > 0 Then
            My.Settings.refundPassword = NEWPW
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