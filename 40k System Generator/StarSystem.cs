using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WH40K_System_Generator;

namespace WH40K_System_Generator
{
    internal enum ZoneType {[Description("Primary Sphere")] PrimarySphere = 0, [Description("Inner Cauldron")] InnerCauldron, [Description("Outer Reaches")] OuterReaches };
    internal enum ZoneStrength { Normal = 0, Weak, Dominant };
    internal enum ResourceType { Mineral = 0, Archeotech, Xenos, Organic, Random };

    class StarSystem
    {
        public KeyFeatureType keyFeature;
        Star star;
        PrimaryBiosphere primarySphere;
        InnerCauldron innerCauldron;
        OuterReaches outerReaches;

        string notes;

        public StarSystem()
        {
            this.star = new Star();

            this.primarySphere = new PrimaryBiosphere(this.star.GetZoneStrength("primary"), ZoneType.PrimarySphere);
            this.innerCauldron = new InnerCauldron(this.star.GetZoneStrength("inner"), ZoneType.InnerCauldron);
            this.outerReaches = new OuterReaches(this.star.GetZoneStrength("outer"), ZoneType.OuterReaches);

            keyFeature = GenerateKeyFeature();


        }

        private List<StellarZoneElement> ListPlanets()
        {
            List<StellarZoneElement> planets = new List<StellarZoneElement>();

            foreach (StellarZoneElement p in innerCauldron.zoneElements)
                if (p.elementType == "Planet")
                    planets.Add(p);
            foreach (StellarZoneElement p in primarySphere.zoneElements)
                if (p.elementType == "Planet")
                    planets.Add(p);
            foreach (StellarZoneElement p in outerReaches.zoneElements)
                if (p.elementType == "Planet")
                    planets.Add(p);

            return planets;
        }

        private void AddArcheotech()
        {
            List<StellarZoneElement> planets = ListPlanets();
            int archeo = Math.Min(1, RNG.RandNumber(1, 5) - 1);
            int altArch = 0;

            if (planets.Count < archeo)
            {
                altArch = archeo - planets.Count;
                archeo = planets.Count;
            }
            for (int i = 0; i < archeo; i++)
            {
                planets[i].AddResources(1, ResourceType.Archeotech);
            }
            for (int i = 0; i < altArch; i++)
            {
                int r = RNG.RandNumber(1, 100);
                switch (RNG.RandNumber(1, 3))
                {
                    case 1:
                        if (r <= 50)
                            innerCauldron.AddElement(new StarshipGraveyard());
                        else
                            innerCauldron.AddElement(new DerelictStation());
                        break;
                    case 2:
                        if (r <= 50)
                            primarySphere.AddElement(new StarshipGraveyard());
                        else
                            primarySphere.AddElement(new DerelictStation());
                        break;
                    case 3:
                        if (r <= 50)
                            outerReaches.AddElement(new StarshipGraveyard());
                        else
                            outerReaches.AddElement(new DerelictStation());
                        break;
                }
            }


            foreach (StellarZoneElement s in innerCauldron.zoneElements)
            {
                foreach (Resource x in s.resourcesAvailable)
                {
                    if (x.name == "Archeotech")
                        x.abundance += RNG.RandNumber(1, 10) + 5;
                }
            }
            foreach (StellarZoneElement s in primarySphere.zoneElements)
            {
                foreach (Resource x in s.resourcesAvailable)
                {
                    if (x.name == "Archeotech")
                        x.abundance += RNG.RandNumber(1, 10) + 5;
                }
            }
            foreach (StellarZoneElement s in outerReaches.zoneElements)
            {
                foreach (Resource x in s.resourcesAvailable)
                {
                    if (x.name == "Archeotech")
                        x.abundance += RNG.RandNumber(1, 10) + 5;
                }
            }
        }

