"use strict"; var n = $("#bloodscreen"), t = $("#kill_messages_box"), r = 9, a = $("#orders"), o, e = $("#audio_hit"), i = [e, e.clone(!0), e.clone(!0), e.clone(!0), e.clone(!0), e.clone(!0), e.clone(!0), e.clone(!0), e.clone(!0), e.clone(!0)], s = 0, c = i.length, l = $("#audio_bombTick"), d = [l, l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0), l.clone(!0)], u = 0, p = d.length, h; function playSound(e) { $("#audio_" + e).trigger("play").volume = .05 } function playHitsound() { i[s++].trigger("play").volume = .05, s == c && (s = 0) } function f() { d[u++].trigger("play").volume = .05, u == p && (u = 0) } function startBombTickSound(e, n) { f(); var t, a = 3e3 - 2950 * (n / e); null != h && clearTimeout(h), h = setTimeout(function () { return startBombTickSound(e, n + a) }, a) } function stopBombTickSound() { null != h && (clearTimeout(h), h = null) } function showBloodscreen() { if (o) { clearTimeout(o); var e = n.get()[0]; e.style.animation = "none", e.offsetHeight, e.style.animation = "BloodscreenAnim 2.5s" } else n.css({ display: "block", animation: "BloodscreenAnim 2.5s" }); o = setTimeout(function () { n.css("display", "none"), o = null }, 2500) } function g(e) { var n = '<span style="color: white;">', t = e; return -1 !== e.indexOf("~") && (t = t.replace(/~r~/g, '</span><span style="color: rgb(222, 50, 50);">').replace(/~b~/g, '</span><span style="color: rgb(92, 180, 227);">').replace(/~g~/g, '</span><span style="color: rgb(113, 202, 113);">').replace(/~y~/g, '</span><span style="color: rgb(238, 198, 80);">').replace(/~o~/g, '</span><span style="color: rgb(253, 132, 85);">').replace(/~s~/g, '</span><span style="color: rgb(220, 220, 220);">').replace(/~w~/g, '</span><span style="color: white;">').replace(/~dr~/g, '</span><span style="color: rgb( 169, 25, 25 );">').replace(/~n~/g, "<br>")), n + t + "</span>" } function m(e) { e.remove() } function addKillMessage(e) { var n = $("<text>" + g(e) + "<br></text>"); t.append(n), n.delay(11e3).fadeOut(4e3, n.remove) } function loadOrderNames(e) { a.empty(), alert(e); var n = JSON.parse(e); alert(n); var t = 0; Object.keys(n).forEach(function (e) { a.append($("<div>" + ++t + ". " + n[e] + "</div>")) }) } function toggleOrders(e) { e ? a.show(1e3) : a.hide(1e3) } var v = $("#mapmenu"), b = $("#tabs-1"), _ = $("#tabs-2"), y = $("#tabs-3"), w = $(R), x = $("#map_info"), C = $("#choose_map_button"), O = $("#add_map_to_favourites"), k = $("#mapvoting"), T, S = "", B = !1, N = [], D = [], A = "", J = !1, M = !1, I = !1, R = "#tabs-4", q = !0; function openMapMenu(e, n) { v.show(1e3), B = !0, r = e, A = S = "", b.empty(), _.empty(), y.empty(), w.empty(), T = JSON.parse(n); for (var t = 0; t < T.length; ++t) { var a = $("<div>" + T[t].Name + "</div>"); 0 === T[t].Type ? b.append(a) : _.append(a), -1 !== D.indexOf(T[t].Name) && y.append(a.clone()) } for (var o = 0; o < N.length; ++o)w.append($("<div>" + N[o].name + "</div>")); b.selectable("refresh"), _.selectable("refresh"), y.selectable("refresh"), w.selectable("refresh") } function closeMapMenu() { B = !1, v.hide(500), M && (C.hide(500), x.hide(500), O.hide(500), M = !1) } function H(e, n) { return e.votes > n.votes ? -1 : 1 } function j() { N.sort(H) } function F(e) { N.push({ name: e, votes: 1 }); var n = $("<div>" + N.length + ". " + e + " (1)</div>"); k.append(n), B && (w.append($("<div>" + e + "</div>")), w.selectable("refresh")), A === e && (n.addClass("mapvoting_selected"), A = "") } function L() { for (var e = k.children(), n = 0; n < e.length; ++n)n in N ? e.eq(n).text(n + 1 + ". " + mapname + " (" + votes[n].votings + ")") : N.splice(n) } function U(e) { for (var n = 0; n < N.length; ++n)if (N[n].name === e) return --N[n].votes <= 0 && (N.splice(n, 1), k.children().eq(n).remove(), B && $(R + " div:contains(" + e + ")")), void L() } function addVoteToMapVoting(e, n) { A === e && $(".mapvoting_selected").removeClass("mapvoting_selected"), void 0 !== n && U(n); for (var t = !1, a = 0; a < N.length && !t; ++a)if (N[a].name === e) { ++N[a].votes; var o = k.children().eq(a); o.text(a + 1 + ". " + e + "(" + N[a].votes + ")"), A === e && (o.addClass("mapvoting_selected"), A = ""), t = !0 } t || F(e), j() } function loadMapVotings(e) { var n = JSON.parse(mapsvotesjson); for (var t in N = [], n) N.push({ name: t, votes: n[t] }); j() } function clearMapVotings() { N = [], k.empty(), B && (w.empty(), w.selectable("refresh")) } function V() { J = !0, C.disabled = !0, setTimeout(function () { J = !1, C.disabled = !1 }, 1500) } function E(e) { -1 !== D.indexOf(e) ? I || (O.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"), I = !0) : I && (O.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"), I = !1) } function loadFavouriteMaps(e) { D = JSON.parse(e) } function toggleCanVoteForMapWithNumpad(e) { (q = e) ? (a.hide(300), k.show(300)) : (a.show(300), k.hide(300)) } $(document).ready(function () { v = $("#mapmenu"), b = $("#tabs-1"), _ = $("#tabs-2"), y = $("#tabs-3"), w = $(R), x = $("#map_info"), C = $("#choose_map_button"), O = $("#add_map_to_favourites"), k = $("#mapvoting"), $("#tabs").tabs({ collapsible: !0, heightStyle: "fill" }), $(".tab_list").each(function (e, n) { $(this).selectable({ selected: function e(n, t) { $(".ui-selected").removeClass("ui-selected"), $(t.selected).addClass("ui-selected"); var a = t.selected.innerHTML; E(a); for (var o = 0; o < T.length; ++o)if (a === T[o].Name) return S = a, M || (C.show(0), x.show(0), O.show(0), M = !0), void x.html(T[o].Description[r]) }, autoRefresh: !1 }) }), C.click(function (e) { e.preventDefault(), J || "" !== S && (A = S, mp.trigger("MapVote_Browser", A), V()) }), O.click(function (e) { if (e.preventDefault(), "" !== S) { var n = D.indexOf(S); -1 === n ? (D.push(S), O.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected"), mp.trigger("ToggleMapFavouriteState_Browser", S, !0), y.append($("<div>" + S + "</div>"))) : (D.splice(n, 1), O.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected"), mp.trigger("ToggleMapFavouriteState_Browser", S, !1), y.children("div:contains('" + S + "')").remove()), y.selectable("refresh") } }), $("body").keydown(function (e) { var n = e.which; if (q && 97 <= n && n <= 105) { if (J) return; e.preventDefault(); var t = 9 - (105 - n) - 1; N.length > t && (A = N[t].name, mp.trigger("MapVote_Browser", A), V()) } }) }); var W = { amountentries: [0, 0], maxentries: 40, active: !0, inputshowing: !1, maininput: $("#main-input"), bodies: [$("#normal-chat-body"), $("#dirty-chat-body")], chatends: ["$normal$", "$dirty$"], chosentab: null, chosenchatbody: 0, myname: null, globalsaykeycode: "Z" === String.fromCharCode(90) ? 90 : Y, playernames: [], autocompleteon: !1 }, Z = [[/#r#/g, "rgb(222, 50, 50)"], [/#b#/g, "rgb(92, 180, 227)"], [/#g#/g, "rgb(113, 202, 113)"], [/#y#/g, "rgb(238, 198, 80)"], [/#p#/g, "rgb(131, 101, 224)"], [/#q#/g, "rgb(226, 79, 128)"], [/#o#/g, "rgb(253, 132, 85)"], [/#s#/g, "rgb(220, 220, 220)"], [/#w#/g, "white"], [/#dr#/g, "rgb(169, 25, 25)"]]; function z() { var e = 0 < arguments.length && void 0 !== arguments[0] ? arguments[0] : null; (e = null === e ? ee() : e).finish().animate({ scrollTop: e.prop("scrollHeight") }, "slow") } function enableChatInput(e) { var n = 1 < arguments.length && void 0 !== arguments[1] ? arguments[1] : ""; !W.active && e || e !== W.inputshowing && (mp.invoke("focus", e), e ? (W.maininput.fadeIn(), W.maininput.val(n), setTimeout(function () { W.maininput.focus() }, 100)) : (W.maininput.hide(), W.maininput.val("")), W.inputshowing = e, mp.trigger("ChatInputToggled_Browser", e)) } var chatAPI = {}; function G(e) { for (var n = e.indexOf("!{"); -1 != n;) { var t = e.indexOf("}", n + 2); if (-1 == t) break; var a = e.substring(n, t + 1), o = a.substring(2, a.length - 1).split("|"), r = void 0; 1 == o.length ? r = o[0] : o.length < 3 ? r = "rgb(".concat(0 in o ? o[0] : 0, ", ").concat(1 in o ? o[1] : 0, ", 0)") : 3 == o.length ? r = "rgb(".concat(o[0], ", ").concat(o[1], ", ").concat(o[2], ")") : 4 == o.length && (r = "rgba(".concat(o[0], ", ").concat(o[1], ", ").concat(o[2], ", ").concat(o[3], ")")); var i = "</span><span style='color: " + r + ";'>"; n = (e = e.replace(a, i)).indexOf("!{", n + i.length) } return e } function K(e, n) { var t = ""; n && (t = '<span style="background-color: rgba(255,178,102,0.6);">'), t += '<span style="color: white;">'; var a = e; if (-1 !== e.indexOf("#")) { for (var o = 0; o < Z.length; ++o)a = a.replace(Z[o][0], "</span><span style='color: " + Z[o][1] + ";'>"); a = a.replace(/#n#/g, "<br>") } return a = G(a), n && (a += "</span>"), t + a + "</span>" } function P(e) { if (null === W.myname) return !1; var n = e.indexOf("@"); if (-1 === n) return !1; var t = e.indexOf(":", n + 1), a; return -1 !== t && e.substring(n + 1, t) === W.myname } function Q(e, n, t) { n.append(e), ++W.amountentries[t] >= W.maxentries && (--W.amountentries[t], n.find("text:first").remove()), z(n) } function X(e) { for (var n = P(e), t = 0; t < W.chatends.length; ++t)if (e.endsWith(W.chatends[t])) { e = e.slice(0, -W.chatends[t].length); var a = ee(t), o = K(e, n), r; return void Q($("<text>" + o + "</text>"), a, t) } for (var i = K(e, n), s = 0; s < W.bodies.length; ++s) { var c = ee(s), l; Q($("<text>" + i + "</text>"), c, s) } } function ee() { var e = 0 < arguments.length && void 0 !== arguments[0] ? arguments[0] : -1, n = -1 === e ? W.chosenchatbody : e; return W.bodies[n] } function loadUserName(e) { W.myname = e, W.playernames.push(e) } function loadNamesForChat(e) { W.playernames = JSON.parse(e) } function addNameForChat(e) { W.playernames.push(e) } function removeNameForChat(e) { var n = W.playernames.indexOf(e); -1 != n && W.playernames.splice(n, 1) } function ne(e) { return void 0 === e || null == e || e.replace(/\s/g, "").length < 1 } chatAPI.push = X, chatAPI.clear = function () { ee().html("") }, chatAPI.activate = function (e) { W.active = e }, chatAPI.show = function (e) { e ? (ee().show(), $("#chat_choice").show()) : (ee().hide(), $("#chat_choice").hide()), W.active = e }, $(document).ready(function () { addAutocomplete(W.maininput, W.playernames, function () { return !(W.autocompleteon = !0) }, function () { setTimeout(function () { W.autocompleteon = !1 }, 500) }), $("body").keydown(function (e) { if (13 === e.which && W.inputshowing && !W.autocompleteon) { e.preventDefault(); var n = W.maininput.val(); ne(n) ? mp.trigger("CloseChat_Browser") : "/" === (n = n.replace(/\\/g, "\\\\").replace(/\"/g, '\\"'))[0] ? 0 < (n = n.substr(1)).length && mp.trigger("CommandUsed_Browser", n) : mp.trigger("ChatUsed_Browser", n, 1 == W.chosenchatbody) } }), W.chosentab = $("#chat_choice div[data-chatID=0]"), W.chosentab.css("background", "#04074e"), $("#chat_choice div").click(function () { W.chosenchatbody !== $(this).attr("data-chatID") && (W.chosentab.css("background", "#A9A9A9"), W.bodies[W.chosenchatbody].hide(400), W.chosentab = $(this), W.chosentab.css("background", "#04074e"), W.chosenchatbody = W.chosentab.attr("data-chatID"), W.bodies[W.chosenchatbody].show(400)) }), mp.trigger("ChatLoaded_Browser") }); var te = $("#round_end_reason"), ae = $("#round_end_reason_container"), oe = {}, re, ie = new SimpleStarRating($("#map_rating_stars")[0], function (e) { re in oe && oe[re] === e || (oe[re] = e, mp.trigger("sendMapRating", re, e), ie.disable()) }); function showRoundEndReason(e, n) { te.html(e), ae.css("display", "table"), (re = n) in oe ? ie.setCurrentRating(oe[n]) : ie.setCurrentRating(0), ie.enable() } function hideRoundEndReason() { ae.hide() } function loadMyMapRatings(e) { oe = JSON.parse(e) }