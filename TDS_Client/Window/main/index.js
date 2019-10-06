"use strict";function o(n){return a(n)||t(n)||e()}function e(){throw new TypeError("Invalid attempt to spread non-iterable instance")}function t(n){if(Symbol.iterator in Object(n)||"[object Arguments]"===Object.prototype.toString.call(n))return Array.from(n)}function a(n){if(Array.isArray(n)){for(var e=0,t=new Array(n.length);e<n.length;e++)t[e]=n[e];return t}}var n=$("#bloodscreen"),r,c=$("#kill_messages_box"),i=$("#voice_chat_player_names_box"),l=9,s=$("#orders"),u,p=$("#audio_hit"),g=[p,p.clone(!0),p.clone(!0),p.clone(!0),p.clone(!0),p.clone(!0),p.clone(!0),p.clone(!0),p.clone(!0),p.clone(!0)],h=0,d=g.length,m=$("#audio_bombTick"),f=[m,m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0),m.clone(!0)],b=0,y=f.length,v,w=.05,_=!1,x=[];function playSound(n){$("#audio_"+n).trigger("play").prop("volume",w)}function playHitsound(){g[h++].trigger("play").prop("volume",w),h==d&&(h=0)}function O(){f[b++].trigger("play").prop("volume",w),b==y&&(b=0)}function playKillstreakSound(n){_?x.push(n):($("#audio_"+soundname).trigger("play").prop("volume",w),_=!0)}function onKillstreakSoundEnded(){var n;(_=!1,0!=x.length)&&playKillstreakSound(x.shift())}function startBombTickSound(n,e){O();var t,o=3e3-2950*(e/n);null!=v&&clearTimeout(v),v=setTimeout(function(){return startBombTickSound(n,e+o)},o)}function stopBombTickSound(){null!=v&&(clearTimeout(v),v=null)}function showBloodscreen(){u?(clearTimeout(u),r.style.animation="none",r.offsetHeight,r.style.animation="BloodscreenAnim 2.5s"):n.css({animation:"BloodscreenAnim 2.5s","animation-fill-mode":"forwards"}),u=setTimeout(function(){u=null,n.css({animation:""})},2500)}function k(n){var e='<span style="color: white;">',t=n;return-1!==n.indexOf("~")&&(t=t.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),e+t+"</span>"}function A(n){n.remove()}function addKillMessage(n){var e=$("<text>"+k(n)+"<br></text>");c.append(e),e.delay(11e3).fadeOut(4e3,e.remove)}function C(n){n?s.show(1e3):s.hide(1e3)}function addPlayerTalking(n){var e,t,o=$("<div id='"+("voice-chat-"+n)+"'>"+"<img src='../pic/speaker.png'/>"+"<span>"+n+"</span></div>");i.append(o)}function removePlayerTalking(n){var e="voice-chat-"+n,t=i.find("#"+e);t&&t.remove()}$(document).ready(function(){r=n.get()[0]});var S={amountentries:[0,0],maxentries:40,active:!0,inputshowing:!1,maininput:$("#main-input"),bodies:[$("#normal-chat-body"),$("#dirty-chat-body")],chatends:["$normal$","$dirty$"],chosentab:null,chosenchatbody:0,myname:null,globalsaykeycode:"Z"===String.fromCharCode(90)?90:Y,playernames:[],autocompleteon:!1},T=[[/#r#/g,"rgb(222, 50, 50)"],[/#b#/g,"rgb(92, 180, 227)"],[/#g#/g,"rgb(113, 202, 113)"],[/#y#/g,"rgb(238, 198, 80)"],[/#p#/g,"rgb(131, 101, 224)"],[/#q#/g,"rgb(226, 79, 128)"],[/#o#/g,"rgb(253, 132, 85)"],[/#s#/g,"rgb(220, 220, 220)"],[/#w#/g,"white"],[/#dr#/g,"rgb(169, 25, 25)"]];function B(){var n=0<arguments.length&&void 0!==arguments[0]?arguments[0]:null;(n=null===n?N():n).finish().animate({scrollTop:n.prop("scrollHeight")},"slow")}function enableChatInput(n){var e=1<arguments.length&&void 0!==arguments[1]?arguments[1]:"";!S.active&&n||n!==S.inputshowing&&(n?(S.maininput.show(0,function(){S.maininput.focus()}),S.maininput.val(e)):(S.maininput.hide(),S.maininput.val("")),S.inputshowing=n)}var chatAPI={};function D(n){for(var e=n.indexOf("!{");-1!=e;){var t=n.indexOf("}",e+2);if(-1==t)break;var o=n.substring(e,t+1),a=o.substring(2,o.length-1).split("|"),r=void 0;1==a.length?r=a[0]:a.length<3?r="rgb(".concat(0 in a?a[0]:0,", ").concat(1 in a?a[1]:0,", 0)"):3==a.length?r="rgb(".concat(a[0],", ").concat(a[1],", ").concat(a[2],")"):4==a.length&&(r="rgba(".concat(a[0],", ").concat(a[1],", ").concat(a[2],", ").concat(a[3],")"));var c="</span><span style='color: "+r+";'>";e=(n=n.replace(o,c)).indexOf("!{",e+c.length)}return n}function I(n,e){var t="";e&&(t='<span style="background-color: rgba(255,178,102,0.6);">'),t+='<span style="color: white;">';var o=n;if(-1!==n.indexOf("#")){for(var a=0;a<T.length;++a)o=o.replace(T[a][0],"</span><span style='color: "+T[a][1]+";'>");o=o.replace(/#n#/g,"<br>")}return o=D(o),e&&(o+="</span>"),t+o+"</span>"}function R(n){if(null===S.myname)return!1;var e=n.indexOf("@");if(-1===e)return!1;var t=n.indexOf(":",e+1),o;return-1!==t&&n.substring(e+1,t)===S.myname}function j(n,e,t){e.append(n),++S.amountentries[t]>=S.maxentries&&(--S.amountentries[t],e.find("text:first").remove()),B(e)}function J(n){for(var e=R(n),t=0;t<S.chatends.length;++t)if(n.endsWith(S.chatends[t])){n=n.slice(0,-S.chatends[t].length);var o=N(t),a=I(n,e),r;return void j($("<text>"+a+"</text>"),o,t)}for(var c=I(n,e),i=0;i<S.bodies.length;++i){var l=N(i),s;j($("<text>"+c+"</text>"),l,i)}}function N(){var n=0<arguments.length&&void 0!==arguments[0]?arguments[0]:-1,e=-1===n?S.chosenchatbody:n;return S.bodies[e]}function loadUserName(n){S.myname=n,S.playernames.push(n)}function loadNamesForChat(n){var e;S.playernames.length=0;var t=JSON.parse(n);(e=S.playernames).push.apply(e,o(t))}function addNameForChat(n){S.playernames.push(n)}function removeNameForChat(n){var e=S.playernames.indexOf(n);-1!=e&&S.playernames.splice(e,1)}function H(n){return void 0===n||null==n||n.replace(/\s/g,"").length<1}chatAPI.push=J,chatAPI.clear=function(){N().html("")},chatAPI.activate=function(n){S.active=n},chatAPI.show=function(n){n?(N().show(),$("#chat_choice").show()):(N().hide(),$("#chat_choice").hide()),S.active=n},S.maininput.blur(function(){enableChatInput(!1),mp.trigger("CloseChat_Browser")}),$(document).ready(function(){addAutocomplete(S.maininput,S.playernames,function(){return!(S.autocompleteon=!0)},function(){setTimeout(function(){S.autocompleteon=!1},500)}),$("body").keydown(function(n){if(13===n.which&&S.inputshowing&&!S.autocompleteon){n.preventDefault();var e=S.maininput.val();H(e)?mp.trigger("CloseChat_Browser"):"/"===(e=e.replace(/\\/g,"\\\\").replace(/\"/g,'\\"'))[0]?0<(e=e.substr(1)).length&&("test"==e&&J(JSON.stringify(S.playernames)),mp.trigger("CommandUsed_Browser",e)):mp.trigger("ChatUsed_Browser",e,1==S.chosenchatbody)}}),S.chosentab=$("#chat_choice div[data-chatID=0]"),S.chosentab.css("background","#04074e"),$("#chat_choice div").click(function(){S.chosenchatbody!==$(this).attr("data-chatID")&&(S.chosentab.css("background","#A9A9A9"),S.bodies[S.chosenchatbody].hide(400),S.chosentab=$(this),S.chosentab.css("background","#04074e"),S.chosenchatbody=S.chosentab.attr("data-chatID"),S.bodies[S.chosenchatbody].show(400))}),mp.trigger("ChatLoaded_Browser")});var U=$("#round_end_reason"),q=$("#round_end_reason_container"),E={},L,M=new SimpleStarRating($("#map_rating_stars")[0],function(n){L in E&&E[L]===n||(E[L]=n,mp.trigger("SendMapRating_Browser",L,n),M.disable())});function showRoundEndReason(n,e){U.html(n),q.css("display","table"),(L=e)in E?M.setCurrentRating(E[e]):M.setCurrentRating(0),M.enable()}function hideRoundEndReason(){q.hide()}function loadMyMapRatings(n){E=JSON.parse(n)}