        private void AddXenosRuins()
        {
            List<StellarZoneElement> planets = ListPlanets();
            int ruins = Math.Min(1, RNG.RandNumber(1, 5) - 1);
            int altRuin = 0;

            if (planets.Count < ruins)
            {
                altRuin = ruins - planets.Count;
                ruins = planets.Count;
            }

            for (int i = 0; i < ruins; i++)
            {
                planets[i].AddResources(1, ResourceType.Xenos);
            }

            for (int i = 0; i < altRuin; i++)
            {
                int r = RNG.RandNumber(1, 100);
                switch (RNG.RandNumber(1, 3))
                {
                    case 1:
                        if (r <= 50)
                            innerCauldron.AddElement(new StarshipGraveyard());
                        else
                            innerCauldron.AddElement(new DerelictStation());
                        break;
                    case 2:
                        if (r <= 50)
                            primarySphere.AddElement(new StarshipGraveyard());
                        else
                            primarySphere.AddElement(new DerelictStation());
                        break;
                    case 3:
                        if (r <= 50)
                            outerReaches.AddElement(new StarshipGraveyard());
                        else
                            outerReaches.AddElement(new DerelictStation());
                        break;
                }
            }


            foreach (StellarZoneElement s in innerCauldron.zoneElements)
            {
                foreach (Resource x in s.resourcesAvailable)
                {
                    if (x.name == "Xenos")
                        x.abundance += RNG.RandNumber(1, 10) + 5;
                }
            }
            foreach (StellarZoneElement s in primarySphere.zoneElements)
            {
                foreach (Resource x in s.resourcesAvailable)
                {
                    if (x.name == "Xenos")
                        x.abundance += RNG.RandNumber(1, 10) + 5;
                }
            }
            foreach (StellarZoneElement s in outerReaches.zoneElements)
            {
                foreach (Resource x in s.resourcesAvailable)
                {
                    if (x.name == "Xenos")
                        x.abundance += RNG.RandNumber(1, 10) + 5;
                }
            }
        }

        public void RemovePlanets()
        {
            int removed = 0;

            List<StellarZoneElement> zoneList = new List<StellarZoneElement>();

            foreach (StellarZoneElement x in innerCauldron.zoneElements)
                zoneList.Add(x);

            foreach (StellarZoneElement p in zoneList)
                if (p.elementType == "Planet" && removed < 2)
                {
                    innerCauldron.zoneElements.Remove(p);
                    removed++;
                }

            zoneList.Clear();
            foreach (StellarZoneElement x in primarySphere.zoneElements)
                zoneList.Add(x);

            foreach (StellarZoneElement p in zoneList)
                if (p.elementType == "Planet" && removed < 2)
                {
                    primarySphere.zoneElements.Remove(p);
                    removed++;
                }

            zoneList.Clear();
            foreach (StellarZoneElement x in outerReaches.zoneElements)
                zoneList.Add(x);

            foreach (StellarZoneElement p in zoneList)
                if (p.elementType == "Planet" && removed < 2)
                {
                    outerReaches.zoneElements.Remove(p);
                    removed++;
                }
        }

        public KeyFeatureType GenerateKeyFeature()
        {
            KeyFeatureType kf = (KeyFeatureType)Math.Min(RNG.RandNumber(0, 10), 9);
            StellarZoneElement sze;

            switch (kf)
            {
                case KeyFeatureType.StellarAnomaly:
                    RemovePlanets();
                    break;
                case KeyFeatureType.Starfarers:
                    int p = ListPlanets().Count;
                    while (p < 4)
                    {
                        p++;
                        primarySphere.AddElement(new ZoneElementPlanet());
                    }
                    break;
                case KeyFeatureType.RuinedEmpire:
                    if (RNG.RandNumber(1, 100) < 50)
                        AddArcheotech();
                    else
                        AddXenosRuins();

                    break;
                case KeyFeatureType.Haven:
                    innerCauldron.AddElement(new ZoneElementPlanet());
                    primarySphere.AddElement(new ZoneElementPlanet());
                    outerReaches.AddElement(new ZoneElementPlanet());
                    break;
                case KeyFeatureType.GravityTides:
                    int riptides = RNG.RandNumber(1, 5);
                    while (riptides > 0)
                    {
                        riptides--;
                        switch (RNG.RandNumber(1, 3))
                        {
                            case 1:
                                innerCauldron.AddElement(new GravityRiptide());
                                break;
                            case 2:
                                primarySphere.AddElement(new GravityRiptide());
                                break;
                            case 3:
                                outerReaches.AddElement(new GravityRiptide());
                                break;
                        }
                    }
                    break;
                case KeyFeatureType.Bountiful:
                    if (RNG.RandNumber(0, 2) == 1)
                        sze = new AsteroidBelt();
                    else
                        sze = new AsteroidCluster();
                    if (RNG.RandNumber(0, 11) >= 6)
                        sze.AddResources(RNG.RandNumber(0, 6));

                    switch (RNG.RandNumber(0, 4))
                    {
                        case 1:
                            innerCauldron.AddElement(sze);
                            break;
                        case 2:
                            primarySphere.AddElement(sze);
                            break;
                        case 3:
                            outerReaches.AddElement(sze);
                            break;
                    }
                    break;
                default:
                    // None.
                    break;

            }

            return kf;
        }

