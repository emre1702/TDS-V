"use strict";var t=[0,0],n=40,e=!0,r=!1,a=$("#main-input"),o=[$("#normal-chat-body"),$("#dirty-chat-body")],c=["$normal$","$dirty$"],g=o[0],l=0,s=[[/~r~/g,"rgb(222, 50, 50)"],[/~b~/g,"rgb(92, 180, 227)"],[/~g~/g,"rgb(113, 202, 113)"],[/~y~/g,"rgb(238, 198, 80)"],[/~p~/g,"rgb(131, 101, 224)"],[/~q~/g,"rgb(226, 79, 128)"],[/~o~/g,"rgb(253, 132, 85)"],[/~s~/g,"rgb(220, 220, 220)"],[/~w~/g,"white"],[/~dr~/g,"rgb(169, 25, 25)"]];function h(){var t=arguments.length>0&&void 0!==arguments[0]?arguments[0]:null;(t=null===t?p():t).animate({scrollTop:t.prop("scrollHeight")},"slow")}function u(t){var n=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"";!e&&t||t!==r&&(mp.invoke("focus",t),t?(a.fadeIn(),a.val(n),setTimeout(function(){a.focus()},100)):(a.hide(),a.val("")),r=t,mp.trigger("onChatInputToggle",t))}var chatAPI={};chatAPI.push=v,chatAPI.clear=function(){p().html("")},chatAPI.activate=function(t){u(t)},chatAPI.show=function(t){t?p().show():p().hide(),e=t};function f(t){var n='<span style="color: white;">',e=t;if(-1!==t.indexOf("~")){for(var r=0;r<s.length;++r)e=e.replace(s[r][0],"</span><span style='color: "+s[r][1]+";'>");e=e.replace(/~n~/g,"<br>")}return n+e+"</span>"}function d(e,r){r.append(e),++t[i]>=n&&(--t[i],r.find("text:first").remove()),h(r)}function v(t){for(var n=0;n<c.length;++n)if(t.startsWith(c[n])){t=t.substring(c[n].length+1);var e=p(n),r;return void d($("<text>"+f(t)+"<br></text>"),e)}for(var a=0;a<o.length;++a){var i=p(a),g;d($("<text>"+f(t)+"<br></text>"),i)}}function p(){var t=arguments.length>0&&void 0!==arguments[0]?arguments[0]:-1,n=-1===t?l:t;return o[t]}$(document).ready(function(){$("body").keydown(function(t){if(84===t.which&&!r&&e)t.preventDefault(),u(!0);else if(90===t.which&&!r&&e)t.preventDefault(),u(!0,"/globalsay ");else if(13===t.which&&r){t.preventDefault();var n=a.val();n&&("/"===n[0]?(n=n.substr(1)).length>0&&mp.invoke("command",n):mp.invoke("chatMessage",c[l]+n)),u(!1)}}),g.css("background","#04074e"),$("#chat_choice div").click(function(){l!==$(this).attr("data-chatID")&&(g.css("background","#A9A9A9"),o[l].hide(400),(g=$(this)).css("background","#04074e"),l=g.attr("data-chatID"),o[l].show(400))}),mp.trigger("onChatLoad")});