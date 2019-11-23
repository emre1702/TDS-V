addEvent ( "getVehBackToPosInMoving", true )
addEvent ( "getVehToPosInMoving", true )
addEvent ( "spectateInGangwar", true )
addEvent ( "portMeToGangArea", true )
addEvent ( "stopGangwarPreparation", true )
addEvent ( "sendGangFunGangwarInvitation", true )


local funcs = {}
playerOnlineInGang = {}
local amountgangareas = 0
gangAreaDatas = {}
local gangwarDatas = {}
local gangwarDimensions = { }		-- 65534 Open-World
local gangareasAlreadyAttackedOnce = {}
local playerGangwarData = {}
local areaByColshape = {}
local gangsInGangwar = {}
local gangsInGangwarPreparation = {}
local areaThingsOwner = {}
playersInGangwar = {} 		-- To use it in /leave to block it, in Damage and Kill Function to count them
local gangwarTimer = {}
local playersBecameDamage = {}
local playersGaveDamage = {}
local weaponMags = { [22] = 17, [23] = 17, [24] = 7, [29] = 30, [30] = 30, [31] = 50, [33] = 10 }
local gangareastook = {}
local playeraskedtojoin = {}
gangCarsSpawned = {}
local lastvehmovedata = {}
local areamoneysteps = 50
local preparationtimer = {}
gangwarsDoneByGang = {}
local vehalreadygettingmoved = {}
local infungangwar = {}
local skinids = {}
local gwpickupchecktime = 3000
local gwpickupseconds = 15
local antiattackspam = {}
local askedforfungw = {}
local invitedfrom = {}


local function getFreeDimension ( )
	local i = 65533
	while gangwarDimensions[i] do 
		i = i - 1
	end
	return i
end


local function clickedOnVehicleInGangwar ( button, state, player )
	if button == "left" and state == "down" then
		if getElementType ( source ) == "vehicle" then
			if not lastvehmovedata[player] or not isElement ( lastvehmovedata[player][1] ) then
				local team = getPlayerTeam ( player )
				if team and getVehicleTeam ( source ) == team then
					if not vehalreadygettingmoved[source] then
						local gang = tdsGetElementData ( player, "gangwarGang" )
						local area = gang and gangsInGangwarPreparation[gang] or nil
						if area and gangwarDatas[area]["attackergang"] == gang then
							if getElementDimension ( source ) ~= gangwarLobbyDimension and getElementDimension ( player ) ~= gangwarLobbyDimension then
								local occupants = getVehicleOccupants ( source )
								for _, _ in pairs ( occupants ) do
									return
								end
								local x, y, z = getElementPosition ( source )
								local xr, yr, zr = getElementRotation ( source )
								lastvehmovedata[player] = { source, x, y, z, xr, yr, zr }
								setElementFrozen ( source, true )
								setElementCollisionsEnabled ( source, false )
								vehalreadygettingmoved[source] = true
								setElementAlpha ( source, 120 )
								triggerClientEvent ( player, "moveTheVehicleByMouse", player, source )
							end
						end
					end
				end
			end
		end
	end
end 


local function givePlayerWeaponsAndRest ( player )
	local acpoints = tdsGetElementData ( player, "Aktivitaetspunkte" ) or 15
	local totalmags = 15 + acpoints
	local chosen = tdsGetElementData ( player, "chosenclass" ) or 1
	takeAllWeapons ( player )
	if gangwarPlayerWeapons[player] and gangwarPlayerWeapons[player][chosen] then
		for weapon, ammopercent in pairs ( gangwarPlayerWeapons[player][chosen] ) do
			local ammo = totalmags * ammopercent / 100 * weaponMags[weapon]
			giveWeapon ( player, weapon, ammo )
		end
	else
		giveWeapon ( player, 24, totalmags * 25 / 100 * weaponMags[24] )
		giveWeapon ( player, 29, totalmags * 25 / 100 * weaponMags[29] )
		giveWeapon ( player, 31, totalmags * 25 / 100 * weaponMags[31] )
		giveWeapon ( player, 33, totalmags * 25 / 100 * weaponMags[33] )
	end
	setElementHealth ( player, 100 )
	setPedArmor ( player, 100 )
end