        public override string ToString()
        {
            string s = string.Empty;

            if (this.star.starType == StarType.Binary)
            {
                if (this.star.binaryStarOne != this.star.binaryStarTwo)
                {
                    s += "Binary Star: " + this.star.binaryStarOne + ", " + this.star.binaryStarTwo;
                }
                else
                {
                    s += "Binar Star: 2x " + this.star.binaryStarTwo;
                }
            }
            else
            {
                s += "Star Type: " + this.star.starType;
            }

            s += "\n\nKey Feature: " + this.keyFeature.ToString();

            s += "\n\nInner Cauldron Elements: \n";
            foreach (StellarZoneElement element in this.innerCauldron.zoneElements)
                s += element.ToString() + "\n";

            s += "\n\nPrimary Sphere Elements: \n";
            foreach (StellarZoneElement element in this.primarySphere.zoneElements)
                s += element.ToString() + "\n";

            s += "\n\nOuter Reaches Elements: \n";
            foreach (StellarZoneElement element in this.outerReaches.zoneElements)
                s += element.ToString() + "\n";

            return s;
        }

    }

    class StellarZone
    {
        public List<StellarZoneElement> zoneElements = new List<StellarZoneElement>();
        ZoneStrength zoneStrength;
        ZoneType zoneType;

        public StellarZone(ZoneStrength zs, ZoneType zoneType)
        {
            this.zoneStrength = zs;
        }

        public StellarZone()
        {
            this.zoneStrength = ZoneStrength.Normal;
        }

        public virtual void AddElement(StellarZoneElement sze)
        {
            zoneElements.Add(sze);
        }

    }

    class InnerCauldron : StellarZone
    {

        public InnerCauldron(ZoneStrength zs, ZoneType zoneType) : base(zs, zoneType)
        {
            int count = RNG.RandNumber(1, 5);
            if (zs == ZoneStrength.Weak)
                count -= 2;
            else if (zs == ZoneStrength.Dominant)
                count += 2;

            while (count > 0)
            {
                int roll = RNG.RandNumber(1, 101);

                if (roll < 21)
                {
                    count--;
                    break;
                }
                else if (roll < 30)
                    AddElement(new AsteroidCluster());
                else if (roll < 42)
                    AddElement(new DustCloud());
                else if (roll < 46)
                    AddElement(new ZoneElementGasGiant());
                else if (roll < 57)
                    AddElement(new GravityRiptide());
                else if (roll < 77)
                    AddElement(new ZoneElementPlanet());
                else if (roll < 89)
                    AddElement(new RadiationBursts());
                else
                    AddElement(new SolarFlares());

                count--;
            }
        }

        public override void AddElement(StellarZoneElement sze)
        {
            zoneElements.Add(sze);
        }
    }

    class PrimaryBiosphere : StellarZone
    {

