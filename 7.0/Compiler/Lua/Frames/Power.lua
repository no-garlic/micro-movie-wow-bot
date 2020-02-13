
local function {function}()
    local power = 1
    if (GetSpecialization() == 2) then
        power = 3
    end

    local resource = UnitPower({unit}, power) or 0
    local combo = UnitPower({unit}, 4) or 0

    local resourceMax = UnitPowerMax({unit}, power) or 0
    local comboMax = UnitPowerMax({unit}, 4) or 0

    local comboPacked = (comboMax * 10) + combo

    local r = packint1(resource)
    local g = packint1(resourceMax)
    local b = packint1(comboPacked)

    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end



