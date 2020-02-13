
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

    local fangs = IsEquippedItem("Fangs of Ashamane")
    local claws = IsEquippedItem("Claws of Ursoc")
    local elune = IsEquippedItem("Scythe of Elune")
    local hanir = IsEquippedItem("G'Hanir, the Mother Tree")
    local isArtifact = fangs or claws or elune or hanir

    local r = packbool8(isStealthed, isInCombat, isInControl, isUsingVehicle, isAlive, isArtifact, isInRaid, isInDungeon)
    local g = packint1(partySize)
	local b = packint1(playerForm)

    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end
