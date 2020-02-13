using System;

namespace CommonLib
{
    public class PET : Data
    {
        public bool IsInBattle      { get; private set; }
        public bool IsTurnReady     { get; private set; }
        public bool IsMaxHealth     { get; private set; }
        public bool IsSafariHat     { get; private set; }
        public bool IsLesserTreat   { get; private set; }
        public bool IsPetTreat      { get; private set; }
        public int  ReviveCooldown  { get; private set; }

        public PET() : base(50)
        {
        }

        public override void Update()
        {
            bool[] values  = Conversion.ToBoolArray(this, ColorChannel.Red, 6);
            ReviveCooldown = Conversion.ToInt2(this, ColorChannel.Green, ColorChannel.Blue);

            IsInBattle      = values[0];
            IsTurnReady     = values[1];
            IsMaxHealth     = values[2];
            IsSafariHat     = values[3];
            IsLesserTreat   = values[4];
            IsPetTreat      = values[5];

            //Console.Out.WriteLine($"{ReviveCooldown}, {IsInBattle}, {IsTurnReady}, {IsMaxHealth}, {IsSafariHat}, {IsLesserTreat}, {IsPetTreat}");
        }

        public override void LogData()
        {
        }


    }
}

/*
local function petBattleInfoUpdate()
    local inBattle = istrue(C_PetBattles.IsInBattle())
    local turnReady = istrue(inBattle and (C_PetBattles.IsSkipAvailable() or C_PetBattles.ShouldShowPetSelect()))

    local health1 = C_PetBattles.GetHealth(1, 1) / C_PetBattles.GetMaxHealth(1, 1)
    local health2 = C_PetBattles.GetHealth(1, 2) / C_PetBattles.GetMaxHealth(1, 2)
    local health3 = C_PetBattles.GetHealth(1, 3) / C_PetBattles.GetMaxHealth(1, 3)
    
    local petHealth = health1 + health2 + health3
    local atFullHealth = istrue(petHealth > 2.99)
    
    local safariHat = ClassHelper:GetUnitBuff("player", "Safari Hat")
    local lesserPetTreat = ClassHelper:GetUnitBuff("player", "Lesser Pet Treat")
    local petTreat = ClassHelper:GetUnitBuff("player", "Pet Treat")
    
    local start, duration, _ = GetSpellCooldown("Revive Battle Pets")
    local reviveCooldown = 0
    if (start and duration) then
        reviveCooldown = math.max(0, start + duration - GetTime())
    end

    local r = ClassHelper:packbool6(inBattle, turnReady, atFullHealth, safariHat, lesserPetTreat, petTreat)
	local g, b = ClassHelper:packfloat2(reviveCooldown)
    
    local frame = MageHelper.Frames["petBattleInfoFrame"]
    ClassHelper:SetFrameColor(frame, r, g, b)
end
*/