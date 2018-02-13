"use strict";var e=$("#money"),t=$("#bloodscreen"),n=$("#kill_messages_box"),a="ENGLISH",o=$("#orders");function setMoney(t){e.text("$"+t)}function playSound(e){$("#audio_"+e).trigger("play").volume=.05}function showBloodscreen(){t.stop().show(0).hide(2500)}function i(e){var t='<span style="color: white;">',n=e;return-1!==e.indexOf("~")&&(n=n.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),t+n+"</span>"}function s(e){e.remove()}function addKillMessage(e){var t=$("<text>"+i(e)+"<br></text>");n.append(t),t.delay(11e3).fadeOut(4e3,t.remove)}function loadOrderNames(e){o.empty();for(var t=JSON.parse(e),n=0;n<t.length&&n<9;++n)o.append($("<div>"+(n+1)+". "+t[n]+"</div>"))}var r=$("#mapmenu"),c=$("#tabs-1"),l=$("#tabs-2"),d=$("#tabs-3"),p=$(O),h=$("#map_info"),u=$("#choose_map_button"),v=$("#add_map_to_favourites"),f=$("#mapvoting"),g=void 0,m="",b=!1,y=[],_=[],w="",x=!1,C=!1,k=!1,O="#tabs-4",D=!0;$("#tabs").tabs({collapsible:!0,heightStyle:"fill"}),$(".tab_list").each(function(e,t){$(this).selectable({selected:function e(t,n){$(".ui-selected").removeClass("ui-selected"),$(n.selected).addClass("ui-selected");var o=n.selected.innerHTML;J(o);for(var i=0;i<g.length;++i)if(o===g[i].Name)return m=o,C||(u.show(0),h.show(0),v.show(0),C=!0),void h.html(g[i].Description[a])},autoRefresh:!1})}),u.click(function(e){e.preventDefault(),x||""!==m&&(w=m,mp.trigger("onMapMenuVote",w),q())}),v.click(function(e){if(e.preventDefault(),""!==m){var t=_.indexOf(m);-1===t?(_.push(m),v.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),mp.trigger("onClientToggleMapFavourite",m,!0),d.append($("<div>"+m+"</div>"))):(_.splice(t,1),v.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),mp.trigger("onClientToggleMapFavourite",m,!1),d.children("div:contains('"+m+"')").remove()),d.selectable("refresh")}});function openMapMenu(e,t){r.show(1e3),b=!0,a=e,m="",w="",c.empty(),l.empty(),d.empty(),p.empty(),g=JSON.parse(t);for(var n=0;n<g.length;++n){var o=$("<div>"+g[n].Name+"</div>");0===g[n].Type?c.append(o):l.append(o),-1!==_.indexOf(g[n].Name)&&d.append(o.clone())}for(var i=0;i<y.length;++i)p.append($("<div>"+y[i].name+"</div>"));c.selectable("refresh"),l.selectable("refresh"),d.selectable("refresh"),p.selectable("refresh")}function closeMapMenu(){b=!1,r.hide(500),C&&(u.hide(500),h.hide(500),v.hide(500),C=!1)}function M(e,t){return e.votes>t.votes?-1:1}function N(){y.sort(M)}function T(e){y.push({name:e,votes:1});var t=$("<div>"+y.length+". "+e+" (1)</div>");f.append(t),b&&(p.append($("<div>"+e+"</div>")),p.selectable("refresh")),w===e&&(t.addClass("mapvoting_selected"),w="")}function S(){for(var e=f.children(),t=0;t<e.length;++t)t in y?e.eq(t).text(t+1+". "+mapname+" ("+votes[t].votings+")"):y.splice(t)}function I(e){for(var t=0;t<y.length;++t)if(y[t].name===e)return--y[t].votes<=0&&(y.splice(t,1),f.children().eq(t).remove(),b&&$(O+" div:contains("+e+")")),void S()}function addVoteToMapVoting(e,t){w===e&&$(".mapvoting_selected").removeClass("mapvoting_selected"),void 0!==t&&I(t);for(var n=!1,a=0;a<y.length&&!n;++a)if(y[a].name===e){++y[a].votes;var o=f.children().eq(a);o.text(a+1+". "+e+"("+y[a].votes+")"),w===e&&(o.addClass("mapvoting_selected"),w=""),n=!0}n||T(e),N()}function loadMapVotings(e){var t=JSON.parse(mapsvotesjson);y=[];for(var n in t)y.push({name:n,votes:t[n]});N()}function clearMapVotings(){y=[],f.empty(),b&&(p.empty(),p.selectable("refresh"))}function q(){x=!0,u.disabled=!0,setTimeout(function(){x=!1,u.disabled=!1},1500)}$("body").keydown(function(e){var t=e.which;if(D&&t>=97&&t<=105){if(x)return;e.preventDefault();var n=9-(105-t)-1;y.length>n&&(w=y[n].name,mp.trigger("onMapMenuVote",w),q())}});function J(e){-1!==_.indexOf(e)?k||(v.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),k=!0):k&&(v.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),k=!1)}function loadFavouriteMaps(e){_=JSON.parse(e)}function toggleCanVoteForMapWithNumpad(e){D=e,e?(o.hide(300),f.show(300)):(o.show(300),f.hide(300))}var A={amountentries:[0,0],maxentries:40,active:!0,inputshowing:!1,maininput:$("#main-input"),bodies:[$("#normal-chat-body"),$("#dirty-chat-body")],chatends:["$normal$","$dirty$"],chosentab:null,chosenchatbody:0,myname:null,globalsaykeycode:"Z"===String.fromCharCode(90)?90:Y},H=[[/#r#/g,"rgb(222, 50, 50)"],[/#b#/g,"rgb(92, 180, 227)"],[/#g#/g,"rgb(113, 202, 113)"],[/#y#/g,"rgb(238, 198, 80)"],[/#p#/g,"rgb(131, 101, 224)"],[/#q#/g,"rgb(226, 79, 128)"],[/#o#/g,"rgb(253, 132, 85)"],[/#s#/g,"rgb(220, 220, 220)"],[/#w#/g,"white"],[/#dr#/g,"rgb(169, 25, 25)"]];function L(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:null;(e=null===e?G():e).animate({scrollTop:e.prop("scrollHeight")},"slow")}function F(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"";!A.active&&e||e!==A.inputshowing&&(mp.invoke("focus",e),e?(A.maininput.fadeIn(),A.maininput.val(t),setTimeout(function(){A.maininput.focus()},100)):(A.maininput.hide(),A.maininput.val("")),A.inputshowing=e,mp.trigger("onChatInputToggle",e))}var chatAPI={};chatAPI.push=E,chatAPI.clear=function(){G().html("")},chatAPI.activate=function(e){F(e),A.active=e},chatAPI.show=function(e){e?(G().show(),$("#chat_choice").show()):(G().hide(),$("#chat_choice").hide()),!e&&A.inputshowing&&F(!1),A.active=e};function i(e,t){var n="";t&&(n='<span style="background-color: rgba(255,178,102,0.5);">'),n+='<span style="color: white;">';var a=e;if(-1!==e.indexOf("#")){for(var o=0;o<H.length;++o)a=a.replace(H[o][0],"</span><span style='color: "+H[o][1]+";'>");a=a.replace(/#n#/g,"<br>")}return t&&(a+="</span>"),n+a+"</span>"}function V(e){if(null===A.myname)return!1;var t=e.indexOf("@");if(-1===t)return!1;var n=e.indexOf(":",t+1);if(-1===n)return!1;var a;return e.substring(t+1,n)===A.myname}function j(e,t,n){t.append(e),++A.amountentries[n]>=A.maxentries&&(--A.amountentries[n],t.find("text:first").remove()),L(t)}function E(e){for(var t=V(e),n=0;n<A.chatends.length;++n)if(e.endsWith(A.chatends[n])){e=e.slice(0,-A.chatends[n].length);var a=G(n),o=i(e,t),s;return void j($("<text>"+o+"</text>"),a,n)}for(var r=i(e,t),c=0;c<A.bodies.length;++c){var l=G(c),d;j($("<text>"+r+"</text>"),l,c)}}function G(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:-1,t=-1===e?A.chosenchatbody:e;return A.bodies[t]}function loadUserName(e){A.myname=e}$(document).ready(function(){$("body").keydown(function(e){if(84===e.which&&!A.inputshowing&&A.active)e.preventDefault(),F(!0);else if(e.which===A.globalsaykeycode&&!A.inputshowing&&A.active)e.preventDefault(),F(!0,"/globalsay ");else if(13===e.which&&A.inputshowing){e.preventDefault();var t=A.maininput.val();t&&("/"===t[0]?(t=t.substr(1)).length>0&&mp.invoke("command",t):mp.invoke("chatMessage",t+A.chatends[A.chosenchatbody])),F(!1)}}),A.chosentab=$("#chat_choice div[data-chatID=0]"),A.chosentab.css("background","#04074e"),$("#chat_choice div").click(function(){A.chosenchatbody!==$(this).attr("data-chatID")&&(A.chosentab.css("background","#A9A9A9"),A.bodies[A.chosenchatbody].hide(400),A.chosentab=$(this),A.chosentab.css("background","#04074e"),A.chosenchatbody=A.chosentab.attr("data-chatID"),A.bodies[A.chosenchatbody].show(400))}),mp.trigger("onChatLoad")});