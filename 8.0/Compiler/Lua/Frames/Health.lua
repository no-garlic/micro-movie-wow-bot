
local function {function}()
	local health = UnitHealth({unit}) or 0
	local maxHealth = UnitHealthMax({unit}) or 0

    if (maxHealth == 0) then
        maxHealth = 1
    end

	local percentHealth = health / maxHealth

    health    = health / {scale}
    maxHealth = maxHealth / {scale}

    local pwr  = 0
    local unit = maxHealth
    
    repeat        
        unit = unit / 10
        pwr  = pwr + 1
    until (unit <= 255.0)

    unit = math.floor(unit)

    local minFloat = 1.0 / 255.0
    if (percentHealth > 0 and percentHealth <= minFloat) then
        percentHealth = minFloat
    end

    local r = ClassHelper:packint1(unit)
    local g = ClassHelper:packint1(pwr)
    local b = ClassHelper:packfloat1(percentHealth)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end





