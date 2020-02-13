
colorstate = {}
colorstate.mt = {}
colorstate.mt.fns = {}
colorstate.mt.__index = colorstate.mt.fns

function colorstate.new(r, g, b)
  local retval = {}
  retval[1] = r
  retval[2] = g
  retval[3] = b
  setmetatable(retval, colorstate.mt)
  return retval
end

function colorstate.mt.fns.print(self)
  print("colorstate: "..self[1]..", "..self[2]..", "..self[3])
end

function colorstate.mt.__tostring(self)
  return "colorstate: "..self[1]..", "..self[2]..", "..self[3]
end

function colorstate.mt.__eq(self, other)
  return (self[1] == other[1]) and (self[2] == other[2]) and (self[3] == other[3])
end
