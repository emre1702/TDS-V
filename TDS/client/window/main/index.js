"use strict";var e=$("#money"),t=$("#bloodscreen"),a=$("#kill_messages_box"),s="ENGLISH",n=$("#orders");function setMoney(t){e.text("$"+t)}function playSound(e){$("#audio_"+e).trigger("play").volume=.05}function showBloodscreen(){t.stop().show(0).hide(2500)}function o(e){var t='<span style="color: white;">',a=e;return-1!==e.indexOf("~")&&(a=a.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),t+a+"</span>"}function r(e){e.remove()}function addKillMessage(e){var t=$("<text>"+o(e)+"<br></text>");a.append(t),t.delay(11e3).fadeOut(4e3,t.remove)}function loadOrderNames(e){n.empty();for(var t=JSON.parse(e),a=0;a<t.length&&a<9;++a)n.append($("<div>"+(a+1)+". "+t[a]+"</div>"))}var i=$("#mapmenu"),l=$("#tabs-1"),d=$("#tabs-2"),p=$("#tabs-3"),c=$(N),v=$("#map_info"),f=$("#choose_map_button"),u=$("#add_map_to_favourites"),m=$("#mapvoting"),g=void 0,h="",_=!1,b=[],y=[],C="",w=!1,x=!1,O=!1,N="#tabs-4",M=!0;$("#tabs").tabs({collapsible:!0,heightStyle:"fill"}),$(".tab_list").each(function(e,t){$(this).selectable({selected:function e(t,a){$(".ui-selected").removeClass("ui-selected"),$(a.selected).addClass("ui-selected");var n=a.selected.innerHTML;F(n);for(var o=0;o<g.length;++o)if(n===g[o].Name)return h=n,x||(f.show(0),v.show(0),u.show(0),x=!0),void v.html(g[o].Description[s])},autoRefresh:!1})}),f.click(function(e){e.preventDefault(),w||""!==h&&(C=h,mp.trigger("onMapMenuVote",C),q())}),u.click(function(e){if(e.preventDefault(),""!==h){var t=y.indexOf(h);-1===t?(y.push(h),u.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),mp.trigger("onClientToggleMapFavourite",h,!0),p.append($("<div>"+h+"</div>"))):(y.splice(t,1),u.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),mp.trigger("onClientToggleMapFavourite",h,!1),p.children("div:contains('"+h+"')").remove()),p.selectable("refresh")}});function openMapMenu(e,t){i.show(1e3),_=!0,s=e,h="",C="",l.empty(),d.empty(),p.empty(),c.empty(),g=JSON.parse(t);for(var a=0;a<g.length;++a){var n=$("<div>"+g[a].Name+"</div>");0===g[a].Type?l.append(n):d.append(n),-1!==y.indexOf(g[a].Name)&&p.append(n.clone())}for(var o=0;o<b.length;++o)c.append($("<div>"+b[o].name+"</div>"));l.selectable("refresh"),d.selectable("refresh"),p.selectable("refresh"),c.selectable("refresh")}function closeMapMenu(){_=!1,i.hide(500),x&&(f.hide(500),v.hide(500),u.hide(500),x=!1)}function S(e,t){return e.votes>t.votes?-1:1}function T(){b.sort(S)}function k(e){b.push({name:e,votes:1});var t=$("<div>"+b.length+". "+e+" (1)</div>");m.append(t),_&&(c.append($("<div>"+e+"</div>")),c.selectable("refresh")),C===e&&(t.addClass("mapvoting_selected"),C="")}function D(){for(var e=m.children(),t=0;t<e.length;++t)t in b?e.eq(t).text(t+1+". "+mapname+" ("+votes[t].votings+")"):b.splice(t)}function J(e){for(var t=0;t<b.length;++t)if(b[t].name===e)return--b[t].votes<=0&&(b.splice(t,1),m.children().eq(t).remove(),_&&$(N+" div:contains("+e+")")),void D()}function addVoteToMapVoting(e,t){C===e&&$(".mapvoting_selected").removeClass("mapvoting_selected"),void 0!==t&&J(t);for(var a=!1,s=0;s<b.length&&!a;++s)if(b[s].name===e){++b[s].votes;var n=m.children().eq(s);n.text(s+1+". "+e+"("+b[s].votes+")"),C===e&&(n.addClass("mapvoting_selected"),C=""),a=!0}a||k(e),T()}function loadMapVotings(e){var t=JSON.parse(mapsvotesjson);b=[];for(var a in t)b.push({name:a,votes:t[a]});T()}function clearMapVotings(){b=[],m.empty(),_&&(c.empty(),c.selectable("refresh"))}function q(){w=!0,f.disabled=!0,setTimeout(function(){w=!1,f.disabled=!1},1500)}$("body").keydown(function(e){var t=e.which;if(M&&t>=97&&t<=105){if(w)return;e.preventDefault();var a=9-(105-t)-1;b.length>a&&(C=b[a].name,mp.trigger("onMapMenuVote",C),q())}});function F(e){-1!==y.indexOf(e)?O||(u.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"),O=!0):O&&(u.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"),O=!1)}function loadFavouriteMaps(e){y=JSON.parse(e)}function toggleCanVoteForMapWithNumpad(e){M=e,e?(n.hide(300),m.show(300)):(n.show(300),m.hide(300))}