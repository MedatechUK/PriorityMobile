Imports Microsoft.VisualBasic
Imports System.Collections.Generic

' Contains classes for dealing with the user session

Public Class DeliveryItem

#Region "Initialisations"

    Public Sub New()
        MyBase.new()
        With Me
            .DELIVERYPART = ""
            .PARTDES = "No selected delivery Item"
            .PARTPRICE = "0.00"
            .SALESTAX = "0.00"
            .REFERER = ""
        End With
    End Sub

    Public Sub New(ByVal PARTNAME As String, ByVal PARTDES As String, ByVal PARTPRICE As String, ByVal SALESTAX As String)
        MyBase.new()
        With Me
            .DELIVERYPART = PARTNAME
            .PARTDES = PARTDES
            .PARTPRICE = PARTPRICE
            .SALESTAX = SALESTAX
            .REFERER = ""
        End With
    End Sub

#End Region

#Region "Public Properties"

    Private _PARTNAME As String = ""
    Public ReadOnly Property PARTNAME() As String
        Get
            Return "DELIVERY"
        End Get
    End Property

    Private _DELPART As String = ""
    Public Property DELIVERYPART() As String
        Get
            Return _DELPART
        End Get
        Set(ByVal value As String)
            _DELPART = value
        End Set
    End Property

    Private _PARTDES As String = ""
    Public Property PARTDES() As String
        Get
            Return _PARTDES
        End Get
        Set(ByVal value As String)
            _PARTDES = value
        End Set
    End Property

    Private _PARTPRICE As String = ""
    Public Property PARTPRICE() As String
        Get
            Return FormatDouble(_PARTPRICE)
        End Get
        Set(ByVal value As String)
            _PARTPRICE = value
        End Set
    End Property

    Private _QTY As String = ""
    Public ReadOnly Property QTY() As String
        Get
            Return "1"
        End Get
    End Property

    Private _SALESTAX As String = ""
    Public Property SALESTAX() As String
        Get
            Return FormatDouble(_SALESTAX)
        End Get
        Set(ByVal value As String)
            _SALESTAX = value
        End Set
    End Property

    Private _LINETOTAL As String = ""
    Public ReadOnly Property LINETOTAL() As String
        Get
            Return FormatDouble(CStr(CInt(_QTY) * (CDbl(_PARTPRICE) * (1 + CDbl(_SALESTAX) / 100))))
        End Get
    End Property

    Private _REFERER As String = ""
    Public Property REFERER() As String
        Get
            Return _REFERER
        End Get
        Set(ByVal value As String)
            _REFERER = value
        End Set
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

#End Region

End Class

Public Class CartItem

#Region "Initialisations"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal PARTNAME As String, ByVal PARTDES As String, ByVal PARTPRICE As String, ByVal QTY As String, ByVal SALESTAX As String, ByVal REFERER As String)
        MyBase.new()
        _PARTNAME = PARTNAME
        _PARTDES = PARTDES
        _PARTPRICE = PARTPRICE
        _QTY = QTY
        _SALESTAX = SALESTAX
        _REFERER = REFERER
    End Sub

#End Region

#Region "Public Properties"

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
    Public Property PARTDES() As String
        Get
            Return _PARTDES
        End Get
        Set(ByVal value As String)
            _PARTDES = value
        End Set
    End Property

    Private _PARTPRICE As String = ""
    Public Property PARTPRICE() As String
        Get
            Return FormatDouble(_PARTPRICE)
        End Get
        Set(ByVal value As String)
            _PARTPRICE = value
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

    Private _SALESTAX As String = ""
    Public Property SALESTAX() As String
        Get
            Return FormatDouble(_SALESTAX)
        End Get
        Set(ByVal value As String)
            _SALESTAX = value
        End Set
    End Property

    Private _LINETOTAL As String = ""
    Public ReadOnly Property LINETOTAL() As String
        Get
            Return FormatDouble(CStr(CInt(_QTY) * (CDbl(_PARTPRICE) * (1 + CDbl(_SALESTAX) / 100))))
        End Get
    End Property

    Private _REFERER As String = ""
    Public Property REFERER() As String
        Get
            Return _REFERER
        End Get
        Set(ByVal value As String)
            _REFERER = value
        End Set
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

