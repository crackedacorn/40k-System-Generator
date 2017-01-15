using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH40K_System_Generator
{
    class Star
    {
        internal StarType starType;
        internal StarType binaryStarOne;
        internal StarType binaryStarTwo;
        internal StarType binaryPrimary;

        public Star()
        {
            switch (RNG.RandNumber(0, 11))
            {
                case 1:
                    starType = StarType.Mighty;
                    break;
                case 2:
                case 3:
                case 4:
                    starType = StarType.Vigorous;
                    break;
                case 5:
                case 6:
                case 7:
                    starType = StarType.Luminous;
                    break;
                case 8:
                    starType = StarType.Dull;
                    break;
                case 9:
                    starType = StarType.Anomalous;
                    break;
                case 10:
                    starType = StarType.Binary;

                    switch (RNG.RandNumber(0, 11))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            switch (RNG.RandNumber(0, 11))
                            {
                                case 1:
                                    binaryStarOne = binaryStarTwo = StarType.Mighty;
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    binaryStarOne = binaryStarTwo = StarType.Vigorous;
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                    binaryStarOne = binaryStarTwo = StarType.Luminous;
                                    break;
                                case 8:
                                    binaryStarOne = binaryStarTwo = StarType.Dull;
                                    break;
                                case 9:
                                    binaryStarOne = binaryStarTwo = StarType.Anomalous;
                                    break;
                            }
                            break;
                        case 8:
                        case 9:
                        case 10:
                            switch (RNG.RandNumber(0, 11))
                            {
                                case 1:
                                    binaryStarOne = StarType.Mighty;
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    binaryStarOne = StarType.Vigorous;
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                    binaryStarOne = StarType.Luminous;
                                    break;
                                case 8:
                                    binaryStarOne = StarType.Dull;
                                    break;
                                case 9:
                                    binaryStarOne = StarType.Anomalous;
                                    break;
                            }
                            switch (RNG.RandNumber(0, 11))
                            {
                                case 1:
                                    binaryStarTwo = StarType.Mighty;
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    binaryStarTwo = StarType.Vigorous;
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                    binaryStarTwo = StarType.Luminous;
                                    break;
                                case 8:
                                    binaryStarTwo = StarType.Dull;
                                    break;
                                case 9:
                                    binaryStarTwo = StarType.Anomalous;
                                    break;
                            }



                            break;

                    }

                    break;
            }


            if (starType == StarType.Binary)
                binaryPrimary = (binaryStarOne > binaryStarTwo ? binaryStarOne : binaryStarTwo);
        }

        public ZoneStrength GetZoneStrength(string zone)
        {
            switch(this.starType)
            {
                case StarType.Anomalous:
                case StarType.Binary:
                case StarType.Dull:
                case StarType.Luminous:
                case StarType.Mighty:
                case StarType.Vigorous:
                default:
                    return ZoneStrength.Normal;
            }
        }
        
    }
}
