<aside class="sidebar column grid_1">
	<section class="cart">
    <h3>Shopping Basket</h3>
		<ul>
			<li><a href="basket.aspx">View Your Basket (<asp:Literal ID="BasketCount2" runat="server"></asp:Literal>)</a></li>
		</ul>
	</section>

	<section>
		<h3>Products By Category</h3>

        <asp:ListView ID="CategoryList" runat="server" DataSourceID="Category">
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
                        ToolTip='<%# Eval("name") %>'/>
                </li>
            </ItemTemplate>

        </asp:ListView>

         <asp:XmlDataSource 
            ID="Category" 
            runat="server" 
            DataFile="~/cat.xml" 
            XPath="//cat/cat[@id='ce1d8dcd-2011-4d6f-b8fc-f83948ecaf9e']/cat[@showonmenu='True']">
        </asp:XmlDataSource> 

	</section>

	<section>
		<h3>Security Advice</h3>

        <asp:ListView ID="SIDEBAR_MENU_STYLE_3" runat="server" DataSourceID="Advice">
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
                        ToolTip='<%# Eval("name") %>'/>
                </li>
            </ItemTemplate>

        </asp:ListView>

         <asp:XmlDataSource 
            ID="Advice" 
            runat="server" 
            DataFile="~/cat.xml" 
            XPath="//cat/cat[@id='91262cf9-2be4-4f57-b132-356c332ee1af']/cat[@showonmenu='True']">
        </asp:XmlDataSource> 
 
	</section>

</aside>
                <!-- EOF sidebar -->