local function setPlayerInGangwar ( player, area, status, attacker, isattackstarter )
	local pname = getPlayerName ( player )
	if not playerGangwarData[pname] or playerGangwarData[pname].allowedtojoin then 
		playerGangwarData[pname] = { area = area, status = status, damage = 0, kills = 0, died = false, offline = false, gotdamage = false, attacker = attacker, allowedtojoin = false }
		playersInGangwar[player] = { attacker = attacker }
		local x, y, z = getElementPosition ( player )
		local _, _, rot = getElementRotation ( player )
		local team = getPlayerTeam ( player )
		local gang = tdsGetElementData ( player, "gangwarGang" )
		spawnPlayer ( player, x, y, z+1, rot, skinids[gang] or 0, 0, gangwarDatas[area].dimension, team ) 
		givePlayerWeaponsAndRest ( player )
		tdsSetElementData ( player, "status", "playing" )

		setElementVisibleTo ( playerblips[player], team, true )

		local side = attacker and "attacker" or "defender"
		local otherside = attacker and "defender" or "attacker"
		gangwarDatas[area][side][#gangwarDatas[area][side]+1] = pname
		gangwarDatas[area][side.."alive"][#gangwarDatas[area][side.."alive"]+1] = pname
		local attackerID = gangwarGangIDs[gangwarDatas[area]["attackergang"]] 
		local defenderID = gangwarGangIDs[gangwarDatas[area]["defendergang"]] 

		playersBecameDamage[area][pname] = {}
		playersGaveDamage[area][pname] = {}

		local triggerinfo = { 
				attacker = gangwarDatas[area]["attackergang"],
				defender = gangwarDatas[area]["defendergang"],
				attackercolor = { gangwarGangData[attackerID].r, gangwarGangData[attackerID].g, gangwarGangData[attackerID].b },
				defendercolor = { gangwarGangData[defenderID].r, gangwarGangData[defenderID].g, gangwarGangData[defenderID].b },
				ownsidelist = gangwarDatas[area][side],
				ownsidealivelist = gangwarDatas[area][side.."alive"], 
				attackeramount = #gangwarDatas[area]["attacker"],
				attackeralive = #gangwarDatas[area]["attackeralive"], 
				defenderamount = #gangwarDatas[area]["defender"], 
				defenderalive = #gangwarDatas[area]["defenderalive"], 
				restsec = gangwarDatas[area].endtick - getTickCount(),
				pickup = gangwarDatas[area].clonepickup,
				ownside = side
		}
		triggerLatentClientEvent ( player, "startGangwarDraw", 50000, false, player, triggerinfo, isattackstarter )

		if gangwarDatas[area][side.."alive"] and gangwarDatas[area][side.."alive"][1] then
			for i=1, #gangwarDatas[area][side.."alive"] do
				local thePlayer = getPlayerFromName ( gangwarDatas[area][side.."alive"][i] )
				if isElement ( thePlayer ) and thePlayer ~= player then
					triggerClientEvent ( thePlayer, "updateGangwarDraw", thePlayer, side, pname )
				end
			end
		end
		if gangwarDatas[area][otherside.."alive"] and gangwarDatas[area][otherside.."alive"][1] then
			for i=1, #gangwarDatas[area][otherside.."alive"] do
				local thePlayer = getPlayerFromName ( gangwarDatas[area][otherside.."alive"][i] )
				if isElement ( thePlayer ) and thePlayer ~= player then
					triggerClientEvent ( thePlayer, "updateGangwarDraw", thePlayer, side )
				end
			end
		end
		if playerspawnedvehicles[player] and playerspawnedvehicles[player][1] then
			for i=1, #playerspawnedvehicles[player] do
				if isElement ( playerspawnedvehicles[player][i] ) then
					local vx, vy, vz = getElementPosition ( playerspawnedvehicles[player][i] )
					if getDistanceBetweenPoints3D ( x, y, z, vx, vy, vz ) <= 20 then
						for _, playerinveh in pairs ( getVehicleOccupants ( playerspawnedvehicles[player][i] ) ) do
							removePedFromVehicle ( playerinveh )
						end
						setElementVelocity ( playerspawnedvehicles[player][i], 0, 0, 0 )
						setElementDimension ( playerspawnedvehicles[player][i], gangwarDatas[area].dimension )
						local r, g, b = 255, 4, 4
						if not attacker then
							r, g, b = 0, 204, 0
						end
						setVehicleColor ( playerspawnedvehicles[player][i], r, g, b )
						removeEventHandler ( "onVehicleStartEnter", playerspawnedvehicles[player][i], letOnlyOwnerSitInVehicle )
						if status == "preparation" then
							addEventHandler ( "onElementClicked", playerspawnedvehicles[player][i], clickedOnVehicleInGangwar )
						end
						setVehicleTeam ( playerspawnedvehicles[player][i], team )
						gangwarDatas[area].vehicles[#gangwarDatas[area].vehicles+1] ={ playerspawnedvehicles[player][i], attacker and attackerID or defenderID }
					end
				end
			end
		end
		playerspawnedvehicles[player] = nil
		addEventHandler ( "onPlayerQuit", player, playerLeftGangwar )

		setTimer ( function ( ) 
			if isElement ( player ) then
				local x, y, z = getElementPosition ( player )
				setElementPosition ( player, x, y, z )
			end
		end, 1000, 1 )
	end
end


local function drawEveryPastInvitation ( area )
	for player, bool in pairs ( playeraskedtojoin ) do
		if isElement ( player ) then
			local gang = tdsGetElementData ( player, "gangwarGang" )
			if gang then
				if not gangsInGangwar[gang] and not gangsInGangwarPreparation[gang] then
					removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
					removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
					unbindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
					unbindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
					unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
					unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
					unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
					unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
					triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
					playeraskedtojoin[player] = true
				else
					local area = gangsInGangwar[gang] or gangsInGangwarPreparation[gang] 
					if not gangwarDatas[area] or ( not gangwarDatas[area].joinallowed and getTickCount() - gangwarDatas[area].starttick > 60*1000 ) then
						removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
						removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
						unbindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
						unbindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
						unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
						unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
						unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
						unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
						triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
						playeraskedtojoin[player] = true
					end
				end
			else
				removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
				removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
				unbindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
				unbindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
				unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
				unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
				unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
				unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
				triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
				playeraskedtojoin[player] = true
			end
		else
			playeraskedtojoin[player] = nil
		end
	end
end


local function clearGangwar ( area, attackerwon )
	if isTimer ( gangwarDatas[area].wintimer ) then
		killTimer ( gangwarDatas[area].wintimer )
	end
	if isTimer ( gangwarDatas[area].checktimer ) then
		killTimer ( gangwarDatas[area].checktimer )
	end
	if isTimer ( gangwarDatas[area].joinallowedtimer ) then
		killTimer ( gangwarDatas[area].joinallowedtimer )
	end
	local attackergang = gangwarDatas[area].attackergang
	local defendergang = gangwarDatas[area].defendergang
	gangsInGangwarPreparation[attackergang] = nil
	gangsInGangwarPreparation[defendergang] = nil
	gangsInGangwar[attackergang] = nil
	gangsInGangwar[defendergang] = nil
	local wasfungw = infungangwar[attackergang] or infungangwar[defendergang] or infungangwar[area] or false 
	infungangwar[attackergang] = nil 
	infungangwar[defendergang] = nil 
	infungangwar[area] = nil 
	local attackerID = gangwarGangIDs[attackergang]
	local defenderID = gangwarGangIDs[defendergang]
	antiattackspam[attackerID] = getTickCount()
	gangwarsDoneByGang[gangwarGangData[attackerID].short] = ( gangwarsDoneByGang[gangwarGangData[attackerID].short] or 0 ) + 1
	gangwarsDoneByGang[gangwarGangData[defenderID].short] = ( gangwarsDoneByGang[gangwarGangData[defenderID].short] or 0 ) + 1
	destroyElement ( gangwarDatas[area].clonearea )
	destroyElement ( gangwarDatas[area].clonepickup )
	destroyElement ( gangwarDatas[area].clonepickupcol )
	gangCarsSpawned[attackerID] = nil
	gangCarsSpawned[defenderID] = nil
	skinids[attackergang] = nil
	skinids[defendergang] = nil
	drawEveryPastInvitation ( area )
	if not wasfungw then
		gangAreaDatas[area].lastattack = getTickCount()
	end
	setRadarAreaFlashing ( area, false )
	gangwarDimensions[gangwarDatas[area].dimension] = nil
	triggerLatentClientEvent ( playersinlobby["gangwar"], "giveGangwarDoneByGang", resourceRoot, gangwarGangData[attackerID].short, gangwarGangData[defenderID].short )

	local alreadygiven = {}
	for attackername, array in pairs ( playersGaveDamage[area] ) do
		local attacker = getPlayerFromName ( attackername )
		if isElement ( attacker ) then
			if playersBecameDamage[area][attackername] then
				alreadygiven[attackername] = true
				triggerLatentClientEvent ( attacker, "syncDamagesGivenAfterRound", 20000, false, attacker, array, playersBecameDamage[area][attackername] )
			else
				triggerClientEvent ( attacker, "syncDamagesGivenAfterRound", attacker, array )
			end
		end
	end
	for sourcename, array in pairs ( playersBecameDamage[area] ) do
		if not alreadygiven[sourcename] then
			local player = getPlayerFromName ( sourcename )
			if isElement ( player ) then
				triggerLatentClientEvent ( player, "syncDamagesTakenAfterRound", 20000, false, player, array )
			end
		end
	end
	playersGaveDamage[area] = nil
	playersBecameDamage[area] = nil

	if gangwarDatas[area].vehicles and gangwarDatas[area].vehicles[1] then
		for i=1, #gangwarDatas[area].vehicles do
			if gangwarDatas[area].vehicles[i] and isElement ( gangwarDatas[area].vehicles[i][1] ) then
				local vehmodel = getElementModel ( gangwarDatas[area].vehicles[i][1] )
				local gangID = gangwarDatas[area].vehicles[i][2]
				for j=1, #gangwarGangVehicles[gangID] do
					if gangwarGangVehicles[gangID][j].model == vehmodel then
						gangwarGangVehicles[gangID][j].inuse = gangwarGangVehicles[gangID][j].inuse > 0 and gangwarGangVehicles[gangID][j].inuse - 1 or 0
						if vehiclesWithLowering[gangwarDatas[area].vehicles[i][1]] then
							vehiclesWithLowering[gangwarDatas[area].vehicles[i][1]] = nil
							gangwarGangVehicles[gangID][j].loweringinuse = gangwarGangVehicles[gangID][j].loweringinuse > 0 and gangwarGangVehicles[gangID][j].loweringinuse - 1 or 0
						end
						break
					end
				end
				destroyElement ( gangwarDatas[area].vehicles[i][1] )
			end
		end
	end

	local winnerID = attackerwon and attackerID or defenderID
	local loserID = attackerwon and defenderID or attackerID
	local winnergang = attackerwon and attackergang or defendergang
	local losergang = attackerwon and defendergang or attackergang
	local winner = attackerwon and "attacker" or "defender"
	local loser = attackerwon and "defender" or "attacker"
	local triggerTo = {}
	local damagearray = { {}, {} }

	local wx, wy, wz, wrot = gangwarGangData[winnerID]["spawn"]["x"], gangwarGangData[winnerID]["spawn"]["y"], gangwarGangData[winnerID]["spawn"]["z"], gangwarGangData[winnerID]["spawn"]["rot"]
	local wskin, wteam = gangwarGangData[winnerID]["skin"], teams[winnergang] 
	if gangwarDatas[area][winner] and gangwarDatas[area][winner][1] then
		for i=1, #gangwarDatas[area][winner] do
			local player = getPlayerFromName ( gangwarDatas[area][winner][i] )
			if isElement ( player ) and tdsGetElementData ( player, "GangwarGewonnen" ) and tdsGetElementData ( player, "Punkte" ) then
				if isTimer ( preparationtimer[player] ) then
					killTimer ( preparationtimer[player] )
				end
				if not testGangs[winnerID] and not wasfungw then
					tdsSetElementData ( player, "GangwarGewonnen", tdsGetElementData ( player, "GangwarGewonnen" ) + 1 )
					tdsSetElementData ( player, "Punkte", tdsGetElementData ( player, "Punkte" ) + gangAreaDatas[area].money )
				end
				playersInGangwar[player] = nil
				if tdsGetElementData ( player, "lobby" ) == "gangwar" then
					takeAllWeapons ( player )
					setElementHealth ( player, 100 )
					setPedArmor ( player, 100 )
					tdsSetElementData ( player, "status", "gangwar" )
					if getElementDimension ( player ) ~= 65534 then
						setElementDimension ( player, 65534 )
						setElementFrozen ( player, false )
						toggleAllControls ( player, true, true, false )
						setCameraTarget ( player )
						triggerClientEvent ( player, "onClientCameraSpectateStop", player )
					end
					triggerTo[#triggerTo+1] = { player, true }
				end
				removeEventHandler ( "onPlayerQuit", player, playerLeftGangwar )
			elseif not testGangs[winnerID] and not wasfungw then
				playerWinsToSave[gangwarDatas[area][winner][i]] = ( playerWinsToSave[gangwarDatas[area][winner][i]] or 0 ) + 1
			end
			if playerGangwarData[gangwarDatas[area][winner][i]] then
				damagearray[2][gangwarDatas[area][winner][i]] = { playerGangwarData[gangwarDatas[area][winner][i]].damage, playerGangwarData[gangwarDatas[area][winner][i]].kills, gangwarGangShort[winnergang] }
				playerGangwarData[gangwarDatas[area][winner][i]] = nil
			else
				damagearray[2][gangwarDatas[area][winner][i]] = { 0, 0, gangwarGangShort[winnergang] }
			end
		end
	end
	if gangwarDatas[area][winner.."alive"] and gangwarDatas[area][winner.."alive"][1] then
		for i=1, #gangwarDatas[area][winner.."alive"] do
			local player = getPlayerFromName ( gangwarDatas[area][winner.."alive"][i] )
			if isElement ( player ) then
				if isTimer ( preparationtimer[player] ) then
					killTimer ( preparationtimer[player] )
				end
				if tdsGetElementData ( player, "lobby" ) == "gangwar" then
					spawnPlayer ( player, wx, wy, wz, wrot, wskin, 0, gangwarLobbyDimension, wteam )
					setElementVisibleTo ( playerblips[player], wteam, true )
					takeAllWeapons ( player )
					setElementHealth ( player, 100 )
					setPedArmor ( player, 100 )
					tdsSetElementData ( player, "status", "gangwar" )
				end
			end
		end
	end


	local lx, ly, lz, lrot = gangwarGangData[loserID]["spawn"]["x"], gangwarGangData[loserID]["spawn"]["y"], gangwarGangData[loserID]["spawn"]["z"], gangwarGangData[loserID]["spawn"]["rot"]
	local lskin, lteam = gangwarGangData[loserID]["skin"], teams[losergang] 
	if gangwarDatas[area][loser] and gangwarDatas[area][loser][1] then
		for i=1, #gangwarDatas[area][loser] do
			local player = getPlayerFromName ( gangwarDatas[area][loser][i] )
			if isElement ( player ) and tdsGetElementData ( player, "GangwarVerloren" ) and tdsGetElementData ( player, "Punkte" ) then
				if isTimer ( preparationtimer[player] ) then
					killTimer ( preparationtimer[player] )
				end
				if not testGangs[loserID] and not wasfungw then
					tdsSetElementData ( player, "GangwarVerloren", tdsGetElementData ( player, "GangwarVerloren" ) + 1 )
					--tdsSetElementData ( player, "Punkte", tdsGetElementData ( player, "Punkte" ) )
				end
				playersInGangwar[player] = nil
				removeEventHandler ( "onPlayerQuit", player, playerLeftGangwar )
				if tdsGetElementData ( player, "lobby" ) == "gangwar" then
					takeAllWeapons ( player )
					setElementHealth ( player, 100 )
					setPedArmor ( player, 100 )
					tdsSetElementData ( player, "status", "gangwar" )
					if getElementDimension ( player ) ~= 65534 then
						setElementDimension ( player, 65534 )
						setElementFrozen ( player, false )
						toggleAllControls ( player, true, true, false )
						setCameraTarget ( player )
						triggerClientEvent ( player, "onClientCameraSpectateStop", player )
					end
					triggerTo[#triggerTo+1] = { player, false }
				end
			else
				if not testGangs[loserID] and not wasfungw then
					playerLossesToSave[gangwarDatas[area][loser][i]] = ( playerLossesToSave[gangwarDatas[area][loser][i]] or 0 ) + 1
				end
			end
			if playerGangwarData[gangwarDatas[area][loser][i]] then
				damagearray[1][gangwarDatas[area][loser][i]] = { playerGangwarData[gangwarDatas[area][loser][i]].damage, playerGangwarData[gangwarDatas[area][loser][i]].kills, gangwarGangShort[losergang] }
				playerGangwarData[gangwarDatas[area][loser][i]] = nil
			else
				damagearray[1][gangwarDatas[area][loser][i]] = { 0, 0, gangwarGangShort[losergang] }
			end
		end
	end
	if gangwarDatas[area][loser.."alive"] and gangwarDatas[area][loser.."alive"][1] then
		for i=1, #gangwarDatas[area][loser.."alive"] do
			local player = getPlayerFromName ( gangwarDatas[area][loser.."alive"][i] )
			if isElement ( player ) then
				if isTimer ( preparationtimer[player] ) then
					killTimer ( preparationtimer[player] )
				end
				if tdsGetElementData ( player, "lobby" ) == "gangwar" then
					spawnPlayer ( player, lx, ly, lz, lrot, lskin, 0, gangwarLobbyDimension, lteam )
					setElementVisibleTo ( playerblips[player], lteam, true )
					tdsSetElementData ( player, "status", "gangwar" )
					takeAllWeapons ( player )
					setElementHealth ( player, 100 )
					setPedArmor ( player, 100 )
				end
			end
		end
	end

	for i=1, #triggerTo do
		triggerClientEvent ( triggerTo[i][1], "gangwarStoppedHUD", triggerTo[i][1], triggerTo[i][2] )
		triggerLatentClientEvent ( triggerTo[i][1], "syncDamagesAfterGangwar", 50000, false, triggerTo[i][1], damagearray )
	end


	if not wasfungw then
		gangwarGangData[winnerID].wins = gangwarGangData[winnerID].wins + 1
		gangwarGangData[loserID].losses = gangwarGangData[loserID].losses + 1
		gangWinsChanged[winnerID] = true
		gangLossesChanged[loserID] = true

		if not testGangs[attackerID] and not testGangs[defenderID] then
			gangAreaDatas[area].money = areamoneysteps
		end

		if attackerwon then
			areaThingsOwner[gangAreaDatas[area].pickupcol] = attackerID
			gangAreaDatas[area].owner = attackergang
			gangAreaDatas[area].r, gangAreaDatas[area].g, gangAreaDatas[area].b = unpack ( TEAM_COLORS[teams[attackergang]] )
			setRadarAreaColor ( area, gangAreaDatas[area].r, gangAreaDatas[area].g, gangAreaDatas[area].b, 180 )
		end
	end

	gangwarDatas[area] = nil
	for pname, array in pairs ( playerGangwarData ) do
		if not gangwarDatas[array.area] then
			playerGangwarData[pname] = nil
		end
	end

	if attackerwon then
		dbExec ( handler, "INSERT INTO Gangwars ( GebieteID, AttackerID, VerteidigerID, Erobert, Sekunden ) VALUES ( ?, ?, ?, ?, ? )", gangAreaDatas[area].id, attackerID, defenderID, 1, getRealTime().timestamp )
		if not testGangs[attackerID] and not wasfungw then
			dbExec ( handler, "UPDATE Gebiete SET BesitzerGang = ?, Angegriffen = ?, Geld = ? WHERE ID = ?", attackerID, 1, areamoneysteps, gangAreaDatas[area].id )
		end
	else
		dbExec ( handler, "INSERT INTO Gangwars ( GebieteID, AttackerID, VerteidigerID, Erobert, Sekunden ) VALUES ( ?, ?, ?, ?, ? )", gangAreaDatas[area].id, attackerID, defenderID, 0, getRealTime().timestamp )
		if not testGangs[attackerID] and not wasfungw then
			dbExec ( handler, "UPDATE Gebiete SET Angegriffen = ?, Geld = ? WHERE ID = ?", 1, areamoneysteps, gangAreaDatas[area].id )
		end
	end

end


funcs.checkIfPickupIsOccupied = function ( area, progress )
	local col = gangwarDatas[area].clonepickupcol
	local playersincol = getElementsWithinColShape ( col, "player" )
	local someoneisingangwarcol = false
	for i=1, #playersincol do
		if getElementDimension ( playersincol[i] ) == gangwarDatas[area].dimension and not isPedDead ( playersincol[i] ) then
			local playername = getPlayerName ( playersincol[i] )
			if playerGangwarData[playername] and playerGangwarData[playername].attacker and playerGangwarData[playername].area == area then  
				someoneisingangwarcol = true
				break
			end
		end
	end
	if not someoneisingangwarcol then
		if progress < 5 then
			if gangwarDatas[area].attackeralive and gangwarDatas[area].attackeralive[1] then
				for i=1, #gangwarDatas[area].attackeralive do
					local player = getPlayerFromName ( gangwarDatas[area].attackeralive[i] )
					if isElement ( player ) then
						outputInformation ( player, langtext[tdsGetElementData(player,"language")]["nooneisatdeathskull"].." - ".. ( gwpickupseconds - progress*(gwpickupchecktime/1000) ) .." "..langtext[tdsGetElementData(player,"language")]["seconds"], "error" )
					end
				end
			end
			gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, progress + 1 )
		else
			clearGangwar ( area, false )
		end
	else
		if gangwarDatas[area].attackeralive and gangwarDatas[area].attackeralive[1] then
			for i=1, #gangwarDatas[area].attackeralive do
				local player = getPlayerFromName ( gangwarDatas[area].attackeralive[i] )
				if isElement ( player ) then
					outputInformation ( player, langtext[tdsGetElementData(player,"language")]["deathskulloccupiedagain"], "success" )
				end
			end
		end
	end
end


local function playerWentInGangwarCol ( element, dim )
	if dim and getElementType ( element ) == "player" then
		local pname = getPlayerName ( element )
		if playerGangwarData[pname] and playerGangwarData[pname].attacker then
			local area = playerGangwarData[pname].area
			if isTimer ( gangwarDatas[area].checktimer ) then
				killTimer ( gangwarDatas[area].checktimer )
				if gangwarDatas[area].attackeralive then
					for i=1, #gangwarDatas[area].attackeralive do
						local player = getPlayerFromName ( gangwarDatas[area].attackeralive[i] )
						if isElement ( player ) then
							outputInformation ( player, langtext[tdsGetElementData(player,"language")]["deathskulloccupiedagain"], "success" )
						end
					end
				end
			end
		end
	end
end


local function playerWentOutOfGangwarCol ( element, dim )
	if dim and getElementType ( element ) == "player" then
		local pname = getPlayerName ( element )
		if playerGangwarData[pname] and playerGangwarData[pname].attacker then
			local area = playerGangwarData[pname].area
			local col = gangwarDatas[area].clonepickupcol
			local playersincol = getElementsWithinColShape ( col, "player" ) 
			local someoneisingangwarcol = false
			for i=1, #playersincol do
				if getElementDimension ( playersincol[i] ) == gangwarDatas[area].dimension and not isPedDead ( playersincol[i] ) then
					local playername = getPlayerName ( playersincol[i] )
					if playerGangwarData[playername] and playerGangwarData[playername].attacker and playerGangwarData[playername].area == area then  
						someoneisingangwarcol = true
						break
					end
				end
			end
			if not someoneisingangwarcol then
				if not isTimer ( gangwarDatas[area].checktimer ) then
					if gangwarDatas[area].attackeralive and gangwarDatas[area].attackeralive[1] then
						for i=1, #gangwarDatas[area].attackeralive do
							local player = getPlayerFromName ( gangwarDatas[area].attackeralive[i] )
							if isElement ( player ) then
								outputInformation ( player, langtext[tdsGetElementData(player,"language")]["nooneisatdeathskull"].." - "..gwpickupseconds.." "..langtext[tdsGetElementData(player,"language")]["seconds"], "error" )
							end
						end
					end
					gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, 1 )
				end
			end
		end
	end 
end


function funcs.acceptGangwarJoinInvitation ( player )
	if tdsGetElementData ( player, "lobby" ) == "gangwar" then
		local gang = tdsGetElementData ( player, "gangwarGang" )
		if gang then
			local area = gangsInGangwar[gang] or gangsInGangwarPreparation[gang]
			if area and gangwarDatas[area] and ( gangwarDatas[area].joinallowed or getTickCount() - gangwarDatas[area].starttick < 60*1000 ) then
				local x, y, z = getElementPosition ( player )
				local px, py, pz = getElementPosition ( gangAreaDatas[area].pickup )
				if getDistanceBetweenPoints3D ( x, y, z, px, py, pz ) >= 200 then
					local side = gangwarDatas[area].attackergang == gang and "attacker" or "defender"
					local amountingw = #gangwarDatas[area][side]
					if amountingw < 2 then 
						setPlayerInGangwar ( player, area, "started", side == "attacker" )
					else
						local opp = side == "attacker" and gangwarDatas[area].defendergang or gangwarDatas[area].attackergang
						local oppshort = gangwarGangShort[opp]
						if playersinteam["gangwar"][oppshort] and playersinteam["gangwar"][oppshort][1] then
							for i=#playersinteam["gangwar"][oppshort], 1, -1 do
								if not isElement ( playersinteam["gangwar"][oppshort][i] ) or not getPlayerName ( playersinteam["gangwar"][oppshort][i] ) or tdsGetElementData ( playersinteam["gangwar"][oppshort][i], "lobby" ) ~= "gangwar" then
									table.remove ( playersinteam["gangwar"][oppshort], i )
								end 
							end
						end
						local amountinoppteam = playersinteam["gangwar"][oppshort] and #playersinteam["gangwar"][oppshort] or 0
						if side == "attacker" and amountinoppteam >= amountingw or amountinoppteam > amountingw then
							setPlayerInGangwar ( player, area, "started", side == "attacker" )
						elseif side == "attacker" then
							outputInformation ( player, langtext[tdsGetElementData(player,"language")]["only1morethanowner"], "error" )
							return
						else
							outputInformation ( player, langtext[tdsGetElementData(player,"language")]["cantbemorethanattacker"], "error" )
							return
						end
					end
				else
					outputInformation ( player, langtext[tdsGetElementData(player,"language")]["havetobeawayfromdeathskull"].." (200)", "error" )
					return
				end
			elseif area and gangwarDatas[area] then
				outputInformation ( player, langtext[tdsGetElementData(player,"language")]["youcantjoinanymore"], "error" )
			else
				outputInformation ( player, langtext[tdsGetElementData(player,"language")]["gangwaralreadyover"], "error" )
			end
		end
		playeraskedtojoin[player] = nil
		unbindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
		unbindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
		removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
		triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
	else
		outputInformation ( player, langtext[tdsGetElementData(player,"language")]["acceptinvitationingangwarlobby"], "error" )
	end
end


funcs.rejectGangwarJoinInvitation = function ( player )
	local isoffline = not isElement ( player )
	local player = isElement ( player ) and player or source
	playeraskedtojoin[player] = nil
	if not isoffline then
		unbindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
		unbindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
		removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
		triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
	end
	local gang = tdsGetElementData ( player, "gangwarGang" )
	if gang then
		local gangshort = gangwarGangShort[gang]
		local pname = getPlayerName ( player )
		if playerOnlineInGang[gangshort] and playerOnlineInGang[gangshort][1] then
			for i=#playerOnlineInGang[gangshort], 1, -1 do
				if isElement ( playerOnlineInGang[gangshort][i] ) then
					outputInformation ( playerOnlineInGang[gangshort][i], pname..langtext[tdsGetElementData(playerOnlineInGang[gangshort][i],"language")]["rejectedthegangwarinvitation"], "error" )
				else
					table.remove ( playerOnlineInGang[gangshort], i )
				end
			end
		end
	end
end


local function checkJoinAllowed ( area )
	if gangwarDatas[area] then
		if #gangwarDatas[area]["defenderalive"] == 0 and not gangwarDatas[area].joinallowed then
			if isTimer ( gangwarDatas[area].wintimer ) then
				killTimer ( gangwarDatas[area].wintimer )
			end
			clearGangwar ( area, true )
		end
	end
end


local function startGangwar ( area )
	if gangwarDatas[area] and not isRadarAreaFlashing ( gangwarDatas[area].clonearea ) then
		setRadarAreaFlashing ( area, true )
		setRadarAreaFlashing ( gangwarDatas[area].clonearea, true )
		local playersingangwarcol = getElementsWithinColShape ( gangwarDatas[area].clonepickupcol, "player" )
		local someoneisingangwarcol = false
		for i=1, #playersingangwarcol do
			if getElementDimension ( playersingangwarcol[i] ) == gangwarDatas[area].dimension and not isPedDead ( playersingangwarcol[i] ) then
				local pname = getPlayerName ( playersingangwarcol[i] )
				if playerGangwarData[pname] and playerGangwarData[pname].attacker then
					someoneisingangwarcol = true
					break
				end
			end
		end
		if not someoneisingangwarcol then
			if gangwarDatas[area].attackeralive and gangwarDatas[area].attackeralive[1] then
				for i=1, #gangwarDatas[area].attackeralive  do
					local player = getPlayerFromName ( gangwarDatas[area].attackeralive[i] )
					if isElement ( player ) then
						outputInformation ( player, langtext[tdsGetElementData(player,"language")]["nooneisatdeathskull"].." - "..gwpickupseconds.." "..langtext[tdsGetElementData(player,"language")]["seconds"], "error" )
					end
				end
			end
			gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, 1 )
		end


		local gwinfo = { name = gangAreaDatas[area].name, owner = gangwarDatas[area].defendergang.." [".. gangwarGangShort[gangwarDatas[area].defendergang].."]", pickup = gangAreaDatas[area].pickup, isfungw = infungangwar[area] }
		local gang = gangwarDatas[area].attackergang
		local short = gangwarGangShort[gang]
		if playerOnlineInGang[short] and playerOnlineInGang[short][1] then
			for i=#playerOnlineInGang[short], 1, -1 do
				if isElement ( playerOnlineInGang[short][i] ) then
					local player = playerOnlineInGang[short][i]
					if not playerGangwarData[getPlayerName(player)] or ( not playerGangwarData[getPlayerName(player)].gotdamage and playerGangwarData[getPlayerName(player)].offline ) then
						triggerLatentClientEvent ( player, "showGangwarJoinDraw", 50000, false, player, gwinfo )
						unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
						unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
						unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
						unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
						removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
						addEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
						bindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
						bindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
						playeraskedtojoin[player] = true
					end
				else
					table.remove ( playerOnlineInGang[short], i )
				end
			end
		end

		gwinfo = { name = gangAreaDatas[area].name, attacker = gangwarDatas[area].attackergang.." [".. gangwarGangShort[gangwarDatas[area].attackergang].."]", pickup = gangAreaDatas[area].pickup, isfungw = infungangwar[area] }
		gang = gangwarDatas[area].defendergang
		short = gangwarGangShort[gang]
		if playerOnlineInGang[short] and playerOnlineInGang[short][1] then
			for i=#playerOnlineInGang[short], 1, -1 do
				if isElement ( playerOnlineInGang[short][i] ) then
					local player = playerOnlineInGang[short][i]
					if not playerGangwarData[getPlayerName(player)] then
						triggerLatentClientEvent ( player, "showGangwarJoinDraw", 50000, false, player, gwinfo )
						unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
						unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
						removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
						addEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
						bindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
						bindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
						playeraskedtojoin[player] = true
					end
				else
					table.remove ( playerOnlineInGang[short], i )
				end
			end
		end

		if gangwarDatas[area].attacker and gangwarDatas[area].attacker[1] then
			for i=1, #gangwarDatas[area].attacker do
				if playerGangwarData[gangwarDatas[area].attacker[i]] then
					playerGangwarData[gangwarDatas[area].attacker[i]].status = "started"
				end
			end
		end
			

		addEventHandler ( "onColShapeHit", gangwarDatas[area].clonepickupcol, playerWentInGangwarCol )
		addEventHandler ( "onColShapeLeave", gangwarDatas[area].clonepickupcol, playerWentOutOfGangwarCol )
		gangsInGangwarPreparation[gangwarDatas[area].attackergang] = nil
		gangsInGangwar[gangwarDatas[area].attackergang] = area
		gangsInGangwarPreparation[gangwarDatas[area].defendergang] = nil
		gangsInGangwar[gangwarDatas[area].defendergang] = area
		gangwarDatas[area].status = "started"
		gangwarDatas[area].starttick = getTickCount()
		gangwarDatas[area].endtick = getTickCount() + 15*60*1000
		gangwarDatas[area].wintimer = setTimer ( clearGangwar, 15*60*1000, 1, area, true )
		gangwarDatas[area].joinallowedtimer = setTimer ( checkJoinAllowed, 60*1000, 1, area )
		local attackerID = gangwarGangIDs[gangwarDatas[area]["attackergang"]] 
		local defenderID = gangwarGangIDs[gangwarDatas[area]["defendergang"]] 

		if gangwarDatas[area]["attackeralive"] and gangwarDatas[area]["attackeralive"][1] then
			for i=1, #gangwarDatas[area]["attackeralive"] do
				local player = getPlayerFromName ( gangwarDatas[area]["attackeralive"][i] )
				if isElement ( player ) then
					triggerClientEvent ( player, "updateGangwarTime", player, gangwarDatas[area].endtick - getTickCount()  )
					givePlayerWeaponsAndRest ( player )
					tdsSetElementData ( player, "status", "playing" )
				end
			end
		end

		for i=1, #gangwarDatas[area].vehicles do
			if isElement ( gangwarDatas[area].vehicles[i][1] ) then
				setVehicleLocked ( gangwarDatas[area].vehicles[i][1], true )
				setElementFrozen ( gangwarDatas[area].vehicles[i][1], true )
				setVehicleDoorsUndamageable ( gangwarDatas[area].vehicles[i][1], true )
				for _, playerinveh in pairs ( getVehicleOccupants ( gangwarDatas[area].vehicles[i][1] ) ) do
					removePedFromVehicle ( playerinveh )
				end
				removeEventHandler ( "onElementClicked", gangwarDatas[area].vehicles[i][1], clickedOnVehicleInGangwar )
			end
		end

		setTimer ( function ( ) 
			if gangwarDatas[area]["attackeralive"] and gangwarDatas[area]["attackeralive"][1] then
				for i=1, #gangwarDatas[area]["attackeralive"] do
					local player = getPlayerFromName ( gangwarDatas[area]["attackeralive"][i] )
					if isElement ( player ) then
						local x, y, z = getElementPosition ( player )
						setElementPosition ( player, x, y, z )
					end
				end
			end
		end, 1000, 1 )
	end
end


funcs.acceptGangwarPreparationInvitation = function ( player )
	if tdsGetElementData ( player, "lobby" ) == "gangwar" then
		local gang = tdsGetElementData ( player, "gangwarGang" )
		if gang then
			local area = gangsInGangwarPreparation[gang]
			if area and gangwarDatas[area] and gangwarDatas[area].attackergang == gang then
				local amountingw = #gangwarDatas[area]["attacker"]
				if amountingw < 2 then 
					setPlayerInGangwar ( player, area, "preparation", true )
				else
					local opp = gangwarDatas[area].defendergang
					local short = gangwarGangShort[opp]
					if playersinteam["gangwar"][short] and playersinteam["gangwar"][short][1] then
						for i=#playersinteam["gangwar"][short], 1, -1 do
							if not isElement ( playersinteam["gangwar"][short][i] ) or not getPlayerName ( playersinteam["gangwar"][short][i] ) or tdsGetElementData ( playersinteam["gangwar"][short][i], "lobby" ) ~= "gangwar" then
								table.remove ( playersinteam["gangwar"][short], i )
							end 
						end
					end
					local amountinoppteam = playersinteam["gangwar"][short] and #playersinteam["gangwar"][short] or 0
					if amountinoppteam >= amountingw then
						setPlayerInGangwar ( player, area, "preparation", true )
					else
						outputInformation ( player, langtext[tdsGetElementData(player,"language")]["only1morethanowner"], "error" )
						return
					end
				end
			end
		end
		unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
		unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
		removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
		triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
		playeraskedtojoin[player] = nil
	else
		outputInformation ( player, langtext[tdsGetElementData(player,"language")]["acceptinvitationingangwarlobby"], "error" )
	end
end


funcs.rejectGangwarPreparationInvitation = function ( player )
	local isoffline = not isElement ( player )
	local player = isElement ( player ) and player or source
	playeraskedtojoin[player] = nil
	if not isoffline then
		unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
		unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
		removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
		triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
	end
	local gang = tdsGetElementData ( player, "gangwarGang" )
	if gang then
		local pname = getPlayerName ( player )
		local gangshort = gangwarGangShort[gang]
		if playerOnlineInGang[gangshort] and playerOnlineInGang[gangshort][1] then
			for i=#playerOnlineInGang[gangshort], 1, -1 do
				if isElement ( playerOnlineInGang[gangshort][i] ) then
					outputInformation ( playerOnlineInGang[gangshort][i], pname..langtext[tdsGetElementData(playerOnlineInGang[gangshort][i],"language")]["rejectedthegangwarinvitation"], "error" )
				else
					table.remove ( playerOnlineInGang[gangshort], i )
				end
			end
		end
	end
end


local function startGangwarPreparation ( player, area, gang, owner, isfungw )
	if not isfungw then
		if infungangwar[area] then 
			clearGangwar ( area, false )
		end
		if infungangwar[gang] then
			local area = gangsInGangwarPreparation[gang] or gangsInGangwar[gang]
			if area then
				clearGangwar ( area, false )
			end
		end
		if infungangwar[owner] then
			local area = gangsInGangwarPreparation[owner] or gangsInGangwar[owner]
			if area then
				clearGangwar ( area, false )
			end
		end
	end
	unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
	unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
	unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
	triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
	gangsInGangwarPreparation[gang] = area
	gangsInGangwarPreparation[owner] = area
	if isfungw then
		infungangwar[gang] = true 
		infungangwar[owner] = true 
		infungangwar[area] = true 
	else
		infungangwar[gang] = false 
		infungangwar[owner] = false 
		infungangwar[area] = false
	end
	local dim = getFreeDimension ()
	gangwarDimensions[dim] = true
	gangwarDatas[area] = { 
		dimension = dim, 
		status = "preparation", 
		attackergang = gang, 
		defendergang = owner, 
		attacker = {}, 
		defender = {}, 
		attackeralive = {}, 
		defenderalive = {}, 
		starttick = getTickCount(),
		endtick = getTickCount()+3*60*1000,	
		joinallowed = true,
		vehicles = {}
	}
	playersBecameDamage[area] = {}
	playersGaveDamage[area] = {} 
	if gangwarGangData[gangwarGangIDs[gang]].skin1 == 0 or gangwarGangData[gangwarGangIDs[gang]].skin1 ~= gangwarGangData[gangwarGangIDs[owner]].skin1 then
		skinids[gang] = gangwarGangData[gangwarGangIDs[gang]].skin1
		skinids[owner] = gangwarGangData[gangwarGangIDs[owner]].skin1
	else
		local rnd = math.random ( 2 )
		if rnd == 1 then
			skinids[gang] = gangwarGangData[gangwarGangIDs[gang]].skin1
			skinids[owner] = gangwarGangData[gangwarGangIDs[owner]].skin2
		else
			skinids[gang] = gangwarGangData[gangwarGangIDs[gang]].skin2
			skinids[owner] = gangwarGangData[gangwarGangIDs[owner]].skin1
		end
	end

	gangwarDatas[area].clonearea = cloneElement ( area )
	gangwarDatas[area].clonepickup = cloneElement ( gangAreaDatas[area].pickup )
	local pickupx, pickupy, pickupz = getElementPosition ( gangwarDatas[area].clonepickup )
	gangwarDatas[area].clonepickupcol = createColSphere ( pickupx, pickupy, pickupz, 15 )
	setElementDimension ( gangwarDatas[area].clonearea, dim )
	setElementDimension ( gangwarDatas[area].clonepickup, dim )
	setElementDimension ( gangwarDatas[area].clonepickupcol, dim )
	for pname, array in pairs ( playerGangwarData ) do
		if not gangwarDatas[array.area] then
			playerGangwarData[pname] = nil
		end
	end
	setPlayerInGangwar ( player, area, "preparation", true, true )
	local ownershort = gangwarGangShort[owner]
	local gwinfo = { name = gangAreaDatas[area].name, owner = owner.." [".. ownershort.."]" }
	local gangshort = gangwarGangShort[gang]
	for i=#playerOnlineInGang[gangshort], 1, -1 do
		if isElement ( playerOnlineInGang[gangshort][i] ) then
			if playerOnlineInGang[gangshort][i] ~= player then
				triggerLatentClientEvent ( playerOnlineInGang[gangshort][i], "showGangwarPreparationDraw", 50000, false, player, gwinfo )
				addEventHandler ( "onPlayerQuit", playerOnlineInGang[gangshort][i], funcs.rejectGangwarPreparationInvitation )
				unbindKey ( playerOnlineInGang[gangshort][i], "J", "down", funcs.acceptFunGangwarInvitation )
				unbindKey ( playerOnlineInGang[gangshort][i], "N", "down", funcs.rejectFunGangwarInvitation )
				bindKey ( playerOnlineInGang[gangshort][i], "J", "down", funcs.acceptGangwarPreparationInvitation )
				bindKey ( playerOnlineInGang[gangshort][i], "N", "down", funcs.rejectGangwarPreparationInvitation )
				playeraskedtojoin[playerOnlineInGang[gangshort][i]] = true
			end
		else
			table.remove ( playerOnlineInGang[gangshort], i )
		end
	end

	if playerOnlineInGang[ownershort] and playerOnlineInGang[ownershort][1] then
		for i=#playerOnlineInGang[ownershort], 1, -1 do 
			if isElement ( playerOnlineInGang[ownershort][i] ) then
				outputInformation ( playerOnlineInGang[ownershort][i], langtext[tdsGetElementData(playerOnlineInGang[ownershort][i],"language")]["youaregettingattacked"], "info" )
			else
				table.remove ( playerOnlineInGang[ownershort], i )
			end
		end
	end
	preparationtimer[player] = { setTimer ( startGangwar, 3*60*1000, 1, area ), area }	-- Zeit anpassbar machen		-- CHANGE 3*60*1000
end


funcs.checkGangwarRequirement = function ( player, _, _, col, area )
	--local gang = tdsGetElementData ( player, "gangwarGang" )
	--local gangID = tdsGetElementData ( player, "gangwarGangID" )
	--if gang and areaThingsOwner[col] ~= gangID then
	--	if tdsGetElementData ( player, "gangwarGangRang" ) >= 2 then		-- Anpassbar machen
	--		if not gangsInGangwar[gang] or infungangwar[gang] then
	--			if not gangsInGangwarPreparation[gang] or infungangwar[gang] then
	--				local owner = gangAreaDatas[area].owner
					--if getTickCount() >= gangAreaDatas[area].lastattack + 1*60*60*1000 then
					--	if areaThingsOwner[col] ~= 0 then
					--		if not antiattackspam[gangID] or antiattackspam[gangID] + 10000 <= getTickCount() then	
					--			if not gangsInGangwar[owner] or infungangwar[owner] then
					--				if not gangsInGangwarPreparation[owner] or infungangwar[owner] then
					--					if not gangwarDatas[area] or infungangwar[area] then
					--						if isElementWithinColShape ( player, gangAreaDatas[area].pickupcol ) then
					--							if tdsGetElementData ( player, "lobby" ) == "gangwar" then
					--								local gangshort = gangwarGangShort[gang]
					--								if playerOnlineInGang[gangshort] and #playerOnlineInGang[gangshort] >= 2 then		-- CHANGE 2
					--									local ownergangshort = gangwarGangShort[owner]
					--									if playerOnlineInGang[ownergangshort] and #playerOnlineInGang[ownergangshort] >= 2 then 
					--										if gangwarsDoneByGang[ownergangshort] and gangwarsDoneByGang[ownergangshort] >= 4 then
					--											if playersinteam["gangwar"][ownergangshort] and playersinteam["gangwar"][ownergangshort][1] then
					--												for i=#playersinteam["gangwar"][ownergangshort], 1, -1 do
					--													if not isElement ( playersinteam["gangwar"][ownergangshort][i] ) or not getPlayerName ( playersinteam["gangwar"][ownergangshort][i] ) or tdsGetElementData ( playersinteam["gangwar"][ownergangshort][i], "lobby" ) ~= "gangwar" then
					--														table.remove ( playersinteam["gangwar"][ownergangshort], i )
					--													end 
					--												end
					--											end	
					--											if not playersinteam["gangwar"][ownergangshort] or #playersinteam["gangwar"][ownergangshort] < 2 then
					--												outputInformation ( player, langtext[tdsGetElementData(player,"language")]["gangdidenoughgwfortodaynoattackinotherlobby"], "error" )
					--												return
					--											end
					--										end
					--										startGangwarPreparation ( player, area, gang, owner )
					--									else
					--										outputInformation ( player, langtext[tdsGetElementData(player,"language")]["oppsnotenoughforgangwar"], "error" )
					--									end
					--								else
					--									outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notenoughforgangwar"], "error" )
					--								end
					--							else
					--								outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notingangwarlobby"], "error" )
					--							end
					--						else
					--							outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notatdeathskull"], "error" )
					--						end
					--					else
					--						outputInformation ( player, langtext[tdsGetElementData(player,"language")]["thisareaisin"][1] .. ( gangwarDatas[area].status == "preparation" and langtext[tdsGetElementData(player,"language")]["thisareaisin"][2] or langtext[tdsGetElementData(player,"language")]["thisareaisin"][3] ) ..".", "error" )
					---					end
					--				else
					--					outputInformation ( player, langtext[tdsGetElementData(player,"language")]["owneringwpreparation"], "error" )
					--				end
					--			else
					--				outputInformation ( player, langtext[tdsGetElementData(player,"language")]["owneringwattack"], "error" )
					--			end
					--		else
					--			outputInformation ( player, langtext[tdsGetElementData(player,"language")]["antiattackspam"], "error" )
					--		end
					--	elseif not gangareastook[gangID] or testGangs[gangID] and gangareastook[gangID] < 1 or gangareastook[gangID] < 4 then
					--		areaThingsOwner[gangAreaDatas[area].pickupcol] = gangID
					--		gangAreaDatas[area].owner = gang
							gangAreaDatas[area].r, gangAreaDatas[area].g, gangAreaDatas[area].b = unpack ( TEAM_COLORS[teams[gang]] )
							setRadarAreaColor ( area, gangAreaDatas[area].r, gangAreaDatas[area].g, gangAreaDatas[area].b, 180 )
					--		if not testGangs[gangID] then
					--			dbExec ( handler, "UPDATE Gebiete SET BesitzerGang = ?, Angegriffen = ? WHERE ID = ?", gangID, 1, gangAreaDatas[area].id )
					--		end
					--		gangareastook[gangID] = ( gangareastook[gangID] or 0 ) + 1
					--		outputInformation ( player, langtext[tdsGetElementData(player,"language")]["thatwasyournumberattack"][1]..gangareastook[gangID]..langtext[tdsGetElementData(player,"language")]["thatwasyournumberattack"][2]..( testGangs[gangID] and 1 or 4 )..".", "info" )
					--	elseif testGangs[gangID] then
					--		outputInformation ( player, langtext[tdsGetElementData(player,"language")]["testgangmaxfreearea"], "error" )
					--	else
					--		outputInformation ( player, langtext[tdsGetElementData(player,"language")]["ganggotallfreeareas"], "error" )
					--	end
					--else
					--	outputInformation ( player, langtext[tdsGetElementData(player,"language")]["areastillgotcooldown"], "error" )
					--end
				--else
				--	outputInformation ( player, langtext[tdsGetElementData(player,"language")]["yourgangisalreadyinpreparation"], "error" )
				--end
			--else
			--	outputInformation ( player, langtext[tdsGetElementData(player,"language")]["yourgangisalreadyingangwar"], "error" )
			--end
		--else
			--outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notallowed"], "error" )
			--tdsRemoveElementData ( player, "inGangArea" )
			unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
			unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
			unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
			triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
		--end
	--else
		--tdsRemoveElementData ( player, "inGangArea" )
		--unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
		--unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
		--unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
		--triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )	
	--end
--end


funcs.rejectGangwarAttack = function ( player )
	unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
	unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
	unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
	triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
end


function funcs.checkForFunGangwar ( player, _, _, col, area )
	--local gang = tdsGetElementData ( player, "gangwarGang" )
	--local gangID = tdsGetElementData ( player, "gangwarGangID" )
	--if gang then
		--if tdsGetElementData ( player, "gangwarGangRang" ) >= 1 then
			--if not antiattackspam[gangID] or antiattackspam[gangID] + 10000 <= getTickCount() then		BRAUCHEN WIR NICHT	
			--	if not gangsInGangwar[gang] then
			--		if not gangsInGangwarPreparation[gang] then
			--			if not gangwarDatas[area] then
							if isElementWithinColShape ( player, gangAreaDatas[area].pickupcol ) then
			--					if tdsGetElementData ( player, "lobby" ) == "gangwar" then
			--						local gangshort = gangwarGangShort[gang]
			--						if playerOnlineInGang[gangshort] then		
										local gangsavailable = {}
										for short, array in pairs ( playerOnlineInGang ) do
											local name = gangwarShortGang[short] 
											if name ~= gang then
												if not gangsInGangwar[name] then
													if not gangsInGangwarPreparation[name] then
														local amount = #array
														if amount > 0 then
															gangsavailable[#gangsavailable+1] = { name, short, amount } 
														end
													end
												end
											end
										end
										if gangsavailable[1] then
											askedforfungw[player] = area
											triggerClientEvent ( player, "showGangsAvailableForFunGangwar", player, gangsavailable )
											unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
											unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
											unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
										else
											outputInformation ( player, langtext[tdsGetElementData(player,"language")]["nogangavailableforthat"], "error" )	
										end
									end
								else
									outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notingangwarlobby"], "error" )
								end
							else
								outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notatdeathskull"], "error" )
							end
						else
							outputInformation ( player, langtext[tdsGetElementData(player,"language")]["thisareaisin"][1] .. ( gangwarDatas[area].status == "preparation" and langtext[tdsGetElementData(player,"language")]["thisareaisin"][2] or langtext[tdsGetElementData(player,"language")]["thisareaisin"][3] ) ..".", "error" )
						end	
					else
						outputInformation ( player, langtext[tdsGetElementData(player,"language")]["yourgangisalreadyinpreparation"], "error" )
					end
				else
					outputInformation ( player, langtext[tdsGetElementData(player,"language")]["yourgangisalreadyingangwar"], "error" )
				end
			else
				outputInformation ( player, langtext[tdsGetElementData(player,"language")]["antiattackspam"], "error" )
			end
		else
			outputInformation ( player, langtext[tdsGetElementData(player,"language")]["notallowed"], "error" )
			tdsRemoveElementData ( player, "inGangArea" )
			unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
			unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
			unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
			triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )
		end
	else
		tdsRemoveElementData ( player, "inGangArea" )
		unbindKey ( player, "J", "down", funcs.checkGangwarRequirement )
		unbindKey ( player, "N", "down", funcs.rejectGangwarAttack )
		unbindKey ( player, "L", "down", funcs.checkForFunGangwar )
		triggerClientEvent ( player, "removeGangwarAttackAskDraw", player )	
	end
end 


local function hitDeathPickupCol ( element, dim )
	if getElementType ( element ) == "player" and dim then
		if not playeraskedtojoin[element] then
			local gangid = tdsGetElementData ( element, "gangwarGangID" ) 
			if areaThingsOwner[source] ~= gangid and tdsGetElementData ( element, "gangwarGangRang" ) >= 2 then
				tdsSetElementData ( element, "inGangArea", areaByColshape[source] )
				local area = areaByColshape[source]
				local gwinfo = { 
						name = gangAreaDatas[area].name, 
						canattack = true, 
						owner = gangAreaDatas[area].owner.." [".. gangwarGangShort[gangAreaDatas[area].owner].."]",
						attacking = gangwarDatas[area] and true or false,
						milsecsleft = 1*60*60*1000 - ( getTickCount() - gangAreaDatas[area].lastattack ),
						pickup = gangAreaDatas[area].pickup,
				}
				if gangwarDatas[area] then
					gwinfo.canattack = false
				else
					bindKey ( element, "J", "down", funcs.checkGangwarRequirement, source, areaByColshape[source] )
					bindKey ( element, "N", "down", funcs.rejectGangwarAttack )
					bindKey ( element, "L", "down", funcs.checkForFunGangwar, source, areaByColshape[source] )
				end
				triggerLatentClientEvent ( element, "showGangwarAttackAskDraw", 50000, false, element, gwinfo )
			else
				local area = areaByColshape[source]
				local gwinfo = { 
						name = gangAreaDatas[area].name, 
						canattack = false, 
						owner = gangAreaDatas[area].owner.." [".. gangwarGangShort[gangAreaDatas[area].owner].."]",
						attacking = gangwarDatas[area] and true or false,
						milsecsleft = 1*60*60*1000 - ( getTickCount() - gangAreaDatas[area].lastattack ),
						pickup = gangAreaDatas[area].pickup,
					}
				triggerLatentClientEvent ( element, "showGangwarAttackAskDraw", 50000, false, element, gwinfo )
			end
		end
	end
end


local function leaveDeathPickupCol ( element, dim )
	if getElementType ( element ) == "player" and dim then
		if not playeraskedtojoin[element] then
			if tdsGetElementData ( element, "gangwarGangRang" ) >= 2 then
				tdsRemoveElementData ( element, "inGangArea" )
				unbindKey ( element, "J", "down", funcs.checkGangwarRequirement )
				unbindKey ( element, "N", "down", funcs.rejectGangwarAttack )
				unbindKey ( element, "L", "down", funcs.checkForFunGangwar )
				triggerClientEvent ( element, "removeGangwarAttackAskDraw", element )
			end
		end
	end
end


addEventHandler ( "onResourceStart", resourceRoot, function ( )
	dbQuery ( function ( query )
		local result = dbPoll ( query, 0 )
		if result and result[1] then
			for i=1, #result do
				if tonumber ( result[i]["Aktiviert"] ) == 1 then
					amountgangareas = amountgangareas + 1
					local name = result[i]["Name"]
					local x1, y1 = result[i]["X1"], result[i]["Y1"]
					local x2, y2 = result[i]["X2"], result[i]["Y2"]
					local dx, dy, dz = result[i]["X3"], result[i]["Y3"], result[i]["Z3"]
					local gangID = result[i]["BesitzerGang"]
					if not gangwarGangData[gangID] then
						gangID = 0
						dbExec ( handler, "UPDATE Gebiete SET BesitzerGang = ? WHERE ID = ?", gangID, result[i]["ID"] )
					end
					local ownergang = gangwarGangData[gangID] and gangwarGangData[gangID].name or "-"
					local r, g, b = 255, 255, 255
					if gangwarGangData[gangID] then
						r, g, b = gangwarGangData[gangID].r, gangwarGangData[gangID].g, gangwarGangData[gangID].b
					end
					local a = gangID == 0 and 255 or 150
					local radararea = createRadarArea ( x1, y1, x2-x1, y2-y1, r, g, b, a )
					setElementDimension ( radararea, gangwarLobbyDimension )
					local pickupcol = createColSphere ( dx, dy, dz, 15 )
					areaByColshape[pickupcol] = radararea
					areaThingsOwner[pickupcol] = gangID
					setElementDimension ( pickupcol, gangwarLobbyDimension )
					addEventHandler ( "onColShapeHit", pickupcol, hitDeathPickupCol )
					addEventHandler ( "onColShapeLeave", pickupcol, leaveDeathPickupCol )	
					local deathpickup = createPickup ( dx, dy, dz, 3, 1254, 50 )
					setElementDimension ( deathpickup, gangwarLobbyDimension )

					gangAreaDatas[radararea] = {
						id = result[i]["ID"],
						owner = ownergang,
						pickup = deathpickup,
						pickupcol = pickupcol,
						name = name,
						areacol = areacol,
						r = r, g = g, b = b,
						lastattack = -99999,
						money = result[i]["Geld"]
					}
				end
			end
		end
	end, handler, "SELECT * FROM Gebiete" )
end )


addEventHandler ( "onResourceStop", resourceRoot, function ( )
	local radarareas = getElementsByType ( "radararea" )
	for i=1, #radarareas do
		destroyElement ( radarareas[i] )
	end
end )


local function respawnPlayerInGangwarLobby ( player )
	if isElement ( player ) then
		local gangID = tdsGetElementData ( player, "gangwarGangID" )
		if gangID ~= 0 then
			local x, y, z, rot, skin, team = gangwarGangData[gangID]["spawn"]["x"], gangwarGangData[gangID]["spawn"]["y"], gangwarGangData[gangID]["spawn"]["z"], gangwarGangData[gangID]["spawn"]["rot"]
			local skin, team = gangwarGangData[gangID]["spawn"]["skin"], teams[tdsGetElementData(player,"gangwarGang")]
			local pname = getPlayerName ( player )
			if playerGangwarData[pname] and playerGangwarData[pname].died then
				local dim = getElementDimension ( player )
				spawnPlayer ( player, x, y, z, rot, skin, 0, dim, team )
				setElementFrozen ( player, true )
				toggleAllControls ( player, true, true, false )
			else
				spawnPlayer ( player, x, y, z, rot, skin, 0, gangwarLobbyDimension, team )
			end
			setElementVisibleTo ( playerblips[player], team, true )
			changePlayerMassByActivity ( player )
		else
			spawnPlayer ( player, 1481, -1763.6999511719, 18.799999237061, 0, 0, 0, gangwarLobbyDimension, teams["nogang"] )
			setElementVisibleTo ( playerblips[player], teams["nogang"], true )
			changePlayerMassByActivity ( player )
		end
		takeAllWeapons ( player )
		setElementHealth ( player, 100 )
		setPedArmor ( player, 100 )
	end
end


function gangwarPlayerDied ( player, attacker, damage )
	local pname = getPlayerName ( player )
	if playerGangwarData[pname] and not playerGangwarData[pname].died and not playerGangwarData[pname].offline then
		playerGangwarData[pname].died = true
		playerGangwarData[pname].gotdamage = true
		tdsSetElementData ( player, "status", "died" )
		local area = playerGangwarData[pname].area
		local gang = playerGangwarData[pname].attacker and gangwarDatas[area].attackergang or gangwarDatas[area].defendergang
		local side = playerGangwarData[pname].attacker and "attacker" or "defender"
		local amountchanged = false
		local alivechanged = false

		if isElement ( attacker ) then
			local attackername = getPlayerName ( attacker )
			playersBecameDamage[area][pname][attackername] = ( playersBecameDamage[area][pname][attackername] or 0 ) + damage
			if playerGangwarData[attackername] then
				playersGaveDamage[area][attackername][pname] = ( playersGaveDamage[area][attackername][pname] or 0 ) + damage
				playerGangwarData[attackername].kills = playerGangwarData[attackername].kills + 1
				playerGangwarData[attackername].damage = playerGangwarData[attackername].damage + damage
				playerGangwarData[attackername].gotdamage = true
				if playerGangwarData[pname].attacker ~= playerGangwarData[attackername].attacker then
					gangwarDatas[area].joinallowed = false
					drawEveryPastInvitation ( area )
				end
			end
			triggerClientEvent ( attacker, "giveDamageClient", attacker, ( damage or 0 ) )
			triggerClientEvent ( attacker, "giveKillClient", attacker )
		end

		if playerGangwarData[pname].status == "preparation" then
			playerGangwarData[pname].allowedtojoin = true
			if gangwarDatas[area][side] and gangwarDatas[area][side][1] then
				for i=1, #gangwarDatas[area][side] do
					if gangwarDatas[area][side][i] == pname then
						table.remove ( gangwarDatas[area][side], i )
						amountchanged = true
						break
					end
				end
			end
			if gangwarDatas[area][side.."alive"] and gangwarDatas[area][side.."alive"][1] then
				for i=1, #gangwarDatas[area][side.."alive"] do
					if gangwarDatas[area][side.."alive"][i] == pname then
						table.remove ( gangwarDatas[area][side.."alive"], i )
						alivechanged = true
						break
					end
				end
			end
		else
			playerGangwarData[pname].allowedtojoin = false
			if gangwarDatas[area][side.."alive"] and gangwarDatas[area][side.."alive"][1] then
				for i=#gangwarDatas[area][side.."alive"], 1, -1 do
					if gangwarDatas[area][side.."alive"][i] == pname then
						table.remove ( gangwarDatas[area][side.."alive"], i )
						alivechanged = true
					elseif not playerGangwarData[gangwarDatas[area][side.."alive"][i]] or playerGangwarData[gangwarDatas[area][side.."alive"][i]].offline then
						table.remove ( gangwarDatas[area][side.."alive"], i )
					end
				end
			end
			if gangwarDatas[area].status == "started" and #gangwarDatas[area][side.."alive"] == 0 then
				if side == "attacker" then
					if isTimer ( gangwarDatas[area].checktimer ) then
						killTimer ( gangwarDatas[area].checktimer )
					end
					gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, 4 )
				elseif side == "defender" and not gangwarDatas[area].joinallowed and getTickCount() - gangwarDatas[area].starttick > 60*1000 then
					if isTimer ( gangwarDatas[area].wintimer ) then
						killTimer ( gangwarDatas[area].wintimer )
					end
					gangwarDatas[area].wintimer = setTimer ( clearGangwar, 5*1000, 1, area, true )
				end
			elseif gangwarDatas[area].status == "started" and side == "attacker" then
				if not isTimer ( gangwarDatas[area].checktimer ) then
					gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, 1 )
				end
			end
		end
		triggerClientEvent ( player, "playerLeftGangwar", player, true )
		if amountchanged or alivechanged then
			if gangwarDatas[area][side.."alive"] and gangwarDatas[area][side.."alive"][1] then
				for i=1, #gangwarDatas[area][side.."alive"] do
					local thePlayer = getPlayerFromName ( gangwarDatas[area][side.."alive"][i] )
					if isElement ( thePlayer ) and thePlayer ~= player then
						triggerClientEvent ( thePlayer, "removePlayerInGangwarDraw", thePlayer, side, amountchanged, alivechanged, pname )
					end
				end
			end
			local otherside = side == "attacker" and "defender" or "attacker"
			if gangwarDatas[area][otherside.."alive"] and gangwarDatas[area][otherside.."alive"][1] then
				for i=1, #gangwarDatas[area][otherside.."alive"] do
					local thePlayer = getPlayerFromName ( gangwarDatas[area][otherside.."alive"][i] )
					if isElement ( thePlayer ) and thePlayer ~= player then
						triggerClientEvent ( thePlayer, "removePlayerInGangwarDraw", thePlayer, side, amountchanged, alivechanged )
					end
				end
			end
		end
		playersInGangwar[player] = nil
		removeEventHandler ( "onPlayerQuit", player, playerLeftGangwar )
	end
	setTimer ( respawnPlayerInGangwarLobby, 5*1000, 1, player )
