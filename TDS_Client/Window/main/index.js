"use strict";var e=$("#bloodscreen"),n,o=$("#kill_messages_box"),a=$("#voice_chat_player_names_box"),l=9,c=$("#orders"),t,r=$("#audio_hit"),i=[r,r.clone(!0),r.clone(!0),r.clone(!0),r.clone(!0),r.clone(!0),r.clone(!0),r.clone(!0),r.clone(!0),r.clone(!0)],s=0,p=i.length,u=$("#audio_bombTick"),d=[u,u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0)],g=0,m=d.length,f,v=.05,b=!1,y=[];function _(){d[g++].trigger("play").prop("volume",v),g==m&&(g=0)}function playKillstreakSound(e){b?y.push(e):($("#audio_"+soundname).trigger("play").prop("volume",v),b=!0)}function onKillstreakSoundEnded(){var e;(b=!1,0!=y.length)&&playKillstreakSound(y.shift())}function startBombTickSound(e,n){_();var o,l=3e3-2950*(n/e);null!=f&&clearTimeout(f),f=setTimeout(function(){return startBombTickSound(e,n+l)},l)}function stopBombTickSound(){null!=f&&(clearTimeout(f),f=null)}function h(e){var n='<span style="color: white;">',o=e;return-1!==e.indexOf("~")&&(o=o.replace(/~r~/g,'</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g,'</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g,'</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g,'</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g,'</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g,'</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g,'</span><span style="color: white;">').replace(/~dr~/g,'</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g,"<br>")),n+o+"</span>"}function w(e){e.remove()}function T(e){e?c.show(1e3):c.hide(1e3)}mp.events.add("a",function(e){$("#audio_"+e).trigger("play").prop("volume",v)}),mp.events.add("b",function(){i[s++].trigger("play").prop("volume",v),s==p&&(s=0)}),mp.events.add("c",function(){t?(clearTimeout(t),n.style.animation="none",n.offsetHeight,n.style.animation="BloodscreenAnim 2.5s"):e.css({animation:"BloodscreenAnim 2.5s","animation-fill-mode":"forwards"}),t=setTimeout(function(){t=null,e.css({animation:""})},2500)}),mp.events.add("d",function(e){var n=$("<text>"+h(e)+"<br></text>");o.append(n),n.delay(11e3).fadeOut(4e3,n.remove)}),mp.events.add("e",function(e){var n="voice-chat-"+e.replace(/\W/g,"_"),o,l=$("<div id='"+n+"'>"+"<img src='../pic/speaker.png'/>"+"<span>"+e+"</span></div>");a.append(l)}),mp.events.add("f",function(e){var n="voice-chat-"+e.replace(/\W/g,"_"),o=a.find("#"+n);o.length&&o.remove()}),$(document).ready(function(){n=e.get()[0]});var x=$("#round_end_reason"),k=$("#round_end_reason_container"),O={},R,S=new SimpleStarRating($("#map_rating_stars")[0],function(e){R in O&&O[R]===e||(O[R]=e,mp.trigger("b34",R,e),S.disable())});function showRoundEndReason(e,n){x.html(e),k.css("display","table"),(R=n)in O?S.setCurrentRating(O[n]):S.setCurrentRating(0),S.enable()}function hideRoundEndReason(){k.hide()}function loadMyMapRatings(e){O=JSON.parse(e)}