local ClassHelper = LibStub("AceAddon-3.0"):GetAddon("ClassHelper")
local {helper} = ClassHelper:GetModule("{helper}")

-- https://www.wowinterface.com/forums/showthread.php?t=31813

local uiscale = 768/(string.match(({GetScreenResolutions()})[GetCurrentResolution()], "%d+x(%d+)"))
local size = {size} * uiscale
local count = {count}

local function lshift(x, by)
    return x * 2 ^ by
end

local function rshift(x, by)
    return math.floor(x / 2 ^ by)
end

local function istrue(value)
    if (value and value ~= 0 and value ~= false) then
        return true
    end

    return false
end

local function clamp(value, low, high)
    low  = low  or 0
    high = high or 1

    return math.min(high, math.max(low, value))
end

{helper}.Frames = {}