        public PrimaryBiosphere(ZoneStrength zs, ZoneType zoneType) : base(zs, zoneType)
        {
            int count = RNG.RandNumber(1, 5);
            if (zs == ZoneStrength.Weak)
                count -= 2;
            else if (zs == ZoneStrength.Dominant)
                count += 2;

            while (count > 0)
            {
                int roll = RNG.RandNumber(1, 101);

                if (roll < 21)
                {
                    count--;
                    break;
                }
                else if (roll < 31)
                    AddElement(new AsteroidBelt());
                else if (roll < 42)
                    AddElement(new AsteroidCluster());
                else if (roll < 48)
                    AddElement(new DerelictStation());
                else if (roll < 59)
                    AddElement(new DustCloud());
                else if (roll < 65)
                    AddElement(new GravityRiptide());
                else if (roll < 94)
                    AddElement(new ZoneElementPlanet());
                else
                    AddElement(new StarshipGraveyard());

                count--;
            }

        }

        public override void AddElement(StellarZoneElement sze)
        {
            zoneElements.Add(sze);
        }
    }

    class OuterReaches : StellarZone
    {

        public OuterReaches(ZoneStrength zs, ZoneType zoneType)
            : base(zs, zoneType)
        {
            int count = RNG.RandNumber(1, 5);
            if (zs == ZoneStrength.Weak)
                count -= 2;
            else if (zs == ZoneStrength.Dominant)
                count += 2;

            while (count > 0)
            {
                int roll = RNG.RandNumber(1, 101);

                if (roll < 21)
                {
                    count--;
                    break;
                }
                else if (roll < 30)
                    AddElement(new AsteroidBelt());
                else if (roll < 41)
                    AddElement(new AsteroidCluster());
                else if (roll < 47)
                    AddElement(new DerelictStation());
                else if (roll < 56)
                    AddElement(new DustCloud());
                else if (roll < 74)
                    AddElement(new ZoneElementGasGiant());
                else if (roll < 81)
                    AddElement(new GravityRiptide());
                else if (roll < 94)
                    AddElement(new ZoneElementPlanet());
                else
                    AddElement(new StarshipGraveyard());

                count--;
            }
        }

        public override void AddElement(StellarZoneElement sze)
        {
            zoneElements.Add(sze);
        }
    }

    class StellarZoneElement
    {
        public string elementType;
        public List<Resource> resourcesAvailable = new List<Resource>();

        public StellarZoneElement(string type)
        {
            this.elementType = type;
        }
        
        public void AddResources(int count, ResourceType type = ResourceType.Random, bool graveyard = false)
        {
            switch (type)
            {
                case ResourceType.Archeotech:
                    if (graveyard)
                        resourcesAvailable.Add(new Archeotech(Resource.GenerateAbundance(RNG.RandNumber(1, 10) + RNG.RandNumber(1, 10) + 25)));
                    else
                        resourcesAvailable.Add(new Archeotech(Resource.GenerateAbundance()));
                    break;
                case ResourceType.Mineral:
                    while (count > 0)
                    {
                        resourcesAvailable.Add(new MineralResource(Resource.GenerateAbundance()));
                        count--;
                    }
                    break;
                case ResourceType.Organic:
                case ResourceType.Xenos:
                    if (graveyard)
                        resourcesAvailable.Add(new XenosResource(Resource.GenerateAbundance(RNG.RandNumber(1, 10) + RNG.RandNumber(1, 10) + 25)));
                    else
                        resourcesAvailable.Add(new XenosResource(Resource.GenerateAbundance()));
                    break;
                default:
                    resourcesAvailable.Add(SystemResources.GenerateResource());
                    break;
            }
        }

        public override string ToString()
        {
            string s = "\tElement: " + this.elementType;

            if (resourcesAvailable.Count > 0)
            {
                s += "\n\t\tResources available: ";

                foreach (Resource r in resourcesAvailable) // Skip if r is empty
                {
                    if (r == null)
                        break;

                    s += "\n\t\t\t" + r.ToString();
                }
            }
            if (this.GetType() == typeof(ZoneElementPlanet))
                s += "\n"+ this.ToString();

            return s;
        }
    }

    class AsteroidBelt : StellarZoneElement
    {

        public AsteroidBelt() : base("Asteroid Belt")
        {
            int resourceCount = Math.Max(RNG.RandNumber(0, 6), 1);

            AddResources(resourceCount, ResourceType.Mineral);
        }
    }


