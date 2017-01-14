using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH40K_System_Generator
{

    class SystemResources
    {
        public static Resource GenerateResource()
        {
            Resource newResource=null;

            int abundance = Resource.GenerateAbundance();

            switch(RNG.RandNumber(0,10))
            {
                case 1:
                case 2:
                    newResource = new Archeotech(abundance);
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    newResource = new MineralResource(abundance);
                    break;
                case 7:
                case 8:
                    newResource = new OrganicResource(abundance);
                    break;
                case 9:
                case 10:
                    newResource = new XenosResource(abundance);
                    break;
            }

            return newResource;
        }
    }

    class Resource
    {
        internal int abundance;
        public string name;

        public string AbundanceName()
        {
            if (abundance < 15)
                return "Minimal";
            else if (abundance < 41)
                return "Limited";
            else if (abundance < 66)
                return "Sustainable";
            else if (abundance < 86)
                return "Significant";
            else if (abundance < 99)
                return "Major";
            else
                return "Plentiful";
        }

        public static int GenerateAbundance(int roll=0)
        {
            return RNG.RandNumber(0, 101);
        }

        public Resource(int abundance, string name)
        {
            this.abundance = abundance;
            this.name = name;
        }

    }

    class Archeotech : Resource
    {
        public Archeotech(int abundance) : base (abundance, "Archeotech")
        {

        }

        public override string ToString()
        {
            return "Archotech (" + base.AbundanceName() + ")";
        }
    }

    class MineralResource : Resource
    {
        string type;

        public MineralResource(int abundance) : base (abundance, "Mineral")
        {
            switch (RNG.RandNumber(0,11))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    this.type = "Industrial Metal";
                    break;
                case 5:
                case 6:
                case 7:
                    this.type = "Ornamental";
                    break;
                case 8:
                case 9:
                    this.type = "Radioactive";
                    break;
                case 10:
                default:
                    this.type = "Exotic Material";
                    break;
            }
        }

        public override string ToString()
        {
            return "Mineral Resource - " + this.type + " (" + base.AbundanceName() + ")";
        }

    }

    class OrganicResource : Resource
    {
        string type;

        public OrganicResource(int abundance)
            : base(abundance, "Organic")
        {
            switch (RNG.RandNumber(0, 11))
            {
                case 1:
                case 2:
                    this.type = "Curative";
                    break;
                case 3:
                case 4:
                    this.type = "Juvenat Compound";
                    break;
                case 5:
                case 6:
                    this.type = "Toxin";
                    break;
                case 7:
                case 8:
                case 9:
                    this.type = "Vivid Accessory";
                    break;
                case 10:
                    this.type = "Exotic Compound";
                    break;
            }
        }

        public override string ToString()
        {
            return "Organic Resource - " + this.type + " (" + base.AbundanceName() + ")";
        }
    }

    class XenosResource : Resource
    {
        string type;

        public XenosResource(int abundance) : base(abundance, "Xenos")
        {
            switch (Math.Min(10,RNG.RandNumber(0, 10)))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    this.type = "Undiscovered Species";
                    break;
                case 5:
                case 6:
                    this.type = "Eldar";
                    break;
                case 7:
                    this.type = "Egarian";
                    break;
                case 8:
                    this.type = "Yu'Vath";
                    break;
                case 9:
                    this.type = "Ork";
                    break;
                case 10:
                    this.type = "Kroot";
                    break;
                default:
                    this.type = "Undiscovered Species";
                    break;
            }
        }

        public override string ToString()
        {
            return "Xenos Ruins - " + this.type + " (" + base.AbundanceName() + ")";
        }
    }
}
