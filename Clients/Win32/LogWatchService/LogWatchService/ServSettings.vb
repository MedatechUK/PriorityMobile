Public Class ServSettings
    Private LogItem As String
    Private LogRefresh As Integer
    Private SQLTabUname As String
    Private SQLTabPassword As String
    Private SQLServ As String
    Private SQLDB As String
    Private SMTPServer As String
    Private SSL As Boolean
    Private SMTPUser As String
    Private SMTPPassword As String
    Private SMTPPort As String
    Private MailErr As Boolean
    Private MailFail As Boolean
    Private MailWarn As Boolean
    Private MailList As String
    Private IgnoreList As String
    Private KeepDays As Integer
    Private WriteLog As Boolean
    Public Property xmlLogItem() As String
        Get
            Return LogItem
        End Get
        Set(ByVal value As String)
            LogItem = value
        End Set
    End Property
    Public Property xmlLogRefresh() As Integer
        Get
            Return LogRefresh
        End Get
        Set(ByVal value As Integer)
            LogRefresh = value
        End Set
    End Property
    Public Property xmlSQLTabUname() As String
        Get
            Return SQLTabUname
        End Get
        Set(ByVal value As String)
            SQLTabUname = value
        End Set
    End Property
    Public Property xmlSQLTabPassword() As String
        Get
            Return SQLTabPassword
        End Get
        Set(ByVal value As String)
            SQLTabPassword = value
        End Set
    End Property
    Public Property xmlSQLServ() As String
        Get
            Return SQLServ
        End Get
        Set(ByVal value As String)
            SQLServ = value
        End Set
    End Property
    Public Property xmlSQLDB() As String
        Get
            Return SQLDB
        End Get
        Set(ByVal value As String)
            SQLDB = value
        End Set
    End Property
    Public Property xmlSMTPServer() As String
        Get
            Return SMTPServer
        End Get
        Set(ByVal value As String)
            SMTPServer = value
        End Set
    End Property
    Public Property xmlSSL() As Boolean
        Get
            Return CBool(SSL)
        End Get
        Set(ByVal value As Boolean)
            SSL = CBool(value)
        End Set
    End Property
    Public Property xmlSMTPUser() As String
        Get
            Return SMTPUser
        End Get
        Set(ByVal value As String)
            SMTPUser = value
        End Set
    End Property
    Public Property xmlSMTPassword() As String
        Get
            Return SMTPPassword
        End Get
        Set(ByVal value As String)
            SMTPPassword = value
        End Set
    End Property
    Public Property xmlSMTPPort() As String
        Get
            Return SMTPPort
        End Get
        Set(ByVal value As String)
            SMTPPort = value
        End Set
    End Property
    Public Property xmlError() As Boolean
        Get
            Return CBool(MailErr)
        End Get
        Set(ByVal value As Boolean)
            MailErr = CBool(value)
        End Set
    End Property
    Public Property xmlFail() As Boolean
        Get
            Return CBool(MailFail)
        End Get
        Set(ByVal value As Boolean)
            MailFail = CBool(value)
        End Set
    End Property
    Public Property xmlWarn() As Boolean
        Get
            Return CBool(MailWarn)
        End Get
        Set(ByVal value As Boolean)
            MailWarn = CBool(value)
        End Set
    End Property
    Public Property xmlMailList() As String
        Get
            Return MailList
        End Get
        Set(ByVal value As String)
            MailList = value
        End Set
    End Property
    Public Property xmlIgnoreList() As String
        Get
            Return IgnoreList
        End Get
        Set(ByVal value As String)
            IgnoreList = value
        End Set
    End Property
    Public Property xmlKeepDays() As Integer
        Get
            Return KeepDays
        End Get
        Set(ByVal value As Integer)
            KeepDays = value
        End Set
    End Property
    Public Property xmlWriteLog() As Boolean
        Get
            Return WriteLog
        End Get
        Set(ByVal value As Boolean)
            WriteLog = value
        End Set
    End Property
    Public Sub New(ByVal li As String, ByVal lr As Integer, ByVal stu As String, ByVal stp As String, ByVal sserv As String, ByVal sdb As String, ByVal smtps As String, ByVal sl As Boolean, ByVal un As String, ByVal pas As String, ByVal po As String, ByVal e As Boolean, ByVal f As Boolean, ByVal w As Boolean, ByVal mlist As String, ByVal ilist As String, ByVal kd As Integer, ByVal wlog As Boolean)
        xmlLogItem = li
        xmlLogRefresh = lr
        xmlSQLTabUname = stu
        xmlSQLTabPassword = stp
        xmlSQLServ = sserv
        xmlSQLDB = sdb
        xmlSMTPServer = smtps
        xmlSSL = sl
        xmlSMTPUser = un
        xmlSMTPassword = pas
        xmlSMTPPort = po
        xmlError = e
        xmlFail = f
        xmlWarn = w
        xmlMailList = mlist
        xmlIgnoreList = ilist
        xmlKeepDays = kd
        xmlWriteLog = wlog
    End Sub

End Class
