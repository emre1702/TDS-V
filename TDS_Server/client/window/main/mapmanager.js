let mapMenuDiv = $("#mapmenu");
let normalMapsList = $("#tabs-1");
let bombMapsList = $("#tabs-2");
let favouriteMapsList = $("#tabs-3");
let votingMapsList = $(votingmapslisttabsid);
let mapInfo = $("#map_info");
let mapVoteButton = $("#choose_map_button");
let mapFavouriteButton = $("#add_map_to_favourites");
let mapVotingDiv = $("#mapvoting");
let mapDatas;
let lastMapName = "";
let showingMapMenu = false;
let votings = [];
let favouriteMaps = [];
let votedMapName = "";
let votingInCooldown = false;
let mapSpecificThingsShowing = false;
let currentlyonfavourite = false;
let votingmapslisttabsid = "#tabs-4";
var canvoteformapwithnumpad = true;

$("#tabs").tabs({
    collapsible: true,
    heightStyle: "fill"
});

$(".tab_list").each(function (index, value) {
    $(this).selectable({
        selected: function (event, ui) {
            $(".ui-selected").removeClass("ui-selected");
            $(ui.selected).addClass("ui-selected");
            let mapname = ui.selected.innerHTML;
            setFavouriteButtonSelectedByMap(mapname);
            for (let i = 0; i < mapDatas.length; ++i) {
                if (mapname === mapDatas[i].Name) {
                    lastMapName = mapname;
                    if (!mapSpecificThingsShowing) {
                        mapVoteButton.show(0);
                        mapInfo.show(0);
                        mapFavouriteButton.show(0);
                        mapSpecificThingsShowing = true;
                    }
                    mapInfo.html(mapDatas[i].Description[language]);
                    return;
                }
            }
        },
        autoRefresh: false
    });
});

mapVoteButton.click(function (event) {
    event.preventDefault();
    if (votingInCooldown)
        return;
    if (lastMapName !== "") {
        votedMapName = lastMapName;
        mp.trigger("onMapMenuVote", votedMapName);
        setMapVotingCooldown();
    }
});

mapFavouriteButton.click(function (event) {
    event.preventDefault();
    if (lastMapName === "")
        return;
    let index = favouriteMaps.indexOf(lastMapName);
    if (index === -1) {
        favouriteMaps.push(lastMapName);
        mapFavouriteButton.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected");
        mp.trigger("onClientToggleMapFavourite", lastMapName, true);
        favouriteMapsList.append($("<div>" + lastMapName + "</div>"));
    } else {
        favouriteMaps.splice(index, 1);
        mapFavouriteButton.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected");
        mp.trigger("onClientToggleMapFavourite", lastMapName, false);
        favouriteMapsList.children("div:contains('" + lastMapName + "')").remove();
    }
    favouriteMapsList.selectable("refresh");
});


function openMapMenu(mylang, mapdatasjson) {
    mapMenuDiv.show(1000);
    showingMapMenu = true;
    language = mylang;
    lastMapName = "";
    votedMapName = "";
    normalMapsList.empty();
    bombMapsList.empty();
    favouriteMapsList.empty();
    votingMapsList.empty();
    mapDatas = JSON.parse(mapdatasjson);
    for (let i = 0; i < mapDatas.length; ++i) {
        let element = $("<div>" + mapDatas[i].Name + "</div>");
        if (mapDatas[i].Type === 0)
            normalMapsList.append(element);
        else
            bombMapsList.append(element);
        if (favouriteMaps.indexOf(mapDatas[i].Name) !== -1)
            favouriteMapsList.append(element.clone());
    }
    for (let i = 0; i < votings.length; ++i) {
        votingMapsList.append($("<div>" + votings[i].name + "</div>"));
    }
    normalMapsList.selectable("refresh");
    bombMapsList.selectable("refresh");
    favouriteMapsList.selectable("refresh");
    votingMapsList.selectable("refresh");
}

function closeMapMenu() {
    showingMapMenu = false;
    mapMenuDiv.hide(500);
    if (mapSpecificThingsShowing) {
        mapVoteButton.hide(500);
        mapInfo.hide(500);
        mapFavouriteButton.hide(500);
        mapSpecificThingsShowing = false;
    }
}

function mapVotingsComparer(a, b) {
    return a.votes > b.votes ? -1 : 1;
}

