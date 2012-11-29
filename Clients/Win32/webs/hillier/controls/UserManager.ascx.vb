Imports Telerik.WebControls
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class UserControls_membershipManager
    Inherits System.Web.UI.UserControl

    Protected grid As RadGrid

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
    End Sub
    Private Function GetSQLData(ByVal query As String) As DataTable
        Dim dbCon As New SqlConnection(ConfigurationManager.ConnectionStrings("Members").ConnectionString)
        dbCon.Open()

        Dim adapter As New SqlDataAdapter(query, dbCon)
        Dim dt As New DataTable()
        adapter.Fill(dt)
        dbCon.Close()

        Return dt
    End Function 'GetSQLData


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim LastLoginDateComparer As New UserDataRow.LastLoginDateComparer(False)
            Dim expression As New GridSortExpression()
            expression.FieldName = "LastLoginDate"
            expression.SortOrder = GridSortOrder.Descending
            UserGrid.MasterTableView.SortExpressions.AddSortExpression(expression)
            UserGrid.Rebind()
        End If
    End Sub


    Protected Sub UserGrid_DeleteCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridCommandEventArgs) Handles UserGrid.DeleteCommand
        If e.CommandName = RadGrid.DeleteCommandName AndAlso e.Item.OwnerTableView.Name = "RoleGrid" Then
            Dim Username As String = e.Item.OwnerTableView.ParentItem("Username").Text

            Dim RoleItem As GridDataItem = e.Item
            Dim RoleName As String = RoleItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("RoleName").ToString

            Roles.RemoveUserFromRole(Username, RoleName)
        End If
        If e.CommandName = RadGrid.DeleteCommandName AndAlso e.Item.OwnerTableView.Name = "UserTable" Then
            Dim UserItem As GridDataItem = e.Item
            Dim Username As String = UserItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Username").ToString

            Membership.DeleteUser(Username)
        End If
    End Sub

    Protected Sub UserGrid_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.WebControls.GridDetailTableDataBindEventArgs) Handles UserGrid.DetailTableDataBind
        Dim dataItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim Username As String = e.DetailTableView.ParentItem("Username").Text

        Dim sql As String = "SELECT aspnet_Roles.RoleName FROM aspnet_UsersInRoles INNER JOIN aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId INNER JOIN aspnet_Users ON aspnet_UsersInRoles.UserId = aspnet_Users.UserId WHERE aspnet_Users.UserName = '{0}' ORDER BY RoleName"
        sql = [String].Format(sql, Username)

        Dim RoleTable As DataTable = GetSQLData(sql)
        e.DetailTableView.DataSource = RoleTable
    End Sub

    Protected Sub UserGrid_InsertCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridCommandEventArgs) Handles UserGrid.InsertCommand
        If e.CommandName = RadGrid.PerformInsertCommandName Then
            e.Canceled = True
            Dim Username As String = e.Item.OwnerTableView.ParentItem("Username").Text
            Dim box As RadComboBox = e.Item.FindControl("RoleComboBox")
            Roles.AddUserToRole(Username, box.SelectedItem.Text)
            e.Item.Edit = False
            e.Item.OwnerTableView.Rebind()
        End If
    End Sub


    Protected Sub UserGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.WebControls.GridItemEventArgs) Handles UserGrid.ItemDataBound
        Try
            If (TypeOf e.Item Is GridDataItem) Then
                Dim lbl As Label = CType(e.Item.FindControl("numberLabel"), Label)
                lbl.Text = e.Item.ItemIndex + 1
            End If
            If e.Item.IsInEditMode = True Then
                Dim EditItem As GridEditableItem = CType(e.Item, GridEditableItem)

                Dim Username As String = e.Item.OwnerTableView.ParentItem("Username").Text
                Dim box As RadComboBox = EditItem.FindControl("RoleComboBox")

                Dim sql As String = "SELECT aspnet_Roles.RoleName FROM aspnet_UsersInRoles INNER JOIN aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId INNER JOIN aspnet_Users ON aspnet_UsersInRoles.UserId = aspnet_Users.UserId WHERE aspnet_Users.UserName = '{0}' ORDER BY RoleName"
                sql = [String].Format(sql, Username)
                Dim dt As DataTable = GetSQLData(sql)
                For Each role As DataRow In dt.Rows
                    box.FindItemByText(role("RoleName")).Remove()
                Next
            End If
        Catch
        End Try
    End Sub

    Protected Sub UserGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.WebControls.GridNeedDataSourceEventArgs) Handles UserGrid.NeedDataSource
        If Not e.IsFromDetailTable Then
            Dim UserTable As ArrayList = GetUserTable()
            Me.grid = UserGrid
            Me.grid.DataSource = UserTable
        End If
    End Sub

    Private Function GetUserTable()
        Dim Users As MembershipUserCollection = Membership.GetAllUsers()
        Dim UserTable As New ArrayList

        For Each item As MembershipUser In Users
            Dim UserRow As New UserDataRow()
            'UserRow.Firstname = Profile.GetProfile(item.UserName).Firstname
            'UserRow.Lastname = Profile.GetProfile(item.UserName).Lastname
            'UserRow.Company = Profile.GetProfile(item.UserName).Company
            UserRow.Username = item.UserName
            UserRow.Email = item.Email
            UserRow.LastLoginDate = item.LastLoginDate
            UserTable.Add(UserRow)
        Next

        Return UserTable
    End Function

    Private Function GetUsersByRole(ByVal RoleName As String)
        Dim Users As Array = Roles.GetUsersInRole(RoleName)
        Dim UserTable As New ArrayList

        For Each item As String In Users
            Dim UserRow As New UserDataRow()
            'UserRow.Firstname = Profile.GetProfile(item).Firstname
            'UserRow.Lastname = Profile.GetProfile(item).Lastname
            'UserRow.Company = Profile.GetProfile(item).Company
            UserRow.Username = item
            UserRow.Email = Membership.GetUser(item).Email
            UserRow.LastLoginDate = Membership.GetUser(item).LastLoginDate
            UserTable.Add(UserRow)
        Next

        Return UserTable
    End Function

    Protected Sub UserGrid_SortCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridSortCommandEventArgs) Handles UserGrid.SortCommand
        If e.CommandName = RadGrid.SortCommandName Then
            Dim UserTable As ArrayList = GetUserTable()
            If e.CommandArgument = "Firstname" Then
                Select Case e.NewSortOrder
                    Case GridSortOrder.None
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Ascending
                        Dim FirstnameComparer As New UserDataRow.FirstnameComparer(False)
                        UserTable.Sort(FirstnameComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Descending
                        Dim FirstnameComparer As New UserDataRow.FirstnameComparer(True)
                        UserTable.Sort(FirstnameComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                End Select
            ElseIf e.CommandArgument = "Lastname" Then
                Select Case e.NewSortOrder
                    Case GridSortOrder.None
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Ascending
                        Dim LastnameComparer As New UserDataRow.LastnameComparer(False)
                        UserTable.Sort(LastnameComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Descending
                        Dim LastnameComparer As New UserDataRow.LastnameComparer(True)
                        UserTable.Sort(LastnameComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                End Select
            ElseIf e.CommandArgument = "Company" Then
                Select Case e.NewSortOrder
                    Case GridSortOrder.None
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Ascending
                        Dim CompanyComparer As New UserDataRow.CompanyComparer(False)
                        UserTable.Sort(CompanyComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Descending
                        Dim CompanyComparer As New UserDataRow.CompanyComparer(True)
                        UserTable.Sort(CompanyComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                End Select
            ElseIf e.CommandArgument = "Username" Then
                Select Case e.NewSortOrder
                    Case GridSortOrder.None
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Ascending
                        Dim UsernameComparer As New UserDataRow.UsernameComparer(False)
                        UserTable.Sort(UsernameComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Descending
                        Dim UsernameComparer As New UserDataRow.UsernameComparer(True)
                        UserTable.Sort(UsernameComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                End Select
            ElseIf e.CommandArgument = "Email" Then
                Select Case e.NewSortOrder
                    Case GridSortOrder.None
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Ascending
                        Dim EmailComparer As New UserDataRow.EmailComparer(False)
                        UserTable.Sort(EmailComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Descending
                        Dim EmailComparer As New UserDataRow.EmailComparer(True)
                        UserTable.Sort(EmailComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                End Select
            ElseIf e.CommandArgument = "LastLoginDate" Then
                Select Case e.NewSortOrder
                    Case GridSortOrder.None
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Ascending
                        Dim LastLoginDateComparer As New UserDataRow.LastLoginDateComparer(False)
                        UserTable.Sort(LastLoginDateComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                    Case GridSortOrder.Descending
                        Dim LastLoginDateComparer As New UserDataRow.LastLoginDateComparer(True)
                        UserTable.Sort(LastLoginDateComparer)
                        e.Item.OwnerTableView.DataSource = UserTable
                        e.Item.OwnerTableView.Rebind()
                        Exit Select
                End Select
            End If

        End If
    End Sub


    Protected Sub UserGrid_UpdateCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridCommandEventArgs) Handles UserGrid.UpdateCommand
        If e.CommandName = RadGrid.UpdateCommandName Then
            e.Canceled = True
            Dim Username As String = e.Item.OwnerTableView.ParentItem("Username").Text
            Dim box As RadComboBox = e.Item.FindControl("RoleComboBox")
            Try
                Dim RoleItem As GridDataItem = e.Item
                Dim RoleName As String = RoleItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("RoleName").ToString
                Roles.RemoveUserFromRole(Username, RoleName)
            Catch ex As Exception
            End Try
            Roles.AddUserToRole(Username, box.SelectedItem.Text)
            e.Item.Edit = False

            e.Canceled = True
            e.Item.OwnerTableView.DataBind()
        End If
    End Sub

    Protected Sub RoleAdminGrid_DeleteCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridCommandEventArgs) Handles RoleAdminGrid.DeleteCommand
        If e.CommandName = RadGrid.DeleteCommandName AndAlso e.Item.OwnerTableView.Name = "RolesTable" Then
            Dim RoleItem As GridDataItem = e.Item
            Dim RoleName As String = RoleItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("RoleName").ToString
            Roles.DeleteRole(RoleName)
        End If

        If e.CommandName = RadGrid.DeleteCommandName AndAlso e.Item.OwnerTableView.Name = "UsersinRolesTable" Then
            Dim UserItem As GridDataItem = e.Item
            Dim Username As String = UserItem.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("Username").ToString
            Dim RoleName As String = e.Item.OwnerTableView.ParentItem("RoleName").Text
            Roles.RemoveUserFromRole(Username, RoleName)
        End If
    End Sub

    Protected Sub RoleAdminGrid_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.WebControls.GridDetailTableDataBindEventArgs) Handles RoleAdminGrid.DetailTableDataBind
        Dim dataItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim RoleName As String = dataItem.GetDataKeyValue("RoleName").ToString()
        Dim UserTable As ArrayList = GetUsersByRole(RoleName)
        If Not e.DetailTableView.ItemsCreated Then
            Dim expression As New GridSortExpression()
            expression.FieldName = "LastLoginDate"
            expression.SortOrder = GridSortOrder.Descending
            e.DetailTableView.SortExpressions.AddSortExpression(expression)
        End If
        e.DetailTableView.DataSource = UserTable
    End Sub

    Protected Sub RoleAdminGrid_InsertCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridCommandEventArgs) Handles RoleAdminGrid.InsertCommand
        If e.CommandName = RadGrid.PerformInsertCommandName Then
            e.Canceled = True
            e.Item.Edit = False
            Dim RoleItem As GridDataItem = e.Item

            Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
            Dim Rolename As String

            Dim newValues As Hashtable = New Hashtable
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
            Rolename = newValues.Item("RoleName").ToString

            Roles.CreateRole(Rolename)
            e.Canceled = True
            e.Item.OwnerTableView.Rebind()
        End If
    End Sub

    Protected Sub RoleAdminGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.WebControls.GridCommandEventArgs) Handles RoleAdminGrid.ItemCommand
        If e.CommandName = RadGrid.EditCommandName Then
            e.Canceled = True
        End If
        If e.CommandName = RadGrid.InitInsertCommandName Then
            Dim editcolumn As GridEditCommandColumn = e.Item.OwnerTableView.GetColumn("EditCommandColumn")
            editcolumn.Display = True
        ElseIf e.CommandName = RadGrid.CancelCommandName Then
            Dim editcolumn As GridEditCommandColumn = e.Item.OwnerTableView.GetColumn("EditCommandColumn")
            editcolumn.Display = False
        End If
    End Sub

End Class

Public Class UserDataRow
    Implements IComparable

    Protected _Firstname As String = String.Empty
    Protected _Lastname As String = String.Empty
    Protected _Company As String = String.Empty
    Protected _Username As String = String.Empty
    Protected _Email As String = String.Empty
    Protected _LastLoginDate As DateTime

    Public Sub UserDataRow()

    End Sub

    Property Firstname() As String
        Get
            Return _Firstname
        End Get
        Set(ByVal value As String)
            _Firstname = value
        End Set
    End Property

    Property Lastname() As String
        Get
            Return _Lastname
        End Get
        Set(ByVal value As String)
            _Lastname = value
        End Set
    End Property

    Property Company() As String
        Get
            Return _Company
        End Get
        Set(ByVal value As String)
            _Company = value
        End Set
    End Property

    Property Username() As String
        Get
            Return _Username
        End Get
        Set(ByVal value As String)
            _Username = value
        End Set
    End Property

    Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property
    Property LastLoginDate() As DateTime
        Get
            Return _LastLoginDate
        End Get
        Set(ByVal value As DateTime)
            _LastLoginDate = value
        End Set
    End Property

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If TypeOf obj Is UserDataRow Then
            Dim UserRow As UserDataRow = CType(obj, UserDataRow)

            Return _LastLoginDate.CompareTo(UserRow._LastLoginDate)
        End If

        Throw New ArgumentException("object is not a UserDataRow")
    End Function

    Public Class FirstnameComparer
        Implements IComparer

        Private m_Reverse As Boolean
        Public Sub New(ByVal reverse As Boolean)
            m_Reverse = reverse
        End Sub
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim userX As UserDataRow = DirectCast(x, UserDataRow)
            Dim userY As UserDataRow = DirectCast(y, UserDataRow)

            If m_Reverse Then
                Return String.Compare(userY.Firstname, userX.Firstname)
            Else
                Return String.Compare(userX.Firstname, userY.Firstname)
            End If

        End Function
    End Class

    Public Class LastnameComparer
        Implements IComparer

        Private m_Reverse As Boolean
        Public Sub New(ByVal reverse As Boolean)
            m_Reverse = reverse
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim userX As UserDataRow = DirectCast(x, UserDataRow)
            Dim userY As UserDataRow = DirectCast(y, UserDataRow)

            If m_Reverse Then
                Return String.Compare(userY.Lastname, userX.Lastname)
            Else
                Return String.Compare(userX.Lastname, userY.Lastname)
            End If
        End Function
    End Class

    Public Class CompanyComparer
        Implements IComparer

        Private m_Reverse As Boolean
        Public Sub New(ByVal reverse As Boolean)
            m_Reverse = reverse
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim userX As UserDataRow = DirectCast(x, UserDataRow)
            Dim userY As UserDataRow = DirectCast(y, UserDataRow)

            If m_Reverse Then
                Return String.Compare(userY.Company, userX.Company)
            Else
                Return String.Compare(userX.Company, userY.Company)
            End If
        End Function
    End Class

    Public Class UsernameComparer
        Implements IComparer

        Private m_Reverse As Boolean
        Public Sub New(ByVal reverse As Boolean)
            m_Reverse = reverse
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim userX As UserDataRow = DirectCast(x, UserDataRow)
            Dim userY As UserDataRow = DirectCast(y, UserDataRow)

            If m_Reverse Then
                Return String.Compare(userY.Username, userX.Username)
            Else
                Return String.Compare(userX.Username, userY.Username)
            End If
        End Function
    End Class

    Public Class EmailComparer
        Implements IComparer

        Private m_Reverse As Boolean
        Public Sub New(ByVal reverse As Boolean)
            m_Reverse = reverse
        End Sub
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim userX As UserDataRow = DirectCast(x, UserDataRow)
            Dim userY As UserDataRow = DirectCast(y, UserDataRow)

            If m_Reverse Then
                Return String.Compare(userY.Email, userX.Email)
            Else
                Return String.Compare(userX.Email, userY.Email)
            End If
        End Function
    End Class

    Public Class LastLoginDateComparer
        Implements IComparer

        Private m_Reverse As Boolean
        Public Sub New(ByVal reverse As Boolean)
            m_Reverse = reverse
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim userX As UserDataRow = DirectCast(x, UserDataRow)
            Dim userY As UserDataRow = DirectCast(y, UserDataRow)

            If m_Reverse Then
                Return DateTime.Compare(userY.LastLoginDate, userX.LastLoginDate)
            Else
                Return DateTime.Compare(userX.LastLoginDate, userY.LastLoginDate)
            End If
        End Function
    End Class

End Class
