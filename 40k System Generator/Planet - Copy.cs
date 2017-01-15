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
    internal enum AtmospherePresence { None = 0, Thin, Moderate, Heavy };
    internal enum AtmosphereComposition { Deadly = 0, Corrosive, Toxic, Tainted, Pure };
    internal enum Climate { Burning = 0, Hot, Temperate, Cold, Ice };
    internal enum LandmassType {[Description("Multiple Landmasess")] multipleLandmasses = 0, [Description("Multiple Major Landmasses")] multileMajorLandmasses, [Description("Supercontinent")] superContinent };
    internal enum TerritoryBaseTerrain { Forest, Mountain, Plains, Swamp, Wasteland };


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
        internal bool isMoon = false;
        internal bool isLesserMoon = false;
        internal List<Planet> moonList = new List<Planet>();

        public Planet()
        {
            generateOrbitalFeatures(this);

            generateResources();
        }

        public Planet(bool isMoon, bool isLesserMoon)
        {
            this.isMoon = isMoon;
            this.isLesserMoon = isLesserMoon;

            if (isLesserMoon)
            {
                // lesser moon 6+ has 5d10+5 Abundance of a Mineral Resource
                if (RNG.RandNumber(0, 11) >= 6)
                {
                    this.resourcesAvailable.Add(new MineralResource(RNG.RandNumber(4, 51) + 5));
                }
            }
        }

        internal void generateResources()
        {
            int baseResourcesAmt = 0;
            int additionalResourcesAmt = 0;


            if (this.planetBody == Body.Small || this.planetBody == Body.SmallDense)
            {
                baseResourcesAmt = Math.Max(0, RNG.RandNumber(1, 6) - 2);
                additionalResourcesAmt = Math.Max(0, RNG.RandNumber(1, 6) - 3);
            }
            else if (this.planetBody == Body.Large || this.planetBody == Body.LargeDense)
            {
                baseResourcesAmt = RNG.RandNumber(0, 6);
                additionalResourcesAmt = Math.Max(0, RNG.RandNumber(1, 6) - 2);
            }
            else if (this.planetBody == Body.Vast)
            {
                baseResourcesAmt = RNG.RandNumber(0, 11);
                additionalResourcesAmt = Math.Max(0, RNG.RandNumber(1, 6) - 1);
            }

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

                GravityModifications(roll, this.gravity);

                if (planet.GetType() == typeof(RockyPlanet))
                {
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
        internal List<Territory> territories = new List<Territory>();
        internal AtmospherePresence atmospherePresence;
        internal AtmosphereComposition atmosphereComposition;
        internal Climate planetClimate;
        internal Habitability planetHabitability;
        internal LandmassType landmasses;
        int numberOfContinents;

        public RockyPlanet(ZoneType zone, bool isMoon) : base(isMoon, false)
        {
            int roll = RNG.RandNumber(0, 11);

            roll = GenerateRockyPlanetBody(roll);

            GenerateRockyPlanetGravity(roll);

            generateAtmosphere();

            GenerateClimate(zone);

            if (this.atmospherePresence != AtmospherePresence.None && (this.atmosphereComposition == AtmosphereComposition.Tainted || this.atmosphereComposition == AtmosphereComposition.Pure))
                GenerateHabitability();

            GenerateLandmasses();
            
        }

        public override string ToString()
        {
            string returnString = string.Empty;

            returnString = "\tElement: Rocky Planet";
            returnString += "\n\t\tAtmosphere: " + atmosphereComposition.ToString() + "(" + atmospherePresence.ToString() + ")";
            returnString += "\n\t\tClimate: " + planetClimate.ToString();
            returnString += "\n\t\tHabitability: " + planetHabitability.ToString();
            returnString += "\n\t\tLandmasses: " + landmasses.ToString();
            if (landmasses != LandmassType.superContinent && numberOfContinents>0)
                returnString += " (" + numberOfContinents + ")";
            if (territories.LongCount() > 0)
            {
                returnString += "\n\t\tEnvironments:";
                foreach (Territory t in territories)
                {
                    returnString += t.ToString();
                }
            }

            return returnString;
        }

        private void GenerateEnvironments()
        {
            if (this.planetHabitability != Habitability.LimitedEcosystem && this.planetHabitability != Habitability.Verdant && this.planetHabitability!=Habitability.LiquidWater)
                return;

            int numberOfTerritories = RNG.RandNumber(1, 6);

            if (this.planetBody == Body.Small)
                numberOfTerritories -= 2;
            if (this.planetBody == Body.Vast)
                numberOfContinents += 3;
            if (this.planetHabitability == Habitability.Verdant)
                numberOfContinents += 2;

            if (numberOfTerritories <= 0)
                return;

            for (int i = 1; i <= numberOfTerritories; i++)
            {
                territories.Add(new Territory());
            }
        }

        private void GenerateLandmasses()
        {
            int roll = RNG.RandNumber(0, 11);
            if (roll >= 8)
            {
                this.landmasses = LandmassType.multipleLandmasses;
                this.numberOfContinents = Math.Max(1, RNG.RandNumber(0, 6));
            }
            else if (roll >= 4 && (this.planetHabitability == Habitability.LiquidWater || this.planetHabitability == Habitability.Verdant))
            {
                this.landmasses = LandmassType.multileMajorLandmasses;
                this.numberOfContinents = Math.Max(1, RNG.RandNumber(0, 6));
            }
            else if (this.planetHabitability == Habitability.LiquidWater || this.planetHabitability == Habitability.Verdant)
                this.landmasses = LandmassType.superContinent;
            else
            {
                switch (RNG.RandNumber(1,4))
                {
                    case 1:
                        this.landmasses = LandmassType.multipleLandmasses;
                        this.numberOfContinents = Math.Max(1, RNG.RandNumber(0, 6));
                        break;
                    case 2:
                        this.landmasses = LandmassType.multileMajorLandmasses;
                        this.numberOfContinents = Math.Max(1, RNG.RandNumber(0, 6));
                        break;
                    case 3:
                        this.landmasses = LandmassType.superContinent;
                        break;
                    default:
                        this.landmasses = LandmassType.multipleLandmasses;
                        this.numberOfContinents = Math.Max(1, RNG.RandNumber(0, 6));
                        break;
                }
            }

            GenerateEnvironments();
        }

        private void GenerateHabitability()
        {
            int roll = RNG.RandNumber(0, 11);

            if (this.planetClimate == Climate.Hot || this.planetClimate == Climate.Cold)
                roll -= 2;
            if (this.planetClimate == Climate.Burning || this.planetClimate == Climate.Ice)
            {
                roll = Math.Min(7, roll - 7);
            }

            if (roll <= 1)
                this.planetHabitability = Habitability.Inhospitable;
            else if (roll <= 3)
                this.planetHabitability = Habitability.TrappedWater;
            else if (roll <= 5)
                this.planetHabitability = Habitability.LiquidWater;
            else if (roll <= 7)
                this.planetHabitability = Habitability.LimitedEcosystem;
            else
                this.planetHabitability = Habitability.Verdant;

        }

        private void GenerateClimate(ZoneType zone)
        {
            if (this.atmospherePresence==AtmospherePresence.None)
            {
                if (zone == ZoneType.InnerCauldron)
                    this.planetClimate = Climate.Burning;
                else if (zone == ZoneType.OuterReaches)
                    this.planetClimate = Climate.Ice;
                else
                {
                    if (RNG.RandNumber(0, 100) < 51)
                        this.planetClimate = Climate.Burning;
                    else
                        this.planetClimate = Climate.Ice;
                }

                return;
            }

            int roll = RNG.RandNumber(0, 11);

            if (zone == ZoneType.InnerCauldron)
                roll -= 6;
            else if (zone == ZoneType.OuterReaches)
                roll += 6;

            if (roll <= 0)
                this.planetClimate = Climate.Burning;
            else if (roll <= 3)
                this.planetClimate = Climate.Hot;
            else if (roll <= 7)
                this.planetClimate = Climate.Temperate;
            else if (roll <= 10)
                this.planetClimate = Climate.Cold;
            else
                this.planetClimate = Climate.Ice;
        }

        private void generateAtmosphere()
        {
            // Low gravity = -2 atmophshere presance, high = +1
            int atmosphereRoll = RNG.RandNumber(0, 11);
            if (this.gravity == Gravity.Low)
                atmosphereRoll -= 2;
            else if (this.gravity == Gravity.High)
                atmosphereRoll += 1;

            if (atmosphereRoll <= 1)
            {
                this.atmospherePresence = AtmospherePresence.None;
                return;
            }
            else if (atmosphereRoll <= 4)
                this.atmospherePresence = AtmospherePresence.Thin;
            else if (atmosphereRoll <= 9)
                this.atmospherePresence = AtmospherePresence.Moderate;
            else
                this.atmospherePresence = AtmospherePresence.Heavy;

            atmosphereRoll = RNG.RandNumber(0, 11);
            if (atmosphereRoll == 1)
                this.atmosphereComposition = AtmosphereComposition.Deadly;
            else if (atmosphereRoll == 2)
                this.atmosphereComposition = AtmosphereComposition.Corrosive;
            else if (atmosphereRoll <= 5)
                this.atmosphereComposition = AtmosphereComposition.Toxic;
            else if (atmosphereRoll <= 7)
                this.atmosphereComposition = AtmosphereComposition.Tainted;
            else
                this.atmosphereComposition = AtmosphereComposition.Pure;
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


    class Territory
    {
        internal Tuple<string,string,TerritoryBaseTerrain> baseTerrain;
        OrganicResource resource = null;

        public Territory()
        {
            int roll = RNG.RandNumber(0, 6);
            int traitRoll = RNG.RandNumber(0, 101);
            string trait1 = string.Empty;
            string trait2 = string.Empty;
            TerritoryBaseTerrain terrain;

            if (roll == 1)
            {
                terrain = TerritoryBaseTerrain.Forest;
            }
            else if (roll == 2)
                terrain = TerritoryBaseTerrain.Mountain;
            else if (roll == 3)
                terrain = TerritoryBaseTerrain.Plains;
            else if (roll == 4)
                terrain = TerritoryBaseTerrain.Swamp;
            else
                terrain = TerritoryBaseTerrain.Wasteland;


            if (traitRoll < 96)
                trait1 = GenerateTrait(traitRoll, terrain);
            else // If Roll > 96, roll two traits.
            {
                while (traitRoll >= 96)
                    traitRoll = RNG.RandNumber(0, 101);

                roll = traitRoll;
                traitRoll = RNG.RandNumber(0, 101);

                trait1 = GenerateTrait(traitRoll, terrain);
                while (traitRoll >= 96 || traitRoll == roll)
                    traitRoll = RNG.RandNumber(0, 101);

                trait2 = GenerateTrait(traitRoll, terrain);
            }

            this.baseTerrain = new Tuple<string, string, TerritoryBaseTerrain>(trait1, trait2, terrain);
        }

        public override string ToString()
        {
            string returnString = string.Empty;
            string trait1 = this.baseTerrain.Item1;
            string trait2 = this.baseTerrain.Item2;
            TerritoryBaseTerrain terrain = this.baseTerrain.Item3;

            if (trait2 == string.Empty)
                returnString = "\n\t\t\tEnvironment: " + terrain.ToString() + " (" + trait1 + ")";
            else
                returnString = "\n\t\t\tEnvironment: " + terrain.ToString() + " (" + trait1 + ", "+trait2+")";

            if (terrain==TerritoryBaseTerrain.Forest)
            {
                if (trait1 == "Unique Compound")
                    returnString += "\n\t\t\t\tResource: " + resource.ToString();
                if (trait2 == "Unique Compound")
                    returnString += "\n\t\t\t\tResource: " + resource.ToString();
            }

            return returnString;
        }

        internal string GenerateTrait(int roll, TerritoryBaseTerrain terrain)
        {
            string trait = string.Empty;

            if (terrain == TerritoryBaseTerrain.Forest)
            {
                if (roll <= 5)
                    trait = "Exotic Nature";
                else if (roll <= 25)
                    trait = "Expansive";
                else if (roll <= 40)
                    trait = "Extreme Temperature";
                else if (roll <= 65)
                    trait = "Notable Species";
                else if (roll <= 80)
                {
                    trait = "Unique Compound";
                    resource = new OrganicResource(RNG.RandNumber(1, 101));
                }
                else if (roll <= 95)
                    trait = "Unusual Location";
            }
            else if (terrain == TerritoryBaseTerrain.Mountain)
            {
                if (roll <= 25)
                    trait = "Boundary";
                else if (roll <= 50)
                    trait = "Expansive";
                else if (roll <= 65)
                    trait = "Extreme Temperature";
                else if (roll <= 75)
                    trait = "Foothills";
                else if (roll <= 5)
                    trait = "Notable Species";
                else if (roll <= 95)
                    trait = "Unusual Location";
            }
            else if (terrain == TerritoryBaseTerrain.Plains)
            {
                if (roll <= 10)
                    trait = "Broken Ground";
                else if (roll <= 30)
                    trait = "Expansive";
                else if (roll <= 45)
                    trait = "Extreme Temperature";
                else if (roll <= 70)
                    trait = "Fertile";
                else if (roll <= 85)
                    trait = "Notable Species";
                else if (roll <= 95)
                    trait = "Unusual Location";
            }
            else if (terrain == TerritoryBaseTerrain.Swamp)
            {
                if (roll <= 10)
                    trait = "Expansive";
                else if (roll <= 30)
                    trait = "Extreme Temperature";
                else if (roll <= 45)
                    trait = "Notable Species";
                else if (roll <= 65)
                    trait = "Stagnant";
                else if (roll <= 75)
                    trait = "Unusual Location";
                else if (roll <= 95)
                    trait = "Virulent";
            }
            else if (terrain == TerritoryBaseTerrain.Wasteland)
            {
                if (roll <= 20)
                    trait = "Desolate";
                else if (roll <= 40)
                    trait = "Expansive";
                else if (roll <= 70)
                    trait = "Extreme Temperature";
                else if (roll <= 75)
                    trait = "Notable Species";
                else if (roll <= 80)
                    trait = "Ruined";
                else if (roll <= 95)
                    trait = "Unusual Location";
            }

            return trait;
        }
    }
}
