(function(ke){"use strict";/**
 * @license
 * Copyright 2019 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const pe=globalThis,Ie=pe.ShadowRoot&&(pe.ShadyCSS===void 0||pe.ShadyCSS.nativeShadow)&&"adoptedStyleSheets"in Document.prototype&&"replace"in CSSStyleSheet.prototype,Te=Symbol(),Ot=new WeakMap;let Ut=class{constructor(e,t,i){if(this._$cssResult$=!0,i!==Te)throw Error("CSSResult is not constructable. Use `unsafeCSS` or `css` instead.");this.cssText=e,this.t=t}get styleSheet(){let e=this.o;const t=this.t;if(Ie&&e===void 0){const i=t!==void 0&&t.length===1;i&&(e=Ot.get(t)),e===void 0&&((this.o=e=new CSSStyleSheet).replaceSync(this.cssText),i&&Ot.set(t,e))}return e}toString(){return this.cssText}};const Br=r=>new Ut(typeof r=="string"?r:r+"",void 0,Te),E=(r,...e)=>{const t=r.length===1?r[0]:e.reduce((i,o,s)=>i+(n=>{if(n._$cssResult$===!0)return n.cssText;if(typeof n=="number")return n;throw Error("Value passed to 'css' function must be a 'css' function result: "+n+". Use 'unsafeCSS' to pass non-literal values, but take care to ensure page security.")})(o)+r[s+1],r[0]);return new Ut(t,r,Te)},Jr=(r,e)=>{if(Ie)r.adoptedStyleSheets=e.map(t=>t instanceof CSSStyleSheet?t:t.styleSheet);else for(const t of e){const i=document.createElement("style"),o=pe.litNonce;o!==void 0&&i.setAttribute("nonce",o),i.textContent=t.cssText,r.appendChild(i)}},kt=Ie?r=>r:r=>r instanceof CSSStyleSheet?(e=>{let t="";for(const i of e.cssRules)t+=i.cssText;return Br(t)})(r):r;/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const{is:Yr,defineProperty:Gr,getOwnPropertyDescriptor:Zr,getOwnPropertyNames:Kr,getOwnPropertySymbols:Xr,getPrototypeOf:Qr}=Object,fe=globalThis,It=fe.trustedTypes,ei=It?It.emptyScript:"",ti=fe.reactiveElementPolyfillSupport,X=(r,e)=>r,me={toAttribute(r,e){switch(e){case Boolean:r=r?ei:null;break;case Object:case Array:r=r==null?r:JSON.stringify(r)}return r},fromAttribute(r,e){let t=r;switch(e){case Boolean:t=r!==null;break;case Number:t=r===null?null:Number(r);break;case Object:case Array:try{t=JSON.parse(r)}catch{t=null}}return t}},Le=(r,e)=>!Yr(r,e),Tt={attribute:!0,type:String,converter:me,reflect:!1,hasChanged:Le};Symbol.metadata??=Symbol("metadata"),fe.litPropertyMetadata??=new WeakMap;class J extends HTMLElement{static addInitializer(e){this._$Ei(),(this.l??=[]).push(e)}static get observedAttributes(){return this.finalize(),this._$Eh&&[...this._$Eh.keys()]}static createProperty(e,t=Tt){if(t.state&&(t.attribute=!1),this._$Ei(),this.elementProperties.set(e,t),!t.noAccessor){const i=Symbol(),o=this.getPropertyDescriptor(e,i,t);o!==void 0&&Gr(this.prototype,e,o)}}static getPropertyDescriptor(e,t,i){const{get:o,set:s}=Zr(this.prototype,e)??{get(){return this[t]},set(n){this[t]=n}};return{get(){return o?.call(this)},set(n){const a=o?.call(this);s.call(this,n),this.requestUpdate(e,a,i)},configurable:!0,enumerable:!0}}static getPropertyOptions(e){return this.elementProperties.get(e)??Tt}static _$Ei(){if(this.hasOwnProperty(X("elementProperties")))return;const e=Qr(this);e.finalize(),e.l!==void 0&&(this.l=[...e.l]),this.elementProperties=new Map(e.elementProperties)}static finalize(){if(this.hasOwnProperty(X("finalized")))return;if(this.finalized=!0,this._$Ei(),this.hasOwnProperty(X("properties"))){const t=this.properties,i=[...Kr(t),...Xr(t)];for(const o of i)this.createProperty(o,t[o])}const e=this[Symbol.metadata];if(e!==null){const t=litPropertyMetadata.get(e);if(t!==void 0)for(const[i,o]of t)this.elementProperties.set(i,o)}this._$Eh=new Map;for(const[t,i]of this.elementProperties){const o=this._$Eu(t,i);o!==void 0&&this._$Eh.set(o,t)}this.elementStyles=this.finalizeStyles(this.styles)}static finalizeStyles(e){const t=[];if(Array.isArray(e)){const i=new Set(e.flat(1/0).reverse());for(const o of i)t.unshift(kt(o))}else e!==void 0&&t.push(kt(e));return t}static _$Eu(e,t){const i=t.attribute;return i===!1?void 0:typeof i=="string"?i:typeof e=="string"?e.toLowerCase():void 0}constructor(){super(),this._$Ep=void 0,this.isUpdatePending=!1,this.hasUpdated=!1,this._$Em=null,this._$Ev()}_$Ev(){this._$ES=new Promise(e=>this.enableUpdating=e),this._$AL=new Map,this._$E_(),this.requestUpdate(),this.constructor.l?.forEach(e=>e(this))}addController(e){(this._$EO??=new Set).add(e),this.renderRoot!==void 0&&this.isConnected&&e.hostConnected?.()}removeController(e){this._$EO?.delete(e)}_$E_(){const e=new Map,t=this.constructor.elementProperties;for(const i of t.keys())this.hasOwnProperty(i)&&(e.set(i,this[i]),delete this[i]);e.size>0&&(this._$Ep=e)}createRenderRoot(){const e=this.shadowRoot??this.attachShadow(this.constructor.shadowRootOptions);return Jr(e,this.constructor.elementStyles),e}connectedCallback(){this.renderRoot??=this.createRenderRoot(),this.enableUpdating(!0),this._$EO?.forEach(e=>e.hostConnected?.())}enableUpdating(e){}disconnectedCallback(){this._$EO?.forEach(e=>e.hostDisconnected?.())}attributeChangedCallback(e,t,i){this._$AK(e,i)}_$EC(e,t){const i=this.constructor.elementProperties.get(e),o=this.constructor._$Eu(e,i);if(o!==void 0&&i.reflect===!0){const s=(i.converter?.toAttribute!==void 0?i.converter:me).toAttribute(t,i.type);this._$Em=e,s==null?this.removeAttribute(o):this.setAttribute(o,s),this._$Em=null}}_$AK(e,t){const i=this.constructor,o=i._$Eh.get(e);if(o!==void 0&&this._$Em!==o){const s=i.getPropertyOptions(o),n=typeof s.converter=="function"?{fromAttribute:s.converter}:s.converter?.fromAttribute!==void 0?s.converter:me;this._$Em=o,this[o]=n.fromAttribute(t,s.type),this._$Em=null}}requestUpdate(e,t,i){if(e!==void 0){if(i??=this.constructor.getPropertyOptions(e),!(i.hasChanged??Le)(this[e],t))return;this.P(e,t,i)}this.isUpdatePending===!1&&(this._$ES=this._$ET())}P(e,t,i){this._$AL.has(e)||this._$AL.set(e,t),i.reflect===!0&&this._$Em!==e&&(this._$Ej??=new Set).add(e)}async _$ET(){this.isUpdatePending=!0;try{await this._$ES}catch(t){Promise.reject(t)}const e=this.scheduleUpdate();return e!=null&&await e,!this.isUpdatePending}scheduleUpdate(){return this.performUpdate()}performUpdate(){if(!this.isUpdatePending)return;if(!this.hasUpdated){if(this.renderRoot??=this.createRenderRoot(),this._$Ep){for(const[o,s]of this._$Ep)this[o]=s;this._$Ep=void 0}const i=this.constructor.elementProperties;if(i.size>0)for(const[o,s]of i)s.wrapped!==!0||this._$AL.has(o)||this[o]===void 0||this.P(o,this[o],s)}let e=!1;const t=this._$AL;try{e=this.shouldUpdate(t),e?(this.willUpdate(t),this._$EO?.forEach(i=>i.hostUpdate?.()),this.update(t)):this._$EU()}catch(i){throw e=!1,this._$EU(),i}e&&this._$AE(t)}willUpdate(e){}_$AE(e){this._$EO?.forEach(t=>t.hostUpdated?.()),this.hasUpdated||(this.hasUpdated=!0,this.firstUpdated(e)),this.updated(e)}_$EU(){this._$AL=new Map,this.isUpdatePending=!1}get updateComplete(){return this.getUpdateComplete()}getUpdateComplete(){return this._$ES}shouldUpdate(e){return!0}update(e){this._$Ej&&=this._$Ej.forEach(t=>this._$EC(t,this[t])),this._$EU()}updated(e){}firstUpdated(e){}}J.elementStyles=[],J.shadowRootOptions={mode:"open"},J[X("elementProperties")]=new Map,J[X("finalized")]=new Map,ti?.({ReactiveElement:J}),(fe.reactiveElementVersions??=[]).push("2.0.4");/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Re=globalThis,ve=Re.trustedTypes,Lt=ve?ve.createPolicy("lit-html",{createHTML:r=>r}):void 0,Rt="$lit$",I=`lit$${(Math.random()+"").slice(9)}$`,Mt="?"+I,ri=`<${Mt}>`,N=document,Q=()=>N.createComment(""),ee=r=>r===null||typeof r!="object"&&typeof r!="function",Dt=Array.isArray,ii=r=>Dt(r)||typeof r?.[Symbol.iterator]=="function",Me=`[ 	
\f\r]`,te=/<(?:(!--|\/[^a-zA-Z])|(\/?[a-zA-Z][^>\s]*)|(\/?$))/g,Nt=/-->/g,Vt=/>/g,V=RegExp(`>|${Me}(?:([^\\s"'>=/]+)(${Me}*=${Me}*(?:[^ 	
\f\r"'\`<>=]|("|')|))|$)`,"g"),Wt=/'/g,qt=/"/g,Ht=/^(?:script|style|textarea|title)$/i,oi=r=>(e,...t)=>({_$litType$:r,strings:e,values:t}),l=oi(1),A=Symbol.for("lit-noChange"),h=Symbol.for("lit-nothing"),jt=new WeakMap,W=N.createTreeWalker(N,129);function Ft(r,e){if(!Array.isArray(r)||!r.hasOwnProperty("raw"))throw Error("invalid template strings array");return Lt!==void 0?Lt.createHTML(e):e}const si=(r,e)=>{const t=r.length-1,i=[];let o,s=e===2?"<svg>":"",n=te;for(let a=0;a<t;a++){const u=r[a];let m,v,f=-1,_=0;for(;_<u.length&&(n.lastIndex=_,v=n.exec(u),v!==null);)_=n.lastIndex,n===te?v[1]==="!--"?n=Nt:v[1]!==void 0?n=Vt:v[2]!==void 0?(Ht.test(v[2])&&(o=RegExp("</"+v[2],"g")),n=V):v[3]!==void 0&&(n=V):n===V?v[0]===">"?(n=o??te,f=-1):v[1]===void 0?f=-2:(f=n.lastIndex-v[2].length,m=v[1],n=v[3]===void 0?V:v[3]==='"'?qt:Wt):n===qt||n===Wt?n=V:n===Nt||n===Vt?n=te:(n=V,o=void 0);const $=n===V&&r[a+1].startsWith("/>")?" ":"";s+=n===te?u+ri:f>=0?(i.push(m),u.slice(0,f)+Rt+u.slice(f)+I+$):u+I+(f===-2?a:$)}return[Ft(r,s+(r[t]||"<?>")+(e===2?"</svg>":"")),i]};class re{constructor({strings:e,_$litType$:t},i){let o;this.parts=[];let s=0,n=0;const a=e.length-1,u=this.parts,[m,v]=si(e,t);if(this.el=re.createElement(m,i),W.currentNode=this.el.content,t===2){const f=this.el.content.firstChild;f.replaceWith(...f.childNodes)}for(;(o=W.nextNode())!==null&&u.length<a;){if(o.nodeType===1){if(o.hasAttributes())for(const f of o.getAttributeNames())if(f.endsWith(Rt)){const _=v[n++],$=o.getAttribute(f).split(I),K=/([.?@])?(.*)/.exec(_);u.push({type:1,index:s,name:K[2],strings:$,ctor:K[1]==="."?ai:K[1]==="?"?ui:K[1]==="@"?li:ge}),o.removeAttribute(f)}else f.startsWith(I)&&(u.push({type:6,index:s}),o.removeAttribute(f));if(Ht.test(o.tagName)){const f=o.textContent.split(I),_=f.length-1;if(_>0){o.textContent=ve?ve.emptyScript:"";for(let $=0;$<_;$++)o.append(f[$],Q()),W.nextNode(),u.push({type:2,index:++s});o.append(f[_],Q())}}}else if(o.nodeType===8)if(o.data===Mt)u.push({type:2,index:s});else{let f=-1;for(;(f=o.data.indexOf(I,f+1))!==-1;)u.push({type:7,index:s}),f+=I.length-1}s++}}static createElement(e,t){const i=N.createElement("template");return i.innerHTML=e,i}}function Y(r,e,t=r,i){if(e===A)return e;let o=i!==void 0?t._$Co?.[i]:t._$Cl;const s=ee(e)?void 0:e._$litDirective$;return o?.constructor!==s&&(o?._$AO?.(!1),s===void 0?o=void 0:(o=new s(r),o._$AT(r,t,i)),i!==void 0?(t._$Co??=[])[i]=o:t._$Cl=o),o!==void 0&&(e=Y(r,o._$AS(r,e.values),o,i)),e}class ni{constructor(e,t){this._$AV=[],this._$AN=void 0,this._$AD=e,this._$AM=t}get parentNode(){return this._$AM.parentNode}get _$AU(){return this._$AM._$AU}u(e){const{el:{content:t},parts:i}=this._$AD,o=(e?.creationScope??N).importNode(t,!0);W.currentNode=o;let s=W.nextNode(),n=0,a=0,u=i[0];for(;u!==void 0;){if(n===u.index){let m;u.type===2?m=new ie(s,s.nextSibling,this,e):u.type===1?m=new u.ctor(s,u.name,u.strings,this,e):u.type===6&&(m=new ci(s,this,e)),this._$AV.push(m),u=i[++a]}n!==u?.index&&(s=W.nextNode(),n++)}return W.currentNode=N,o}p(e){let t=0;for(const i of this._$AV)i!==void 0&&(i.strings!==void 0?(i._$AI(e,i,t),t+=i.strings.length-2):i._$AI(e[t])),t++}}class ie{get _$AU(){return this._$AM?._$AU??this._$Cv}constructor(e,t,i,o){this.type=2,this._$AH=h,this._$AN=void 0,this._$AA=e,this._$AB=t,this._$AM=i,this.options=o,this._$Cv=o?.isConnected??!0}get parentNode(){let e=this._$AA.parentNode;const t=this._$AM;return t!==void 0&&e?.nodeType===11&&(e=t.parentNode),e}get startNode(){return this._$AA}get endNode(){return this._$AB}_$AI(e,t=this){e=Y(this,e,t),ee(e)?e===h||e==null||e===""?(this._$AH!==h&&this._$AR(),this._$AH=h):e!==this._$AH&&e!==A&&this._(e):e._$litType$!==void 0?this.$(e):e.nodeType!==void 0?this.T(e):ii(e)?this.k(e):this._(e)}S(e){return this._$AA.parentNode.insertBefore(e,this._$AB)}T(e){this._$AH!==e&&(this._$AR(),this._$AH=this.S(e))}_(e){this._$AH!==h&&ee(this._$AH)?this._$AA.nextSibling.data=e:this.T(N.createTextNode(e)),this._$AH=e}$(e){const{values:t,_$litType$:i}=e,o=typeof i=="number"?this._$AC(e):(i.el===void 0&&(i.el=re.createElement(Ft(i.h,i.h[0]),this.options)),i);if(this._$AH?._$AD===o)this._$AH.p(t);else{const s=new ni(o,this),n=s.u(this.options);s.p(t),this.T(n),this._$AH=s}}_$AC(e){let t=jt.get(e.strings);return t===void 0&&jt.set(e.strings,t=new re(e)),t}k(e){Dt(this._$AH)||(this._$AH=[],this._$AR());const t=this._$AH;let i,o=0;for(const s of e)o===t.length?t.push(i=new ie(this.S(Q()),this.S(Q()),this,this.options)):i=t[o],i._$AI(s),o++;o<t.length&&(this._$AR(i&&i._$AB.nextSibling,o),t.length=o)}_$AR(e=this._$AA.nextSibling,t){for(this._$AP?.(!1,!0,t);e&&e!==this._$AB;){const i=e.nextSibling;e.remove(),e=i}}setConnected(e){this._$AM===void 0&&(this._$Cv=e,this._$AP?.(e))}}class ge{get tagName(){return this.element.tagName}get _$AU(){return this._$AM._$AU}constructor(e,t,i,o,s){this.type=1,this._$AH=h,this._$AN=void 0,this.element=e,this.name=t,this._$AM=o,this.options=s,i.length>2||i[0]!==""||i[1]!==""?(this._$AH=Array(i.length-1).fill(new String),this.strings=i):this._$AH=h}_$AI(e,t=this,i,o){const s=this.strings;let n=!1;if(s===void 0)e=Y(this,e,t,0),n=!ee(e)||e!==this._$AH&&e!==A,n&&(this._$AH=e);else{const a=e;let u,m;for(e=s[0],u=0;u<s.length-1;u++)m=Y(this,a[i+u],t,u),m===A&&(m=this._$AH[u]),n||=!ee(m)||m!==this._$AH[u],m===h?e=h:e!==h&&(e+=(m??"")+s[u+1]),this._$AH[u]=m}n&&!o&&this.j(e)}j(e){e===h?this.element.removeAttribute(this.name):this.element.setAttribute(this.name,e??"")}}class ai extends ge{constructor(){super(...arguments),this.type=3}j(e){this.element[this.name]=e===h?void 0:e}}class ui extends ge{constructor(){super(...arguments),this.type=4}j(e){this.element.toggleAttribute(this.name,!!e&&e!==h)}}class li extends ge{constructor(e,t,i,o,s){super(e,t,i,o,s),this.type=5}_$AI(e,t=this){if((e=Y(this,e,t,0)??h)===A)return;const i=this._$AH,o=e===h&&i!==h||e.capture!==i.capture||e.once!==i.once||e.passive!==i.passive,s=e!==h&&(i===h||o);o&&this.element.removeEventListener(this.name,this,i),s&&this.element.addEventListener(this.name,this,e),this._$AH=e}handleEvent(e){typeof this._$AH=="function"?this._$AH.call(this.options?.host??this.element,e):this._$AH.handleEvent(e)}}class ci{constructor(e,t,i){this.element=e,this.type=6,this._$AN=void 0,this._$AM=t,this.options=i}get _$AU(){return this._$AM._$AU}_$AI(e){Y(this,e)}}const di=Re.litHtmlPolyfillSupport;di?.(re,ie),(Re.litHtmlVersions??=[]).push("3.1.2");const hi=(r,e,t)=>{const i=t?.renderBefore??e;let o=i._$litPart$;if(o===void 0){const s=t?.renderBefore??null;i._$litPart$=o=new ie(e.insertBefore(Q(),s),s,void 0,t??{})}return o._$AI(r),o};/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */let w=class extends J{constructor(){super(...arguments),this.renderOptions={host:this},this._$Do=void 0}createRenderRoot(){const e=super.createRenderRoot();return this.renderOptions.renderBefore??=e.firstChild,e}update(e){const t=this.render();this.hasUpdated||(this.renderOptions.isConnected=this.isConnected),super.update(e),this._$Do=hi(t,this.renderRoot,this.renderOptions)}connectedCallback(){super.connectedCallback(),this._$Do?.setConnected(!0)}disconnectedCallback(){super.disconnectedCallback(),this._$Do?.setConnected(!1)}render(){return A}};w._$litElement$=!0,w.finalized=!0,globalThis.litElementHydrateSupport?.({LitElement:w});const pi=globalThis.litElementPolyfillSupport;pi?.({LitElement:w}),(globalThis.litElementVersions??=[]).push("4.0.4");/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const y=r=>(e,t)=>{t!==void 0?t.addInitializer(()=>{customElements.define(r,e)}):customElements.define(r,e)};/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const fi={attribute:!0,type:String,converter:me,reflect:!1,hasChanged:Le},mi=(r=fi,e,t)=>{const{kind:i,metadata:o}=t;let s=globalThis.litPropertyMetadata.get(o);if(s===void 0&&globalThis.litPropertyMetadata.set(o,s=new Map),s.set(t.name,r),i==="accessor"){const{name:n}=t;return{set(a){const u=e.get.call(this);e.set.call(this,a),this.requestUpdate(n,u,r)},init(a){return a!==void 0&&this.P(n,void 0,r),a}}}if(i==="setter"){const{name:n}=t;return function(a){const u=this[n];e.call(this,a),this.requestUpdate(n,u,r)}}throw Error("Unsupported decorator location: "+i)};function d(r){return(e,t)=>typeof t=="object"?mi(r,e,t):((i,o,s)=>{const n=o.hasOwnProperty(s);return o.constructor.createProperty(s,n?{...i,wrapped:!0}:i),n?Object.getOwnPropertyDescriptor(o,s):void 0})(r,e,t)}/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */function b(r){return d({...r,state:!0,attribute:!1})}/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const De=(r,e,t)=>(t.configurable=!0,t.enumerable=!0,Reflect.decorate&&typeof e!="object"&&Object.defineProperty(r,e,t),t);/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */function Ne(r,e){return(t,i,o)=>{const s=n=>n.renderRoot?.querySelector(r)??null;if(e){const{get:n,set:a}=typeof i=="object"?t:o??(()=>{const u=Symbol();return{get(){return this[u]},set(m){this[u]=m}}})();return De(t,i,{get(){let u=n.call(this);return u===void 0&&(u=s(this),(u!==null||this.hasUpdated)&&a.call(this,u)),u}})}return De(t,i,{get(){return s(this)}})}}/**
 * @license
 * Copyright 2021 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */function Bt(r){return(e,t)=>{const{slot:i,selector:o}=r??{},s="slot"+(i?`[name=${i}]`:":not([name])");return De(e,t,{get(){const n=this.renderRoot?.querySelector(s),a=n?.assignedElements(r)??[];return o===void 0?a:a.filter(u=>u.matches(o))}})}}/**
 * @license
 * Copyright 2018 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Ve=r=>r??h;/**
 * @license
 * Copyright 2020 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const vi=r=>r===null||typeof r!="object"&&typeof r!="function",gi=r=>r.strings===void 0;/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Jt={ATTRIBUTE:1,CHILD:2,PROPERTY:3,BOOLEAN_ATTRIBUTE:4,EVENT:5,ELEMENT:6},Yt=r=>(...e)=>({_$litDirective$:r,values:e});let Gt=class{constructor(e){}get _$AU(){return this._$AM._$AU}_$AT(e,t,i){this._$Ct=e,this._$AM=t,this._$Ci=i}_$AS(e,t){return this.update(e,t)}update(e,t){return this.render(...t)}};/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const oe=(r,e)=>{const t=r._$AN;if(t===void 0)return!1;for(const i of t)i._$AO?.(e,!1),oe(i,e);return!0},be=r=>{let e,t;do{if((e=r._$AM)===void 0)break;t=e._$AN,t.delete(r),r=e}while(t?.size===0)},Zt=r=>{for(let e;e=r._$AM;r=e){let t=e._$AN;if(t===void 0)e._$AN=t=new Set;else if(t.has(r))break;t.add(r),yi(e)}};function bi(r){this._$AN!==void 0?(be(this),this._$AM=r,Zt(this)):this._$AM=r}function wi(r,e=!1,t=0){const i=this._$AH,o=this._$AN;if(o!==void 0&&o.size!==0)if(e)if(Array.isArray(i))for(let s=t;s<i.length;s++)oe(i[s],!1),be(i[s]);else i!=null&&(oe(i,!1),be(i));else oe(this,r)}const yi=r=>{r.type==Jt.CHILD&&(r._$AP??=wi,r._$AQ??=bi)};class _i extends Gt{constructor(){super(...arguments),this._$AN=void 0}_$AT(e,t,i){super._$AT(e,t,i),Zt(this),this.isConnected=e._$AU}_$AO(e,t=!0){e!==this.isConnected&&(this.isConnected=e,e?this.reconnected?.():this.disconnected?.()),t&&(oe(this,e),be(this))}setValue(e){if(gi(this._$Ct))this._$Ct._$AI(e,this);else{const t=[...this._$Ct._$AH];t[this._$Ci]=e,this._$Ct._$AI(t,this,0)}}disconnected(){}reconnected(){}}/**
 * @license
 * Copyright 2021 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */class $i{constructor(e){this.Y=e}disconnect(){this.Y=void 0}reconnect(e){this.Y=e}deref(){return this.Y}}class Ci{constructor(){this.Z=void 0,this.q=void 0}get(){return this.Z}pause(){this.Z??=new Promise(e=>this.q=e)}resume(){this.q?.(),this.Z=this.q=void 0}}/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Kt=r=>!vi(r)&&typeof r.then=="function",Xt=1073741823;class Pi extends _i{constructor(){super(...arguments),this._$Cwt=Xt,this._$Cbt=[],this._$CK=new $i(this),this._$CX=new Ci}render(...e){return e.find(t=>!Kt(t))??A}update(e,t){const i=this._$Cbt;let o=i.length;this._$Cbt=t;const s=this._$CK,n=this._$CX;this.isConnected||this.disconnected();for(let a=0;a<t.length&&!(a>this._$Cwt);a++){const u=t[a];if(!Kt(u))return this._$Cwt=a,u;a<o&&u===i[a]||(this._$Cwt=Xt,o=0,Promise.resolve(u).then(async m=>{for(;n.get();)await n.get();const v=s.deref();if(v!==void 0){const f=v._$Cbt.indexOf(u);f>-1&&f<v._$Cwt&&(v._$Cwt=f,v.setValue(m))}}))}return A}disconnected(){this._$CK.disconnect(),this._$CX.pause()}reconnected(){this._$CK.reconnect(this),this._$CX.resume()}}const p=Yt(Pi);var We=function(r,e){return We=Object.setPrototypeOf||{__proto__:[]}instanceof Array&&function(t,i){t.__proto__=i}||function(t,i){for(var o in i)Object.prototype.hasOwnProperty.call(i,o)&&(t[o]=i[o])},We(r,e)};function se(r,e){if(typeof e!="function"&&e!==null)throw new TypeError("Class extends value "+String(e)+" is not a constructor or null");We(r,e);function t(){this.constructor=r}r.prototype=e===null?Object.create(e):(t.prototype=e.prototype,new t)}function qe(r){var e=typeof Symbol=="function"&&Symbol.iterator,t=e&&r[e],i=0;if(t)return t.call(r);if(r&&typeof r.length=="number")return{next:function(){return r&&i>=r.length&&(r=void 0),{value:r&&r[i++],done:!r}}};throw new TypeError(e?"Object is not iterable.":"Symbol.iterator is not defined.")}function He(r,e){var t=typeof Symbol=="function"&&r[Symbol.iterator];if(!t)return r;var i=t.call(r),o,s=[],n;try{for(;(e===void 0||e-- >0)&&!(o=i.next()).done;)s.push(o.value)}catch(a){n={error:a}}finally{try{o&&!o.done&&(t=i.return)&&t.call(i)}finally{if(n)throw n.error}}return s}function je(r,e,t){if(t||arguments.length===2)for(var i=0,o=e.length,s;i<o;i++)(s||!(i in e))&&(s||(s=Array.prototype.slice.call(e,0,i)),s[i]=e[i]);return r.concat(s||Array.prototype.slice.call(e))}typeof SuppressedError=="function"&&SuppressedError;function z(r){return typeof r=="function"}function Fe(r){var e=function(i){Error.call(i),i.stack=new Error().stack},t=r(e);return t.prototype=Object.create(Error.prototype),t.prototype.constructor=t,t}var Be=Fe(function(r){return function(t){r(this),this.message=t?t.length+` errors occurred during unsubscription:
`+t.map(function(i,o){return o+1+") "+i.toString()}).join(`
  `):"",this.name="UnsubscriptionError",this.errors=t}});function Je(r,e){if(r){var t=r.indexOf(e);0<=t&&r.splice(t,1)}}var we=function(){function r(e){this.initialTeardown=e,this.closed=!1,this._parentage=null,this._finalizers=null}return r.prototype.unsubscribe=function(){var e,t,i,o,s;if(!this.closed){this.closed=!0;var n=this._parentage;if(n)if(this._parentage=null,Array.isArray(n))try{for(var a=qe(n),u=a.next();!u.done;u=a.next()){var m=u.value;m.remove(this)}}catch(P){e={error:P}}finally{try{u&&!u.done&&(t=a.return)&&t.call(a)}finally{if(e)throw e.error}}else n.remove(this);var v=this.initialTeardown;if(z(v))try{v()}catch(P){s=P instanceof Be?P.errors:[P]}var f=this._finalizers;if(f){this._finalizers=null;try{for(var _=qe(f),$=_.next();!$.done;$=_.next()){var K=$.value;try{tr(K)}catch(P){s=s??[],P instanceof Be?s=je(je([],He(s)),He(P.errors)):s.push(P)}}}catch(P){i={error:P}}finally{try{$&&!$.done&&(o=_.return)&&o.call(_)}finally{if(i)throw i.error}}}if(s)throw new Be(s)}},r.prototype.add=function(e){var t;if(e&&e!==this)if(this.closed)tr(e);else{if(e instanceof r){if(e.closed||e._hasParent(this))return;e._addParent(this)}(this._finalizers=(t=this._finalizers)!==null&&t!==void 0?t:[]).push(e)}},r.prototype._hasParent=function(e){var t=this._parentage;return t===e||Array.isArray(t)&&t.includes(e)},r.prototype._addParent=function(e){var t=this._parentage;this._parentage=Array.isArray(t)?(t.push(e),t):t?[t,e]:e},r.prototype._removeParent=function(e){var t=this._parentage;t===e?this._parentage=null:Array.isArray(t)&&Je(t,e)},r.prototype.remove=function(e){var t=this._finalizers;t&&Je(t,e),e instanceof r&&e._removeParent(this)},r.EMPTY=function(){var e=new r;return e.closed=!0,e}(),r}(),Qt=we.EMPTY;function er(r){return r instanceof we||r&&"closed"in r&&z(r.remove)&&z(r.add)&&z(r.unsubscribe)}function tr(r){z(r)?r():r.unsubscribe()}var rr={onUnhandledError:null,onStoppedNotification:null,Promise:void 0,useDeprecatedSynchronousErrorHandling:!1,useDeprecatedNextContext:!1},ir={setTimeout:function(r,e){for(var t=[],i=2;i<arguments.length;i++)t[i-2]=arguments[i];return setTimeout.apply(void 0,je([r,e],He(t)))},clearTimeout:function(r){var e=ir.delegate;return(e?.clearTimeout||clearTimeout)(r)},delegate:void 0};function Ei(r){ir.setTimeout(function(){throw r})}function or(){}function ye(r){r()}var sr=function(r){se(e,r);function e(t){var i=r.call(this)||this;return i.isStopped=!1,t?(i.destination=t,er(t)&&t.add(i)):i.destination=zi,i}return e.create=function(t,i,o){return new _e(t,i,o)},e.prototype.next=function(t){this.isStopped||this._next(t)},e.prototype.error=function(t){this.isStopped||(this.isStopped=!0,this._error(t))},e.prototype.complete=function(){this.isStopped||(this.isStopped=!0,this._complete())},e.prototype.unsubscribe=function(){this.closed||(this.isStopped=!0,r.prototype.unsubscribe.call(this),this.destination=null)},e.prototype._next=function(t){this.destination.next(t)},e.prototype._error=function(t){try{this.destination.error(t)}finally{this.unsubscribe()}},e.prototype._complete=function(){try{this.destination.complete()}finally{this.unsubscribe()}},e}(we),xi=Function.prototype.bind;function Ye(r,e){return xi.call(r,e)}var Si=function(){function r(e){this.partialObserver=e}return r.prototype.next=function(e){var t=this.partialObserver;if(t.next)try{t.next(e)}catch(i){$e(i)}},r.prototype.error=function(e){var t=this.partialObserver;if(t.error)try{t.error(e)}catch(i){$e(i)}else $e(e)},r.prototype.complete=function(){var e=this.partialObserver;if(e.complete)try{e.complete()}catch(t){$e(t)}},r}(),_e=function(r){se(e,r);function e(t,i,o){var s=r.call(this)||this,n;if(z(t)||!t)n={next:t??void 0,error:i??void 0,complete:o??void 0};else{var a;s&&rr.useDeprecatedNextContext?(a=Object.create(t),a.unsubscribe=function(){return s.unsubscribe()},n={next:t.next&&Ye(t.next,a),error:t.error&&Ye(t.error,a),complete:t.complete&&Ye(t.complete,a)}):n=t}return s.destination=new Si(n),s}return e}(sr);function $e(r){Ei(r)}function Ai(r){throw r}var zi={closed:!0,next:or,error:Ai,complete:or},Oi=function(){return typeof Symbol=="function"&&Symbol.observable||"@@observable"}();function Ui(r){return r}function ki(r){return r.length===0?Ui:r.length===1?r[0]:function(t){return r.reduce(function(i,o){return o(i)},t)}}var nr=function(){function r(e){e&&(this._subscribe=e)}return r.prototype.lift=function(e){var t=new r;return t.source=this,t.operator=e,t},r.prototype.subscribe=function(e,t,i){var o=this,s=Ti(e)?e:new _e(e,t,i);return ye(function(){var n=o,a=n.operator,u=n.source;s.add(a?a.call(s,u):u?o._subscribe(s):o._trySubscribe(s))}),s},r.prototype._trySubscribe=function(e){try{return this._subscribe(e)}catch(t){e.error(t)}},r.prototype.forEach=function(e,t){var i=this;return t=ar(t),new t(function(o,s){var n=new _e({next:function(a){try{e(a)}catch(u){s(u),n.unsubscribe()}},error:s,complete:o});i.subscribe(n)})},r.prototype._subscribe=function(e){var t;return(t=this.source)===null||t===void 0?void 0:t.subscribe(e)},r.prototype[Oi]=function(){return this},r.prototype.pipe=function(){for(var e=[],t=0;t<arguments.length;t++)e[t]=arguments[t];return ki(e)(this)},r.prototype.toPromise=function(e){var t=this;return e=ar(e),new e(function(i,o){var s;t.subscribe(function(n){return s=n},function(n){return o(n)},function(){return i(s)})})},r.create=function(e){return new r(e)},r}();function ar(r){var e;return(e=r??rr.Promise)!==null&&e!==void 0?e:Promise}function Ii(r){return r&&z(r.next)&&z(r.error)&&z(r.complete)}function Ti(r){return r&&r instanceof sr||Ii(r)&&er(r)}var Li=Fe(function(r){return function(){r(this),this.name="ObjectUnsubscribedError",this.message="object unsubscribed"}}),ur=function(r){se(e,r);function e(){var t=r.call(this)||this;return t.closed=!1,t.currentObservers=null,t.observers=[],t.isStopped=!1,t.hasError=!1,t.thrownError=null,t}return e.prototype.lift=function(t){var i=new lr(this,this);return i.operator=t,i},e.prototype._throwIfClosed=function(){if(this.closed)throw new Li},e.prototype.next=function(t){var i=this;ye(function(){var o,s;if(i._throwIfClosed(),!i.isStopped){i.currentObservers||(i.currentObservers=Array.from(i.observers));try{for(var n=qe(i.currentObservers),a=n.next();!a.done;a=n.next()){var u=a.value;u.next(t)}}catch(m){o={error:m}}finally{try{a&&!a.done&&(s=n.return)&&s.call(n)}finally{if(o)throw o.error}}}})},e.prototype.error=function(t){var i=this;ye(function(){if(i._throwIfClosed(),!i.isStopped){i.hasError=i.isStopped=!0,i.thrownError=t;for(var o=i.observers;o.length;)o.shift().error(t)}})},e.prototype.complete=function(){var t=this;ye(function(){if(t._throwIfClosed(),!t.isStopped){t.isStopped=!0;for(var i=t.observers;i.length;)i.shift().complete()}})},e.prototype.unsubscribe=function(){this.isStopped=this.closed=!0,this.observers=this.currentObservers=null},Object.defineProperty(e.prototype,"observed",{get:function(){var t;return((t=this.observers)===null||t===void 0?void 0:t.length)>0},enumerable:!1,configurable:!0}),e.prototype._trySubscribe=function(t){return this._throwIfClosed(),r.prototype._trySubscribe.call(this,t)},e.prototype._subscribe=function(t){return this._throwIfClosed(),this._checkFinalizedStatuses(t),this._innerSubscribe(t)},e.prototype._innerSubscribe=function(t){var i=this,o=this,s=o.hasError,n=o.isStopped,a=o.observers;return s||n?Qt:(this.currentObservers=null,a.push(t),new we(function(){i.currentObservers=null,Je(a,t)}))},e.prototype._checkFinalizedStatuses=function(t){var i=this,o=i.hasError,s=i.thrownError,n=i.isStopped;o?t.error(s):n&&t.complete()},e.prototype.asObservable=function(){var t=new nr;return t.source=this,t},e.create=function(t,i){return new lr(t,i)},e}(nr),lr=function(r){se(e,r);function e(t,i){var o=r.call(this)||this;return o.destination=t,o.source=i,o}return e.prototype.next=function(t){var i,o;(o=(i=this.destination)===null||i===void 0?void 0:i.next)===null||o===void 0||o.call(i,t)},e.prototype.error=function(t){var i,o;(o=(i=this.destination)===null||i===void 0?void 0:i.error)===null||o===void 0||o.call(i,t)},e.prototype.complete=function(){var t,i;(i=(t=this.destination)===null||t===void 0?void 0:t.complete)===null||i===void 0||i.call(t)},e.prototype._subscribe=function(t){var i,o;return(o=(i=this.source)===null||i===void 0?void 0:i.subscribe(t))!==null&&o!==void 0?o:Qt},e}(ur),cr={now:function(){return(cr.delegate||Date).now()},delegate:void 0},dr=function(r){se(e,r);function e(t,i,o){t===void 0&&(t=1/0),i===void 0&&(i=1/0),o===void 0&&(o=cr);var s=r.call(this)||this;return s._bufferSize=t,s._windowTime=i,s._timestampProvider=o,s._buffer=[],s._infiniteTimeWindow=!0,s._infiniteTimeWindow=i===1/0,s._bufferSize=Math.max(1,t),s._windowTime=Math.max(1,i),s}return e.prototype.next=function(t){var i=this,o=i.isStopped,s=i._buffer,n=i._infiniteTimeWindow,a=i._timestampProvider,u=i._windowTime;o||(s.push(t),!n&&s.push(a.now()+u)),this._trimBuffer(),r.prototype.next.call(this,t)},e.prototype._subscribe=function(t){this._throwIfClosed(),this._trimBuffer();for(var i=this._innerSubscribe(t),o=this,s=o._infiniteTimeWindow,n=o._buffer,a=n.slice(),u=0;u<a.length&&!t.closed;u+=s?1:2)t.next(a[u]);return this._checkFinalizedStatuses(t),i},e.prototype._trimBuffer=function(){var t=this,i=t._bufferSize,o=t._timestampProvider,s=t._buffer,n=t._infiniteTimeWindow,a=(n?1:2)*i;if(i<1/0&&a<s.length&&s.splice(0,s.length-a),!n){for(var u=o.now(),m=0,v=1;v<s.length&&s[v]<=u;v+=2)m=v;m&&s.splice(0,m+1)}},e}(ur),Ri=Fe(function(r){return function(){r(this),this.name="EmptyError",this.message="no elements in sequence"}});function hr(r,e){var t=typeof e=="object";return new Promise(function(i,o){var s=new _e({next:function(n){i(n),s.unsubscribe()},error:o,complete:function(){t?i(e.defaultValue):o(new Ri)}});r.subscribe(s)})}class Mi{#e=!1;#t="localizedtext";#r=new dr(1);async#i(){if(this.#e)return;this.#e=!0;const e=await fetch(this.#t);if(!e.ok)throw new Error(`Failed to fetch localized resources: ${e.status} ${e.statusText}`);const t=await e.json(),i=new Map;for(const[o,s]of Object.entries(t))for(const[n,a]of Object.entries(s))i.set(`${o}_${n}`,a);this.#r.next(i)}async localize(e,t,i){return this._lookup(e,t,i)}async localizeMany(e){this.#i();const t=await hr(this.#r);return e.map(i=>t.get(i)??i??"")}tokenReplace(e,t){if(t)for(let i=0;i<t.length;i++)e=e.replace("%"+i+"%",t[i]);return e}async _lookup(e,t,i){this.#i();const o=await hr(this.#r);e&&e[0]==="@"&&(e=e.substring(1));let s=e.indexOf("_");e&&s<0&&(e="general_"+e);const n=o.get(e);return n?this.tokenReplace(n,t):i||`[${e}]`}}const c=new Mi;class Di{#e="backoffice/umbracoapi/authentication/postlogin";async login(e){try{const t=new Request(this.#e,{method:"POST",body:JSON.stringify({username:e.username,password:e.password,rememberMe:e.persist}),headers:{"Content-Type":"application/json"}}),i=await fetch(t);let o;try{const s=await i.text();s&&(o=JSON.parse(this.#r(s)))}catch{}return{status:i.status,error:i.ok?void 0:await this.#t(i),data:o,twoFactorView:o?.twoFactorView}}catch(t){return{status:500,error:t instanceof Error?t.message:"Unknown error"}}}async resetPassword(e){const t=new Request("backoffice/umbracoapi/authentication/PostRequestPasswordReset",{method:"POST",body:JSON.stringify({email:e}),headers:{"Content-Type":"application/json"}}),i=await fetch(t);return{status:i.status,error:i.ok?void 0:await this.#t(i)}}async validatePasswordResetCode(e,t){const i=new Request("backoffice/umbracoapi/authentication/validatepasswordresetcode",{method:"POST",body:JSON.stringify({userId:e,resetCode:t}),headers:{"Content-Type":"application/json"}}),o=await fetch(i);return{status:o.status,error:o.ok?void 0:await this.#t(o)}}async newPassword(e,t,i){const o=new Request("backoffice/umbracoapi/authentication/PostSetPassword",{method:"POST",body:JSON.stringify({password:e,resetCode:t,userId:i}),headers:{"Content-Type":"application/json"}}),s=await fetch(o);return{status:s.status,error:s.ok?void 0:await this.#t(s)}}async newInvitedUserPassword(e){const t=new Request("backoffice/umbracoapi/authentication/PostSetInvitedUserPassword",{method:"POST",body:JSON.stringify({newPassWord:e}),headers:{"Content-Type":"application/json"}}),i=await fetch(t);return{status:i.status,error:i.ok?void 0:await this.#t(i)}}async getPasswordConfig(e){const t=new Request(`backoffice/umbracoapi/authentication/GetPasswordConfig?userId=${e}`,{method:"GET",headers:{"Content-Type":"application/json"}}),i=await fetch(t);if(i.ok){let o=await i.text();o=this.#r(o);const s=JSON.parse(o);return{status:i.status,data:s}}return{status:i.status,error:i.ok?void 0:this.#t(i)}}async getInvitedUser(){const e=new Request("backoffice/umbracoapi/authentication/GetCurrentInvitedUser",{method:"GET",headers:{"Content-Type":"application/json"}}),t=await fetch(e);if(t.ok){let i=await t.text();i=this.#r(i);const o=JSON.parse(i);return{status:t.status,user:o}}return{status:t.status,error:this.#t(t)}}async getMfaProviders(){const e=new Request("backoffice/umbracoapi/authentication/Get2faProviders",{method:"GET",headers:{"Content-Type":"application/json"}}),t=await fetch(e);if(t.ok){let i=await t.text();i=this.#r(i);const o=JSON.parse(i);return{status:t.status,providers:o}}return{status:t.status,error:await this.#t(t),providers:[]}}async validateMfaCode(e,t){const i=new Request("backoffice/umbracoapi/authentication/PostVerify2faCode",{method:"POST",body:JSON.stringify({code:e,provider:t}),headers:{"Content-Type":"application/json"}}),o=await fetch(i);let s=await o.text();s=this.#r(s);const n=JSON.parse(s);return o.ok?{data:n,status:o.status}:{status:o.status,error:n.Message??"An unknown error occurred."}}async#t(e){switch(e.status){case 400:case 401:return c.localize("login_userFailedLogin",void 0,"Oops! We couldn't log you in. Please check your credentials and try again.");case 402:return c.localize("login_2faText",void 0,"You have enabled 2-factor authentication and must verify your identity.");case 500:return c.localize("errors_receivedErrorFromServer",void 0,"Received error from server");default:return e.statusText??await c.localize("errors_receivedErrorFromServer",void 0,"Received error from server")}}#r(e){return e.startsWith(`)]}',
`)&&(e=e.split(`
`)[1]),e}}class Ni{constructor(){this.supportsPersistLogin=!1,this.disableLocalLogin=!1,this.#e=new Di,this.#t=""}#e;#t;set returnPath(e){this.#t=e}get returnPath(){const e=new URLSearchParams(window.location.search);let t=e.get("ReturnUrl")??e.get("returnPath")??this.#t;if(t.indexOf("/security/back-office/authorize")===-1&&(t=decodeURIComponent(t)),!t)return"";const i=new URL(t,window.location.origin);return i.origin!==window.location.origin?"":i.toString()}async login(e){return this.#e.login(e)}async resetPassword(e){return this.#e.resetPassword(e)}async validatePasswordResetCode(e,t){return this.#e.validatePasswordResetCode(e,t)}async newPassword(e,t,i){const o=Number.parseInt(i);return this.#e.newPassword(e,t,o)}async newInvitedUserPassword(e){return this.#e.newInvitedUserPassword(e)}async getPasswordConfig(e){return this.#e.getPasswordConfig(e)}async getInvitedUser(){return this.#e.getInvitedUser()}getMfaProviders(){return this.#e.getMfaProviders()}validateMfaCode(e,t){return this.#e.validateMfaCode(e,t)}}const g=new Ni,Vi="#umb-login-form input{width:100%;height:var(--input-height);box-sizing:border-box;display:block;border:1px solid var(--uui-color-border);border-radius:var(--uui-border-radius);background-color:var(--uui-color-surface);padding:var(--uui-size-1, 3px) var(--uui-size-space-4, 9px)}#umb-login-form uui-form-layout-item{margin-top:var(--uui-size-space-4);margin-bottom:var(--uui-size-space-4)}#umb-login-form input:focus-within{border-color:var(--uui-input-border-color-focus, var(--uui-color-border-emphasis, #a1a1a1));outline:calc(2px * var(--uui-show-focus-outline, 1)) solid var(--uui-color-focus)}#umb-login-form input:hover:not(:focus-within){border-color:var(--uui-input-border-color-hover, var(--uui-color-border-standalone, #c2c2c2))}#umb-login-form input::-ms-reveal{display:none}#umb-login-form input span{position:absolute;right:1px;top:50%;transform:translateY(-50%);z-index:100}#umb-login-form input span svg{background-color:#fff;display:block;padding:.2em;width:1.3em;height:1.3em}";var Wi=Object.defineProperty,qi=Object.getOwnPropertyDescriptor,O=(r,e,t,i)=>{for(var o=i>1?void 0:i?qi(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&Wi(e,t,o),o},Hi=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},ji=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Fi=(r,e,t)=>(Hi(r,e,"access private method"),t),Ge,pr;const fr=r=>{const e=document.createElement("input");return e.type=r.type,e.name=r.name,e.autocomplete=r.autocomplete,e.id=r.id,e.required=!0,e.inputMode=r.inputmode,e.ariaLabel=r.label,e.autofocus=r.autofocus||!1,e},mr=r=>{const e=document.createElement("label"),t=document.createElement("umb-localize");return t.key=r.localizeAlias,t.innerHTML=r.localizeFallback,e.htmlFor=r.forId,e.appendChild(t),e},vr=(r,e)=>{const t=document.createElement("uui-form-layout-item");return t.appendChild(r),t.appendChild(e),t},Bi=r=>{const e=document.createElement("style");e.innerHTML=Vi;const t=document.createElement("form");return t.id="umb-login-form",t.name="login-form",t.noValidate=!0,r.push(e),r.forEach(i=>t.appendChild(i)),t};let x=class extends w{constructor(){super(),ji(this,Ge),this.usernameIsEmail=!1,this.allowPasswordReset=!1,this.allowUserInvite=!1,this.addEventListener("umb-login-flow",r=>{r instanceof CustomEvent&&(this.flow=r.detail.flow||void 0),this.requestUpdate()})}set disableLocalLogin(r){g.disableLocalLogin=r}get disableLocalLogin(){return g.disableLocalLogin}set returnPath(r){r?g.returnPath=`${r}${encodeURIComponent(window.location.hash)}`:g.returnPath=r}get returnPath(){return g.returnPath}connectedCallback(){super.connectedCallback(),this.classList.add("uui-text"),this.classList.add("uui-font"),Fi(this,Ge,pr).call(this)}disconnectedCallback(){super.disconnectedCallback(),this._usernameLayoutItem?.remove(),this._passwordLayoutItem?.remove(),this._usernameLabel?.remove(),this._usernameInput?.remove(),this._passwordLabel?.remove(),this._passwordInput?.remove()}render(){return l`
      <umb-auth-layout
        background-image=${Ve(this.backgroundImage)}
        logo-image=${Ve(this.logoImage)}
        logo-image-alternative=${Ve(this.logoImageAlternative)}>
        ${this._renderFlowAndStatus()}
      </umb-auth-layout>
    `}_renderFlowAndStatus(){const r=new URLSearchParams(window.location.search);let e=this.flow||r.get("flow")?.toLowerCase();const t=r.get("status");if(t==="resetCodeExpired")return l`
        <umb-error-layout
          header="Hi there"
          message=${p(c.localize("login_resetCodeExpired",void 0,"The link you have clicked on is invalid or has expired"))}>
        </umb-error-layout>`;if(e==="invite-user"&&t==="false")return l`
        <umb-error-layout
          header="Hi there"
          message=${p(c.localize("user_userinviteExpiredMessage",void 0,"Welcome to Umbraco! Unfortunately your invite has expired. Please contact your administrator and ask them to resend it."))}>
        </umb-error-layout>`;switch(e&&e==="mfa"&&!g.isMfaEnabled&&(e=void 0),e){case"mfa":return l`
          <umb-mfa-page></umb-mfa-page>`;case"reset":return l`
          <umb-reset-password-page></umb-reset-password-page>`;case"reset-password":return l`
          <umb-new-password-page></umb-new-password-page>`;case"invite-user":return l`
          <umb-invite-page></umb-invite-page>`;default:return l`
          <umb-login-page
            ?allow-password-reset=${this.allowPasswordReset}
            ?username-is-email=${this.usernameIsEmail}>
            <slot></slot>
            <slot name="subheadline" slot="subheadline"></slot>
            <slot name="external" slot="external"></slot>
          </umb-login-page>`}}};Ge=new WeakSet,pr=async function(){const r=this.usernameIsEmail?await c.localize("general_email",void 0,"Email"):await c.localize("general_username",void 0,"Username"),e=await c.localize("general_password",void 0,"Password");this._usernameInput=fr({id:"username-input",type:"text",name:"username",autocomplete:"username",label:r,inputmode:this.usernameIsEmail?"email":"",autofocus:!0}),this._passwordInput=fr({id:"password-input",type:"password",name:"password",autocomplete:"current-password",label:e,inputmode:""}),this._usernameLabel=mr({forId:"username-input",localizeAlias:this.usernameIsEmail?"general_email":"general_username",localizeFallback:this.usernameIsEmail?"Email":"Username"}),this._passwordLabel=mr({forId:"password-input",localizeAlias:"general_password",localizeFallback:"Password"}),this._usernameLayoutItem=vr(this._usernameLabel,this._usernameInput),this._passwordLayoutItem=vr(this._passwordLabel,this._passwordInput),this._form=Bi([this._usernameLayoutItem,this._passwordLayoutItem]),this.insertAdjacentElement("beforeend",this._form)},O([d({type:Boolean,attribute:"disable-local-login"})],x.prototype,"disableLocalLogin",1),O([d({attribute:"background-image"})],x.prototype,"backgroundImage",2),O([d({attribute:"logo-image"})],x.prototype,"logoImage",2),O([d({attribute:"logo-image-alternative"})],x.prototype,"logoImageAlternative",2),O([d({type:Boolean,attribute:"username-is-email"})],x.prototype,"usernameIsEmail",2),O([d({type:Boolean,attribute:"allow-password-reset"})],x.prototype,"allowPasswordReset",2),O([d({type:Boolean,attribute:"allow-user-invite"})],x.prototype,"allowUserInvite",2),O([d({attribute:"return-url"})],x.prototype,"returnPath",1),x=O([y("umb-auth")],x);var Ji=Object.defineProperty,Yi=Object.getOwnPropertyDescriptor,Gi=(r,e,t,i)=>{for(var o=i>1?void 0:i?Yi(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&Ji(e,t,o),o},Zi=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},Ki=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Xi=(r,e,t)=>(Zi(r,e,"access private method"),t),Ze,gr;let Ke=class extends w{constructor(){super(...arguments),Ki(this,Ze)}render(){return l`
			<button type="button" @click=${Xi(this,Ze,gr)}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
					<path
						fill="currentColor"
						d="M7.82843 10.9999H20V12.9999H7.82843L13.1924 18.3638L11.7782 19.778L4 11.9999L11.7782 4.22168L13.1924 5.63589L7.82843 10.9999Z"></path>
				</svg>
				<span><umb-localize key="login_returnToLogin">Return to login form</umb-localize></span>
			</button>
		`}};Ze=new WeakSet,gr=function(){this.dispatchEvent(new CustomEvent("umb-login-flow",{composed:!0,detail:{flow:"login"}}))},Ke.styles=[E`
			:host {
				display: flex;
				align-items: center;
				justify-content: center;
			}
			button {
				cursor: pointer;
				background: none;
				border: 0;
				height: 1rem;
				color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
				gap: var(--uui-size-space-1);
				align-self: center;
				text-decoration: none;
				display: inline-flex;
				line-height: 1;
				font-size: 14px;
				font-family: var(--uui-font-family);
			}
			button svg {
				width: 1rem;
			}
			button:hover {
				color: var(--uui-color-interactive-emphasis);
			}
		`],Ke=Gi([y("umb-back-to-login-button")],Ke);/**
 * @license
 * Copyright 2021 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */function Ce(r,e,t){return r?e(r):t?.(r)}var Qi=Object.defineProperty,eo=Object.getOwnPropertyDescriptor,Pe=(r,e,t,i)=>{for(var o=i>1?void 0:i?eo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&Qi(e,t,o),o},to=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},br=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},wr=(r,e,t)=>(to(r,e,"access private method"),t),Xe,yr,Qe,_r;let G=class extends w{constructor(){super(...arguments),br(this,Xe),br(this,Qe)}updated(r){super.updated(r),r.has("backgroundImage")&&(this.style.setProperty("--logo-alternative-display",this.backgroundImage?"none":"unset"),this.style.setProperty("--image",`url('${this.backgroundImage}') no-repeat center center/cover`))}render(){return l`
      <div id=${this.backgroundImage?"main":"main-no-image"}>
        ${wr(this,Xe,yr).call(this)} ${wr(this,Qe,_r).call(this)}
      </div>
      ${Ce(this.logoImageAlternative,()=>l`<img id="logo-on-background" src=${this.logoImageAlternative} alt="logo" aria-hidden="true"/>`)}
    `}};Xe=new WeakSet,yr=function(){return this.backgroundImage?l`
      <div id="image-container">
        <div id="image">
          <svg
            id="curve-top"
            width="1746"
            height="1374"
            viewBox="0 0 1746 1374"
            fill="none"
            xmlns="http://www.w3.org/2000/svg">
            <path d="M8 1C61.5 722.5 206.5 1366.5 1745.5 1366.5" stroke="currentColor" stroke-width="15"/>
          </svg>
          <svg
            id="curve-bottom"
            width="1364"
            height="552"
            viewBox="0 0 1364 552"
            fill="none"
            xmlns="http://www.w3.org/2000/svg">
            <path d="M1 8C387 24 1109 11 1357 548" stroke="currentColor" stroke-width="15"/>
          </svg>
          ${Ce(this.logoImage,()=>l`<img id="logo-on-image" src=${this.logoImage} alt="logo" aria-hidden="true"/>`)}
        </div>
      </div>
    `:h},Qe=new WeakSet,_r=function(){return l`
      <div id="content-container">
        <div id="content">
          <slot></slot>
        </div>
      </div>
    `},G.styles=[E`
      :host {
        --uui-color-interactive: var(--umb-login-primary-color, #283a97);
        --uui-button-border-radius: var(--umb-login-button-border-radius, 45px);
        --uui-color-default: var(--uui-color-interactive);
        --uui-button-height: 42px;
        --uui-select-height: 38px;
        --input-height: 40px;
        --header-font-size: var(--umb-login-header-font-size, 3rem);
        --header-secondary-font-size: var(--umb-login-header-secondary-font-size, 2.4rem);
        --curves-color: var(--umb-login-curves-color, #f5c1bc);
        --curves-display: var(--umb-login-curves-display, inline);
        display: block;
        background: var(--umb-login-background, #f4f4f4);
        color: var(--umb-login-text-color, #000);
      }
      #main-no-image,
      #main {
        max-width: 1920px;
        display: flex;
        justify-content: center;
        height: 100vh;
        padding: 8px;
        box-sizing: border-box;
        margin: 0 auto;
      }
      #image-container {
        display: var(--umb-login-image-display, none);
        width: 100%;
      }
      #content-container {
        background: var(--umb-login-content-background, none);
        display: var(--umb-login-content-display, flex);
        width: var(--umb-login-content-width, 100%);
        height: var(--umb-login-content-height, 100%);
        box-sizing: border-box;
        overflow: auto;
        border-radius: var(--umb-login-content-border-radius, 0);
      }
      #content {
        max-width: 360px;
        margin: auto;
        width: 100%;
      }
      #image {
        background: var(--umb-login-image, var(--image));
        width: 100%;
        height: 100%;
        border-radius: var(--umb-login-image-border-radius, 38px);
        position: relative;
        overflow: hidden;
        color: var(--curves-color);
      }
      #image svg {
        position: absolute;
        width: 45%;
        height: fit-content;
        display: var(--curves-display);
      }
      #curve-top {
        top: 0;
        right: 0;
      }
      #curve-bottom {
        bottom: 0;
        left: 0;
      }
      #logo-on-image,
      #logo-on-background {
        position: absolute;
        top: 24px;
        left: 24px;
        height: 55px;
      }
      @media only screen and (min-width: 900px) {
        :host {
          --header-font-size: var(--umb-login-header-font-size-large, 4rem);
        }
        #main {
          padding: 32px;
          padding-right: 0;
          align-items: var(--umb-login-align-items, unset);
        }
        #image-container {
          display: var(--umb-login-image-display, block);
        }
        #content-container {
          display: var(--umb-login-content-display, flex);
          padding: 16px;
        }
        #logo-on-background {
          display: var(--logo-alternative-display);
        }
      }
    `],Pe([d({attribute:"background-image"})],G.prototype,"backgroundImage",2),Pe([d({attribute:"logo-image"})],G.prototype,"logoImage",2),Pe([d({attribute:"logo-image-alternative"})],G.prototype,"logoImageAlternative",2),G=Pe([y("umb-auth-layout")],G);var ro=Object.defineProperty,io=Object.getOwnPropertyDescriptor,et=(r,e,t,i)=>{for(var o=i>1?void 0:i?io(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&ro(e,t,o),o},$r=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},oo=(r,e,t)=>($r(r,e,"read from private field"),t?t.call(r):e.get(r)),Ee=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},tt=(r,e,t)=>($r(r,e,"access private method"),t),rt,it,Cr,ot,Pr,st,Er;let ne=class extends w{constructor(){super(...arguments),Ee(this,it),Ee(this,ot),Ee(this,st),this.resetCallState=void 0,this.error="",Ee(this,rt,async r=>{r.preventDefault();const e=r.target;if(!e||!e.checkValidity())return;const i=new FormData(e).get("email");this.resetCallState="waiting";const o=await g.resetPassword(i);this.resetCallState=o.status===200?"success":"failed",this.error=o.error||""})}render(){return this.resetCallState==="success"?tt(this,st,Er).call(this):tt(this,it,Cr).call(this)}};rt=new WeakMap,it=new WeakSet,Cr=function(){return l`
      <uui-form>
        <form id="LoginForm" name="login" @submit="${oo(this,rt)}">
          <header id="header">
            <h1>
              <umb-localize key="login_forgottenPassword">Forgotten password?</umb-localize>
            </h1>
            <span>
							<umb-localize key="login_forgottenPasswordInstruction"
              >An email will be sent to the address specified with a link to reset your password</umb-localize
              >
						</span>
          </header>
          <uui-form-layout-item>
            <uui-label for="email" slot="label" required>
              <umb-localize key="general_email">Email</umb-localize>
            </uui-label>
            <uui-input
              type="email"
              id="email"
              name="email"
              .label=${p(c.localize("general_email",void 0,"Email"))}
              required
              required-message=${p(c.localize("general_required",void 0,"Required"))}></uui-input>
          </uui-form-layout-item>
          ${tt(this,ot,Pr).call(this)}
          <uui-button
            type="submit"
            .label=${p(c.localize("general_submit",void 0,"Submit"))}
            look="primary"
            color="default"
            .state=${this.resetCallState}></uui-button>
        </form>
      </uui-form>
      <umb-back-to-login-button style="margin-top: var(--uui-size-space-6)"></umb-back-to-login-button>
    `},ot=new WeakSet,Pr=function(){return!this.error||this.resetCallState!=="failed"?h:l`<span class="text-danger">${this.error}</span>`},st=new WeakSet,Er=function(){return l`
      <umb-confirmation-layout
        header=${p(c.localize("login_forgottenPassword",void 0,"Forgotten password?"))}
        message=${p(c.localize("login_requestPasswordResetConfirmation",void 0,"An email with password reset instructions will be sent to the specified address if it matched our records"))}>
      </umb-confirmation-layout>
    `},ne.styles=[E`
      #header {
        text-align: center;
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-5);
      }
      #header span {
        color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
        font-size: 14px;
      }
      #header h1 {
        margin: 0;
        font-weight: 400;
        font-size: var(--header-secondary-font-size);
        color: var(--uui-color-interactive);
        line-height: 1.2;
      }
      form {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-layout-2);
      }
      uui-form-layout-item {
        margin: 0;
      }
      uui-input,
      uui-input-password {
        width: 100%;
        height: var(--input-height);
        border-radius: var(--uui-border-radius);
      }
      uui-input {
        width: 100%;
      }
      uui-button {
        width: 100%;
        --uui-button-padding-top-factor: 1.5;
        --uui-button-padding-bottom-factor: 1.5;
      }
      #resend {
        display: inline-flex;
        font-size: 14px;
        align-self: center;
        gap: var(--uui-size-space-1);
      }
      #resend a {
        color: var(--uui-color-selected);
        font-weight: 600;
        text-decoration: none;
      }
      #resend a:hover {
        color: var(--uui-color-interactive-emphasis);
      }
    `],et([b()],ne.prototype,"resetCallState",2),et([b()],ne.prototype,"error",2),ne=et([y("umb-reset-password-page")],ne);var so=Object.defineProperty,no=Object.getOwnPropertyDescriptor,q=(r,e,t,i)=>{for(var o=i>1?void 0:i?no(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&so(e,t,o),o},ao=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},xr=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Sr=(r,e,t)=>(ao(r,e,"access private method"),t),nt,Ar,at,zr;let T=class extends w{constructor(){super(),xr(this,nt),xr(this,at),this.state=void 0,this.page="new",this.error="",this.userId=null,this.resetCode=null;const r=new URLSearchParams(window.location.search);this.resetCode=r.get("resetCode"),this.userId=r.get("userId"),(!this.userId||!this.resetCode)&&(this.page="error")}render(){return this.userId&&this.resetCode?Sr(this,at,zr).call(this):l`
        <umb-error-layout
          header=${p(c.localize("general_error",void 0,"Error"))}
          message=${p(c.localize("errors_defaultError",void 0,"An unknown failure has occured"))}>
        </umb-error-layout>`}};nt=new WeakSet,Ar=async function(r){r.preventDefault();const e=new URLSearchParams(window.location.search),t=e.get("resetCode"),i=e.get("userId"),o=r.detail.password;if(!t||!i)return;this.state="waiting";const s=await g.newPassword(o,t,i);this.state=s.status===200?"success":"failed",this.page=s.status===200?"done":"new",this.error=s.error||""},at=new WeakSet,zr=function(){switch(this.page){case"new":return l`
          <umb-new-password-layout
            @submit=${Sr(this,nt,Ar)}
            .userId=${this.userId}
            .state=${this.state}
            .error=${this.error}></umb-new-password-layout>`;case"error":return l`
          <umb-error-layout
            header=${p(c.localize("general_error",void 0,"Error"))}
            message=${p(c.localize("errors_defaultError",void 0,"An unknown failure has occured"))}>
          </umb-error-layout>`;case"done":return l`
          <umb-confirmation-layout
            header=${p(c.localize("general_success",void 0,"Success"))}
            message=${p(c.localize("login_setPasswordConfirmation",void 0,"Your new password has been set and you may now use it to log in."))}>
          </umb-confirmation-layout>`}},q([Ne("#confirmPassword")],T.prototype,"confirmPasswordElement",2),q([b()],T.prototype,"state",2),q([b()],T.prototype,"page",2),q([b()],T.prototype,"error",2),q([b()],T.prototype,"userId",2),q([b()],T.prototype,"resetCode",2),T=q([y("umb-new-password-page")],T);/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */class ut extends Gt{constructor(e){if(super(e),this.it=h,e.type!==Jt.CHILD)throw Error(this.constructor.directiveName+"() can only be used in child bindings")}render(e){if(e===h||e==null)return this._t=void 0,this.it=e;if(e===A)return e;if(typeof e!="string")throw Error(this.constructor.directiveName+"() called with a non-string value");if(e===this.it)return this._t;this.it=e;const t=[e];return t.raw=t,this._t={_$litType$:this.constructor.resultType,strings:t,values:[]}}}ut.directiveName="unsafeHTML",ut.resultType=1;const Or=Yt(ut);async function lt(r){if(r.endsWith(".html"))return fetch(r).then(t=>t.text());const e=await import(r);if(!e.default)throw new Error("No default export found");return new e.default}function ct(r){return typeof r=="string"?l`${Or(r)}`:r}var uo=Object.defineProperty,lo=Object.getOwnPropertyDescriptor,ae=(r,e,t,i)=>{for(var o=i>1?void 0:i?lo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&uo(e,t,o),o},co=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},ho=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},po=(r,e,t)=>(co(r,e,"access private method"),t),dt,Ur;let H=class extends w{constructor(){super(...arguments),ho(this,dt),this.providers=[],this.loading=!0,this.error=null}connectedCallback(){super.connectedCallback(),po(this,dt,Ur).call(this)}async handleSubmit(r){r.preventDefault(),this.error=null;const e=r.target;if(!e)return;const t=e.elements.namedItem("2facode");if(t&&(t.error=!1,t.errorMessage=""),!e.checkValidity())return;const i=new FormData(e);let o=i.get("provider");if(o||(o=this.providers[0].value),!o){this.error="No provider selected";return}const s=i.get("token");this.buttonState="waiting";try{const n=await g.validateMfaCode(s,o);if(n.error){t?(t.error=!0,t.errorMessage=n.error):this.error=n.error,this.buttonState="failed";return}this.buttonState="success";const a=g.returnPath;a&&(location.href=a),this.dispatchEvent(new CustomEvent("umb-login-success",{bubbles:!0,composed:!0,detail:n.data}))}catch(n){n instanceof Error?this.error=n.message??"Unknown error":this.error="Unknown error",this.buttonState="failed",this.dispatchEvent(new CustomEvent("umb-login-failed",{bubbles:!0,composed:!0,detail:n}))}}renderDefaultView(){return l`
      <uui-form>
        <form id="LoginForm" @submit=${this.handleSubmit}>
          <header id="header">
            <h1>
              <umb-localize key="login_2faTitle">One last step</umb-localize>
            </h1>
            <p>
              <umb-localize key="login_2faText">
                You have enabled 2-factor authentication and must verify your identity.
              </umb-localize>
            </p>
          </header>
          <!-- if there's only one provider active, it will skip this step! -->
          ${this.providers.length>1?l`
              <uui-form-layout-item label="@login_2faMultipleText">
                <uui-label id="providerLabel" for="provider" slot="label" required>
                  <umb-localize key="login_2faMultipleText">Please choose a 2-factor provider</umb-localize>
                </uui-label>
                <uui-select id="provider" name="provider" .options=${this.providers} aria-required="true" required></uui-select>
              </uui-form-layout-item>
            `:h}
          <uui-form-layout-item>
            <uui-label id="2facodeLabel" for="2facode" slot="label" required>
              <umb-localize key="login_2faCodeInput">Verification code</umb-localize>
            </uui-label>
            <uui-input
              autofocus
              id="2facode"
              type="text"
              name="token"
              inputmode="numeric"
              autocomplete="one-time-code"
              placeholder=${p(c.localize("login_2faCodeInputHelp"),"Please enter the verification code")}
              aria-required="true"
              required
              required-message=${p(c.localize("login_2faCodeInputHelp"),"Please enter the verification code")}
              style="width:100%;">
            </uui-input>
          </uui-form-layout-item>
          ${this.error?l` <span class="text-danger">${this.error}</span> `:h}
          <uui-button
            .state=${this.buttonState}
            button-style="success"
            look="primary"
            color="default"
            label=${p(c.localize("general_validate"),"Validate")}
            type="submit"></uui-button>
        </form>
      </uui-form>
      <umb-back-to-login-button style="margin-top: var(--uui-size-space-6)"></umb-back-to-login-button>
    `}async renderCustomView(){const r=g.twoFactorView;if(!r)return h;try{const e=await lt(r);return typeof e=="object"&&(e.providers=this.providers.map(t=>t.value),e.returnPath=g.returnPath),ct(e)}catch(e){const t=e instanceof Error?e.message:"Unknown error";return console.group("[MFA login] Failed to load custom view"),console.log("Element reference",this),console.log("Custom view",r),console.error("Failed to load custom view:",e),console.groupEnd(),l`<span class="text-danger">${t}</span>`}}render(){return this.loading?l`
        <uui-loader-bar></uui-loader-bar>`:g.twoFactorView?p(this.renderCustomView(),l`
          <uui-loader-bar></uui-loader-bar>`):this.renderDefaultView()}};dt=new WeakSet,Ur=async function(){try{const r=await g.getMfaProviders();this.providers=r.providers.map(e=>({name:e,value:e,selected:!1})),this.providers.length&&(this.providers[0].selected=!0),r.error&&(this.error=r.error)}catch(r){r instanceof Error?this.error=r.message??"Unknown error":this.error="Unknown error",this.providers=[]}this.loading=!1},H.styles=[E`
      #header {
        text-align: center;
      }
      #header h1 {
        font-weight: 400;
        font-size: var(--header-secondary-font-size);
        color: var(--uui-color-interactive);
        line-height: 1.2;
      }
      form {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-layout-2);
      }
      #provider {
        width: 100%;
      }
      uui-form-layout-item {
        margin: 0;
      }
      uui-input,
      uui-input-password {
        width: 100%;
        height: var(--input-height);
        border-radius: var(--uui-border-radius);
      }
      uui-input {
        width: 100%;
      }
      uui-button {
        width: 100%;
        --uui-button-padding-top-factor: 1.5;
        --uui-button-padding-bottom-factor: 1.5;
      }
      .text-danger {
        color: var(--uui-color-danger-standalone);
      }
    `],ae([b()],H.prototype,"providers",2),ae([b()],H.prototype,"loading",2),ae([b()],H.prototype,"buttonState",2),ae([b()],H.prototype,"error",2),H=ae([y("umb-mfa-page")],H);var fo=Object.defineProperty,mo=Object.getOwnPropertyDescriptor,j=(r,e,t,i)=>{for(var o=i>1?void 0:i?mo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&fo(e,t,o),o},ht=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},L=(r,e,t)=>(ht(r,e,"read from private field"),t?t.call(r):e.get(r)),F=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},vo=(r,e,t,i)=>(ht(r,e,"write to private field"),i?i.call(r,t):e.set(r,t),t),pt=(r,e,t)=>(ht(r,e,"access private method"),t),B,ft,kr,mt,vt,Ir,xe,gt,Tr,bt,Lr;let U=class extends w{constructor(){super(...arguments),F(this,ft),F(this,vt),F(this,gt),F(this,bt),this.usernameIsEmail=!1,this.allowPasswordReset=!1,this._loginState=void 0,this._loginError="",F(this,B,void 0),F(this,mt,async r=>{r.preventDefault();const e=r.target;if(!e||!e.checkValidity())return;const t=new FormData(e),i=t.get("username"),o=t.get("password"),s=t.has("persist");if(!i||!o){this._loginError=await c.localize("auth_userFailedLogin"),this._loginState="failed";return}this._loginState="waiting";const n=await g.login({username:i,password:o,persist:s});if(this._loginError=n.error||"",this._loginState=n.error?"failed":"success",n.status===402){g.isMfaEnabled=!0,n.twoFactorView&&(g.twoFactorView=n.twoFactorView),this.dispatchEvent(new CustomEvent("umb-login-flow",{composed:!0,detail:{flow:"mfa"}}));return}if(n.error){this.dispatchEvent(new CustomEvent("umb-login-failed",{bubbles:!0,composed:!0,detail:n}));return}const a=g.returnPath;a&&(location.href=a),this.dispatchEvent(new CustomEvent("umb-login-success",{bubbles:!0,composed:!0,detail:n.data}))}),F(this,xe,()=>{L(this,B)?.requestSubmit()})}get disableLocalLogin(){return g.disableLocalLogin}render(){return l`
      <header id="header">
        <h1 id="greeting">
          <umb-localize .key=${L(this,vt,Ir)}></umb-localize>
        </h1>
        <slot name="subheadline"></slot>
      </header>
      ${this.disableLocalLogin?h:l`
          <slot @slotchange=${pt(this,ft,kr)}></slot>
          <div id="secondary-actions">
            ${Ce(g.supportsPersistLogin,()=>l`
                <uui-form-layout-item>
                  <uui-checkbox
                    name="persist"
                    .label=${p(c.localize("user_rememberMe",void 0,"Remember me"))}>
                    <umb-localize key="user_rememberMe">Remember me</umb-localize>
                  </uui-checkbox>
                </uui-form-layout-item>`)}
            ${Ce(this.allowPasswordReset,()=>l`
                  <button type="button" id="forgot-password" @click=${pt(this,bt,Lr)}>
                    <umb-localize key="login_forgottenPassword">Forgotten password?</umb-localize>
                  </button>`)}
          </div>
          <uui-button
            type="submit"
            id="umb-login-button"
            look="primary"
            @click=${L(this,xe)}
            .label=${p(c.localize("general_login",void 0,"Login"),"Login")}
            color="default"
            .state=${this._loginState}></uui-button>
          ${pt(this,gt,Tr).call(this)}
        `}
      <umb-external-login-providers-layout .showDivider=${!this.disableLocalLogin}>
        <slot name="external"></slot>
      </umb-external-login-providers-layout>
    `}};B=new WeakMap,ft=new WeakSet,kr=async function(){vo(this,B,this.slottedElements?.find(r=>r.id==="umb-login-form")),L(this,B)&&(L(this,B).addEventListener("keypress",r=>{r.key==="Enter"&&L(this,xe).call(this)}),L(this,B).onsubmit=L(this,mt))},mt=new WeakMap,vt=new WeakSet,Ir=function(){return["login_greeting0","login_greeting1","login_greeting2","login_greeting3","login_greeting4","login_greeting5","login_greeting6"][new Date().getDay()]},xe=new WeakMap,gt=new WeakSet,Tr=function(){return!this._loginError||this._loginState!=="failed"?h:l`<span class="text-error text-danger">${this._loginError}</span>`},bt=new WeakSet,Lr=function(){this.dispatchEvent(new CustomEvent("umb-login-flow",{composed:!0,detail:{flow:"reset"}}))},U.styles=[E`
      :host {
        display: flex;
        flex-direction: column;
      }
      #header {
        text-align: center;
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-5);
      }
      #header span {
        color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
        font-size: 14px;
      }
      #greeting {
        color: var(--uui-color-interactive);
        text-align: center;
        font-weight: 400;
        font-size: var(--header-font-size);
        margin: 0 0 var(--uui-size-layout-1);
        line-height: 1.2;
      }
      #umb-login-button {
        margin-top: var(--uui-size-space-4);
        width: 100%;
      }
      #forgot-password {
        cursor: pointer;
        background: none;
        border: 0;
        height: 1rem;
        color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
        gap: var(--uui-size-space-1);
        align-self: center;
        text-decoration: none;
        display: inline-flex;
        line-height: 1;
        font-size: 14px;
        font-family: var(--uui-font-family);
        margin-left: auto;
        margin-bottom: var(--uui-size-space-3);
      }
      #forgot-password:hover {
        color: var(--uui-color-interactive-emphasis);
      }
      .text-error {
        margin-top: var(--uui-size-space-4);
      }
      .text-danger {
        color: var(--uui-color-danger-standalone);
      }
      #secondary-actions {
        display: flex;
        align-items: center;
        justify-content: space-between;
      }
    `],j([d({type:Boolean,attribute:"username-is-email"})],U.prototype,"usernameIsEmail",2),j([Bt({flatten:!0})],U.prototype,"slottedElements",2),j([d({type:Boolean,attribute:"allow-password-reset"})],U.prototype,"allowPasswordReset",2),j([b()],U.prototype,"_loginState",2),j([b()],U.prototype,"_loginError",2),j([b()],U.prototype,"disableLocalLogin",1),U=j([y("umb-login-page")],U);var go=Object.defineProperty,bo=Object.getOwnPropertyDescriptor,Se=(r,e,t,i)=>{for(var o=i>1?void 0:i?bo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&go(e,t,o),o},wo=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},yo=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},_o=(r,e,t)=>(wo(r,e,"access private method"),t),wt,Rr;let ue=class extends w{constructor(){super(...arguments),yo(this,wt),this.state=void 0,this.error=""}async firstUpdated(r){super.firstUpdated(r);const e=await g.getInvitedUser();if(!e.user?.id){this.error="No invited user found";return}this.invitedUser=e.user}render(){return this.invitedUser?l`
        <umb-new-password-layout
          @submit=${_o(this,wt,Rr)}
          .userId=${this.invitedUser.id}
          .userName=${this.invitedUser.name}
          .state=${this.state}
          .error=${this.error}></umb-new-password-layout>`:this.error?l`
          <umb-error-layout
            .header=${p(c.localize("general_error",void 0,"Error"))}
            .message=${this.error}></umb-error-layout>`:l`
          <umb-error-layout
            header=${p(c.localize("general_error",void 0,"Error"))}
            message=${p(c.localize("errors_defaultError",void 0,"An unknown failure has occured"))}>
          </umb-error-layout>`}};wt=new WeakSet,Rr=async function(r){r.preventDefault();const e=r.detail.password;if(!e)return;this.state="waiting";const t=await g.newInvitedUserPassword(e);if(t.error){this.error=t.error,this.state="failed";return}this.state="success",window.location.href=g.returnPath},Se([b()],ue.prototype,"state",2),Se([b()],ue.prototype,"error",2),Se([b()],ue.prototype,"invitedUser",2),ue=Se([y("umb-invite-page")],ue);var $o=Object.defineProperty,Co=Object.getOwnPropertyDescriptor,yt=(r,e,t,i)=>{for(var o=i>1?void 0:i?Co(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&$o(e,t,o),o};let le=class extends w{constructor(){super(...arguments),this.showDivider=!0}firstUpdated(){this.slottedElements?.length?this.toggleAttribute("empty",!1):this.toggleAttribute("empty",!0)}render(){return l`
      ${this.showDivider?l`
          <div id="divider" aria-hidden="true">
            <span>${p(c.localize("general_or",void 0,"or").then(r=>r.toLocaleLowerCase()))}</span>
          </div>
        `:h}
      <div>
        <slot></slot>
      </div>
    `}};le.styles=[E`
      :host {
        margin-top: 16px;
        display: flex;
        flex-direction: column;
      }
      :host([empty]) {
        display: none;
      }
      slot {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-4);
      }
      #divider {
        width: calc(100% - 18px);
        margin: 0 auto;
        margin-bottom: 16px;
        text-align: center;
        z-index: 0;
        overflow: hidden;
      }
      #divider span {
        padding-inline: 10px;
        position: relative;
        color: var(--uui-color-border-emphasis);
      }
      #divider span::before,
      #divider span::after {
        content: '';
        display: block;
        width: 500px; /* Arbitrary value, just be bigger than 50% of the max width of the container */
        height: 1px;
        background-color: var(--uui-color-border);
        position: absolute;
        top: calc(50% + 1px);
      }
      #divider span::before {
        right: 100%;
      }
      #divider span::after {
        left: 100%;
      }
    `],yt([d({type:Boolean,attribute:"divider"})],le.prototype,"showDivider",2),yt([Bt({flatten:!0})],le.prototype,"slottedElements",2),le=yt([y("umb-external-login-providers-layout")],le);var Po=Object.defineProperty,Eo=Object.getOwnPropertyDescriptor,k=(r,e,t,i)=>{for(var o=i>1?void 0:i?Eo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&Po(e,t,o),o},Mr=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},xo=(r,e,t)=>(Mr(r,e,"read from private field"),t?t.call(r):e.get(r)),So=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Ao=(r,e,t,i)=>(Mr(r,e,"write to private field"),i?i.call(r,t):e.set(r,t),t),Ae;let C=class extends w{constructor(){super(),this.displayName="",this.providerName="",this.userViewState="loggingIn",this.icon="icon-lock",this.buttonLook="outline",this.buttonColor="default",So(this,Ae,""),new URLSearchParams(window.location.search).get("logout")==="true"&&(this.userViewState="loggedOut")}set externalLoginUrl(r){const e=new URL(r,window.location.origin),t=new URLSearchParams(window.location.search);let i=decodeURIComponent(t.get("returnPath")??"");!i&&window.location.hash&&(i=`/umbraco${window.location.hash}`),e.searchParams.append("redirectUrl",i),Ao(this,Ae,e.pathname+e.search)}get externalLoginUrl(){return xo(this,Ae)}render(){return this.customView?p(this.renderCustomView(),l`
        <uui-loader-bar></uui-loader-bar>`):this.renderDefaultView()}renderDefaultView(){return l`
      <form id="defaultView" method="post" action=${this.externalLoginUrl}>
        <uui-button
          type="submit"
          name="provider"
          .value=${this.providerName}
          .label=${p(c.localize("login_signInWith",void 0,"Sign in with").then(r=>`${r} ${this.displayName}`))}
          .look=${this.buttonLook}
          .color=${this.buttonColor}>
          ${this.displayName?l`
              <div>
                <uui-icon name=${this.icon}></uui-icon>
                <umb-localize key="login_signInWith">Sign in with</umb-localize>
                ${this.displayName}
              </div>
            `:h}
          <slot></slot>
        </uui-button>
      </form>
    `}async renderCustomView(){try{if(!this.customView)return;const r=await lt(this.customView);return typeof r=="object"&&(r.displayName=this.displayName,r.providerName=this.providerName,r.externalLoginUrl=this.externalLoginUrl,r.userViewState=this.userViewState),ct(r)}catch(r){console.group("[External login] Failed to load custom view"),console.log("Provider name",this.providerName),console.log("Element reference",this),console.log("Custom view",this.customView),console.error("Failed to load custom view:",r),console.groupEnd()}}};Ae=new WeakMap,C.styles=[E`
      #defaultView uui-button {
        width: 100%;
        --uui-button-font-weight: 400;
      }
      #defaultView uui-button div {
        /* TODO: Remove this when uui-button has setting for aligning content */
        position: absolute;
        top: 50%;
        left: 0;
        margin: auto;
        transform: translateY(-50%);
        text-align: left;
        padding-left: 15px;
      }
      #defaultView uui-icon {
				opacity: 0.85;
        padding-right: 2px;
      }
      #defaultView button {
        font-size: var(--uui-button-font-size);
        border: 1px solid var(--uui-color-border);
        border-radius: var(--uui-button-border-radius);
        width: 100%;
        padding: 9px;
        text-align: left;
        background-color: var(--uui-color-surface);
        cursor: pointer;
        display: flex;
        align-items: center;
        gap: var(--uui-size-space-2);
        box-sizing: border-box;
        line-height: 1.1; /* makes the text vertically centered */
        color: var(--uui-color-interactive);
      }
      #defaultView button:hover {
        color: var(--uui-color-interactive-emphasis);
        border-color: var(--uui-color-border-standalone);
      }
    `],k([d({attribute:"custom-view"})],C.prototype,"customView",2),k([d({attribute:"display-name"})],C.prototype,"displayName",2),k([d({attribute:"provider-name"})],C.prototype,"providerName",2),k([d({attribute:"user-view-state"})],C.prototype,"userViewState",2),k([d({attribute:"external-login-url"})],C.prototype,"externalLoginUrl",1),k([d({attribute:"icon"})],C.prototype,"icon",2),k([d({attribute:"button-look"})],C.prototype,"buttonLook",2),k([d({attribute:"button-color"})],C.prototype,"buttonColor",2),C=k([y("umb-external-login-provider")],C);var zo=Object.defineProperty,Oo=Object.getOwnPropertyDescriptor,R=(r,e,t,i)=>{for(var o=i>1?void 0:i?Oo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&zo(e,t,o),o},Uo=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},Dr=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Nr=(r,e,t)=>(Uo(r,e,"access private method"),t),_t,Vr,$t,Wr;let S=class extends w{constructor(){super(...arguments),Dr(this,_t),Dr(this,$t),this.state=void 0,this.error=""}async firstUpdated(r){if(super.firstUpdated(r),this.userId){const e=await g.getPasswordConfig(this.userId);this.passwordConfig=e.data}}renderHeader(){return this.userName?l`
        <h1>Hi, ${this.userName}</h1>
        <span>
          <umb-localize key="user_userinviteWelcomeMessage">
            Welcome to Umbraco! Just need to get your password setup and then you're good to go
          </umb-localize>
        </span>
      `:l`
        <h1>
          <umb-localize key="user_newPassword">New password</umb-localize>
        </h1>
        <span>
            <umb-localize key="login_setPasswordInstruction">Please provide a new password.</umb-localize>
        </span>
      `}render(){return l`
      <uui-form>
        <form id="LoginForm" name="login" @submit=${Nr(this,_t,Vr)}>
          <header id="header">${this.renderHeader()}</header>
          <uui-form-layout-item>
            <uui-label id="passwordLabel" for="password" slot="label" required>
              <umb-localize key="user_newPassword">New password</umb-localize>
            </uui-label>
            <uui-input-password
              type="password"
              id="password"
              name="password"
              autocomplete="new-password"
              .label=${p(c.localize("user_newPassword",void 0,"New password"))}
              required
              required-message=${p(c.localize("user_passwordIsBlank",void 0,"Your new password cannot be blank!"))}></uui-input-password>
          </uui-form-layout-item>
          <uui-form-layout-item>
            <uui-label id="confirmPasswordLabel" for="confirmPassword" slot="label" required>
              <umb-localize key="user_confirmNewPassword">Confirm new password</umb-localize>
            </uui-label>
            <uui-input-password
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              autocomplete="new-password"
              .label=${p(c.localize("user_confirmNewPassword",void 0,"Confirm new password"))}
              required
              required-message=${p(c.localize("general_required",void 0,"Required"))}></uui-input-password>
          </uui-form-layout-item>
          ${Nr(this,$t,Wr).call(this)}
          <uui-button
            type="submit"
            label=${p(c.localize("general_continue",void 0,"Continue"))}
            look="primary"
            color="default"
            .state=${this.state}></uui-button>
        </form>
      </uui-form>
      <umb-back-to-login-button style="margin-top: var(--uui-size-space-6)"></umb-back-to-login-button>
    `}};_t=new WeakSet,Vr=async function(r){if(r.preventDefault(),!this.passwordConfig)return;const e=r.target;if(this.passwordElement.setCustomValidity(""),this.confirmPasswordElement.setCustomValidity(""),!e||!e.checkValidity())return;const t=new FormData(e),i=t.get("password"),o=t.get("confirmPassword");let s=!1;if(this.passwordConfig.minPasswordLength>0&&i.length<this.passwordConfig.minPasswordLength&&(s=!0),this.passwordConfig.minNonAlphaNumericChars>0&&i.replace(/[a-zA-Z0-9]/g,"").length<this.passwordConfig?.minNonAlphaNumericChars&&(s=!0),s){const n=await c.localize("errorHandling_errorInPasswordFormat",[this.passwordConfig.minPasswordLength,this.passwordConfig.minNonAlphaNumericChars],"The password doesn't meet the minimum requirements!");this.passwordElement.setCustomValidity(n);return}if(i!==o){const n=await c.localize("user_passwordMismatch",void 0,"The confirmed password doesn't match the new password!");this.confirmPasswordElement.setCustomValidity(n);return}this.dispatchEvent(new CustomEvent("submit",{detail:{password:i}}))},$t=new WeakSet,Wr=function(){return!this.error||this.state!=="failed"?h:l`<span class="text-danger">${this.error}</span>`},S.styles=[E`
      #header {
        text-align: center;
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-5);
      }
      #header span {
        color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
        font-size: 14px;
      }
      #header h1 {
        margin: 0;
        font-weight: 400;
        font-size: var(--header-secondary-font-size);
        color: var(--uui-color-interactive);
        line-height: 1.2;
      }
      form {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-5);
      }
      uui-form-layout-item {
        margin: 0;
      }
      uui-input-password {
        width: 100%;
        height: var(--input-height);
        border-radius: var(--uui-border-radius);
      }
      uui-button {
        width: 100%;
        margin-top: var(--uui-size-space-5);
        --uui-button-padding-top-factor: 1.5;
        --uui-button-padding-bottom-factor: 1.5;
      }
      .text-danger {
        color: var(--uui-color-danger-standalone);
      }
    `],R([Ne("#password")],S.prototype,"passwordElement",2),R([Ne("#confirmPassword")],S.prototype,"confirmPasswordElement",2),R([d()],S.prototype,"state",2),R([d()],S.prototype,"error",2),R([d()],S.prototype,"userId",2),R([d()],S.prototype,"userName",2),R([b()],S.prototype,"passwordConfig",2),S=R([y("umb-new-password-layout")],S);var ko=Object.defineProperty,Io=Object.getOwnPropertyDescriptor,Ct=(r,e,t,i)=>{for(var o=i>1?void 0:i?Io(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&ko(e,t,o),o};let ce=class extends w{constructor(){super(...arguments),this.header="",this.message=""}render(){return l`
      <header id="header">
        <h1>${this.header}</h1>
        <span>${this.message}</span>
      </header>
      <umb-back-to-login-button></umb-back-to-login-button>
      <slot></slot>
    `}};ce.styles=[E`
      :host {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-layout-1);
      }
      #header {
        text-align: center;
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-5);
      }
      #header span {
        color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
        font-size: 14px;
      }
      #header h1 {
        margin: 0;
        font-weight: 400;
        font-size: var(--header-secondary-font-size);
        color: var(--uui-color-interactive);
        line-height: 1.2;
      }
      uui-button {
        width: 100%;
        margin-top: var(--uui-size-space-5);
        --uui-button-padding-top-factor: 1.5;
        --uui-button-padding-bottom-factor: 1.5;
      }
    `],Ct([d({type:String})],ce.prototype,"header",2),Ct([d({type:String})],ce.prototype,"message",2),ce=Ct([y("umb-confirmation-layout")],ce);var To=Object.defineProperty,Lo=Object.getOwnPropertyDescriptor,Pt=(r,e,t,i)=>{for(var o=i>1?void 0:i?Lo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&To(e,t,o),o};let de=class extends w{constructor(){super(...arguments),this.header="",this.message=""}render(){return l`
      <header id="header">
        <h1>${this.header}</h1>
        <span>${this.message}</span>
      </header>
      <slot></slot>
      <umb-back-to-login-button></umb-back-to-login-button>
    `}};de.styles=[E`
      :host {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-layout-1);
      }
      #header {
        text-align: center;
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-5);
      }
      #header span {
        color: var(--uui-color-text-alt); /* TODO Change to uui color when uui gets a muted text variable */
        font-size: 14px;
      }
      #header h1 {
        margin: 0;
        font-weight: 400;
        font-size: var(--header-secondary-font-size);
        color: var(--uui-color-interactive);
        line-height: 1.2;
      }
      ::slotted(uui-button) {
        width: 100%;
        margin-top: var(--uui-size-space-5);
        --uui-button-padding-top-factor: 1.5;
        --uui-button-padding-bottom-factor: 1.5;
      }
    `],Pt([d({type:String})],de.prototype,"header",2),Pt([d({type:String})],de.prototype,"message",2),de=Pt([y("umb-error-layout")],de);var Ro=Object.defineProperty,Mo=Object.getOwnPropertyDescriptor,Do=(r,e,t,i)=>{for(var o=i>1?void 0:i?Mo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&Ro(e,t,o),o};class No extends ke.UUIIconRegistry{acceptIcon(e){if(e.startsWith("{{")&&e.endsWith("}}"))return!1;const t=this.provideIcon(e);return this.#r().subscribe(i=>{i[e]?t.svg=i[e].replace("<svg",'<svg fill="currentColor"'):console.warn(`Icon ${e} not found`)}),!0}#e=!1;#t=new dr(1);#r(){return this.#e||(this.#e=!0,fetch("backoffice/umbracoapi/icon/geticons").then(e=>{if(!e.ok)throw new Error("Could not fetch icons");return e.json()}).then(e=>{this.#t.next(e),this.#t.complete()})),this.#t.asObservable()}}let qr=class extends ke.UUIIconRegistryElement{constructor(){super(),new ke.UUIIconRegistryEssential().attach(this),this.registry=new No}createRenderRoot(){return this}};qr=Do([y("umb-backoffice-icon-registry")],qr);var Vo=Object.defineProperty,Wo=Object.getOwnPropertyDescriptor,ze=(r,e,t,i)=>{for(var o=i>1?void 0:i?Wo(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&Vo(e,t,o),o},Et=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},M=(r,e,t)=>(Et(r,e,"read from private field"),t?t.call(r):e.get(r)),xt=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Hr=(r,e,t,i)=>(Et(r,e,"write to private field"),i?i.call(r,t):e.set(r,t),t),jr=(r,e,t)=>(Et(r,e,"access private method"),t),Z,D,Oe,St;let he=class extends w{constructor(){super(...arguments),xt(this,Oe),this.component=null,xt(this,Z,void 0),xt(this,D,"")}set customView(r){Hr(this,D,r),jr(this,Oe,St).call(this)}get customView(){return M(this,D)}set args(r){Hr(this,Z,r),jr(this,Oe,St).call(this)}get args(){return M(this,Z)}render(){return p(this.component)}};Z=new WeakMap,D=new WeakMap,Oe=new WeakSet,St=async function(){if(!M(this,D)||!M(this,D).endsWith(".js")&&!M(this,D).endsWith(".html"))return;const r=await lt(M(this,D));M(this,Z)&&Object.entries(M(this,Z)).forEach(([e,t])=>{r[e]=t}),this.component=ct(r)},ze([d({attribute:"custom-view"})],he.prototype,"customView",1),ze([d({type:Object,attribute:"args"})],he.prototype,"args",1),ze([b()],he.prototype,"component",2),he=ze([y("umb-custom-view")],he);var qo=Object.defineProperty,Ho=Object.getOwnPropertyDescriptor,At=(r,e,t,i)=>{for(var o=i>1?void 0:i?Ho(e,t):e,s=r.length-1,n;s>=0;s--)(n=r[s])&&(o=(i?n(e,t,o):n(o))||o);return i&&o&&qo(e,t,o),o},jo=(r,e,t)=>{if(!e.has(r))throw TypeError("Cannot "+t)},Fo=(r,e,t)=>{if(e.has(r))throw TypeError("Cannot add the same private member more than once");e instanceof WeakSet?e.add(r):e.set(r,t)},Bo=(r,e,t)=>(jo(r,e,"access private method"),t),zt,Fr;let Ue=class extends w{constructor(){super(...arguments),Fo(this,zt),this.value=""}connectedCallback(){super.connectedCallback(),Bo(this,zt,Fr).call(this)}async localize(r,e){return c.localize(r,void 0,e)}render(){return this.value?l`${Or(this.value)}`:l`<slot></slot>`}};zt=new WeakSet,Fr=async function(){try{this.value=await this.localize(this.key)}catch(r){console.error("Failed to localize key:",this.key,r)}},At([d({type:String})],Ue.prototype,"key",2),At([b()],Ue.prototype,"value",2),Ue=At([y("umb-localize")],Ue)})(uui);
//# sourceMappingURL=/umbraco/login/login.iife.js.map;