"use strict";var e=$("#bloodscreen"),t=$("#kill_messages_box"),n=9,o=$("#orders"),a,c=$("#audio_hit"),r=[c,c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0)],i=0,l=r.length,s=$("#audio_bombTick"),u=[s,s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0),s.clone(!0)],g=0,p=u.length,h,d=!1,m=[];function playSound(n){$("#audio_"+n).trigger("play").volume=.05}function playHitsound(){r[i++].trigger("play").volume=.05,i==l&&(i=0)}function f(){u[g++].trigger("play").volume=.05,g==p&&(g=0)}function playKillstreakSound(n){d?m.push(n):($("#audio_"+soundname).trigger("play").volume=.05,d=!0)}function onKillstreakSoundEnded(){var n;(d=!1,0!=m.length)&&playKillstreakSound(m.shift())}function startBombTickSound(n,e){f();var t,o=3e3-2950*(e/n);null!=h&&clearTimeout(h),h=setTimeout(function(){return startBombTickSound(n,e+o)},o)}function stopBombTickSound(){null!=h&&(clearTimeout(h),h=null)}function showBloodscreen(){if(a){clearTimeout(a);var n=e.get()[0];n.style.animation="none",n.offsetHeight,n.style.animation="BloodscreenAnim 2.5s"}else e.css({display:"block",animation:"BloodscreenAnim 2.5s"});a=setTimeout(function(){e.css("display","none"),a=null},2500)}function b(n){var e='<span style="color: white;">',t=n;return-1!==n.indexOf("~")&&(t=t.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),e+t+"</span>"}function v(n){n.remove()}function addKillMessage(n){var e=$("<text>"+b(n)+"<br></text>");t.append(e),e.delay(11e3).fadeOut(4e3,e.remove)}function y(n){n?o.show(1e3):o.hide(1e3)}var w={amountentries:[0,0],maxentries:40,active:!0,inputshowing:!1,maininput:$("#main-input"),bodies:[$("#normal-chat-body"),$("#dirty-chat-body")],chatends:["$normal$","$dirty$"],chosentab:null,chosenchatbody:0,myname:null,globalsaykeycode:"Z"===String.fromCharCode(90)?90:Y,playernames:[],autocompleteon:!1},_=[[/#r#/g,"rgb(222, 50, 50)"],[/#b#/g,"rgb(92, 180, 227)"],[/#g#/g,"rgb(113, 202, 113)"],[/#y#/g,"rgb(238, 198, 80)"],[/#p#/g,"rgb(131, 101, 224)"],[/#q#/g,"rgb(226, 79, 128)"],[/#o#/g,"rgb(253, 132, 85)"],[/#s#/g,"rgb(220, 220, 220)"],[/#w#/g,"white"],[/#dr#/g,"rgb(169, 25, 25)"]];function x(){var n=0<arguments.length&&void 0!==arguments[0]?arguments[0]:null;(n=null===n?A():n).finish().animate({scrollTop:n.prop("scrollHeight")},"slow")}function enableChatInput(n){var e=1<arguments.length&&void 0!==arguments[1]?arguments[1]:"";!w.active&&n||n!==w.inputshowing&&(mp.invoke("focus",n),n?(w.maininput.fadeIn(),w.maininput.val(e),setTimeout(function(){w.maininput.focus()},100)):(w.maininput.hide(),w.maininput.val("")),w.inputshowing=n,mp.trigger("ChatInputToggled_Browser",n))}var chatAPI={};function k(n){for(var e=n.indexOf("!{");-1!=e;){var t=n.indexOf("}",e+2);if(-1==t)break;var o=n.substring(e,t+1),a=o.substring(2,o.length-1).split("|"),c=void 0;1==a.length?c=a[0]:a.length<3?c="rgb(".concat(0 in a?a[0]:0,", ").concat(1 in a?a[1]:0,", 0)"):3==a.length?c="rgb(".concat(a[0],", ").concat(a[1],", ").concat(a[2],")"):4==a.length&&(c="rgba(".concat(a[0],", ").concat(a[1],", ").concat(a[2],", ").concat(a[3],")"));var r="</span><span style='color: "+c+";'>";e=(n=n.replace(o,r)).indexOf("!{",e+r.length)}return n}function O(n,e){var t="";e&&(t='<span style="background-color: rgba(255,178,102,0.6);">'),t+='<span style="color: white;">';var o=n;if(-1!==n.indexOf("#")){for(var a=0;a<_.length;++a)o=o.replace(_[a][0],"</span><span style='color: "+_[a][1]+";'>");o=o.replace(/#n#/g,"<br>")}return o=k(o),e&&(o+="</span>"),t+o+"</span>"}function C(n){if(null===w.myname)return!1;var e=n.indexOf("@");if(-1===e)return!1;var t=n.indexOf(":",e+1),o;return-1!==t&&n.substring(e+1,t)===w.myname}function T(n,e,t){e.append(n),++w.amountentries[t]>=w.maxentries&&(--w.amountentries[t],e.find("text:first").remove()),x(e)}function B(n){for(var e=C(n),t=0;t<w.chatends.length;++t)if(n.endsWith(w.chatends[t])){n=n.slice(0,-w.chatends[t].length);var o=A(t),a=O(n,e),c;return void T($("<text>"+a+"</text>"),o,t)}for(var r=O(n,e),i=0;i<w.bodies.length;++i){var l=A(i),s;T($("<text>"+r+"</text>"),l,i)}}function A(){var n=0<arguments.length&&void 0!==arguments[0]?arguments[0]:-1,e=-1===n?w.chosenchatbody:n;return w.bodies[e]}function loadUserName(n){w.myname=n,w.playernames.push(n)}function loadNamesForChat(n){w.playernames=JSON.parse(n)}function addNameForChat(n){w.playernames.push(n)}function removeNameForChat(n){var e=w.playernames.indexOf(n);-1!=e&&w.playernames.splice(e,1)}function S(n){return void 0===n||null==n||n.replace(/\s/g,"").length<1}chatAPI.push=B,chatAPI.clear=function(){A().html("")},chatAPI.activate=function(n){w.active=n},chatAPI.show=function(n){n?(A().show(),$("#chat_choice").show()):(A().hide(),$("#chat_choice").hide()),w.active=n},$(document).ready(function(){addAutocomplete(w.maininput,w.playernames,function(){return!(w.autocompleteon=!0)},function(){setTimeout(function(){w.autocompleteon=!1},500)}),$("body").keydown(function(n){if(13===n.which&&w.inputshowing&&!w.autocompleteon){n.preventDefault();var e=w.maininput.val();S(e)?mp.trigger("CloseChat_Browser"):"/"===(e=e.replace(/\\/g,"\\\\").replace(/\"/g,'\\"'))[0]?0<(e=e.substr(1)).length&&mp.trigger("CommandUsed_Browser",e):mp.trigger("ChatUsed_Browser",e,1==w.chosenchatbody)}}),w.chosentab=$("#chat_choice div[data-chatID=0]"),w.chosentab.css("background","#04074e"),$("#chat_choice div").click(function(){w.chosenchatbody!==$(this).attr("data-chatID")&&(w.chosentab.css("background","#A9A9A9"),w.bodies[w.chosenchatbody].hide(400),w.chosentab=$(this),w.chosentab.css("background","#04074e"),w.chosenchatbody=w.chosentab.attr("data-chatID"),w.bodies[w.chosenchatbody].show(400))}),mp.trigger("ChatLoaded_Browser")});var I=$("#round_end_reason"),D=$("#round_end_reason_container"),R={},H,J=new SimpleStarRating($("#map_rating_stars")[0],function(n){H in R&&R[H]===n||(R[H]=n,mp.trigger("SendMapRating_Browser",H,n),J.disable())});function showRoundEndReason(n,e){I.html(n),D.css("display","table"),(H=e)in R?J.setCurrentRating(R[e]):J.setCurrentRating(0),J.enable()}function hideRoundEndReason(){D.hide()}function loadMyMapRatings(n){R=JSON.parse(n)}