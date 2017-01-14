//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WH40K_System_Generator
//{
//    internal enum PlanetBody
//    {
//        [Description("Low Mass")]
//        LowMass = 0,
//        Small,
//        [Description("Small and Dense")]
//        SmallDense,
//        Large,
//        [Description("Large and Dense")]
//        LargeDense,
//        Vast
//    };
//    internal enum GasGiantBody
//    {
//        [Description("Gas Dwarf")]
//        GasDwarf = 0,
//        [Description("Gas Giant")]
//        GasGiant,
//        [Description("Massive Gas Giant")]
//        MassiveGasGiant
//    };
//    internal enum PlanetGravity { Low = 0, Normal, High };
//    internal enum GasGiantGravity { Weak = 0, Strong, Powerful, Titanic };
//    internal enum AtmospherePresence { None = 0, Tin, Moderate, Heavy };
//    internal enum AtmosphereComposition { Deadly = 0, Corrosive, Toxic, Tainted, Pure };
//    internal enum Climate { Burning = 0, Hot, Temperate, Cold, Ice };

//    internal enum Habitability { Inhospitable = 0,
//        [Description("Trapped Water")] TrappedWater,
//        [Description("Liquid Water")] LiquidWater,
//        [Description("Limited Ecosystem")] LimitedEcosystem,
//        Verdant };

//    internal enum OrbitalFeature
//    {
//        [Description("No Features")] NoFeatures =0,
//        [Description("Planetary Rings (Debris)")] Debris,
//        [Description("Planetary Rings (Dust)")] Dust,
//        [Description("Large Asteroid")] Asteroid,
//        [Description("Lesser Moon")] LesserMoon,
//        Moon
//    };

//    class Planet
//    {
//        public List<Resource> resourcesAvailable = new List<Resource>();
//        public int orbitalFeaturesAmt = 0;
//        public List<object> orbitalFeatures = new List<object>();
//        internal PlanetGravity planetGravity;
//        internal GasGiantGravity gasGiantGravity;
//        internal PlanetBody planetBody;
//        internal AtmospherePresence atmospherePresence;
//        internal AtmosphereComposition atmosphereComposition;
//        internal Climate planetClimate;
//        internal Habitability planetHabitability;

//        public Planet()
//        {
//            this.orbitalFeatures = generateOrbitalFeatures(this);
//        }

//        public override string ToString()
//        {
//            string s = string.Empty;

//            s += "\n\t\tPlanet Type: " + this.planetBody.ToString();
//            s += "\n\t\tGravity: " + this.planetGravity.ToString();
//            s += "\n\t\tAtmosphere: " + this.atmosphereComposition.ToString() + "(" + this.atmospherePresence.ToString() + ")";
//            s += "\n\t\tClimate: " + this.planetClimate.ToString();
//            s += "\n\t\tHabitability: " + this.planetHabitability.ToString();


//            if (orbitalFeaturesAmt > 0 && orbitalFeatures.LongCount() > 0)
//            {
//                s += "\n\t\tFeatures:";
//                foreach (object f in orbitalFeatures)
//                {
//                    s += "\n\t\t\t" + f.ToString();
//                }
//            }

//            return s;
//        }

//        internal List<object> generateOrbitalFeatures(Planet planet)
//        {
//            List<object> orbitalFeatures = new List<object>();

//            // Oribtal Features. Low Gravity = -10, High Gravity = +10
//            for (int i = 1; i <= orbitalFeaturesAmt; i++)
//            {
//                int r = RNG.RandNumber(0, 101);


//                if (planet.GetType() == typeof(RockyPlanet))
//                {
//                    if (planet.planetGravity == PlanetGravity.Low)
//                        r = Math.Min(0, r - 10);
//                    else if (planet.planetGravity == PlanetGravity.High)
//                        r = Math.Min(100, r + 10);

//                    if (r <= 45)
//                        orbitalFeatures.Add(new NoFeature());
//                    else if (r <= 60)
//                        orbitalFeatures.Add(new LargeAsteroid());
//                    else if (r <= 90)
//                        orbitalFeatures.Add(new LesserMoon());
//                    else
//                        orbitalFeatures.Add(new TrueMoon());
//                }
//                else if (planet.GetType() == typeof(GasGiant))
//                {
//                    if (planet.gasGiantGravity == GasGiantGravity.Weak)
//                        r = Math.Min(100, r + 10);
//                    else if (planet.gasGiantGravity == GasGiantGravity.Strong)
//                        r = Math.Min(100, r + 15);
//                    else if (planet.gasGiantGravity==GasGiantGravity.Powerful)
//                        r = Math.Min(100, r + 20);
//                    else if (planet.gasGiantGravity==GasGiantGravity.Titanic)
//                        r = Math.Min(100, r + 30);

//                    if (r <= 20)
//                        orbitalFeatures.Add(new NoFeature());
//                    else if (r <= 35)
//                        orbitalFeatures.Add(new PlanetaryRings("Debris"));
//                    else if (r <= 50)
//                        orbitalFeatures.Add(new PlanetaryRings("Dust"));
//                    else if (r <= 85)
//                        orbitalFeatures.Add(new LesserMoon());
//                    else if (r <= 101)
//                        orbitalFeatures.Add(new TrueMoon());
//                }
//            }

