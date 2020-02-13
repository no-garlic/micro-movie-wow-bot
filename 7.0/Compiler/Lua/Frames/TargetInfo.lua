
local function {function}()
    local targetExists = UnitGUID("target") or false     
    if (istrue(targetExists)) then targetExists = true end

    local targetIsFriend = UnitIsFriend("player","target") or false
    local targetIsEnemy = not istrue(targetIsFriend)

    local targetInCombat = UnitAffectingCombat("target") or false

    local targetIsDead = UnitIsDeadOrGhost("target") or false
    local targetIsAlive = not istrue(targetIsDead)

    local c = 0
    local uc = UnitClassification("target")
    if uc == "normal" then c = 1 end
    if uc == "elite" then c = 2 end
    if uc == "minus" then c = 3 end
    if uc == "rare" then c = 4 end
    if uc == "rareelite" then c = 5 end
    if uc == "worldboss" then c = 6 end

    local level = UnitLevel("target") or 0
    if level < 0 then level = 113 end

    if IsInRaid() and level == 112 then
        local name = UnitName("target")
        if name == "Hymdall" or name == "Hyrja" then            
            level = 113            
        end
    end    

    local r = packint1(level)
    local g = packbool5(targetIsFriend, targetIsEnemy, targetExists, targetIsAlive, targetInCombat)
	local b = packint1(c)

    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end
