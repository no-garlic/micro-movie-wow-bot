
local function {function}()
    local targetExists = UnitGUID("target") or false     
    if (istrue(targetExists)) then targetExists = true end

    local targetIsFriend = UnitIsFriend("player","target") or false
    local targetIsEnemy = not istrue(targetIsFriend)

    local targetInCombat = UnitAffectingCombat("target") or false

    local targetIsDead = UnitIsDeadOrGhost("target") or false
    local targetIsAlive = not istrue(targetIsDead)

    local targetName = UnitName("target") or ""
    local targetIsDummy = false
    if (targetName:find(" Dummy") ~= nil) then targetIsDummy = true end

    local c = 0
    local uc = UnitClassification("target")
    if uc == "normal" then c = 1 end
    if uc == "elite" then c = 2 end
    if uc == "minus" then c = 3 end
    if uc == "rare" then c = 4 end
    if uc == "rareelite" then c = 5 end
    if uc == "worldboss" then c = 6 end
    if uc == "trivial" then c = 7 end

    local level = UnitLevel("target") or 0
    local playerLevel = UnitLevel("player")
    if level < 0 then level = playerLevel + 3 end

    local isInst, instTy = IsInInstance()
    local isInDungeon = false    
    if (isInst and instTy == "party") then isInDungeon = true end
    local isInRaid = istrue(IsInRaid())
    
    local notInDungeon = not isInDungeon
    local notInRaid = not isInRaid

    local highHealth   = istrue(UnitHealthMax("target") > (UnitHealthMax("player") * 1.5))
    local outdoorBoss  = istrue(notInDungeon and notInRaid and (istrue(level > playerLevel) or istrue(highHealth)))
    local dungeonBoss  = istrue(isInDungeon) and istrue(level > playerLevel + 1)
    local raidBoss     = istrue(isInRaid) and istrue(level > playerLevel + 2)
    local bossType     = istrue(c ~= 1 and c ~= 3 and c ~= 7)
    local targetIsBoss = istrue(targetExists) and istrue(targetIsEnemy) and istrue(targetIsAlive) and istrue(bossType or highHealth) and istrue(outdoorBoss or dungeonBoss or raidBoss)

    --print(isInDungeon, isInRaid, highHealth, outdoorBoss, dungeonBoss, raidBoss, bossType, targetIsBoss)

    local casting = false
    if (select(8, UnitCastingInfo("target")) == false) then 
        casting = true
    end

    local r = ClassHelper:packint1(level)
    local g = ClassHelper:packbool8(targetIsFriend, targetIsEnemy, targetExists, targetIsAlive, targetInCombat, targetIsBoss, targetIsDummy, casting)
	local b = ClassHelper:packint1(c)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end