
local function eventHandler(self, event, ...)
    local arg1 = ...
    if event == "ADDON_LOADED" then
        if (arg1 == "{addon}") then
            local _, _, classIndex = UnitClass("player");
            if (classIndex == {class}) then
                initFrames1()
                initFrames2()
                initFrames3()
            end
        end
    end
end

f:SetScript("OnEvent", eventHandler)
