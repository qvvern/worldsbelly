/* eslint-disable */
const calculatePlugin = {
  install(Vue) {
      Vue.prototype.$calculate = {};

      function getTypeConversion(ingredient, type, defaultType) {
        switch(type)
        {
            case 2: /* Weight */
                if(defaultType == 3)
                {
                    // return ingredient.oneMilliliterInGram;
                }
                else if (defaultType == 4)
                {
                  return {
                    amount: ingredient.oneCentimeterInGram,
                    operation: "/"
                  };
                }
                else if (defaultType == 5)
                {
                  return {
                    amount: ingredient.onePieceInGram,
                    operation: "/"
                  };
                }
                return 0;
            // case 3: /* Volume */
            //     if (defaultType == 2)
            //     {
            //         return ingredient.oneCentimeterInGram;
            //     }
            //     else if (defaultType == 4)
            //     {
            //         return ingredient.oneCentimeterInMilliliter;
            //     }
            //     else if (defaultType == 5)
            //     {
            //         return ingredient.onePieceInMilliliter;
            //     }
            //     return 0;
            case 4: /* Length */
                if (defaultType == 2)
                {
                  return {
                    amount: ingredient.oneCentimeterInGram,
                    operation: "*"
                  };
                }
                // else if (defaultType == 3)
                // {
                //     return ingredient.oneCentimeterInMilliliter;
                // }
                else if (defaultType == 5)
                {
                  return {
                    amount: ingredient.onePieceInCentimeter,
                    operation: "/"
                  };
                }
                return 0;
            case 5: /* Quantity */
                if (defaultType == 2)
                {
                  return {
                    amount: ingredient.onePieceInGram,
                    operation: "*"
                  };
                }
                // else if (defaultType == 3)
                // {
                //     return ingredient.onePieceInMilliliter;
                // }
                else if (defaultType == 4)
                {
                  return {
                    amount: ingredient.onePieceInCentimeter,
                    operation: "*"
                  };
                }
                return 0;
            default:
                return 0;
        }
      }

      Vue.prototype.$calculate.measurement = function (oldMeasurement, newMeasurement, amount, ingredient) {
        // console.log('oldMeasurement', oldMeasurement,'newMeasurement', newMeasurement,'amount', amount,'ingredient', ingredient);
        const oldBaseAmount = (amount * oldMeasurement.conversionAmount);
        if(oldMeasurement.typeId != newMeasurement.typeId) {
          var typeConversionAmount = getTypeConversion(ingredient, oldMeasurement.typeId, newMeasurement.typeId)?.amount;
          var typeConversionOperation = getTypeConversion(ingredient, oldMeasurement.typeId, newMeasurement.typeId)?.operation;
          // console.log('typeConversionAmount', typeConversionAmount, baseAmount);
          if(typeConversionOperation == "*") {
            var currentAmount = oldBaseAmount * typeConversionAmount;
          }
          else {
            var currentAmount = oldBaseAmount / typeConversionAmount;
          }
          return (currentAmount / newMeasurement.conversionAmount);
        }
        else {
          return (oldBaseAmount / newMeasurement.conversionAmount);
        }
      };

      Vue.prototype.$calculate.measurementTrimmed = function (oldMeasurement, newMeasurement, amount) {
        var num = Vue.prototype.$calculate.measurement(oldMeasurement, newMeasurement, amount);
        return parseFloat((num).toFixed(2));
      };

      Vue.prototype.$calculate.mins = function (mins) {
        if (mins < 60) {
            return {h: 0, m: mins};
        }
        else {
            const hours = Math.floor(mins / 60);
            const minutes = mins % 60;
            return {h: hours, m: minutes};
        }
      };
  }
};

export default calculatePlugin;
