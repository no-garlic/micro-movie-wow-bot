
Signal = {}
Signal.{name}Value = 0

local function {function}()
    if GetCurrentKeyBoardFocus() == nil then
        Signal:setSignalBit(24, false)
    else
        Signal:setSignalBit(24, true)
    end

    local r, g, b = ClassHelper:packbits(Signal.{name}Value)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end

function Signal:setSignal(value)
    if value ~= nil then
        self.{name}Value = tonumber(value)
    else
        self.{name}Value = 0
    end
end

function Signal:setSignalBit(bit, on)
    local {name}BitValue = lshift(1, bit - 1)
    local ison = self.getSignalBit(self, bit)
    local seton = istrue(on) or (on == nil)
    if not ison and seton then
        self.{name}Value = self.{name}Value + {name}BitValue
    elseif ison and not seton then
        self.{name}Value = self.{name}Value - {name}BitValue
    end
end

function Signal:getSignalBit(bit)
    local a = rshift(self.{name}Value, bit - 1)
    local b = a % 2
    if b == 1 then
        return true
    end
    return false
end

function Signal:toggleSignalBit(bit)
    local ison = not self.getSignalBit(self, bit)
    self.setSignalBit(self, bit, ison)
    return ison
end

function Signal:command(msg)
    local cmd = msg:lower()
    if cmd:sub(1,6) == "setbit" then
        local bit, val = msg:match("(%d+) (%d+)")
        if val == nil then
            bit = msg:match("(%d+)")
            val = 1
        end
        if val == nil or bit == nil then return end
        self.setSignalBit(self, bit, tonumber(val))
        return
    elseif cmd:sub(1,3) == "set" then
        local val = msg:match("(%d+)")
        if val == nil then val = 0 end
        self.setSignal(self, tonumber(val))
        return
    elseif cmd:sub(1,6) == "toggle" then
        local bit = msg:match("(%d+)")
        if bit == nil then return end
        self.toggleSignalBit(self, tonumber(bit))
        return
    elseif cmd:sub(1,5) == "pause" then
        self.toggleSignalBit(self, 1)
        self.toggleSignalBit(self, 2)
        self.toggleSignalBit(self, 3)
        return
    end
end

function Signal:isPaused()
    if Signal.signalValue == 0 then
        return false
    end
    return true
end

SLASH_{command}1 = "/{slash}"
SLASH_{command}2 = "/signal"
SLASH_{command}3 = "/sig"
SlashCmdList["{command}"] = function(msg) Signal:command(msg) end
