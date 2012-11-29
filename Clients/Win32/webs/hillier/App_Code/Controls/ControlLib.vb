Imports Microsoft.VisualBasic
Imports System.io
Imports System.Collections.Generic

' Contains Priority user control defintions 

Public MustInherit Class CMS
    Inherits System.Web.UI.UserControl
    Dim olu As New OnlineUsers
    Dim WS As New priwebsvc.Service
    Dim SD As New Priority.SerialData

#Region "Initialisations"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack Then
            html = thisSession.Request.Params("ctl00$ContentPlaceHolder1$CMS$Editor")
            Response.Redirect(Split(thisSession.Request.Url.ToString, "?")(0))
        Else
            Dim hlogin As LoginView = sender.FindControl("LoginView1")
            If Not IsNothing(hlogin) Then
                Dim hEdit As HyperLink = hlogin.FindControl("LinkEdit")
                If Not IsNothing(hEdit) Then
                    hEdit.NavigateUrl = thisSession.Request.Url.ToString & "?act=edit"
                End If
            End If

            Dim cph As PlaceHolder = Me.FindControl("ph_Content")
            If Not IsNothing(cph) Then
                Select Case thisSession.Request.Params("Act")
                    Case "edit"
                        Dim txt As New TextBox()
                        With txt
                            .ID = "Editor"
                            .TextMode = TextBoxMode.MultiLine
                            .Text = html
                            .Height = New Unit(100)
                            .Width = New Unit(500)
                        End With
                        cph.Controls.Add(txt)
                        cph.Controls.Add(New LiteralControl(CKEditor))
                    Case Else
                        cph.Controls.Add(New LiteralControl(html))
                End Select
            End If
            End If
    End Sub

#End Region

#Region "public Properties"

    Public Property html() As String
        Get
            Dim ret As String = ""
            Dim data(,) As String = SD.DeSerialiseData(WS.GetData("select HTML from ZWEB_PAGES where FILENAME = '" & FName & "'"))
            If IsNothing(data) Then
                Return ""
            Else
                Return thisSession.Server.UrlDecode(data(0, 0))
            End If
        End Get
        Set(ByVal value As String)
            WS.GetData("insert into ZWEB_PAGES (FILENAME , HTML) VALUES ('" & FName & "','')")
            WS.GetData("update ZWEB_PAGES set HTML = '" & thisSession.Server.UrlEncode(value) & "' where FILENAME = '" & FName & "'")
        End Set
    End Property

#End Region

#Region "Private Properties"

    Private ReadOnly Property FName() As String
        Get
            Return thisSession.Server.MapPath(thisSession.Request.Path.ToString)
        End Get
    End Property

    Private ReadOnly Property thisSession() As Session
        Get
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

    Private ReadOnly Property CKEditor() As String
        Get
            Return "<script type='text/javascript'> " & _
                   "	    CKEDITOR.replace('ctl00$ContentPlaceHolder1$CMS$Editor' ); " & _
                   "</script>"
        End Get
    End Property

#End Region

End Class

Public MustInherit Class WebMenu
    Inherits System.Web.UI.UserControl
    Dim olu As New OnlineUsers
    Dim WS As New priwebsvc.Service
    Dim SD As New Priority.SerialData

#Region "Public Properties"

    Private _View As String = ""
    Public Property View() As String
        Get
            Return _View
        End Get
        Set(ByVal value As String)
            _View = value
        End Set
    End Property

    Private _ItemText As String = ""
    Public Property ItemText() As String
        Get
            Return _ItemText
        End Get
        Set(ByVal value As String)
            _ItemText = value
        End Set
    End Property

    Private _ItemValue As String = ""
    Public Property ItemValue() As String
        Get
            Return _ItemValue
        End Get
        Set(ByVal value As String)
            _ItemValue = value
        End Set
    End Property

    Private _ItemURIModifier As String = ""
    Public Property ItemURIModifier() As String
        Get
            Return _ItemURIModifier
        End Get
        Set(ByVal value As String)
            _ItemURIModifier = value
        End Set
    End Property

    Private _ItemBaseURL As String = ""
    Public Property ItemBaseURL() As String
        Get
            Return _ItemBaseURL
        End Get
        Set(ByVal value As String)
            _ItemBaseURL = value
        End Set
    End Property

    Private _ItemURLParam As String = ""
    Public Property ItemURLParam() As String
        Get
            Return _ItemURLParam
        End Get
        Set(ByVal value As String)
            _ItemURLParam = value
        End Set
    End Property

    Private ReadOnly Property thisSession() As Session
        Get
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

