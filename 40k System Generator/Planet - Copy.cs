using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH40K_System_Generator
{
    internal enum Body
    {
        [Description("Low Mass")]
        LowMass = 0,
        Small,
        [Description("Small and Dense")]
        SmallDense,
        Large,
        [Description("Large and Dense")]
        LargeDense,
        Vast,
        [Description("Gas Dwarf")]
        GasDwarf,
        [Description("Gas Giant")]
        GasGiant,
        [Description("Massive Gas Giant")]
        MassiveGasGiant
    };
    internal enum Gravity { Low = 0, Normal, High, Weak, Strong, Powerful, Titanic };
    internal enum AtmospherePresence { None = 0, Tin, Moderate, Heavy };
    internal enum AtmosphereComposition { Deadly = 0, Corrosive, Toxic, Tainted, Pure };
    internal enum Climate { Burning = 0, Hot, Temperate, Cold, Ice };

    internal enum Habitability { Inhospitable = 0,
        [Description("Trapped Water")] TrappedWater,
        [Description("Liquid Water")] LiquidWater,
        [Description("Limited Ecosystem")] LimitedEcosystem,
        Verdant };

    internal enum OrbitalFeature
    {
        [Description("No Features")] NoFeatures = 0,
        [Description("Planetary Rings (Debris)")] Debris,
        [Description("Planetary Rings (Dust)")] Dust,
        [Description("Large Asteroid")] Asteroid,
        [Description("Lesser Moon")] LesserMoon,
        Moon
    };

    class Planet
    {
        public List<Resource> resourcesAvailable = new List<Resource>();
        public int orbitalFeaturesAmt = 0;
        public List<OrbitalFeature> orbitalFeatures = new List<OrbitalFeature>();
        internal Gravity gravity;
        internal Body planetBody;
        internal AtmospherePresence atmospherePresence;
        internal AtmosphereComposition atmosphereComposition;
        internal Climate planetClimate;
        internal Habitability planetHabitability;
        internal bool isMoon = false;
        internal bool isLesserMoon = false;
        internal List<Planet> moonList = new List<Planet>();

        public Planet()
        {
        }
        
        public Planet(bool isMoon, bool isLesserMoon)
        {
            this.isMoon = isMoon;
            this.isLesserMoon = isLesserMoon;

            if (isLesserMoon)
            {
                // lesser moon 6+ has 5d10+5 Abundance of a Mineral Resource
                if (RNG.RandNumber(0,11)>=6)
                {
                    this.resourcesAvailable.Add(new MineralResource(RNG.RandNumber(4, 51) + 5));
                }
            }

            if (!isMoon && !isLesserMoon)
                generateOrbitalFeatures(this);
        }

        internal int GravityModifications(int roll, Gravity gravity)
        {
            if (this.GetType() == typeof(RockyPlanet))
            {
                if (gravity == Gravity.Low)
                    roll = Math.Min(0, roll - 10);
                else if (gravity == Gravity.High)
                    roll = Math.Min(100, roll + 10);
            }
            else if (this.GetType() == typeof(GasGiant))
            {

                if (gravity == Gravity.Weak)
                    roll = Math.Min(100, roll + 10);
                else if (gravity == Gravity.Strong)
                    roll = Math.Min(100, roll + 15);
                else if (gravity == Gravity.Powerful)
                    roll = Math.Min(100, roll + 20);
                else if (gravity == Gravity.Titanic)
                    roll = Math.Min(100, roll + 30);
            }

            return 0;
        }

        internal List<object> generateOrbitalFeatures(Planet planet)
        {
            if (this.isMoon || this.isLesserMoon)
                return null;

            List<object> orbitalFeatures = new List<object>();

            // Oribtal Features. Low Gravity = -10, High Gravity = +10
            for (int i = 1; i <= orbitalFeaturesAmt; i++)
            {
                int roll = RNG.RandNumber(0, 101);

                if (planet.GetType() == typeof(RockyPlanet))
                {
                    if (planet.gravity == Gravity.Low)
                        roll = Math.Min(0, roll - 10);
                    else if (planet.gravity == Gravity.High)
                        roll = Math.Min(100, roll + 10);

                    if (roll <= 45)
                        orbitalFeatures.Add(OrbitalFeature.NoFeatures);
                    else if (roll <= 60)
                        orbitalFeatures.Add(OrbitalFeature.Asteroid);
                    else if (roll <= 90)
                    {
                        orbitalFeatures.Add(OrbitalFeature.LesserMoon);
                        moonList.Add(new Planet(false, true));
                    }
                    else
                    {
                        orbitalFeatures.Add(OrbitalFeature.Moon);
                        moonList.Add(new Planet(true, false));
                    }
                }
                else if (planet.GetType() == typeof(GasGiant))
                {
                    if (planet.gravity == Gravity.Weak)
                        roll = Math.Min(100, roll + 10);
                    else if (planet.gravity == Gravity.Strong)
                        roll = Math.Min(100, roll + 15);
                    else if (planet.gravity == Gravity.Powerful)
                        roll = Math.Min(100, roll + 20);
                    else if (planet.gravity == Gravity.Titanic)
                        roll = Math.Min(100, roll + 30);

                    if (roll <= 20)
                        orbitalFeatures.Add(OrbitalFeature.NoFeatures);
                    else if (roll <= 35)
                        orbitalFeatures.Add(OrbitalFeature.Debris);
                    else if (roll <= 50)
                        orbitalFeatures.Add(OrbitalFeature.Dust);
                    else if (roll <= 85)
                    {
                        orbitalFeatures.Add(OrbitalFeature.LesserMoon);
                        moonList.Add(new Planet(false, true));
                    }
                    else if (roll <= 101)
                    {
                        orbitalFeatures.Add(OrbitalFeature.Moon);
                        moonList.Add(new Planet(true, false));
                    }
                }
            }

            return orbitalFeatures;
        }
    }

    class RockyPlanet : Planet
    {

        public RockyPlanet() : base()
        {
            int gravityRoll = RNG.RandNumber(0, 11);

            gravityRoll = GenerateRockyPlanetBody(gravityRoll);

            GenerateRockyPlanetGravity(gravityRoll);

        }

        private int GenerateRockyPlanetBody(int gravityRoll)
        {
            switch (RNG.RandNumber(0, 11))
            {
                case 1:
                    this.planetBody = Body.LowMass;
                    gravityRoll = Math.Min(gravityRoll - 7, 1);
                    break;
                case 2:
                case 3:
                    this.planetBody = Body.Small;
                    gravityRoll = Math.Min(gravityRoll - 5, 1);
                    break;
                case 4:
                    this.planetBody = Body.SmallDense;
                    break;
                case 5:
                case 6:
                case 7:
                    this.planetBody = Body.Large;
                    break;
                case 8:
                    this.planetBody = Body.LargeDense;
                    gravityRoll = Math.Min(gravityRoll + 5, 10);
                    break;
                case 9:
                case 10:
                    this.planetBody = Body.Vast;
                    gravityRoll = Math.Min(gravityRoll + 4, 10);
                    break;
                default:
                    this.planetBody = Body.SmallDense;
                    break;

            }

            return gravityRoll;
        }

        private void GenerateRockyPlanetGravity(int gravityRoll)
        {
            switch (gravityRoll)
            {
                case 1:
                case 2:
                    this.gravity = Gravity.Low;
                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 3, 1);
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    this.gravity = Gravity.Normal;
                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 2, 1);
                    break;
                case 9:
                case 10:
                    this.gravity = Gravity.High;
                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 1, 1);
                    break;
                default:
                    this.gravity = Gravity.Normal;
                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 2, 1);
                    break;
            }
        }
    }

    class GasGiant : Planet
    {

        public GasGiant() : base()
        {
            int gravityRoll = RNG.RandNumber(0, 11);
            // Generating Planet Type
            gravityRoll = GenerateGasGiant(gravityRoll);

            // Generating Gravity
            GenerateGasGiantGravity(gravityRoll);
        }

        private int GenerateGasGiant(int gravityRoll)
        {
            switch (RNG.RandNumber(0, 11))
            {
                case 1:
                case 2:
                    this.planetBody = Body.GasDwarf;
                    gravityRoll = Math.Min(1, gravityRoll - 5);
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    this.planetBody = Body.GasGiant;
                    break;
                case 9:
                case 10:
                    this.planetBody = Body.MassiveGasGiant;
                    gravityRoll = Math.Min(10, gravityRoll + 3);
                    break;
                default:
                    this.planetBody = Body.GasGiant;
                    break;
            }

            return gravityRoll;
        }

        private void GenerateGasGiantGravity(int gravityRoll)
        {
            switch (gravityRoll)
            {
                case 1:
                case 2:
                    this.gravity = Gravity.Weak;
                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 11) - 5, 1);
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    this.gravity = Gravity.Strong;
                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 10) - 3, 1);
                    break;
                case 7:
                case 8:
                case 9:
                    this.gravity = Gravity.Powerful;
                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 10) + 2, 1);
                    break;
                case 10:
                    this.gravity = Gravity.Titanic;
                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(3, 15) + 3, 1);
                    break;
                default:
                    this.gravity = Gravity.Strong;
                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 10) - 3, 1);
                    break;
            }
        }
    }

}
