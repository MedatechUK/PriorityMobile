<aside class="sidebar column grid_1">
	<section class="cart">
    <h3>Shopping Basket</h3>
		<ul>
			<li><a href="basket.aspx">View Your Basket (<asp:Literal ID="BasketCount2" runat="server"></asp:Literal>)</a></li>
		</ul>
	</section>

  <asp:xmldatasource
              ID="rpt2"
              runat="server"
              datafile="~/cat.xml"
              xpath='cat/cat[@name="sidemenus"]/cat[@showonmenu="True"]'>
  </asp:xmldatasource>
  <asp:Repeater ID="rpt" runat="server" datasourceid="rpt2">
    <itemtemplate>
      <section>
        <h3>
          <%#Eval("name")%>                   
            </h3>
        <asp:xmldatasource
            ID="rpt3"
            runat="server"
            datafile="~/cat.xml"
            xpath="cat/cat[@showonmenu='True']/cat[@showonmenu='True']">
        </asp:xmldatasource>

        <asp:ListView ID="SIDEBAR_MENU_STYLE_3" runat="server" DataSource='<%# Xpathselect("cat[@showonmenu=""True""]") %>' >
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
                  NavigateUrl='<%# Xpath("@id") %>'
                            Text='<%# xpath("@name") %>' 
                            ToolTip='<%# Eval("name") %>'/>
            </li>
          </ItemTemplate>

        </asp:ListView>
      </section>
    </itemtemplate>
  </asp:Repeater>
	<!--<section>
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
            XPath="//cat/cat[@id='products-by-category']/cat[@showonmenu='True']">
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
  <section>
    <h3>Hot Tips</h3>

    <asp:ListView ID="HotTips" runat="server" DataSourceID="HotTips1">
      <LayoutTemplate>
        <ul>
          <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </ul>
      </LayoutTemplate>

      <ItemTemplate>
        <li>
          <asp:HyperLink
              ID="HotTips"
              runat="server"
              NavigateUrl='<%# Eval("id") %>'
              Text='<%# Eval("name") %>' 
              ToolTip='<%# Eval("name") %>'
              />
        </li>
      </ItemTemplate>

    </asp:ListView>

    <asp:XmlDataSource
       ID="HotTips1"
       runat="server"
       DataFile="~/cat.xml"
       XPath="//cat/cat[@id='53d4e203-6d31-4bc3-87d0-a67f0cf8bfa1']/cat[@showonmenu='True']">
    </asp:XmlDataSource>

  </section>-->

</aside>
                <!-- EOF sidebar -->