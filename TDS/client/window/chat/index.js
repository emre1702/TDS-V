"use strict";var t=[0,0],n=40,e=!0,r=!1,a=$("#main-input"),o=[$("#normal-chat-body"),$("#dirty-chat-body")],i=["$normal$","$dirty$"],c=$("#normal_chat"),g=0,l=[[/#r#/g,"rgb(222, 50, 50)"],[/#b#/g,"rgb(92, 180, 227)"],[/#g#/g,"rgb(113, 202, 113)"],[/#y#/g,"rgb(238, 198, 80)"],[/#p#/g,"rgb(131, 101, 224)"],[/#q#/g,"rgb(226, 79, 128)"],[/#o#/g,"rgb(253, 132, 85)"],[/#s#/g,"rgb(220, 220, 220)"],[/#w#/g,"white"],[/#dr#/g,"rgb(169, 25, 25)"]];function h(){var t=arguments.length>0&&void 0!==arguments[0]?arguments[0]:null;(t=null===t?v():t).animate({scrollTop:t.prop("scrollHeight")},"slow")}function s(t){var n=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"";!e&&t||t!==r&&(mp.invoke("focus",t),t?(a.fadeIn(),a.val(n),setTimeout(function(){a.focus()},100)):(a.hide(),a.val("")),r=t,mp.trigger("onChatInputToggle",t))}var chatAPI={};chatAPI.push=d,chatAPI.clear=function(){v().html("")},chatAPI.activate=function(t){s(t)},chatAPI.show=function(t){t?v().show():v().hide(),e=t};function u(t){var n='<span style="color: white;">',e=t;if(-1!==t.indexOf("#")){for(var r=0;r<l.length;++r)e=e.replace(l[r][0],"</span><span style='color: "+l[r][1]+";'>");e=e.replace(/#n#/g,"<br>")}return n+e+"</span>"}function f(e,r,a){r.append(e),++t[a]>=n&&(--t[a],r.find("text:first").remove()),h(r)}function d(t){for(var n=0;n<i.length;++n)if(t.endsWith(i[n])){t=t.slice(0,-i[n].length);var e=v(n),r=u(t),a;return void f($("<text>"+r+"</text>"),e,n)}for(var c=u(t),g=0;g<o.length;++g){var l=v(g),h;f($("<text>"+c+"</text>"),l,g)}}function v(){var t=arguments.length>0&&void 0!==arguments[0]?arguments[0]:-1,n;return o[-1===t?g:t]}$(document).ready(function(){$("body").keydown(function(t){if(84===t.which&&!r&&e)t.preventDefault(),s(!0);else if(90===t.which&&!r&&e)t.preventDefault(),s(!0,"/globalsay ");else if(13===t.which&&r){t.preventDefault();var n=a.val();n&&("/"===n[0]?(n=n.substr(1)).length>0&&mp.invoke("command",n):mp.invoke("chatMessage",n+i[g])),s(!1)}}),c.css("background","#04074e"),$("#chat_choice div").click(function(){g!==$(this).attr("data-chatID")&&(c.css("background","#A9A9A9"),o[g].hide(400),(c=$(this)).css("background","#04074e"),g=c.attr("data-chatID"),o[g].show(400))}),mp.trigger("onChatLoad")});