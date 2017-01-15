using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WH40K_System_Generator;

namespace _40k_System_Generator.life
{
    //TODO: move this to planets

    public enum WorldType
    {
        DeathWorld,
        OceanWorld,
        JungleWorld,
        TemperateWorld
    }

    internal enum FloraBase
    {
        [Description("Diffuse Flora")]
        LowMass = 0,
        [Description("Small Flora")]
        Small,
        [Description("Large Flora")]
        Large,
        [Description("Massive Flora")]
        Mssive
    };

    internal enum FloraType
    {
        [Description("Trap, Passive")]
        PassiveTrap = 0,
        [Description("Trap, Acive")]
        ActiveTrap,
        [Description("Combatant")]
        Combatant
    };

    class Flora : Entity
    {
        FloraType floraType;
        WorldType homeWorld;

        // This generates a new random flora 
        public static Flora GenerateRandomFlora()
        {
            Flora flora;           

            // The Koronus Bestiary, Table 4-1
            int roll = RNG.RandNumber(0, 11);
            if (roll == 1)
                flora = new DiffuseFlora();
            else if (roll >= 2 && roll <= 4)
                flora = new SmallFlora();
            else if (roll >= 5 && roll <= 8)
                flora = new LargeFlora();
            else
                flora = new MassiveFlora();

            // The Koronus Bestiary, Table 4-2
            roll = RNG.RandNumber(0, 11);
            if (roll >= 1 && roll <= 3)
            {
                flora.floraType = FloraType.PassiveTrap;

                // Passive traps lose everything except their toughness.
                flora.weaponSkill = 0;
                flora.ballisticSkill = 0;
                flora.strength = 0;
                flora.agility = 0;
                flora.intelligence = 0;
                flora.perception = 0;
                flora.willpower = 0;
                flora.fellowship = 0;
                flora.weapons = null;
            }
            else if (roll >= 4 && roll <= 6)
            {
                flora.floraType = FloraType.ActiveTrap;

                // Active traps lose 10 weaponskill and have their perception set to 5
                flora.weaponSkill -= 10;
                flora.perception = 5;                
            }
            else
            {
                flora.floraType = FloraType.Combatant;

                //TODO: all in 30% chance of Snare quality for weapon
            }

            flora.GenerateSpeciesTraits();

            return flora;
        }

        // Generates the species traits appropriate for our floraType and our homeworld, as per p. 128 of the Koronus Bestiary
        public void GenerateSpeciesTraits()
        {
            // flora rolls twice on the Flora Type table
            switch (floraType)
            {                
                case FloraType.PassiveTrap:
                    break;
            }

            // TODO
        }

        public void GeneratePassiveTrapSpeciesTraits()
        {
            int roll = RNG.RandNumber(0, 11);
            switch (roll)
            {
                /*
                case 1:
                    traits.Add(Traits.Armoured);
                    break;
                case 2:
                    traits.Add(Traits.Deterrent);
                    break;
                case 3:
                    traits.Add(Traits.Frictionless);
                    break;
                case 4:
                    traits.Add(Traits.Sticky);
                    break;
                case 5:
                case 6:
                    traits.Add(Traits.FoulAuraSoporific);
                    break;
                case 7:
                case 8:
                    traits.Add(Traits.FoulAuraToxic);
                    break;
                case 9:
                    traits.Add(Traits.Resilient);
                    break;
                case 10:
                    GenerateExoticSpeciesTrait();
                    break;                  
                    */  
            }
        }

        

    }

    class DiffuseFlora : Flora
    {       
        public DiffuseFlora()
        {
            weaponSkill = 30;
            ballisticSkill = 0;
            strength = 10;
            toughness = 20;
            agility = 25;
            intelligence = 0;
            perception = 15;
            willpower = 0;
            fellowship = 0;

            movement = 0;
            wounds = 24;

            skills = new List<Skills>();
            talents = new List<Talents>();
            traits = new List<Traits>();
            traits.Add(Traits.Diffuse);
            traits.Add(Traits.FromBeyond);
            traits.Add(Traits.NaturalWeapons);
            traits.Add(Traits.SizeEnormous);
            traits.Add(Traits.StrangePhysiology);

            armour = null;
            weapons = new List<Weapon>();
            weapons.Add(new Weapon("Thorns, Barbs or Tendrils", "1d10+1 R or I", 0));
            gear = new List<Gear>();
        }
    }

    class SmallFlora : Flora
    {
        public SmallFlora()
        {
            weaponSkill = 40;
            ballisticSkill = 0;
            strength = 35;
            toughness = 35;
            agility = 35;
            intelligence = 0;
            perception = 25;
            willpower = 0;
            fellowship = 0;

            movement = 0;
            wounds = 8;

            skills = new List<Skills>();
            talents = new List<Talents>();
            talents.Add(Talents.Sturdy);
            traits = new List<Traits>();            
            traits.Add(Traits.FromBeyond);
            traits.Add(Traits.NaturalWeapons);
            traits.Add(Traits.SizeScrawny);
            traits.Add(Traits.StrangePhysiology);

            armour = null;
            weapons = new List<Weapon>();
            weapons.Add(new Weapon("Thorns, Barbs or Tendrils", "1d10+2 R or I", 0));
            gear = new List<Gear>();
        }
    }

    class LargeFlora : Flora
    {
        public LargeFlora()
        {
            weaponSkill = 50;
            ballisticSkill = 0;
            strength = 50;
            toughness = 50;
            agility = 20;
            intelligence = 0;
            perception = 35;
            willpower = 0;
            fellowship = 0;

            movement = 0;
            wounds = 20;

            skills = new List<Skills>();
            talents = new List<Talents>();
            talents.Add(Talents.Sturdy);
            traits = new List<Traits>();
            traits.Add(Traits.FromBeyond);
            traits.Add(Traits.NaturalWeapons);
            traits.Add(Traits.SizeEnormous);
            traits.Add(Traits.StrangePhysiology);

            armour = new Armour("Bark or Rind", 0, 2, 0);
            weapons = new List<Weapon>();
            weapons.Add(new Weapon("Oversized Thorns, Barbs or Tendrils", "1d10+6 R or I", 0));
            gear = new List<Gear>();
        }
    }

    class MassiveFlora : Flora
    {
        public MassiveFlora()
        {
            weaponSkill = 45;
            ballisticSkill = 0;
            strength = 60;
            toughness = 75;
            agility = 15;
            intelligence = 0;
            perception = 20;
            willpower = 0;
            fellowship = 0;

            movement = 0;
            wounds = 40;

            skills = new List<Skills>();
            talents = new List<Talents>();
            talents.Add(Talents.Sturdy);
            traits = new List<Traits>();
            traits.Add(Traits.FromBeyond);
            traits.Add(Traits.ImprovedNaturalWeapons);
            traits.Add(Traits.SizeMassive);
            traits.Add(Traits.StrangePhysiology);
            traits.Add(Traits.SwiftAttack);

            armour = new Armour("Thick Bark or Rind", 0, 4, 0);
            weapons = new List<Weapon>();
            weapons.Add(new Weapon("Fearsome Thorns, Barbs or Tendrils", "1d10+9 R or I", 0));
            gear = new List<Gear>();
        }
    }

}
