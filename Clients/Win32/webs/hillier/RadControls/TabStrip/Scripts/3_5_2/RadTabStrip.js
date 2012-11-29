if(typeof (window.RadTabStripNamespace)=="undefined"){
window.RadTabStripNamespace=new Object();
}
RadTabStripNamespace.ItemGroup=function(_1,_2){
this.Size=0;
this.ExpandableSize=0;
this.FixedSize=0;
this.Items=[];
this.SizeMethod=_2;
this.SizeProperty=_1;
};
RadTabStripNamespace.ItemGroup.prototype.RegisterItem=function(_3,_4){
var _5=_3.className.indexOf("separator")>-1;
if(_5){
_4=true;
}else{
_3=_3.firstChild;
}
this.Size+=RadTabStripNamespace.Box[this.SizeMethod](_3);
if(_5||(_4&&_3.firstChild.firstChild.style[this.SizeProperty])){
this.FixedSize+=RadTabStripNamespace.Box[this.SizeMethod](_3);
return;
}
this.ExpandableSize+=RadTabStripNamespace.Box[this.SizeMethod](_3);
this.Items[this.Items.length]=_3;
};
RadTabStripNamespace.Align=function(_6,_7,_8){
this.Element=_6;
this.ItemGroups=[];
if(_7=="horizontal"){
this.OuterSizeMethod="GetOuterWidth";
this.InnerSizeMethod="GetInnerWidth";
this.SetSizeMethod="SetOuterWidth";
this.OffsetProperty="offsetTop";
this.SizeProperty="width";
}else{
this.OuterSizeMethod="GetOuterHeight";
this.InnerSizeMethod="GetInnerHeight";
this.SetSizeMethod="SetOuterHeight";
this.OffsetProperty="offsetLeft";
this.SizeProperty="height";
}
this.SkipFixedSize=_8;
if(!this.Element.ItemGroups){
this.BuildItemGroups();
this.Element.ItemGroups=this.ItemGroups;
}else{
this.ItemGroups=this.Element.ItemGroups;
}
};
RadTabStripNamespace.Align.prototype.CreateItemGroup=function(){
return new RadTabStripNamespace.ItemGroup(this.SizeProperty,this.OuterSizeMethod);
};
RadTabStripNamespace.Align.prototype.BuildItemGroups=function(){
var _9=3;
var _a=this.Element.childNodes;
var _b=0;
var _c=-1;
this.ItemGroups[0]=this.CreateItemGroup();
for(var i=0;i<_a.length;i++){
var _e=_a[i];
var _f=_e[this.OffsetProperty];
if(_e.nodeType==_9){
continue;
}
if(_c==-1){
_c=_f;
}
if(_f>_c+1){
_b++;
this.ItemGroups[_b]=this.CreateItemGroup();
_c=_f;
}
this.ItemGroups[_b].RegisterItem(_e);
}
this.CalculateItemSizePercentage();
};
RadTabStripNamespace.Align.prototype.CalculateItemSizePercentage=function(){
for(var j=0;j<this.ItemGroups.length;j++){
var _11=this.ItemGroups[j];
for(var i=0;i<_11.Items.length;i++){
var _13=_11.Items[i];
if(this.SkipFixedSize&&_13.style[this.SizeProperty]){
continue;
}
var _14=RadTabStripNamespace.Box[this.OuterSizeMethod](_13);
var _15=RadTabStripNamespace.Box[this.OuterSizeMethod](_13.firstChild.firstChild);
if(_11.ExpandableSize==0){
_13.Percentage=0;
}else{
_13.Percentage=_14/_11.ExpandableSize;
}
_13.PaddingDiff=_14-_15;
}
}
};
RadTabStripNamespace.Align.prototype.InterateOverRows=function(_16){
var _17=RadTabStripNamespace.Box[this.InnerSizeMethod](this.Element);
for(var j=0;j<this.ItemGroups.length;j++){
if(!this.ItemGroups[j].Items.length){
continue;
}
_16(this.ItemGroups[j],_17);
}
};
RadTabStripNamespace.Align.Justify=function(_19){
var _1a=new RadTabStripNamespace.Align(_19,"horizontal",true);
var _1b=function(row,_1d){
for(var i=0;i<row.Items.length;i++){
var _1f=row.Items[i];
var _20=_1f.Percentage*(_1d-row.FixedSize)-_1f.PaddingDiff;
var _21=_1f.firstChild.firstChild;
RadTabStripNamespace.Box.SetOuterWidth(_21,Math.floor(_20));
}
};
_1a.InterateOverRows(_1b);
};
RadTabStripNamespace.Align.Right=function(_22){
var _23=new RadTabStripNamespace.Align(_22,"horizontal");
var _24=function(row,_26){
var _27=row.Items[0];
_27.style.marginLeft=(_26-row.Size-1)+"px";
_27.style.cssText=_27.style.cssText;
};
_23.InterateOverRows(_24);
};
RadTabStripNamespace.Align.Center=function(_28){
var _29=new RadTabStripNamespace.Align(_28,"horizontal");
var _2a=function(row,_2c){
var _2d=row.Items[0];
var _2e=Math.floor((_2c-row.Size)/2)+"px";
_2d.style.marginLeft=_2e;
_2d.style.cssText=_2d.style.cssText;
};
_29.InterateOverRows(_2a);
};
RadTabStripNamespace.Align.VJustify=function(_2f){
var _30=new RadTabStripNamespace.Align(_2f,"vertical",true);
var _31=function(row,_33){
for(var i=0;i<row.Items.length;i++){
var _35=row.Items[i];
var _36=_35.Percentage*(_33-row.FixedSize)-_35.PaddingDiff;
var _37=_35.firstChild.firstChild;
RadTabStripNamespace.Box.SetOuterHeight(_37,Math.floor(_36));
}
};
_30.InterateOverRows(_31);
};
RadTabStripNamespace.Align.Bottom=function(_38){
var _39=new RadTabStripNamespace.Align(_38,"vertical");
var _3a=function(row,_3c){
var _3d=row.Items[0];
_3d.style.marginTop=(_3c-row.Size-1)+"px";
};
_39.InterateOverRows(_3a);
};
RadTabStripNamespace.Align.Middle=function(_3e){
var _3f=new RadTabStripNamespace.Align(_3e,"vertical");
var _40=function(row,_42){
var _43=row.Items[0];
var _44=Math.floor((_42-row.Size)/2)+"px";
_43.style.marginTop=_44;
};
_3f.InterateOverRows(_40);
};;if(typeof (window.RadTabStripNamespace)=="undefined"){
window.RadTabStripNamespace=new Object();
}
RadTabStripNamespace.Box={GetOuterWidth:function(_1){
var _2=this.GetCurrentStyle(_1);
return _1.offsetWidth+this.GetHorizontalMarginValue(_2);
},GetOuterHeight:function(_3){
var _4=this.GetCurrentStyle(_3);
return _3.offsetHeight+this.GetVerticalMarginValue(_4);
},GetInnerWidth:function(_5){
var _6=this.GetCurrentStyle(_5);
return _5.offsetWidth-this.GetHorizontalPaddingAndBorderValue(_6);
},GetInnerHeight:function(_7){
var _8=this.GetCurrentStyle(_7);
return _7.offsetHeight-this.GetVerticalPaddingAndBorderValue(_8);
},SetOuterWidth:function(_9,_a){
var _b=this.GetCurrentStyle(_9);
_a-=this.GetHorizontalMarginValue(_b);
if(RadControlsNamespace.Browser.StandardsMode){
_a-=this.GetHorizontalPaddingAndBorderValue(_b);
}
if(_a<0){
_9.style.width="auto";
}else{
_9.style.width=_a+"px";
}
},SetOuterHeight:function(_c,_d){
var _e=_d;
var _f=this.GetCurrentStyle(_c);
_d-=this.GetVerticalMarginValue(_f);
if(RadControlsNamespace.Browser.StandardsMode){
_d-=this.GetVerticalPaddingAndBorderValue(_f);
}
_c.style.height=_d+"px";
var _10=this.GetOuterHeight(_c);
if(_10!=_e){
var _11=(_10-_e);
var _12=(_e-_11);
if(_12>0){
_c.style.height=_12+"px";
}
}
},SafeParseInt:function(_13){
var _14=parseInt(_13);
return isNaN(_14)?0:_14;
},GetStyleValues:function(_15){
var _16=0;
for(var i=1;i<arguments.length;i++){
_16+=this.SafeParseInt(_15[arguments[i]]);
}
return _16;
},GetHorizontalPaddingAndBorderValue:function(_18){
return this.GetStyleValues(_18,"borderLeftWidth","paddingLeft","paddingRight","borderRightWidth");
},GetVerticalPaddingAndBorderValue:function(_19){
return this.GetStyleValues(_19,"borderTopWidth","paddingTop","paddingBottom","borderBottomWidth");
},GetHorizontalMarginValue:function(_1a){
return this.GetStyleValues(_1a,"marginLeft","marginRight");
},GetVerticalMarginValue:function(_1b){
return this.GetStyleValues(_1b,"marginTop","marginBottom");
},GetCurrentStyle:function(_1c){
if(_1c.currentStyle){
return _1c.currentStyle;
}else{
if(document.defaultView&&document.defaultView.getComputedStyle){
return document.defaultView.getComputedStyle(_1c,null);
}else{
return null;
}
}
}};;if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.Browser)=="undefined"||typeof (window.RadControlsNamespace.Browser.Version)==null||window.RadControlsNamespace.Browser.Version<1){
window.RadControlsNamespace.Browser={Version:1};
window.RadControlsNamespace.Browser.ParseBrowserInfo=function(){
this.IsMacIE=(navigator.appName=="Microsoft Internet Explorer")&&((navigator.userAgent.toLowerCase().indexOf("mac")!=-1)||(navigator.appVersion.toLowerCase().indexOf("mac")!=-1));
this.IsSafari=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1);
this.IsMozilla=window.netscape&&!window.opera;
this.IsNetscape=/Netscape/.test(navigator.userAgent);
this.IsOpera=window.opera;
this.IsOpera9=window.opera&&(parseInt(window.opera.version())>8);
this.IsIE=!this.IsMacIE&&!this.IsMozilla&&!this.IsOpera&&!this.IsSafari;
this.IsIE7=/MSIE 7/.test(navigator.appVersion);
this.StandardsMode=this.IsSafari||this.IsOpera9||this.IsMozilla||document.compatMode=="CSS1Compat";
this.IsMac=/Mac/.test(navigator.userAgent);
};
RadControlsNamespace.Browser.ParseBrowserInfo();
};if(typeof window.RadControlsNamespace=="undefined"){
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
};if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.EventMixin)=="undefined"||typeof (window.RadControlsNamespace.EventMixin.Version)==null||window.RadControlsNamespace.EventMixin.Version<2){
RadControlsNamespace.EventMixin={Version:2,Initialize:function(_1){
_1._listeners={};
_1._eventsEnabled=true;
_1.AttachEvent=this.AttachEvent;
_1.DetachEvent=this.DetachEvent;
_1.RaiseEvent=this.RaiseEvent;
_1.EnableEvents=this.EnableEvents;
_1.DisableEvents=this.DisableEvents;
_1.DisposeEventHandlers=this.DisposeEventHandlers;
},DisableEvents:function(){
this._eventsEnabled=false;
},EnableEvents:function(){
this._eventsEnabled=true;
},AttachEvent:function(_2,_3){
if(!this._listeners[_2]){
this._listeners[_2]=[];
}
this._listeners[_2][this._listeners[_2].length]=(RadControlsNamespace.EventMixin.ResolveFunction(_3));
},DetachEvent:function(_4,_5){
var _6=this._listeners[_4];
if(!_6){
return false;
}
var _7=RadControlsNamespace.EventMixin.ResolveFunction(_5);
for(var i=0;i<_6.length;i++){
if(_7==_6[i]){
_6.splice(i,1);
return true;
}
}
return false;
},DisposeEventHandlers:function(){
for(var _9 in this._listeners){
var _a=null;
if(this._listeners.hasOwnProperty(_9)){
_a=this._listeners[_9];
for(var i=0;i<_a.length;i++){
_a[i]=null;
}
_a=null;
}
}
},ResolveFunction:function(_c){
if(typeof (_c)=="function"){
return _c;
}else{
if(typeof (window[_c])=="function"){
return window[_c];
}else{
return new Function("var Sender = arguments[0]; var Arguments = arguments[1];"+_c);
}
}
},RaiseEvent:function(_d,_e){
if(!this._eventsEnabled){
return true;
}
var _f=true;
if(this[_d]){
var _10=RadControlsNamespace.EventMixin.ResolveFunction(this[_d])(this,_e);
if(typeof (_10)=="undefined"){
_10=true;
}
_f=_f&&_10;
}
if(!this._listeners[_d]){
return _f;
}
for(var i=0;i<this._listeners[_d].length;i++){
var _12=this._listeners[_d][i];
var _10=_12(this,_e);
if(typeof (_10)=="undefined"){
_10=true;
}
_f=_f&&_10;
}
return _f;
}};
};if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.JSON)=="undefined"||typeof (window.RadControlsNamespace.JSON.Version)==null||window.RadControlsNamespace.JSON.Version<1){
window.RadControlsNamespace.JSON={Version:1,copyright:"(c)2005 JSON.org",license:"http://www.crockford.com/JSON/license.html",stringify:function(v,_2){
var a=[];
var _4=arguments[2]||{};
function e(s){
a[a.length]=s;
}
function g(x){
var c,i,l,v;
switch(typeof x){
case "object":
if(x){
if(x instanceof Array){
e("[");
l=a.length;
for(i=0;i<x.length;i+=1){
v=x[i];
if(typeof v!="undefined"&&typeof v!="function"){
if(l<a.length){
e(",");
}
g(v);
}
}
e("]");
return "";
}else{
if(typeof x.valueOf=="function"){
e("{");
l=a.length;
for(i in x){
v=x[i];
if(_2&&v==_2[i]){
continue;
}
var _a=typeof v;
if(_a=="undefined"||_a=="function"){
continue;
}
if(_a=="object"&&!_4[i]){
continue;
}
if(l<a.length){
e(",");
}
g(i);
e(":");
g(v);
}
return e("}");
}
}
}
e("null");
return "";
case "number":
e(isFinite(x)?+x:"null");
return "";
case "string":
l=x.length;
e("\"");
for(i=0;i<l;i+=1){
c=x.charAt(i);
if(c>=" "){
if(c=="\\"||c=="\""){
e("\\");
}
e(c);
}else{
switch(c){
case "\b":
e("\\b");
break;
case "\f":
e("\\f");
break;
case "\n":
e("\\n");
break;
case "\r":
e("\\r");
break;
case "\t":
e("\\t");
break;
default:
c=c.charCodeAt();
e("\\u00"+Math.floor(c/16).toString(16)+(c%16).toString(16));
}
}
}
e("\"");
return "";
case "boolean":
e(String(x));
return "";
default:
e("null");
return "";
}
}
g(v,0);
return a.join("");
},stringifyHashTable:function(_b,_c,_d){
var a=[];
if(!_d){
_d=[];
}
for(var i=0;i<_b.length;i++){
var ser=this.stringify(_b[i],_d[i]);
if(ser=="{}"){
continue;
}
a[a.length]="\""+_b[i][_c]+"\":"+ser;
}
return "{"+a.join(",")+"}";
},parse:function(_11){
return (/^([ \t\r\n,:{}\[\]]|"(\\["\\\/bfnrtu]|[^\x00-\x1f"\\]+)*"|-?\d+(\.\d*)?([eE][+-]?\d+)?|true|false|null)+$/.test(_11))&&eval("("+_11+")");
}};
};function RadMultiPage(id,_2){
var _3=window[id];
if(_3!=null&&typeof (_3.Dispose)=="function"){
_3.Dispose();
}
this.DomElement=document.getElementById(id);
this.PageViews=_2;
this.HiddenInput=document.getElementById(id+"_Selected");
this.PageView=null;
}
RadMultiPage.prototype.Dispose=function(){
if(this.disposed==null){
return;
}
this.disposed=true;
this.DomElement=null;
this.HiddenInput=null;
};
RadMultiPage.prototype.GetSelectedIndex=function(){
return parseInt(this.HiddenInput.value);
};
RadMultiPage.prototype.GetPageViewDomElement=function(_4){
return document.getElementById(this.PageViews[_4].ClientID);
};
RadMultiPage.prototype.Show=function(_5){
if(this.NavigateAfterClick){
return;
}
_5.style.display="block";
var _6=_5.getElementsByTagName("*");
for(var i=0,_8=_6.length;i<_8;i++){
var _9=_6[i];
if(_9.RadShow){
_9.RadShow();
}
}
};
RadMultiPage.prototype.Hide=function(_a){
if(this.NavigateAfterClick){
return;
}
_a.style.display="none";
};
RadMultiPage.prototype.SelectPageById=function(id){
if(id=="Null"){
return;
}
var _c=-1;
for(var i=0;i<this.PageViews.length;i++){
var _e=this.GetPageViewDomElement(i);
if(this.PageViews[i].ID==id){
if(_e){
this.Show(this.GetPageViewDomElement(i));
}
_c=i;
}else{
if(_e){
this.Hide(this.GetPageViewDomElement(i));
}
}
}
this.HiddenInput.value=_c;
};
RadMultiPage.prototype.SelectPageByIndex=function(_f){
if(_f>=this.PageViews.length){
return;
}
for(var i=0;i<this.PageViews.length;i++){
var _11=this.GetPageViewDomElement(i);
if(_11){
if(i==_f){
this.Show(_11);
}else{
this.Hide(_11);
}
}
}
this.HiddenInput.value=_f;
};;function RadTab(_1,_2){
this.Parent=null;
this.TabStrip=null;
this.SelectedTab=null;
this.SelectedIndex=-1;
this.Selected=false;
this.ScrollChildren=false;
this.ScrollPosition=0;
this.ScrollButtonsPosition=RadControlsNamespace.ScrollButtonsPosition.Right;
this.PerTabScrolling=false;
this.Tabs=[];
this.PageViewID="";
this.PageViewClientID="";
this.Index=-1;
this.GlobalIndex=-1;
this.CssClass="";
this.SelectedCssClass="selected";
this.DisabledCssClass="disabled";
this.NavigateAfterClick=false;
this.Enabled=true;
this.Value="";
this.DepthLevel=-1;
this.IsBreak=false;
this.ID=_1.id;
this.DomElement=_1;
this.Text=_1.firstChild.firstChild.innerHTML;
this.ImageDomElement=_1.getElementsByTagName("img")[0];
if(this.ImageDomElement){
if(_1.firstChild.firstChild.childNodes.length>1){
this.Text=_1.firstChild.firstChild.childNodes[1].nodeValue;
}else{
this.Text="";
}
}
this.ChildStripDomElement=_1.parentNode.getElementsByTagName("ul")[0];
}
RadTab.prototype.Initialize=function(){
RadControlsNamespace.DomEventMixin.Initialize(this);
this.AttachEventHandlers();
if(this.TabStrip.TabData[this.ID]!=null){
for(var _3 in this.TabStrip.TabData[this.ID]){
this[_3]=this.TabStrip.TabData[this.ID][_3];
}
}
RadTabStrip.CreateState(this);
};
RadTab.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var i in this.DomElement){
if(typeof (this.DomElement[i])=="function"){
this.DomElement[i]=null;
}
}
if(this.Scroll){
this.Scroll.Dispose();
}
this.DomElement=null;
this.ImageDomElement=null;
this.ChildStripDomElement=null;
};
RadTab.prototype.ClickHandler=function(e){
return this.Click(e);
};
RadTab.prototype.MouseOverHandler=function(e){
var a=this.DomElement;
var _8=RadControlsNamespace.DomEvent.GetRelatedTarget(e);
if(_8&&(_8==a||_8.parentNode==a||_8.parentNode.parentNode==a)){
return;
}
if(this.Enabled){
this.SetImageUrl(this.ImageOverUrl);
}
this.TabStrip.RaiseEvent("OnClientMouseOver",{Tab:this,EventObject:e});
};
RadTab.prototype.SetImageUrl=function(_9){
if(!this.ImageDomElement||!_9){
return;
}
_9=_9.replace(/&amp;/ig,"&");
if(this.ImageDomElement.src!=_9){
this.ImageDomElement.src=_9;
}
};
RadTab.prototype.MouseOutHandler=function(e){
var a=this.DomElement;
var to=RadControlsNamespace.DomEvent.GetRelatedTarget(e);
if(to&&(to==a||to.parentNode==a||to.parentNode.parentNode==a)){
return;
}
if(this.Enabled){
if(this.Parent.SelectedTab==this&&this.SelectedImageUrl){
this.SetImageUrl(this.SelectedImageUrl);
}else{
this.SetImageUrl(this.ImageUrl);
}
}
this.TabStrip.RaiseEvent("OnClientMouseOut",{Tab:this,EventObject:e});
};
RadTab.prototype.KeyPressHandler=function(e){
};
RadTab.prototype.FocusHandler=function(e){
if(!e.altKey){
return;
}
this.Click();
var _f=this;
setTimeout(function(){
_f.DomElement.focus();
},0);
};
RadTab.prototype.AttachEventHandlers=function(){
this.AttachDomEvent(this.DomElement,"click","ClickHandler");
this.AttachDomEvent(this.DomElement,"mouseover","MouseOverHandler");
this.AttachDomEvent(this.DomElement,"contextmenu","ContextMenuHandler");
this.AttachDomEvent(this.DomElement,"dblclick","DoubleClickHandler");
this.AttachDomEvent(this.DomElement,"mouseout","MouseOutHandler");
if(RadControlsNamespace.Browser.IsIE){
this.AttachDomEvent(this.DomElement,"focus","FocusHandler");
}
};
RadTab.prototype.DoubleClickHandler=function(e){
if(!this.TabStrip.RaiseEvent("OnClientDoubleClick",{Tab:this,EventObject:e})){
return RadControlsNamespace.DomEvent.PreventDefault(e);
}
};
RadTab.prototype.ContextMenuHandler=function(e){
if(!this.TabStrip.RaiseEvent("OnClientContextMenu",{Tab:this,EventObject:e})){
return RadControlsNamespace.DomEvent.PreventDefault(e);
}
};
RadTab.prototype.Validate=function(){
if(!this.TabStrip.CausesValidation){
return true;
}
if(typeof (Page_ClientValidate)!="function"){
return true;
}
return Page_ClientValidate(this.TabStrip.ValidationGroup);
};
RadTab.prototype.Click=function(e){
if((!this.Enabled)||(!this.Validate())){
return RadControlsNamespace.DomEvent.PreventDefault(e);
}
var _13=this.NavigateAfterClick;
if(this.DomElement.target&&this.DomElement.target!="_self"){
_13=false;
}
if(!this.TabStrip.EnableImmediateNavigation){
_13=false;
}
var _14=this.Select(_13);
if((!_14)||(!this.NavigateAfterClick)){
return RadControlsNamespace.DomEvent.PreventDefault(e);
}else{
if(!e||(this.ImageDomElement&&(e.srcElement==this.ImageDomElement))){
var _15=this.DomElement.target;
if(!_15||_15=="_self"){
location.href=this.DomElement.href;
}else{
if(_15=="_blank"){
window.open(this.DomElement.href);
}else{
if(top.frames[_15]){
top.frames[_15].window.location.href=this.DomElement.href;
}
}
}
}
}
return true;
};
RadTab.prototype.InternalUnSelect=function(_16){
this.Selected=false;
this.Parent.SelectedTab=null;
this.Parent.SelectedIndex=-1;
if(this.SelectedTab!=null&&this.TabStrip.UnSelectChildren){
this.SelectedTab.UnSelect(_16);
}
this.RecordState();
};
RadTab.prototype.UnSelect=function(_17){
if(!this.Selected){
return;
}
this.InternalUnSelect(_17);
if(!_17){
this.ModifyZIndex(-this.MaxZIndex);
this.DomElement.className=this.CssClass;
this.HideChildren();
this.SetImageUrl(this.ImageUrl);
}
this.TabStrip.RaiseEvent("OnClientTabUnSelected",{Tab:this});
};
RadTab.prototype.RecordState=function(){
this.InitialState.Selected=!this.Selected;
var _18=RadControlsNamespace.JSON.stringify(this,this.InitialState);
if(_18=="{}"){
this.TabStrip.TabsState[this.ID]="";
}else{
this.TabStrip.TabsState[this.ID]="\""+this.ID+"\":"+_18;
}
this.TabStrip.RecordState();
};
RadTab.prototype.ModifyZIndex=function(_19){
this.DomElement.style.zIndex=parseInt(this.DomElement.style.zIndex)+_19;
this.DomElement.style.cssText=this.DomElement.style.cssText;
};
RadTab.prototype.InternalSelect=function(_1a){
var _1b=this.Parent.SelectedTab;
if(_1b){
this.TabStrip.InUpdate=true;
this.Parent.SelectedTab.UnSelect(_1a);
this.TabStrip.InUpdate=false;
}
this.Selected=true;
this.Parent.SelectedTab=this;
this.Parent.SelectedIndex=this.Index;
this.RecordState();
};
RadTab.prototype.Select=function(_1c){
if(!this.Enabled){
return false;
}
if(this.Selected&&!this.TabStrip.ClickSelectedTab){
return false;
}
var _1d=this.Parent.SelectedTab;
var _1e={Tab:this,PreviousTab:_1d};
if(!this.TabStrip.RaiseEvent("OnClientTabSelecting",_1e)){
return false;
}
this.TabStrip.SelectPageView(this);
this.InternalSelect(_1c);
if(!_1c){
if(this.TabStrip.ReorderTabRows&&!this.TabStrip.RenderInProgress()){
this.PopRow();
}
this.DomElement.className=this.SelectedCssClass;
this.ModifyZIndex(this.MaxZIndex);
this.FixFirstTabPosition();
this.SetImageUrl(this.SelectedImageUrl);
}
this.ShowChildren(_1c);
this.TabStrip.RaiseEvent("OnClientTabSelected",_1e);
return true;
};
RadTab.prototype.FixFirstTabPosition=function(){
if(this.Parent.Tabs[0]&&this.Parent.Tabs[0].DomElement){
this.Parent.Tabs[0].DomElement.style.cssText=this.Parent.Tabs[0].DomElement.style.cssText;
}
};
RadTab.prototype.SelectParents=function(){
var _1f=[];
var _20=this;
while(_20!=this.TabStrip){
_1f[_1f.length]=_20;
_20=_20.Parent;
}
var i=_1f.length;
while(i--){
_1f[i].Select();
}
};
RadTab.prototype.IsVisible=function(){
var _22=this.Parent;
if(_22==this.TabStrip){
return true;
}
while(_22!=this.TabStrip){
if(!_22.Selected){
return false;
}
_22=_22.Parent;
}
return true;
};
RadTab.prototype.ShowChildren=function(_23){
if(!this.ChildStripDomElement){
return;
}
if(!this.IsVisible()){
return;
}
if(!_23){
this.ChildStripDomElement.style.display="block";
this.TabStrip.ShowLevels(this.DepthLevel);
this.TabStrip.ApplyTabBreaks(this.ChildStripDomElement);
this.TabStrip.AlignElement(this.ChildStripDomElement);
if(this.ScrollChildren){
this.TabStrip.MakeScrollable(this);
}
}
if(this.SelectedTab){
this.SelectedTab.Selected=false;
this.SelectedTab.Select(_23);
}
};
RadTab.prototype.HideChildren=function(){
if(!this.ChildStripDomElement){
return;
}
this.TabStrip.ShowLevels(this.DepthLevel-1);
this.ChildStripDomElement.style.display="none";
if(this.SelectedTab){
this.SelectedTab.HideChildren();
}
};
RadTab.prototype.Enable=function(){
if(this.Enabled){
return;
}
this.Enabled=true;
this.DomElement.className=this.CssClass;
this.DomElement.disabled="";
this.RecordState();
if(this.Parent.SelectedTab==this&&this.SelectedImageUrl){
this.SetImageUrl(this.SelectedImageUrl);
}else{
this.SetImageUrl(this.ImageUrl);
}
this.TabStrip.RaiseEvent("OnClientTabEnabled",{Tab:this});
};
RadTab.prototype.Disable=function(){
this.Enabled=false;
this.UnSelect();
this.DomElement.className=this.DisabledCssClass;
this.DomElement.disabled="disabled";
this.RecordState();
this.SetImageUrl(this.DisabledImageUrl);
this.TabStrip.RaiseEvent("OnClientTabDisabled",{Tab:this});
};
RadTab.prototype.OnScrollStop=function(){
this.RecordState();
};
RadTab.prototype.SetCssClass=function(_24){
this.CssClass=_24;
if(this.Enabled&&!this.Selected){
this.DomElement.className=_24;
}
};
RadTab.prototype.SetText=function(_25){
this.Text=_25;
var _26=this.DomElement.firstChild.firstChild;
var _27=_26.firstChild.nodeType==3?_26.firstChild:_26.childNodes[1];
_27.nodeValue=_25;
this.RecordState();
};
RadTab.prototype.SetDisabledCssClass=function(_28){
this.DisabledCssClass=_28;
if(!this.Enabled){
this.DomElement.className=_28;
}
};
RadTab.prototype.SetSelectedCssClass=function(_29){
this.SelectedCssClass=_29;
if(this.Selected){
this.DomElement.className=_29;
}
};
RadTab.prototype.PopRow=function(){
var _2a=this.DomElement.parentNode.offsetTop;
if(this.IsBreak&&RadControlsNamespace.Browser.IsIE){
var _2b=RadTabStripNamespace.Box.GetCurrentStyle(this.DomElement);
_2a-=RadTabStripNamespace.Box.GetStyleValues(_2b,"marginTop");
}
var _2c=[];
for(var i=0;i<this.Parent.Tabs.length;i++){
var _2e=this.Parent.Tabs[i].DomElement.parentNode;
var _2f=_2e.offsetTop;
var _2b=RadTabStripNamespace.Box.GetCurrentStyle(this.Parent.Tabs[i].DomElement);
if(this.Parent.Tabs[i].IsBreak&&(this.Parent.Tabs[i].Selected)&&RadControlsNamespace.Browser.IsIE){
_2f-=RadTabStripNamespace.Box.GetStyleValues(_2b,"marginTop");
}
if(_2f==_2a||this==this.Parent.Tabs[i]){
_2c[_2c.length]=this.Parent.Tabs[i].DomElement.parentNode;
}
}
if(_2c.length==this.Parent.Tabs.length){
return;
}
var _30=this.DomElement.parentNode.parentNode;
for(var i=0;i<_2c.length;i++){
_2c[i].parentNode.removeChild(_2c[i]);
_30.appendChild(_2c[i]);
}
};;if(typeof (window.RadTabStripNamespace)=="undefined"){
window.RadTabStripNamespace=new Object();
}
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
RadTabStripNamespace.TabStripAlign={Left:0,Center:1,Right:2,Justify:3};
RadTabStripNamespace.GetChildren=function(_5,_6){
var _7=[];
var _8=_5.firstChild;
_6=_6.toLowerCase();
while(_8){
if(_8.nodeType==1&&_8.tagName.toLowerCase()==_6){
_7[_7.length]=_8;
}
_8=_8.nextSibling;
}
return _7;
};
function RadTabStrip(_9){
var _a=window[_9];
if(_a!=null&&typeof (_a.Dispose)=="function"){
_a.Dispose();
}
this.DomElement=document.getElementById(_9);
this.ChildStripDomElement=this.DomElement.getElementsByTagName("ul")[0];
this.StateField=document.getElementById(_9+"_Hidden");
this.Tabs=[];
this.AllTabs=[];
this.ID=_9;
this.LevelWraps=[];
this.LevelWraps[0]=this.ChildStripDomElement.parentNode;
RadControlsNamespace.EventMixin.Initialize(this);
this.SelectedTab=null;
this.SelectedIndex=-1;
this.IsVertical=false;
this.ReverseLevelOrder=false;
this.ScrollChildren=false;
this.EnableImmediateNavigation=true;
this.ScrollPosition=0;
this.ScrollButtonsPosition=RadControlsNamespace.ScrollButtonsPosition.Right;
this.PerTabScrolling=false;
this.MultiPageID="";
this.MultiPageClientID="";
this.CausesValidation=true;
this.ValidationGroup="";
this.Enabled=true;
this.Direction="ltr";
this.Align=RadTabStripNamespace.TabStripAlign.Left;
this.ReorderTabRows=false;
this.UnSelectChildren=false;
this.ClickSelectedTab=false;
this.OnClientTabSelected="";
this.OnClientTabSelecting="";
this.OnClientMouseOver="";
this.OnClientMouseOut="";
this.OnClientTabUnSelected="";
this.OnClientTabEnabled="";
this.OnClientTabDisabled="";
this.OnClientLoad="";
this.DepthLevel=0;
this.MaxLevel=0;
this.TabData={};
this.InPostBack=false;
this.Disposed=false;
this.InitialAllTabs=[];
this.TabsState={};
this.InUpdate=false;
this.Initialized=false;
}
RadTabStrip.prototype.Dispose=function(){
if(this.Disposed){
return;
}
this.Disposed=true;
try{
if(this.Scroll){
this.Scroll.Dispose();
}
for(var i=0;i<this.AllTabs.length;i++){
this.AllTabs[i].Dispose();
}
this.DisposeDomEventHandlers();
if(this.DomElement){
this.DomElement.RadShow=null;
this.DomElement.RadResize=null;
}
this.DomElement=null;
this.ChildStripDomElement=null;
this.StateField=null;
this.LevelWraps[0]=null;
}
catch(e){
}
};
RadTabStrip.prototype.MakeScrollable=function(_c){
var _d=RadControlsNamespace.Scroll.Create(_c.ChildStripDomElement,this.IsVertical,_c);
_d.WrapNeeded=true;
_d.Initialize();
_d.OnScrollStop=function(){
_c.OnScrollStop();
};
_c.Scroll=_d;
};
RadTabStrip.prototype.AlignElement=function(_e){
if(this.IsVertical){
if(_e.offsetHeight==0){
return;
}
if(this.Align==RadTabStripNamespace.TabStripAlign.Center){
RadTabStripNamespace.Align.Middle(_e);
}else{
if(this.Align==RadTabStripNamespace.TabStripAlign.Right){
RadTabStripNamespace.Align.Bottom(_e);
}else{
if(this.Align==RadTabStripNamespace.TabStripAlign.Justify){
RadTabStripNamespace.Align.VJustify(_e);
}
}
}
}else{
if(_e.offsetWidth==0){
return;
}
if(this.Align==RadTabStripNamespace.TabStripAlign.Center){
RadTabStripNamespace.Align.Center(_e);
}else{
if(this.Align==RadTabStripNamespace.TabStripAlign.Right){
RadTabStripNamespace.Align.Right(_e);
}else{
if(this.Align==RadTabStripNamespace.TabStripAlign.Justify){
RadTabStripNamespace.Align.Justify(_e);
}
}
}
}
};
RadTabStrip.prototype.FindTabById=function(id){
for(var i=0;i<this.AllTabs.length;i++){
if(this.AllTabs[i].ID==id){
return this.AllTabs[i];
}
}
return null;
};
RadTabStrip.prototype.FindTabByText=function(_11){
for(var i=0;i<this.AllTabs.length;i++){
if(this.AllTabs[i].Text==_11){
return this.AllTabs[i];
}
}
return null;
};
RadTabStrip.prototype.FindTabByValue=function(_13){
for(var i=0;i<this.AllTabs.length;i++){
if(this.AllTabs[i].Value==_13){
return this.AllTabs[i];
}
}
return null;
};
RadTabStrip.prototype.FindTabByUrl=function(url){
for(var i=0;i<this.AllTabs.length;i++){
if(this.AllTabs[i].DomElement.href==url){
return this.AllTabs[i];
}
}
return null;
};
RadTabStrip.prototype.GetAllTabs=function(){
return this.AllTabs;
};
RadTabStrip.prototype.RenderInProgress=function(){
return ((!this.IsVertical)&&this.ChildStripDomElement.offsetWidth==0)||(this.IsVertical&&this.ChildStripDomElement.offsetHeight==0);
};
RadTabStrip.prototype.ApplyAlign=function(){
if(this.RenderInProgress()){
return;
}
this.AlignElement(this.ChildStripDomElement);
var _17=this.SelectedTab;
while(_17){
if(!_17.ChildStripDomElement){
break;
}
this.AlignElement(_17.ChildStripDomElement);
_17=_17.SelectedTab;
}
};
RadTabStrip.prototype.Initialize=function(_18,_19){
this.LoadConfiguration(_18);
this.TabData=_19;
this.DetermineDirection();
this.ApplyRTL();
this.DisableEvents();
this.CreateControlHierarchy(this,this.ChildStripDomElement);
if(!this.Enabled){
this.Disable();
}
this.ApplyTabBreaks(this.ChildStripDomElement);
this.ApplyAlign();
if(this.LevelWraps.length==1){
this.ShowLevels(1);
}
if(this.ScrollChildren){
this.MakeScrollable(this);
}
this.ApplySelected();
this.EnableEvents();
RadControlsNamespace.DomEventMixin.Initialize(this);
this.AttachEventHandlers();
this.Initialized=true;
RadTabStrip.CreateState(this);
this.RaiseEvent("OnClientLoad",null);
this.RecordState();
};
RadTabStrip.CreateState=function(_1a){
_1a.InitialState={};
for(var i in _1a){
var _1c=typeof _1a[i];
if(_1c=="number"||_1c=="string"||_1c=="boolean"){
_1a.InitialState[i]=_1a[i];
}
}
};
RadTabStrip.prototype.AttachEventHandlers=function(){
this.HandleResize();
this.AttachDomEvent(window,"unload","Dispose");
this.AttachDomEvent(window,"load","HandleResize");
if(this.RenderInProgress()){
this.AttachDomEvent(window,"load","PopRowOnLoad");
}
this.AttachDomEvent(window,"resize","HandleResize");
var _1d=this;
this.DomElement.RadShow=function(){
_1d.HandleResize();
_1d.DomElement.style.cssText=_1d.DomElement.style.cssText;
};
this.DomElement.RadResize=function(){
_1d.HandleResize();
_1d.DomElement.style.cssText=_1d.DomElement.style.cssText;
};
};
RadTabStrip.prototype.PopRowOnLoad=function(){
if(this.ReorderTabRows&&this.SelectedTab){
if(!this.SelectedTab.NavigateAfterClick){
this.SelectedTab.PopRow();
}
}
};
RadTabStrip.prototype.ApplySelected=function(){
for(var i=0;i<this.AllTabs.length;i++){
if(this.AllTabs[i].Selected){
this.AllTabs[i].Selected=false;
this.AllTabs[i].Select();
this.AllTabs[i].DomElement.style.cssText=this.AllTabs[i].DomElement.style.cssText;
}
}
};
RadTabStrip.prototype.HandleResize=function(){
this.ApplyAlign();
if(this.Scroll){
this.Scroll.ResizeHandler();
}
var _1f=this.SelectedTab;
while(_1f){
if(_1f.Scroll){
_1f.Scroll.ResizeHandler();
}
_1f=_1f.SelectedTab;
}
};
RadTabStrip.prototype.LoadConfiguration=function(_20){
for(var _21 in _20){
this[_21]=_20[_21];
}
};
RadTabStrip.prototype.ShowLevels=function(_22){
for(var i=0;i<=this.MaxLevel;i++){
var _24=i>_22?"none":"block";
if(this.LevelWraps[i].style.display!=_24){
this.LevelWraps[i].style.display=_24;
}
}
};
RadTabStrip.prototype.DetermineDirection=function(){
var el=this.DomElement;
while(el.tagName.toLowerCase()!="html"){
if(el.dir){
this.Direction=el.dir.toLowerCase();
return;
}
el=el.parentNode;
}
this.Direction="ltr";
};
RadTabStrip.prototype.ApplyTabBreaks=function(_26){
var lis=_26.getElementsByTagName("li");
for(var i=0;i<lis.length;i++){
var li=lis[i];
if(li.className.indexOf("break")==-1){
continue;
}
var a=li.getElementsByTagName("a")[0];
if(this.Direction=="rtl"&&li.firstChild.tagName.toLowerCase()=="a"){
a.style.cssFloat="right";
a.style.styleFloat="right";
}
}
};
RadTabStrip.prototype.CreateTab=function(_2b,_2c,_2d){
var tab=new RadTab(_2c);
tab.MaxZIndex=_2d;
tab.DepthLevel=_2b.DepthLevel+1;
tab.Parent=_2b;
tab.TabStrip=this;
tab.Index=_2b.Tabs.length;
tab.GlobalIndex=this.AllTabs.length;
return tab;
};
RadTabStrip.prototype.CreateTabObject=function(_2f,_30,_31){
var tab=this.CreateTab(_2f,_30,_31);
_2f.Tabs[_2f.Tabs.length]=tab;
this.AllTabs[this.AllTabs.length]=tab;
return tab;
};
RadTabStrip.prototype.CreateLevelWrap=function(_33){
if(this.LevelWraps[_33]){
return this.LevelWraps[_33];
}
this.LevelWraps[_33]=document.createElement("div");
this.LevelWraps[_33].style.display=_33>0?"none":"block";
if(this.ReverseLevelOrder&&_33>0){
this.DomElement.insertBefore(this.LevelWraps[_33],this.LevelWraps[_33-1]);
}else{
this.DomElement.appendChild(this.LevelWraps[_33]);
}
this.LevelWraps[_33].className="levelwrap level"+(_33+1);
if(this.Direction=="rtl"){
this.LevelWraps[_33].style.cssFloat="right";
this.LevelWraps[_33].style.styleFloat="right";
}
return this.LevelWraps[_33];
};
RadTabStrip.prototype.CreateControlHierarchy=function(_34,_35){
this.MaxLevel=Math.max(_34.DepthLevel,this.MaxLevel);
if(_34.DepthLevel>0){
this.CreateLevelWrap(_34.DepthLevel).appendChild(_35);
}
var lis=RadTabStripNamespace.GetChildren(_35,"li");
for(var i=0;i<lis.length;i++){
var li=lis[i];
if(i==0){
li.className+=" first";
}
var _39=li.getElementsByTagName("a")[0];
if(!_39){
continue;
}
_39.style.zIndex=lis.length-i;
var tab=this.CreateTabObject(_34,_39,lis.length);
tab.Initialize();
if(tab.ChildStripDomElement){
this.CreateControlHierarchy(tab,tab.ChildStripDomElement);
}
}
if(li){
li.className+=" last";
}
};
RadTabStrip.prototype.SelectPageView=function(tab){
if(!this.Initialized){
return;
}
if(this.MultiPageClientID==""||typeof (window[this.MultiPageClientID])=="undefined"||window[this.MultiPageClientID].innerHTML){
return;
}
var _3c=window[this.MultiPageClientID];
if(tab.NavigateAfterClick&&this.EnableImmediateNavigation){
_3c.NavigateAfterClick=true;
}
if(tab.PageViewID){
_3c.SelectPageById(tab.PageViewID);
}else{
_3c.SelectPageByIndex(tab.GlobalIndex);
}
};
RadTabStrip.prototype.ApplyRTL=function(){
if(this.Direction=="ltr"){
return;
}
if(RadControlsNamespace.Browser.IsIE){
this.DomElement.dir="ltr";
}
var lis=this.DomElement.getElementsByTagName("li");
if(this.IsVertical){
return;
}
for(var i=0;i<lis.length;i++){
if(lis[i].className.indexOf("break")>-1){
continue;
}
lis[i].style.styleFloat="right";
lis[i].style.cssFloat="right";
}
var uls=this.DomElement.getElementsByTagName("ul");
for(var i=0;i<uls.length;i++){
uls[i].style["clear"]="right";
}
};
RadTabStrip.prototype.Enable=function(){
this.Enabled=true;
this.DomElement.disabled="";
this.InUpdate=true;
for(var i=0;i<this.AllTabs.length;i++){
this.AllTabs[i].Enable();
}
this.InUpdate=false;
this.RecordState();
};
RadTabStrip.prototype.Disable=function(){
this.Enabled=false;
this.DomElement.disabled="disabled";
this.InUpdate=true;
for(var i=0;i<this.AllTabs.length;i++){
this.AllTabs[i].Disable();
}
this.InUpdate=false;
this.RecordState();
};
RadTabStrip.prototype.RecordState=function(){
if(this.InUpdate||!this.Initialized||!this.Enabled){
return;
}
var _42=RadControlsNamespace.JSON.stringify(this,this.InitialState);
var _43=[];
for(var i in this.TabsState){
if(this.TabsState[i]==""){
continue;
}
if(typeof this.TabsState[i]=="function"){
continue;
}
_43[_43.length]=this.TabsState[i];
}
this.StateField.value="{\"State\":"+_42+",\"TabState\":{"+_43.join(",")+"}}";
};
RadTabStrip.prototype.OnScrollStop=function(){
this.RecordState();
};;if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
RadControlsNamespace.ScrollButtonsPosition={Left:0,Middle:1,Right:2};
RadControlsNamespace.Scroll=function(_1,_2,_3){
this.Owner=_3;
this.Element=_1;
this.IsVertical=_2;
this.ScrollButtonsPosition=_3.ScrollButtonsPosition;
this.ScrollPosition=_3.ScrollPosition;
this.PerTabScrolling=_3.PerTabScrolling;
this.ScrollOnHover=false;
this.WrapNeeded=false;
this.LeaveGapsForArrows=true;
this.LeftArrowClass="leftArrow";
this.LeftArrowClassDisabled="leftArrowDisabled";
this.RightArrowClass="rightArrow";
this.RightArrowClassDisabled="rightArrowDisabled";
this.Initialized=false;
};
RadControlsNamespace.Scroll.Create=function(_4,_5,_6){
return new RadControlsNamespace.Scroll(_4,_5,_6);
};
RadControlsNamespace.Scroll.prototype.Initialize=function(){
if(this.Initialized){
this.ApplyOverflow();
this.CalculateMinMaxPosition();
this.EvaluateArrowStatus();
return false;
}
if((this.Element.offsetWidth==0&&!this.IsVertical)||(this.Element.offsetHeight==0&&this.IsVertical)){
return false;
}
this.Initialized=true;
this.ScrollAmount=2;
this.Direction=0;
if(this.WrapNeeded){
var _7=this.CreateScrollWrap();
}
this.ApplyOverflow();
this.Element.style.position="relative";
this.AttachArrows();
this.CalculateMinMaxPosition();
if(this.PerTabScrolling){
this.CalculateInitialTab();
}
this.AttachScrollMethods();
this.EvaluateArrowStatus();
this.AttachEventHandlers();
this.ScrollTo(this.ScrollPosition);
this.ApplyOverflow();
return _7;
};
RadControlsNamespace.Scroll.prototype.ApplyOverflow=function(){
if(RadControlsNamespace.Browser.IsIE){
this.Element.parentNode.style.overflow="visible";
if(this.IsVertical){
this.Element.parentNode.style.overflowX="";
this.Element.parentNode.style.overflowY="hidden";
}else{
this.Element.parentNode.style.overflowX="hidden";
this.Element.parentNode.style.overflowY="hidden";
}
}else{
this.Element.parentNode.style.overflow="hidden";
}
if(!this.ScrollNeeded()){
this.Element.parentNode.style.overflow="visible";
this.Element.parentNode.style.overflowX="visible";
this.Element.parentNode.style.overflowY="visible";
}
};
RadControlsNamespace.Scroll.prototype.ResizeHandler=function(){
if(this.Disposed){
return;
}
if(!this.Initialized){
this.Initialize();
}
if(!this.Initialized){
return;
}
if(!this.Element.offsetHeight||!this.Element.offsetWidth){
return;
}
this.CalculateMinMaxPosition();
if(this.Element.offsetWidth<this.Element.parentNode.offsetWidth){
this.ScrollTo(0);
}
var _8=parseInt(this.IsVertical?this.Element.style.top:this.Element.style.left);
if(isNaN(_8)){
_8=0;
}
var _9=this;
};
RadControlsNamespace.Scroll.prototype.AttachEventHandlers=function(){
var _a=this.Element;
var _b=this;
this.resizeClosure=function(){
_b.ResizeHandler();
};
if(window.addEventListener){
window.addEventListener("resize",this.resizeClosure,false);
}else{
window.attachEvent("onresize",this.resizeClosure);
}
};
RadControlsNamespace.Scroll.prototype.Dispose=function(){
this.Disposed=true;
this.Element=null;
clearTimeout(this.intervalPointer);
if(window.removeEventListener){
window.removeEventListener("resize",this.resizeClosure,false);
}else{
window.detachEvent("onresize",this.resizeClosure);
}
};
RadControlsNamespace.Scroll.prototype.AttachArrows=function(){
var _c=this.CreateArrow("&laquo;",1,this.LeftArrowClass);
var _d=this.CreateArrow("&raquo;",-1,this.RightArrowClass);
this.LeftArrow=_c;
this.RightArrow=_d;
if(this.IsVertical){
_c.style.left="0px";
_d.style.left="0px";
if(this.ScrollButtonsPosition==RadControlsNamespace.ScrollButtonsPosition.Middle){
_c.style.top="0px";
_d.style.bottom="0px";
}else{
if(this.ScrollButtonsPosition==RadControlsNamespace.ScrollButtonsPosition.Left){
_c.style.top="0px";
_d.style.top=_c.offsetHeight+"px";
}else{
_d.style.bottom="0px";
_c.style.bottom=_c.offsetHeight+"px";
}
}
}else{
_c.style.top="0px";
_d.style.top="0px";
if(this.ScrollButtonsPosition==RadControlsNamespace.ScrollButtonsPosition.Middle){
_c.style.left="-1px";
_d.style.right="-1px";
}else{
if(this.ScrollButtonsPosition==RadControlsNamespace.ScrollButtonsPosition.Left){
_c.style.left="-1px";
_d.style.left=(_c.offsetWidth-1)+"px";
}else{
_d.style.right="-1px";
_c.style.right=(_d.offsetWidth-1)+"px";
}
}
}
};
RadControlsNamespace.Scroll.prototype.CreateArrow=function(_e,_f,_10){
var _11=document.createElement("a");
_11.href="#";
_11.className=_10;
_11.innerHTML="&nbsp;";
_11.style.zIndex="2000";
this.Element.parentNode.appendChild(_11);
var _12=this;
_11.ScrollDirection=_f;
if(this.ScrollOnHover){
_11.onmousedown=function(){
if(this.disabled){
return false;
}
_12.ScrollAmount=3;
return true;
};
_11.onmouseup=function(){
_12.ScrollAmount=1;
};
_11.onmouseover=function(){
if(this.disabled){
return false;
}
_12.ScrollAmount=1;
_12.Scroll(this.ScrollDirection);
return true;
};
_11.onmouseout=function(){
_12.scrollAmount=0;
_12.Stop();
return false;
};
}else{
_11.onmousedown=function(){
_12.Scroll(this.ScrollDirection);
};
_11.onmouseup=function(){
_12.Stop();
};
}
_11.onclick=function(){
return false;
};
return _11;
};
RadControlsNamespace.Scroll.prototype.SetHeight=function(_13){
if(parseInt(_13)==0){
return;
}
this.Element.parentNode.style.height=_13;
this.Initialize();
};
RadControlsNamespace.Scroll.prototype.SetWidth=function(_14){
if(parseInt(_14)==0){
return;
}
this.Element.parentNode.style.width=_14;
this.Initialize();
};
RadControlsNamespace.Scroll.prototype.CreateScrollWrap=function(){
var _15=document.createElement("div");
var _16=this.Element.parentNode;
_15.appendChild(this.Element);
_15.style.position="relative";
_15.align="left";
_16.appendChild(_15);
if(this.IsVertical){
_15.style.styleFloat="left";
_15.style.cssFloat="left";
this.Element.style.display="none";
_15.style.height=_15.parentNode.parentNode.offsetHeight+"px";
this.Element.style.display="block";
}else{
var _17=0;
for(var i=0;i<this.Element.childNodes.length;i++){
var _19=this.Element.childNodes[i];
if(!_19.tagName){
continue;
}
_17+=_19.offsetWidth;
}
this.Element.style.width=(_17+3)+"px";
}
return _15;
};
RadControlsNamespace.Scroll.prototype.CalculateMinMaxPosition=function(){
if(!this.Initialized){
return;
}
if(this.IsVertical){
var _1a=this.Element.parentNode.offsetHeight-this.Element.offsetHeight;
var _1b=this.LeftArrow.offsetHeight;
var _1c=this.RightArrow.offsetHeight;
}else{
var _1a=this.Element.parentNode.offsetWidth-this.Element.offsetWidth;
var _1b=this.LeftArrow.offsetWidth;
var _1c=this.RightArrow.offsetWidth;
}
if(!this.LeaveGapsForArrows){
_1b=0;
_1c=0;
}
this.MaxScrollPosition=0;
this.MinScrollPosition=_1a-_1c-_1b;
if(this.ScrollButtonsPosition==RadControlsNamespace.ScrollButtonsPosition.Middle){
this.Offset=_1b;
}else{
if(this.ScrollButtonsPosition==RadControlsNamespace.ScrollButtonsPosition.Left){
this.Offset=_1b+_1c;
}else{
this.Offset=0;
}
}
};
RadControlsNamespace.Scroll.prototype.CalculateInitialTab=function(){
var lis=this.Element.getElementsByTagName("li");
if(lis.length>0){
var i=0;
while(this.ScrollPosition<-(this.IsVertical?lis[i].offsetTop:lis[i].offsetLeft)){
i++;
}
this.CurrentTab=i;
}
};
RadControlsNamespace.Scroll.prototype.AttachScrollMethods=function(){
if(this.PerTabScrolling){
this.Scroll=RadControlsNamespace.Scroll.StartPerTabScroll;
this.Stop=RadControlsNamespace.Scroll.StopPerTabScroll;
}else{
this.Scroll=RadControlsNamespace.Scroll.StartSmoothScroll;
this.Stop=RadControlsNamespace.Scroll.StopSmoothScroll;
}
};
RadControlsNamespace.Scroll.prototype.EvaluateArrowStatus=function(){
var _1f=!(this.ScrollPosition>this.MinScrollPosition);
var _20=!(this.ScrollPosition<this.MaxScrollPosition);
this.RightArrow.disabled=_1f;
this.LeftArrow.disabled=_20;
if(_20){
if(this.LeftArrow.className!=this.LeftArrowClassDisabled){
this.LeftArrow.className=this.LeftArrowClassDisabled;
}
}else{
if(this.LeftArrow.className!=this.LeftArrowClass){
this.LeftArrow.className=this.LeftArrowClass;
}
}
if(_1f){
if(this.RightArrow.className!=this.RightArrowClassDisabled){
this.RightArrow.className=this.RightArrowClassDisabled;
}
}else{
if(this.RightArrow.className!=this.RightArrowClass){
this.RightArrow.className=this.RightArrowClass;
}
}
};
RadControlsNamespace.Scroll.StartSmoothScroll=function(_21){
this.Stop();
this.Direction=_21;
var _22=this;
var _23=function(){
_22.ScrollBy(_22.Direction*_22.ScrollAmount);
};
_23();
this.scrollInterval=setInterval(_23,10);
};
RadControlsNamespace.Scroll.prototype.ScrollTo=function(_24){
_24=Math.max(_24,this.MinScrollPosition);
_24=Math.min(_24,this.MaxScrollPosition);
_24+=this.Offset;
if(this.IsVertical){
this.Element.style.top=_24+"px";
}else{
this.Element.style.left=_24+"px";
}
this.Owner.ScrollPosition=this.ScrollPosition=_24-this.Offset;
this.EvaluateArrowStatus();
};
RadControlsNamespace.Scroll.prototype.ScrollBy=function(_25){
var _26=this.ScrollPosition;
this.ScrollTo(_26+_25);
};
RadControlsNamespace.Scroll.StartPerTabScroll=function(_27){
this.Stop();
var lis=this.Element.getElementsByTagName("li");
var _29=this.CurrentTab-_27;
if(_29<0||_29>lis.length){
return;
}
var _2a=_27==-1?this.CurrentTab:_29;
this.CurrentTab=_29;
if(this.IsVertical){
var _2b=lis[_2a].offsetHeight;
}else{
var _2b=lis[_2a].offsetWidth;
}
this.ScrollBy(_2b*_27);
this.EvaluateArrowStatus();
};
RadControlsNamespace.Scroll.prototype.ScrollNeeded=function(){
return true;
if(this.IsVertical){
return this.Element.offsetHeight>this.Element.parentNode.offsetHeight;
}
return this.Element.offsetWidth>this.Element.parentNode.offsetWidth;
};
RadControlsNamespace.Scroll.StopSmoothScroll=function(_2c){
if(this.OnScrollStop){
this.OnScrollStop();
}
clearInterval(this.scrollInterval);
};
RadControlsNamespace.Scroll.StopPerTabScroll=function(_2d){
if(this.OnScrollStop){
this.OnScrollStop();
}
};;
//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