#Region "Initialisations"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim thismenu As Menu = Me.FindControl("ph_Menu")
        If Not IsNothing(thismenu) Then
            With thismenu
                .Items.Clear()

                Dim sql As String = "select " & Me.ItemValue & ", " & Me.ItemText & ", " & Me.ItemURIModifier & " from " & Me.View
                Dim Data(,) As String = SD.DeSerialiseData(WS.GetData(sql))

                If Not (IsNothing(Data)) Then
                    If Not (Left(Data(0, 0), 1) = "!") Then
                        For n As Integer = 0 To UBound(Data, 2)
                            Dim mnuitm As New System.Web.UI.WebControls.MenuItem()
                            With mnuitm
                                .Value = Data(0, n)
                                .Text = "&nbsp;" & Data(1, n)
                                .NavigateUrl = thisSession.NamePairs.ParseURL(Me.ItemBaseURL, Me.ItemURLParam, Data(2, n))
                            End With
                            .Items.Add(mnuitm)
                        Next
                    End If
                End If

            End With
        End If
    End Sub

#End Region

End Class

Public Class EnumerateSpec

#Region "Initialisations"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal FieldName As String, ByVal LabelName As String, ByVal SpecName As String, ByVal SpecValue As String)
        With Me
            .Fieldname = FieldName
            .LabelName = LabelName
            .SpecName = SpecName
            .SpecValue = SpecValue
        End With
    End Sub

#End Region

#Region "Public Properties"

    Private _Fieldname As String
    Public Property Fieldname() As String
        Get
            Return _Fieldname
        End Get
        Set(ByVal value As String)
            _Fieldname = value
        End Set
    End Property
    Private _LabelName As String
    Public Property LabelName() As String
        Get
            Return _LabelName
        End Get
        Set(ByVal value As String)
            _LabelName = value
        End Set
    End Property
    Private _SpecName As String
    Public Property SpecName() As String
        Get
            Return _SpecName
        End Get
        Set(ByVal value As String)
            _SpecName = value
        End Set
    End Property
    Private _SpecValue As String
    Public Property SpecValue() As String
        Get
            Return _SpecValue
        End Get
        Set(ByVal value As String)
            _SpecValue = value
        End Set
    End Property

#End Region

End Class

Public MustInherit Class PartTemplate
    Inherits System.Web.UI.UserControl
    Dim olu As New OnlineUsers
    Dim Specs As New Dictionary(Of String, EnumerateSpec)

#Region "Private variables"

    Private ws As New priwebsvc.Service
    Private sd As New Priority.SerialData

#End Region

#Region "initialisation"

    Private _ctrlBTN As Button = Nothing
    Private _ctrlQTY As TextBox = Nothing

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal ID As String, ByVal PARTNAME As String)
        MyBase.New()
        Me.ID = ID
        _PARTNAME = PARTNAME
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        With sender
            _ctrlBTN = .FindControl("btn")
            _ctrlQTY = .FindControl("txtQTY")
        End With

        If Not IsNothing(_ctrlBTN) Then
            AddHandler _ctrlBTN.Click, AddressOf AddToBasket
        End If

        If Not IsNothing(_ctrlQTY) Then
            _ctrlQTY.AutoPostBack = True
            AddHandler _ctrlQTY.TextChanged, AddressOf txtQTY_TextChanged
        End If

        If Not Page.IsPostBack Then
            QTY = DefaultQTY
            If LoadPart() Then SetFields()
        Else
            QTY = _ctrlQTY.Text
        End If

    End Sub

