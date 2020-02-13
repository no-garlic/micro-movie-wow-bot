
local maxbyte = 255

function lshift(x, by)
    return x * 2 ^ by
end

function rshift(x, by)
    return math.floor(x / 2 ^ by)
end

function istrue(value)
    if (value and value ~= 0 and value ~= false) then
        return true
    end

    return false
end

function clamp(value, low, high)
    low  = low  or 0
    high = high or 1

    return math.min(high, math.max(low, value))
end

function clamp3(a, b, c, low, high)
    low  = low  or 0
    high = high or 1

    local a = math.min(high, math.max(low, a))
    local b = math.min(high, math.max(low, b))
    local c = math.min(high, math.max(low, c))

    return a, b, c
end

function splitbiasfactor(bigunit)
    local factor = math.floor(256 / (bigunit + 1), 0)
    local bias = (256 - factor) - (bigunit * factor)

    return factor, bias
end

function enumbiasfactor(numopts)
    local factor = math.floor(256 / (numopts - 1), 0)
    local bias = 256 - (factor * (numopts - 1))

    return factor, bias
end

function packbool(value)
    if (istrue(value)) then
        return 1
    end

    return 0
end

function packbool2(a, b)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    return x / 3
end

function packbool3(a, b, c)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    if istrue(c) then x = x + lshift(1, 2) end
    return x / 7
end

function packbool4(a, b, c, d)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    if istrue(c) then x = x + lshift(1, 2) end
    if istrue(d) then x = x + lshift(1, 3) end
    return x / 15
end

function packbool5(a, b, c, d, e)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    if istrue(c) then x = x + lshift(1, 2) end
    if istrue(d) then x = x + lshift(1, 3) end
    if istrue(e) then x = x + lshift(1, 4) end
    return x / 31
end

function packbool6(a, b, c, d, e, f)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    if istrue(c) then x = x + lshift(1, 2) end
    if istrue(d) then x = x + lshift(1, 3) end
    if istrue(e) then x = x + lshift(1, 4) end
    if istrue(f) then x = x + lshift(1, 5) end
    return x / 63
end

function packbool7(a, b, c, d, e, f, g)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    if istrue(c) then x = x + lshift(1, 2) end
    if istrue(d) then x = x + lshift(1, 3) end
    if istrue(e) then x = x + lshift(1, 4) end
    if istrue(f) then x = x + lshift(1, 5) end
    if istrue(g) then x = x + lshift(1, 6) end
    return x / 127
end

function packbool8(a, b, c, d, e, f, g, h)
    local x = 0
    if istrue(a) then x = x + lshift(1, 0) end
    if istrue(b) then x = x + lshift(1, 1) end
    if istrue(c) then x = x + lshift(1, 2) end
    if istrue(d) then x = x + lshift(1, 3) end
    if istrue(e) then x = x + lshift(1, 4) end
    if istrue(f) then x = x + lshift(1, 5) end
    if istrue(g) then x = x + lshift(1, 6) end
    if istrue(h) then x = x + lshift(1, 7) end
    return x / 255
end

function packint1(value)
    return clamp(value / maxbyte)
end

function unpackint1(value)
    return math.floor(value * maxbyte)
end

function packint2(value, bigunit)
    bigunit = bigunit or maxbyte
    local factor, bias = splitbiasfactor(bigunit)

    local wholepart, fractionpart = math.modf(value)
    local bigpart = math.floor(wholepart / bigunit)
    local smallpart = wholepart - (bigpart * bigunit)

    local a = (bias + (bigpart * factor)) / 255
    local b = (bias + (smallpart * factor)) / 255

    if (a > 1) then
        return 1, 1
    end

    return a, b
end

function packenum1(value, numopts)
    local factor, bias = enumbiasfactor(numopts)
    local a = ((factor * value) + bias) / 255

    return a
end

function packfloat1(value)
    return clamp(value)
end

function packfloat3(value, bigunit)
    bigunit = bigunit or maxbyte
    local factor, bias = splitbiasfactor(bigunit)

    local wholepart, fractionpart = math.modf(value)
    local bigpart = math.floor(wholepart / bigunit)
    local smallpart = wholepart - (bigpart * bigunit)

    local a = (bias + (bigpart * factor)) / 256
    local b = (bias + (smallpart * factor)) / 256
    local c = fractionpart

    if (a > 1) then
        return 1, 1, 1
    end

    return a, b, c
end

function packfloat2(value)
    local wholepart, fractionpart = math.modf(value)

    local a = wholepart / 256
    local b = fractionpart

    if (a > 1) then
        return 1, 1
    end

    return a, b
end

function unpackfloat3(a, b, c, bigunit)
    bigunit = bigunit or maxbyte

    local a256 = a * 256;
    local b256 = b * 256;
    local c256 = c * 256;

    local factor, bias = splitbiasfactor(bigunit)

    local bigpart = (a256 - bias) / factor
    local smallpart = (b256 - bias) / factor
    local fractionpart = c256 / 256
    local wholepart = (bigpart * bigunit) + smallpart

    local value = wholepart + fractionpart
    return value
end

function packtime3(value)
    return packfloat3(value, 60)
end

function unpacktime3(a, b, c)
    return unpackfloat3(a, b, c, 60)
end

function packbits(value)
	local b = rshift(value, 16)
	local g = rshift(value, 8) - lshift(b, 8)
	local r = value - (lshift(g, 8) + lshift(b, 16))
	return packint1(r), packint1(g), packint1(b)
end
