
local function {function}()
    local playerClass, englishClass, classIndex = UnitClass("player");
    local currentSpec = GetSpecialization()
    local currentSpecId = currentSpec and select(1, GetSpecializationInfo(currentSpec)) or 0

    local r = packenum1(classIndex, 15)
    local g, b = packint2(currentSpecId, 32)
    
    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end