end 


function gangwarPlayerGotDamage ( pname, attackername, damage )
	local area = nil
	local playerattacker = nil
	local attackerattacker = nil
	if playerGangwarData[attackername] then
		playerGangwarData[attackername].damage = playerGangwarData[attackername].damage + damage 
		playerGangwarData[attackername].gotdamage = true
		area = playerGangwarData[attackername].area
		attackerattacker = playerGangwarData[attackername].attacker
	end
	if playerGangwarData[pname] then
		playerGangwarData[pname].gotdamage = true
		area = playerGangwarData[pname].area
		playerattacker = playerGangwarData[pname].attacker
	end
	if area then
		if attackername then
			if not playersGaveDamage[area][attackername] then
				playersGaveDamage[area][attackername] = {}
			end
			playersGaveDamage[area][attackername][pname] = ( playersGaveDamage[area][attackername][pname] or 0 ) + damage
		end
		playersBecameDamage[area][pname][attackername] = ( playersBecameDamage[area][pname][attackername] or 0 ) + damage
		if playerattacker ~= nil and attackerattacker ~= nil then
			if playerattacker ~= attackerattacker then
				gangwarDatas[area].joinallowed = false
				drawEveryPastInvitation ( area )
			end
		end
	end
	local attacker = getPlayerFromName ( attackername )
	if isElement ( attacker ) then
		triggerClientEvent ( attacker, "giveDamageClient", attacker, damage )
	end
