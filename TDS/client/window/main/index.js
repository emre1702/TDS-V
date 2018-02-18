"use strict";var e=$("#money"),t=$("#bloodscreen"),n=$("#kill_messages_box"),a="ENGLISH",o=$("#orders"),i=$("#round_end_reason");function setMoney(t){e.text("$"+t)}function playSound(e){$("#audio_"+e).trigger("play").volume=.05}function showBloodscreen(){t.stop().show(0).hide(2500)}function s(e){var t='<span style="color: white;">',n=e;return-1!==e.indexOf("~")&&(n=n.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),t+n+"</span>"}function r(e){e.remove()}function addKillMessage(e){var t=$("<text>"+s(e)+"<br></text>");n.append(t),t.delay(11e3).fadeOut(4e3,t.remove)}function loadOrderNames(e){o.empty();for(var t=JSON.parse(e),n=0;n<t.length&&n<9;++n)o.append($("<div>"+(n+1)+". "+t[n]+"</div>"))}function showRoundEndReason(e){i.html(e).css("display","table-cell")}function hideRoundEndReason(){i.hide()}var c=$("#mapmenu"),l=$("#tabs-1"),d=$("#tabs-2"),p=$("#tabs-3"),u=$(D),h=$("#map_info"),v=$("#choose_map_button"),f=$("#add_map_to_favourites"),m=$("#mapvoting"),g=void 0,b="",y=!1,_=[],w=[],x="",C=!1,k=!1,O=!1,D="#tabs-4",T=!0;$("#tabs").tabs({collapsible:!0,heightStyle:"fill"}),$(".tab_list").each(function(e,t){$(this).selectable({selected:function e(t,n){$(".ui-selected").removeClass("ui-selected"),$(n.selected).addClass("ui-selected");var o=n.selected.innerHTML;J(o);for(var i=0;i<g.length;++i)if(o===g[i].Name)return b=o,k||(v.show(0),h.show(0),f.show(0),k=!0),void h.html(g[i].Description[a])},autoRefresh:!1})}),v.click(function(e){e.preventDefault(),C||""!==b&&(x=b,mp.trigger("onMapMenuVote",x),A())}),f.click(function(e){if(e.preventDefault(),""!==b){var t=w.indexOf(b);-1===t?(w.push(b),f.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),mp.trigger("onClientToggleMapFavourite",b,!0),p.append($("<div>"+b+"</div>"))):(w.splice(t,1),f.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),mp.trigger("onClientToggleMapFavourite",b,!1),p.children("div:contains('"+b+"')").remove()),p.selectable("refresh")}});function openMapMenu(e,t){c.show(1e3),y=!0,a=e,b="",x="",l.empty(),d.empty(),p.empty(),u.empty(),g=JSON.parse(t);for(var n=0;n<g.length;++n){var o=$("<div>"+g[n].Name+"</div>");0===g[n].Type?l.append(o):d.append(o),-1!==w.indexOf(g[n].Name)&&p.append(o.clone())}for(var i=0;i<_.length;++i)u.append($("<div>"+_[i].name+"</div>"));l.selectable("refresh"),d.selectable("refresh"),p.selectable("refresh"),u.selectable("refresh")}function closeMapMenu(){y=!1,c.hide(500),k&&(v.hide(500),h.hide(500),f.hide(500),k=!1)}function M(e,t){return e.votes>t.votes?-1:1}function N(){_.sort(M)}function S(e){_.push({name:e,votes:1});var t=$("<div>"+_.length+". "+e+" (1)</div>");m.append(t),y&&(u.append($("<div>"+e+"</div>")),u.selectable("refresh")),x===e&&(t.addClass("mapvoting_selected"),x="")}function I(){for(var e=m.children(),t=0;t<e.length;++t)t in _?e.eq(t).text(t+1+". "+mapname+" ("+votes[t].votings+")"):_.splice(t)}function q(e){for(var t=0;t<_.length;++t)if(_[t].name===e)return--_[t].votes<=0&&(_.splice(t,1),m.children().eq(t).remove(),y&&$(D+" div:contains("+e+")")),void I()}function addVoteToMapVoting(e,t){x===e&&$(".mapvoting_selected").removeClass("mapvoting_selected"),void 0!==t&&q(t);for(var n=!1,a=0;a<_.length&&!n;++a)if(_[a].name===e){++_[a].votes;var o=m.children().eq(a);o.text(a+1+". "+e+"("+_[a].votes+")"),x===e&&(o.addClass("mapvoting_selected"),x=""),n=!0}n||S(e),N()}function loadMapVotings(e){var t=JSON.parse(mapsvotesjson);_=[];for(var n in t)_.push({name:n,votes:t[n]});N()}function clearMapVotings(){_=[],m.empty(),y&&(u.empty(),u.selectable("refresh"))}function A(){C=!0,v.disabled=!0,setTimeout(function(){C=!1,v.disabled=!1},1500)}$("body").keydown(function(e){var t=e.which;if(T&&t>=97&&t<=105){if(C)return;e.preventDefault();var n=9-(105-t)-1;_.length>n&&(x=_[n].name,mp.trigger("onMapMenuVote",x),A())}});function J(e){-1!==w.indexOf(e)?O||(f.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),O=!0):O&&(f.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),O=!1)}function loadFavouriteMaps(e){w=JSON.parse(e)}function toggleCanVoteForMapWithNumpad(e){T=e,e?(o.hide(300),m.show(300)):(o.show(300),m.hide(300))}var H={amountentries:[0,0],maxentries:40,active:!0,inputshowing:!1,maininput:$("#main-input"),bodies:[$("#normal-chat-body"),$("#dirty-chat-body")],chatends:["$normal$","$dirty$"],chosentab:null,chosenchatbody:0,myname:null,globalsaykeycode:"Z"===String.fromCharCode(90)?90:Y,playernames:[],autocompleteon:!1},L=[[/#r#/g,"rgb(222, 50, 50)"],[/#b#/g,"rgb(92, 180, 227)"],[/#g#/g,"rgb(113, 202, 113)"],[/#y#/g,"rgb(238, 198, 80)"],[/#p#/g,"rgb(131, 101, 224)"],[/#q#/g,"rgb(226, 79, 128)"],[/#o#/g,"rgb(253, 132, 85)"],[/#s#/g,"rgb(220, 220, 220)"],[/#w#/g,"white"],[/#dr#/g,"rgb(169, 25, 25)"]];function F(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:null;(e=null===e?R():e).animate({scrollTop:e.prop("scrollHeight")},"slow")}function V(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"";!H.active&&e||e!==H.inputshowing&&(mp.invoke("focus",e),e?(H.maininput.fadeIn(),H.maininput.val(t),setTimeout(function(){H.maininput.focus()},100)):(H.maininput.hide(),H.maininput.val("")),H.inputshowing=e,mp.trigger("onChatInputToggle",e))}var chatAPI={};chatAPI.push=G,chatAPI.clear=function(){R().html("")},chatAPI.activate=function(e){V(e),H.active=e},chatAPI.show=function(e){e?(R().show(),$("#chat_choice").show()):(R().hide(),$("#chat_choice").hide()),!e&&H.inputshowing&&V(!1),H.active=e};function s(e,t){var n="";t&&(n='<span style="background-color: rgba(255,178,102,0.6);">'),n+='<span style="color: white;">';var a=e;if(-1!==e.indexOf("#")){for(var o=0;o<L.length;++o)a=a.replace(L[o][0],"</span><span style='color: "+L[o][1]+";'>");a=a.replace(/#n#/g,"<br>")}return t&&(a+="</span>"),n+a+"</span>"}function j(e){if(null===H.myname)return!1;var t=e.indexOf("@");if(-1===t)return!1;var n=e.indexOf(":",t+1);if(-1===n)return!1;var a;return e.substring(t+1,n)===H.myname}function E(e,t,n){t.append(e),++H.amountentries[n]>=H.maxentries&&(--H.amountentries[n],t.find("text:first").remove()),F(t)}function G(e){for(var t=j(e),n=0;n<H.chatends.length;++n)if(e.endsWith(H.chatends[n])){e=e.slice(0,-H.chatends[n].length);var a=R(n),o=s(e,t),i;return void E($("<text>"+o+"</text>"),a,n)}for(var r=s(e,t),c=0;c<H.bodies.length;++c){var l=R(c),d;E($("<text>"+r+"</text>"),l,c)}}function R(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:-1,t=-1===e?H.chosenchatbody:e;return H.bodies[t]}function loadUserName(e){H.myname=e,H.playernames.push(e)}function addNameForChat(e){H.playernames.push(username)}function removeNameForChat(e){var t=H.playernames.indexOf(e);-1!==t&&H.playernames.splice(t,1)}$(document).ready(function(){addAutocomplete(H.maininput,H.playernames,function(){return H.autocompleteon=!0,!1},function(){setTimeout(function(){H.autocompleteon=!1},500)}),$("body").keydown(function(e){if(84===e.which&&!H.inputshowing&&H.active)e.preventDefault(),V(!0);else if(e.which===H.globalsaykeycode&&!H.inputshowing&&H.active)e.preventDefault(),V(!0,"/globalsay ");else if(13===e.which&&H.inputshowing&&!H.autocompleteon){e.preventDefault();var t=H.maininput.val();t&&("/"===t[0]?(t=t.substr(1)).length>0&&mp.invoke("command",t):mp.invoke("chatMessage",t+H.chatends[H.chosenchatbody])),V(!1)}}),H.chosentab=$("#chat_choice div[data-chatID=0]"),H.chosentab.css("background","#04074e"),$("#chat_choice div").click(function(){H.chosenchatbody!==$(this).attr("data-chatID")&&(H.chosentab.css("background","#A9A9A9"),H.bodies[H.chosenchatbody].hide(400),H.chosentab=$(this),H.chosentab.css("background","#04074e"),H.chosenchatbody=H.chosentab.attr("data-chatID"),H.bodies[H.chosenchatbody].show(400))}),mp.trigger("onChatLoad")});