#End Region

#Region "Public Properties"

    Private _CURRENCY As String = "GBP"
    Public ReadOnly Property CURRENCY() As String
        Get
            Return olu.CurrentSession(HttpContext.Current).Basket.CURRENCY
        End Get
    End Property

    Private _PARTPRICE As String = "0.00"
    Public ReadOnly Property PARTPRICE() As String
        Get
            If InStr(_PARTPRICE, ".") Then
                Return Split(_PARTPRICE, ".")(0) & "." & Left(Split(_PARTPRICE, ".")(1) & "00", 2)
            Else
                Return _PARTPRICE & ".00"
            End If
        End Get
    End Property

    Private _PARTNAME As String = ""
    Public Property PARTNAME() As String
        Get
            Return _PARTNAME
        End Get
        Set(ByVal value As String)
            _PARTNAME = value
        End Set
    End Property

    Private _PARTDES As String = ""
    Public ReadOnly Property PARTDES() As String
        Get
            Return _PARTDES
        End Get
    End Property

    Private _DefaultQTY As String = "1"
    Public Property DefaultQTY() As String
        Get
            Return _DefaultQTY
        End Get
        Set(ByVal value As String)
            _DefaultQTY = value
        End Set
    End Property

    Private _QTY As String = ""
    Public Property QTY() As String
        Get
            Return _QTY
        End Get
        Set(ByVal value As String)
            _QTY = value
        End Set
    End Property

    Private _AVAILABLE As String = "0"
    Public ReadOnly Property AVAILABLE() As String
        Get
            Return _AVAILABLE
        End Get
    End Property

    Private _SALESTAX As String = ""
    Public ReadOnly Property SALESTAX() As String
        Get
            Return FormatDouble(_SALESTAX)
        End Get
    End Property

    Private _LINETOTAL As String = ""
    Public ReadOnly Property LINETOTAL() As String
        Get
            Return FormatDouble(CStr(CInt(_QTY) * (CDbl(_PARTPRICE) * (1 + (CDbl(_SALESTAX) / 100)))))
        End Get
    End Property

    Private _TAXCODE As String
    Public ReadOnly Property TAXCODE() As String
        Get
            Return olu.CurrentSession(HttpContext.Current).Basket.TAXCODE
        End Get
    End Property

    Private _IMGURL As String = ""
    Public ReadOnly Property IMGURL() As String
        Get
            Return _IMGURL
        End Get
    End Property

    Private _PARTREMARK As String
    Public ReadOnly Property PARTREMARK() As String
        Get
            Return _PARTREMARK
        End Get
    End Property

#End Region

