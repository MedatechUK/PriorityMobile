<%@ Control Language="VB" AutoEventWireup="false" ClassName="membershipManager" CodeFile="UserManager.ascx.vb" Inherits="UserControls_membershipManager" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>

<div style="background-color: #e0e0e0">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" skin="WinVista" MultiPageID="RadMultiPage1" SelectedIndex="0">
        <Tabs>
            <radTS:Tab Text="User Manager"></radTS:Tab>
            <radTS:Tab Text="Role Manager"></radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
</div>
<div style="border-top:3px solid #9999bb">

<radTS:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
    <radTS:PageView ID="PageView1" runat="server">
    <asp:Panel ID="DetailsPanel" runat="server">    
        <radG:RadGrid ID="UserGrid" runat="server" GridLines="None"
                      Skin="WebBlue" 
                      EnableAJAX="true" 
                      ShowFooter="true" 
                      ShowStatusBar="true" 
                      AllowSorting="true" 
                      AutoGenerateColumns="false" 
                      ShowGroupPanel="true" 
                      AllowFilteringByColumn="true"  
                      GroupingSettings-CaseSensitive="false" 
                      ClientSettings-EnablePostBackOnRowClick="false" 
                      GroupingEnabled="true"
                      >
            <MasterTableView Name="UserTable" 
                                AllowMultiColumnSorting="False" 
                                Width= "100%" 
                                AllowFilteringByColumn="true" 
                                DataKeyNames="Username"
                                PagerStyle-Mode="NextPrevAndNumeric"
                                AllowPaging="true"
                                AllowCustomSorting="false"
                                AllowNaturalSort="false"
                                PageSize="10">
            <DetailTables>
                <radG:GridTableView Name="RoleGrid"
                                    AllowFilteringByColumn="false" 
                                    ShowHeader="true" 
                                    ShowFooter="false" 
                                    Frame="Border" 
                                    AllowSorting="true" 
                                    HierarchyLoadMode="ServerOnDemand" 
                                    CommandItemSettings-AddNewRecordText="Add To Role"
                                    EditMode="inPlace" 
                                    CommandItemDisplay="bottom" 
                                    DataKeyNames="RoleName"
                                    AllowAutomaticInserts="true" 
                                    AllowAutomaticDeletes="true" 
                                    AllowAutomaticUpdates="true" >
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="RowNumColumn" HeaderText="" AllowFiltering="false" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Label ID="numberLabel" runat="server" Width="25px" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </radG:GridTemplateColumn>      

                        <radG:GridTemplateColumn UniqueName="RoleName" HeaderText="Role Name" SortExpression="RoleName" DataField="RoleName" GroupByExpression="RoleName Group By RoleName">
                            <HeaderStyle Width="250px" />
                            <ItemTemplate>
                                <asp:Label id="RoleNameLabel" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "RoleName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <radC:RadComboBox ID="RoleComboBox" runat="server" 
                                                  Skin="ClassicBlue" 
                                                  DataTextField="RoleName" 
                                                  DataSourceID="SqlDataSource2" 
                                                  DataValueField="RoleName" 
                                                  AutoPostBack="false"
                                                  Width="250px">
                            </radC:RadComboBox>
                            </EditItemTemplate>
                        </radG:GridTemplateColumn>

                  <radG:GridEditCommandColumn FilterImageUrl="" SortAscImageUrl="" SortDescImageUrl="" ButtonType="ImageButton" ItemStyle-CssClass="imgButton">
                    <ItemStyle HorizontalAlign="Center" Width="40px"/>
                    </radG:GridEditCommandColumn>

                    <radG:GridButtonColumn CommandName="Delete" Text="Remove Role" UniqueName="column" ButtonType="ImageButton" ConfirmText="Are you sure you want to remove the user from this role?" ItemStyle-CssClass="imgButton">
                        <HeaderStyle Width="20px" />
                    </radG:GridButtonColumn>
                    </Columns>
                </radG:GridTableView>
            </DetailTables>
                <Columns>
                        <radG:GridTemplateColumn UniqueName="RowNumColumn" HeaderText="" AllowFiltering="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="25px" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </radG:GridTemplateColumn>                
                        <radG:GridBoundColumn SortExpression="Firstname" HeaderText="Firstname" AllowFiltering="true" HeaderButtonType="TextButton"
                            DataField= "Firstname" UniqueName="Firstname" ItemStyle-Width="75" Groupable="true"/>

                        <radG:GridBoundColumn SortExpression="Lastname" HeaderText="Lastname" HeaderButtonType="TextButton"
                            DataField= "Lastname" UniqueName="Lastname" ItemStyle-Width="75" Groupable="true"/>

                        <radG:GridBoundColumn SortExpression="Company" HeaderText="Company" HeaderButtonType="TextButton"
                            DataField= "Company" UniqueName="Company" ItemStyle-Width="100" Groupable="true"/>

                        <radG:GridBoundColumn SortExpression="Username" HeaderText="Username" HeaderButtonType="TextButton"
                            DataField= "Username" UniqueName="Username" ItemStyle-Width="100" />

                        <radG:GridBoundColumn SortExpression="Email" HeaderText="Email" HeaderButtonType="TextButton"
                            DataField= "Email" UniqueName="Email" ItemStyle-Width="175" />

                        <radG:GridBoundColumn SortExpression="LastLoginDate" HeaderText="Last Login" HeaderButtonType="TextButton"
                            DataField= "LastLoginDate" UniqueName="LastLoginDate"  ItemStyle-Width="120" />

                    <radG:GridButtonColumn CommandName="Delete" Text="Delete User" UniqueName="column" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete this user?" ItemStyle-CssClass="imgButton">
                        <HeaderStyle Width="20px" />
                    </radG:GridButtonColumn>
                            
                </Columns>
                <PagerStyle AlwaysVisible="True" />
            </MasterTableView>
                <ClientSettings AllowColumnsReorder="true" 
                                AllowDragToGroup="true" 
                                AllowKeyboardNavigation="true"
                                AllowColumnHide="True" 
                                AllowExpandCollapse="True" 
                                AllowGroupExpandCollapse="True" >
                    <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true"  />
                </ClientSettings>            
        </radG:RadGrid>
    </asp:Panel>
  
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Members %>"
        SelectCommand="SELECT [RoleName] FROM [vw_aspnet_Roles] ORDER BY [RoleName]"></asp:SqlDataSource>
    </radTS:PageView>





    <radTS:PageView ID="PageView2" runat="server" Width="100%">
        <asp:Panel ID="RolesPanel" runat="server">
        <radG:RadGrid ID="RoleAdminGrid" runat="server"
                          EnableAJAX="true" 
                          GridLines="None" 
                          DataSourceID="SqlDataSource1"
                          Skin="WebBlue" 
                          ShowFooter="True" 
                          ShowStatusBar="True" 
                          AllowSorting="True" 
                          AutoGenerateColumns="False" 
                          AllowPaging="True"
                          AllowFilteringByColumn="True"  
                          GroupingSettings-CaseSensitive="false" 
                          ClientSettings-EnablePostBackOnRowClick="false">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView name="RolesTable" DataSourceID="SqlDataSource1"
                             Width= "100%" 
                             DataKeyNames="RoleName"
                             AllowAutomaticInserts="True" 
                             AllowAutomaticDeletes="True" 
                             AllowAutomaticUpdates="True"
                             EditMode="InPlace" 
                             CommandItemDisplay="Top" 
                             >
                <DetailTables>
                <radG:GridTableView Name="UsersinRolesTable"
                                AllowMultiColumnSorting="false" 
                                AllowFilteringByColumn="true" 
                                DataKeyNames="Username"
                                PagerStyle-Mode="NextPrevAndNumeric"
                                AllowPaging="true"
                                AllowCustomSorting="false"
                                AllowNaturalSort="false"
                                PageSize="10" 
                                PagerStyle-AlwaysVisible="true" 
                                ShowHeader="true"
                                >
                    <Columns>
                        <radG:GridBoundColumn SortExpression="Firstname" HeaderText="Firstname" AllowFiltering="true" HeaderButtonType="TextButton"
                            DataField= "Firstname" UniqueName="Firstname" ItemStyle-Width="75" Groupable="true"/>

                        <radG:GridBoundColumn SortExpression="Lastname" HeaderText="Lastname" HeaderButtonType="TextButton"
                            DataField= "Lastname" UniqueName="Lastname" ItemStyle-Width="75" Groupable="true"/>

                        <radG:GridBoundColumn SortExpression="Company" HeaderText="Company" HeaderButtonType="TextButton"
                            DataField= "Company" UniqueName="Company" ItemStyle-Width="100" Groupable="true"/>

                        <radG:GridBoundColumn SortExpression="Username" HeaderText="Username" HeaderButtonType="TextButton"
                            DataField= "Username" UniqueName="Username" ItemStyle-Width="100" />

                        <radG:GridBoundColumn SortExpression="Email" HeaderText="Email" HeaderButtonType="TextButton"
                            DataField= "Email" UniqueName="Email" ItemStyle-Width="175" />

                        <radG:GridBoundColumn SortExpression="LastLoginDate" HeaderText="Last Login" HeaderButtonType="TextButton"
                            DataField= "LastLoginDate" UniqueName="LastLoginDate"  ItemStyle-Width="120" />

                    <radG:GridButtonColumn CommandName="Delete" Text="Remove from Role" UniqueName="column" ButtonType="ImageButton" ConfirmText="Are you sure you want to remove the user? \r(Will not delete the user - only remove them from the role)" ItemStyle-CssClass="imgButton">
                        <HeaderStyle Width="20px" />
                    </radG:GridButtonColumn>
                    </Columns>
                </radG:GridTableView>
                </DetailTables>
                <Columns>
                    <radG:GridBoundColumn SortExpression="RoleName" HeaderText="Roles" HeaderButtonType="TextButton"
                        DataField= "RoleName" UniqueName="RoleName">
                        <ItemStyle Width="100%" />
                    </radG:GridBoundColumn>

                    <radG:GridEditCommandColumn ButtonType="ImageButton" display="false">
                        <HeaderStyle Width="40px" />
                        <ItemStyle HorizontalAlign="Center" Width="40px" CssClass="imgButton"/>
                    </radG:GridEditCommandColumn>

                    <radG:GridButtonColumn CommandName="Delete" Text="Delete Role" UniqueName="column" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete this role?">
                        <HeaderStyle Width="20px" />
                        <ItemStyle CssClass="imgButton" />
                    </radG:GridButtonColumn>
                </Columns>
                <CommandItemSettings AddNewRecordText="Add New Role" />
                <ExpandCollapseColumn Visible="False">
                    <HeaderStyle Width="19px" />
                </ExpandCollapseColumn>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
            </MasterTableView>
            <GroupingSettings CaseSensitive="False" />
            <PagerStyle Mode="Advanced" />
        </radG:RadGrid></asp:Panel>    
 
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Members %>"
            SelectCommand="SELECT [RoleName] FROM [vw_aspnet_Roles] ORDER BY [RoleName]"></asp:SqlDataSource>

    </radTS:PageView>
</radTS:RadMultiPage>
</div>