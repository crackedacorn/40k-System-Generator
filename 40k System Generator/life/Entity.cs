using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace _40k_System_Generator.life
{
    // All entities in Rogue Trader have the following basic characterististics:
    public abstract class Entity
    {
        public string name;
        public string description;

        public int weaponSkill;
        public int ballisticSkill;
        public int strength;
        public int toughness;
        public int agility;
        public int intelligence;
        public int perception;
        public int willpower;
        public int fellowship;

        public int movement;
        public int wounds;

        public List<Skills> skills;
        public List<Talents> talents;
        public List<Traits> traits;

        public Armour armour;        
        public List<Weapon> weapons;
        public List<Gear> gear;


    }
        
    public enum Skills
    {
    }

    public enum Talents
    {
        [Description("Sturdy: hard to move and gain a +20 bonus to tests made to resist grappling and the Takedown talent.")]
        Sturdy
    }

    // Traits with description
    public enum Traits
    {
        [Description("From Beyond: Immune to Fear, Pinning, Insanity Points, and mind-affecting powers.")]
        FromBeyond,
        [Description("Diffuse: Any attack that does not have Blast, Flame or Scatter only inflicts half damage. Cannot be Knocked Down, Grappled, or Pinned. Counts as destroyed once all Wounds are lost.")]
        Diffuse,
        [Description("Natural Weapons: Unarmed attacks deal 1d10+SB damage.")]
        NaturalWeapons,
        [Description("Size(Enormous): +30 to strike in combat. -30 to concealment. AB+3 Base Movement.")]
        SizeEnormous,
        [Description("Strange Physiology: Death when damage equals Wounds.")]
        StrangePhysiology,
        [Description("Size(Scrawny): -10 to strike in combat. +10 to concealment. AB-1 Base Movement.")]
        SizeScrawny,
        [Description("Improved Natural Weapons: Multiples the damage of its Natural Weapons by 2.")]
        ImprovedNaturalWeapons,
        [Description("Size(Massive): +30 to strike in combat. -30 to concealment. AB+3 Base Movement.")]
        SizeMassive,
        [Description("Swift Attack: Attack twice with a Full Action.")]
        SwiftAttack

    };

    public class Weapon
    {
        string Description;
        string Roll;
        int Penetration;

        public Weapon(string description, string roll, int penetration)
        {
            Description = description;
            Roll = roll;
            Penetration = penetration;
        }
    }

    public class Armour
    {
        string Description;
        int Arms;
        int Body;
        int Legs;

        public Armour(string description, int arms, int body, int legs)
        {
            Description = description;
            Arms = arms;
            Body = body;
            Legs = legs;
        }
    }

    public class Gear
    {
        string Description;
    }
}
