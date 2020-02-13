
local function {function}()
    local r = 0
    local g = 0
	local b = 0

    local focusTarget = UnitGUID("focus") or false     
    if istrue(focusTarget) and focusTarget ~= UnitGUID("target") then 

        local focusIsFriend = UnitIsFriend("player","focus") or false
        local focusIsEnemy = not istrue(focusIsFriend)

        --local focusInCombat = true
        local focusInCombat = UnitAffectingCombat("focus") or false

        local focusIsDead = UnitIsDeadOrGhost("focus") or false
        local focusIsAlive = not istrue(focusIsDead)

        local focusHealth = UnitHealth("focus") or 0
        local maxFocusHealth = UnitHealthMax("focus") or 0

        if (maxFocusHealth == 0) then
            maxFocusHealth = 1
        end

        local percentFocusHealth = focusHealth / maxFocusHealth

        if focusIsEnemy and focusInCombat and focusIsAlive then
            local isready = true
            local needri  = true
            local needrk  = true
            local needmf  = true

            local riname, riduration, riexpires = ClassHelper:GetUnitDebuff("focus", "Rip")
            local rkname, rkduration, rkexpires = ClassHelper:GetUnitDebuff("focus", "Rake")
            local mfname, mfduration, mfexpires = ClassHelper:GetUnitDebuff("focus", "Moonfire")

            local now = GetTime()

            if (riname and riduration and riexpires) then
                local remaining = riexpires - now
                if remaining > 5 then
                    needri = false
                end
            end
        
            if percentFocusHealth < 0.25 then
                needri = false
            end

            if (rkname and rkduration and rkexpires) then
                local remaining = rkexpires - now
                if remaining > 4 then
                    needrk = false
                end
            end

            if (mfname and mfduration and mfexpires) then
                local remaining = mfexpires - now
                if remaining > 4.7 then
                    needmf = false
                end
            end

            if not istrue(IsSpellInRange("Rip", "focus")) then needri = false end
            if not istrue(IsSpellInRange("Rake", "focus")) then needrk = false end
            if not istrue(IsSpellInRange("Moonfire", "focus")) then needmf = false end

            r = ClassHelper:packbool4(isready, needri, needrk, needmf)            
        end
    end

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end
