    <header class="header row group">
      <asp:Button ID="btnDisableEnter" runat="server" Text="" OnClientClick="return false" style="display:none;"/>
        <div class="logo column grid_4">            
            <h1><a href="Default.aspx" title="Home">
              <asp:Image ID="MainLogo" runat="server" />
              <asp:Literal ID="WebName" runat="server"></asp:Literal>
            </a></h1>            
        </div>
        
        <div class="icons column grid_1">
          <a href="basket.aspx" class="cart" title="View the items in your Shopping Basket">
            <asp:Literal ID="BasketCount" runat="server"></asp:Literal><span>Items in shopping basket</span>
          </a>

            <asp:ListView ID="ListView3" runat="server" DataSourceID="SocialIcons">
                <LayoutTemplate>
                    <ul class="social">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </ul>
                </LayoutTemplate>

                <ItemTemplate>
                <li class="<%# Eval("class") %>">
                    <asp:HyperLink ID="HyperLink1" runat="server" 
                        NavigateUrl='<%# Eval("loc") %>'
                        Text='<%# Eval("pagetitle") %>' 
                        ToolTip='<%# Eval("pagetitle") %>'/>
                </li>
                </ItemTemplate>

            </asp:ListView>
            
            <asp:XmlDataSource 
                ID="SocialIcons" 
                runat="server" 
                DataFile="~/links.xml" 
                XPath="/pages/page[@menutitle='Community']/page">
            </asp:XmlDataSource> 

			  </div>

      <!-- Top Menu -->
			<nav class="navigation column grid_4">

        <asp:ListView ID="topmenu" runat="server" DataSourceID="Mainmenu">
         <LayoutTemplate>
             <ul>
             <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
             </ul>
         </LayoutTemplate>
         
         <ItemTemplate>
             <li>
               <asp:HyperLink 
                 ID="HyperLink2" 
                 runat="server"
                 NavigateUrl='<%# Eval("id") %>'
                 Text='<%# Eval("name") %>' 
                 ToolTip='<%# Eval("name") %>'>
               </asp:HyperLink>
             </li>
         </ItemTemplate>
         
        </asp:ListView>

         <asp:XmlDataSource                      
             ID="Mainmenu" 
             runat="server" 
             DataFile="http://www.doorsecuritydirect.co.uk/topmenu.ashx" 
             XPath="//cat/cat[@showonmenu='True']"
             EnableCaching="false">
        </asp:XmlDataSource>             

      </nav>
             
		<div class="search column grid_1">
      <asp:TextBox ID="SearchTerms" runat="server" CssClass="searchbox" placeholder="Search..."/>
      <asp:Button ID="SearchButton" runat="server" CssClass="searchbutton" Text="Go" />
		</div>

		<nav class="breadcrumb column grid_5">

		</nav>

  </header><!--EOF header -->


  <div class="page">
<div class="content group">		
			
			