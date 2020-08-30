var isregistered = false;
var langdata = {};

$(document).ready(function () {
    $('.forgotpassword').hide();

    $('.form').find('input, textarea').on('keyup blur focus', function (e) {
        var $this = $(this),
            label = $this.prev('label');

        if (e.type === 'keyup') {
            if ($this.val() === '') {
                label.removeClass('active highlight');
            } else {
                label.addClass('active highlight');
            }
        } else if (e.type === 'blur') {
            if ($this.val() === '') {
                label.removeClass('active highlight');
            } else {
                label.removeClass('highlight');
            }
        } else if (e.type === 'focus') {
            if ($this.val() === '') {
                label.removeClass('highlight');
            } else {
                label.addClass('highlight');
            }
        }
    });

    var lasttick = 0;

    $('.tab a').on('click', function (e) {
        var tick = new Date().getTime();
        if (tick >= lasttick + 1000) {
            let target = $(this).attr('href');
            if (!isregistered || target !== "#signup") {
                lasttick = tick;
                e.preventDefault();

                $('.forgotpassword').hide();
                $('.tab-content').fadeIn(1000);
                $(this).parent().addClass('active');
                $(this).parent().siblings().removeClass('active');

                $('.tab-content > div').not(target).hide();

                $(target).fadeIn(1000);
            }
        }
    });

    $('.forgotpw').on('click', function (e) {
        var tick = new Date().getTime();
        if (tick >= lasttick + 1000) {
            $('.tab-content').hide();
            $('.forgotpassword').fadeIn(1000);
            lasttick = 0;
        }
    });

    $("button:not( [type = 'submit'] )").click(function (event) {
        event.preventDefault();
        var type = $(this).attr("data-eventtype");
        switch (type) {
            case "lang_english":
                mp.trigger("b19", 9);	// LanguageChange_Browser
                break;
            case "lang_german":
                mp.trigger("b19", 7);	// LanguageChange_Browser
                break;
            default:
                return;
        }
        mp.trigger("b36");	// SyncRegisterLoginLanguageTexts_Browser
    });

    $(".form").submit(function (event) {
        event.preventDefault();
        var $this = $(this);
        var button = $this.find(':submit:not(:hidden)');
        var type = button.attr("data-eventtype");
        switch (type) {
            case "login":
				var username = $this.find("input[id=login_username]").val();
                var password = $this.find("input[id=login_password]").val();
                mp.trigger("b40", username, password);   // TryLogin_Browser
                break;

            case "register":
                $this.find("input:not(:hidden)[id=register_password_again]").each(function () {
					let username = $this.find("input[id=register_username]").val();
                    let password = $this.find("input[id=register_password]").val();
                    let email = $this.find("input[id=register_email]").val();
                    if (password === $(this).val()) {
						if (!(/^\d+$/.test(username))) {
							mp.trigger("b41", username, password, email);	// TryRegister_Browser
						} else {
							alert(langdata["name_may_not_only_numbers"]);
							event.preventDefault();
						}
                    } else {
                        alert(langdata["password_has_to_be_same"]);
                        event.preventDefault();
                    }
                });
                break;
        }
    });
});

function setLoginPanelData(playername, isreg, lang) {
    //mp.trigger( "outputCEF", "getLoginPanelData" );
    loadLanguage(lang);
    $("[data-lang=username]").each(function () {
        $(this).addClass('active highlight');
        $(this).next("input").val(playername).prop("disabled", isregistered);
    });

    isregistered = isreg;
}
alt.on("b", setLoginPanelData);

function loadLanguage(lang) {
    var langdata = JSON.parse(lang);
    $("[data-lang]").each(function () {
        $(this).html(langdata[$(this).attr("data-lang")] + ($(this).next().attr("required") ? "*" : ""));
        //$( this ).html( $( this ).attr( "data-lang" ) );
    });
}
alt.on("a", loadLanguage);
