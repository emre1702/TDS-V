"use strict";var t=!1,e={};$(document).ready(function(){$(".forgotpassword").hide(),$(".form").find("input, textarea").on("keyup blur focus",function(t){var e=$(this),a=e.prev("label");"keyup"===t.type?""===e.val()?a.removeClass("active highlight"):a.addClass("active highlight"):"blur"===t.type?""===e.val()?a.removeClass("active highlight"):a.removeClass("highlight"):"focus"===t.type&&(""===e.val()?a.removeClass("highlight"):a.addClass("highlight"))});var a=0;$(".tab a").on("click",function(e){var i=(new Date).getTime();i>=a+1e3&&(target=$(this).attr("href"),t&&"#signup"===target||(a=i,e.preventDefault(),$(".forgotpassword").hide(),$(".tab-content").fadeIn(1e3),$(this).parent().addClass("active"),$(this).parent().siblings().removeClass("active"),$(".tab-content > div").not(target).hide(),$(target).fadeIn(1e3)))}),$(".forgotpw").on("click",function(t){var e;(new Date).getTime()>=a+1e3&&($(".tab-content").hide(),$(".forgotpassword").fadeIn(1e3),a=0)}),$("button:not( [type = 'submit'] )").click(function(t){t.preventDefault();var e;switch($(this).attr("data-eventtype")){case"lang_english":mp.trigger("setLanguage","ENGLISH"),mp.trigger("getRegisterLoginLanguage");break;case"lang_german":mp.trigger("setLanguage","GERMAN"),mp.trigger("getRegisterLoginLanguage")}}),$(".form").submit(function(t){t.preventDefault();var a=$(this),i,n;switch(a.find(":submit:not(:hidden)").attr("data-eventtype")){case"login":var r=a.find("input[id=login_password]").val();mp.trigger("loginFunc",r);break;case"register":a.find("input:not(:hidden)[id=register_password_again]").each(function(){var i=a.find("input[id=register_password]").val();i===$(this).val()?mp.trigger("registerFunc",i,a.find("input[id=register_email]").val()):(alert(e.passwordhastobesame),t.preventDefault())})}})});function setLoginPanelData(e,a,i){mp.trigger("outputCEF","getLoginPanelData"),loadLanguage(i),$("[data-lang=username]").each(function(){$(this).addClass("active highlight"),$(this).next("input").val(e)}),$("[data-disable]").each(function(){$(this).prop("disabled",!0)}),t="1"===a}function loadLanguage(t){mp.trigger("outputCEF","loadLanguage");var e=JSON.parse(t);$("[data-lang]").each(function(){$(this).html(e[$(this).attr("data-lang")]+($(this).next().attr("required")?"*":""))})}