#Region "Private Functions"

    Private Function FormatDouble(ByVal str As String) As String
        If InStr(str, ".") Then
            Return Split(str, ".")(0) & "." & Left(Split(str, ".")(1) & "00", 2)
        Else
            Return str & ".00"
        End If
    End Function

    Private Function LoadPart() As Boolean

        Dim ret As Boolean = True
        Dim c As New OnlineUsers
        Dim sql As String = "SELECT PARTNAME,PARTDES,QTY,PARTPRICE,AVAILABLE,TAXRATE,IMGURL " & _
                            "FROM dbo.ZWEBFunc_BASKETITEM('" & _
                                PARTNAME & "'," & _
                                CInt(QTY) & ",'" & _
                                CURRENCY & "','" & _
                                TAXCODE & "')"

        Dim Data(,) As String = sd.DeSerialiseData(ws.GetData(sql))
        If Not IsNothing(Data) Then
            For i As Integer = 0 To UBound(Data, 2)
                If CInt(QTY) >= CInt(Data(2, i)) Then
                    _PARTNAME = Data(0, i)
                    _PARTDES = Data(1, i)
                    _PARTPRICE = Data(3, i)
                    _AVAILABLE = Data(4, i)
                    _SALESTAX = Data(5, i)
                    _IMGURL = Data(6, i)
                End If
            Next
            If CDbl(_PARTPRICE) = 0 Then
                ret = False
                Me.Visible = False
            End If
        Else
            ret = False
            Me.Visible = False
        End If

        _PARTREMARK = ""
        sql = "select TEXT from PARTTEXT " & _
                "where PART = (select PART from PART where PARTNAME = '" & PARTNAME & "') " & _
                "order by TEXTORD"
        Data = sd.DeSerialiseData(ws.GetData(sql))
        If Not IsNothing(Data) Then
            For i As Integer = 0 To UBound(Data, 2)
                _PARTREMARK += Data(0, i) & " "
            Next
        End If

        Specs.Clear()
        sql = "SELECT ORD, SPECNAME, SPECVALUE FROM dbo.ZWEBFunc_BASKETSPEC('" & PARTNAME & "')"
        Data = sd.DeSerialiseData(ws.GetData(sql))
        If Not IsNothing(Data) Then
            For i As Integer = 0 To UBound(Data, 2)
                Specs.Add(Data(0, i), New EnumerateSpec("txtSPEC" & Data(0, i), "lblSPEC" & Data(0, i), Data(1, i), Data(2, i)))
            Next
        End If
        Return ret

    End Function

#End Region

#Region "Private Subs"

    Private Sub SetFields()

        setField("txtPARTNAME", PARTNAME)
        setField("txtPARTDES", PARTDES)
        setField("txtUNITPRICE", PARTPRICE)
        setField("txtCURRENCY", CURRENCY)
        setField("txtAVAILIBLE", AVAILABLE)
        setField("txtQTY", QTY)

        Dim ctrl As Image = Me.FindControl("IMAGE")
        If Not IsNothing(ctrl) Then
            ctrl.ImageUrl = "http://mobile.ntsa.org.uk/PriImage.aspx?image=" & IMGURL
        End If
        Dim ph_PARTREMARK As PlaceHolder = Me.FindControl("ph_PARTREMARK")
        If Not IsNothing(ph_PARTREMARK) Then
            ph_PARTREMARK.Controls.Add(New LiteralControl(PARTREMARK))
        End If

        For Each i As EnumerateSpec In Specs.Values
            setField(i.LabelName, i.SpecName)
            setField(i.Fieldname, i.SpecValue)
        Next

    End Sub

    Private Sub setField(ByVal FieldName As String, ByVal Value As String)
        Dim ctrl As Object = Me.FindControl(FieldName)
        If Not IsNothing(ctrl) Then
            ctrl.text = Value
        End If
    End Sub

#End Region

#Region "Control Handlers"

    Protected Sub txtQTY_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        QTY = _ctrlQTY.Text
        If LoadPart() Then SetFields()
    End Sub

    Protected Sub AddToBasket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim c As New OnlineUsers
        With c.CurrentSession(HttpContext.Current).Basket
            .AddBasketItem(New BasketItem(PARTNAME, _ctrlQTY.Text))
            Response.Redirect(.BasketURL())
        End With

    End Sub

#End Region

End Class

Public MustInherit Class CurrencySelect
    Inherits System.Web.UI.UserControl
    Dim olu As New OnlineUsers
    Private WithEvents myCurrency As DropDownList

#Region "Initialisation"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        myCurrency = sender.findcontrol("lstCurrency")
        If Not IsNothing(myCurrency) Then
            AddHandler myCurrency.SelectedIndexChanged, AddressOf hCurrency_Changed
            If Not Page.IsPostBack Then
                Dim sd As New Priority.SerialData
                Dim ws As New priwebsvc.Service
                Dim sql As String = "select CODE from dbo.ZWEBV_BASKETCURR"
                Dim Data(,) As String = sd.DeSerialiseData(ws.GetData(sql))
                If Not IsNothing(Data) Then
                    For i As Integer = 0 To UBound(Data, 2)
                        Dim li As New ListItem
                        With olu.CurrentSession(HttpContext.Current)
                            li.Text = Data(0, i)
                            li.Value = Data(0, i)
                            If .Basket.CURRENCY = Data(0, i) Then
                                li.Selected = True
                            Else
                                li.Selected = False
                            End If
                            myCurrency.Items.Add(li)
                        End With
                    Next
                End If
            End If
        End If
    End Sub