function sortMapVotings() {
    votings.sort(mapVotingsComparer);
}

function addMapToVoting(mapname) {
    votings.push({ name: mapname, votes: 1 });
    let element = $("<div>" + votings.length + ". " + mapname + " (1)</div>");
    mapVotingDiv.append(element);
    if (showingMapMenu) {
        votingMapsList.append($("<div>" + mapname + "</div>"));
        votingMapsList.selectable("refresh");
    }
    if (votedMapName === mapname) {
        element.addClass("mapvoting_selected");
        votedMapName = "";
    }
}

function refreshMapVotingNames() {
    let children = mapVotingDiv.children();
    for (let i = 0; i < children.length; ++i) {
        if (i in votings) {
            children.eq(i).text(i + 1 + ". " + mapname + " (" + votes[i].votings + ")");
        } else
            votings.splice(i);
    }
}

function removeVoteFromMap(mapname) {
    for (let i = 0; i < votings.length; ++i) {
        if (votings[i].name === mapname) {
            if (--votings[i].votes <= 0) {
                votings.splice(i, 1);
                mapVotingDiv.children().eq(i).remove();
                if (showingMapMenu)
                    $(votingmapslisttabsid + " div:contains(" + mapname + ")");
            }
            refreshMapVotingNames();
            return;
        }
    }
}

function addVoteToMapVoting(mapname, oldmapname) {
    if (votedMapName === mapname)
        $(".mapvoting_selected").removeClass("mapvoting_selected");
    if (typeof oldmapname !== "undefined")
        removeVoteFromMap(oldmapname);
    let foundmap = false;
    for (let i = 0; i < votings.length && !foundmap; ++i) {
        if (votings[i].name === mapname) {
            ++votings[i].votes;
            let element = mapVotingDiv.children().eq(i);
            element.text(i + 1 + ". " + mapname + "(" + votings[i].votes + ")");
            if (votedMapName === mapname) {
                element.addClass("mapvoting_selected");
                votedMapName = "";
            }
            foundmap = true;
        }
    }
    if (!foundmap)
        addMapToVoting(mapname);
    sortMapVotings();
}

function loadMapVotings(votingsjson) {
    let mapsvotesdict = JSON.parse(mapsvotesjson);
    votings = [];
    for (let key in mapsvotesdict) {
        votings.push({ name: key, votes: mapsvotesdict[key] });
    }
    sortMapVotings();
}

function clearMapVotings() {
    votings = [];
    mapVotingDiv.empty();
    if (showingMapMenu) {
        votingMapsList.empty();
        votingMapsList.selectable("refresh");
    }
}

function setMapVotingCooldown() {
    votingInCooldown = true;
    mapVoteButton.disabled = true;
    setTimeout(() => { votingInCooldown = false; mapVoteButton.disabled = false; }, 1500);
}

$("body").keydown(function (event) {
    let key = event.which;
    // map-voting //
    if (canvoteformapwithnumpad) {
        if (key >= 0x61 && key <= 0x69) {
            if (votingInCooldown)
                return;
            event.preventDefault();
            let index = 9 - (0x69 - key) - 1;   // -1 because of indexing starting at 0
            if (votings.length > index) {
                votedMapName = votings[index].name;
                mp.trigger("onMapMenuVote", votedMapName);
                setMapVotingCooldown();
            }
        }
    }
});

function setFavouriteButtonSelectedByMap(mapname) {
    if (favouriteMaps.indexOf(mapname) !== -1) {
        if (!currentlyonfavourite) {
            mapFavouriteButton.removeClass("add_map_to_favourites_notselected").addClass("add_map_to_favourites_selected");
            currentlyonfavourite = true;
        }
    } else {
        if (currentlyonfavourite) {
            mapFavouriteButton.removeClass("add_map_to_favourites_selected").addClass("add_map_to_favourites_notselected");
            currentlyonfavourite = false;
        }
    }
}

function loadFavouriteMaps(favmapsjson) {
    favouriteMaps = JSON.parse(favmapsjson);
}

function toggleCanVoteForMapWithNumpad(bool) {
    canvoteformapwithnumpad = bool;
    if (bool) {
        ordersDiv.hide(300);
        mapVotingDiv.show(300);
    } else {
        ordersDiv.show(300);
        mapVotingDiv.hide(300);
    }
}