
local function {function}()
    local talents = {}
    for t = 1,7 do
        local a = 0
        for i = 1,3 do
            if (select(4, GetTalentInfo(t,i,1)) == true) then 
            a = i 
            end
        end
        talents[t] = a
    end   
    local level = UnitLevel("player") or 0

    local r = (talents[4] + lshift(talents[3], 2) + lshift(talents[2], 4) + lshift(talents[1], 6)) / 255.0
    local g = (0          + lshift(talents[7], 2) + lshift(talents[6], 4) + lshift(talents[5], 6)) / 255.0
    local b = ClassHelper:packint1(level)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end