end


function playerLeftGangwar ( player )
	local player = isElement ( source ) and source or player
	playeraskedtojoin[player] = nil
	local pname = getPlayerName ( player )
	removeEventHandler ( "onPlayerQuit", player, playerLeftGangwar )
	unbindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
	unbindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
	removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
	unbindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
	unbindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
	removeEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
	unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
	unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
	playersInGangwar[player] = nil
	local amountchanged = false
	local alivechanged = false
	local gang = tdsGetElementData ( player, "gangwarGang" )
	local short = gangwarGangShort[gang]
	if playersinteam["gangwar"][short] and playersinteam["gangwar"][short][1] then
		for i=1, #playersinteam["gangwar"][short] do
			if playersinteam["gangwar"][short][i] == player then
				table.remove ( playersinteam["gangwar"][short], i )
				break
			end
		end
	end
	triggerLatentClientEvent ( playersinlobby["gangwar"], "syncPlayerInTeams", 50000, false, player, playersinteam["gangwar"] )
	if playerGangwarData[pname] then
		local area = playerGangwarData[pname].area
		if gangwarDatas[area] and gangwarDatas[area].attackergang then
			playerGangwarData[pname].offline = true
			local side = gangwarDatas[area].attackergang == gang and "attacker" or "defender"
			triggerClientEvent ( player, "playerLeftGangwar", player )
			if playerGangwarData[pname].status == "preparation" or gangwarDatas[area].joinallowed then
				if gangwarDatas[area][side] and gangwarDatas[area][side][1] then
					for i=1, #gangwarDatas[area][side] do
						if gangwarDatas[area][side][i] == pname then
							table.remove ( gangwarDatas[area][side], i )
							amountchanged = true
							playerGangwarData[pname] = nil
							break
						end
					end
				end
			end
			if gangwarDatas[area][side.."alive"] and gangwarDatas[area][side.."alive"][1] then
				for i=1, #gangwarDatas[area][side.."alive"] do
					if gangwarDatas[area][side.."alive"][i] == pname then
						table.remove ( gangwarDatas[area][side.."alive"], i )
						alivechanged = true
						break
					end
				end
			end
			if gangwarDatas[area].status == "started" and #gangwarDatas[area][side.."alive"] == 0 then
				if side == "attacker" then
					if isTimer ( gangwarDatas[area].checktimer ) then
						killTimer ( gangwarDatas[area].checktimer )
					end
					gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, 4 )
				elseif side == "defender" and not gangwarDatas[area].joinallowed and getTickCount() - gangwarDatas[area].starttick >= 60*1000 then
					if isTimer ( gangwarDatas[area].wintimer ) then
						killTimer ( gangwarDatas[area].wintimer )
					end
					gangwarDatas[area].wintimer = setTimer ( clearGangwar, 5*1000, 1, area, true )
				end
			elseif gangwarDatas[area].status == "started" and side == "attacker" then
				if not isTimer ( gangwarDatas[area].checktimer ) then
					gangwarDatas[area].checktimer = setTimer ( funcs.checkIfPickupIsOccupied, gwpickupchecktime, 1, area, 1 )
				end
			end
			if amountchanged or alivechanged then
				if gangwarDatas[area][side.."alive"] and gangwarDatas[area][side.."alive"][1] then
					for i=1, #gangwarDatas[area][side.."alive"] do
						local thePlayer = getPlayerFromName ( gangwarDatas[area][side.."alive"][i] )
						if isElement ( thePlayer ) and thePlayer ~= player then
							triggerClientEvent ( thePlayer, "removePlayerInGangwarDraw", thePlayer, side, amountchanged, alivechanged, pname )
						end
					end
				end
				local otherside = side == "attacker" and "defender" or "attacker"
				if gangwarDatas[area][otherside.."alive"] and gangwarDatas[area][otherside.."alive"][1] then
					for i=1, #gangwarDatas[area][otherside.."alive"] do
						local thePlayer = getPlayerFromName ( gangwarDatas[area][otherside.."alive"][i] )
						if isElement ( thePlayer ) and thePlayer ~= player then
							triggerClientEvent ( thePlayer, "removePlayerInGangwarDraw", thePlayer, side, amountchanged, alivechanged )
						end
					end
				end
			end
		else
			playerGangwarData[pname] = nil
		end
	end
