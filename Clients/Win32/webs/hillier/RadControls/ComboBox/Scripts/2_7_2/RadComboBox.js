if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_1,_2,_3){
if(!_3){
return;
}
if(!_1){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_3+"' />");
}else{
var _4=document.createElement("LINK");
_4.rel="stylesheet";
_4.type="text/css";
_4.href=_3;
document.getElementById(_2+"StyleSheetHolder").appendChild(_4);
}
};
function RadComboItem(){
this.ComboBox=null;
this.ClientID=null;
this.Highlighted=false;
this.Index=0;
this.Enabled=1;
this.Selected=0;
this.Text="";
this.Value=null;
this.Attributes=new Array();
}
RadComboItem.prototype.Initialize=function(_5){
for(var _6 in _5){
this[_6]=_5[_6];
}
};
RadComboItem.prototype.AdjustDownScroll=function(){
var _7=0;
var _8=document.getElementById(this.ComboBox.ClientID+"_DropDown");
if(_8.offsetWidth!=_8.scrollWidth+16){
_7=16;
}
if(this.ComboBox.Items.length>0){
var _9=0;
for(var i=0;i<=this.Index;i++){
var _b=document.getElementById(this.ComboBox.Items[i].ClientID);
if(_b){
_9+=_b.offsetHeight;
}
}
_8.scrollTop=_9-_8.offsetHeight+_7;
}
};
RadComboItem.prototype.AdjustUpScroll=function(){
if(this.ComboBox.Items.length>0){
var _c=0;
for(var i=0;i<this.Index;i++){
var _e=document.getElementById(this.ComboBox.Items[i].ClientID);
if(_e){
_c+=_e.offsetHeight;
}
}
var _f=document.getElementById(this.ComboBox.ClientID+"_DropDown").scrollTop;
if(_f>_c){
document.getElementById(this.ComboBox.ClientID+"_DropDown").scrollTop=_c;
}
}
};
RadComboItem.prototype.Highlight=function(){
if(!this.Highlighted&&this.Enabled){
this.ComboBox.UnHighlightAll();
if(!this.ComboBox.IsTemplated||this.ComboBox.HighlightTemplatedItems){
var _10=document.getElementById(this.ClientID);
if(_10){
if(!this.ComboBox.HighlightedItem){
if(_10.className!=this.ComboBox.ItemCssClassHover){
this.CssClass=_10.className;
}else{
this.CssClass=this.ComboBox.ItemCssClass;
}
}
_10.className=this.ComboBox.ItemCssClassHover;
}
}
this.Highlighted=true;
this.ComboBox.HighlightedItem=this;
}
};
RadComboItem.prototype.UnHighlight=function(){
if(this.Highlighted&&this.Enabled&&document.getElementById(this.ClientID)){
document.getElementById(this.ClientID).className=this.CssClass;
this.Highlighted=false;
this.ComboBox.HighlightedItem=null;
}
};
RadComboItem.prototype.Select=function(){
this.ComboBox.SelectedItem=this;
this.ComboBox.SetState(this);
this.ComboBox.HideDropDown();
this.ComboBox.PostBackActive=true;
this.ComboBox.PostBack();
};
function RadComboBox(_11,_12,_13){
var _14=window[_12];
if(_14!=null&&!_14.tagName){
_14.Dispose();
}
if(window.tlrkComboBoxes==null){
window.tlrkComboBoxes=new Array();
}
tlrkComboBoxes[tlrkComboBoxes.length]=this;
this.Items=new Array();
this.Created=false;
this.ID=_11;
this.ClientID=_12;
this.TagID=_12;
this.DropDownID=_12+"_DropDown";
this.InputID=_12+"_Input";
this.ImageID=_12+"_Image";
this.DropDownPlaceholderID=_12+"_DropDownPlaceholder";
this.MoreResultsBoxID=_12+"_MoreResultsBox";
this.MoreResultsBoxImageID=_12+"_MoreResultsBoxImage";
this.MoreResultsBoxMessageID=_12+"_MoreResultsBoxMessage";
this.Header=_12+"_Header";
this.InputDomElement=document.getElementById(this.InputID);
this.ImageDomElement=document.getElementById(this.ImageID);
this.DropDownPlaceholderDomElement=document.getElementById(this.DropDownPlaceholderID);
this.TextHidden=document.getElementById(this.ClientID+"_text");
this.ValueHidden=document.getElementById(this.ClientID+"_value");
this.IndexHidden=document.getElementById(this.ClientID+"_index");
this.ClientWidthHidden=document.getElementById(this.ClientID+"_clientWidth");
this.ClientHeightHidden=document.getElementById(this.ClientID+"_clientHeight");
this.Enabled=true;
this.DropDownVisible=false;
this.LoadOnDemandUrl=null;
this.PollTimeOut=0;
this.HighlightedItem=null;
this.SelectedItem=null;
this.ItemRequestTimeout=300;
this.EnableLoadOnDemand=false;
this.AutoPostBack=false;
this.ShowMoreResultsBox=false;
this.OpenDropDownOnLoad=false;
this.CurrentlyPolling=false;
this.MarkFirstMatch=false;
this.IsCaseSensitive=false;
this.SelectOnTab=true;
this.PostBackReference=null;
this.LoadingMessage="Loading...";
this.ScrollDownImage=null;
this.ScrollDownImageDisabled=null;
this.IFrameShim=null;
this.RadComboBoxImagePosition="Right";
this.ItemCssClass=null;
this.ItemCssClassHover=null;
this.ItemCssClassDisabled=null;
this.ImageCssClass=null;
this.ImageCssClassHover=null;
this.InputCssClass=null;
this.InputCssClassHover=null;
this.LoadingMessageCssClass="ComboBoxLoadingMessage";
this.AutoCompleteSeparator=null;
this.ExternalCallBackPage=null;
this.OnClientSelectedIndexChanging=null;
this.OnClientDropDownOpening=null;
this.OnClientDropDownClosing=null;
this.OnClientItemsRequesting=null;
this.OnClientSelectedIndexChanged=null;
this.OnClientItemsRequested=null;
this.OnClientKeyPressing=null;
this.Skin="Classic";
this.HideTimeoutID=0;
this.RequestTimeoutID=0;
this.IsDetached=false;
this.TextPriorToCallBack=null;
this.AllowCustomText=false;
this.ExpandEffectString=null;
this.HighlightTemplatedItems=false;
this.CausesValidation=false;
this.ClientDataString=null;
this.ShowDropDownOnTextboxClick=true;
this.ShowWhileLoading=_13;
this.MoreResultsImageHovered=false;
this.PostBackActive=false;
this.SelectedIndex=-1;
this.IsTemplated=false;
this.CurrentText=null;
this.OffsetX=0;
this.OffsetY=0;
this.Disposed=false;
this.DetermineDirection();
var _15=this;
this.HideOnClickHandler=function(){
_15.HideOnClick();
};
this.AttachEvent(document,"click",this.HideOnClickHandler);
this.OnBlurHandler=function(e){
_15.HandleBlur(e||event);
};
this.AttachEvent(this.InputDomElement,"blur",this.OnBlurHandler);
this.OnFocusHandler=function(){
_15.HandleFocus();
};
this.AttachEvent(this.InputDomElement,"focus",this.OnFocusHandler);
this.InputDomElement.setAttribute("autocomplete","off");
this.DropDownPlaceholderDomElement.onselectstart=function(){
return false;
};
this.OnWindowLoadHandler=function(){
_15.FixUp(_15.InputDomElement,true);
};
if(typeof (RadCallbackNamespace)!="undefined"){
window.setTimeout(function(){
_15.FixUp(document.getElementById(_15.InputID),true);
},100);
}else{
if(window.addEventListener){
if(window.opera){
this.AttachEvent(window,"load",this.OnWindowLoadHandler);
}else{
this.OnWindowLoadHandler();
}
}else{
if(document.getElementById(this.ClientID).offsetWidth==0){
this.AttachEvent(window,"load",this.OnWindowLoadHandler);
}else{
this.OnWindowLoadHandler();
}
}
}
this.OnUnloadHandler=function(){
_15.Dispose();
};
this.AttachEvent(window,"unload",this.OnUnloadHandler);
}
RadComboBox.prototype.AttachEvent=function(_17,_18,_19){
if(_17.attachEvent){
_17.attachEvent("on"+_18,_19);
}else{
if(_17.addEventListener){
_17.addEventListener(_18,_19,false);
}
}
};
RadComboBox.prototype.DetachEvent=function(_1a,_1b,_1c){
if(_1a.detachEvent){
_1a.detachEvent("on"+_1b,_1c);
}else{
if(_1a.removeEventListener){
_1a.removeEventListener(_1b,_1c,false);
}
}
};
RadComboBox.prototype.ClearItems=function(){
this.Items=[];
document.getElementById(this.DropDownID).innerHTML="";
};
RadComboBox.prototype.GetViewPortSize=function(){
var _1d=0;
var _1e=0;
var _1f=document.body;
if(window.innerWidth){
_1d=window.innerWidth;
_1e=window.innerHeight;
}else{
if(document.compatMode&&document.compatMode=="CSS1Compat"){
_1f=document.documentElement;
}
_1d=_1f.clientWidth;
_1e=_1f.clientHeight;
}
_1d+=_1f.scrollLeft;
_1e+=_1f.scrollTop;
return {width:_1d-6,height:_1e-6};
};
RadComboBox.prototype.GetElementPosition=function(el){
var _21=null;
var pos={x:0,y:0};
var box;
if(el.getBoundingClientRect){
box=el.getBoundingClientRect();
var _24=document.documentElement.scrollTop||document.body.scrollTop;
var _25=document.documentElement.scrollLeft||document.body.scrollLeft;
pos.x=box.left+_25-2;
pos.y=box.top+_24-2;
return pos;
}else{
if(document.getBoxObjectFor){
box=document.getBoxObjectFor(el);
pos.x=box.x-2;
pos.y=box.y-2;
}else{
pos.x=el.offsetLeft;
pos.y=el.offsetTop;
_21=el.offsetParent;
if(_21!=el){
while(_21){
pos.x+=_21.offsetLeft;
pos.y+=_21.offsetTop;
_21=_21.offsetParent;
}
}
}
}
if(window.opera){
_21=el.offsetParent;
while(_21&&_21.tagName!="BODY"&&_21.tagName!="HTML"){
pos.x-=_21.scrollLeft;
pos.y-=_21.scrollTop;
_21=_21.offsetParent;
}
}else{
_21=el.parentNode;
while(_21&&_21.tagName!="BODY"&&_21.tagName!="HTML"){
pos.x-=_21.scrollLeft;
pos.y-=_21.scrollTop;
_21=_21.parentNode;
}
}
return pos;
};
RadComboBox.prototype.Dispose=function(){
this.DropDownPlaceholderDomElement.onselectstart=null;
this.HideDropDown();
if(this.DropDownPlaceholderDomElement!=null&&this.DropDownPlaceholderDomElement.parentNode!=null){
this.DropDownPlaceholderDomElement.parentNode.removeChild(this.DropDownPlaceholderDomElement);
}
this.DetachEvent(document,"click",this.HideOnClickHandler);
this.DetachEvent(this.InputDomElement,"blur",this.OnBlurHandler);
this.DetachEvent(this.InputDomElement,"focus",this.OnFocusHandler);
this.DetachEvent(window,"load",this.OnWindowLoadHandler);
this.DetachEvent(window,"unload",this.OnUnloadHandler);
this.InputDomElement=null;
this.DropDownPlaceholderDomElement=null;
this.Items=null;
this.ImageDomElement=null;
this.TextHidden=null;
this.ValueHidden=null;
this.IndexHidden=null;
this.IFrameShim=null;
tlrkComboBoxes[this.ID]=null;
this.Disposed=true;
};
RadComboBox.prototype.Initialize=function(_26,_27){
this.LoadConfiguration(_26);
this.CreateItems(_27);
this.InitCssNames();
this.HighlightSelectedItem();
};
RadComboBox.prototype.LoadConfiguration=function(_28){
for(var _29 in _28){
this[_29]=_28[_29];
}
};
RadComboBox.prototype.InitCssNames=function(){
this.ItemCssClass="ComboBoxItem_"+this.Skin;
this.ItemCssClassHover="ComboBoxItemHover_"+this.Skin;
this.ItemCssClassDisabled="ComboBoxItemDisabled_"+this.Skin;
this.ImageCssClass="ComboBoxImage_"+this.Skin;
this.ImageCssClassHover="ComboBoxImageHover_"+this.Skin;
this.InputCssClass="ComboBoxInput_"+this.Skin;
this.InputCssClassHover="ComboBoxInputHover_"+this.Skin;
this.LoadingMessageCssClass="ComboBoxLoadingMessage_"+this.Skin;
};
RadComboBox.prototype.SetState=function(_2a){
if(_2a!=null){
this.SetTextAfterLastDelimiter(_2a.Text);
this.SetValue(_2a.Value);
this.SetIndex(_2a.Index);
}else{
this.SetText("");
this.SetValue("");
this.SetIndex("-1");
}
};
RadComboBox.prototype.PostBack=function(){
if(this.AutoPostBack){
if(this.CausesValidation){
if(typeof (WebForm_DoPostBackWithOptions)!="function"&&!(typeof (Page_ClientValidate)!="function"||Page_ClientValidate())){
return;
}
}
eval(this.PostBackReference);
}
};
RadComboBox.prototype.HandleClick=function(_2b){
var _2c=this.SelectedItem;
var _2d=this.HighlightedItem;
if(_2d!=null&&_2c!=_2d){
this.ExecuteAction(_2b);
}
};
RadComboBox.prototype.HandleBlur=function(e){
var _2f=this.SelectedItem;
if(!this.AllowCustomText&&this.HighlightedItem==null){
this.HighlightMatches();
}
var _30=this.HighlightedItem;
if(_30!=null&&_2f!=_30){
if(this.ExecuteAction()==false){
return;
}
}
if(this.MoreResultsImageHovered){
return;
}
var _31=this.CurrentText;
var _32=this.GetText();
if(_31!=_32&&this.AllowCustomText){
this.SetText(this.GetText());
if(!this.PostBackActive){
if(_30!=null||_31!=_32){
if(this.FireEvent(this.OnClientSelectedIndexChanging,_30,e)==false){
return;
}
if(_30!=null){
this.SetText(_30.Text);
this.SetValue(_30.Value);
}
this.FireEvent(this.OnClientSelectedIndexChanged,_30,e);
this.PostBack();
}
}else{
this.PostBackActive=false;
}
}
};
RadComboBox.prototype.HandleFocus=function(e){
this.CurrentText=this.GetText();
this.RaiseOnClientFocus();
};
RadComboBox.prototype.FindParentForm=function(){
var _34=document.getElementById(this.TagID);
while(_34.tagName!="FORM"){
_34=_34.parentNode;
}
return _34;
};
RadComboBox.prototype.DropDownRequiresForm=function(){
var _35=this.DropDownPlaceholderDomElement.getElementsByTagName("input");
return _35.length>0;
};
RadComboBox.prototype.DetachDropDown=function(){
if((!document.readyState||document.readyState=="complete")&&(!this.IsDetached)){
var _36=document.body;
if(this.DropDownRequiresForm()){
_36=this.FindParentForm();
}
this.DropDownPlaceholderDomElement.parentNode.removeChild(this.DropDownPlaceholderDomElement);
this.DropDownPlaceholderDomElement.style.marginLeft="0";
_36.insertBefore(this.DropDownPlaceholderDomElement,_36.firstChild);
this.IsDetached=true;
}
};
RadComboBox.prototype.HideOnClick=function(){
var _37=this;
this.HideTimeoutID=window.setTimeout(function(){
_37.DoHideOnClick();
},5);
};
RadComboBox.prototype.DoHideOnClick=function(){
if(this.HideTimeoutID){
this.HideDropDown();
}
};
RadComboBox.prototype.ClearHideTimeout=function(){
this.HideTimeoutID=0;
};
RadComboBox.prototype.GetLastSeparatorIndex=function(_38){
var _39=-1;
for(var i=0;i<this.AutoCompleteSeparator.length;i++){
var _3b=this.AutoCompleteSeparator.charAt(i);
var _3c=_38.lastIndexOf(_3b);
if(_3c>_39){
_39=_3c;
}
}
return _39;
};
RadComboBox.prototype.SetTextAfterLastDelimiter=function(_3d){
var _3e=-1;
var _3f=this.GetText();
if(this.AutoCompleteSeparator!=null){
_3e=this.GetLastSeparatorIndex(_3f);
}
var _40=_3f.substring(0,_3e+1)+_3d;
this.SetText(_40);
};
RadComboBox.prototype.ClearSelection=function(){
this.SetState(null);
this.SelectedItem=null;
this.HighLightedItem=null;
};
RadComboBox.prototype.CreateItems=function(_41){
for(var i=0;i<_41.length;i++){
var _43=new RadComboItem();
_43.ComboBox=this;
_43.Index=this.Items.length;
_43.Initialize(_41[i]);
this.Items[this.Items.length]=_43;
if(_43.Selected&&!this.AllowCustomText){
this.SetText(_43.Text);
this.SetValue(_43.Value);
}
}
};
RadComboBox.prototype.HighlightSelectedItem=function(){
if(this.SelectedItem!=null){
this.SelectedItem.Highlight();
}else{
var _44;
var _45=this.GetValue();
_44=this.FindItemByValue(_45);
if(_44==null){
var _46=this.GetText();
_44=this.FindItemByText(_46);
}
if(_44!=null){
this.SelectedItem=_44;
this.SetState(_44);
this.SelectedItem.Highlight();
}
}
this.Created=true;
if(this.SelectedItem==null&&this.SelectedIndex==-1&&this.Items.length>0){
this.SelectedItem=this.Items[0];
this.Items[0].Selected=true;
this.SelectedItem.Highlight();
}
var _47=this;
this.OpenOnLoadHandler=function(){
_47.ShowDropDown();
};
if(this.OpenDropDownOnLoad){
this.AttachEvent(window,"load",this.OpenOnLoadHandler);
}
};
RadComboBox.prototype.InitializeAfterCallBack=function(_48,_49){
if(!_49){
this.Items.length=0;
}
this.HighlightedItem=null;
this.SelectedItem=null;
this.Created=false;
if(this.Items.length>0){
if(this.Items[0].Text==this.InputDomElement.value){
this.SetValue(this.Items[0].Value);
}else{
this.SetValue("");
}
this.TextPriorToCallBack=this.GetText();
}
this.CreateItems(_48);
};
RadComboBox.prototype.SetText=function(_4a){
this.InputDomElement.value=_4a;
this.TextHidden.value=_4a;
};
RadComboBox.prototype.GetText=function(){
return this.InputDomElement.value;
};
RadComboBox.prototype.SetValue=function(_4b){
if(_4b||_4b==""){
this.ValueHidden.value=_4b;
}
};
RadComboBox.prototype.GetValue=function(){
return this.ValueHidden.value;
};
RadComboBox.prototype.SetIndex=function(_4c){
this.IndexHidden.value=_4c;
};
RadComboBox.prototype.getXY=function(el){
var _4e=null;
var pos=[];
var box;
if(el.getBoundingClientRect){
box=el.getBoundingClientRect();
var _51=document.documentElement.scrollTop||document.body.scrollTop;
var _52=document.documentElement.scrollLeft||document.body.scrollLeft;
var x=box.left+_52-2;
var y=box.top+_51-2;
return [x,y];
}else{
if(document.getBoxObjectFor){
box=document.getBoxObjectFor(el);
pos=[box.x-1,box.y-1];
}else{
pos=[el.offsetLeft,el.offsetTop];
_4e=el.offsetParent;
if(_4e!=el){
while(_4e){
pos[0]+=_4e.offsetLeft;
pos[1]+=_4e.offsetTop;
_4e=_4e.offsetParent;
}
}
}
}
if(window.opera){
_4e=el.offsetParent;
while(_4e&&_4e.tagName.toUpperCase()!="BODY"&&_4e.tagName.toUpperCase()!="HTML"){
pos[0]-=_4e.scrollLeft;
pos[1]-=_4e.scrollTop;
_4e=_4e.offsetParent;
}
}else{
_4e=el.parentNode;
while(_4e&&_4e.tagName.toUpperCase()!="BODY"&&_4e.tagName.toUpperCase()!="HTML"){
pos[0]-=_4e.scrollLeft;
pos[1]-=_4e.scrollTop;
_4e=_4e.parentNode;
}
}
return pos;
};
RadComboBox.prototype.ShowOverlay=function(x,y){
if(document.readyState&&document.readyState!="complete"){
return;
}
var _57=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
var _58=window.opera;
if(_57||_58||(!document.all)){
return;
}
if(this.IFrameShim==null){
this.IFrameShim=document.createElement("IFRAME");
this.IFrameShim.src="javascript:''";
this.IFrameShim.id=this.ClientID+"_Overlay";
this.IFrameShim.frameBorder=0;
this.IFrameShim.style.position="absolute";
this.IFrameShim.style.display="none";
this.DetachDropDown();
this.DropDownPlaceholderDomElement.parentNode.insertBefore(this.IFrameShim,this.DropDownPlaceholderDomElement);
this.IFrameShim.style.zIndex=this.DropDownPlaceholderDomElement.style.zIndex-1;
}
this.IFrameShim.style.left=x;
this.IFrameShim.style.top=y;
var _59=this.DropDownPlaceholderDomElement.offsetWidth;
var _5a=this.DropDownPlaceholderDomElement.offsetHeight;
this.IFrameShim.style.width=_59+"px";
this.IFrameShim.style.height=_5a+"px";
this.IFrameShim.style.display="";
};
RadComboBox.prototype.HideOverlay=function(){
var _5b=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
var _5c=window.opera;
if(_5b||_5c||(!document.all)){
return;
}
if(this.IFrameShim!=null){
this.IFrameShim.style.display="none";
}
};
RadComboBox.prototype.HideAllDropDowns=function(){
for(var i=0;i<tlrkComboBoxes.length;i++){
if(tlrkComboBoxes[i].ClientID!=this.ClientID){
tlrkComboBoxes[i].HideDropDown();
}
}
};
RadComboBox.prototype.DetermineDirection=function(){
var el=document.getElementById(this.ClientID+"_wrapper");
while(el.tagName.toLowerCase()!="html"){
if(el.dir){
this.RightToLeft=(el.dir.toLowerCase()=="rtl");
return;
}
el=el.parentNode;
}
this.RightToLeft=false;
};
RadComboBox.prototype.ShowDropDown=function(){
if(this.FireEvent(this.OnClientDropDownOpening,this)==false){
return;
}
this.HideAllDropDowns();
this.DetachDropDown();
var _5f;
(this.RadComboBoxImagePosition=="Right"&&!this.RightToLeft)?_5f=this.InputDomElement:_5f=this.ImageDomElement;
var _60=this.getXY(_5f);
var x=_60[0]+this.OffsetX;
var y=_60[1]+_5f.offsetHeight+this.OffsetY;
var _63=document.getElementById(this.TagID);
dropDownWidth=_63.offsetWidth;
if(this.ExpandEffectString!=null&&document.all){
try{
this.DropDownPlaceholderDomElement.style.filter=this.ExpandEffectString;
this.DropDownPlaceholderDomElement.filters[0].Apply();
this.DropDownPlaceholderDomElement.filters[0].Play();
}
catch(e){
}
}
if(this.RightToLeft){
this.DropDownPlaceholderDomElement.dir="rtl";
}
var _64=this.GetViewPortSize();
this.DropDownPlaceholderDomElement.style.position="absolute";
if(window.netscape||window.opera){
dropDownWidth-=2;
}
this.DropDownPlaceholderDomElement.style.width=dropDownWidth+"px";
this.DropDownPlaceholderDomElement.style.display="block";
if(this.ElementOverflowsBottom(_64,this.DropDownPlaceholderDomElement,_5f)){
var _65=y-this.DropDownPlaceholderDomElement.offsetHeight-_5f.offsetHeight;
if(_65>0){
y=_65;
}
}
this.DropDownPlaceholderDomElement.style.left=x+"px";
this.DropDownPlaceholderDomElement.style.top=y+"px";
this.ShowOverlay(x+"px",y+"px");
if(this.HighlightedItem!=null){
this.HighlightedItem.AdjustDownScroll();
}
if(this.SelectedItem!=null){
this.SelectedItem.AdjustDownScroll();
}
this.ClearHideTimeout();
this.DropDownVisible=true;
try{
this.InputDomElement.focus();
}
catch(e){
}
if((this.EnableLoadOnDemand)&&(this.Items.length==0)){
this.PollServerInterMediate(true,null);
}
if(this.SelectedItem!=null){
this.SelectedItem.Highlighted=false;
this.SelectedItem.Highlight();
this.SelectedItem.AdjustUpScroll();
}
};
RadComboBox.prototype.ElementOverflowsBottom=function(_66,_67,_68){
var _69=this.GetElementPosition(_68).y+_67.offsetHeight;
return _69>_66.height;
};
RadComboBox.prototype.FindItemByText=function(_6a){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Text==_6a){
return this.Items[i];
}
}
return null;
};
RadComboBox.prototype.FindItemByValue=function(_6c){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_6c){
return this.Items[i];
}
}
return null;
};
RadComboBox.prototype.HideDropDown=function(){
if(this.DropDownVisible){
if(this.FireEvent(this.OnClientDropDownClosing,this)==false){
return;
}
this.DropDownPlaceholderDomElement.style.display="none";
this.HideOverlay();
this.DropDownVisible=false;
this.RaiseOnClientBlur();
}
};
RadComboBox.prototype.RaiseOnClientBlur=function(){
this.FireEvent(this.OnClientBlur,this);
};
RadComboBox.prototype.RaiseOnClientFocus=function(){
this.FireEvent(this.OnClientFocus,this);
};
RadComboBox.prototype.ToggleDropDown=function(){
(this.DropDownVisible)?this.HideDropDown():this.ShowDropDown();
};
RadComboBox.prototype.HtmlElementToItem=function(obj){
if(obj){
while(obj!=null){
if(obj.id&&this.IsElementAnItem(obj.id)){
return obj;
}
obj=obj.parentNode;
}
}
return null;
};
RadComboBox.prototype.IsElementAnItem=function(_6f){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].ClientID==_6f){
return true;
}
}
return false;
};
RadComboBox.prototype.ItemToInstance=function(_71){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].ClientID==_71.id){
return this.Items[i];
}
}
return null;
};
RadComboBox.prototype.HandleMouseOver=function(_73){
_73.Highlight();
};
RadComboBox.prototype.HandleMouseOut=function(_74){
_74.UnHighlight();
};
RadComboBox.prototype.ExecuteAction=function(_75){
var _76=this.HighlightedItem;
if(_76!=null){
if(this.FireEvent(this.OnClientSelectedIndexChanging,_76,_75)==false){
return false;
}
this.FireEvent(this.OnClientSelectedIndexChanged,_76,_75);
_76.Select();
}
this.HideDropDown();
return true;
};
RadComboBox.prototype.FindNextAvailableItem=function(_77){
var i=_77;
var _79=false;
while(i<this.Items.length-1){
i=i+1;
if(this.Items[i].Enabled){
_79=true;
break;
}
}
if(_79){
return i;
}
return _77;
};
RadComboBox.prototype.FindPrevAvailableItem=function(_7a){
var i=_7a;
var _7c=false;
while(i>0){
i=i-1;
if(this.Items[i].Enabled){
_7c=true;
break;
}
}
if(_7c){
return i;
}
return _7a;
};
RadComboBox.prototype.HandleKeyPress=function(_7d,_7e){
this.FireEvent(this.OnClientKeyPressing,this,_7e);
var _7f=_7e.keyCode;
if(_7f==40){
if(_7e.altKey&&(!this.DropDownVisible)){
this.ShowDropDown();
return;
}
var _80=0;
if(this.HighlightedItem!=null){
_80=this.FindNextAvailableItem(this.HighlightedItem.Index);
}
if(_80>=0&&this.Items.length>0){
this.Items[_80].Highlight();
this.Items[_80].AdjustDownScroll();
this.SetState(this.Items[_80]);
this.PreventDefault(_7e);
}
return;
}
if(_7f==27&&this.DropDownVisible){
this.HideDropDown();
return;
}
if(_7f==38){
if(_7e.altKey&&this.DropDownVisible){
this.HideDropDown();
return;
}
var _80=-1;
if(this.HighlightedItem!=null){
_80=this.FindPrevAvailableItem(this.HighlightedItem.Index);
}
if(_80>=0){
this.Items[_80].AdjustUpScroll();
this.Items[_80].Highlight();
this.SetState(this.Items[_80]);
this.PreventDefault(_7e);
}
return;
}
if((_7f==13||_7f==9)&&this.DropDownVisible){
if(_7f==13){
this.PreventDefault(_7e);
this.ExecuteAction();
}
return;
}
if(_7f==9&&!this.DropDownVisible){
this.RaiseOnClientBlur();
return;
}
if(_7f==35||_7f==36||_7f==37||_7f==39){
return;
}
if(this.EnableLoadOnDemand&&(!_7e.altKey)&&(!_7e.ctrlKey)&&(!(_7f==16))){
if(!this.DropDownVisible){
this.ShowDropDown();
}
this.PollServer(false,_7f);
return;
}
if((_7f<32)||(_7f>=33&&_7f<=46)||(_7f>=112&&_7f<=123)||(_7e.altKey==true)){
return;
}
var _81=this;
window.setTimeout(function(){
_81.HighlightMatches();
},20);
};
RadComboBox.prototype.HandleKeyDown=function(_82){
if(_82.preventDefault){
if(_82.keyCode==13||(_82.keyCode==32&&(!this.EnableLoadOnDemand))){
_82.preventDefault();
}
}
};
RadComboBox.prototype.EncodeURI=function(s){
if(typeof (encodeURIComponent)!="undefined"){
return encodeURIComponent(this.EscapeQuotes(s));
}
if(escape){
return escape(this.EscapeQuotes(s));
}
};
RadComboBox.prototype.EscapeQuotes=function(_84){
if(typeof (_84)!="number"){
return _84.replace(/'/g,"&squote");
}
};
RadComboBox.prototype.GetXmlHttpRequest=function(){
if(typeof (XMLHttpRequest)!="undefined"){
return new XMLHttpRequest();
}
if(typeof (ActiveXObject)!="undefined"){
return new ActiveXObject("Microsoft.XMLHTTP");
}
};
RadComboBox.prototype.GetAjaxUrl=function(_85,_86,_87,_88){
_85=_85.replace(/'/g,"&squote");
var url=window.unescape(this.LoadOnDemandUrl)+"&text="+this.EncodeURI(_85);
url=url+"&comboText="+this.EncodeURI(_86);
url=url+"&comboValue="+this.EncodeURI(_87);
url=url+"&skin="+this.EncodeURI(this.Skin);
if(_88){
url=url+"&itemCount="+this.Items.length;
}
if(this.ExternalCallBackPage!=null){
url=url+"&external=true";
}
if(this.ClientDataString!=null){
url+="&clientDataString="+this.EncodeURI(this.ClientDataString);
}
url=url+"&timeStamp="+encodeURIComponent((new Date()).getTime());
return url;
};
RadComboBox.prototype.FetchCallBackData=function(_8a,_8b,_8c){
if(!this.CurrentlyPolling){
if(this.Disposed){
return;
}
this.CurrentlyPolling=true;
var _8d=this.GetText();
var _8e=this.GetValue();
var _8f=(_8b)?_8b:_8d;
var _90=this.GetAjaxUrl(_8f,_8d,_8e,_8a);
var _91=this;
var _92=this.GetXmlHttpRequest();
_92.onreadystatechange=function(){
if(_92.readyState!=4){
return;
}
_91.OnCallBackResponse(_92.responseText,_8a,_8f,_8c,_92.status,_90);
};
_92.open("GET",_90,true);
_92.setRequestHeader("Content-Type","application/json; charset=utf-8");
_92.send("");
}
};
RadComboBox.prototype.OnCallBackResponse=function(_93,_94,_95,_96,_97,url){
if(_97==500){
alert("RadComboBox: Server error in the ItemsRequested event handler, press ok to view the result.");
document.body.innerHTML=_93;
return;
}
if(_97==404){
alert("RadComboBox: Load On Demand Page not found: "+url);
var _99="RadComboBox: Load On Demand Page not found: "+url+"<br/>";
_99+="Please, try using ExternalCallBackPage to map to the exact location of the callbackpage you are using.";
document.body.innerHTML=_99;
return;
}
try{
eval("var callBackData = "+_93+";");
}
catch(e){
alert("RadComboBox: load on demand callback error. Press Enter for more information");
var _99="If RadComboBox is not initially visible on your ASPX page, you may need to use streamers (the ExternallCallBackPage property)";
_99+="<br/>Please, read our online documentation on this problem for details";
_99+="<br/><a href='http://www.telerik.com/help/radcombobox/v2%5FNET2/?combo_externalcallbackpage.html'>http://www.telerik.com/help/radcombobox/v2%5FNET2/combo_externalcallbackpage.html</a>";
document.body.innerHTML=_99;
return;
}
if(this.GetText()!=callBackData.Text){
this.CurrentlyPolling=false;
this.PollServer(false,null);
return;
}
if(this.ShowMoreResultsBox){
document.getElementById(this.MoreResultsBoxMessageID).innerHTML=callBackData.Message;
}
var _9a=this.Items.length;
this.InitializeAfterCallBack(callBackData.Items,_94);
if(_94){
document.getElementById(this.DropDownID).removeChild(document.getElementById(this.ClientID+"_LoadingDiv"));
document.getElementById(this.DropDownID).innerHTML+=callBackData.DropDownHtml;
if(this.Items[_9a+1]!=null){
this.Items[_9a+1].AdjustDownScroll();
}
}else{
document.getElementById(this.DropDownID).innerHTML=callBackData.DropDownHtml;
}
this.ShowOverlay(this.DropDownPlaceholderDomElement.style.left,this.DropDownPlaceholderDomElement.style.top);
this.CurrentlyPolling=false;
var _9b=this.FindItemByText(this.GetText());
if(_9b!=null){
_9b.Highlight();
_9b.AdjustDownScroll();
this.SelectedItem=_9b;
}
this.FireEvent(this.OnClientItemsRequested,this,_95,_94);
if(!_96){
return;
}
if(_96<32||(_96>=33&&_96<=46)||(_96>=112&&_96<=123)&&_96!=8){
return;
}
this.HighlightMatches();
};
RadComboBox.prototype.GetLastWord=function(_9c){
var _9d=-1;
if(this.AutoCompleteSeparator!=null){
_9d=this.GetLastSeparatorIndex(_9c);
}
var _9e=_9c.substring(_9d+1,_9c.length);
return _9e;
};
RadComboBox.prototype.CompareWords=function(_9f,_a0){
if(!this.IsCaseSensitive){
return (_9f.toLowerCase()==_a0.toLowerCase());
}else{
return (_9f==_a0);
}
};
RadComboBox.prototype.HighlightMatches=function(){
if(!this.MarkFirstMatch){
return;
}
var _a1=this.GetText();
var _a2=this.GetLastWord(_a1);
if(_a2.length==0){
return;
}
for(var i=0;i<this.Items.length;i++){
var _a4=this.Items[i].Text;
if(_a4.length>=_a2.length){
var _a5=_a4.substring(0,_a2.length);
if(this.CompareWords(_a5,_a2)){
if(this.Items[i].Enabled==false){
continue;
}
var _a6=-1;
if(this.AutoCompleteSeparator!=null){
_a6=this.GetLastSeparatorIndex(_a1);
}
var _a7=_a1.substring(0,_a6+1)+_a4;
this.SetText(_a7);
this.SetValue(this.Items[i].Value);
this.SetIndex(this.Items[i].Index);
if(this.FireEvent(this.OnClientSelectedIndexChanging,this.Items[i],null)==false){
return;
}
this.Items[i].Highlight();
this.Items[i].AdjustDownScroll();
var _a8=_a6+_a2.length+1;
var _a9=_a7.length-_a8;
if(document.all){
var _aa=this.InputDomElement.createTextRange();
_aa.moveStart("character",_a8);
_aa.moveEnd("character",_a9);
_aa.select();
}else{
this.InputDomElement.setSelectionRange(_a8,_a8+_a9);
}
return;
}else{
this.SetValue("");
this.SetIndex(-1);
if(this.HighlightedItem!=null){
this.HighlightedItem.UnHighlight();
}
}
}
}
this.SetValue("");
this.SetIndex("-1");
if(!this.AllowCustomText){
var _ab=_a1.substring(0,_a1.length-1);
if(this.TextPriorToCallBack!=null){
this.SetText(this.TextPriorToCallBack);
return;
}
this.SetText(_ab);
this.HighlightMatches();
}
};
RadComboBox.prototype.PollServer=function(_ac,_ad){
if(!this.CurrentlyPolling){
var _ae=this;
if(this.RequestTimeoutID){
window.clearTimeout(this.RequestTimeoutID);
this.RequestTimeoutID=0;
}
this.RequestTimeoutID=window.setTimeout(function(){
_ae.PollServerInterMediate(_ac,_ad);
},this.ItemRequestTimeout);
}
};
RadComboBox.prototype.PollServerInterMediate=function(_af,_b0){
var _b1=this.InputDomElement.value;
if(_b1==""){
_b1=false;
}
if(this.FireEvent(this.OnClientItemsRequesting,this,_b1,_af)==false){
return;
}
if(!this.CurrentlyPolling){
if(!document.getElementById(this.ClientID+"_LoadingDiv")){
document.getElementById(this.DropDownID).innerHTML="<div id='"+this.ClientID+"_LoadingDiv'"+" class='"+this.LoadingMessageCssClass+" '>"+this.LoadingMessage+"</div>"+document.getElementById(this.DropDownID).innerHTML;
}
}
var _b2=this;
window.setTimeout(function(){
_b2.FetchCallBackData(_af,_b1,_b0);
},20);
};
RadComboBox.prototype.RequestItems=function(_b3,_b4){
this.FetchCallBackData(_b4,_b3,null);
};
RadComboBox.prototype.UnHighlightAll=function(){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Highlighted){
this.Items[i].UnHighlight();
}
}
};
RadComboBox.prototype.HandleInputImageOut=function(){
this.InputDomElement.className=this.InputCssClass;
var _b6=this.ImageDomElement;
if(_b6){
_b6.className=this.ImageCssClass;
}
};
RadComboBox.prototype.HandleInputImageHover=function(){
this.InputDomElement.className=this.InputCssClassHover;
var _b7=this.ImageDomElement;
if(_b7){
_b7.className=this.ImageCssClassHover;
}
};
RadComboBox.prototype.HandleMoreResultsImageOut=function(){
document.getElementById(this.MoreResultsBoxImageID).style.cursor="default";
document.getElementById(this.MoreResultsBoxImageID).src=this.ScrollDownImageDisabled;
this.MoreResultsImageHovered=false;
};
RadComboBox.prototype.HandleMoreResultsImageHover=function(){
document.getElementById(this.MoreResultsBoxImageID).style.cursor="hand";
document.getElementById(this.MoreResultsBoxImageID).src=this.ScrollDownImage;
this.MoreResultsImageHovered=true;
};
RadComboBox.prototype.HandleMoreResultsImageClick=function(){
this.UnHighlightAll();
this.PollServer(true,null);
this.InputDomElement.focus();
};
RadComboBox.prototype.CancelPropagation=function(_b8){
if(_b8.stopPropagation){
_b8.stopPropagation();
}else{
_b8.cancelBubble=true;
}
};
RadComboBox.prototype.PreventDefault=function(_b9){
if(_b9.preventDefault){
_b9.preventDefault();
}else{
_b9.returnValue=false;
}
};
RadComboBox.prototype.FireEvent=function(_ba,a,b,c){
if(!_ba){
return true;
}
RadComboBoxGlobalFirstParam=a;
RadComboBoxGlobalSecondParam=b;
RadComboBoxGlobalThirdParam=c;
var s=_ba;
s=s+"(RadComboBoxGlobalFirstParam";
s=s+",RadComboBoxGlobalSecondParam";
s=s+",RadComboBoxGlobalThirdParam";
s=s+");";
return eval(s);
};
RadComboBox.prototype.HandleEvent=function(_bf,_c0){
var _c1;
var _c2=(document.all)?_c0.srcElement:_c0.target;
var _c3=this.HtmlElementToItem(_c2);
if(_c3!=null){
_c1=this.ItemToInstance(_c3);
}
if(!this.Enabled){
return;
}
switch(_bf){
case "showdropdown":
this.CancelPropagation(_c0);
this.ShowDropDown();
break;
case "hidedropdown":
this.CancelPropagation(_c0);
this.HideDropDown();
break;
case "toggledropdown":
this.CancelPropagation(_c0);
this.ToggleDropDown();
break;
case "mouseover":
if(_c1!=null){
this.HandleMouseOver(_c1);
}
break;
case "mouseout":
if(_c1!=null){
this.HandleMouseOut(_c1);
}
break;
case "keypress":
this.HandleKeyPress(this,_c0);
break;
case "keydown":
this.HandleKeyDown(_c0);
break;
case "click":
this.HandleClick(_c0);
break;
case "inputclick":
this.CancelPropagation(_c0);
this.InputDomElement.select();
if(this.ShowDropDownOnTextboxClick){
this.ShowDropDown();
}
break;
case "inputimageout":
this.HandleInputImageOut();
break;
case "inputimagehover":
this.HandleInputImageHover();
break;
case "moreresultsimageclick":
this.CancelPropagation(_c0);
this.HandleMoreResultsImageClick();
break;
case "moreresultsimagehover":
this.HandleMoreResultsImageHover();
break;
case "moreresultsimageout":
this.HandleMoreResultsImageOut();
break;
}
};
RadComboBox.prototype.Enable=function(){
this.InputDomElement.disabled=false;
this.Enabled=true;
};
RadComboBox.prototype.Disable=function(){
this.InputDomElement.disabled="disabled";
this.Enabled=false;
this.TextHidden.value=this.GetText();
};
RadComboBox.prototype.FixUp=function(_c4,_c5){
if((this.ClientWidthHidden.value!="")&&(this.ClientHeightHidden.value!="")){
if(_c4.style.width!=this.ClientWidthHidden.value){
_c4.style.width=this.ClientWidthHidden.value;
}
if(_c4.style.height!=this.ClientHeightHidden.value){
_c4.style.height=this.ClientHeightHidden.value;
}
this.ShowWrapperElement();
return;
}
var _c6=_c4.parentNode.getElementsByTagName("img")[0];
if(_c5&&_c6&&(_c6.offsetWidth==0)){
var _c7=this;
if(document.attachEvent){
if(document.readyState=="complete"){
window.setTimeout(function(){
_c7.FixUp(_c4,false);
},100);
}else{
window.attachEvent("onload",function(){
_c7.FixUp(_c4,false);
});
}
}else{
window.addEventListener("load",function(){
_c7.FixUp(_c4,false);
},false);
}
return;
}
var _c8=null;
if(_c4.currentStyle){
_c8=_c4.currentStyle;
}else{
if(document.defaultView&&document.defaultView.getComputedStyle){
_c8=document.defaultView.getComputedStyle(_c4,null);
}
}
if(_c8==null){
this.ShowWrapperElement();
return;
}
var _c9=parseInt(_c8.height);
var _ca=parseInt(_c4.offsetWidth);
var _cb=parseInt(_c8.paddingTop);
var _cc=parseInt(_c8.paddingBottom);
var _cd=parseInt(_c8.paddingLeft);
var _ce=parseInt(_c8.paddingRight);
var _cf=parseInt(_c8.borderTopWidth);
if(isNaN(_cf)){
_cf=0;
}
var _d0=parseInt(_c8.borderBottomWidth);
if(isNaN(_d0)){
_d0=0;
}
var _d1=parseInt(_c8.borderLeftWidth);
if(isNaN(_d1)){
_d1=0;
}
var _d2=parseInt(_c8.borderRightWidth);
if(isNaN(_d2)){
_d2=0;
}
if(document.compatMode&&document.compatMode=="CSS1Compat"){
if(!isNaN(_c9)&&(this.ClientHeightHidden.value=="")){
_c4.style.height=_c9-_cb-_cc-_cf-_d0+"px";
this.ClientHeightHidden.value=_c4.style.height;
}
}
if(!isNaN(_ca)&&_ca&&(this.ClientWidthHidden.value=="")){
var _d3=0;
if(_c6){
_d3=_c6.offsetWidth;
}
if(document.compatMode&&document.compatMode=="CSS1Compat"){
var _d4=_ca-_d3-_cd-_ce-_d1-_d2;
if(_d4>=0){
_c4.style.width=_d4+"px";
}
this.ClientWidthHidden.value=_c4.style.width;
}else{
_c4.style.width=_ca-_d3;
}
}
this.ShowWrapperElement();
};
RadComboBox.prototype.ShowWrapperElement=function(){
if(!this.ShowWhileLoading){
document.getElementById(this.ClientID+"_wrapper").style.visibility="visible";
}
};
function rcbDispatcher(_d5,_d6,_d7){
var _d8=null;
try{
_d8=window[_d5];
}
catch(e){
}
if(typeof (_d8)=="undefined"||_d8==null){
return;
}
if(typeof (_d8.HandleEvent)!="undefined"){
_d8.HandleEvent(_d6,_d7);
}
}
function rcbAppendStyleSheet(_d9,_da){
var _db=(navigator.appName=="Microsoft Internet Explorer")&&((navigator.userAgent.toLowerCase().indexOf("mac")!=-1)||(navigator.appVersion.toLowerCase().indexOf("mac")!=-1));
var _dc=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
if(_db||_dc){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_da+"'>");
}else{
var _dd=document.createElement("LINK");
_dd.rel="stylesheet";
_dd.type="text/css";
_dd.href=_da;
document.getElementById(_d9+"StyleSheetHolder").appendChild(_dd);
}
}


//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
