
local function {function}()
    local charges, _, _, _ = GetSpellCharges({spell}) or 1
    local start, duration, _ = GetSpellCooldown({spell})
    local inrange = IsSpellInRange({spell}, "target") or 1

    local cooldown = 0
    if (start and duration) then
        cooldown = math.max(0, start + duration - GetTime())
    end

    local nocharges = true
    if (charges > 0) then nocharges = false end

    local outofrange = 1 - inrange

    local r = ClassHelper:packbool2(nocharges, outofrange)
	local g, b = ClassHelper:packfloat2(cooldown)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end
