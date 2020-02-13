
local function {function}()
    local name, _, _, _, _, duration, expires = Unit{type}({unit}, {aura}, nil, "PLAYER")

    if (name and duration and expires) then
        local now = GetTime()

        local finish = expires * 0.001 
        local remaining = expires - now

        local r, g, b = packtime3(remaining)

        local color = colorstate.new(r, g, b)

        if ({frame}.prev ~= color) then
            {frame}.t:SetColorTexture(color[1], color[2], color[3])
            {frame}.t:SetAllPoints({frame})
            {frame}.prev = color
        end
    else
        local color = colorstate.new(0, 0, 0)

        if ({frame}.prev ~= color) then
            {frame}.t:SetColorTexture(color[1], color[2], color[3])
            {frame}.t:SetAllPoints({frame})
            {frame}.prev = color
        end
    end
end

