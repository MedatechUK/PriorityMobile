Imports System.Configuration


Public Class PriorityConfig
    Inherits System.Configuration.ConfigurationSection

    Private m_cfg As Configuration

#Region " Instance methods "
    Public Sub Encript(ByVal encript As Boolean)
        If encript Then
            Me.SectionInformation.ProtectSection("DataProtectionConfigurationProvider")
        Else
            Me.SectionInformation.UnprotectSection()
        End If
        Me.Save()
    End Sub

    Public Sub Save()
        Me.SectionInformation.ForceSave = True
        m_cfg.Save(ConfigurationSaveMode.Full)
    End Sub

#End Region

#Region " Properties "

    <ConfigurationProperty("Bin95", DefaultValue:="", IsRequired:=True)> _
    Public Property Bin95() As String
        Get
            Return CType(Me("Bin95"), String)
        End Get
        Set(ByVal value As String)
            Me("Bin95") = value
        End Set
    End Property

    <ConfigurationProperty("PriorityCompany", DefaultValue:="", IsRequired:=True)> _
    Public Property PriorityCompany() As String
        Get
            Return CType(Me("PriorityCompany"), String)
        End Get
        Set(ByVal value As String)
            Me("PriorityCompany") = value
        End Set
    End Property

    'applicationName
    <ConfigurationProperty("PriorityUser", DefaultValue:="", IsRequired:=True)> _
    Public Property PriorityUser() As String
        Get
            Return CType(Me("PriorityUser"), String)
        End Get
        Set(ByVal value As String)
            Me("PriorityUser") = value
        End Set
    End Property

    'connectionStringName
    <ConfigurationProperty("PriorityPwd", DefaultValue:="", IsRequired:=True)> _
    Public Property PriorityPwd() As String
        Get
            Return CType(Me("PriorityPwd"), String)
        End Get
        Set(ByVal value As String)
            Me("PriorityPwd") = value
        End Set
    End Property

    <ConfigurationProperty("RunAs", IsRequired:=True)> _
    Public ReadOnly Property RunAs() As PriorityRunAsClass
        Get
            Return DirectCast(Me("RunAs"), _
                              PriorityRunAsClass)
        End Get
    End Property

    <ConfigurationProperty("Prepare", IsRequired:=True)> _
    Public ReadOnly Property Prepare() As PriorityPrepareClass
        Get
            Return DirectCast(Me("Prepare"), _
                              PriorityPrepareClass)
        End Get
    End Property

#End Region

#Region " Shared methods "
    Private Const CS_SECTION As String = "PriorityCnf"
    Private Shared m_current As PriorityConfig

    Public Shared ReadOnly Property Current() As PriorityConfig
        Get
            If m_current Is Nothing Then
                Dim cfg As Configuration
                Dim ctx As System.Web.HttpContext = System.Web.HttpContext.Current
                If ctx Is Nothing Then
                    cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                Else
                    cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(ctx.Request.ApplicationPath)
                End If
                m_current = DirectCast(cfg.Sections(CS_SECTION), _
                                       PriorityConfig)
                m_current.m_cfg = cfg
            End If
            Return m_current
        End Get
    End Property

#End Region

End Class

Public Class PriorityRunAsClass
    Inherits ConfigurationElement

    <ConfigurationProperty("Domain", IsRequired:=True)> _
    Public Property Domain() As String
        Get
            Return CStr(Me("Domain"))
        End Get
        Set(ByVal value As String)
            Me("Domain") = value
        End Set
    End Property

    <ConfigurationProperty("Username", IsRequired:=True)> _
    Public Property Username() As String
        Get
            Return CStr(Me("Username"))
        End Get
        Set(ByVal value As String)
            Me("Username") = value
        End Set
    End Property

    <ConfigurationProperty("Password", IsRequired:=True)> _
    Public Property Password() As String
        Get
            Return CStr(Me("Password"))
        End Get
        Set(ByVal value As String)
            Me("Password") = value
        End Set
    End Property
End Class

Public Class PriorityPrepareClass
    Inherits ConfigurationElement

    <ConfigurationProperty("UNC", IsRequired:=True)> _
        Public Property UNC() As String
        Get
            Return CStr(Me("UNC"))
        End Get
        Set(ByVal value As String)
            Me("UNC") = value
        End Set
    End Property

    <ConfigurationProperty("MapDrive", IsRequired:=True)> _
    Public Property MapDrive() As String
        Get
            Return CStr(Me("MapDrive"))
        End Get
        Set(ByVal value As String)
            Me("MapDrive") = value
        End Set
    End Property

End Class