#End Region

End Class

Public Class Cart

#Region "Public Variables"

    Public CartItems As New Dictionary(Of String, CartItem)

#End Region

#Region "Private Variables"

    Private c As New OnlineUsers

#End Region

#Region "Initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Public Properties"

    Private _CURRENCY As String = "GBP"
    Public Property CURRENCY() As String
        Get
            Return _CURRENCY
        End Get
        Set(ByVal value As String)
            _CURRENCY = value
        End Set
    End Property

    Private _Value As String = "0.00"
    Public ReadOnly Property Value() As String
        Get
            Dim t As Double = 0
            For Each i As CartItem In Me.CartItems.Values
                t += CDbl(i.LINETOTAL)
            Next
            Return FormatDouble(CStr(t))
        End Get
    End Property

    Private _DELIVERY As New DeliveryItem
    Public Property DELIVERY() As DeliveryItem
        Get
            Return _DELIVERY
        End Get
        Set(ByVal value As DeliveryItem)
            _DELIVERY = value
        End Set
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

#End Region

End Class

Public Class BasketItem

#Region "initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal PARTNAME As String, ByVal QTY As String)
        MyBase.New()
        _PARTNAME = PARTNAME
        _QTY = QTY
        _REFERER = System.Web.HttpContext.Current.Request.Url.AbsolutePath
    End Sub

#End Region

#Region "Public Properties"

    Private _PARTNAME As String = String.Empty
    Public Property PARTNAME() As String
        Get
            Return _PARTNAME
        End Get
        Set(ByVal value As String)
            _PARTNAME = value
        End Set
    End Property

    Private _QTY As String = String.Empty
    Public Property QTY() As String
        Get
            Return _QTY
        End Get
        Set(ByVal value As String)
            _QTY = value
        End Set
    End Property

    Private _REFERER As String = String.Empty
    Public Property REFERER() As String
        Get
            Return _REFERER
        End Get
        Set(ByVal value As String)
            _REFERER = value
        End Set
    End Property

#End Region

End Class

Public Class SessionInfo

#Region "Initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal CurrentContext As HttpContext)
        MyBase.new()
        With CurrentContext
            _SessionStart = CStr(Now)
            _UserAgent = .Request.UserAgent
            _UserHostAddress = .Request.UserHostAddress
            _ViewingPage = .Request.RawUrl
        End With
    End Sub

#End Region

#Region "Public Properties"

    Private _SessionStart As String
    Public Property SessionStart() As String
        Get
            Return _SessionStart
        End Get
        Set(ByVal value As String)
            _SessionStart = value
        End Set
    End Property

    Private _UserAgent As String
    Public Property UserAgent() As String
        Get
            Return _UserAgent
        End Get
        Set(ByVal value As String)
            _UserAgent = value
        End Set
    End Property

    Private _UserHostAddress As String
    Public Property UserHostAddress() As String
        Get
            Return _UserHostAddress
        End Get
        Set(ByVal value As String)
            _UserHostAddress = value
        End Set
    End Property

    Private _ViewingPage As String
    Public Property ViewingPage() As String
        Get
            Return _ViewingPage
        End Get
        Set(ByVal value As String)
            _ViewingPage = value
        End Set
    End Property

#End Region

End Class

Public Class BasketCls

#Region "initialisations"

    Public Sub New()
        '********************************* This sets up the basket
        GetDeliveryOptions()
        BasketURL = "~/basket.aspx"
        CURRENCY = "GBP"
        TAXCODE = "001"
        DEFAULTDELIVERY = "DEL-STD"
    End Sub

#End Region

#Region "Public Variables"

    Public BasketItems As New Dictionary(Of String, BasketItem)
    Public DeliveryItems As New Dictionary(Of String, DeliveryItem)

#End Region

#Region "Private Variables"

    Private c As New OnlineUsers

#End Region

