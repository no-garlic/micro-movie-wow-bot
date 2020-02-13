
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

    local r = ClassHelper:packint1(resource)
    local g = ClassHelper:packint1(resourceMax)
    local b = ClassHelper:packint1(comboPacked)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end



