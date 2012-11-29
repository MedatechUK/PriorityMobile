if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.DomEventMixin)=="undefined"||typeof (window.RadControlsNamespace.DomEventMixin.Version)==null||window.RadControlsNamespace.DomEventMixin.Version<2){
RadControlsNamespace.DomEventMixin={Version:2,Initialize:function(_1){
_1.CreateEventHandler=this.CreateEventHandler;
_1.AttachDomEvent=this.AttachDomEvent;
_1.DetachDomEvent=this.DetachDomEvent;
_1.DisposeDomEventHandlers=this.DisposeDomEventHandlers;
_1._domEventHandlingEnabled=true;
_1.EnableDomEventHandling=this.EnableDomEventHandling;
_1.DisableDomEventHandling=this.DisableDomEventHandling;
_1.RemoveHandlerRegister=this.RemoveHandlerRegister;
_1.GetHandlerRegister=this.GetHandlerRegister;
_1.AddHandlerRegister=this.AddHandlerRegister;
_1.handlerRegisters=[];
},EnableDomEventHandling:function(){
this._domEventHandlingEnabled=true;
},DisableDomEventHandling:function(){
this._domEventHandlingEnabled=false;
},CreateEventHandler:function(_2,_3){
var _4=this;
return function(e){
if(!_4._domEventHandlingEnabled&&!_3){
return false;
}
return _4[_2](e||window.event);
};
},AttachDomEvent:function(_6,_7,_8,_9){
var _a=this.CreateEventHandler(_8,_9);
var _b=this.GetHandlerRegister(_6,_7,_8);
if(_b!=null){
this.DetachDomEvent(_b.Element,_b.EventName,_8);
}
var _c={"Element":_6,"EventName":_7,"HandlerName":_8,"Handler":_a};
this.AddHandlerRegister(_c);
if(_6.addEventListener){
_6.addEventListener(_7,_a,false);
}else{
if(_6.attachEvent){
_6.attachEvent("on"+_7,_a);
}
}
},DetachDomEvent:function(_d,_e,_f){
var _10=null;
var _11="";
if(typeof _f=="string"){
_11=_f;
_10=this.GetHandlerRegister(_d,_e,_11);
if(_10==null){
return;
}
_f=_10.Handler;
}
if(!_d){
return;
}
if(_d.removeEventListener){
_d.removeEventListener(_e,_f,false);
}else{
if(_d.detachEvent){
_d.detachEvent("on"+_e,_f);
}
}
if(_10!=null&&_11!=""){
this.RemoveHandlerRegister(_10);
_10=null;
}
},DisposeDomEventHandlers:function(){
for(var i=0;i<this.handlerRegisters.length;i++){
var _13=this.handlerRegisters[i];
if(_13!=null){
this.DetachDomEvent(_13.Element,_13.EventName,_13.Handler);
}
}
this.handlerRegisters=[];
},RemoveHandlerRegister:function(_14){
try{
var _15=_14.index;
for(var i in _14){
_14[i]=null;
}
this.handlerRegisters[_15]=null;
}
catch(e){
}
},GetHandlerRegister:function(_17,_18,_19){
for(var i=0;i<this.handlerRegisters.length;i++){
var _1b=this.handlerRegisters[i];
if(_1b!=null&&_1b.Element==_17&&_1b.EventName==_18&&_1b.HandlerName==_19){
return this.handlerRegisters[i];
}
}
return null;
},AddHandlerRegister:function(_1c){
_1c.index=this.handlerRegisters.length;
this.handlerRegisters[this.handlerRegisters.length]=_1c;
}};
RadControlsNamespace.DomEvent={};
RadControlsNamespace.DomEvent.PreventDefault=function(e){
if(!e){
return true;
}
if(e.preventDefault){
e.preventDefault();
}
e.returnValue=false;
return false;
};
RadControlsNamespace.DomEvent.StopPropagation=function(e){
if(!e){
return;
}
if(e.stopPropagation){
e.stopPropagation();
}else{
e.cancelBubble=true;
}
};
RadControlsNamespace.DomEvent.GetTarget=function(e){
if(!e){
return null;
}
return e.target||e.srcElement;
};
RadControlsNamespace.DomEvent.GetRelatedTarget=function(e){
if(!e){
return null;
}
return e.relatedTarget||(e.type=="mouseout"?e.toElement:e.fromElement);
};
RadControlsNamespace.DomEvent.GetKeyCode=function(e){
if(!e){
return 0;
}
return e.which||e.keyCode;
};
}
var RadGridNamespace={};
RadGridNamespace.Prefix="grid_";
RadGridNamespace.InitializeClient=function(_22){
var _23=document.getElementById(_22+"AtlasCreation");
if(!_23){
return;
}
var _24=document.createElement("script");
if(navigator.userAgent.indexOf("Safari")!=-1){
_24.innerHTML=_23.innerHTML;
}else{
_24.text=_23.innerHTML;
}
document.body.appendChild(_24);
document.body.removeChild(_24);
_23.parentNode.removeChild(_23);
};
RadGridNamespace.AsyncRequest=function(_25,_26,_27){
var _28=window[_27];
if(_28!=null&&typeof (_28.AsyncRequest)=="function"){
_28.AsyncRequest(_25,_26);
}
};
RadGridNamespace.AsyncRequestWithOptions=function(_29,_2a){
var _2b=window[_2a];
if(_2b!=null&&typeof (_2b.AsyncRequestWithOptions)=="function"){
_2b.AsyncRequestWithOptions(_29);
}
};
RadGridNamespace.GetWidth=function(_2c){
var _2d;
if(window.getComputedStyle){
_2d=window.getComputedStyle(_2c,"").getPropertyValue("width");
}else{
if(_2c.currentStyle){
_2d=_2c.currentStyle.width;
}else{
_2d=_2c.offsetWidth;
}
}
if(_2d.toString().indexOf("%")!=-1){
_2d=_2c.offsetWidth;
}
if(_2d.toString().indexOf("px")!=-1){
_2d=parseInt(_2d);
}
return _2d;
};
RadGridNamespace.GetScrollBarWidth=function(){
try{
if(typeof (RadGridNamespace.scrollbarWidth)=="undefined"){
var _2e,_2f=0;
var _30=document.createElement("div");
_30.style.position="absolute";
_30.style.top="-1000px";
_30.style.left="-1000px";
_30.style.width="100px";
_30.style.overflow="auto";
var _31=document.createElement("div");
_31.style.width="1000px";
_30.appendChild(_31);
document.body.appendChild(_30);
_2e=_30.offsetWidth;
_2f=_30.clientWidth;
document.body.removeChild(document.body.lastChild);
RadGridNamespace.scrollbarWidth=_2e-_2f;
if(RadGridNamespace.scrollbarWidth<=0||_2f==0){
RadGridNamespace.scrollbarWidth=16;
}
}
return RadGridNamespace.scrollbarWidth;
}
catch(error){
return false;
}
};
RadGridNamespace.GetTableColGroup=function(_32){
try{
return _32.getElementsByTagName("colgroup")[0];
}
catch(error){
return false;
}
};
RadGridNamespace.GetTableColGroupCols=function(_33){
try{
var _34=new Array();
var _35=_33.childNodes[0];
for(var i=0;i<_33.childNodes.length;i++){
if((_33.childNodes[i].tagName)&&(_33.childNodes[i].tagName.toLowerCase()=="col")){
_34[_34.length]=_33.childNodes[i];
}
}
return _34;
}
catch(error){
return false;
}
};
RadGridNamespace.Confirm=function(_37,e){
if(!confirm(_37)){
e.cancelBubble=true;
e.returnValue=false;
return false;
}
};
RadGridNamespace.SynchronizeWithWindow=function(){
};
RadGridNamespace.IsParentRightToLeft=function(_39){
try{
while(_39){
_39=_39.parentNode;
if(_39.currentStyle&&_39.currentStyle.direction.toLowerCase()=="rtl"){
return true;
}else{
if(getComputedStyle&&getComputedStyle(_39,"").getPropertyValue("direction").toLowerCase()=="rtl"){
return true;
}else{
if(_39.dir.toLowerCase()=="rtl"){
return true;
}
}
}
}
return false;
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError,this.OnError);
}
};
RadGridNamespace.FireEvent=function(_3a,_3b,_3c){
try{
var _3d=true;
if(typeof (_3a[_3b])=="string"){
eval(_3a[_3b]);
}else{
if(typeof (_3a[_3b])=="function"){
if(_3c){
switch(_3c.length){
case 1:
_3d=_3a[_3b](_3c[0]);
break;
case 2:
_3d=_3a[_3b](_3c[0],_3c[1]);
break;
}
}else{
_3d=_3a[_3b]();
}
}
}
if(typeof (_3d)!="boolean"){
return true;
}else{
return _3d;
}
}
catch(error){
throw error;
}
};
RadGridNamespace.CheckParentNodesFor=function(_3e,_3f){
while(_3e){
if(_3e==_3f){
return true;
}
_3e=_3e.parentNode;
}
return false;
};
RadGridNamespace.GetCurrentElement=function(e){
if(!e){
var e=window.event;
}
var _41;
if(e.srcElement){
_41=e.srcElement;
}else{
_41=e.target;
}
return _41;
};
RadGridNamespace.GetEventPosX=function(e){
var x=e.clientX;
var _44=RadGridNamespace.GetCurrentElement(e);
while(_44.parentNode){
if(typeof (_44.parentNode.scrollLeft)=="number"){
x+=_44.parentNode.scrollLeft;
}
_44=_44.parentNode;
}
if(document.body.currentStyle&&document.body.currentStyle.margin.indexOf("px")!=-1){
x=parseInt(x)-parseInt(document.body.currentStyle.marginLeft);
}
return x;
};
RadGridNamespace.GetEventPosY=function(e){
var y=e.clientY;
var _47=RadGridNamespace.GetCurrentElement(e);
while(_47.parentNode){
if(typeof (_47.parentNode.scrollTop)=="number"){
y+=_47.parentNode.scrollTop;
}
_47=_47.parentNode;
}
if(document.body.currentStyle&&document.body.currentStyle.margin.indexOf("px")!=-1){
y=parseInt(y)-parseInt(document.body.currentStyle.marginTop);
}
return y;
};
RadGridNamespace.IsChildOf=function(_48,_49){
while(_48.parentNode){
if(_48.parentNode==_49){
return true;
}
_48=_48.parentNode;
}
return false;
};
RadGridNamespace.GetFirstParentByTagName=function(_4a,_4b){
while(_4a.parentNode){
if(_4a.tagName.toLowerCase()==_4b.toLowerCase()){
return _4a;
}
_4a=_4a.parentNode;
}
return null;
};
RadGridNamespace.FindScrollPosX=function(_4c){
var x=0;
while(_4c.parentNode){
if(typeof (_4c.parentNode.scrollLeft)=="number"){
x+=_4c.parentNode.scrollLeft;
}
_4c=_4c.parentNode;
}
if(document.body.currentStyle&&document.body.currentStyle.margin.indexOf("px")!=-1){
x=parseInt(x)-parseInt(document.body.currentStyle.marginLeft);
}
return x;
};
RadGridNamespace.FindScrollPosY=function(_4e){
var y=0;
while(_4e.parentNode){
if(typeof (_4e.parentNode.scrollTop)=="number"){
y+=_4e.parentNode.scrollTop;
}
_4e=_4e.parentNode;
}
if(document.body.currentStyle&&document.body.currentStyle.margin.indexOf("px")!=-1){
y=parseInt(y)-parseInt(document.body.currentStyle.marginTop);
}
return y;
};
RadGridNamespace.FindPosX=function(_50){
try{
var x=0;
if(_50.offsetParent){
while(_50.offsetParent){
x+=_50.offsetLeft;
_50=_50.offsetParent;
}
}else{
if(_50.x){
x+=_50.x;
}
}
return x;
}
catch(error){
return x;
}
};
RadGridNamespace.FindPosY=function(_52){
var y=0;
if(_52.offsetParent){
while(_52.offsetParent){
y+=_52.offsetTop;
_52=_52.offsetParent;
}
}else{
if(_52.y){
y+=_52.y;
}
}
return y;
};
RadGridNamespace.GetNodeNextSiblingByTagName=function(_54,_55){
while((_54!=null)&&(_54.tagName!=_55)){
_54=_54.nextSibling;
}
return _54;
};
RadGridNamespace.GetNodeNextSibling=function(_56){
while(_56!=null){
if(_56.nextSibling){
_56=_56.nextSibling;
}else{
_56=null;
}
if(_56){
if(_56.nodeType==1){
break;
}
}
}
return _56;
};
RadGridNamespace.DeleteSubString=function(_57,_58,_59){
return _57=_57.substring(0,_58)+_57.substring(_59+1,_57.length);
};
RadGridNamespace.ClearDocumentEvents=function(){
if(document.onmousedown!=this.mouseDownHandler){
this.documentOnMouseDown=document.onmousedown;
}
if(document.onselectstart!=this.selectStartHandler){
this.documentOnSelectStart=document.onselectstart;
}
if(document.ondragstart!=this.dragStartHandler){
this.documentOnDragStart=document.ondragstart;
}
this.mouseDownHandler=function(e){
return false;
};
this.selectStartHandler=function(){
return false;
};
this.dragStartHandler=function(){
return false;
};
document.onmousedown=this.mouseDownHandler;
document.onselectstart=this.selectStartHandler;
document.ondragstart=this.dragStartHandler;
};
RadGridNamespace.RestoreDocumentEvents=function(){
if((typeof (this.documentOnMouseDown)=="function")&&(document.onmousedown!=this.mouseDownHandler)){
document.onmousedown=this.documentOnMouseDown;
}else{
document.onmousedown="";
}
if((typeof (this.documentOnSelectStart)=="function")&&(document.onselectstart!=this.selectStartHandler)){
document.onselectstart=this.documentOnSelectStart;
}else{
document.onselectstart="";
}
if((typeof (this.documentOnDragStart)=="function")&&(document.ondragstart!=this.dragStartHandler)){
document.ondragstart=this.documentOnDragStart;
}else{
document.ondragstart="";
}
};
RadGridNamespace.AddStyleSheet=function(_5b){
if(RadGridNamespace.StyleSheets==null){
RadGridNamespace.StyleSheets={};
}
var _5c=RadGridNamespace.StyleSheets[_5b];
if(_5c!=null){
return null;
}
if(window.opera!=null){
return;
}
var css=null;
var _5e=null;
var _5f=document.getElementsByTagName("head")[0];
if(window.netscape){
css=document.createElement("style");
css.media="all";
css.type="text/css";
_5f.appendChild(css);
}else{
try{
css=document.createStyleSheet();
}
catch(e){
return false;
}
}
var _60=document.styleSheets[document.styleSheets.length-1];
RadGridNamespace.StyleSheets[_5b]=_60;
return _60;
};
RadGridNamespace.AddRule=function(ss,_62,_63){
try{
if(!ss){
return false;
}
if(ss.insertRule){
var _64=ss.insertRule(_62+" {"+_63+"}",ss.cssRules.length);
return ss.cssRules[ss.cssRules.length-1];
}
if(ss.addRule){
ss.addRule(_62,_63);
return true;
}
return false;
}
catch(e){
return false;
}
};
RadGridNamespace.addClassName=function(_65,_66){
var s=_65.className;
var p=s.split(" ");
if(p.length==1&&p[0]==""){
p=[];
}
var l=p.length;
for(var i=0;i<l;i++){
if(p[i]==_66){
return;
}
}
p[p.length]=_66;
_65.className=p.join(" ");
};
RadGridNamespace.removeClassName=function(_6b,_6c){
if(_6b.className.replace(/^\s*|\s*$/g,"")==_6c){
_6b.className="";
return;
}
var _6d=_6b.className.split(" ");
var _6e=[];
for(var i=0,l=_6d.length;i<l;i++){
if(_6d[i]==""){
continue;
}
if(_6c.indexOf(_6d[i])==-1){
_6e[_6e.length]=_6d[i];
}
}
_6b.className=_6e.join(" ");
return;
_6b.className=(_6b.className.toString()==_6c)?"":_6b.className.replace(_6c,"").replace(/\s*$/g,"");
return;
var p=s.split(" ");
var np=[];
var l=p.length;
var j=0;
for(var i=0;i<l;i++){
if(p[i]!=_6c){
np[j++]=p[i];
}
}
_6b.className=np.join(" ");
};
RadGridNamespace.CheckIsParentDisplay=function(_74){
try{
while(_74){
if(_74.style){
if(_74.currentStyle){
if(_74.currentStyle.display=="none"){
return false;
}
}else{
if(_74.style.display=="none"){
return false;
}
}
}
_74=_74.parentNode;
}
if(window.top){
if(window.top.location!=window.location){
return false;
}
}
return true;
}
catch(e){
return false;
}
};
if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_75,_76,_77){
if(!_77){
return;
}
if(!_75){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_77+"' />");
}else{
var _78=document.createElement("link");
_78.rel="stylesheet";
_78.type="text/css";
_78.href=_77;
var _79=document.getElementById(_76+"StyleSheetHolder");
if(_79!=null){
document.getElementById(_76+"StyleSheetHolder").appendChild(_78);
}
}
};
RadGridNamespace.RadGrid=function(_7a){
var _7b=window[_7a.ClientID];
if(_7b!=null&&typeof (_7b.Dispose)=="function"){
window.setTimeout(function(){
_7b.Dispose();
},100);
}
RadControlsNamespace.DomEventMixin.Initialize(this);
this.AttachDomEvent(window,"unload","OnWindowUnload");
window[_7a.ClientID]=this;
window["grid_"+_7a.ClientID]=this;
if(!document.readyState||document.readyState=="complete"||window.opera){
this._constructor(_7a);
}else{
this.objectData=_7a;
this.AttachDomEvent(window,"load","OnWindowLoad");
}
};
RadGridNamespace.RadGrid.prototype.OnWindowUnload=function(e){
this.Dispose();
};
RadGridNamespace.RadGrid.prototype.OnWindowLoad=function(e){
this._constructor(this.objectData);
this.objectData=null;
};
RadGridNamespace.RadGrid.prototype._constructor=function(_7e){
this.Type="RadGrid";
this.InitializeEvents(_7e.ClientSettings.ClientEvents);
RadGridNamespace.FireEvent(this,"OnGridCreating");
for(var _7f in _7e){
this[_7f]=_7e[_7f];
}
this.Initialize();
RadGridNamespace.FireEvent(this,"OnMasterTableViewCreating");
this.GridStyleSheet=RadGridNamespace.AddStyleSheet(this.ClientID);
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
var ID=_7e.MasterTableView.ClientID;
_7e.MasterTableView.ClientID=ID+"_Header";
this.MasterTableViewHeader=new RadGridNamespace.RadGridTable(_7e.MasterTableView);
this.MasterTableViewHeader._constructor(this);
if(document.getElementById(ID+"_Footer")){
_7e.MasterTableView.ClientID=ID+"_Footer";
this.MasterTableViewFooter=new RadGridNamespace.RadGridTable(_7e.MasterTableView);
this.MasterTableViewFooter._constructor(this);
}
_7e.MasterTableView.ClientID=ID;
}
this.MasterTableView._constructor(this);
RadGridNamespace.FireEvent(this,"OnMasterTableViewCreated");
this.DetailTablesCollection=new Array();
this.LoadDetailTablesCollection(this.MasterTableView,1);
this.AttachDomEvents();
RadGridNamespace.FireEvent(this,"OnGridCreated");
this.InitializeFeatures(_7e);
this.Url=this.ClientSettings.AJAXUrl;
this.EnableOutsideScripts=this.ClientSettings.EnableOutsideScripts;
if(typeof (window.event)=="undefined"){
window.event=null;
}
};
RadGridNamespace.RadGrid.prototype.Dispose=function(){
if(this.Disposed){
return;
}
this.Disposed=true;
try{
RadGridNamespace.FireEvent(this,"OnGridDestroying");
this.DisposeDomEventHandlers();
this.DisposeEvents();
this.GridStyleSheet=null;
this.DisposeFeatures();
this.DisposeDetailTablesCollection(this.MasterTableView,1);
if(this.MasterTableViewHeader!=null){
this.MasterTableViewHeader.Dispose();
}
if(this.MasterTableViewFooter!=null){
this.MasterTableViewFooter.Dispose();
}
if(this.MasterTableView!=null){
this.MasterTableView.Dispose();
}
this.DisposeProperties();
}
catch(error){
}
};
RadGridNamespace.RadGrid.ClientEventNames={OnGridCreating:true,OnGridCreated:true,OnGridDestroying:true,OnMasterTableViewCreating:true,OnMasterTableViewCreated:true,OnTableCreating:true,OnTableCreated:true,OnTableDestroying:true,OnScroll:true,OnKeyPress:true,OnRequestStart:true,OnRequestEnd:true,OnRequestError:true,OnError:true,OnRowDeleting:true,OnRowDeleted:true};
RadGridNamespace.RadGrid.prototype.IsClientEventName=function(_81){
return RadGridNamespace.RadGrid.ClientEventNames[_81]==true;
};
RadGridNamespace.RadGrid.prototype.InitializeEvents=function(_82){
for(var _83 in _82){
if(typeof (_82[_83])!="string"){
continue;
}
if(this.IsClientEventName(_83)){
if(_82[_83]!=""){
var _84=_82[_83];
if(_84.indexOf("(")!=-1){
this[_83]=_84;
}else{
this[_83]=eval(_84);
}
}else{
this[_83]=null;
}
}
}
};
RadGridNamespace.RadGrid.prototype.DisposeEvents=function(){
for(var _85 in RadGridNamespace.RadGrid.ClientEventNames){
this[_85]=null;
}
};
RadGridNamespace.RadGrid.prototype.GetDetailTable=function(_86,_87){
if(_86.HierarchyIndex==_87){
return _86;
}
if(_86.DetailTables){
for(var i=0;i<_86.DetailTables.length;i++){
var res=this.GetDetailTable(_86.DetailTables[i],_87);
if(res){
return res;
}
}
}
};
RadGridNamespace.RadGrid.prototype.LoadDetailTablesCollection=function(_8a,_8b){
try{
if(_8a.Controls[0]!=null&&_8a.Controls[0].Rows!=null){
for(var i=0;i<_8a.Controls[0].Rows.length;i++){
var _8d=_8a.Controls[0].Rows[i].ItemType;
if(_8d=="NestedView"){
var _8e=_8a.Controls[0].Rows[i].NestedTableViews;
for(var j=0;j<_8e.length;j++){
var _90=_8e[j];
if(_90.Visible){
var _91=this.GetDetailTable(this.MasterTableView,_90.HierarchyIndex);
_90.RenderColumns=_91.RenderColumns;
RadGridNamespace.FireEvent(this,"OnTableCreating",[_91]);
_90._constructor(this);
this.DetailTablesCollection[this.DetailTablesCollection.length]=_90;
if(_90.AllowFilteringByColumn){
this.InitializeFilterMenu(_90);
}
RadGridNamespace.FireEvent(this,"OnTableCreated",[_90]);
}
this.LoadDetailTablesCollection(_90,_8b+1);
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DisposeDetailTablesCollection=function(_92,_93){
if(_92.Controls[0]!=null&&_92.Controls[0].Rows!=null){
for(var i=0;i<_92.Controls[0].Rows.length;i++){
var _95=_92.Controls[0].Rows[i].ItemType;
if(_95=="NestedView"){
var _96=_92.Controls[0].Rows[i].NestedTableViews;
for(var j=0;j<_96.length;j++){
var _98=_96[j];
_98.Dispose();
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.Initialize=function(){
this.Control=document.getElementById(this.ClientID);
if(this.Control==null){
return;
}
this.GridDataDiv=document.getElementById(this.ClientID+"_GridData");
this.GroupPanelControl=document.getElementById(this.GroupPanel.ClientID+"_GroupPanel");
this.GridHeaderDiv=document.getElementById(this.ClientID+"_GridHeader");
this.GridFooterDiv=document.getElementById(this.ClientID+"_GridFooter");
this.PostDataValue=document.getElementById(this.ClientID+"PostDataValue");
this.LoadingTemplate=document.getElementById(this.ClientID+"_LoadingTemplate");
this.PagerControl=document.getElementById(this.MasterTableView.ClientID+"_Pager");
this.TopPagerControl=document.getElementById(this.MasterTableView.ClientID+"_TopPager");
if(this.LoadingTemplate){
this.LoadingTemplate.style.display="none";
if(this.GridDataDiv){
this.GridDataDiv.appendChild(this.LoadingTemplate);
}
}
this.FormID=this.ClientSettings.FormID;
};
RadGridNamespace.RadGrid.prototype.DisposeProperties=function(){
this.Control=null;
this.GridDataDiv=null;
this.GroupPanelControl=null;
this.GridHeaderDiv=null;
this.GridFooterDiv=null;
this.PostDataValue=null;
this.LoadingTemplate=null;
this.PagerControl=null;
};
RadGridNamespace.RadGrid.prototype.InitializeFeatures=function(_99){
if(!this.MasterTableView.Control){
return;
}
if(this.GroupPanelControl!=null){
this.GroupPanelObject=new RadGridNamespace.RadGridGroupPanel(this.GroupPanelControl,this);
}
if(this.ClientSettings.Scrolling.AllowScroll){
this.InitializeDimensions();
this.InitializeScroll();
}
if(this.Control.align==""){
var _9a=RadGridNamespace.IsParentRightToLeft(this.GridHeaderDiv);
if(!_9a){
this.Control.align="left";
}else{
this.Control.align="right";
}
}
if(this.AllowFilteringByColumn||this.MasterTableView.AllowFilteringByColumn){
var _9b=(this.MasterTableViewHeader)?this.MasterTableViewHeader:this.MasterTableView;
this.InitializeFilterMenu(_9b);
}
if(this.ClientSettings.AllowKeyboardNavigation&&this.MasterTableView.Rows){
if(!this.MasterTableView.RenderActiveItemStyleClass||this.MasterTableView.RenderActiveItemStyleClass==""){
if(this.MasterTableView.RenderActiveItemStyle&&this.MasterTableView.RenderActiveItemStyle!=""){
RadGridNamespace.AddRule(this.GridStyleSheet,".ActiveItemStyle"+this.MasterTableView.ClientID+"1 td",this.MasterTableView.RenderActiveItemStyle);
}else{
RadGridNamespace.AddRule(this.GridStyleSheet,".ActiveItemStyle"+this.MasterTableView.ClientID+"2 td","background-color:#FFA07A;");
}
}
if(this.ActiveRow==null){
this.ActiveRow=this.MasterTableView.Rows[0];
}
this.SetActiveRow(this.ActiveRow);
}
if(window[this.ClientID+"_Slider"]){
this.Slider=new RadGridNamespace.Slider(window[this.ClientID+"_Slider"]);
}
};
RadGridNamespace.RadGrid.prototype.DisposeFeatures=function(){
if(this.Slider!=null){
this.Slider.Dispose();
this.Slider=null;
}
if(this.GroupPanelControl!=null){
this.GroupPanelObject.Dispose();
this.GroupPanelControl=null;
}
if(this.AllowFilteringByColumn||this.MasterTableView.AllowFilteringByColumn){
var _9c=(this.MasterTableViewHeader)?this.MasterTableViewHeader:this.MasterTableView;
this.DisposeFilterMenu(_9c);
}
this.Control=null;
};
RadGridNamespace.RadGrid.prototype.AsyncRequest=function(_9d,_9e){
var _9f;
if(this.StatusBarSettings!=null&&this.StatusBarSettings.StatusLabelID!=null&&this.StatusBarSettings.StatusLabelID!=""){
var _a0=document.getElementById(this.StatusBarSettings.StatusLabelID);
if(_a0!=null){
_9f=_a0.innerHTML;
_a0.innerHTML=this.StatusBarSettings.LoadingText;
}
}
var _a1=this.ClientID;
this.OnRequestEndInternal=function(){
RadGridNamespace.FireEvent(window[_a1],"OnRequestEnd");
if(_a0){
_a0.innerHTML=_9f;
}
};
RadAjaxNamespace.AsyncRequest(_9d,_9e,_a1);
};
RadGridNamespace.RadGrid.prototype.AjaxRequest=function(_a2,_a3){
this.AsyncRequest(_a2,_a3);
};
RadGridNamespace.RadGrid.prototype.ClearSelectedRows=function(){
for(var i=0;i<this.DetailTablesCollection.length;i++){
var _a5=this.DetailTablesCollection[i];
_a5.ClearSelectedRows();
}
this.MasterTableView.ClearSelectedRows();
};
RadGridNamespace.RadGrid.prototype.AsyncRequestWithOptions=function(_a6){
RadAjaxNamespace.AsyncRequestWithOptions(_a6,this.ClientID);
};
RadGridNamespace.RadGrid.prototype.DeleteRow=function(_a7,_a8,e){
var _aa=(e.srcElement)?e.srcElement:e.target;
if(!_aa){
return;
}
var row=_aa.parentNode.parentNode;
var _ac=row.parentNode.parentNode;
var _ad=row.rowIndex;
var _ae=row.cells.length;
var _af=this.GetTableObjectByID(_a7);
var _b0=this.GetRowObjectByRealRow(_af,row);
var _b1={Row:_b0};
if(!RadGridNamespace.FireEvent(this,"OnRowDeleting",[_af,_b1])){
return;
}
_ac.deleteRow(row.rowIndex);
for(var i=_ad;i<_ac.rows.length;i++){
if(_ac.rows[i].cells.length!=_ae&&_ac.rows[i].style.display!="none"){
_ac.deleteRow(i);
i--;
}else{
break;
}
}
if(_ac.tBodies[0].rows.length==1&&_ac.tBodies[0].rows[0].style.display=="none"){
_ac.tBodies[0].rows[0].style.display="";
}
this.PostDataValue.value+="DeletedRows,"+_a7+","+_a8+";";
RadGridNamespace.FireEvent(this,"OnRowDeleted",[_af,_b1]);
};
RadGridNamespace.RadGrid.prototype.SelectRow=function(_b3,_b4,e){
var _b6=(e.srcElement)?e.srcElement:e.target;
if(!_b6){
return;
}
var row=_b6.parentNode.parentNode;
var _b8=row.parentNode.parentNode;
var _b9=row.rowIndex;
var _ba;
if(_b3==this.MasterTableView.UID){
_ba=this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].ClientID==_b8.id){
_ba=this.DetailTablesCollection[i];
break;
}
}
}
if(_ba!=null){
if(this.AllowMultiRowSelection){
_ba.SelectRow(row,false);
}else{
_ba.SelectRow(row,true);
}
}
};
RadGridNamespace.RadGrid.prototype.SelectAllRows=function(_bc,_bd,e){
var _bf=(e.srcElement)?e.srcElement:e.target;
if(!_bf){
return;
}
var row=_bf.parentNode.parentNode;
var _c1=row.parentNode.parentNode;
var _c2=row.rowIndex;
var _c3;
if(_bc==this.MasterTableView.UID){
_c3=this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].UID==_bc){
_c3=this.DetailTablesCollection[i];
break;
}
}
}
if(_c3!=null){
if(this.AllowMultiRowSelection){
if(_c3==this.MasterTableViewHeader){
_c3=this.MasterTableView;
}
_c3.ClearSelectedRows();
if(_bf.checked){
for(var i=0;i<_c3.Control.tBodies[0].rows.length;i++){
var row=_c3.Control.tBodies[0].rows[i];
_c3.SelectRow(row,false);
}
}else{
for(var i=0;i<_c3.Control.tBodies[0].rows.length;i++){
var row=_c3.Control.tBodies[0].rows[i];
_c3.DeselectRow(row);
}
this.SavePostData("SelectedRows",_c3.ClientID,"");
}
}
}
};
RadGridNamespace.RadGrid.prototype.HandleActiveRow=function(e){
if((this.AllowRowResize)||(this.AllowRowSelect)){
var _c6=this.GetCellFromPoint(e);
if((_c6!=null)&&(_c6.parentNode.id!="")&&(_c6.parentNode.id!=-1)&&(_c6.cellIndex==0)){
var _c7=_c6.parentNode.parentNode.parentNode;
this.SetActiveRow(_c7,_c6.parentNode.rowIndex);
}
}
};
RadGridNamespace.RadGrid.prototype.SetActiveRow=function(_c8){
if(_c8==null){
return;
}
if(_c8.Owner.RenderActiveItemStyle){
RadGridNamespace.removeClassName(this.ActiveRow.Control,"ActiveItemStyle"+_c8.Owner.ClientID+"1");
}else{
RadGridNamespace.removeClassName(this.ActiveRow.Control,"ActiveItemStyle"+_c8.Owner.ClientID+"2");
}
RadGridNamespace.removeClassName(this.ActiveRow.Control,_c8.Owner.RenderActiveItemStyleClass);
if(this.ActiveRow.Control.style.cssText==_c8.Owner.RenderActiveItemStyle){
this.ActiveRow.Control.style.cssText="";
}
this.ActiveRow=_c8;
if(!this.ActiveRow.Owner.RenderActiveItemStyleClass||this.ActiveRow.Owner.RenderActiveItemStyleClass==""){
if(this.ActiveRow.Owner.RenderActiveItemStyle&&this.ActiveRow.Owner.RenderActiveItemStyle!=""){
RadGridNamespace.addClassName(this.ActiveRow.Control,"ActiveItemStyle"+this.ActiveRow.Owner.ClientID+"1");
}else{
RadGridNamespace.addClassName(this.ActiveRow.Control,"ActiveItemStyle"+this.ActiveRow.Owner.ClientID+"2");
}
}else{
RadGridNamespace.addClassName(this.ActiveRow.Control,this.ActiveRow.Owner.RenderActiveItemStyleClass);
}
this.SavePostData("ActiveRow",this.ActiveRow.Owner.ClientID,this.ActiveRow.RealIndex);
};
RadGridNamespace.RadGrid.prototype.GetNextRow=function(_c9,_ca){
if(_c9!=null){
if(_c9.tBodies[0].rows[_ca]!=null){
while(_c9.tBodies[0].rows[_ca]!=null){
_ca++;
if(_ca<=(_c9.tBodies[0].rows.length-1)){
return _c9.tBodies[0].rows[_ca];
}else{
return null;
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetPreviousRow=function(_cb,_cc){
if(_cb!=null){
if(_cb.tBodies[0].rows[_cc]!=null){
while(_cb.tBodies[0].rows[_cc]!=null){
_cc--;
if(_cc>=0){
return _cb.tBodies[0].rows[_cc];
}else{
return null;
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetNextHierarchicalRow=function(_cd,_ce){
if(_cd!=null){
if(_cd.tBodies[0].rows[_ce]!=null){
_ce++;
var row=_cd.tBodies[0].rows[_ce];
if(_cd.tBodies[0].rows[_ce]!=null){
if((row.cells[1]!=null)&&(row.cells[2]!=null)){
if((row.cells[1].getElementsByTagName("table").length>0)||(row.cells[2].getElementsByTagName("table").length>0)){
var _d0=this.GetNextRow(row.cells[2].firstChild,0);
return _d0;
}else{
return null;
}
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetPreviousHierarchicalRow=function(_d1,_d2){
if(_d1!=null){
if(_d1.parentNode!=null){
if(_d1.parentNode.tagName.toLowerCase()=="td"){
var _d3=_d1.parentNode.parentNode.parentNode.parentNode;
var _d4=_d1.parentNode.parentNode.rowIndex;
return this.GetPreviousRow(_d3,_d4);
}else{
return null;
}
}else{
return this.GetPreviousRow(_d1,_d2);
}
}
};
RadGridNamespace.RadGrid.prototype.HandleCellEdit=function(e){
var _d6=RadGridNamespace.GetCurrentElement(e);
var _d7=RadGridNamespace.GetFirstParentByTagName(_d6,"td");
if(_d7!=null){
_d6=_d7;
var _d8=_d6.parentNode.parentNode.parentNode;
var _d9=this.GetTableObjectByID(_d8.id);
if((_d9!=null)&&(_d9.Columns.length>0)&&(_d9.Columns[_d6.cellIndex]!=null)){
if(_d9.Columns[_d6.cellIndex].ColumnType!="GridBoundColumn"){
return;
}
this.EditedCell=_d9.Control.rows[_d6.parentNode.rowIndex].cells[_d6.cellIndex];
this.CellEditor=new RadGridNamespace.RadGridCellEditor(this.EditedCell,_d9.Columns[_d6.cellIndex],this);
}
}
};
RadGridNamespace.RadGridCellEditor=function(_da,_db,_dc){
if(_dc.CellEditor){
return;
}
this.Control=document.createElement("input");
this.Control.style.border="1px groove";
this.Control.style.width="100%";
this.Control.value=_da.innerHTML;
this.OldValue=this.Control.value;
_da.innerHTML="";
var _dd=this;
this.Control.onblur=function(e){
if(!e){
var e=window.event;
}
_da.removeChild(this);
_da.innerHTML=this.value;
if(this.value!=_dd.OldValue){
alert(1);
}
_dc.CellEditor=null;
};
_da.appendChild(this.Control);
if(this.Control.focus){
this.Control.focus();
}
};
if(!("console" in window)||!("firebug" in console)){
var names=["log","debug","info","warn","error","assert","dir","dirxml","group","groupEnd","time","timeEnd","count","trace","profile","profileEnd"];
window.console={};
for(var i=0;i<names.length;++i){
window.console[names[i]]=function(){
};
}
}
RadGridNamespace.Error=function(_df,_e0,_e1){
if((!_df)||(!_e0)||(!_e1)){
return false;
}
this.Message=_df.message;
if(_e1!=null){
if("string"==typeof (_e1)){
try{
eval(_e1);
}
catch(e){
var _e2="";
_e2="";
_e2+="Telerik RadGrid Error:\r\n";
_e2+="-----------------\r\n";
_e2+="Message: \""+e.message+"\"\r\n";
_e2+="Raised by: "+_e0.Type+"\r\n";
alert(_e2);
}
}else{
if("function"==typeof (_e1)){
try{
_e1(this);
}
catch(e){
var _e2="";
_e2="";
_e2+="Telerik RadGrid Error:\r\n";
_e2+="-----------------\r\n";
_e2+="Message: \""+e.message+"\"\r\n";
_e2+="Raised by: "+_e0.Type+"\r\n";
alert(_e2);
}
}
}
}else{
this.Owner=_e0;
for(var _e3 in _df){
this[_e3]=_df[_e3];
}
this.Message="";
this.Message+="Telerik RadGrid Error:\r\n";
this.Message+="-----------------\r\n";
this.Message+="Message: \""+_df.message+"\"\r\n";
this.Message+="Raised by: "+_e0.Type+"\r\n";
alert(this.Message);
}
};
RadGridNamespace.RadGrid.prototype.GetTableObjectByID=function(id){
if(this.MasterTableView.ClientID==id||this.MasterTableView.UID==id){
return this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].ClientID==id||this.DetailTablesCollection[i].UID==id){
return this.DetailTablesCollection[i];
}
}
}
if(this.MasterTableViewHeader!=null){
if(this.MasterTableViewHeader.ClientID==id||this.MasterTableViewHeader.UID==id){
return table=this.MasterTableViewHeader;
}
}
};
RadGridNamespace.RadGrid.prototype.GetRowObjectByRealRow=function(_e6,row){
if(_e6.Rows!=null){
for(var i=0;i<_e6.Rows.length;i++){
if(_e6.Rows[i].Control==row){
return _e6.Rows[i];
}
}
}
};
RadGridNamespace.RadGrid.prototype.SavePostData=function(){
try{
var _e9=new String();
for(var i=0;i<arguments.length;i++){
_e9+=arguments[i]+",";
}
_e9=_e9.substring(0,_e9.length-1);
if(this.PostDataValue!=null){
switch(arguments[0]){
case "ReorderedColumns":
this.PostDataValue.value+=_e9+";";
break;
case "HidedColumns":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="ShowedColumns"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ShowedColumns":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="HidedColumns"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "HidedRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="ShowedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ShowedRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="HidedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ResizedColumns":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ResizedRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ResizedControl":
var _eb=arguments[0]+","+arguments[1];
this.UpdatePostData(_e9,_eb);
break;
case "ClientCreated":
var _eb=arguments[0]+","+arguments[1];
this.UpdatePostData(_e9,_eb);
break;
case "ScrolledControl":
var _eb=arguments[0]+","+arguments[1];
this.UpdatePostData(_e9,_eb);
break;
case "AJAXScrolledControl":
var _eb=arguments[0]+","+arguments[1];
this.UpdatePostData(_e9,_eb);
break;
case "SelectedRows":
var _eb=arguments[0]+","+arguments[1]+",";
this.UpdatePostData(_e9,_eb);
break;
case "EditRow":
var _eb=arguments[0]+","+arguments[1];
this.UpdatePostData(_e9,_eb);
break;
case "ActiveRow":
var _eb=arguments[0]+","+arguments[1];
this.UpdatePostData(_e9,_eb);
break;
case "CollapsedRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="ExpandedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ExpandedRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="CollapsedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "CollapsedGroupRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="ExpandedGroupRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
case "ExpandedGroupRows":
var _eb=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
_eb="CollapsedGroupRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_e9,_eb);
break;
default:
this.UpdatePostData(_e9,_e9);
break;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.UpdatePostData=function(_ec,_ed){
var _ee,_ef=new Array();
_ee=this.PostDataValue.value.split(";");
for(var i=0;i<_ee.length;i++){
if(_ee[i].indexOf(_ed)==-1){
_ef[_ef.length]=_ee[i];
}
}
this.PostDataValue.value=_ef.join(";");
this.PostDataValue.value+=_ec+";";
};
RadGridNamespace.RadGrid.prototype.DeletePostData=function(_f1,_f2){
var _f3,_f4=new Array();
_f3=this.PostDataValue.value.split(";");
for(var i=0;i<_f3.length;i++){
if(_f3[i].indexOf(_f2)==-1){
_f4[_f4.length]=_f3[i];
}
}
this.PostDataValue.value=_f4.join(";");
};
RadGridNamespace.RadGrid.prototype.HandleDragAndDrop=function(e,_f7){
try{
var _f8=this;
if((_f7!=null)&&(_f7.tagName.toLowerCase()=="th")){
var _f9=_f7.parentNode.parentNode.parentNode;
var _fa=this.GetTableObjectByID(_f9.id);
if((_fa!=null)&&(_fa.Columns.length>0)&&(_fa.Columns[_f7.cellIndex]!=null)&&((_fa.Columns[_f7.cellIndex].Reorderable)||(_fa.Owner.ClientSettings.AllowDragToGroup&&_fa.Columns[_f7.cellIndex].Groupable))){
var _fb=RadGridNamespace.GetEventPosX(e);
var _fc=RadGridNamespace.FindPosX(_f7);
var _fd=_fc+_f7.offsetWidth;
this.ResizeTolerance=5;
var _fe=_f7.title;
var _ff=_f7.style.cursor;
if(!((_fb>=_fd-this.ResizeTolerance)&&(_fb<=_fd+this.ResizeTolerance))){
if(this.MoveHeaderDiv){
if(this.MoveHeaderDiv.innerHTML!=_f7.innerHTML){
_f7.title=this.ClientSettings.ClientMessages.DropHereToReorder;
_f7.style.cursor="default";
if(_f7.parentNode.parentNode.parentNode==this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode){
this.MoveReorderIndicators(e,_f7);
}else{
if(this.ReorderIndicator1!=null){
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
}
if(this.ReorderIndicator2!=null){
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
}
}
}
}else{
_f7.title=this.ClientSettings.ClientMessages.DragToGroupOrReorder;
_f7.style.cursor="move";
}
this.AttachDomEvent(_f7,"mousedown","OnDragDropMouseDown");
}else{
_f7.style.cursor=_ff;
_f7.title="";
}
}
}
if(this.MoveHeaderDiv!=null){
this.MoveHeaderDiv.style.visibility="";
this.MoveHeaderDiv.style.display="";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.PositionDragElement=function(_100,_101){
_100.style.top=_101.clientY+document.documentElement.scrollTop+document.body.scrollTop+1+"px";
_100.style.left=_101.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+1+"px";
};
RadGridNamespace.RadGrid.prototype.OnDragDropMouseDown=function(e){
var _103=RadGridNamespace.GetCurrentElement(e);
var _104=false;
var form=document.getElementById(this.FormID);
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value!=""){
_104=true;
}
if((_103.tagName.toLowerCase()=="input"&&_103.type.toLowerCase()=="text")||(_103.tagName.toLowerCase()=="textarea")){
return;
}
_103=RadGridNamespace.GetFirstParentByTagName(_103,"th");
if(_103.tagName.toLowerCase()=="th"&&!this.IsResize){
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
this.CreateDragAndDrop(e,_103);
}
RadGridNamespace.ClearDocumentEvents();
this.DetachDomEvent(_103,"mousedown","OnDragDropMouseDown");
this.AttachDomEvent(document,"mouseup","OnDragDropMouseUp");
if(this.GroupPanelControl!=null){
this.AttachDomEvent(this.GroupPanelControl,"mouseup","OnDragDropMouseUp");
}
}
};
RadGridNamespace.RadGrid.prototype.OnDragDropMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnDragDropMouseUp");
if(this.GroupPanelControl!=null){
this.DetachDomEvent(this.GroupPanelControl,"mouseup","OnDragDropMouseUp");
}
this.FireDropAction(e);
this.DestroyDragAndDrop(e);
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.CopyAttributes=function(_107,_108){
for(var i=0;i<_108.attributes.length;i++){
try{
if(_108.attributes[i].name.toLowerCase()=="id"){
continue;
}
if(_108.attributes[i].value!=null&&_108.attributes[i].value!="null"&&_108.attributes[i].value!=""){
_107.setAttribute(_108.attributes[i].name,_108.attributes[i].value);
}
}
catch(e){
continue;
}
}
};
RadGridNamespace.RadGrid.prototype.CreateDragAndDrop=function(e,_10b){
this.MoveHeaderDivRefCell=_10b;
this.MoveHeaderDiv=document.createElement("div");
var _10c=document.createElement("table");
if(this.MoveHeaderDiv.mergeAttributes){
this.MoveHeaderDiv.mergeAttributes(this.Control);
}else{
RadGridNamespace.CopyAttributes(this.MoveHeaderDiv,this.Control);
}
if(_10c.mergeAttributes){
_10c.mergeAttributes(this.MasterTableView.Control);
}else{
RadGridNamespace.CopyAttributes(_10c,this.MasterTableView.Control);
}
_10c.style.margin="0px";
_10c.style.height=_10b.offsetHeight+"px";
_10c.style.width=_10b.offsetWidth+"px";
var _10d=document.createElement("thead");
var tr=document.createElement("tr");
_10c.appendChild(_10d);
_10d.appendChild(tr);
tr.appendChild(_10b.cloneNode(true));
this.MoveHeaderDiv.appendChild(_10c);
document.body.appendChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.style.height=_10b.offsetHeight+"px";
this.MoveHeaderDiv.style.width=_10b.offsetWidth+"px";
this.MoveHeaderDiv.style.position="absolute";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
if(window.netscape){
this.MoveHeaderDiv.style.MozOpacity=3/4;
}else{
this.MoveHeaderDiv.style.filter="alpha(opacity=75);";
}
this.MoveHeaderDiv.style.cursor="move";
this.MoveHeaderDiv.style.visibility="hidden";
this.MoveHeaderDiv.style.display="none";
this.MoveHeaderDiv.style.fontWeight="bold";
this.MoveHeaderDiv.onmousedown=null;
RadGridNamespace.ClearDocumentEvents();
if(this.ClientSettings.AllowColumnsReorder){
this.CreateReorderIndicators(_10b);
}
};
RadGridNamespace.RadGrid.prototype.DestroyDragAndDrop=function(){
if(this.MoveHeaderDiv!=null){
var _10f=this.MoveHeaderDiv.parentNode;
_10f.removeChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.onmouseup=null;
this.MoveHeaderDiv.onmousemove=null;
this.MoveHeaderDiv=null;
this.MoveHeaderDivRefCell=null;
this.DragCellIndex=null;
RadGridNamespace.RestoreDocumentEvents();
this.DestroyReorderIndicators();
}
};
RadGridNamespace.RadGrid.prototype.FireDropAction=function(e){
if((this.MoveHeaderDiv!=null)&&(this.MoveHeaderDiv.style.display!="none")){
var _111=RadGridNamespace.GetCurrentElement(e);
if((_111!=null)&&(this.MoveHeaderDiv!=null)){
if(_111!=this.MoveHeaderDivRefCell){
var _112=this.GetTableObjectByID(this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode.id);
var _113=_112.HeaderRow;
if(RadGridNamespace.IsChildOf(_111,_113)){
if(_111.tagName.toLowerCase()!="th"){
_111=RadGridNamespace.GetFirstParentByTagName(_111,"th");
}
var _114=_111.parentNode.parentNode.parentNode;
var _115=this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode;
if(_114.id==_115.id){
var _116=this.GetTableObjectByID(_114.id);
var _117=_111.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_117=RadGridNamespace.GetRealCellIndex(_116,_111);
}
var _118=this.MoveHeaderDivRefCell.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_118=RadGridNamespace.GetRealCellIndex(_116,this.MoveHeaderDivRefCell);
}
if(!_116||!_116.Columns[_117]){
return;
}
if(!_116.Columns[_117].Reorderable){
return;
}
_116.SwapColumns(_117,_118,(this.ClientSettings.ColumnsReorderMethod!="Reorder"));
if(this.ClientSettings.ColumnsReorderMethod=="Reorder"){
if((!this.ClientSettings.ReorderColumnsOnClient)&&(this.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
eval(this.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
}
}
}else{
if(RadGridNamespace.CheckParentNodesFor(_111,this.GroupPanelControl)){
if((this.ClientSettings.PostBackReferences.PostBackGroupByColumn!="")&&(this.ClientSettings.AllowDragToGroup)){
var _116=this.GetTableObjectByID(this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode.id);
var _119=this.MoveHeaderDivRefCell.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_119=RadGridNamespace.GetRealCellIndex(_116,this.MoveHeaderDivRefCell);
}
var _11a=_116.Columns[_119].RealIndex;
if(_116.Columns[_119].Groupable){
if(_116==this.MasterTableViewHeader){
this.SavePostData("GroupByColumn",this.MasterTableView.ClientID,_11a);
}else{
this.SavePostData("GroupByColumn",_116.ClientID,_11a);
}
eval(this.ClientSettings.PostBackReferences.PostBackGroupByColumn);
}
}
}
}
}
}
}
};
RadGridNamespace.GetRealCellIndex=function(_11b,cell){
for(var i=0;i<_11b.Columns.length;i++){
if(_11b.Columns[i].Control==cell){
return i;
}
}
};
RadGridNamespace.RadGrid.prototype.CreateReorderIndicators=function(_11e){
if((this.ReorderIndicator1==null)&&(this.ReorderIndicator2==null)){
var _11f=this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode;
var _120=this.GetTableObjectByID(_11f.id);
var _121=_120.HeaderRow;
if(!RadGridNamespace.IsChildOf(_11e,_121)){
return;
}
this.ReorderIndicator1=document.createElement("span");
this.ReorderIndicator2=document.createElement("span");
this.ReorderIndicator1.innerHTML="&darr;";
this.ReorderIndicator2.innerHTML="&uarr;";
this.ReorderIndicator1.style.backgroundColor="transparent";
this.ReorderIndicator1.style.color="darkblue";
this.ReorderIndicator1.style.font="bold 18px Arial";
this.ReorderIndicator2.style.backgroundColor=this.ReorderIndicator1.style.backgroundColor;
this.ReorderIndicator2.style.color=this.ReorderIndicator1.style.color;
this.ReorderIndicator2.style.font=this.ReorderIndicator1.style.font;
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_11e)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_11e)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_11e)+_11e.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
document.body.appendChild(this.ReorderIndicator1);
document.body.appendChild(this.ReorderIndicator2);
}
};
RadGridNamespace.RadGrid.prototype.DestroyReorderIndicators=function(){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
document.body.removeChild(this.ReorderIndicator1);
document.body.removeChild(this.ReorderIndicator2);
this.ReorderIndicator1=null;
this.ReorderIndicator2=null;
}
};
RadGridNamespace.RadGrid.prototype.MoveReorderIndicators=function(e,_123){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
this.ReorderIndicator1.style.visibility="visible";
this.ReorderIndicator1.style.display="";
this.ReorderIndicator2.style.visibility="visible";
this.ReorderIndicator2.style.display="";
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_123)-RadGridNamespace.FindScrollPosY(_123)+document.documentElement.scrollTop+document.body.scrollTop-_123.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_123)-RadGridNamespace.FindScrollPosX(_123)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
if(parseInt(this.ReorderIndicator1.style.left)<RadGridNamespace.FindPosX(this.Control)){
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(this.Control)+5;
}
this.ReorderIndicator2.style.top=parseInt(this.ReorderIndicator1.style.top)+_123.offsetHeight*2+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
}
};
RadGridNamespace.RadGrid.prototype.AttachDomEvents=function(){
try{
this.AttachDomEvent(this.Control,"mousemove","OnMouseMove");
this.AttachDomEvent(document,"keydown","OnKeyDown");
this.AttachDomEvent(document,"keyup","OnKeyUp");
this.AttachDomEvent(this.Control,"click","OnClick");
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnMouseMove=function(e){
try{
var _125=RadGridNamespace.GetCurrentElement(e);
if(this.ClientSettings.Resizing.AllowRowResize){
this.DetectResizeCursorsOnRows(e,_125);
this.MoveRowResizer(e);
}
if((this.ClientSettings.AllowDragToGroup)||(this.ClientSettings.AllowColumnsReorder)){
this.HandleDragAndDrop(e,_125);
}
}
catch(error){
return false;
}
};
RadGridNamespace.RadGrid.prototype.OnKeyDown=function(e){
var _127={KeyCode:e.keyCode,IsShiftPressed:e.shiftKey,IsCtrlPressed:e.ctrlKey,IsAltPressed:e.altKey,Event:e};
if(!RadGridNamespace.FireEvent(this,"OnKeyPress",[_127])){
return;
}
if(e.keyCode==16){
this.IsShiftPressed=true;
}
if(e.keyCode==17){
this.IsCtrlPressed=true;
}
if(this.ClientSettings.AllowKeyboardNavigation){
this.ActiveRow.HandleActiveRow(e);
}
};
RadGridNamespace.RadGrid.prototype.OnClick=function(e){
};
RadGridNamespace.RadGrid.prototype.OnKeyUp=function(e){
if(e.keyCode==16){
this.IsShiftPressed=false;
}
if(e.keyCode==17){
this.IsCtrlPressed=false;
}
};
RadGridNamespace.RadGrid.prototype.DetectResizeCursorsOnRows=function(e,_12b){
try{
var _12c=this;
if((_12b!=null)&&(_12b.tagName.toLowerCase()=="td")){
var _12d=_12b.parentNode.parentNode.parentNode;
var _12e=this.GetTableObjectByID(_12d.id);
if(_12e!=null){
if(_12e.Columns!=null){
if(_12e.Columns[_12b.cellIndex].ColumnType!="GridRowIndicatorColumn"){
return;
}
}
if(!_12e.Control.tBodies[0]){
return;
}
var _12f=this.GetRowObjectByRealRow(_12e,_12b.parentNode);
if(_12f!=null){
var _130=RadGridNamespace.GetEventPosY(e);
var _131=RadGridNamespace.FindPosY(_12b);
var endY=_131+_12b.offsetHeight;
this.ResizeTolerance=5;
var _133=_12b.title;
if((_130>endY-this.ResizeTolerance)&&(_130<endY+this.ResizeTolerance)){
_12b.style.cursor="n-resize";
_12b.title=this.ClientSettings.ClientMessages.DragToResize;
this.AttachDomEvent(_12b,"mousedown","OnResizeMouseDown");
}else{
_12b.style.cursor="default";
_12b.title="";
this.DetachDomEvent(_12b,"mousedown","OnResizeMouseDown");
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnResizeMouseDown=function(e){
this.CreateRowResizer(e);
RadGridNamespace.ClearDocumentEvents();
this.AttachDomEvent(document,"mouseup","OnResizeMouseUp");
};
RadGridNamespace.RadGrid.prototype.OnResizeMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnResizeMouseUp");
this.DestroyRowResizerAndResizeRow(e,true);
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.RadGrid.prototype.CreateRowResizer=function(e){
try{
this.DestroyRowResizer();
var _137=RadGridNamespace.GetCurrentElement(e);
if((_137!=null)&&(_137.tagName.toLowerCase()=="td")){
if(_137.cellIndex>0){
var _138=_137.parentNode.rowIndex;
_137=_137.parentNode.parentNode.parentNode.rows[_138].cells[0];
}
this.RowResizer=null;
this.CellToResize=_137;
var _139=_137.parentNode.parentNode.parentNode;
var _13a=this.GetTableObjectByID(_139.id);
this.RowResizer=document.createElement("div");
this.RowResizer.style.backgroundColor="navy";
this.RowResizer.style.height="1px";
this.RowResizer.style.fontSize="1";
this.RowResizer.style.position="absolute";
this.RowResizer.style.cursor="n-resize";
if(_13a!=null){
this.RowResizerRefTable=_13a;
if(this.GridDataDiv){
this.RowResizer.style.left=RadGridNamespace.FindPosX(this.GridDataDiv)+"px";
var _13b=(RadGridNamespace.FindPosX(this.GridDataDiv)+this.GridDataDiv.offsetWidth)-parseInt(this.RowResizer.style.left);
if(_13b>_13a.Control.offsetWidth){
this.RowResizer.style.width=_13a.Control.offsetWidth+"px";
}else{
this.RowResizer.style.width=_13b+"px";
}
if(parseInt(this.RowResizer.style.width)>this.GridDataDiv.offsetWidth){
this.RowResizer.style.width=this.GridDataDiv.offsetWidth+"px";
}
}else{
this.RowResizer.style.width=_13a.Control.offsetWidth+"px";
this.RowResizer.style.left=RadGridNamespace.FindPosX(_137)+"px";
}
}
this.RowResizer.style.top=RadGridNamespace.GetEventPosY(e)-(RadGridNamespace.GetEventPosY(e)-e.clientY)+document.body.scrollTop+document.documentElement.scrollTop+"px";
var _13c=document.body;
_13c.appendChild(this.RowResizer);
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DestroyRowResizerAndResizeRow=function(e,_13e){
try{
if((this.CellToResize!="undefined")&&(this.CellToResize!=null)&&(this.CellToResize.tagName.toLowerCase()=="td")&&(this.RowResizer!="undefined")&&(this.RowResizer!=null)){
var _13f;
if(this.GridDataDiv){
_13f=parseInt(this.RowResizer.style.top)+this.GridDataDiv.scrollTop-(RadGridNamespace.FindPosY(this.CellToResize));
}else{
_13f=parseInt(this.RowResizer.style.top)-(RadGridNamespace.FindPosY(this.CellToResize));
}
if(_13f>0){
var _140=this.CellToResize.parentNode.parentNode.parentNode;
var _141=this.GetTableObjectByID(_140.id);
if(_141!=null){
_141.ResizeRow(this.CellToResize.parentNode.rowIndex,_13f);
}
}
}
if(_13e){
this.DestroyRowResizer();
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DestroyRowResizer=function(){
try{
if((this.RowResizer!="undefined")&&(this.RowResizer!=null)&&(this.RowResizer.parentNode!=null)){
var _142=this.RowResizer.parentNode;
_142.removeChild(this.RowResizer);
this.RowResizer=null;
this.RowResizerRefTable=null;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.MoveRowResizer=function(e){
try{
if((this.RowResizer!="undefined")&&(this.RowResizer!=null)&&(this.RowResizer.parentNode!=null)){
this.RowResizer.style.top=RadGridNamespace.GetEventPosY(e)-(RadGridNamespace.GetEventPosY(e)-e.clientY)+document.body.scrollTop+document.documentElement.scrollTop+"px";
if(this.ClientSettings.Resizing.EnableRealTimeResize){
this.DestroyRowResizerAndResizeRow(e,false);
this.UpdateRowResizerWidth(e);
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.UpdateRowResizerWidth=function(e){
var _145=RadGridNamespace.GetCurrentElement(e);
if((_145!=null)&&(_145.tagName.toLowerCase()=="td")){
var _146=this.RowResizerRefTable;
if(_146!=null){
if(this.GridDataDiv){
var _147=(RadGridNamespace.FindPosX(this.GridDataDiv)+this.GridDataDiv.offsetWidth)-parseInt(this.RowResizer.style.left);
if(_147>_146.Control.offsetWidth){
this.RowResizer.style.width=_146.Control.offsetWidth+"px";
}else{
this.RowResizer.style.width=_147+"px";
}
if(parseInt(this.RowResizer.style.width)>this.GridDataDiv.offsetWidth){
this.RowResizer.style.width=this.GridDataDiv.offsetWidth+"px";
}
}else{
this.RowResizer.style.width=_146.Control.offsetWidth+"px";
}
}
}
};
RadGridNamespace.RadGrid.prototype.SetHeaderAndFooterDivsWidth=function(){
if((document.compatMode=="BackCompat"&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1)||(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1)){
if(this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.GridHeaderDiv!=null&&this.GridDataDiv!=null&&this.GridHeaderDiv!=null){
this.GridHeaderDiv.style.width="100%";
if(this.GridHeaderDiv&&this.GridDataDiv){
if(this.GridDataDiv.offsetWidth>0){
this.GridHeaderDiv.style.width=this.GridDataDiv.offsetWidth-RadGridNamespace.GetScrollBarWidth()+"px";
}
}
if(this.GridHeaderDiv&&this.GridFooterDiv){
this.GridFooterDiv.style.width=this.GridHeaderDiv.style.width;
}
}
}
}
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
var _148=RadGridNamespace.IsParentRightToLeft(this.GridHeaderDiv);
if((!_148&&this.GridHeaderDiv&&parseInt(this.GridHeaderDiv.style.marginRight)!=RadGridNamespace.GetScrollBarWidth())||(_148&&this.GridHeaderDiv&&parseInt(this.GridHeaderDiv.style.marginLeft)!=RadGridNamespace.GetScrollBarWidth())){
if(!_148){
this.GridHeaderDiv.style.marginRight=RadGridNamespace.GetScrollBarWidth()+"px";
this.GridHeaderDiv.style.marginLeft="";
}else{
this.GridHeaderDiv.style.marginLeft=RadGridNamespace.GetScrollBarWidth()+"px";
this.GridHeaderDiv.style.marginRight="";
}
}
if(this.GridHeaderDiv&&this.GridDataDiv){
if((this.GridDataDiv.clientWidth==this.GridDataDiv.offsetWidth)){
this.GridHeaderDiv.style.width="100%";
if(!_148){
this.GridHeaderDiv.style.marginRight="";
}else{
this.GridHeaderDiv.style.marginLeft="";
}
}
}
if(this.GroupPanelObject&&this.GroupPanelObject.Items.length>0&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
if(this.MasterTableView&&this.MasterTableViewHeader){
this.MasterTableView.Control.style.width=this.MasterTableViewHeader.Control.offsetWidth+"px";
}
}
if(this.GridFooterDiv){
this.GridFooterDiv.style.marginRight=this.GridHeaderDiv.style.marginRight;
this.GridFooterDiv.style.marginLeft=this.GridHeaderDiv.style.marginLeft;
this.GridFooterDiv.style.width=this.GridHeaderDiv.style.width;
}
}
};
RadGridNamespace.RadGrid.prototype.SetDataDivHeight=function(){
if(this.GridDataDiv&&this.Control.style.height!=""){
this.GridDataDiv.style.height="10px";
var _149=0;
if(this.GroupPanelObject){
_149+=this.GroupPanelObject.Control.offsetHeight;
}
if(this.GridHeaderDiv){
_149+=this.GridHeaderDiv.offsetHeight;
}
if(this.GridFooterDiv){
_149+=this.GridFooterDiv.offsetHeight;
}
if(this.PagerControl){
_149+=this.PagerControl.offsetHeight;
}
if(this.TopPagerControl){
_149+=this.TopPagerControl.offsetHeight;
}
var _14a=this.Control.clientHeight-_149;
if(_14a>0){
var _14b=this.Control.style.position;
if(window.netscape){
this.Control.style.position="absolute";
}
this.GridDataDiv.style.height=_14a+"px";
if(window.netscape){
this.Control.style.position=_14b;
}
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeDimensions=function(){
try{
var _14c=this;
this.InitializeAutoLayout();
if(!this.EnableAJAX){
this.OnWindowResize();
}else{
var _14d=function(){
_14c.OnWindowResize();
};
if(window.netscape&&!window.opera){
_14d();
}else{
setTimeout(_14d,0);
}
}
this.Control.RadResize=function(){
_14c.OnWindowResize();
};
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
setTimeout(function(){
_14c.AttachDomEvent(window,"resize","OnWindowResize");
},0);
}else{
this.AttachDomEvent(window,"resize","OnWindowResize");
}
this.Control.RadShow=function(){
_14c.OnWindowResize();
};
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnWindowResize=function(e){
this.SetHeaderAndFooterDivsWidth();
this.SetDataDivHeight();
};
RadGridNamespace.RadGrid.prototype.InitializeAutoLayout=function(){
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.MasterTableView&&this.MasterTableViewHeader){
if(this.MasterTableView.TableLayout!="Auto"||window.netscape||window.opera){
return;
}
this.MasterTableView.Control.style.tableLayout=this.MasterTableViewHeader.Control.style.tableLayout="";
var _14f=this.MasterTableView.Control.tBodies[0].rows[this.ClientSettings.FirstDataRowClientRowIndex];
for(var i=0;i<this.MasterTableViewHeader.HeaderRow.cells.length;i++){
var col=this.MasterTableViewHeader.ColGroup.Cols[i];
if(!col){
continue;
}
if(col.width!=""){
continue;
}
var _152=this.MasterTableViewHeader.HeaderRow.cells[i].offsetWidth;
var _153=_14f.cells[i].offsetWidth;
var _154=(_152>_153)?_152:_153;
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i]){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].offsetWidth>_154){
_154=this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].offsetWidth;
}
}
}
if(_154<=0){
continue;
}
this.MasterTableViewHeader.HeaderRow.cells[i].style.width=_14f.cells[i].style.width=this.MasterTableView.ColGroup.Cols[i].width=col.width=_154;
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i]){
this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].style.width=_154;
}
}
}
this.MasterTableView.Control.style.tableLayout=this.MasterTableViewHeader.Control.style.tableLayout="fixed";
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
this.MasterTableViewFooter.Control.style.tableLayout="fixed";
}
if(window.netscape){
this.OnWindowResize();
}
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeSaveScrollPosition=function(){
if(!this.ClientSettings.Scrolling.SaveScrollPosition||this.ClientSettings.Scrolling.EnableAJAXScrollPaging){
return;
}
if(this.ClientSettings.Scrolling.ScrollTop!=""){
this.GridDataDiv.scrollTop=this.ClientSettings.Scrolling.ScrollTop;
}
if(this.ClientSettings.Scrolling.ScrollLeft!=""){
if(this.GridHeaderDiv){
this.GridHeaderDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
if(this.GridFooterDiv){
this.GridFooterDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
this.GridDataDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
};
RadGridNamespace.RadGrid.prototype.InitializeAjaxScrollPaging=function(){
if(!this.ClientSettings.Scrolling.EnableAJAXScrollPaging){
return;
}
this.ScrollCounter=0;
this.CurrentAJAXScrollTop=0;
if(this.ClientSettings.Scrolling.AJAXScrollTop!=""){
this.CurrentAJAXScrollTop=this.ClientSettings.Scrolling.AJAXScrollTop;
}
var _155=this.CurrentPageIndex*this.MasterTableView.PageSize*20;
var _156=this.MasterTableView.PageCount*this.MasterTableView.PageSize*20;
var _157=this.MasterTableView.Control;
var _158=_157.offsetHeight;
if(!window.opera){
_157.style.marginTop=_155+"px";
_157.style.marginBottom=_156-_155-_158+"px";
}else{
_157.style.position="relative";
_157.style.top=_155+"px";
_157.style.marginBottom=_156-_158+"px";
}
this.CurrentAJAXScrollTop=_155;
this.GridDataDiv.scrollTop=_155;
this.CreateScrollerToolTip();
this.AttachDomEvent(this.GridDataDiv,"scroll","OnAJAXScroll");
};
RadGridNamespace.RadGrid.prototype.CreateScrollerToolTip=function(){
var _159=document.getElementById(this.ClientID+"ScrollerToolTip");
if(!_159){
this.ScrollerToolTip=document.createElement("span");
this.ScrollerToolTip.id=this.ClientID+"ScrollerToolTip";
this.ScrollerToolTip.style.backgroundColor="#F5F5DC";
this.ScrollerToolTip.style.border="1px solid";
this.ScrollerToolTip.style.position="absolute";
this.ScrollerToolTip.style.display="none";
this.ScrollerToolTip.style.font="icon";
this.ScrollerToolTip.style.padding="2";
document.body.appendChild(this.ScrollerToolTip);
}
};
RadGridNamespace.RadGrid.prototype.HideScrollerToolTip=function(){
var _15a=this;
setTimeout(function(){
var _15b=document.getElementById(_15a.ClientID+"ScrollerToolTip");
if(_15b&&_15b.parentNode){
_15b.style.display="none";
}
},200);
};
RadGridNamespace.RadGrid.prototype.ShowScrollerTooltip=function(_15c,_15d){
var _15e=document.getElementById(this.ClientID+"ScrollerToolTip");
if(_15e){
_15e.style.display="";
_15e.style.top=parseInt(RadGridNamespace.FindPosY(this.GridDataDiv))+Math.round(this.GridDataDiv.offsetHeight*_15c)+"px";
_15e.style.left=parseInt(RadGridNamespace.FindPosX(this.GridDataDiv))+this.GridDataDiv.offsetWidth-(this.GridDataDiv.offsetWidth-this.GridDataDiv.clientWidth)-_15e.offsetWidth+"px";
_15e.innerHTML="Page: <b>"+((_15d==0)?1:_15d+1)+"</b> out of <b>"+this.MasterTableView.PageCount+"</b> pages";
}
};
RadGridNamespace.RadGrid.prototype.InitializeScroll=function(){
var _15f=this;
var grid=this;
var _161=function(){
grid.InitializeSaveScrollPosition();
};
if(window.netscape&&!window.opera){
window.setTimeout(_161,0);
}else{
_161();
}
this.InitializeAjaxScrollPaging();
this.AttachDomEvent(this.GridDataDiv,"scroll","OnGridScroll");
};
RadGridNamespace.RadGrid.prototype.OnGridScroll=function(e){
if(this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.GridHeaderDiv){
this.GridHeaderDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
if(this.GridFooterDiv){
this.GridFooterDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
}
this.SavePostData("ScrolledControl",this.ClientID,this.GridDataDiv.scrollTop,this.GridDataDiv.scrollLeft);
var evt={};
evt.ScrollTop=this.GridDataDiv.scrollTop;
evt.ScrollLeft=this.GridDataDiv.scrollLeft;
evt.ScrollControl=this.GridDataDiv;
evt.IsOnTop=(this.GridDataDiv.scrollTop==0)?true:false;
evt.IsOnBottom=((this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16)==this.GridDataDiv.scrollTop)?true:false;
RadGridNamespace.FireEvent(this,"OnScroll",[evt]);
};
RadGridNamespace.RadGrid.prototype.OnAJAXScroll=function(e){
if(this.GridDataDiv){
this.CurrentScrollTop=this.GridDataDiv.scrollTop;
}
this.ScrollCounter++;
var _165=this;
RadGridNamespace.AJAXScrollHanlder=function(_166){
if(_165.ScrollCounter!=_166){
return;
}
if(_165.CurrentAJAXScrollTop!=_165.GridDataDiv.scrollTop){
if(_165.CurrentPageIndex==_167){
return;
}
var _168=_165.ClientID;
var _169=_165.MasterTableView.ClientID;
_165.SavePostData("AJAXScrolledControl",_165.GridDataDiv.scrollLeft,_165.LastScrollTop,_165.GridDataDiv.scrollTop,_167);
var _16a=_165.ClientSettings.PostBackFunction;
_16a=_16a.replace("{0}",_165.UniqueID);
eval(_16a);
}
_165.ScrollCounter=0;
_165.HideScrollerToolTip();
};
var evt={};
evt.ScrollTop=this.GridDataDiv.scrollTop;
evt.ScrollLeft=this.GridDataDiv.scrollLeft;
evt.ScrollControl=this.GridDataDiv;
evt.IsOnTop=(this.GridDataDiv.scrollTop==0)?true:false;
evt.IsOnBottom=((this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16)==this.GridDataDiv.scrollTop)?true:false;
RadGridNamespace.FireEvent(this,"OnScroll",[evt]);
var _16c=this.GridDataDiv.scrollTop/(this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16);
var _167=Math.round((this.MasterTableView.PageCount-1)*_16c);
setTimeout("RadGridNamespace.AJAXScrollHanlder("+this.ScrollCounter+")",500);
this.ShowScrollerTooltip(_16c,_167);
};
RadGridNamespace.RadGridTable=function(_16d){
if((!_16d)||typeof (_16d)!="object"){
return;
}
for(var _16e in _16d){
this[_16e]=_16d[_16e];
}
this.Type="RadGridTable";
this.ServerID=this.ID;
this.SelectedRows=new Array();
this.SelectedCells=new Array();
this.SelectedColumns=new Array();
this.ExpandCollapseColumns=new Array();
this.GroupSplitterColumns=new Array();
this.HeaderRow=null;
};
RadGridNamespace.RadGridTable.prototype._constructor=function(_16f){
if((!_16f)||typeof (_16f)!="object"){
return;
}
this.Control=document.getElementById(this.ClientID);
if(!this.Control){
return;
}
this.ColGroup=RadGridNamespace.GetTableColGroup(this.Control);
if(!this.ColGroup){
return;
}
this.ColGroup.Cols=RadGridNamespace.GetTableColGroupCols(this.ColGroup);
this.Owner=_16f;
this.InitializeEvents(this.Owner.ClientSettings.ClientEvents);
this.Control.style.overflow=((this.Owner.ClientSettings.Resizing.ClipCellContentOnResize&&((this.Owner.ClientSettings.Resizing.AllowColumnResize)||(this.Owner.ClientSettings.Resizing.AllowRowResize)))||(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders))?"hidden":"";
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&this.Control.style.tableLayout=="fixed"&&this.Control.style.width.indexOf("%")!=-1){
this.Control.style.width="";
}
this.CreateStyles();
if(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Detail")!=-1){
this.Columns=this.GetTableColumns(this.Control,this.RenderColumns);
}else{
this.Columns=this.Owner.MasterTableViewHeader.Columns;
this.ExpandCollapseColumns=this.Owner.MasterTableViewHeader.ExpandCollapseColumns;
this.GroupSplitterColumns=this.Owner.MasterTableViewHeader.GroupSplitterColumns;
}
}else{
this.Columns=this.GetTableColumns(this.Control,this.RenderColumns);
}
if(this.Owner.ClientSettings.ShouldCreateRows){
this.InitializeRows(this.Controls[0].Rows);
}
};
RadGridNamespace.RadGridTable.prototype.Dispose=function(){
if(this.ColGroup&&this.ColGroup.Cols){
this.ColGroup.Cols=null;
this.ColGroup=null;
}
this.Owner=null;
this.DisposeEvents();
this.ExpandCollapseColumns=null;
this.GroupSplitterColumns=null;
this.DisposeRows();
this.DisposeColumns();
this.RenderColumns=null;
this.SelectedRows=null;
this.ExpandCollapseColumns=null;
this.DetailTables=null;
this.DetailTablesCollection=null;
this.Control=null;
this.HeaderRow=null;
};
RadGridNamespace.RadGridTable.prototype.CreateStyles=function(){
if(!this.SelectedItemStyleClass||this.SelectedItemStyleClass==""){
if(this.SelectedItemStyle&&this.SelectedItemStyle!=""){
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".SelectedItemStyle"+this.ClientID+"1 td",this.SelectedItemStyle);
}else{
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".SelectedItemStyle"+this.ClientID+"2 td","background-color:Navy;color:White;");
}
}
var _170=((this.Owner.ClientSettings.Resizing.ClipCellContentOnResize&&((this.Owner.ClientSettings.Resizing.AllowColumnResize)||(this.Owner.ClientSettings.Resizing.AllowRowResize)))||(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders))?"hidden":"";
_170="hidden";
if(_170=="hidden"){
RadGridNamespace.addClassName(this.Control,"grid"+this.ClientID);
if(window.netscape){
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" td","overflow: hidden;-moz-user-select:-moz-none;");
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" th","overflow: hidden;-moz-user-select:-moz-none;");
}else{
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" td","overflow: hidden; text-overflow: ellipsis;");
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" th","overflow: hidden; text-overflow: ellipsis;");
}
}
};
RadGridNamespace.RadGridTable.prototype.InitializeEvents=function(_171){
for(clientEvent in _171){
if(typeof (_171[clientEvent])!="string"){
continue;
}
if(!this.Owner.IsClientEventName(clientEvent)){
if(_171[clientEvent]!=""){
var _172=_171[clientEvent];
if(_172.indexOf("(")!=-1){
this[clientEvent]=_172;
}else{
this[clientEvent]=eval(_172);
}
}else{
this[clientEvent]=null;
}
}
}
};
RadGridNamespace.RadGridTable.prototype.DisposeEvents=function(){
for(var _173 in RadGridNamespace.RadGridTable.ClientEventNames){
this[_173]=null;
}
};
RadGridNamespace.RadGridTable.prototype.InitializeRows=function(rows){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Footer")!=-1){
return;
}
try{
var _175=[];
for(var i=0;i<rows.length;i++){
if(!rows[i].Visible||rows[i].ClientRowIndex<0){
continue;
}
if(rows[i].ItemType=="THead"||rows[i].ItemType=="TFoot"){
continue;
}
RadGridNamespace.FireEvent(this,"OnRowCreating");
rows[i]._constructor(this);
_175[_175.length]=rows[i];
RadGridNamespace.FireEvent(this,"OnRowCreated",[rows[i]]);
}
this.Rows=_175;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.DisposeRows=function(){
if(this.Rows!=null){
for(var i=0;i<this.Rows.length;i++){
var row=this.Rows[i];
row.Dispose();
}
this.Rows=null;
}
};
RadGridNamespace.RadGridTable.prototype.DisposeColumns=function(){
if(this.Columns!=null){
for(var i=0;i<this.Columns.length;i++){
var _17a=this.Columns[i];
_17a.Dispose();
}
this.Columns=null;
}
};
RadGridNamespace.RadGridTable.prototype.GetTableRows=function(_17b,_17c){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Footer")!=-1){
return;
}
try{
var _17d=new Array();
var j=0;
for(var i=0;i<_17c.length;i++){
if((_17c[i].ItemType=="THead")||(_17c[i].ItemType=="TFoot")){
continue;
}
if((_17c[i])&&(_17c[i].Visible)){
RadGridNamespace.FireEvent(this,"OnRowCreating");
_17d[_17d.length]=_17c[i]._constructor(this);
RadGridNamespace.FireEvent(this,"OnRowCreated",[_17d[j]]);
j++;
}
}
return _17d;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.GetTableHeaderRow=function(){
try{
if(this.Control.tHead){
for(var i=0;i<this.Control.tHead.rows.length;i++){
if(this.Control.tHead.rows[i]!=null){
if(this.Control.tHead.rows[i].cells[0]!=null){
if(this.Control.tHead.rows[i].cells[0].tagName!=null){
if(this.Control.tHead.rows[i].cells[0].tagName.toLowerCase()=="th"){
this.HeaderRow=this.Control.tHead.rows[i];
break;
}
}
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.GetTableColumns=function(_181,_182){
try{
this.GetTableHeaderRow();
var _183=new Array();
if(!this.HeaderRow){
return;
}
if(!this.HeaderRow.cells[0]){
return;
}
var j=0;
for(var i=0;i<_182.length;i++){
if(_182[i].Visible){
RadGridNamespace.FireEvent(this,"OnColumnCreating");
_183[_183.length]=new RadGridNamespace.RadGridTableColumn(_182[i]);
_183[j]._constructor(this.HeaderRow.cells[j],this);
_183[j].RealIndex=i;
if(_182[i].ColumnType=="GridExpandColumn"){
this.ExpandCollapseColumns[this.ExpandCollapseColumns.length]=_183[j];
}
if(_182[i].ColumnType=="GridGroupSplitterColumn"){
this.GroupSplitterColumns[this.GroupSplitterColumns.length]=_183[j];
}
RadGridNamespace.FireEvent(this,"OnColumnCreated",[_183[j]]);
j++;
}
}
return _183;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.RemoveTableLayOut=function(){
this.masterTableLayOut=this.Owner.MasterTableView.Control.style.tableLayout;
this.detailTablesTableLayOut=new Array();
for(var i=0;i<this.Owner.DetailTablesCollection.length;i++){
this.detailTablesTableLayOut[this.detailTablesTableLayOut.length]=this.Owner.DetailTablesCollection[i].Control.style.tableLayout;
this.Owner.DetailTablesCollection[i].Control.style.tableLayout="";
}
};
RadGridNamespace.RadGridTable.prototype.RestoreTableLayOut=function(){
this.Owner.MasterTableView.Control.style.tableLayout=this.masterTableLayOut;
for(var i=0;i<this.Owner.DetailTablesCollection.length;i++){
this.Owner.DetailTablesCollection[i].Control.style.tableLayout=this.detailTablesTableLayOut[i];
}
};
RadGridNamespace.RadGridTable.prototype.SelectRow=function(row,_189){
try{
if(!this.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
var _18a=this.Owner.GetRowObjectByRealRow(this,row);
if(_18a!=null){
if(_18a.ItemType=="Item"||_18a.ItemType=="AlternatingItem"){
_18a.SetSelected(_189);
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.DeselectRow=function(row){
try{
if(!this.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
var _18c=this.Owner.GetRowObjectByRealRow(this,row);
if(_18c!=null){
if(_18c.ItemType=="Item"||_18c.ItemType=="AlternatingItem"){
this.RemoveFromSelectedRows(_18c);
_18c.RemoveSelectedRowStyle();
_18c.Selected=false;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ResizeRow=function(_18d,_18e,_18f){
try{
if(!this.Owner.ClientSettings.Resizing.AllowRowResize){
return;
}
if(!RadGridNamespace.FireEvent(this,"OnRowResizing",[_18d,_18e])){
return;
}
this.RemoveTableLayOut();
var _190=this.Control.style.tableLayout;
this.Control.style.tableLayout="";
var _191=this.Control.parentNode.parentNode.parentNode.parentNode;
var _192=this.Owner.GetTableObjectByID(_191.id);
var _193;
if(_192!=null){
_193=_192.Control.style.tableLayout;
_192.Control.style.tableLayout="";
}
if(!_18f){
if(this.Control){
if(this.Control.rows[_18d]){
if(this.Control.rows[_18d].cells[0]){
this.Control.rows[_18d].cells[0].style.height=_18e+"px";
this.Control.rows[_18d].style.height=_18e+"px";
}
}
}
}else{
if(this.Control){
if(this.Control.tBodies[0]){
if(this.Control.tBodies[0].rows[_18d]){
if(this.Control.tBodies[0].rows[_18d].cells[0]){
this.Control.tBodies[0].rows[_18d].cells[0].style.height=_18e+"px";
this.Control.tBodies[0].rows[_18d].style.height=_18e+"px";
}
}
}
}
}
this.Control.style.tableLayout=_190;
if(_192!=null){
_192.Control.style.tableLayout=_193;
}
this.RestoreTableLayOut();
var _194=this.Owner.GetRowObjectByRealRow(this,this.Control.rows[_18d]);
this.Owner.SavePostData("ResizedRows",this.Control.id,_194.RealIndex,_18e+"px");
RadGridNamespace.FireEvent(this,"OnRowResized",[_18d,_18e]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ResizeColumn=function(_195,_196){
if(isNaN(parseInt(_195))){
var _197="Column index must be of type \"Number\"!";
alert(_197);
return;
}
if(isNaN(parseInt(_196))){
var _197="Column width must be of type \"Number\"!";
alert(_197);
return;
}
if(_195<0){
var _197="Column index must be non-negative!";
alert(_197);
return;
}
if(_196<0){
var _197="Column width must be non-negative!";
alert(_197);
return;
}
if(_195>(this.Columns.length-1)){
var _197="Column index must be less than columns count!";
alert(_197);
return;
}
if(!this.Owner.ClientSettings.Resizing.AllowColumnResize){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_195].Resizable){
return;
}
if(!RadGridNamespace.FireEvent(this,"OnColumnResizing",[_195,_196])){
return;
}
try{
if(this==this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.ResizeColumn(_195,_196);
}
var _198=this.Control.clientWidth;
var _199=this.Owner.Control.clientWidth;
if(this.HeaderRow){
var _19a=this.HeaderRow.cells[_195].scrollWidth-_196;
}
if(window.netscape||window.opera){
if(this.HeaderRow){
if(this.HeaderRow.cells[_195]){
this.HeaderRow.cells[_195].style.width=_196+"px";
}
}
if(this==this.Owner.MasterTableViewHeader){
var _19b=this.Owner.MasterTableView.Control.tBodies[0].rows[this.Owner.ClientSettings.FirstDataRowClientRowIndex];
if(_19b){
if(_19b.cells[_195]){
_19b.cells[_195].style.width=_196+"px";
}
}
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.Control){
if(this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[_195]){
if(_196>0){
this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[_195].style.width=_196+"px";
}
}
}
}
}
if(this.ColGroup){
if(this.ColGroup.Cols[_195]){
if(_196>0){
this.ColGroup.Cols[_195].width=_196+"px";
}
}
}
if(this==this.Owner.MasterTableViewHeader){
if(this.Owner.MasterTableView.ColGroup){
if(this.Owner.MasterTableView.ColGroup.Cols[_195]){
if(_196>0){
this.Owner.MasterTableView.ColGroup.Cols[_195].width=_196+"px";
}
}
}
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.ColGroup){
if(this.Owner.MasterTableViewFooter.ColGroup.Cols[_195]){
if(_196>0){
this.Owner.MasterTableViewFooter.ColGroup.Cols[_195].width=_196+"px";
}
}
}
}
if(this==this.Owner.MasterTableView||this==this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ResizedColumns",this.Owner.MasterTableView.ClientID,this.Columns[_195].RealIndex,_196+"px");
}else{
this.Owner.SavePostData("ResizedColumns",this.ClientID,this.Columns[_195].RealIndex,_196+"px");
}
if(this.Owner.ClientSettings.Resizing.ResizeGridOnColumnResize){
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.ColGroup.Cols.length;i++){
if(i!=_195&&this.ColGroup.Cols[i].width==""){
this.ColGroup.Cols[i].width=this.HeaderRow.cells[i].scrollWidth+"px";
this.Owner.MasterTableView.ColGroup.Cols[i].width=this.ColGroup.Cols[i].width;
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.ColGroup){
this.Owner.MasterTableViewFooter.ColGroup.Cols[i].width=this.ColGroup.Cols[i].width;
}
}
}
this.Control.style.width=(this.Control.offsetWidth-_19a)+"px";
this.Owner.MasterTableView.Control.style.width=this.Control.style.width;
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.Control){
this.Owner.MasterTableViewFooter.Control.style.width=this.Control.style.width;
}
var _19d=(this.Control.scrollWidth>this.Control.offsetWidth)?this.Control.scrollWidth:this.Control.offsetWidth;
var _19e=this.Owner.GridDataDiv.offsetWidth;
this.Owner.SavePostData("ResizedControl",this.ClientID,_19d+"px",_19e+"px",this.Owner.Control.offsetHeight+"px");
}else{
this.Control.style.width=(this.Control.offsetWidth-_19a)+"px";
this.Owner.Control.style.width=this.Control.style.width;
var _19d=(this.Control.scrollWidth>this.Control.offsetWidth)?this.Control.scrollWidth:this.Control.offsetWidth;
this.Owner.SavePostData("ResizedControl",this.ClientID,_19d+"px",this.Owner.Control.offsetWidth+"px",this.Owner.Control.offsetHeight+"px");
}
}
if(this.Owner.GroupPanelObject&&this.Owner.GroupPanelObject.Items.length>0&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
if(this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableView.Control.style.width=this.Owner.MasterTableViewHeader.Control.offsetWidth+"px";
}
}
RadGridNamespace.FireEvent(this,"OnColumnResized",[_195,_196]);
if(window.netscape){
this.Control.style.cssText=this.Control.style.cssText;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ReorderColumns=function(_19f,_1a0){
if(isNaN(parseInt(_19f))){
var _1a1="First column index must be of type \"Number\"!";
alert(_1a1);
return;
}
if(isNaN(parseInt(_1a0))){
var _1a1="Second column index must be of type \"Number\"!";
alert(_1a1);
return;
}
if(_19f<0){
var _1a1="First column index must be non-negative!";
alert(_1a1);
return;
}
if(_1a0<0){
var _1a1="Second column index must be non-negative!";
alert(_1a1);
return;
}
if(_19f>(this.Columns.length-1)){
var _1a1="First column index must be less than columns count!";
alert(_1a1);
return;
}
if(_1a0>(this.Columns.length-1)){
var _1a1="Second column index must be less than columns count!";
alert(_1a1);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_19f].Reorderable){
return;
}
if(!this.Columns[_1a0].Reorderable){
return;
}
this.SwapColumns(_19f,_1a0);
if((!this.Owner.ClientSettings.ReorderColumnsOnClient)&&(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
if(this==this.Owner.MasterTableView){
eval(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
}
};
RadGridNamespace.RadGridTable.prototype.SwapColumns=function(_1a2,_1a3,_1a4){
if(isNaN(parseInt(_1a2))){
var _1a5="First column index must be of type \"Number\"!";
alert(_1a5);
return;
}
if(isNaN(parseInt(_1a3))){
var _1a5="Second column index must be of type \"Number\"!";
alert(_1a5);
return;
}
if(_1a2<0){
var _1a5="First column index must be non-negative!";
alert(_1a5);
return;
}
if(_1a3<0){
var _1a5="Second column index must be non-negative!";
alert(_1a5);
return;
}
if(_1a2>(this.Columns.length-1)){
var _1a5="First column index must be less than columns count!";
alert(_1a5);
return;
}
if(_1a3>(this.Columns.length-1)){
var _1a5="Second column index must be less than columns count!";
alert(_1a5);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_1a2].Reorderable){
return;
}
if(!this.Columns[_1a3].Reorderable){
return;
}
try{
if(this==this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.SwapColumns(_1a2,_1a3,!this.Owner.ClientSettings.ReorderColumnsOnClient);
return;
}
if(typeof (_1a4)=="undefined"){
_1a4=true;
}
if(this.Owner.ClientSettings.ColumnsReorderMethod=="Reorder"){
if(_1a3>_1a2){
while(_1a2+1<_1a3){
this.SwapColumns(_1a3-1,_1a3,false);
_1a3--;
}
}else{
while(_1a3<_1a2-1){
this.SwapColumns(_1a3+1,_1a3,false);
_1a3++;
}
}
}
if(!RadGridNamespace.FireEvent(this,"OnColumnSwapping",[_1a2,_1a3])){
return;
}
var _1a6=this.Control;
var _1a7=this.Columns[_1a2];
var _1a8=this.Columns[_1a3];
this.Columns[_1a2]=_1a8;
this.Columns[_1a3]=_1a7;
var _1a9=this.ColGroup.Cols[_1a2].width;
if(_1a9==""&&this.HeaderRow){
_1a9=this.HeaderRow.cells[_1a2].offsetWidth;
}
var _1aa=this.ColGroup.Cols[_1a3].width;
if(_1aa==""&&this.HeaderRow){
_1aa=this.HeaderRow.cells[_1a3].offsetWidth;
}
var _1ab=this.Owner.ClientSettings.Resizing.AllowColumnResize;
var _1ac=(typeof (this.Columns[_1a2].Resizable)=="boolean")?this.Columns[_1a2].Resizable:false;
var _1ad=(typeof (this.Columns[_1a3].Resizable)=="boolean")?this.Columns[_1a3].Resizable:false;
this.Owner.ClientSettings.Resizing.AllowColumnResize=true;
this.Columns[_1a2].Resizable=true;
this.Columns[_1a3].Resizable=true;
this.ResizeColumn(_1a2,_1aa);
this.ResizeColumn(_1a3,_1a9);
this.Owner.ClientSettings.Resizing.AllowColumnResize=_1ab;
this.Columns[_1a2].Resizable=_1ac;
this.Columns[_1a3].Resizable=_1ad;
var _1ae=(this==this.Owner.MasterTableViewHeader)?this.Owner.MasterTableView.ClientID:this.ClientID;
this.Owner.SavePostData("ReorderedColumns",_1ae,this.Columns[_1a2].UniqueName,this.Columns[_1a3].UniqueName);
for(var i=0;i<_1a6.rows.length;i++){
if(_1a6.rows[i]!=null){
if((_1a6.rows[i].cells[_1a2]!=null)&&(_1a6.rows[i].cells[_1a3]!=null)){
if(!_1a6.rows[i].cells[_1a3].swapNode){
if(_1a6.rows[i].cells[_1a2].innerHTML!=null){
var _1b0=_1a6.rows[i].cells[_1a2].innerHTML;
var _1b1=_1a6.rows[i].cells[_1a3].innerHTML;
_1a6.rows[i].cells[_1a2].innerHTML=_1b1;
_1a6.rows[i].cells[_1a3].innerHTML=_1b0;
}
}else{
_1a6.rows[i].cells[_1a3].swapNode(_1a6.rows[i].cells[_1a2]);
}
}
}
}
if(this.Owner.MasterTableViewHeader==this){
var _1a6=this.Owner.MasterTableView.Control;
for(var i=0;i<_1a6.rows.length;i++){
if(_1a6.rows[i]!=null){
if((_1a6.rows[i].cells[_1a2]!=null)&&(_1a6.rows[i].cells[_1a3]!=null)){
if(window.netscape||window.opera){
if(_1a6.rows[i].cells[_1a2].innerHTML!=null){
var _1b0=_1a6.rows[i].cells[_1a2].innerHTML;
var _1b1=_1a6.rows[i].cells[_1a3].innerHTML;
_1a6.rows[i].cells[_1a2].innerHTML=_1b1;
_1a6.rows[i].cells[_1a3].innerHTML=_1b0;
}
}else{
_1a6.rows[i].cells[_1a3].swapNode(_1a6.rows[i].cells[_1a2]);
}
}
}
}
}
if(_1a4&&(!this.Owner.ClientSettings.ReorderColumnsOnClient)&&(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
eval(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
RadGridNamespace.FireEvent(this,"OnColumnSwapped",[_1a2,_1a3]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.MoveColumnToLeft=function(_1b2){
if(isNaN(parseInt(_1b2))){
var _1b3="Column index must be of type \"Number\"!";
alert(_1b3);
return;
}
if(_1b2<0){
var _1b3="Column index must be non-negative!";
alert(_1b3);
return;
}
if(_1b2>(this.Columns.length-1)){
var _1b3="Column index must be less than columns count!";
alert(_1b3);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnMovingToLeft",[_1b2])){
return;
}
var _1b4=_1b2--;
this.SwapColumns(_1b2,_1b4);
RadGridNamespace.FireEvent(this,"OnColumnMovedToLeft",[_1b2]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.MoveColumnToRight=function(_1b5){
if(isNaN(parseInt(_1b5))){
var _1b6="Column index must be of type \"Number\"!";
alert(_1b6);
return;
}
if(_1b5<0){
var _1b6="Column index must be non-negative!";
alert(_1b6);
return;
}
if(_1b5>(this.Columns.length-1)){
var _1b6="Column index must be less than columns count!";
alert(_1b6);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnMovingToRight",[_1b5])){
return;
}
var _1b7=_1b5++;
this.SwapColumns(_1b5,_1b7);
RadGridNamespace.FireEvent(this,"OnColumnMovedToRight",[_1b5]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.HideColumn=function(_1b8){
if(!this.Owner.ClientSettings.AllowColumnHide){
return;
}
if(isNaN(parseInt(_1b8))){
var _1b9="Column index must be of type \"Number\"!";
alert(_1b9);
return;
}
if(_1b8<0){
var _1b9="Column index must be non-negative!";
alert(_1b9);
return;
}
if(_1b8>(this.Columns.length-1)){
var _1b9="Column index must be less than columns count!";
alert(_1b9);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnHiding",[_1b8])){
return;
}
for(var i=0;i<this.Control.rows.length;i++){
if(this.Control.rows[i].cells[_1b8]!=null){
if(this.Control.rows[i].cells[_1b8].colSpan==1){
this.Control.rows[i].cells[_1b8].style.display="none";
}
}
}
this.Columns[_1b8].Display=false;
if(this.Owner.FooterControl){
for(var i=0;i<this.Owner.FooterControl.rows.length;i++){
if(this.Owner.FooterControl.rows[i].cells[_1b8]!=null){
if(this.Owner.FooterControl.rows[i].cells[_1b8].colSpan==1){
this.Owner.FooterControl.rows[i].cells[_1b8].style.display="none";
}
}
}
}
if(this.Owner.HeaderControl){
for(var i=0;i<this.Owner.MasterTableViewHeader.Control.rows.length;i++){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1b8]!=null){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1b8].colSpan==1){
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1b8].style.display="none";
}
}
}
}
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.Owner.MasterTableView.Control.rows.length;i++){
if(this.Owner.MasterTableView.Control.rows[i].cells[_1b8]!=null){
if(this.Owner.MasterTableView.Control.rows[i].cells[_1b8].colSpan==1){
this.Owner.MasterTableView.Control.rows[i].cells[_1b8].style.display="none";
}
}
}
}
if(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders&&this==this.Owner.MasterTableView){
for(var i=0;i<this.Owner.MasterTableViewHeader.Control.rows.length;i++){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1b8]!=null){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1b8].colSpan==1){
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1b8].style.display="none";
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("HidedColumns",this.ClientID,this.Columns[_1b8].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnColumnHidden",[_1b8]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ShowColumn=function(_1bb){
if(!this.Owner.ClientSettings.AllowColumnHide){
return;
}
if(isNaN(parseInt(_1bb))){
var _1bc="Column index must be of type \"Number\"!";
alert(_1bc);
return;
}
if(_1bb<0){
var _1bc="Column index must be non-negative!";
alert(_1bc);
return;
}
if(_1bb>(this.Columns.length-1)){
var _1bc="Column index must be less than columns count!";
alert(_1bc);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnShowing",[_1bb])){
return;
}
if(this.Control.tHead){
for(var i=0;i<this.Control.tHead.rows.length;i++){
if(this.Control.tHead.rows[i].cells[_1bb]!=null){
if(window.netscape){
this.Control.tHead.rows[i].cells[_1bb].style.display="table-cell";
}else{
this.Control.tHead.rows[i].cells[_1bb].style.display="";
}
}
}
}
if(this.Control.tBodies[0]){
for(var i=0;i<this.Control.tBodies[0].rows.length;i++){
if(this.Control.tBodies[0].rows[i].cells[_1bb]!=null){
if(window.netscape){
this.Control.tBodies[0].rows[i].cells[_1bb].style.display="table-cell";
}else{
this.Control.tBodies[0].rows[i].cells[_1bb].style.display="";
}
}
}
}
if(this.Owner.FooterControl){
for(var i=0;i<this.Owner.FooterControl.rows.length;i++){
if(this.Owner.FooterControl.rows[i].cells[_1bb]!=null){
if(window.netscape){
this.Owner.FooterControl.rows[i].cells[_1bb].style.display="table-cell";
}else{
this.Owner.FooterControl.rows[i].cells[_1bb].style.display="";
}
}
}
}
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.Owner.MasterTableView.Control.rows.length;i++){
if(this.Owner.MasterTableView.Control.rows[i].cells[_1bb]!=null){
if(window.netscape){
this.Owner.MasterTableView.Control.rows[i].cells[_1bb].style.display="table-cell";
}else{
this.Owner.MasterTableView.Control.rows[i].cells[_1bb].style.display="";
}
}
}
}
if(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders&&this==this.Owner.MasterTableView){
for(var i=0;i<this.Owner.MasterTableViewHeader.Control.rows.length;i++){
if(this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb]!=null){
if(window.netscape){
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb].style.display="table-cell";
}else{
this.Owner.MasterTableViewHeader.Control.rows[i].cells[_1bb].style.display="";
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ShowedColumns",this.ClientID,this.Columns[_1bb].RealIndex);
}
this.Columns[_1bb].Display=true;
RadGridNamespace.FireEvent(this,"OnColumnShowed",[_1bb]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.HideRow=function(_1be){
if(!this.Owner.ClientSettings.AllowRowHide){
return;
}
if(isNaN(parseInt(_1be))){
var _1bf="Row index must be of type \"Number\"!";
alert(_1bf);
return;
}
if(_1be<0){
var _1bf="Row index must be non-negative!";
alert(_1bf);
return;
}
if(_1be>(this.Rows.length-1)){
var _1bf="Row index must be less than rows count!";
alert(_1bf);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnRowHiding",[_1be])){
return;
}
if(this.Rows){
if(this.Rows[_1be]){
if(this.Rows[_1be].Control){
this.Rows[_1be].Control.style.display="none";
this.Rows[_1be].Display=false;
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("HidedRows",this.ClientID,this.Rows[_1be].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnRowHidden",[_1be]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ShowRow=function(_1c0){
if(!this.Owner.ClientSettings.AllowRowHide){
return;
}
if(isNaN(parseInt(_1c0))){
var _1c1="Row index must be of type \"Number\"!";
alert(_1c1);
return;
}
if(_1c0<0){
var _1c1="Row index must be non-negative!";
alert(_1c1);
return;
}
if(_1c0>this.Rows.length){
var _1c1="Row index must be less than rows count!";
alert(_1c1);
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnRowShowing",[_1c0])){
return;
}
if(this.Rows){
if(this.Rows[_1c0]){
if(this.Rows[_1c0].Control){
if(this.Rows[_1c0].ItemType!="NestedView"){
if(window.netscape){
this.Rows[_1c0].Control.style.display="table-row";
}else{
this.Rows[_1c0].Control.style.display="";
}
this.Rows[_1c0].Display=true;
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ShowedRows",this.ClientID,this.Rows[_1c0].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnRowShowed",[_1c0]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ExportToExcel=function(_1c2){
try{
if(this.Owner.ClientSettings.PostBackReferences.PostBackExportToExcel!=""){
this.Owner.SavePostData("ExportToExcel",this.ClientID,_1c2);
eval(this.Owner.ClientSettings.PostBackReferences.PostBackExportToExcel);
}
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ExportToWord=function(_1c3){
try{
if(this.Owner.ClientSettings.PostBackReferences.PostBackExportToWord!=""){
this.Owner.SavePostData("ExportToWord",this.ClientID,_1c3);
eval(this.Owner.ClientSettings.PostBackReferences.PostBackExportToWord);
}
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.AddToSelectedRows=function(_1c4){
try{
this.SelectedRows[this.SelectedRows.length]=_1c4;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.IsInSelectedRows=function(_1c5){
try{
for(var i=0;i<this.SelectedRows.length;i++){
if(this.SelectedRows[i]!=_1c5){
return true;
}
}
return false;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ClearSelectedRows=function(){
var _1c7=this.SelectedRows;
for(var i=0;i<this.SelectedRows.length;i++){
if(!RadGridNamespace.FireEvent(this,"OnRowDeselecting",[this.SelectedRows[i]])){
continue;
}
this.SelectedRows[i].Selected=false;
this.SelectedRows[i].CheckClientSelectColumns();
this.SelectedRows[i].RemoveSelectedRowStyle();
var last=this.SelectedRows[i];
try{
this.SelectedRows.splice(i,1);
i--;
}
catch(ex){
}
RadGridNamespace.FireEvent(this,"OnRowDeselected",[last]);
}
this.SelectedRows=new Array();
};
RadGridNamespace.RadGridTable.prototype.RemoveFromSelectedRows=function(_1ca){
try{
var _1cb=new Array();
for(var i=0;i<this.SelectedRows.length;i++){
var last=this.SelectedRows[i];
if(this.SelectedRows[i]!=_1ca){
_1cb[_1cb.length]=this.SelectedRows[i];
}else{
if(!this.Owner.AllowMultiRowSelection){
if(!RadGridNamespace.FireEvent(this,"OnRowDeselecting",[this.SelectedRows[i]])){
continue;
}
}
try{
this.SelectedRows.splice(i,1);
i--;
}
catch(ex){
}
_1ca.CheckClientSelectColumns();
setTimeout(function(){
RadGridNamespace.FireEvent(_1ca.Owner,"OnRowDeselected",[_1ca]);
},100);
}
}
this.SelectedRows=_1cb;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.GetSelectedRowsIndexes=function(){
try{
var _1ce=new Array();
for(var i=0;i<this.SelectedRows.length;i++){
_1ce[_1ce.length]=this.SelectedRows[i].RealIndex;
}
return _1ce.join(",");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.GetCellByColumnUniqueName=function(_1d0,_1d1){
if(this.ClientID.indexOf("_Header")!=-1){
return;
}
if((!_1d0)||(!_1d1)){
return;
}
if(!this.Columns){
return;
}
for(var i=0;i<this.Columns.length;i++){
if(this.Columns[i].UniqueName.toUpperCase()==_1d1.toUpperCase()){
return _1d0.Control.cells[i];
}
}
return null;
};
RadGridNamespace.RadGridTableColumn=function(_1d3){
if((!_1d3)||typeof (_1d3)!="object"){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _1d4 in _1d3){
this[_1d4]=_1d3[_1d4];
}
this.Type="RadGridTableColumn";
this.ResizeTolerance=5;
this.CanResize=false;
};
RadGridNamespace.RadGridTableColumn.prototype._constructor=function(_1d5,_1d6){
this.Control=_1d5;
this.Owner=_1d6;
this.Index=_1d5.cellIndex;
if(window.opera&&typeof (_1d5.cellIndex)=="undefined"){
this.Index=0;
}
this.AttachDomEvent(this.Control,"click","OnClick");
this.AttachDomEvent(this.Control,"dblclick","OnDblClick");
this.AttachDomEvent(this.Control,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
this.AttachDomEvent(this.Control,"mouseup","OnMouseUp");
this.AttachDomEvent(this.Control,"mouseover","OnMouseOver");
this.AttachDomEvent(this.Control,"mouseout","OnMouseOut");
this.AttachDomEvent(this.Control,"contextmenu","OnContextMenu");
};
RadGridNamespace.RadGridTableColumn.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
if(this.ColumnResizer){
this.ColumnResizer.Dispose();
}
this.Control=null;
this.Owner=null;
this.Index=null;
};
RadGridNamespace.RadGridTableColumn.prototype.OnContextMenu=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnContextMenu",[this.Index,e])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnClick",[this.Index])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnDblClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnDblClick",[this.Index])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseMove=function(e){
if(this.Owner.Owner.ClientSettings.Resizing.AllowColumnResize&&this.Resizable&&this.Control.tagName.toLowerCase()=="th"){
var _1db=RadGridNamespace.GetEventPosX(e);
var _1dc=RadGridNamespace.FindPosX(this.Control);
var endX=_1dc+this.Control.offsetWidth;
var _1de=RadGridNamespace.GetCurrentElement(e);
if((_1db>=endX-this.ResizeTolerance)&&(_1db<=endX+this.ResizeTolerance)){
this.Control.style.cursor="e-resize";
this.Control.title=this.Owner.Owner.ClientSettings.ClientMessages.DragToResize;
this.CanResize=true;
_1de.style.cursor="e-resize";
this.Owner.Owner.IsResize=true;
}else{
this.Control.style.cursor="";
this.Control.title="";
this.CanResize=false;
_1de.style.cursor="";
this.Owner.Owner.IsResize=false;
}
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseDown=function(e){
if(this.CanResize){
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
var _1e0=RadGridNamespace.GetEventPosX(e);
var _1e1=RadGridNamespace.FindPosX(this.Control);
var endX=_1e1+this.Control.offsetWidth;
if((_1e0>=endX-this.ResizeTolerance)&&(_1e0<=endX+this.ResizeTolerance)){
this.ColumnResizer=new RadGridNamespace.RadGridColumnResizer(this,this.Owner.Owner.ClientSettings.Resizing.EnableRealTimeResize);
this.ColumnResizer.Position(e);
}
}
RadGridNamespace.ClearDocumentEvents();
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseUp=function(e){
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseOver=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnMouseOver",[this.Index])){
return;
}
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.addClassName(this.Control,"GridHeaderOver_"+this.Owner.Owner.Skin);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseOut=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnMouseOut",[this.Index])){
return;
}
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.removeClassName(this.Control,"GridHeaderOver_"+this.Owner.Owner.Skin);
}
};
RadGridNamespace.RadGridColumnResizer=function(_1e6,_1e7){
if(!_1e6){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
this.Column=_1e6;
this.IsRealTimeResize=_1e7;
this.CurrentWidth=null;
this.LeftResizer=document.createElement("span");
this.LeftResizer.style.backgroundColor="navy";
this.LeftResizer.style.width="1"+"px";
this.LeftResizer.style.position="absolute";
this.LeftResizer.style.cursor="e-resize";
this.RightResizer=document.createElement("span");
this.RightResizer.style.backgroundColor="navy";
this.RightResizer.style.width="1"+"px";
this.RightResizer.style.position="absolute";
this.RightResizer.style.cursor="e-resize";
this.ResizerToolTip=document.createElement("span");
this.ResizerToolTip.style.backgroundColor="#F5F5DC";
this.ResizerToolTip.style.border="1px solid";
this.ResizerToolTip.style.position="absolute";
this.ResizerToolTip.style.font="icon";
this.ResizerToolTip.style.padding="2";
this.ResizerToolTip.innerHTML="Width: <b>"+this.Column.Control.offsetWidth+"</b> <em>pixels</em>";
document.body.appendChild(this.LeftResizer);
document.body.appendChild(this.RightResizer);
document.body.appendChild(this.ResizerToolTip);
this.CanDestroy=true;
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(this.Column.Owner.Owner.Control,"mousemove","OnMouseMove");
};
RadGridNamespace.RadGridColumnResizer.prototype.OnMouseUp=function(e){
this.Destroy(e);
};
RadGridNamespace.RadGridColumnResizer.prototype.OnMouseMove=function(e){
this.Move(e);
};
RadGridNamespace.RadGridColumnResizer.prototype.Position=function(e){
this.LeftResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)-RadGridNamespace.FindScrollPosY(this.Column.Control)+document.documentElement.scrollTop+document.body.scrollTop+"px";
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)-RadGridNamespace.FindScrollPosX(this.Column.Control)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
this.RightResizer.style.top=this.LeftResizer.style.top;
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left)+this.Column.Control.offsetWidth+"px";
this.ResizerToolTip.style.top=parseInt(this.RightResizer.style.top)-20+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
if(parseInt(this.LeftResizer.style.left)<RadGridNamespace.FindPosX(this.Column.Owner.Control)){
this.LeftResizer.style.display="none";
}
this.LeftResizer.style.height=this.Column.Control.offsetHeight+"px";
this.RightResizer.style.height=this.Column.Control.offsetHeight+"px";
};
RadGridNamespace.RadGridColumnResizer.prototype.Destroy=function(e){
if(this.CanDestroy){
this.DetachDomEvent(document,"mouseup","OnMouseUp");
this.DetachDomEvent(this.Column.Owner.Owner.Control,"mousemove","OnMouseMove");
if(this.CurrentWidth!=null){
if(this.CurrentWidth>0){
this.Column.Owner.ResizeColumn(RadGridNamespace.GetRealCellIndex(this.Column.Owner,this.Column.Control),this.CurrentWidth);
this.CurrentWidth=null;
}
}
document.body.removeChild(this.LeftResizer);
document.body.removeChild(this.RightResizer);
document.body.removeChild(this.ResizerToolTip);
this.CanDestroy=false;
}
};
RadGridNamespace.RadGridColumnResizer.prototype.Dispose=function(){
try{
this.Destroy();
}
catch(error){
}
this.DisposeDomEventHandlers();
this.MouseUpHandler=null;
this.MouseMoveHandler=null;
this.LeftResizer=null;
this.RightResizer=null;
this.ResizerToolTip=null;
};
RadGridNamespace.RadGridColumnResizer.prototype.Move=function(e){
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)-RadGridNamespace.FindScrollPosX(this.Column.Control)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left)+(RadGridNamespace.GetEventPosX(e)-RadGridNamespace.FindPosX(this.Column.Control))+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
var _1ed=parseInt(this.RightResizer.style.left)-parseInt(this.LeftResizer.style.left);
var _1ee=this.Column.Control.scrollWidth-_1ed;
this.ResizerToolTip.innerHTML="Width: <b>"+_1ed+"</b> <em>pixels</em>";
if(!RadGridNamespace.FireEvent(this.Column.Owner,"OnColumnResizing",[this.Column.Index,_1ed])){
return;
}
if(_1ed<=0){
this.RightResizer.style.left=this.RightResizer.style.left;
this.Destroy(e);
return;
}
this.CurrentWidth=_1ed;
if(this.IsRealTimeResize){
this.Column.Owner.ResizeColumn(RadGridNamespace.GetRealCellIndex(this.Column.Owner,this.Column.Control),_1ed);
}else{
this.CurrentWidth=_1ed;
return;
}
if(RadGridNamespace.FindPosX(this.LeftResizer)!=RadGridNamespace.FindPosX(this.Column.Control)){
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)+"px";
}
if(RadGridNamespace.FindPosX(this.RightResizer)!=(RadGridNamespace.FindPosX(this.Column.Control)+this.Column.Control.offsetWidth)){
this.RightResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)+this.Column.Control.offsetWidth+"px";
}
if(RadGridNamespace.FindPosY(this.LeftResizer)!=RadGridNamespace.FindPosY(this.Column.Control)){
this.LeftResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)+"px";
this.RightResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)+"px";
}
if(this.LeftResizer.offsetHeight!=this.Column.Control.offsetHeight){
this.LeftResizer.style.height=this.Column.Control.offsetHeight+"px";
this.RightResizer.style.height=this.Column.Control.offsetHeight+"px";
}
if(this.Column.Owner.Owner.GridDataDiv){
this.LeftResizer.style.left=parseInt(this.LeftResizer.style.left.replace("px",""))-this.Column.Owner.Owner.GridDataDiv.scrollLeft+"px";
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left.replace("px",""))+this.Column.Control.offsetWidth+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
}
};
RadGridNamespace.RadGridTableRow=function(_1ef){
if((!_1ef)||typeof (_1ef)!="object"){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _1f0 in _1ef){
this[_1f0]=_1ef[_1f0];
}
this.Type="RadGridTableRow";
var _1f1=document.getElementById(this.OwnerID);
this.Control=_1f1.tBodies[0].rows[this.ClientRowIndex];
if(!this.Control){
return;
}
this.Index=this.Control.sectionRowIndex;
this.RealIndex=this.RowIndex;
};
RadGridNamespace.RadGridTableRow.prototype._constructor=function(_1f2){
this.Owner=_1f2;
this.CreateStyles();
if(this.Selected){
this.LoadSelected();
}
if(this.Owner.HierarchyLoadMode=="Client"){
if(this.Owner.Owner.ClientSettings.AllowExpandCollapse){
for(var i=0;i<this.Owner.ExpandCollapseColumns.length;i++){
var _1f4=this.Owner.ExpandCollapseColumns[i].Control.cellIndex;
var _1f5=this.Control.cells[_1f4];
var html=this.Control.innerHTML;
if(!_1f5){
continue;
}
var _1f7;
for(var j=0;j<_1f5.childNodes.length;j++){
if(!_1f5.childNodes[j].tagName){
continue;
}
var _1f9;
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="ImageButton"){
_1f9="img";
}else{
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="LinkButton"){
_1f9="a";
}else{
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="PushButton"){
_1f9="button";
}
}
}
if(_1f5.childNodes[j].tagName.toLowerCase()==_1f9){
_1f7=_1f5.childNodes[j];
break;
}
}
if(_1f7){
var _1fa=this;
var _1fb=function(){
_1fa.OnHierarchyExpandButtonClick(this);
};
_1f7.onclick=_1fb;
_1f7.ondblclick=null;
_1fb=null;
}
_1f7=null;
}
}
}
if(this.Owner.GroupLoadMode=="Client"){
if(this.Owner.Owner.ClientSettings.AllowGroupExpandCollapse){
for(var i=0;i<this.Owner.GroupSplitterColumns.length;i++){
var _1f4=this.Owner.GroupSplitterColumns[i].Control.cellIndex;
var html=this.Control.innerHTML;
var _1f5=this.Control.cells[_1f4];
if(!_1f5){
continue;
}
var _1f7;
for(var j=0;j<_1f5.childNodes.length;j++){
if(!_1f5.childNodes[j].tagName){
continue;
}
if(_1f5.childNodes[j].tagName.toLowerCase()=="img"){
_1f7=_1f5.childNodes[j];
break;
}
}
if(_1f7){
var _1fa=this;
var _1fb=function(){
_1fa.OnGroupExpandButtonClick(this);
};
_1f7.onclick=_1fb;
_1f7.ondblclick=null;
_1fb=null;
}
_1f7=null;
}
}
}
this.AttachDomEvent(this.Control,"click","OnClick");
this.AttachDomEvent(this.Control,"dblclick","OnDblClick");
this.AttachDomEvent(document,"mousedown","OnMouseDown");
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(document,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"mouseover","OnMouseOver");
this.AttachDomEvent(this.Control,"mouseout","OnMouseOut");
this.AttachDomEvent(this.Control,"contextmenu","OnContextMenu");
if(this.Owner.Owner.ClientSettings.ActiveRowData&&this.Owner.Owner.ClientSettings.ActiveRowData!=""){
var data=this.Owner.Owner.ClientSettings.ActiveRowData.split(";")[0].split(",");
if(data[0]==this.Owner.ClientID&&data[1]==this.RealIndex){
this.Owner.Owner.ActiveRow=this;
}
}
};
RadGridNamespace.GroupRowExpander=function(_1fd){
this.startRow=_1fd;
};
RadGridNamespace.GroupRowExpander.prototype.NotFinished=function(_1fe){
var _1ff=(this.currentGridRow!=null);
if(!_1ff){
return false;
}
var _200=(this.currentGridRow.GroupIndex=="");
var _201=(this.currentGridRow.GroupIndex==_1fe.GroupIndex);
var _202=(this.currentGridRow.GroupIndex.indexOf(_1fe.GroupIndex+"_")==0);
return (_200||_201||_202);
};
RadGridNamespace.GroupRowExpander.prototype.ToggleExpandCollapse=function(_203){
var _204=this.startRow;
var _205=_204.Owner;
var _206=_203.parentNode.parentNode.sectionRowIndex;
var _207=_205.Rows[_206];
if(_207.Expanded){
if(!RadGridNamespace.FireEvent(_207.Owner,"OnGroupCollapsing",[_207])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(_207.Owner,"OnGroupExpanding",[_207])){
return;
}
}
var _208=_205.Control.rows[_206+1];
if(!_208){
return;
}
this.currentRowIndex=_208.rowIndex;
this.lastGroupIndex=null;
while(true){
this.currentGridRow=_205.Rows[this.currentRowIndex];
var _209=this.NotFinished(_207);
if(!_209){
break;
}
var _20a=(this.lastGroupIndex!=null)&&(this.currentGridRow.GroupIndex.indexOf(this.lastGroupIndex)!=-1);
var _20b=(this.currentGridRow.ItemType!="GroupHeader")&&(!this.currentGridRow.IsVisible());
var _20c=_20a&&_20b;
if(this.currentGridRow.ItemType=="GroupHeader"&&!this.currentGridRow.Expanded){
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
_203.src=_205.GroupSplitterColumns[0].ExpandImageUrl;
if(_205.Rows[this.currentRowIndex+1]==null||_205.Rows[this.currentRowIndex+1].ItemType=="GroupHeader"){
this.currentGridRow.Expanded=false;
}
}else{
_203.src=_205.GroupSplitterColumns[0].CollapseImageUrl;
this.currentGridRow.Show();
if(_205.Rows[this.currentRowIndex+1]==null||_205.Rows[this.currentRowIndex+1].ItemType=="GroupHeader"){
this.currentGridRow.Expanded=true;
}
}
this.lastGroupIndex=this.currentGridRow.GroupIndex;
}else{
if(!_20c){
if(this.currentGridRow.ItemType=="NestedView"){
if(this.currentGridRow.Expanded){
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
}else{
this.currentGridRow.Show();
}
}
}else{
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
_203.src=_205.GroupSplitterColumns[0].ExpandImageUrl;
_207.Expanded=false;
}else{
_203.src=_205.GroupSplitterColumns[0].CollapseImageUrl;
this.currentGridRow.Show();
_207.Expanded=true;
}
}
}
}
this.currentRowIndex++;
}
if(_207.Expanded!=null){
if(_207.Expanded){
_205.Owner.SavePostData("ExpandedGroupRows",_205.ClientID,_207.RealIndex);
_204.title=_205.Owner.GroupingSettings.CollapseTooltip;
}else{
_205.Owner.SavePostData("CollapsedGroupRows",_205.ClientID,_207.RealIndex);
_204.title=_205.Owner.GroupingSettings.ExpandTooltip;
}
}
if(_207.Expanded){
if(!RadGridNamespace.FireEvent(_207.Owner,"OnGroupExpanded",[_207])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(_207.Owner,"OnGroupCollapsed",[_207])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnGroupExpandButtonClick=function(_20d){
var _20e=new RadGridNamespace.GroupRowExpander(this);
_20e.ToggleExpandCollapse(_20d);
};
RadGridNamespace.RadGridTableRow.prototype.OnHierarchyExpandButtonClick=function(_20f){
var _210=this.Owner.Control.rows[_20f.parentNode.parentNode.rowIndex+1];
var _211=this.Owner.Rows[_20f.parentNode.parentNode.sectionRowIndex];
if(!_210){
return;
}
if(this.TableRowIsVisible(_210)){
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyCollapsing",[this])){
return;
}
this.HideTableRow(_210);
_211.Expanded=false;
if(this.Owner.ExpandCollapseColumns[0].ButtonType=="ImageButton"){
_20f.src=this.Owner.ExpandCollapseColumns[0].ExpandImageUrl;
}else{
_20f.innerHTML="+";
}
_20f.title=this.Owner.Owner.HierarchySettings.ExpandTooltip;
this.Owner.Owner.SavePostData("CollapsedRows",this.Owner.ClientID,this.RealIndex);
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyCollapsed",[this])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyExpanding",[this])){
return;
}
if(this.Owner.ExpandCollapseColumns[0].ButtonType=="ImageButton"){
_20f.src=this.Owner.ExpandCollapseColumns[0].CollapseImageUrl;
}else{
_20f.innerHTML="-";
}
_20f.title=this.Owner.Owner.HierarchySettings.CollapseTooltip;
this.ShowTableRow(_210);
_211.Expanded=true;
this.Owner.Owner.SavePostData("ExpandedRows",this.Owner.ClientID,this.RealIndex);
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyExpanded",[this])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.TableRowIsVisible=function(_212){
return _212.style.display!="none";
};
RadGridNamespace.RadGridTableRow.prototype.IsVisible=function(){
return this.TableRowIsVisible(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.HideTableRow=function(_213){
if(this.TableRowIsVisible(_213)){
_213.style.display="none";
}
};
RadGridNamespace.RadGridTableRow.prototype.Hide=function(){
this.HideTableRow(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.ShowTableRow=function(_214){
if(window.netscape||window.opera){
_214.style.display="table-row";
}else{
_214.style.display="block";
}
};
RadGridNamespace.RadGridTableRow.prototype.Show=function(){
this.ShowTableRow(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridTableRow.prototype.CreateStyles=function(){
if(!this.Owner.Owner.ClientSettings.ApplyStylesOnClient){
return;
}
switch(this.ItemType){
case "GroupHeader":
break;
case "EditFormItem":
this.Control.className+=" "+this.Owner.RenderEditItemStyleClass;
this.Control.style.cssText+=" "+this.Owner.RenderEditItemStyle;
break;
default:
var _215=eval("this.Owner.Render"+this.ItemType+"StyleClass");
if(typeof (_215)!="undefined"){
this.Control.className+=" "+_215;
}
var _216=eval("this.Owner.Render"+this.ItemType+"Style");
if(typeof (_216)!="undefined"){
this.Control.style.cssText+=" "+_216;
}
break;
}
if(!this.Display){
if(this.Control.style.cssText!=""){
if(this.Control.style.cssText.lastIndexOf(";")==this.Control.style.cssText.length-1){
this.Control.style.cssText+="display:none;";
}else{
this.Control.style.cssText+=";display:none;";
}
}else{
this.Control.style.cssText+="display:none;";
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnContextMenu=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowContextMenu",[this.Index,e])){
return;
}
if(this.Owner.Owner.ClientSettings.ClientEvents.OnRowContextMenu!=""){
if(e.preventDefault){
e.preventDefault();
}else{
e.returnValue=false;
return false;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.OnClick=function(e){
try{
if(this.Owner.Owner.RowResizer){
return;
}
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowClick",[this.Control.sectionRowIndex,e])){
return;
}
if(e.shiftKey&&this.Owner.SelectedRows[0]){
if(this.Owner.SelectedRows[0].Control.rowIndex>this.Control.rowIndex){
for(var i=this.Control.rowIndex;i<this.Owner.SelectedRows[0].Control.rowIndex+1;i++){
var _21a=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_21a){
if(!_21a.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
if(this.Owner.SelectedRows[0].Control.rowIndex<this.Control.rowIndex){
for(var i=this.Owner.SelectedRows[0].Control.rowIndex;i<this.Control.rowIndex+1;i++){
var _21a=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_21a){
if(!_21a.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
}
if(!e.shiftKey){
this.HandleRowSelection(e);
}
var _21b=RadGridNamespace.GetCurrentElement(e);
if(!_21b){
return;
}
if(!_21b.tagName){
return;
}
if(_21b.tagName.toLowerCase()=="input"&&_21b.tagName.toLowerCase()=="select"&&_21b.tagName.toLowerCase()=="option"&&_21b.tagName.toLowerCase()=="button"&&_21b.tagName.toLowerCase()=="a"&&_21b.tagName.toLowerCase()=="textarea"){
return;
}
if(this.ItemType=="Item"||this.ItemType=="AlternatingItem"){
if(this.Owner.Owner.ClientSettings.EnablePostBackOnRowClick){
var _21c=this.Owner.Owner.ClientSettings.PostBackFunction;
_21c=_21c.replace("{0}",this.Owner.Owner.UniqueID).replace("{1}","RowClick;"+this.ItemIndexHierarchical);
var form=document.getElementById(this.Owner.Owner.FormID);
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value==""){
eval(_21c);
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.HandleActiveRow=function(e){
var _21f=RadGridNamespace.GetCurrentElement(e);
if(_21f!=null&&_21f.tagName&&(_21f.tagName.toLowerCase()=="input"||_21f.tagName.toLowerCase()=="textarea")){
return;
}
if(this.Owner.Owner.ActiveRow!=null){
if(!RadGridNamespace.FireEvent(this.Owner,"OnActiveRowChanging",[this.Owner.Owner.ActiveRow])){
return;
}
if(e.keyCode==13){
this.Owner.Owner.SavePostData("EditRow",this.Owner.ClientID,this.Owner.Owner.ActiveRow.RealIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackEditRow);
}
if(e.keyCode==40){
var _220=this.Owner.Rows[this.Owner.Owner.ActiveRow.Control.sectionRowIndex+1];
if(_220!=null){
this.Owner.Owner.SetActiveRow(_220);
this.ScrollIntoView(_220);
}
}
if(e.keyCode==39){
return;
var _220=this.Owner.Owner.GetNextHierarchicalRow(_221,this.Owner.Owner.ActiveRow.Control.sectionRowIndex);
if(_220!=null){
_221=_220.parentNode.parentNode;
this.Owner.Owner.SetActiveRow(_221,_220.sectionRowIndex);
this.ScrollIntoView(_220);
}
}
if(e.keyCode==38){
var _222=this.Owner.Rows[this.Owner.Owner.ActiveRow.Control.sectionRowIndex-1];
if(_222!=null){
this.Owner.Owner.SetActiveRow(_222);
this.ScrollIntoView(_222);
}
}
if(e.keyCode==37){
return;
var _222=this.Owner.Owner.GetPreviousHierarchicalRow(_221,this.Owner.Owner.ActiveRow.Control.sectionRowIndex);
if(_222!=null){
var _221=_222.parentNode.parentNode;
this.Owner.Owner.SetActiveRow(_221,_222.sectionRowIndex);
this.ScrollIntoView(_222);
}
}
if(e.keyCode==32){
if(this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
this.Owner.Owner.ActiveRow.Owner.SelectRow(this.Owner.Owner.ActiveRow.Control,!this.Owner.Owner.AllowMultiRowSelection);
}
}
}
if(window.netscape){
e.preventDefault();
return false;
}else{
e.returnValue=false;
}
RadGridNamespace.FireEvent(this.Owner,"OnActiveRowChanged",[this.Owner.Owner.ActiveRow]);
};
RadGridNamespace.RadGridTableRow.prototype.ScrollIntoView=function(row){
if(row.Control&&row.Control.focus){
row.Control.scrollIntoView(false);
try{
row.Control.focus();
}
catch(e){
}
}
};
RadGridNamespace.RadGridTableRow.prototype.HandleExpandCollapse=function(){
};
RadGridNamespace.RadGridTableRow.prototype.HandleGroupExpandCollapse=function(){
};
RadGridNamespace.RadGridTableRow.prototype.HandleRowSelection=function(e){
var _225=RadGridNamespace.GetCurrentElement(e);
if(_225.onclick){
return;
}
if(_225.tagName.toLowerCase()=="a"&&_225.tagName.toLowerCase()=="img"||_225.tagName.toLowerCase()=="input"){
return;
}
this.SetSelected(!e.ctrlKey);
};
RadGridNamespace.RadGridTableRow.prototype.CheckClientSelectColumns=function(){
if(!this.Owner.Columns){
return;
}
for(var i=0;i<this.Owner.Columns.length;i++){
if(this.Owner.Columns[i].ColumnType=="GridClientSelectColumn"){
var cell=this.Owner.GetCellByColumnUniqueName(this,this.Owner.Columns[i].UniqueName);
if(cell!=null){
var _228=cell.getElementsByTagName("input")[0];
if(_228!=null){
_228.checked=this.Selected;
}
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.SetSelected=function(_229){
if(!this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowSelecting",[this])){
return;
}
}
if((this.ItemType=="Item")||(this.ItemType=="AlternatingItem")){
if(_229){
this.SingleSelect();
}else{
this.MultiSelect();
}
}
this.CheckClientSelectColumns();
if(this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowSelected",[this])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.SingleSelect=function(){
if(!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
this.Owner.ClearSelectedRows();
this.Owner.Owner.ClearSelectedRows();
this.Selected=true;
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
var _22a=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22a);
};
RadGridNamespace.RadGridTableRow.prototype.SingleDeselect=function(){
if(!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
this.Owner.ClearSelectedRows();
this.Owner.Owner.ClearSelectedRows();
this.Selected=false;
this.RemoveSelectedRowStyle();
this.Owner.RemoveFromSelectedRows(this);
var _22b=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22b);
};
RadGridNamespace.RadGridTableRow.prototype.MultiSelect=function(){
if((!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect)||(!this.Owner.Owner.AllowMultiRowSelection)){
return;
}
if(this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowDeselecting",[this])){
return;
}
this.Selected=false;
this.RemoveSelectedRowStyle();
this.Owner.RemoveFromSelectedRows(this);
var _22c=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22c);
}else{
this.Selected=true;
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
var _22c=this.Owner.GetSelectedRowsIndexes();
this.Owner.Owner.SavePostData("SelectedRows",this.Owner.ClientID,_22c);
}
};
RadGridNamespace.RadGridTableRow.prototype.LoadSelected=function(){
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
};
RadGridNamespace.RadGridTableRow.prototype.ApplySelectedRowStyle=function(){
if(!this.Owner.SelectedItemStyleClass||this.Owner.SelectedItemStyleClass==""){
if(this.Owner.SelectedItemStyle&&this.Owner.SelectedItemStyle!=""){
RadGridNamespace.addClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"1");
}else{
RadGridNamespace.addClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"2");
}
}else{
RadGridNamespace.addClassName(this.Control,this.Owner.SelectedItemStyleClass);
}
};
RadGridNamespace.RadGridTableRow.prototype.RemoveSelectedRowStyle=function(){
if(this.Owner.SelectedItemStyle){
RadGridNamespace.removeClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"1");
}else{
RadGridNamespace.removeClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"2");
}
RadGridNamespace.removeClassName(this.Control,this.Owner.SelectedItemStyleClass);
if(this.Control.style.cssText==this.Owner.SelectedItemStyle){
this.Control.style.cssText="";
}
};
RadGridNamespace.RadGridTableRow.prototype.OnDblClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowDblClick",[this.Control.sectionRowIndex,e])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.CreateRowSelectorArea=function(e){
if((this.Owner.Owner.RowResizer)||(e.ctrlKey)){
return;
}
var _22f=null;
if(e.srcElement){
_22f=e.srcElement;
}else{
if(e.target){
_22f=e.target;
}
}
if(!_22f.tagName){
return;
}
if(_22f.tagName.toLowerCase()=="input"){
return;
}
if((!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect)||(!this.Owner.Owner.AllowMultiRowSelection)){
return;
}
var _230=RadGridNamespace.GetCurrentElement(e);
if((!_230)||(!RadGridNamespace.IsChildOf(_230,this.Control))){
return;
}
if(!this.RowSelectorArea){
this.RowSelectorArea=document.createElement("span");
this.RowSelectorArea.style.backgroundColor="navy";
this.RowSelectorArea.style.border="indigo 1px solid";
this.RowSelectorArea.style.position="absolute";
this.RowSelectorArea.style.font="icon";
if(window.netscape&&!window.opera){
this.RowSelectorArea.style.MozOpacity=1/10;
}else{
if(window.opera||navigator.userAgent.indexOf("Safari")>-1){
this.RowSelectorArea.style.opacity=0.1;
}else{
this.RowSelectorArea.style.filter="alpha(opacity=10);";
}
}
if(this.Owner.Owner.GridDataDiv){
this.RowSelectorArea.style.top=RadGridNamespace.FindPosY(this.Control)-this.Owner.Owner.GridDataDiv.scrollTop+"px";
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Control)-this.Owner.Owner.GridDataDiv.scrollLeft+"px";
if(parseInt(this.RowSelectorArea.style.left)<RadGridNamespace.FindPosX(this.Owner.Owner.Control)){
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Owner.Owner.Control)+"px";
}
}else{
this.RowSelectorArea.style.top=RadGridNamespace.FindPosY(this.Control)+"px";
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Control)+"px";
}
document.body.appendChild(this.RowSelectorArea);
this.FirstRow=this.Control;
RadGridNamespace.ClearDocumentEvents();
}
};
RadGridNamespace.RadGridTableRow.prototype.DestroyRowSelectorArea=function(e){
if(this.RowSelectorArea){
var _232=this.RowSelectorArea.style.height;
document.body.removeChild(this.RowSelectorArea);
this.RowSelectorArea=null;
RadGridNamespace.RestoreDocumentEvents();
var _233=RadGridNamespace.GetCurrentElement(e);
var _234;
if((!_233)||(!RadGridNamespace.IsChildOf(_233,this.Owner.Control))){
return;
}
if((_233.tagName.toLowerCase()=="td")||(_233.tagName.toLowerCase()=="tr")){
if(_233.tagName.toLowerCase()=="td"){
_234=_233.parentNode;
}else{
if(_233.tagName.toLowerCase()=="tr"){
_234=_233;
}
}
for(var i=this.FirstRow.rowIndex;i<_234.rowIndex+1;i++){
var _236=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_236){
if(_232!=""){
if(!_236.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.ResizeRowSelectorArea=function(e){
if((this.RowSelectorArea)&&(this.RowSelectorArea.parentNode)){
var _238=RadGridNamespace.GetCurrentElement(e);
if((!_238)||(!RadGridNamespace.IsChildOf(_238,this.Owner.Control))){
return;
}
var _239=parseInt(this.RowSelectorArea.style.left);
if(this.Owner.Owner.GridDataDiv){
var _23a=RadGridNamespace.GetEventPosX(e)-this.Owner.Owner.GridDataDiv.scrollLeft;
}else{
var _23a=RadGridNamespace.GetEventPosX(e);
}
var _23b=parseInt(this.RowSelectorArea.style.top);
if(this.Owner.Owner.GridDataDiv){
var _23c=RadGridNamespace.GetEventPosY(e)-this.Owner.Owner.GridDataDiv.scrollTop;
}else{
var _23c=RadGridNamespace.GetEventPosY(e);
}
if((_23a-_239-5)>0){
this.RowSelectorArea.style.width=_23a-_239-5+"px";
}
if((_23c-_23b-5)>0){
this.RowSelectorArea.style.height=_23c-_23b-5+"px";
}
if(this.RowSelectorArea.offsetWidth>this.Owner.Control.offsetWidth){
this.RowSelectorArea.style.width=this.Owner.Control.offsetWidth+"px";
}
var _23d=(RadGridNamespace.FindPosX(this.Owner.Control)+this.Owner.Control.offsetHeight)-parseInt(this.RowSelectorArea.style.top);
if(this.RowSelectorArea.offsetHeight>_23d){
if(_23d>0){
this.RowSelectorArea.style.height=_23d+"px";
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseDown=function(e){
if(this.Owner.Owner.ClientSettings.Selecting.EnableDragToSelectRows&&this.Owner.Owner.AllowMultiRowSelection){
if(!this.Owner.Owner.RowResizer){
this.CreateRowSelectorArea(e);
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseUp=function(e){
this.DestroyRowSelectorArea(e);
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseMove=function(e){
this.ResizeRowSelectorArea(e);
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseOver=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowMouseOver",[this.Control.sectionRowIndex,e])){
return;
}
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.addClassName(this.Control,"GridRowOver_"+this.Owner.Owner.Skin);
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseOut=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowMouseOut",[this.Control.sectionRowIndex,e])){
return;
}
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.removeClassName(this.Control,"GridRowOver_"+this.Owner.Owner.Skin);
}
};
RadGridNamespace.RadGridGroupPanel=function(_243,_244){
this.Control=_243;
this.Owner=_244;
this.Items=new Array();
this.groupPanelItemCounter=0;
this.getGroupPanelItems(this.Control,0);
var _245=this;
};
RadGridNamespace.RadGridGroupPanel.prototype.Dispose=function(){
this.UnLoadHandler=null;
this.Control=null;
this.Owner=null;
this.DisposeItems();
for(var _246 in this){
this[_246]=null;
}
};
RadGridNamespace.RadGridGroupPanel.prototype.DisposeItems=function(){
if(this.Items!=null){
for(var i=0;i<this.Items.length;i++){
var item=this.Items[i];
item.Dispose();
}
}
};
RadGridNamespace.RadGridGroupPanel.prototype.groupPanelItemCounter=0;
RadGridNamespace.RadGridGroupPanel.prototype.getGroupPanelItems=function(_249){
for(var i=0;i<_249.rows.length;i++){
var _24b=false;
var row=_249.rows[i];
for(var j=0;j<row.cells.length;j++){
var cell=row.cells[j];
if(cell.tagName.toLowerCase()=="th"){
var _24f;
if(this.Owner.GroupPanel.GroupPanelItems[this.groupPanelItemCounter]){
_24f=this.Owner.GroupPanel.GroupPanelItems[this.groupPanelItemCounter].HierarchicalIndex;
}
if(_24f){
this.Items[this.Items.length]=new RadGridNamespace.RadGridGroupPanelItem(cell,this,_24f);
_24b=true;
this.groupPanelItemCounter++;
}
}
if((cell.firstChild)&&(cell.firstChild.tagName)){
if(cell.firstChild.tagName.toLowerCase()=="table"){
this.getGroupPanelItems(cell.firstChild);
}
}
}
}
};
RadGridNamespace.RadGridGroupPanel.prototype.IsItem=function(_250){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Control==_250){
return this.Items[i];
}
}
return null;
};
RadGridNamespace.RadGridGroupPanelItem=function(_252,_253,_254){
RadControlsNamespace.DomEventMixin.Initialize(this);
this.Control=_252;
this.Owner=_253;
this.HierarchicalIndex=_254;
this.Control.style.cursor="move";
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
};
RadGridNamespace.RadGridGroupPanelItem.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var _255 in this){
this[_255]=null;
}
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseDown=function(e){
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
this.CreateDragDrop(e);
this.CreateReorderIndicators(this.Control);
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(document,"mousemove","OnMouseMove");
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseUp=function(e){
this.FireDropAction(e);
this.DestroyDragDrop(e);
this.DestroyReorderIndicators();
this.DetachDomEvent(document,"mouseup","OnMouseUp");
this.DetachDomEvent(document,"mousemove","OnMouseMove");
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseMove=function(e){
this.MoveDragDrop(e);
};
RadGridNamespace.RadGridGroupPanelItem.prototype.FireDropAction=function(e){
var _25a=RadGridNamespace.GetCurrentElement(e);
if(_25a!=null){
if(!RadGridNamespace.IsChildOf(_25a,this.Owner.Control)){
this.Owner.Owner.SavePostData("UnGroupByExpression",this.HierarchicalIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackUnGroupByExpression);
}else{
var item=this.Owner.IsItem(_25a);
if((_25a!=this.Control)&&(item!=null)&&(_25a.parentNode==this.Control.parentNode)){
this.Owner.Owner.SavePostData("ReorderGroupByExpression",this.HierarchicalIndex,item.HierarchicalIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackReorderGroupByExpression);
}
if(window.netscape){
this.Control.style.MozOpacity=4/4;
}else{
this.Control.style.filter="alpha(opacity=100);";
}
}
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.CreateDragDrop=function(e){
this.MoveHeaderDiv=document.createElement("div");
var _25d=document.createElement("table");
if(this.MoveHeaderDiv.mergeAttributes){
this.MoveHeaderDiv.mergeAttributes(this.Owner.Owner.Control);
}else{
RadGridNamespace.CopyAttributes(this.MoveHeaderDiv,this.Control);
}
if(_25d.mergeAttributes){
_25d.mergeAttributes(this.Owner.Control);
}else{
RadGridNamespace.CopyAttributes(_25d,this.Owner.Control);
}
_25d.style.margin="0px";
_25d.style.height=this.Control.offsetHeight+"px";
_25d.style.width=this.Control.offsetWidth+"px";
_25d.style.border="0px";
_25d.style.borderCollapse="collapse";
_25d.style.padding="0px";
var _25e=document.createElement("thead");
var tr=document.createElement("tr");
_25d.appendChild(_25e);
_25e.appendChild(tr);
tr.appendChild(this.Control.cloneNode(true));
this.MoveHeaderDiv.appendChild(_25d);
document.body.appendChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.style.height=_25d.style.height;
this.MoveHeaderDiv.style.width=_25d.style.width;
this.MoveHeaderDiv.style.position="absolute";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
if(window.netscape){
this.MoveHeaderDiv.style.MozOpacity=3/4;
}else{
this.MoveHeaderDiv.style.filter="alpha(opacity=75);";
}
this.MoveHeaderDiv.style.cursor="move";
this.MoveHeaderDiv.style.display="none";
this.MoveHeaderDiv.onmousedown=null;
RadGridNamespace.ClearDocumentEvents();
};
RadGridNamespace.RadGridGroupPanelItem.prototype.DestroyDragDrop=function(e){
if(this.MoveHeaderDiv!=null){
var _261=this.MoveHeaderDiv.parentNode;
_261.removeChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.onmouseup=null;
this.MoveHeaderDiv.onmousemove=null;
this.MoveHeaderDiv=null;
RadGridNamespace.RestoreDocumentEvents();
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.MoveDragDrop=function(e){
if(this.MoveHeaderDiv!=null){
if(window.netscape){
this.Control.style.MozOpacity=1/4;
}else{
this.Control.style.filter="alpha(opacity=25);";
}
this.MoveHeaderDiv.style.visibility="";
this.MoveHeaderDiv.style.display="";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
var _263=RadGridNamespace.GetCurrentElement(e);
if(_263!=null){
if(RadGridNamespace.IsChildOf(_263,this.Owner.Control)){
var item=this.Owner.IsItem(_263);
if((_263!=this.Control)&&(item!=null)&&(_263.parentNode==this.Control.parentNode)){
this.MoveReorderIndicators(e,_263);
}else{
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
}
}
}
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.CreateReorderIndicators=function(_265){
if((this.ReorderIndicator1==null)&&(this.ReorderIndicator2==null)){
this.ReorderIndicator1=document.createElement("span");
this.ReorderIndicator2=document.createElement("span");
this.ReorderIndicator1.innerHTML="&darr;";
this.ReorderIndicator2.innerHTML="&uarr;";
this.ReorderIndicator1.style.backgroundColor="transparent";
this.ReorderIndicator1.style.color="darkblue";
this.ReorderIndicator1.style.font="bold 18px Arial";
this.ReorderIndicator2.style.backgroundColor=this.ReorderIndicator1.style.backgroundColor;
this.ReorderIndicator2.style.color=this.ReorderIndicator1.style.color;
this.ReorderIndicator2.style.font=this.ReorderIndicator1.style.font;
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_265)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_265)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_265)+_265.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
document.body.appendChild(this.ReorderIndicator1);
document.body.appendChild(this.ReorderIndicator2);
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.DestroyReorderIndicators=function(){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
document.body.removeChild(this.ReorderIndicator1);
document.body.removeChild(this.ReorderIndicator2);
this.ReorderIndicator1=null;
this.ReorderIndicator2=null;
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.MoveReorderIndicators=function(e,_267){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
this.ReorderIndicator1.style.visibility="visible";
this.ReorderIndicator1.style.display="";
this.ReorderIndicator2.style.visibility="visible";
this.ReorderIndicator2.style.display="";
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_267)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_267)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_267)+_267.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
}
};
RadGridNamespace.RadGridMenu=function(_268,_269,_26a){
if(!_268||!_269){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _26b in _268){
this[_26b]=_268[_26b];
}
this.Owner=_269;
this.ItemData=_268.Items;
this.Items=[];
};
RadGridNamespace.RadGridMenu.prototype.Initialize=function(){
if(this.Control!=null){
return;
}
this.Control=document.createElement("table");
this.Control.style.backgroundColor=this.SelectColumnBackColor;
this.Control.style.border="outset 1px";
this.Control.style.fontSize="small";
this.Control.style.textAlign="left";
this.Control.cellPadding="0";
this.Control.style.borderCollapse="collapse";
this.Control.style.zIndex=998;
this.Items=this.CreateItems(this.ItemData);
this.Control.style.position="absolute";
this.Control.style.display="none";
document.body.appendChild(this.Control);
var _26c=document.createElement("img");
_26c.src=this.SelectedImageUrl;
_26c.src=this.NotSelectedImageUrl;
this.Control.className=this.CssClass;
};
RadGridNamespace.RadGridMenu.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
this.DisposeItems();
this.ItemData=null;
this.Owner=null;
this.Control=null;
};
RadGridNamespace.RadGridMenu.prototype.CreateItems=function(_26d){
var _26e=[];
for(var i=0;i<_26d.length;i++){
_26e[_26e.length]=new RadGridNamespace.RadGridMenuItem(_26d[i],this);
}
return _26e;
};
RadGridNamespace.RadGridMenu.prototype.DisposeItems=function(){
for(var i=0;i<this.Items.length;i++){
var item=this.Items[i];
item.Dispose();
}
this.Items=null;
};
RadGridNamespace.RadGridMenu.prototype.HideItem=function(_272){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_272){
this.Items[i].Control.style.display="none";
}
}
};
RadGridNamespace.RadGridMenu.prototype.ShowItem=function(_274){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_274){
this.Items[i].Control.style.display="";
}
}
};
RadGridNamespace.RadGridMenu.prototype.SelectItem=function(_276){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_276){
this.Items[i].Selected=true;
this.Items[i].SelectImage.src=this.SelectedImageUrl;
}else{
this.Items[i].Selected=false;
this.Items[i].SelectImage.src=this.NotSelectedImageUrl;
}
}
};
RadGridNamespace.RadGridMenu.prototype.Show=function(_278,_279,e){
this.Initialize();
this.Control.style.display="";
this.Control.style.top=e.clientY+document.documentElement.scrollTop+document.body.scrollTop+5+"px";
this.Control.style.left=e.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+5+"px";
this.AttachHideEvents();
};
RadGridNamespace.RadGridMenu.prototype.OnKeyPress=function(e){
if(e.keyCode==27){
this.DetachHideEvents();
this.Hide();
}
};
RadGridNamespace.RadGridMenu.prototype.OnClick=function(e){
if(!e.cancelBubble){
this.DetachHideEvents();
this.Hide();
}
};
RadGridNamespace.RadGridMenu.prototype.AttachHideEvents=function(){
this.AttachDomEvent(document,"keypress","OnKeyPress");
this.AttachDomEvent(document,"click","OnClick");
};
RadGridNamespace.RadGridMenu.prototype.DetachHideEvents=function(){
this.DetachDomEvent(document,"keypress","OnKeyPress");
this.DetachDomEvent(document,"click","OnClick");
};
RadGridNamespace.RadGridMenu.prototype.Hide=function(){
if(this.Control.style.display==""){
this.Control.style.display="none";
}
};
RadGridNamespace.RadGridMenuItem=function(_27d,_27e){
for(var _27f in _27d){
this[_27f]=_27d[_27f];
}
this.Owner=_27e;
this.Control=this.Owner.Control.insertRow(-1);
this.Control.insertCell(-1);
var _280=document.createElement("table");
_280.style.width="100%";
_280.cellPadding="0";
_280.cellSpacing="0";
_280.insertRow(-1);
var td1=_280.rows[0].insertCell(-1);
var td2=_280.rows[0].insertCell(-1);
td1.style.borderTop="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.borderLeft="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.borderRight="none 0px";
td1.style.borderBottom="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.padding="2px";
td1.style.textAlign="center";
td1.style.width="16px";
td1.appendChild(document.createElement("img"));
td1.childNodes[0].src=this.Owner.NotSelectedImageUrl;
this.SelectImage=td1.childNodes[0];
td2.style.borderTop="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.borderLeft="none 0px";
td2.style.borderRight="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.borderBottom="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.padding="2px";
td2.innerHTML=this.Text;
td2.style.backgroundColor=this.Owner.TextColumnBackColor;
td2.style.cursor="hand";
this.Control.cells[0].appendChild(_280);
var _283=this;
this.Control.onclick=function(){
if(_283.Owner.Owner.Owner.EnableAJAX){
if(_283.Owner.Owner==_283.Owner.Owner.Owner.MasterTableViewHeader){
RadGridNamespace.AsyncRequest(_283.UID,_283.Owner.Owner.Owner.MasterTableView.UID+"!"+_283.Owner.Column.UniqueName,_283.Owner.Owner.Owner.ClientID);
}else{
RadGridNamespace.AsyncRequest(_283.UID,_283.Owner.Owner.UID+"!"+_283.Owner.Column.UniqueName,_283.Owner.Owner.Owner.ClientID);
}
}else{
var _284=_283.Owner.Owner.Owner.ClientSettings.PostBackFunction;
if(_283.Owner.Owner==_283.Owner.Owner.Owner.MasterTableViewHeader){
_284=_284.replace("{0}",_283.UID).replace("{1}",_283.Owner.Owner.Owner.MasterTableView.UID+"!"+_283.Owner.Column.UniqueName);
}else{
_284=_284.replace("{0}",_283.UID).replace("{1}",_283.Owner.Owner.UID+"!"+_283.Owner.Column.UniqueName);
}
eval(_284);
}
};
this.Control.onmouseover=function(e){
this.cells[0].childNodes[0].rows[0].cells[0].style.backgroundColor=_283.Owner.HoverBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderTop="solid 1px "+_283.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderLeft="solid 1px "+_283.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderBottom="solid 1px "+_283.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.backgroundColor=_283.Owner.HoverBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderTop="solid 1px "+_283.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderRight="solid 1px "+_283.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderBottom="solid 1px "+_283.Owner.HoverBorderColor;
};
this.Control.onmouseout=function(e){
this.cells[0].childNodes[0].rows[0].cells[0].style.borderTop="solid 1px "+_283.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderLeft="solid 1px "+_283.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderBottom="solid 1px "+_283.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.backgroundColor="";
this.cells[0].childNodes[0].rows[0].cells[1].style.borderTop="solid 1px "+_283.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderRight="solid 1px "+_283.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderBottom="solid 1px "+_283.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.backgroundColor=_283.Owner.TextColumnBackColor;
};
};
RadGridNamespace.RadGridMenuItem.prototype.Dispose=function(){
this.Control.onclick=null;
this.Control.onmouseover=null;
this.Control.onmouseout=null;
var _287=this.Control.getElementsByTagName("table");
while(_287.length>0){
var _288=_287[0];
if(_288.parentNode!=null){
_288.parentNode.removeChild(_288);
}
}
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridFilterMenu=function(_289,_28a){
RadGridNamespace.RadGridMenu.call(this,_289,_28a);
};
RadGridNamespace.RadGridFilterMenu.prototype=new RadGridNamespace.RadGridMenu;
RadGridNamespace.RadGridFilterMenu.prototype.Show=function(_28b,e){
this.Initialize();
if(!_28b){
return;
}
this.Owner=_28b.Owner;
this.Column=_28b;
for(var i=0;i<this.Items.length;i++){
if(_28b.DataTypeName!="System.String"){
if((this.Items[i].Value=="StartsWith")||(this.Items[i].Value=="EndsWith")||(this.Items[i].Value=="Contains")||(this.Items[i].Value=="DoesNotContain")||(this.Items[i].Value=="IsEmpty")||(this.Items[i].Value=="NotIsEmpty")){
this.Items[i].Control.style.display="none";
continue;
}
}
if(_28b.FilterListOptions=="VaryByDataType"){
if(this.Items[i].Value=="Custom"){
this.Items[i].Control.style.display="none";
continue;
}
}
this.Items[i].Control.style.display="";
}
this.SelectItem(_28b.CurrentFilterFunction);
var args={Menu:this,TableView:this.Owner,Column:this.Column,Event:e};
if(!RadGridNamespace.FireEvent(this.Owner,"OnFilterMenuShowing",[this.Owner,args])){
return;
}
this.Control.style.display="";
this.Control.style.top=e.clientY+document.documentElement.scrollTop+document.body.scrollTop+5+"px";
this.Control.style.left=e.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+5+"px";
this.AttachHideEvents();
};
RadGridNamespace.RadGrid.prototype.InitializeFilterMenu=function(_28f){
if(this.AllowFilteringByColumn||_28f.AllowFilteringByColumn){
if(!_28f||!_28f.Control){
return;
}
if(!_28f.Control.tHead){
return;
}
if(!_28f.IsItemInserted){
var _290=_28f.Control.tHead.rows[_28f.Control.tHead.rows.length-1];
}else{
var _290=_28f.Control.tHead.rows[_28f.Control.tHead.rows.length-2];
}
if(!_290){
return;
}
var _291=_290.getElementsByTagName("img");
var _292=this;
if(!_28f.Columns){
return;
}
if(!_28f.Columns[0]){
return;
}
var _293=_28f.Columns[0].FilterImageUrl;
for(var i=0;i<_291.length;i++){
if(_291[i].getAttribute("src").indexOf(_293)==-1){
continue;
}
_291[i].onclick=function(e){
if(!e){
var e=window.event;
}
e.cancelBubble=true;
var _296=this.parentNode.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_296=RadGridNamespace.GetRealCellIndexFormCells(this.parentNode.parentNode.cells,this.parentNode);
}
_292.FilteringMenu.Show(_28f.Columns[_296],e);
if(e.preventDefault){
e.preventDefault();
}else{
e.returnValue=false;
return false;
}
};
}
this.FilteringMenu=new RadGridNamespace.RadGridFilterMenu(this.FilterMenu,_28f);
}
};
RadGridNamespace.RadGrid.prototype.DisposeFilterMenu=function(_297){
if(this.FilteringMenu!=null){
this.FilteringMenu.Dispose();
this.FilteringMenu=null;
}
};
RadGridNamespace.GetRealCellIndexFormCells=function(_298,cell){
for(var i=0;i<_298.length;i++){
if(_298[i]==cell){
return i;
}
}
};
if(typeof (window.RadGridNamespace)=="undefined"){
window.RadGridNamespace=new Object();
}
RadGridNamespace.Slider=function(_29b){
RadControlsNamespace.DomEventMixin.Initialize(this);
if(!document.readyState||document.readyState=="complete"||window.opera){
this._constructor(_29b);
}else{
this.objectData=_29b;
this.AttachDomEvent(window,"load","OnWindowLoad");
}
};
RadGridNamespace.Slider.prototype.OnWindowLoad=function(e){
this.DetachDomEvent(window,"load","OnWindowLoad");
this._constructor(this.objectData);
this.objectData=null;
};
RadGridNamespace.Slider.prototype._constructor=function(_29d){
var _29e=this;
for(var _29f in _29d){
this[_29f]=_29d[_29f];
}
this.Owner=window[this.OwnerID];
this.OwnerGrid=window[this.OwnerGridID];
this.Control=document.getElementById(this.ClientID);
if(this.Control==null){
return;
}
this.Control.unselectable="on";
this.Control.parentNode.style.padding="10px";
this.ToolTip=document.createElement("div");
this.ToolTip.unselectable="on";
this.ToolTip.style.backgroundColor="#F5F5DC";
this.ToolTip.style.border="1px outset";
this.ToolTip.style.font="icon";
this.ToolTip.style.padding="2px";
this.ToolTip.style.marginTop="5px";
this.ToolTip.style.marginBottom="15px";
this.Control.appendChild(this.ToolTip);
this.Line=document.createElement("hr");
this.Line.unselectable="on";
this.Line.style.width="100%";
this.Line.style.height="2px";
this.Line.style.backgroundColor="buttonface";
this.Line.style.border="1px outset threedshadow";
this.Control.appendChild(this.Line);
this.Thumb=document.createElement("div");
this.Thumb.unselectable="on";
this.Thumb.style.position="relative";
this.Thumb.style.width="8px";
this.Thumb.style.marginTop="-15px";
this.Thumb.style.height="16px";
this.Thumb.style.backgroundColor="buttonface";
this.Thumb.style.border="1px outset threedshadow";
this.Control.appendChild(this.Thumb);
this.Link=document.createElement("a");
this.Link.unselectable="on";
this.Link.style.width="100%";
this.Link.style.height="100%";
this.Link.style.display="block";
this.Link.href="javascript:void(0);";
this.Thumb.appendChild(this.Link);
this.LineX=RadGridNamespace.FindPosX(this.Line);
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
this.AttachDomEvent(this.Link,"keydown","OnKeyDown");
var _2a0=this.OwnerGrid.CurrentPageIndex/this.OwnerGrid.MasterTableView.PageCount;
this.SetPosition(_2a0*this.Line.offsetWidth);
var _2a1=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2a2=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2a1);
this.ToolTip.innerHTML="Page: <b>"+(this.OwnerGrid.CurrentPageIndex+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
};
RadGridNamespace.Slider.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var _2a3 in this){
this[_2a3]=null;
}
this.Control=null;
this.Line=null;
this.Thumb=null;
this.ToolTip=null;
};
RadGridNamespace.Slider.prototype.OnKeyDown=function(e){
this.AttachDomEvent(this.Link,"keyup","OnKeyUp");
if(e.keyCode==39){
this.SetPosition(parseInt(this.Thumb.style.left)+this.Thumb.offsetWidth);
}
if(e.keyCode==37){
this.SetPosition(parseInt(this.Thumb.style.left)-this.Thumb.offsetWidth);
}
if(e.keyCode==39||e.keyCode==37){
var _2a5=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2a6=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2a5);
this.ToolTip.innerHTML="Page: <b>"+((_2a6==0)?1:_2a6+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
}
};
RadGridNamespace.Slider.prototype.OnKeyUp=function(e){
this.DetachDomEvent(this.Link,"keyup","OnKeyUp");
if(e.keyCode==39||e.keyCode==37){
var _2a8=this;
setTimeout(function(){
_2a8.ChangePage();
},100);
}
};
RadGridNamespace.Slider.prototype.OnMouseDown=function(e){
this.DetachDomEvent(this.Control,"mousedown","OnMouseDown");
if(((window.netscape||window.opera)&&(e.button==0))||(e.button==1)){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
this.AttachDomEvent(document,"mousemove","OnMouseMove");
this.AttachDomEvent(document,"mouseup","OnMouseUp");
}
};
RadGridNamespace.Slider.prototype.OnMouseUp=function(e){
this.DetachDomEvent(document,"mousemove","OnMouseMove");
this.DetachDomEvent(document,"mouseup","OnMouseUp");
var _2ab=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2ac=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2ab);
this.ToolTip.innerHTML="Page: <b>"+((_2ac==0)?1:_2ac+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
var _2ad=this;
setTimeout(function(){
_2ad.ChangePage();
},100);
};
RadGridNamespace.Slider.prototype.OnMouseMove=function(e){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
var _2af=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2b0=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2af);
this.ToolTip.innerHTML="Page: <b>"+((_2b0==0)?1:_2b0+1)+"</b> out of <b>"+this.OwnerGrid.MasterTableView.PageCount+"</b> pages";
};
RadGridNamespace.Slider.prototype.GetPosition=function(e){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
};
RadGridNamespace.Slider.prototype.SetPosition=function(_2b2){
if(_2b2>=0&&_2b2<=this.Line.offsetWidth){
this.Thumb.style.left=_2b2+"px";
}
};
RadGridNamespace.Slider.prototype.ChangePage=function(){
var _2b3=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _2b4=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_2b3);
if(this.OwnerGrid.CurrentPageIndex==_2b4){
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
return;
}
this.OwnerGrid.SavePostData("AJAXScrolledControl",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.GridDataDiv.scrollLeft:"",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.LastScrollTop:"",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.GridDataDiv.scrollTop:"",_2b4);
var _2b5=this.OwnerGrid.ClientSettings.PostBackFunction;
_2b5=_2b5.replace("{0}",this.OwnerGrid.UniqueID);
eval(_2b5);
};


//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
