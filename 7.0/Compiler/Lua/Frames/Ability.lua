
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

    local r = packbool2(nocharges, outofrange)
	local g, b = packfloat2(cooldown)

    local color = colorstate.new(r, g, b)

    if ({frame}.prev ~= color) then
        {frame}.t:SetColorTexture(color[1], color[2], color[3])
        {frame}.t:SetAllPoints({frame})
        {frame}.prev = color
    end
end
