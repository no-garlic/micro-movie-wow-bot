
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

    local r = packint1(unit)
    local g = packint1(pwr)
    local b = packfloat1(percentHealth)

    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end





