"use strict";var n=$("#money"),e=$("#bloodscreen"),t=$("#kill_messages_box"),i="ENGLISH",a=$("#orders"),o=$("#audio_hit"),s=[o,o.clone(!0),o.clone(!0),o.clone(!0),o.clone(!0),o.clone(!0),o.clone(!0),o.clone(!0),o.clone(!0),o.clone(!0)],r=0,c=s.length;function setMoney(e){n.text("$"+e)}function playSound(e){$("#audio_"+e).trigger("play").volume=.05}function playHitsound(){s[r++].trigger("play").volume=.05,r==c&&(r=0)}function showBloodscreen(){e.stop().css("opacity",1),e.css("display","block"),e.fadeOut(2500)}function d(e){var n='<span style="color: white;">',t=e;return-1!==e.indexOf("~")&&(t=t.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),n+t+"</span>"}function l(e){e.remove()}function addKillMessage(e){var n=$("<text>"+d(e)+"<br></text>");t.append(n),n.delay(11e3).fadeOut(4e3,n.remove)}function loadOrderNames(e){a.empty();for(var n=JSON.parse(e),t=0;t<n.length&&t<9;++t)a.append($("<div>"+(t+1)+". "+n[t]+"</div>"))}var p=$("#mapmenu"),u=$("#tabs-1"),h=$("#tabs-2"),g=$("#tabs-3"),f=$(M),v=$("#map_info"),m=$("#choose_map_button"),b=$("#add_map_to_favourites"),y=$("#mapvoting"),_,w="",x=!1,C=[],O=[],k="",S=!1,D=!1,N=!1,M="#tabs-4",T=!0;function openMapMenu(e,n){p.show(1e3),x=!0,i=e,k=w="",u.empty(),h.empty(),g.empty(),f.empty(),_=JSON.parse(n);for(var t=0;t<_.length;++t){var a=$("<div>"+_[t].Name+"</div>");0===_[t].Type?u.append(a):h.append(a),-1!==O.indexOf(_[t].Name)&&g.append(a.clone())}for(var o=0;o<C.length;++o)f.append($("<div>"+C[o].name+"</div>"));u.selectable("refresh"),h.selectable("refresh"),g.selectable("refresh"),f.selectable("refresh")}function closeMapMenu(){x=!1,p.hide(500),D&&(m.hide(500),v.hide(500),b.hide(500),D=!1)}function I(e,n){return e.votes>n.votes?-1:1}function J(){C.sort(I)}function R(e){C.push({name:e,votes:1});var n=$("<div>"+C.length+". "+e+" (1)</div>");y.append(n),x&&(f.append($("<div>"+e+"</div>")),f.selectable("refresh")),k===e&&(n.addClass("mapvoting_selected"),k="")}function q(){for(var e=y.children(),n=0;n<e.length;++n)n in C?e.eq(n).text(n+1+". "+mapname+" ("+votes[n].votings+")"):C.splice(n)}function A(e){for(var n=0;n<C.length;++n)if(C[n].name===e)return--C[n].votes<=0&&(C.splice(n,1),y.children().eq(n).remove(),x&&$(M+" div:contains("+e+")")),void q()}function addVoteToMapVoting(e,n){k===e&&$(".mapvoting_selected").removeClass("mapvoting_selected"),void 0!==n&&A(n);for(var t=!1,a=0;a<C.length&&!t;++a)if(C[a].name===e){++C[a].votes;var o=y.children().eq(a);o.text(a+1+". "+e+"("+C[a].votes+")"),k===e&&(o.addClass("mapvoting_selected"),k=""),t=!0}t||R(e),J()}function loadMapVotings(e){var n=JSON.parse(mapsvotesjson);for(var t in C=[],n)C.push({name:t,votes:n[t]});J()}function clearMapVotings(){C=[],y.empty(),x&&(f.empty(),f.selectable("refresh"))}function B(){S=!0,m.disabled=!0,setTimeout(function(){S=!1,m.disabled=!1},1500)}function H(e){-1!==O.indexOf(e)?N||(b.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),N=!0):N&&(b.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),N=!1)}function loadFavouriteMaps(e){O=JSON.parse(e)}function toggleCanVoteForMapWithNumpad(e){(T=e)?(a.hide(300),y.show(300)):(a.show(300),y.hide(300))}$("#tabs").tabs({collapsible:!0,heightStyle:"fill"}),$(".tab_list").each(function(e,n){$(this).selectable({selected:function e(n,t){$(".ui-selected").removeClass("ui-selected"),$(t.selected).addClass("ui-selected");var a=t.selected.innerHTML;H(a);for(var o=0;o<_.length;++o)if(a===_[o].Name)return w=a,D||(m.show(0),v.show(0),b.show(0),D=!0),void v.html(_[o].Description[i])},autoRefresh:!1})}),m.click(function(e){e.preventDefault(),S||""!==w&&(k=w,mp.trigger("onMapMenuVote",k),B())}),b.click(function(e){if(e.preventDefault(),""!==w){var n=O.indexOf(w);-1===n?(O.push(w),b.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),mp.trigger("onClientToggleMapFavourite",w,!0),g.append($("<div>"+w+"</div>"))):(O.splice(n,1),b.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),mp.trigger("onClientToggleMapFavourite",w,!1),g.children("div:contains('"+w+"')").remove()),g.selectable("refresh")}}),$("body").keydown(function(e){var n=e.which;if(T&&97<=n&&n<=105){if(S)return;e.preventDefault();var t=9-(105-n)-1;C.length>t&&(k=C[t].name,mp.trigger("onMapMenuVote",k),B())}});var L={amountentries:[0,0],maxentries:40,active:!0,inputshowing:!1,maininput:$("#main-input"),bodies:[$("#normal-chat-body"),$("#dirty-chat-body")],chatends:["$normal$","$dirty$"],chosentab:null,chosenchatbody:0,myname:null,globalsaykeycode:"Z"===String.fromCharCode(90)?90:Y,playernames:[],autocompleteon:!1},F=[[/#r#/g,"rgb(222, 50, 50)"],[/#b#/g,"rgb(92, 180, 227)"],[/#g#/g,"rgb(113, 202, 113)"],[/#y#/g,"rgb(238, 198, 80)"],[/#p#/g,"rgb(131, 101, 224)"],[/#q#/g,"rgb(226, 79, 128)"],[/#o#/g,"rgb(253, 132, 85)"],[/#s#/g,"rgb(220, 220, 220)"],[/#w#/g,"white"],[/#dr#/g,"rgb(169, 25, 25)"]];function V(){var e=0<arguments.length&&void 0!==arguments[0]?arguments[0]:null;(e=null===e?Z():e).finish().animate({scrollTop:e.prop("scrollHeight")},"slow")}function j(e){var n=1<arguments.length&&void 0!==arguments[1]?arguments[1]:"";!L.active&&e||e!==L.inputshowing&&(mp.invoke("focus",e),e?(L.maininput.fadeIn(),L.maininput.val(n),setTimeout(function(){L.maininput.focus()},100)):(L.maininput.hide(),L.maininput.val("")),L.inputshowing=e,mp.trigger("ChatInputToggled_Browser",e))}var chatAPI={};function E(e){for(var n=e.indexOf("!{");-1!=n;){var t=e.indexOf("}",n+2);if(-1==t)break;var a=e.substring(n,t+1),o=a.substring(2,a.length-1).split("|"),i=void 0;1==o.length?i=o[0]:o.length<3?i="rgb(".concat(0 in o?o[0]:0,", ").concat(1 in o?o[1]:0,", 0)"):3==o.length?i="rgb(".concat(o[0],", ").concat(o[1],", ").concat(o[2],")"):4==o.length&&(i="rgba(".concat(o[0],", ").concat(o[1],", ").concat(o[2],", ").concat(o[3],")"));var s="</span><span style='color: "+i+";'>";n=(e=e.replace(a,s)).indexOf("!{",n+s.length)}return e}function d(e,n){var t="";n&&(t='<span style="background-color: rgba(255,178,102,0.6);">'),t+='<span style="color: white;">';var a=e;if(-1!==e.indexOf("#")){for(var o=0;o<F.length;++o)a=a.replace(F[o][0],"</span><span style='color: "+F[o][1]+";'>");a=a.replace(/#n#/g,"<br>")}return a=E(a),n&&(a+="</span>"),t+a+"</span>"}function G(e){if(null===L.myname)return!1;var n=e.indexOf("@");if(-1===n)return!1;var t=e.indexOf(":",n+1),a;return-1!==t&&e.substring(n+1,t)===L.myname}function U(e,n,t){n.append(e),++L.amountentries[t]>=L.maxentries&&(--L.amountentries[t],n.find("text:first").remove()),V(n)}function W(e){for(var n=G(e),t=0;t<L.chatends.length;++t)if(e.endsWith(L.chatends[t])){e=e.slice(0,-L.chatends[t].length);var a=Z(t),o=d(e,n),i;return void U($("<text>"+o+"</text>"),a,t)}for(var s=d(e,n),r=0;r<L.bodies.length;++r){var c=Z(r),l;U($("<text>"+s+"</text>"),c,r)}}function Z(){var e=0<arguments.length&&void 0!==arguments[0]?arguments[0]:-1,n=-1===e?L.chosenchatbody:e;return L.bodies[n]}function loadUserName(e){L.myname=e,L.playernames.push(e)}function loadNamesForChat(e){L.playernames=JSON.parse(e)}function addNameForChat(e){L.playernames.push(e)}function removeNameForChat(e){var n=L.playernames.indexOf(e);-1!=n&&L.playernames.splice(n,1)}chatAPI.push=W,chatAPI.clear=function(){Z().html("")},chatAPI.activate=function(e){j(e),L.active=e},chatAPI.show=function(e){e?(Z().show(),$("#chat_choice").show()):(Z().hide(),$("#chat_choice").hide()),!e&&L.inputshowing&&j(!1),L.active=e},$(document).ready(function(){addAutocomplete(L.maininput,L.playernames,function(){return!(L.autocompleteon=!0)},function(){setTimeout(function(){L.autocompleteon=!1},500)}),$("body").keydown(function(e){if(84===e.which&&!L.inputshowing&&L.active)e.preventDefault(),j(!0);else if(e.which===L.globalsaykeycode&&!L.inputshowing&&L.active)e.preventDefault(),j(!0,"/globalsay ");else if(13===e.which&&L.inputshowing&&!L.autocompleteon){e.preventDefault();var n=L.maininput.val();n&&("/"===(n=n.replace(/\\/g,"\\\\").replace(/\"/g,'\\"'))[0]?0<(n=n.substr(1)).length&&mp.trigger("CommandUsed_Browser",n):mp.invoke("chatMessage",n+L.chatends[L.chosenchatbody])),j(!1)}}),L.chosentab=$("#chat_choice div[data-chatID=0]"),L.chosentab.css("background","#04074e"),$("#chat_choice div").click(function(){L.chosenchatbody!==$(this).attr("data-chatID")&&(L.chosentab.css("background","#A9A9A9"),L.bodies[L.chosenchatbody].hide(400),L.chosentab=$(this),L.chosentab.css("background","#04074e"),L.chosenchatbody=L.chosentab.attr("data-chatID"),L.bodies[L.chosenchatbody].show(400))}),mp.trigger("ChatLoaded_Browser")});var z=$("#round_end_reason"),K=$("#round_end_reason_container"),P={},Q,X=new SimpleStarRating($("#map_rating_stars")[0],function(e){Q in P&&P[Q]===e||(P[Q]=e,mp.trigger("sendMapRating",Q,e),X.disable())});function showRoundEndReason(e,n){z.html(e),K.css("display","table"),(Q=n)in P?X.setCurrentRating(P[n]):X.setCurrentRating(0),X.enable()}function hideRoundEndReason(){K.hide()}function loadMyMapRatings(e){P=JSON.parse(e)}