#Region "public properties"

    Private _CURRENCY As String = "GBP"
    Public Property CURRENCY() As String
        Get
            Return _CURRENCY
        End Get
        Set(ByVal value As String)
            _CURRENCY = value
        End Set
    End Property

    Private _TAXCODE As String = "001"
    Public Property TAXCODE() As String
        Get
            Return _TAXCODE
        End Get
        Set(ByVal value As String)
            _TAXCODE = value
        End Set
    End Property

    Private _BasketURL As String = "basket.aspx"
    Public Property BasketURL() As String
        Get
            Return _BasketURL
        End Get
        Set(ByVal value As String)
            _BasketURL = value
        End Set
    End Property

    Private mDDL As New DropDownList
    Public Property DelOptList() As DropDownList
        Get
            Return mDDL
        End Get
        Set(ByVal value As DropDownList)
            mDDL = value
        End Set
    End Property

    Private _DEFAULTDELIVERY As String = ""
    Public Property DEFAULTDELIVERY() As String
        Get
            Return _DEFAULTDELIVERY
        End Get
        Set(ByVal value As String)
            _DEFAULTDELIVERY = value
        End Set
    End Property

#End Region

#Region "Update Delivery Options"

    Public Sub GetDeliveryOptions()

        mDDL.ID = "lstDEL"
        Dim sql As String = "SELECT PARTNAME,PARTDES,QTY,PARTPRICE,AVAILABLE,TAXRATE " & _
                            "FROM dbo.ZWEBFunc_BASKETDEL('" & CURRENCY & "','" & TAXCODE & "')"
        Dim Data(,) As String = OnlineUsers.sd.DeSerialiseData(OnlineUsers.ws.GetData(sql))
        If Not IsNothing(Data) Then

            For i As Integer = 0 To UBound(Data, 2)
                Dim li As New ListItem
                li.Value = Data(0, i)
                li.Text = Data(1, i)
                mDDL.Items.Add(li)

                DeliveryItems.Add(Data(0, i), _
                    New DeliveryItem(Data(0, i), _
                    Data(1, i), _
                    Data(3, i), _
                    Data(5, i)) _
                )
            Next
        End If

    End Sub

#End Region

#Region "Update Basket"

    Public Sub AddBasketItem(ByVal Item As BasketItem)
        With BasketItems
            If Not .ContainsKey(Item.PARTNAME) Then
                .Add(Item.PARTNAME, Item)
            Else
                Dim newQTY As String = CStr(CInt(.Item(Item.PARTNAME).QTY) + CInt(Item.QTY))
                .Item(Item.PARTNAME).QTY = newQTY
            End If
        End With
    End Sub

    Public Sub RemoveBasketItem(ByVal PARTNAME As String)
        With BasketItems
            If .ContainsKey(PARTNAME) Then
                .Remove(PARTNAME)
            End If
        End With

        With c.CurrentSession(HttpContext.Current).cart.CartItems
            If .ContainsKey(PARTNAME) Then
                .Remove(PARTNAME)
            End If
        End With
    End Sub

    Public Sub SetBasketItemQTY(ByVal PARTNAME As String, ByVal QTY As String)
        With BasketItems
            If .ContainsKey(PARTNAME) Then
                .Item(PARTNAME).QTY = QTY
            End If
        End With
    End Sub

#End Region