end


function playerJoinedGangwar ( player )
	if not playeraskedtojoin[player] then
		local gang = tdsGetElementData ( player, "gangwarGang" )
		if gang then
			local pname = getPlayerName ( player )
			if not playerGangwarData[pname] or playerGangwarData[pname].allowedtojoin then
				if gangsInGangwar[gang] then
					local area = gangsInGangwar[gang]
					if gangwarDatas[area] and ( gangwarDatas[area].joinallowed or getTickCount() - gangwarDatas[area].starttick < 60*1000 ) then
						local gwinfo = { name = gangAreaDatas[area].name, owner = gangwarDatas[area].defendergang.." [".. gangwarGangShort[gangwarDatas[area].defendergang].."]", isfungw = infungangwar[area] }
						triggerLatentClientEvent ( player, "showGangwarPreparationDraw", 50000, false, player, gwinfo )
						addEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarJoinInvitation )
						unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
						unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
						bindKey ( player, "J", "down", funcs.acceptGangwarJoinInvitation )
						bindKey ( player, "N", "down", funcs.rejectGangwarJoinInvitation )
						playeraskedtojoin[player] = true
					end
				elseif gangsInGangwarPreparation[gang] then
					local area = gangsInGangwarPreparation[gang]
					if gangwarDatas[area] and gangwarDatas[area].attackergang == gang then
						local gwinfo = { name = gangAreaDatas[area].name, owner = gangwarDatas[area].defendergang.." [".. gangwarGangShort[gangwarDatas[area].defendergang].."]", isfungw = infungangwar[area] }
						triggerLatentClientEvent ( player, "showGangwarPreparationDraw", 50000, false, player, gwinfo )
						unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
						unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
						addEventHandler ( "onPlayerQuit", player, funcs.rejectGangwarPreparationInvitation )
						bindKey ( player, "J", "down", funcs.acceptGangwarPreparationInvitation )
						bindKey ( player, "N", "down", funcs.rejectGangwarPreparationInvitation )
						playeraskedtojoin[player] = true
					end
				end
			end
		end
	end
