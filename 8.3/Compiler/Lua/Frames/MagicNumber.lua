
local function {function}()
    local r = ClassHelper:packint1({x})
    local g = ClassHelper:packint1({y})
    local b = ClassHelper:packint1({z})
    
    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end



