local uiscale = 768/(string.match(({GetScreenResolutions()})[GetCurrentResolution()], "%d+x(%d+)"))
local size = {size} * uiscale
local count = {count}

local {addon} = {}

local f = CreateFrame("frame")
f:SetSize(size * count, size)
f:SetPoint("TOPLEFT", {xPos}, {yPos})
f:SetFrameStrata("TOOLTIP")
f:SetFrameLevel(1000)
f:SetToplevel(true)
f:RegisterEvent("ADDON_LOADED")
