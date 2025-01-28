using System;
using System.Linq;
using System.Threading.Tasks;
using PuppeteerSharp;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Utilities.Extensions
{
    public static class MeasurementExtensions
    {

        public static double GetTypeConversion(Ingredient ingredient, int type, int defaultType)
        {
            switch(type)
            {
                case 2: /* Weight */
                    if(defaultType == 3) 
                    {
                        return ingredient.OneMilliliterInGram.GetValueOrDefault();
                    }
                    else if (defaultType == 4) 
                    {
                        return ingredient.OneCentimeterInGram.GetValueOrDefault();
                    }
                    else if (defaultType == 5)
                    {
                        return ingredient.OnePieceInGram.GetValueOrDefault();
                    }
                    return 0;
                case 3: /* Volume */
                    if (defaultType == 2)
                    {
                        return ingredient.OneCentimeterInGram.GetValueOrDefault();
                    }
                    else if (defaultType == 4)
                    {
                        return ingredient.OneCentimeterInMilliliter.GetValueOrDefault();
                    }
                    else if (defaultType == 5)
                    {
                        return ingredient.OnePieceInMilliliter.GetValueOrDefault();
                    }
                    return 0;
                case 4: /* Length */
                    if (defaultType == 2)
                    {
                        return ingredient.OneCentimeterInGram.GetValueOrDefault();
                    }
                    else if (defaultType == 3)
                    {
                        return ingredient.OneCentimeterInMilliliter.GetValueOrDefault();
                    }
                    else if (defaultType == 5)
                    {
                        return ingredient.OnePieceInCentimeter.GetValueOrDefault();
                    }
                    return 0;
                case 5: /* Quantity */
                    if (defaultType == 2)
                    {
                        return ingredient.OnePieceInGram.GetValueOrDefault();
                    }
                    else if (defaultType == 3)
                    {
                        return ingredient.OnePieceInMilliliter.GetValueOrDefault();
                    }
                    else if (defaultType == 4)
                    {
                        return ingredient.OnePieceInCentimeter.GetValueOrDefault();
                    }
                    return 0;
                default:
                    return 0;
            }
        }
    }
}