//            return orbitalFeatures;
//        }
//    }

//    class RockyPlanet : Planet
//    {

//        public RockyPlanet() : base ()
//        {
//            int gravityRoll = RNG.RandNumber(0, 11);

//            switch (RNG.RandNumber(0,11))
//            {
//                case 1:
//                    this.planetBody = PlanetBody.LowMass;
//                    gravityRoll = Math.Min(gravityRoll - 7, 1);
//                    break;
//                case 2:
//                case 3:
//                    this.planetBody = PlanetBody.Small;
//                    gravityRoll = Math.Min(gravityRoll - 5, 1);
//                    break;
//                case 4:
//                    this.planetBody = PlanetBody.SmallDense;
//                    break;
//                case 5:
//                case 6:
//                case 7:
//                    this.planetBody = PlanetBody.Large;
//                    break;
//                case 8:
//                    this.planetBody = PlanetBody.LargeDense;
//                    gravityRoll = Math.Min(gravityRoll + 5, 10);
//                    break;
//                case 9:
//                case 10:
//                    this.planetBody = PlanetBody.Vast;
//                    gravityRoll = Math.Min(gravityRoll + 4, 10);
//                    break;
//                default:
//                    this.planetBody = PlanetBody.SmallDense;
//                    break;

//            }

//            switch (gravityRoll)
//            {
//                case 1:
//                case 2:
//                    this.planetGravity=PlanetGravity.Low;
//                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 3, 1);
//                    break;
//                case 3:
//                case 4:
//                case 5:
//                case 6:
//                case 7:
//                case 8:
//                    this.planetGravity = PlanetGravity.Normal;
//                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 2, 1);
//                    break;
//                case 9:
//                case 10:
//                    this.planetGravity = PlanetGravity.High;
//                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 1, 1);
//                    break;
//                default:
//                    this.planetGravity = PlanetGravity.Normal;
//                    this.orbitalFeaturesAmt = Math.Max(RNG.RandNumber(0, 6) - 2, 1);
//                    break;
//            }


//        }
//    }

//    class GasGiant : Planet
//    {
//        internal GasGiantBody gasGiantBody;

//        public GasGiant() : base()
//        {
//            int gravityRoll = RNG.RandNumber(0, 11);


//            // Generating Planet Type
//            switch (RNG.RandNumber(0,11))
//            {
//                case 1:
//                case 2:
//                    this.gasGiantBody = GasGiantBody.GasDwarf;
//                    gravityRoll = Math.Min(1, gravityRoll - 5);
//                    break;
//                case 3:
//                case 4:
//                case 5:
//                case 6:
//                case 7:
//                case 8:
//                    this.gasGiantBody = GasGiantBody.GasGiant;
//                    break;
//                case 9:
//                case 10:
//                    this.gasGiantBody = GasGiantBody.MassiveGasGiant;
//                    gravityRoll = Math.Min(10, gravityRoll + 3);
//                    break;
//                default:
//                    this.gasGiantBody = GasGiantBody.GasGiant;
//                    break;
//            }


//            // Generating Gravity
//            switch (gravityRoll)
//            {
//                case 1:
//                case 2:
//                    this.gasGiantGravity = GasGiantGravity.Weak;
//                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 11) - 5, 1);
//                    break;
//                case 3:
//                case 4:
//                case 5:
//                case 6:
//                    this.gasGiantGravity = GasGiantGravity.Strong;
//                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 10) - 3, 1);
//                    break;
//                case 7:
//                case 8:
//                case 9:
//                    this.gasGiantGravity = GasGiantGravity.Powerful;
//                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 10) + 2, 1);
//                    break;
//                case 10:
//                    this.gasGiantGravity = GasGiantGravity.Titanic;
//                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(3, 15) + 3, 1);
//                    break;
//                default:
//                    this.gasGiantGravity = GasGiantGravity.Strong;
//                    this.orbitalFeaturesAmt = Math.Min(RNG.RandNumber(0, 10) - 3, 1);
//                    break;
//            }
//        }
//    }

//    class TrueMoon : RockyPlanet
//    {
//        public TrueMoon() : base ()
//        {

//        }

//        public override string ToString()
//        {
//            return "Moon " + base.ToString();
//        }
//    }

//    class Feature
//    {
        
//    }

//    class NoFeature : Feature
//    {
//        public override string ToString()
//        {
//            return base.ToString();
//        }
//    }

//    class LargeAsteroid : Feature
//    {
//        public override string ToString()
//        {
//            return "Large Asteroid" + base.ToString();
//        }
//    }

//    class LesserMoon : Feature
//    {
//        public List<Resource> resourcesAvailable = new List<Resource>();

//        public override string ToString()
//        {
//            return "Lesser Moon" + base.ToString();
//        }
//    }

//    class PlanetaryRings : Feature
//    {
//        string type;

//        public PlanetaryRings(string type)
//        {
//            this.type = type;
//        }


//        public override string ToString()
//        {
//            return "Planetary Rings ("+type+")"+ base.ToString();
//        }
//    }
    


//}
