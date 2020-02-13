
local function {function}()
    local playerClass, englishClass, classIndex = UnitClass("player");
    local currentSpec = GetSpecialization()
    local currentSpecId = currentSpec and select(1, GetSpecializationInfo(currentSpec)) or 0

    local r = ClassHelper:packenum1(classIndex, 15)
    local g, b = ClassHelper:packint2(currentSpecId, 32)
    
    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end
