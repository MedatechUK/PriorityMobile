<%@ Page Language="VB" MasterPageFile="~/membership.master" AutoEventWireup="false" CodeFile="search.aspx.vb" Inherits="search" title="Seach Results" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">                  
        
        <asp:ListView ID="SearchResults" runat="server" DataKeyNames="loc" >
            <LayoutTemplate>    
                <br />
                
                <asp:DataPager ID="DataPager1" runat="server" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
                
                <br/>
                <br/>
                <table cellpadding="10" width="640">    
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"/>   
                </table>   
                
                <br/>  		             
                
                <asp:DataPager ID="DataPager2" runat="server" PageSize="5" >
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
                
                <br/>
                <br/>
                
            </LayoutTemplate>

            <EmptyDataTemplate>
                <h2>Sorry ...</h2>
                <p>...but we couldn't find any matching pages.</p>
            </EmptyDataTemplate>
            
            <ItemTemplate>                    
                <tr>
                    <td align="center" width="110" style="vertical-align:middle">
                        <a href='<%# Eval("loc") %>'> 
                            <img src='<%# Eval("img") %>' alt='<%# Eval("description") %>' width='100'/>
                        </a>
                    </td>
                    <td width="20">&nbsp;</td>
                    <td align="left" valign="top" style="vertical-align:top">
                        <h2>
                            <a href='<%# Eval("loc") %>'> 
                                <%# Eval("pagetitle") %>
                            </a>
                        </h2>                
                        <p>
                            <asp:Label ID="descriptionLabel" runat="server" Text='<%# Eval("description") %>' />
                        </p>   
                     </td>
                </tr>                    
            </ItemTemplate>
            
            <AlternatingItemTemplate>
                <tr style="background-color:#ebebeb">
                    <td align="center" width="110" style="vertical-align:middle">
                        <a href='<%# Eval("loc") %>'> 
                            <img src='<%# Eval("img") %>' alt='<%# Eval("description") %>' width='100'/>
                        </a>
                    </td>
                    <td width="20">&nbsp;</td>
                    <td align="left" valign="top" style="vertical-align:top">
                        <h2>
                            <a href='<%# Eval("loc") %>'> 
                                <%# Eval("pagetitle") %>
                            </a>
                        </h2>                
                        <p>
                            <asp:Label ID="descriptionLabel" runat="server" Text='<%# Eval("description") %>' />
                        </p>   
                     </td>
                </tr>            
            </AlternatingItemTemplate>
                        
        </asp:ListView>        
    </asp:Content>