end


function putVehicleInGangwar ( player, vehicle )
	local pname = getPlayerName ( player )
	if not playerGangwarData[pname].died and not playerGangwarData[pname].offline then
 		local area = playerGangwarData[pname].area
		local gangID = tdsGetElementData ( player, "gangwarGangID" )
		local team = getPlayerTeam ( player )
		local r, g, b = 0, 204, 0
		if tdsGetElementData ( player, "gangwarGang" ) == gangwarDatas[area].attackergang then
			r, g, b = 255, 4, 4 
		end
		setVehicleTeam ( vehicle, team )
		setVehicleColor ( vehicle, r, g, b )
		setVehicleDoorsUndamageable ( vehicle, true )
		for _, playerinveh in pairs ( getVehicleOccupants ( vehicle ) ) do
			removePedFromVehicle ( playerinveh )
			if tdsGetElementData ( playerinveh, "gangwarGangID" ) == gangID then
				local name = getPlayerName ( playerinveh )
				if not playerGangwarData[name] or playerGangwarData[name].died or playerGangwarData[name].offline then
					setElementDimension ( playerinveh, gangwarLobbyDimension )
				else
					triggerClientEvent ( playerinveh, "ghostModeForVehicle", playerinveh, vehicle )
				end
			else
				setElementDimension ( playerinveh, gangwarLobbyDimension )
			end
		end
		if gangwarDatas[area].status == "preparation" then
			addEventHandler ( "onElementClicked", vehicle, clickedOnVehicleInGangwar )
		end
		gangwarDatas[area].vehicles[#gangwarDatas[area].vehicles+1] = { vehicle, gangID }
	end
end


function getPlayerGangwarStatus ( player )
	local pname = getPlayerName ( player )
	return playerGangwarData[pname] and playerGangwarData[pname].status or false
end


-- ALT --
-- Jede Woche Reset --
--[=[function resetTheGangAreas ( )
	local newareaowner = {}
	local areaattackcount = {}
	dbExec ( handler, "DELETE FROM Gangwars WHERE Sekunden < ?", getRealTime().timestamp-30*24*60*60 )
	local gwresult = dbPoll ( dbQuery ( handler, "SELECT GebieteID, AttackerID FROM Gangwars WHERE Sekunden > ?", getRealTime().timestamp-7*24*60*60 ), -1 )
	if gwresult and gwresult[1] then
		for i=1, #gwresult do
			if not areaattackcount[gwresult[i]["GebieteID"]] then
				areaattackcount[gwresult[i]["GebieteID"]] = {}
			end
			areaattackcount[gwresult[i]["GebieteID"]][gwresult[i]["AttackerID"]] = ( areaattackcount[gwresult[i]["GebieteID"]][gwresult[i]["AttackerID"]] or 0 ) + 1
		end
	end
	for areaID, array in pairs ( areaattackcount ) do
		local highest = 0
		local highestgang = nil
		for gangID, amount in pairs ( array ) do
			if amount > highest then
				highest = amount
				highestgang = gangID
			end
		end
		if highestgang then
			newareaowner[areaID] = highestgang
		end
	end
	local result = dbPoll ( dbQuery ( handler, "SELECT ID, BesitzerGang, Angegriffen FROM Gebiete" ), -1 )
	if result and result[1] then
		for i=1, #result do
			if result[i]["Angegriffen"] == 1 then
				if gangwarGangData[result[i]["BesitzerGang"]] then
					giveGangMoney ( result[i]["BesitzerGang"], 2000 )
				end
			end
			dbExec ( handler, "UPDATE Gebiete SET BesitzerGang = ?, Angegriffen = 0 WHERE ID = ?", newareaowner[result[i]["ID"]] or 0, result[i]["ID"] )
		end
	end
end]=]


function resetTheGangAreas ()
	local result = dbPoll ( dbQuery ( handler, "SELECT GangID FROM GangwarGangOnline" ), -1 )
	local gangwasonline = {}
	if result and result[1] then
		for i=1, #result do
			gangwasonline[result[i]["GangID"]] = true
		end
	end
	local alreadydid = {}
	for _, array in pairs ( gangAreaDatas ) do
		local gangID = gangwarGangIDs[array.owner]
		if gangID and gangID ~= 0 and not alreadydid[gangID] then
			if gangonlinecounter[gangID] then
				if getTickCount() - gangonlinecounter[gangID] >= 30*60*1000 then
					gangwasonline[gangID] = true
				end
				gangonlinecounter[gangID] = nil
			end
			if not gangwasonline[gangID] then
				alreadydid[gangID] = array.name
				dbExec ( handler, "UPDATE Gebiete SET BesitzerGang = 0, Angegriffen = 0 WHERE ID = ?", array.id )
			end
		end
	end
	for gangID, areaname in pairs ( alreadydid ) do
		local msg = "Your gang lost the area "..areaname.." because of inactivity (atleast two members for min. 30 minutes)"
		if gangwarGangMembers[gangID] and gangwarGangMembers[gangID][1] then
			for i=1, #gangwarGangMembers[gangID] do
				if playerUIDName[gangwarGangMembers[gangID][i].player] then
					insertNewOfflineMSGEntry ( "Gang", playerUIDName[gangwarGangMembers[gangID][i].player], msg )
				end
			end
		end
	end
	dbExec ( handler, "UPDATE Gebiete SET Geld = Geld + ? WHERE Geld <= 1000 - ?", areamoneysteps, areamoneysteps )
end


function resetTheGangAreasFromGang ( gang, gangID )
	for area, array in pairs ( gangAreaDatas ) do
		if array.owner == gang then
			gangAreaDatas[area].owner = TEAM_NAMES["nogang"]
		end
	end
	dbExec ( handler, "UPDATE Gebiete SET BesitzerGang = 0 WHERE BesitzerGang = ?", gangID )
end


function getGangAreaInformations ( ) 
	local array = {}
	local highestcount = 0
	for area, info in pairs ( gangAreaDatas ) do
		local cool = info.lastattack + 1*60*60*1000 - getTickCount()
		cool = cool > 0 and math.ceil ( cool / 60000 ) or "Nein"
		local status = gangwarDatas[area] and gangwarDatas[area].status or "Angreifbar"
		if status == "preparation" then
			status = "Vorbereitung"
		elseif status == "started" then
			status = "Läuft"
		elseif cool ~= "Nein" then
			status = "Cooldown"
		end
		local owner = #info.owner <= 10 and info.owner or string.sub ( info.owner, 1, 10 ).."."
		array[info.id] = { 
				["name"] = info.name, 
				["owner"] = owner..rgbToHex ( { info.r, info.g, info.b } ).. "["..gangwarGangShort[info.owner].."]", 
				["status"] = status, 
				["cooldown"] = cool,
				["money"] = info.money
		}
		highestcount = info.id > highestcount and info.id or highestcount
	end
	for i=highestcount, 1, -1 do
		if not array[i] then
			table.remove ( array, i )
		end
	end
	return array 
end


function getGangAreaInformationsForMap ( )
	local returnarray = {}
	local counter = 0
	for area, array in pairs ( gangAreaDatas ) do
		counter = counter + 1
		local cool = array.lastattack + 1*60*60*1000 - getTickCount()
		cool = cool > 0 and math.ceil ( cool / 60000 ) or "Nein"
		local status = array and array.status or "Angreifbar"
		if status == "preparation" then
			status = "Vorbereitung"
		elseif status == "started" then
			status = "Läuft"
		elseif cool ~= "Nein" then
			status = "Cooldown"
		end
		returnarray[counter] = { owner = gangwarGangShort[array.owner], area = area, pickup = array.pickup, name = array.name, status = status }
	end
	return returnarray
end


function destroyAllGangVehiclesFromGang ( gang )
	local area = nil
	local gangID = gangwarGangIDs[gang]
	if gangsInGangwarPreparation[gang] then
		area = gangsInGangwarPreparation[gang]
	elseif gangsInGangwar[gang] then
		area = gangsInGangwar[gang]
	end
	if area then
		if gangwarDatas[area]["vehicles"] and gangwarDatas[area]["vehicles"][1] then
			for i=1, #gangwarDatas[area]["vehicles"] do
				if gangwarDatas[area]["vehicles"][i] and gangwarDatas[area]["vehicles"][i][2] then
					if gangwarDatas[area]["vehicles"][i][2] == gangID then
						if isElement ( gangwarDatas[area]["vehicles"][i][1] ) then
							destroyElement ( gangwarDatas[area]["vehicles"][i][1] )
						end
						gangwarDatas[area]["vehicles"][i] = nil
					end
				end
			end
		end
	end
end


addEventHandler ( "getVehBackToPosInMoving", root, function ( )
	if source == client then
		if lastvehmovedata[client] then
			if isElement ( lastvehmovedata[client][1] ) then
				setElementPosition ( lastvehmovedata[client][1], lastvehmovedata[client][2], lastvehmovedata[client][3], lastvehmovedata[client][4] )
				setElementRotation ( lastvehmovedata[client][1], lastvehmovedata[client][5], lastvehmovedata[client][6], lastvehmovedata[client][7] )
				setElementCollisionsEnabled ( lastvehmovedata[client][1], true )
				setElementFrozen ( lastvehmovedata[client][1], false )
				setElementAlpha ( lastvehmovedata[client][1], 255 )
			end
			vehalreadygettingmoved[lastvehmovedata[client][1]] = nil
			lastvehmovedata[client] = nil
		end
		showCursor ( client, false )
	end
end )

addEventHandler ( "getVehToPosInMoving", root, function ( x, y, z, rx, ry, rz, freezeit )
	if source == client then
		if lastvehmovedata[client] then
			if isElement ( lastvehmovedata[client][1] ) then
				local occupants = getVehicleOccupants ( lastvehmovedata[client][1] )
				for _, _ in pairs ( occupants ) do
					setElementPosition ( lastvehmovedata[client][1], lastvehmovedata[client][2], lastvehmovedata[client][3], lastvehmovedata[client][4] )
					setElementRotation ( lastvehmovedata[client][1], lastvehmovedata[client][5], lastvehmovedata[client][6], lastvehmovedata[client][7] )
					setElementCollisionsEnabled ( lastvehmovedata[client][1], true )
					setElementFrozen ( lastvehmovedata[client][1], false )
					vehalreadygettingmoved[lastvehmovedata[client][1]] = nil
					lastvehmovedata[client] = nil
					showCursor ( client, false )
					return
				end
				setElementPosition ( lastvehmovedata[client][1], x, y, z )
				setElementRotation ( lastvehmovedata[client][1], rx, ry, rz )
				setElementCollisionsEnabled ( lastvehmovedata[client][1], true )
				setElementFrozen ( lastvehmovedata[client][1], false )
				setElementAlpha ( lastvehmovedata[client][1], 255 )
			end
			vehalreadygettingmoved[lastvehmovedata[client][1]] = nil
			lastvehmovedata[client] = nil
		end
		showCursor ( client, false )
	end
end )


function getDistanceToGangwarPickup ( player )
	local pname = getPlayerName ( player )
	if playerGangwarData[pname] then
		local area = playerGangwarData[pname].area
		local x, y, z = getElementPosition ( player )
		local px, py, pz = getElementPosition ( gangAreaDatas[area].pickup )
		return getDistanceBetweenPoints3D ( x, y, z, px, py, pz )
	end
	return 0
end


function changeareaColorsOfGang ( gang, r, g, b )
	for area, array in pairs ( gangAreaDatas ) do
		if array.owner == gang then
			setRadarAreaColor ( area, r, g, b, 150 )
			gangAreaDatas[area].r = r
			gangAreaDatas[area].g = g
			gangAreaDatas[area].b = b
		end
	end
end


addCommandHandler ( "brake", function ( player )
	local gang = tdsGetElementData ( player, "gangwarGang" )
	local lang = tdsGetElementData ( player, "language" )
	if gang and gang ~= TEAM_NAMES["nogang"] then
		local veh = getPedOccupiedVehicle ( player )
		if isElement ( veh ) then
			local typ = getVehicleType ( veh )
			local isfrozen = isElementFrozen ( veh )
			if typ == "Bike" or typ == "Quad" then
				if gangsInGangwarPreparation[gang] or gangsInGangwar[gang] then
					setElementFrozen ( veh, not isfrozen )
					outputInformation ( player, langtext[lang]["thebikewasbraked"][1].." ".. ( isfrozen and langtext[lang]["thebikewasbraked"][2] or langtext[lang]["thebikewasbraked"][3] ), isfrozen and "info" or "success" )
				else
					outputInformation ( player,  langtext[lang]["youarenotingw"], "error" )
				end
			else
				if gangsInGangwarPreparation[gang] then
					setElementFrozen ( veh, not isfrozen )
					outputInformation ( player, langtext[lang]["thecarwasbraked"][1].." ".. ( isfrozen and langtext[lang]["thecarwasbraked"][2] or langtext[lang]["thecarwasbraked"][3] ), isfrozen and "info" or "success" )
				else
					outputInformation ( player, langtext[lang]["youarenotinpreparation"], "error" )
				end
			end
		else
			outputInformation ( player, langtext[lang]["notsittinginvehicle"], "error" )
		end
	else
		outputInformation ( player, langtext[lang]["notingang"], "error" )
	end
end )


addEventHandler ( "spectateInGangwar", root, function ( )
	if tdsGetElementData ( client, "lobby" ) == "gangwar" then
		local gang = tdsGetElementData ( client, "gangwarGang" )
		if gang and gang ~= TEAM_NAMES["nogang"] then
			local pname = getPlayerName ( client )
			if playerGangwarData[pname] and playerGangwarData[pname].died then
				local dim = getElementDimension ( client )
				local area = playerGangwarData[pname].area
				if dim == 65534 then
					setElementDimension ( client, gangwarDatas[area] and gangwarDatas[area].dimension or dim )
					if isPedInVehicle ( client ) then
						removePedFromVehicle ( client )
					end 
					setElementPosition ( client, 0, 0, 9999 )
					setElementDimension ( playerblips[client], 65534 )
					setElementFrozen ( client, true )
					toggleAllControls ( client, false, true, false )
					tdsSetElementData ( client, "status", "died" )
					triggerClientEvent ( client, "onClientCameraSpectateStart", client )
				else
					setElementDimension ( client, 65534 )
					setElementFrozen ( client, false )
					toggleAllControls ( client, true, true, false )
					triggerClientEvent ( client, "onClientCameraSpectateStop", client )
					tdsSetElementData ( client, "status", "gangwar" )
					setCameraTarget ( client )
				end
				return
			end
		end
	end
	triggerClientEvent ( client, "playerLeftGangwar", client )
end )


function changeGangNameAndShortInGangwar ( oldname, newname )
	for area, array in pairs ( gangAreaDatas ) do
		if array.owner == oldname then
			gangAreaDatas[area].owner = newname 
		end
	end
	for area, array in pairs ( gangwarDatas ) do
		if array.attackergang == oldname then
			gangwarDatas[area].attackergang = newname
		elseif array.defendergang == oldname then
			gangwarDatas[area].defendergang = newname
		end
	end
end


function playerChangedNameForGangwar ( oldname, newname )
	if playerGangwarData[oldname] then
		local area = playerGangwarData[oldname].area
		if gangwarDatas[area] then
			playerGangwarData[newname] = {}
			for i, v in pairs ( playerGangwarData[oldname] ) do
				playerGangwarData[newname][i] = v
			end
			playerGangwarData[oldname] = nil
			local found = false
			for i, array in pairs ( gangwarDatas[area] ) do
				if type ( array ) == "table" and array[1] and ( not found or found == i.."alive" or found.."alive" == i ) then
					for j=1, #array do
						if array[j] == oldname then
							gangwarDatas[area][i][j] = newname
							found = i
							break
						end
					end
				end
			end
			if playersBecameDamage[area][oldname] then
				playersBecameDamage[area][newname] = playersBecameDamage[area][oldname]
				playersBecameDamage[area][oldname] = nil
			end
			if playersGaveDamage[area][oldname] then
				playersGaveDamage[area][newname] = playersGaveDamage[area][oldname]
				playersGaveDamage[area][oldname] = nil
			end
		else
			playerGangwarData[oldname] = nil
		end
	end
end


function giveAreaToOtherGang ( oldID, newID )
	local oldname = gangwarGangData[oldID].name 
	local newname = gangwarGangData[newID].name 
	for area, array in pairs ( gangAreaDatas ) do
		if array.owner == oldname then
			gangAreaDatas[area].owner = newname 
			setRadarAreaColor ( area, gangwarGangData[newID].r, gangwarGangData[newID].g, gangwarGangData[newID].b, 150 )
			gangAreaDatas[area].r = gangwarGangData[newID].r
			gangAreaDatas[area].g = gangwarGangData[newID].g
			gangAreaDatas[area].b = gangwarGangData[newID].b
		end
	end
end


addEventHandler ( "portMeToGangArea", root, function ( areaname )
	local lang = tdsGetElementData ( client, "language" )
	if tdsGetElementData ( client, "lobby" ) == "gangwar" then
		local pname = getPlayerName ( client )
		if getElementDimension ( client ) == gangwarLobbyDimension or not playerGangwarData[pname] or playerGangwarData[pname].status == "preparation" then
			for _, info in pairs ( gangAreaDatas ) do
				if info.name == areaname then
					local x, y, z = getElementPosition ( info.pickup )
					setElementPosition ( client, x, y, z )
				end
			end
		else
			outputInformation ( client, langtext[lang]["notingangwar"], "error" )
		end
	else 
		outputInformation ( client, langtext[lang]["notingangwarlobby"], "error" )
	end
end )


addEventHandler ( "stopGangwarPreparation", root, function ( )
	if isTimer ( preparationtimer[client][1] ) then
		killTimer ( preparationtimer[client][1] )
		startGangwar ( preparationtimer[client][2] )
	end
end )


function funcs.acceptFunGangwarInvitation ( player )
	if tdsGetElementData ( player, "lobby" ) == "gangwar" then
		local gang = tdsGetElementData ( player, "gangwarGang" )
		if gang and invitedfrom[gang] then
			if not gangsInGangwar[gang] then
				if not gangsInGangwarPreparation[gang] then
					local inviter = invitedfrom[gang]
					local invitergang = tdsGetElementData ( inviter, "gangwarGang" )
					if isElement ( inviter ) and askedforfungw[inviter] and type ( askedforfungw[inviter] ) == "table" and askedforfungw[inviter][2] == gang and invitergang then
						local short = gangwarGangShort[gang]
						if playerOnlineInGang[short] and playerOnlineInGang[short][1] then
							for i=1, #playerOnlineInGang[short] do
								local target = playerOnlineInGang[short][i]
								if isElement ( target ) then
									unbindKey ( target, "J", "down", funcs.acceptFunGangwarInvitation )
									unbindKey ( target, "N", "down", funcs.rejectFunGangwarInvitation )
									outputInformation ( target, getPlayerName ( player )..langtext[tdsGetElementData(target,"language")]["hasacceptedfungangwar"], "success" )
									triggerClientEvent ( target, "stopFunGangwarJoinDraw", target )
								end
							end
							startGangwarPreparation ( inviter, askedforfungw[inviter][1], invitergang, gang, true )
						end
					else
						outputInformation ( player, langtext[tdsGetElementData(player,"language")]["invitationexpired"], "error" )
					end
				end
			end
		end
	end
	unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
	unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
	triggerClientEvent ( player, "stopFunGangwarJoinDraw", player )
end


function funcs.rejectFunGangwarInvitation ( player )
	unbindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
	unbindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
	triggerClientEvent ( player, "stopFunGangwarJoinDraw", player )
end


addEventHandler ( "sendGangFunGangwarInvitation", root, function ( gang )
	if askedforfungw[client] then
		local invitergang = tdsGetElementData ( client, "gangwarGang" )
		if invitergang then
			if not gangsInGangwar[invitergang] then
				if not gangsInGangwarPreparation[invitergang] then
					if gangwarGangIDs[gang] then
						if not gangsInGangwar[gang] then
							if not gangsInGangwarPreparation[gang] then
								local gangID = gangwarGangIDs[gang]
								local short = gangwarGangShort[gang]
								if playerOnlineInGang[short] and playerOnlineInGang[short][1] then
									invitedfrom[gang] = client
									local area = askedforfungw[client]
									askedforfungw[client] = { area, gang }
									local gwinfo = { name = gangAreaDatas[area].name, attacker = invitergang.." ["..gangwarGangShort[invitergang].."]" }
									local sendarray = { [0] = {}, {}, {}, {} }
									if playerOnlineInGang[short] and playerOnlineInGang[short][1] then
										for i=1, #playerOnlineInGang[short] do
											if isElement ( playerOnlineInGang[short][i] ) then
												local rang = tdsGetElementData ( playerOnlineInGang[short][i], "gangwarGangRang" ) or 0 
												sendarray[rang][#sendarray[rang]+1] =  playerOnlineInGang[short][i]
												outputInformation ( playerOnlineInGang[short][i], langtext[tdsGetElementData(playerOnlineInGang[short][i],"language")]["yougotfungangwarinvitation"]..gwinfo.attacker, "info" )
											end
										end
										for i=3, 0, -1 do 
											if sendarray[i] and sendarray[i][1] then
												for j=1, #sendarray[i] do
													local player = sendarray[i][j]
													bindKey ( player, "J", "down", funcs.acceptFunGangwarInvitation )
													bindKey ( player, "N", "down", funcs.rejectFunGangwarInvitation )
													triggerClientEvent ( player, "showFunGangwarJoinDraw", player, gwinfo )
												end
												break 
											end
										end
									else
										outputInformation ( player, langtext[tdsGetElementData(player,"language")]["noonefromgangonline"], "error" )
									end
								end
							end
						end	
					end
				end
			else
				outputInformation ( player, langtext[tdsGetElementData(player,"language")]["gangdoesntexist"], "error" )
			end
		end
	end
end )


function isInFunGangwar ( player )
	local gang = tdsGetElementData ( player, "gangwarGang" )
	return infungangwar[gang]
end