    class AsteroidCluster : StellarZoneElement
    {
        public AsteroidCluster() : base("Asteroid Cluster")
        {
            int resourceCount = Math.Min(RNG.RandNumber(0, 6), 1);

            AddResources(resourceCount, ResourceType.Mineral);
        }
    }

    class DerelictStation : StellarZoneElement
    {
        public string StationType;

        public DerelictStation() : base("Derelict Station")
        {
            int roll = RNG.RandNumber(1, 100);

            if (roll < 11)
                this.StationType = "Egarian Void-Maze";
            else if (roll < 21)
                this.StationType = "Eldar Orrery";
            else if (roll < 31)
                this.StationType = "Eldar Gate";
            else if (roll < 41)
                this.StationType = "Ork Rok";
            else if (roll < 51)
                this.StationType = "STC Defence Station";
            else if (roll < 66)
                this.StationType = "STC Monitor Station";
            else if (roll < 76)
                this.StationType = "Stryxis Collection";
            else if (roll < 86)
                this.StationType = "Xenos Defence Station";
            else
                this.StationType = "Xenos Monitor Station";

        }
    }

    class DustCloud : StellarZoneElement
    {
        public DustCloud() : base("Dust Cloud")
        { }
    }

    class GravityRiptide : StellarZoneElement
    {
        public GravityRiptide() : base("Gravity Riptide")
        { }
    }

    class RadiationBursts : StellarZoneElement
    {
        public RadiationBursts() : base("Radiation Bursts")
        { }
    }

    class SolarFlares : StellarZoneElement
    {
        public SolarFlares() : base("Solar Flares")
        { }
    }

    class StarshipGraveyard : StellarZoneElement
    {
        public string graveyardOrigins;
        public int shipCount;

        public StarshipGraveyard() : base("Starship Graveyard")
        {
            int roll = RNG.RandNumber(1, 100);

            #region GraveyardSource
            if (roll < 16)
            {
                this.graveyardOrigins = "Crushed Defence Force/Routed Invasion";
                shipCount = RNG.RandNumber(1, 5) + RNG.RandNumber(1, 5);
            }
            else if (roll < 21)
            {
                this.graveyardOrigins = "Fleet Engagement";
                shipCount = RNG.RandNumber(1, 10) + 6;
            }
            else if (roll < 36)
            {
                this.graveyardOrigins = "Lost Explorers";
                shipCount = Math.Max(0, RNG.RandNumber(1, 6) - 2);
            }
            else if (roll < 66)
            {
                this.graveyardOrigins = "Plundered Convoy";
                shipCount = RNG.RandNumber(1, 5) + 2;
            }
            else if (roll < 91)
            {
                this.graveyardOrigins = "Skirmish";
                shipCount = RNG.RandNumber(1, 5) + 3;
            }
            else
            {
                this.graveyardOrigins = "Unknown Provenance";
                shipCount = RNG.RandNumber(1, 5);
            }
            #endregion


            int resourceCount = RNG.RandNumber(1, 10) + 2;

            roll = RNG.RandNumber(1, 100);

            while (resourceCount > 0)
            {

                if (roll <= 50)
                    AddResources(1, ResourceType.Xenos);
                else
                    AddResources(1, ResourceType.Archeotech);

                resourceCount--;
            }

        }

        public override string ToString()
        {
            string s = "\tElement: " + this.elementType;

            s += "\n\t\tSource: " + this.graveyardOrigins;

            if (resourcesAvailable.Count > 0)
            {
                s += "\n\t\tResources available: ";

                foreach (Resource r in resourcesAvailable) // Skip if r is empty
                {
                    if (r == null)
                        break;

                    s += "\n\t\t\t" + r.ToString();
                }
            }

            return s;
        }
    }

    class ZoneElementPlanet : StellarZoneElement
    {
        Planet planet;
        public Planet GetPlanet { get { return planet; } }

        public ZoneElementPlanet()
            : base("Planet")
        {
            this.planet = new RockyPlanet();
        }

        public override string ToString()
        {
            return planet.ToString();
        }

        
    }

    class ZoneElementGasGiant : StellarZoneElement
    {
        Planet planet;

        public ZoneElementGasGiant()
            : base("Gas Giant")
        {
            this.planet = new Planet();
        }

    }


}