#Region "Public Subs (Cart Bindings)"

    Public Sub BindBasket(ByRef Grid As System.Web.UI.WebControls.GridView)

        c.CurrentSession(HttpContext.Current).cart.CartItems.Clear()
        For Each key As String In BasketItems.Keys

            Dim sql As String = "SELECT PARTNAME,PARTDES,QTY,PARTPRICE,AVAILABLE,TAXRATE " & _
                    "FROM dbo.ZWEBFunc_BASKETITEM('" & _
                        BasketItems(key).PARTNAME & "'," & _
                        CInt(BasketItems(key).QTY) & ",'" & _
                        CURRENCY & "','" & _
                        TAXCODE & _
                        "')"

            Dim Data(,) As String = OnlineUsers.sd.DeSerialiseData(OnlineUsers.ws.GetData(sql))
            If Not IsNothing(Data) Then
                For i As Integer = 0 To UBound(Data, 2)
                    With c.CurrentSession(HttpContext.Current).cart.CartItems

                        .Add(Data(0, i), _
                            New CartItem(Data(0, i), _
                            Data(1, i), _
                            Data(3, i), _
                            BasketItems(key).QTY, _
                             Data(5, i), _
                            BasketItems(key).REFERER _
                            ) _
                        )
                    End With
                Next
            End If
        Next

        'Add delivery Part from the cart
        Dim del As String = c.CurrentSession(HttpContext.Current).cart.DELIVERY.DELIVERYPART
        With c.CurrentSession(HttpContext.Current)
            If .cart.DELIVERY.DELIVERYPART.Length = 0 Then
                .cart.DELIVERY = .Basket.DeliveryItems.Item(.Basket.DEFAULTDELIVERY)
            End If
            With .cart
                Dim sql As String = "SELECT PARTNAME,PARTDES,QTY,PARTPRICE,AVAILABLE,TAXRATE " & _
                        "FROM dbo.ZWEBFunc_BASKETITEM('" & _
                            .DELIVERY.DELIVERYPART & "'," & _
                            "1" & ",'" & _
                            CURRENCY & "','" & _
                            TAXCODE & _
                            "')"

                Dim Data(,) As String = OnlineUsers.sd.DeSerialiseData(OnlineUsers.ws.GetData(sql))
                If Not IsNothing(Data) Then
                    For i As Integer = 0 To UBound(Data, 2)
                        With .DELIVERY
                            .DELIVERYPART = Data(0, i)
                            .PARTDES = Data(1, i)
                            .PARTPRICE = Data(3, i)
                            .SALESTAX = Data(5, i)
                            .REFERER = ""
                        End With
                    Next
                End If

                .CartItems.Add(.DELIVERY.PARTNAME, _
                    New CartItem(.DELIVERY.PARTNAME, _
                    .DELIVERY.PARTDES, _
                    .DELIVERY.PARTPRICE, _
                    "1", _
                    .DELIVERY.SALESTAX, _
                    "" _
                    ) _
                )
            End With
        End With

        With Grid
            .DataSource = c.CurrentSession(HttpContext.Current).cart.CartItems.Values
            .DataBind()

            For Each col As GridViewRow In .Rows
                Dim PARTNAME As String = SelectedPart(Grid, col.RowIndex)
                If PARTNAME = c.CurrentSession(HttpContext.Current).cart.DELIVERY.PARTNAME Then
                    For Each ct As Object In col.Cells(7).Controls
                        Try
                            If ct.commandname = "Delete" Then ct.visible = False
                        Catch
                        End Try
                    Next
                End If
                'If col.RowType = DataControlRowType.Footer Then
                '    col.Cells(6).Text = c.CurrentSession(HttpContext.Current).cart.Value & " " & CURRENCY
                'End If
            Next
        End With

    End Sub

    Public Sub AddHandlers(ByRef Grid As System.Web.UI.WebControls.GridView)
        c.CurrentSession(HttpContext.Current).cart.CURRENCY = CURRENCY
        With Grid
            AddHandler .RowDataBound, AddressOf hRowDataBound
            AddHandler .RowDeleting, AddressOf hRowDeleting
            AddHandler .RowEditing, AddressOf hRowEditing
            AddHandler .RowCancelingEdit, AddressOf hRowCancelingEdit
            AddHandler .RowUpdating, AddressOf hRowUpdating
        End With
    End Sub

#End Region