#End Region

#Region "Event Handlers"

    Protected Sub hCurrency_Changed(ByVal sender As Object, ByVal e As System.EventArgs)
        With olu.CurrentSession(HttpContext.Current)
            .Basket.CURRENCY = sender.selecteditem.value
            .cart.CURRENCY = sender.selecteditem.value
            Response.Redirect(.Info.ViewingPage)
        End With
    End Sub

#End Region

End Class

Public Class PriDataView
    Inherits System.Web.UI.UserControl

#Region "Session Reference"

    Shared olu As OnlineUsers = Nothing
    Public ReadOnly Property thisSession() As Session
        Get
            If IsNothing(olu) Then
                olu = New OnlineUsers()
            End If
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

#Region "Private variables"

    Private _FilterView As String = ""
    Private _DataView As String = ""
    Private _UID As String = ""
    Private _Template As String

#End Region

#Region "public properties"

    Public Property FilterView() As String
        Get
            Return _FilterView
        End Get
        Set(ByVal value As String)
            _FilterView = value
        End Set
    End Property

    Public Property DataView() As String
        Get
            Return _DataView
        End Get
        Set(ByVal value As String)
            _DataView = value
        End Set
    End Property

    Public Property UID() As String
        Get
            Return _UID
        End Get
        Set(ByVal value As String)
            _UID = value
        End Set
    End Property

    Public Property Template() As String
        Get
            Return _Template
        End Get
        Set(ByVal value As String)
            _Template = value
        End Set
    End Property

#End Region

#Region "initialisation"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Load The Enclosure
        Dim cph As PlaceHolder = Me.FindControl("ph_Enclosure")
        If Not IsNothing(cph) Then
            Dim Enclosure As New tEnclosure(Me.FilterView, Me.UID)
            Dim template As New tTemplate(Page, Me.Template)
            With Enclosure
                .Create(template, thisSession.NamePairs, Me.FilterView, cph)
                .LoadTemplate(template, Me.DataView, thisSession.NamePairs)
                .ddl.DrawFilters(Me)
            End With
        End If

    End Sub

#End Region

#Region "Event Handlers"

    Sub hDDLCLick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddl As DropDownList
        ddl = sender
        Response.Redirect( _
            thisSession.NamePairs.ParseURL(System.IO.Path.GetFileName _
            ( _
                thisSession.Request.Url.AbsolutePath _
            ) _
            , ddl.ID, ddl.SelectedValue))
    End Sub

    Sub hFilterBtn(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As String = Replace(sender.id(), "tf_", "")
        Dim t As TextBox = sender.parent.findcontrol(o)
        Dim te As String = ""
        If Not IsNothing(t) Then
            te = Replace(t.Text, "%", "*")
            If Len(te) > 0 Then
                If Not InStr(te, "*") > 0 Then
                    te = te & "*"
                End If
            End If
            Response.Redirect( _
                thisSession.NamePairs.ParseURL(System.IO.Path.GetFileName(thisSession.Request.Url.AbsolutePath) _
                , t.ID, te))
        End If
    End Sub

#End Region

End Class

Public Class PriCart
    Inherits System.Web.UI.UserControl
    Dim olu As New OnlineUsers

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim BasketGrid As GridView = Me.FindControl("BasketGrid")
        If Not IsNothing(BasketGrid) Then
            With thisSession.Basket
                .AddHandlers(BasketGrid)
                If Not Page.IsPostBack Then
                    .BindBasket(BasketGrid)
                End If
            End With
        End If
    End Sub

#Region "Private Properties"

    Private ReadOnly Property thisSession() As Session
        Get
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

End Class