
local function {function}()
    local r = packint1({x})
    local g = packint1({y})
    local b = packint1({z})
    
    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end