#Region "Basket event Handlers"

    Private Sub hRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim PARTNAME As String = SelectedPart(sender, e.Row.RowIndex)
        With c.CurrentSession(HttpContext.Current).cart
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(6).Text = .Value & " " & .CURRENCY
            End If
        End With

    End Sub

    Private Sub hRowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)

        Dim PARTNAME As String = SelectedPart(sender, e.NewEditIndex)
        Dim Cell As TableCellCollection

        With sender
            'Set the edit index.
            .EditIndex = e.NewEditIndex
        End With

        With c.CurrentSession(HttpContext.Current)
            'Bind data to the GridView control.        
            .Basket.BindBasket(sender)
            Cell = sender.Rows(e.NewEditIndex).Cells()
            If PARTNAME = .cart.DELIVERY.PARTNAME Then
                Cell(2).Controls.Clear()
                Dim ctrl As DropDownList = .Basket.DelOptList
                ctrl.ID = "DELOPT"
                ctrl.SelectedValue = .cart.DELIVERY.DELIVERYPART
                'ctrl.AutoPostBack = True
                'AddHandler ctrl.SelectedIndexChanged, AddressOf hSelectedIndexChanged
                Cell(2).Controls.Add(ctrl)
            Else
                Cell(4).Controls.Clear()
                Dim ctrl As New TextBox()
                ctrl.Text = .Basket.BasketItems(PARTNAME).QTY
                ctrl.ID = "txtQTY"
                ctrl.Width = "25"
                Cell(4).Controls.Add(ctrl)
            End If

        End With
    End Sub

    Private Sub hSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim SelDel As DeliveryItem = DeliveryItems(sender.Items(sender.SelectedIndex).Value)
        With c.CurrentSession(HttpContext.Current).cart.DELIVERY
            .DELIVERYPART = SelDel.DELIVERYPART
            .SALESTAX = SelDel.SALESTAX
            .PARTDES = SelDel.PARTDES
        End With

    End Sub

    Private Sub hRowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)

        Dim PARTNAME As String = SelectedPart(sender, e.RowIndex)
        With sender
            Try
                .Rows(.EditIndex).Cells(4).FindControl("txtQTY").text = BasketItems(PARTNAME).QTY
            Catch ex As Exception

            End Try
        End With


        'Reset the edit index.
        sender.EditIndex = -1
        'Bind data to the GridView control.
        With c.CurrentSession(HttpContext.Current).Basket
            .BindBasket(sender)
        End With
    End Sub

    Private Sub hRowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        Dim PARTNAME As String = SelectedPart(sender, e.RowIndex)
        If Not IsNothing(PARTNAME) Then
            With c.CurrentSession(HttpContext.Current).Basket
                .RemoveBasketItem(PARTNAME)
                .BindBasket(sender)
            End With
        End If
    End Sub

    Protected Sub hRowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

        Dim r As NameValueCollection = c.CurrentSession(HttpContext.Current).Request.Params
        Dim RID As String = Right("00" & CStr(e.RowIndex + 2), 2)
        Dim PARTNAME As String = SelectedPart(sender, e.RowIndex)

        Select Case PARTNAME
            Case "DELIVERY"
                Dim DELPART As String = r("ctl00$ContentPlaceHolder1$Cart$BasketGrid$ctl" & RID & "$DELOPT")
                If Not IsNothing(DELPART) Then
                    Dim SelDel As DeliveryItem = DeliveryItems.Item(DELPART)
                    With c.CurrentSession(HttpContext.Current).cart.DELIVERY
                        .DELIVERYPART = SelDel.DELIVERYPART
                        .SALESTAX = SelDel.SALESTAX
                        .PARTDES = SelDel.PARTDES
                        .PARTPRICE = SelDel.PARTPRICE
                    End With
                End If
            Case Else
                Dim newQTY As String = r("ctl00$ContentPlaceHolder1$Cart$BasketGrid$ctl" & RID & "$txtQTY")
                If Not IsNothing(newQTY) Then
                    With c.CurrentSession(HttpContext.Current).Basket
                        If Not IsNothing(PARTNAME) And Not IsNothing(newQTY) Then
                            .SetBasketItemQTY(PARTNAME, newQTY)
                        End If
                    End With
                End If
        End Select

        'Reset the edit index.
        sender.EditIndex = -1
        'Bind data to the GridView control.
        With c.CurrentSession(HttpContext.Current).Basket
            .BindBasket(sender)
        End With
    End Sub

#End Region

#Region "Private Functions"

    Private Function SelectedPart(ByVal sender As System.Web.UI.WebControls.GridView, ByVal row As Integer) As String
        Dim k As Object = Nothing
        Try
            k = sender.DataKeys(row)
        Catch
            Return Nothing
        End Try
        If Not IsNothing(k) Then
            If k.value.Length Then
                Return k.value
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

#End Region

End Class

Public Class Session

#Region "Initialisation"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal CurrentContext As HttpContext)
        With Me
            .mSessionID = CurrentContext.Session.SessionID
            .mInfo = New SessionInfo(CurrentContext)
            .mUser = CurrentContext.User
            .mProfile = CurrentContext.Profile
            .mRequest = CurrentContext.Request
            .mResponse = CurrentContext.Response
            .mServer = CurrentContext.Server
            .mSession = CurrentContext.Session
            .mNamePairs = New tNamePairs(Debug)
        End With
    End Sub

