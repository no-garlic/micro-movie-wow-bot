
local function {function}()
    local name, duration, expires, count = ClassHelper:GetUnit{type}({unit}, {aura})

    if (name and duration and expires) then
        local now = GetTime()

        local finish = expires * 0.001 
        local remaining = expires - now

        local r, g = ClassHelper:packfloat2(remaining)
        local b = ClassHelper:packint1(count)

        local frame = {helper}.Frames["{frame}"]
        ClassHelper:SetFrameColor(frame, r, g, b)
    else
        local frame = {helper}.Frames["{frame}"]
        ClassHelper:SetFrameColor(frame, 0, 0, 0)
    end
end

