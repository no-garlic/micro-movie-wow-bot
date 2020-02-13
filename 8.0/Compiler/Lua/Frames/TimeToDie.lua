
local function round2(num, idp)
  mult = 10^(idp or 0)
  return math.floor(num * mult + 0.5) / mult
end

local function ttd(unit)
	unit = unit or "target";
	if thpcurr == nil then
		thpcurr = 0
	end
	if thpstart == nil then
		thpstart = 0
	end
	if timestart == nil then
		timestart = 0
	end
	if timereset == nil then
		timereset = 0
	end
	if UnitExists(unit) and not UnitIsDeadOrGhost(unit) then
		if currtar ~= UnitGUID(unit) then
			currtar = UnitGUID(unit)
            timereset = GetTime()
		end
		if thpstart==0 and timestart==0 then
			thpstart = UnitHealth(unit)
			timestart = GetTime()
		else
			thpcurr = UnitHealth(unit)
			timecurr = GetTime()
			if thpcurr >= thpstart then -- todo, handle target healed
				thpstart = thpcurr
				timeToDie = 0
			else
				if ((timecurr - timestart)==0) or ((thpstart - thpcurr)==0) then
					timeToDie = 0
				else
					timeToDie = round2(thpcurr/((thpstart - thpcurr) / (timecurr - timestart)),2)
				end
			end
		end
	elseif not UnitExists(unit) or currtar ~= UnitGUID(unit) then
		currtar = 0 
		thpstart = 0
        timereset = 0
		timestart = 0
		timeToDie = 0
	end
	if timeToDie==nil then
		return 0
	else
        if timereset > 0 then
            --print(timeToDie)
    		return timeToDie
        end
        return 0
	end
end	

local function {function}()
    local timeToDie = ttd("target")
    local r, g, b = ClassHelper:packtime3(timeToDie)

    local frame = {helper}.Frames["{frame}"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end
