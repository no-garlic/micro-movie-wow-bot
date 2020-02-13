
local function {function}()
    local isStealthed     = IsStealthed() or false
    local isInCombat      = UnitAffectingCombat("player") or false
    local isInControl     = HasFullControl() or false
    local isUsingVehicle  = UnitUsingVehicle("player") or false
    local isDead          = UnitIsDeadOrGhost("player") or false
    local isAlive         = not isDead

    local isInRaid        = IsInRaid()
    local isInParty       = IsInGroup()
    local isInst, instTy  = IsInInstance()
    local partySize       = GetNumGroupMembers() or 1

    partySize = math.max(partySize, 1)

    local playerForm      = GetShapeshiftFormID() or 0

    local isInDungeon     = false    
    if (isInst and instTy == "party") then 
        isInDungeon = true 
    end

    local race = UnitRace("player")
    local isHuman = istrue(race == 'Human')

    local r = ClassHelper:packbool8(isStealthed, isInCombat, isInControl, isUsingVehicle, isAlive, isHuman, isInRaid, isInDungeon)
    local g = ClassHelper:packint1(partySize)
	local b = ClassHelper:packint1(playerForm)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end