#End Region

#Region "Public Subs"

    Public Sub Update(ByVal CurrentContext As HttpContext)
        With Me
            .mInfo.ViewingPage = CurrentContext.Request.RawUrl
            .mUser = CurrentContext.User
            .mProfile = CurrentContext.Profile
            .mRequest = CurrentContext.Request
            .mResponse = CurrentContext.Response
            .mServer = CurrentContext.Server
            .mSession = CurrentContext.Session
            .mNamePairs = New tNamePairs(Debug)
        End With
    End Sub

#End Region

#Region "public properties"

    Private mSessionID As String = ""
    Public Property SessionID() As String
        Get
            Return mSessionID
        End Get
        Set(ByVal value As String)
            mSessionID = value
        End Set
    End Property

    Private _debug As Boolean = False
    Public Property Debug() As Boolean
        Get
            Return _debug
        End Get
        Set(ByVal value As Boolean)
            _debug = value
        End Set
    End Property

    Private mCart As New Cart()
    Public Property cart() As Cart
        Get
            Return mCart
        End Get
        Set(ByVal value As Cart)
            mCart = value
        End Set
    End Property

    Private mBasket As New BasketCls
    Public Property Basket() As BasketCls
        Get
            Return mBasket
        End Get
        Set(ByVal value As BasketCls)
            mBasket = value
        End Set
    End Property

    Private mInfo As New SessionInfo
    Public Property Info() As SessionInfo
        Get
            Return mInfo
        End Get
        Set(ByVal value As SessionInfo)
            mInfo = value
        End Set
    End Property

    Private mUser As System.Security.Principal.IPrincipal
    Public Property User() As System.Security.Principal.IPrincipal
        Get
            Return mUser
        End Get
        Set(ByVal value As System.Security.Principal.IPrincipal)
            mUser = value
        End Set
    End Property

    Private mProfile As New System.Web.Profile.ProfileBase
    Public Property Profile() As System.Web.Profile.ProfileBase
        Get
            Return mProfile
        End Get
        Set(ByVal value As System.Web.Profile.ProfileBase)
            mProfile = value
        End Set
    End Property

    Private mNamePairs As tNamePairs = Nothing
    Public ReadOnly Property NamePairs() As tNamePairs
        Get
            If IsNothing(mNamePairs) Then
                mNamePairs = New tNamePairs(Debug)
            End If
            Return mNamePairs
        End Get
    End Property

    Private mRequest As HttpRequest
    Public ReadOnly Property Request() As HttpRequest
        Get
            Return mRequest
        End Get
    End Property

    Private mResponse As HttpResponse
    Public ReadOnly Property Response() As HttpResponse
        Get
            Return mResponse
        End Get
    End Property

    Private mServer As HttpServerUtility
    Public ReadOnly Property Server() As HttpServerUtility
        Get
            Return mServer
        End Get        
    End Property

    Private mSession As HttpSessionState
    Public ReadOnly Property Session() As HttpSessionState
        Get
            Return mSession
        End Get
    End Property

#End Region

End Class

Public Class OnlineUsers

#Region "Public Shared variables"

    Public Shared Sessions As Dictionary(Of String, Session)
    Public Shared ws As New priwebsvc.Service
    Public Shared sd As New Priority.SerialData

#End Region

#Region "Public Subs"

    Public Function CurrentSession(ByVal CurrentContext As HttpContext) As Session
        If IsNothing(Sessions) Then
            Sessions = New Dictionary(Of String, Session)
        End If
        If Not Sessions.ContainsKey(CurrentContext.Session.SessionID) Then
            Sessions.Add(CurrentContext.Session.SessionID, New Session(CurrentContext))
        Else
            Sessions.Item(CurrentContext.Session.SessionID).Update(CurrentContext)
        End If
        Return Sessions.Item(CurrentContext.Session.SessionID)
    End Function

    Public Sub CloseSession(ByVal SessionID As String)
        Sessions.Remove(SessionID)
    End Sub

#End Region

End Class