"use strict";var n=$("#bloodscreen"),e,o=9,l=$("#orders"),t,c=$("#audio_hit"),i=[c,c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0),c.clone(!0)],a=0,r=i.length,u=$("#audio_bombTick"),s=[u,u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0),u.clone(!0)],m=0,d=s.length,f,p=.05,g=!1,h=[];function v(){s[m++].trigger("play").prop("volume",p),m==d&&(m=0)}function playKillstreakSound(n){g?h.push(n):($("#audio_"+soundname).trigger("play").prop("volume",p),g=!0)}function onKillstreakSoundEnded(){var n;(g=!1,0!=h.length)&&playKillstreakSound(h.shift())}function startBombTickSound(n,e){v();var o,l=3e3-2950*(e/n);null!=f&&clearTimeout(f),f=setTimeout(function(){return startBombTickSound(n,e+l)},l)}function stopBombTickSound(){null!=f&&(clearTimeout(f),f=null)}function _(n){n?l.show(1e3):l.hide(1e3)}mp.events.add("a",function(n){$("#audio_"+n).trigger("play").prop("volume",p)}),mp.events.add("b",function(){i[a++].trigger("play").prop("volume",p),a==r&&(a=0)}),mp.events.add("c",function(){t?(clearTimeout(t),e.style.animation="none",e.offsetHeight,e.style.animation="BloodscreenAnim 2.5s"):n.css({animation:"BloodscreenAnim 2.5s","animation-fill-mode":"forwards"}),t=setTimeout(function(){t=null,n.css({animation:""})},2500)}),$(document).ready(function(){e=n.get()[0]});var y=$("#round_end_reason"),b=$("#round_end_reason_container"),T={},w,R=new SimpleStarRating($("#map_rating_stars")[0],function(n){w in T&&T[w]===n||(T[w]=n,mp.trigger("b34",w,n))});function showRoundEndReason(n,e){y.html(n),b.css("display","table"),(w=e)in T?R.setCurrentRating(T[e]):R.setCurrentRating(0),R.enable()}function hideRoundEndReason(){b.hide()}function loadMyMapRatings(n){T=JSON.parse(n)}