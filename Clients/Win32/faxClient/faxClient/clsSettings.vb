Imports System.ComponentModel
Imports ntSettings
'''
<DefaultPropertyAttribute("PRICOMPANY")> _
Public Class SimpleProperties

    Private mySettings As xmlSettings
    Public Sub New(ByRef xS As xmlSettings)
        mySettings = xS
    End Sub
    '''
    <CategoryAttribute("Server"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The \\UNC name of the machine that will process the FAX.")> _
       Public Property FaxServer() As String
        Get
            Return mySettings.Setting("FaxServer")
        End Get
        Set(ByVal Value As String)
            If Not Value = mySettings.Setting("FaxServer") Then
                mySettings.Setting("FaxServer") = Value

            End If
        End Set
    End Property
    '''
    <CategoryAttribute("Sender"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The senders Title.")> _
       Public Property Title() As String
        Get
            Return MySettings.Setting("Title")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("Title") Then
                MySettings.Setting("Title") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders Name.")> _
   Public Property Name() As String
        Get
            Return MySettings.Setting("Name")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("Name") Then
                MySettings.Setting("Name") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders City.")> _
   Public Property City() As String
        Get
            Return MySettings.Setting("City")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("City") Then
                MySettings.Setting("City") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders State.")> _
   Public Property State() As String
        Get
            Return MySettings.Setting("State")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("State") Then
                MySettings.Setting("State") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders Company.")> _
   Public Property Company() As String
        Get
            Return MySettings.Setting("Company")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("Company") Then
                MySettings.Setting("Company") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders Country.")> _
   Public Property Country() As String
        Get
            Return MySettings.Setting("Country")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("Country") Then
                MySettings.Setting("Country") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders Email.")> _
   Public Property Email() As String
        Get
            Return MySettings.Setting("Email")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("Email") Then
                MySettings.Setting("Email") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders FaxNumber.")> _
   Public Property FaxNumber() As String
        Get
            Return MySettings.Setting("FaxNumber")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("FaxNumber") Then
                MySettings.Setting("FaxNumber") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders HomePhone.")> _
   Public Property HomePhone() As String
        Get
            Return MySettings.Setting("HomePhone")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("HomePhone") Then
                MySettings.Setting("HomePhone") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders OfficeLocation.")> _
   Public Property OfficeLocation() As String
        Get
            Return MySettings.Setting("OfficeLocation")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("OfficeLocation") Then
                MySettings.Setting("OfficeLocation") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders OfficePhone.")> _
   Public Property OfficePhone() As String
        Get
            Return MySettings.Setting("OfficePhone")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("OfficePhone") Then
                MySettings.Setting("OfficePhone") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders StreetAddress.")> _
   Public Property StreetAddress() As String
        Get
            Return MySettings.Setting("StreetAddress")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("StreetAddress") Then
                MySettings.Setting("StreetAddress") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders TSID.")> _
   Public Property TSID() As String
        Get
            Return MySettings.Setting("TSID")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("TSID") Then
                MySettings.Setting("TSID") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders ZipCode.")> _
   Public Property ZipCode() As String
        Get
            Return MySettings.Setting("ZipCode")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("ZipCode") Then
                MySettings.Setting("ZipCode") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders BillingCode.")> _
   Public Property BillingCode() As String
        Get
            Return MySettings.Setting("BillingCode")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("BillingCode") Then
                MySettings.Setting("BillingCode") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Sender"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The senders Department.")> _
   Public Property Department() As String
        Get
            Return MySettings.Setting("Department")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("Department") Then
                MySettings.Setting("Department") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Receipts"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The email address to send receipt notifications to.")> _
   Public Property ReceiptAddress() As String
        Get
            Return MySettings.Setting("ReceiptAddress")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("ReceiptAddress") Then
                MySettings.Setting("ReceiptAddress") = Value

            End If
        End Set
    End Property

    <CategoryAttribute("Cover Page"), _
   Browsable(True), _
   [ReadOnly](False), _
   BindableAttribute(False), _
   DefaultValueAttribute(""), _
   DesignOnly(False), _
   DescriptionAttribute("The cover page to use when FAXing.")> _
   Public Property CoverPage() As String
        Get
            Return MySettings.Setting("CoverPage")
        End Get
        Set(ByVal Value As String)
            If Not Value = MySettings.Setting("CoverPage") Then
                MySettings.Setting("CoverPage") = Value

            End If
        End Set
    End Property

End Class