"use strict"; var i = "<div class='text-field'>\t                        <label for='lobby_name' data-lang='lobby_name'>lobby-name*</label>\t                        <input type='text' id='lobby_name' required minlength='3' maxlength='20' />\t                    </div>\t                    <div class='select-field'>\t                        <select class='select-field-select' required id='lobby_mode' size='1'>\t                            <option selected data-lang='arena'>arena</option>\t                            <option data-lang='bomb'>bomb</option>\t                        </select>\t                        <label class='select-field-label' data-lang='mode'>mode*</label>\t                    </div>\t                     <div class='text-field'>\t                        <label for='lobby_password' data-lang='lobby_password'>lobby-password</label>\t                        <input type='text' id='lobby_password' required minlength='3' maxlength='20' />\t                    </div>\t                    <div class='number-field'>\t                        <label for='max_players' data-lang='max_players'>max-players*</label>\t                        <input type='number' id='max_players' required min='1' max='1000' value='100' step='1' />\t                    </div> \t                     <div class='number-field'>\t                        <label for='round_time' data-lang='round_time'>round-time (seconds)*</label>\t                        <input type='number' id='round_time' required min='30' max='99999999' value='240' step='1' />\t                    </div>   \t                     <div class='number-field'>\t                        <label for='countdown_time' data-lang='countdown_time'>countdown-time (seconds)*</label>\t                        <input type='number' id='countdown_time'required min='0' max='99999' value='3' step='1' />\t                    </div>  \t                    <div class='number-field'>\t                        <label for='armor' data-lang='armor'>armor*</label>\t                        <input type='number' id='armor' required min='0' max='100' value='100' step='1' />\t                    </div> \t                     <div class='number-field'>\t                        <label for='health' data-lang='health'>health*</label>\t                        <input type='number' id='health' required min='1' max='100' value='100' step='1' />\t                    </div> \t                    <div class='number-field'>\t                        <label for='time-scale' data-lang='time-scale'>time-scale*</label>\t                        <input type='number' id='time-scale' required min='0' max='1' value='1.0' step='0.1' />\t                    </div>"; function setLobbyChoiceLanguage(e) { var a = JSON.parse(e); $("[data-lang]").each(function () { $(this).html(a[$(this).attr("data-lang")] + ($(this).prop("required") ? "*" : "")) }) } $(document).ready(function () { function t(e) { var a = $(this), t = a.prev("label"); "keyup" === e.type ? "" === a.val() ? t.removeClass("active highlight") : t.addClass("active highlight") : "blur" === e.type ? "" === a.val() ? t.removeClass("active highlight") : t.removeClass("highlight") : "focus" === e.type && ("" === a.val() ? t.removeClass("highlight") : t.addClass("highlight")) } function l() { var e = $(this), a = e.prev("label"); "" === e.val() ? a.removeClass("active highlight") : a.addClass("active highlight") } $(".form").find("input, textarea").on("keyup blur focus", t), $(".form").find("input, textarea").each(l), $("button:not([type='submit'])").click(function (e) { var a; switch (e.preventDefault(), $(this).attr("data-eventtype")) { case "join_arena": $("#lobby_choice").hide(), $("#team_choice").show(); break; case "join_arena_player": mp.trigger("ChooseLobbyToJoin_Browser", 1, 1); break; case "join_arena_spectator": mp.trigger("ChooseLobbyToJoin_Browser", 1, 0); break; case "join_arena_back": $("#team_choice").hide(), $("#lobby_choice").show(); break; case "join_gang": break; case "custom_lobby": $("#lobby_choice").hide(), $("#custom_lobby").fadeIn(2e3); break; case "lang_english": mp.trigger("LanguageChange_Browser", 9), mp.trigger("SyncChoiceLanguageTexts_Browser"); break; case "lang_german": mp.trigger("LanguageChange_Browser", 7), mp.trigger("SyncChoiceLanguageTexts_Browser"); break; case "custom_lobby_back": $("#custom_lobby").hide(), $("#lobby_choice").show(); break; case "custom_lobby_own": $("#custom_lobby_setting_form").empty(), $("#custom_lobby_setting_form").append(i), $(".form").find("input, textarea").on("keyup blur focus", t), $(".form").find("input, textarea").each(l) } }), $("form").submit(function (e) { e.preventDefault(); var a = $("#lobby_name").val(), t = $("#lobby_mode :selected").text(), l = $("#lobby_password").val(), i = $("#round_time").val(), r = $("#countdown_time").val(), o = $("#max_players").val(), n = $("#armor").val(), s = $("#health").val(), d = $("#time-scale").val(); mp.trigger("createLobby", a, t, l, i, r, o, n, s, d) }), $("form").validate({ errorPlacement: function e(a, t) { $("#validate-error").html("<div class='validate-error'>" + a.text() + "</div>") } }) });