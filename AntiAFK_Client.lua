-------------------
---- Copyright ----
-- Do not remove --
---- Script by ----
------ Bonus ------
-------------------

local screenX, screenY = guiGetScreenSize()
local sx, sy = screenX/1920, screenY/1080
local oldpos = {}
local starttick = 0
local lobby = ""
local stillafk = false
local tickplayed = {}
local checktimer
local funcs = {}


function funcs.drawAFKWarning ( )
	if getElementData ( localPlayer, "status" ) == "playing" then
		if lobby == playerlobby then
			local x, y, z = getElementPosition ( localPlayer )
			if getDistanceBetweenPoints3D ( oldpos[1], oldpos[2], oldpos[3], x, y, z ) <= 2 then
				if not isinairstrike then
					local wasted = getTickCount() - starttick
					if wasted <= 40*1000 then
						dxDrawRectangle ( 0, 0, screenX, screenY, tocolor ( 200, 0, 0, 40 ) )
						local sec = (40 - math.floor ( ( wasted ) / 1000 ) )
						local text = langtext[language]["youareafk"][1]..sec..langtext[language]["youareafk"][2]
						dxDrawText ( text, 1, -1, screenX+1, screenY-1, tocolor ( 0, 0, 0 ), 5*sy, "default", "center", "center" )
						dxDrawText ( text, 0, 0, screenX, screenY, tocolor ( 255, 255, 255 ), 5*sy, "default", "center", "center" )
						if not tickplayed[sec] then
							playSoundFrontEnd ( 5 )
							tickplayed[sec] = true
						end
						return 
					else
						triggerServerEvent ( "playerIsAFK", localPlayer )
					end
				end
			end
		end
	end
	stillafk = false
	removeEventHandler ( "onClientRender", root, funcs.drawAFKWarning )
end


local function checkAFK ( )
	if getElementData ( localPlayer, "status" ) == "playing" then
		if lobby == playerlobby then
			local x, y, z = getElementPosition ( localPlayer )
			if getDistanceBetweenPoints3D ( oldpos[1], oldpos[2], oldpos[3], x, y, z ) <= 2 then
				if getTickCount() - starttick >= 25*1000 then
					if isTimer ( checktimer ) then
						killTimer ( checktimer )
					end
					addEventHandler ( "onClientRender", root, funcs.drawAFKWarning )
				end
				return
			end
		end
	end
	stillafk = false
	if isTimer ( checktimer ) then
		killTimer ( checktimer )
	end
	removeEventHandler ( "onClientPlayerWeaponFire", localPlayer, funcs.shootForNotAFK )
end


--function funcs.shootForNotAFK ( )
--	stillafk = false
--	if isTimer ( checktimer ) then
--		killTimer ( checktimer )
--	end
--	removeEventHandler ( "onClientPlayerWeaponFire", localPlayer, funcs.shootForNotAFK )
--end


--addEventHandler ( "onClientPlayerWasted", localPlayer, function ( )
--	if stillafk then
--		outputInformation ( "Kicked because of AFK", "error" )
--		triggerServerEvent ( "playerIsAFK", localPlayer )
--	end
--end )


function clientRoundStartetAntiAFK ( )
	if playerlobby == "standard" then
		local team = getPlayerTeam ( localPlayer )
		if team then
			local teamname = getTeamName ( team )
			if teamname ~= TEAM_NAMES["spectator"] and teamname ~= TEAM_NAMES["spectator_graveyard"] then
				oldpos = { getElementPosition ( localPlayer ) }
				starttick = getTickCount()
				lobby = playerlobby
				stillafk = true
				tickplayed = {}
				if isTimer ( checktimer ) then
					killTimer ( checktimer )
				end
				checktimer = setTimer ( checkAFK, 5000, 0 )
				addEventHandler ( "onClientPlayerWeaponFire", localPlayer, funcs.shootForNotAFK )
			end
		end
	end
end


--addEventHandler ( "onClientRoundFinish", root, function ( )
--	removeEventHandler ( "onClientRender", root, funcs.drawAFKWarning )
--	if isTimer ( checktimer ) then
--		killTimer ( checktimer )
--	end
--	removeEventHandler ( "onClientPlayerWeaponFire", localPlayer, funcs.shootForNotAFK )
--end )


