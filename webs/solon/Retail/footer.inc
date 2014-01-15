
	        </div><!-- EOF content-->
		</div><!-- EOF .page -->

        <hr class="split" />

		<div class="page">
            <footer class="footer group row">
                <nav class="column grid_1 links">
					<h4>Navigation</h4>
                          <asp:ListView ID="listview2" runat="server" DataSourceID="Navigation">
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
                            ID="Navigation" 
                            runat="server" 
                            DataFile="~/cat.xml" 
                            XPath="//cat/cat[@name='topmenu']/cat[@showonmenu='True']">
                        </asp:XmlDataSource>    
                         
				</nav>
				
                <nav class="column grid_1 links">
					<h4>Information</h4>
		                   
                     <asp:ListView ID="ListView1" runat="server" DataSourceID="FooterMenu2">
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
                            ID="FooterMenu2" 
                            runat="server" 
                            DataFile="~/cat.xml" 
                            XPath="//cat/cat[@name='Information']/cat[@showonmenu='True']">
                        </asp:XmlDataSource>   
				
				</nav>
				
                <div class="column grid_1 links">
					<h4>Community</h4>
                    <asp:ListView ID="ListView4" runat="server" DataSourceID="SocialIcons">
                    
                         <LayoutTemplate>
                             <ul>
                             <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                             </ul>
                         </LayoutTemplate>
                     
                         <ItemTemplate>
                             <li><asp:HyperLink runat="server" NavigateUrl='<%# Eval("loc") %>'
                             Text='<%# Eval("pagetitle") %>' 
                             ToolTip='<%# Eval("pagetitle") %>'/></li>
                         </ItemTemplate>
                     
                    </asp:ListView>
  				</div>  				  				
  				
				<asp:LoginView ID="LoginView1" runat="server">
					<RoleGroups>
						<asp:RoleGroup Roles="webmaster">
							<ContentTemplate>
								<div class="column grid_1 links">
									<h4>
										<% Response.Write(string.format("Hi {0}!", Profile.Name.First))%>
									</h4>

									<asp:ListView ID="ListView5" runat="server" DataSourceID="FooterMenu4">
										 <LayoutTemplate>
											 <ul>
											 <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
											 </ul>
										 </LayoutTemplate>
		                                 
										 <ItemTemplate>
											 <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("loc") %>'
											 Text='<%# Eval("menutitle") %>' 
											 ToolTip='<%# Eval("pagetitle") %>'/></li>
										 </ItemTemplate>
		                                 
									</asp:ListView>
									<asp:XmlDataSource 
										ID="FooterMenu4" 
										runat="server" 
										DataFile="~/links.xml" 
										XPath="/pages/page[@menutitle='webmaster']/page">
									</asp:XmlDataSource>
								</div>
							</ContentTemplate>
						</asp:RoleGroup>
					</RoleGroups>
					<LoggedInTemplate>
						<div class="column grid_1 links">
							<h4>
							<% Response.Write(string.format("Hi {0}!", Profile.Name.First))%>
								 
								 
							</h4>

							<asp:ListView ID="ListView5" runat="server" DataSourceID="FooterMenu4">
								 <LayoutTemplate>
									 <ul>
									 <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
									 </ul>
								 </LayoutTemplate>
		                         
								 <ItemTemplate>
									 <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("loc") %>'
									 Text='<%# Eval("menutitle") %>' 
									 ToolTip='<%# Eval("pagetitle") %>'/></li>
								 </ItemTemplate>
		                         
							</asp:ListView>
							<asp:XmlDataSource 
								ID="FooterMenu4" 
								runat="server" 
								DataFile="~/links.xml" 
								XPath="/pages/page[@menutitle='LoggedIn']/page">
							</asp:XmlDataSource>
						</div>
					</LoggedInTemplate>
					<AnonymousTemplate>
						<div class="column grid_1 links">
							<h4>Account</h4>

							<asp:ListView ID="ListView5" runat="server" DataSourceID="FooterMenu4">
								 <LayoutTemplate>
									 <ul>
									 <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
									 </ul>
								 </LayoutTemplate>
		                         
								 <ItemTemplate>
									 <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("loc") %>'
									 Text='<%# Eval("menutitle") %>' 
									 ToolTip='<%# Eval("pagetitle") %>'/></li>
								 </ItemTemplate>
		                         
							</asp:ListView>
							<asp:XmlDataSource 
								ID="FooterMenu4" 
								runat="server" 
								DataFile="~/links.xml" 
								XPath="/pages/page[@menutitle='Anonymous']/page">
							</asp:XmlDataSource>
						</div>                
					</AnonymousTemplate>
				</asp:LoginView>
				
                <div class="column grid_1 copyright">
					<!-- #include file="~/my_documents/include/copyright.inc" -->
				</div>
			</footer><!--EOF footer -->

        </div><!--EOF page -->



        <!--[if lt IE 7]>
            <p class="chromeframe">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> or <a href="http://www.google.com/chromeframe/?redirect=true">activate Google Chrome Frame</a> to improve your experience.</p>
        <![endif]-->

		<script>
		    Modernizr.load([
				{
				    load: '//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js',
				    complete: function() {
				        if (!window.jQuery) {
				            Modernizr.load('js/vendor/jquery-1.9.1.min.js');
				        }
				    }
				},
				{
				    // This will wait for the fallback to load..
				    load: ['js/plugins.js', 'js/main.js','js/vendor/jquery.nivo.slider.pack.js'],
				    complete: function(){
						startSlideshow();
						console.log('loaded');
				    }
				}
			]);
		</script>
        <!-- #include file="analytics.inc" -->

