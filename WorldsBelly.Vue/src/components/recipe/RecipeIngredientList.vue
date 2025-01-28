<template>
    <div class="recipe-page-ingredient-list">
      <div v-if="value.title" class="recipe-page-ingredient-list-title">{{ value.title }}</div>
        <v-data-table
            hide-default-header
            :headers="ingredientHeaders"
            :items.sync="ingredientRows"
            :items-per-page="-1"
            hide-default-footer
            fixed-header
            :mobile-breakpoint="0"
        >
            <!-- <template v-slot:[`header.amount`]>
                <span>Amount</span>
            </template> -->
            <template v-slot:[`item.name`]="{ item }">
                <p style="font-size: 14px; font-weight: 500">{{ item.name }}</p>
                <p style="font-size: 12px; font-weight: 400; line-height: 13px" v-if="item.description">{{ item.description }}</p>
            </template>
            <!-- <template v-slot:[`item.amount`]="{ item }">
                <b v-tooltip="'' + item.amount">{{ parseFloat(item.amount.toFixed(2)) }}</b>
            </template> -->
            <template v-slot:[`item.unit`]="{ value, index, item }">
                <RecipeDataTableDropdown
                    :value="value"
                    :item.sync="ingredientRows[index]"
                    :index="index"
                    @selectMeasurement="selectMeasurement($event, index, item)"
                >
                <b v-tooltip="'' + item.amount">{{ parseFloat(item.amount.toFixed(2)) }}</b>
                </RecipeDataTableDropdown>
            </template>
        </v-data-table>
    </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import _ from "lodash";

export default {
    name: "RecipeIngredientList",
    components: {
        RecipeDataTableDropdown: () => import("@/components/recipe/RecipeDataTableDropdown.vue"),
    },
    props: {
        value: Object,
        recipeServings: [Number, String],
        newRecipeServings: [Number, String],
    },
    data() {
        return {
            ingredientHeaders: [
                // {
                //     text: "#",
                //     align: "start",
                //     sortable: true,
                //     value: "orderindex",
                // },
                {
                    text: "Ingredient",
                    align: "start",
                    sortable: true,
                    value: "name",
                },
                // {
                //     text: "Amount",
                //     align: "end",
                //     sortable: true,
                //     value: "amount",
                // },
                {
                    text: "Unit",
                    align: "end",
                    sortable: false,
                    value: "unit",
                },
            ],
            ingredientRows: [],
            ingredients: []
        };
    },
    computed: {
        ...mapGetters("measurement/", ["get_measurements"]),
    },
    methods: {
      ...mapActions("ingredient/", ["fetch_ingredients_by_id"]),
      selectMeasurement(newMeasurement, index, item) {
        this.$emit('updatedIngredientMeasurement', {newMeasurement: newMeasurement, ingredientId: item.id, oldMeasurement: _.cloneDeep(item.unit), ingredient: _.cloneDeep(item)});
        const currentItemIndex = this.ingredientRows.findIndex(_ => _.id == item.id);
        this.ingredientRows[currentItemIndex].amount = this.$calculate.measurement(_.cloneDeep(item.unit), _.cloneDeep(newMeasurement), _.clone(item.amount), _.cloneDeep(item));
        this.ingredientRows[currentItemIndex].unit =  _.cloneDeep(newMeasurement);
      }
    },
    mounted() {
      this.currentServings = this.recipeServings;
      this.ingredientRows = _.cloneDeep(this.value.ingredients).map((ingredient) => {
            return {
                id: ingredient.id,
                ingredientId: ingredient.ingredientId,
                name: ingredient.name,
                description: ingredient.description,
                amount: ingredient.amount,
                unit: this.get_measurements.find((_) => _.id == ingredient.measurementId),
            };
        });

        const ingredientIds = this.value.ingredients.map(i => i.ingredientId);
        this.fetch_ingredients_by_id(ingredientIds).then((ingredients) => {
          this.ingredientRows = this.ingredientRows.map(i => {
            var ingredient = ingredients.find(_ => _.id == i.ingredientId)
            if(ingredient != null) {
              i.defaultAmount = i.amount;
             i.defaultMeasurement = this.get_measurements.find((_) => _.id == ingredient.defaultMeasurementId);
              i.oneCentimeterInGram = ingredient.oneCentimeterInGram;
              i.oneCentimeterInMilliliter = ingredient.oneCentimeterInMilliliter;
              i.oneMilliliterInGram = ingredient.oneMilliliterInGram;
              i.onePieceInCentimeter = ingredient.onePieceInCentimeter;
              i.onePieceInGram = ingredient.onePieceInGram;
              i.onePieceInMilliliter = ingredient.onePieceInMilliliter;
            }
            return i;
          })
        })
    },
    watch: {
      newRecipeServings: {
        handler(serving) {
          if(serving == 0) return;
          this.$emit('updatedIngredientServing', {currentServings: _.clone(this.currentServings), newRecipeServings: _.clone(serving)});
          this.ingredientRows.map(item => {
            var singleAmount = item.amount / this.currentServings;
            item.amount = (singleAmount * serving);
          })
          this.currentServings = serving;
        }
      },
    }
};
</script>

<style lang="scss">
.recipe-page-ingredient-list {
     tr:hover {
        background-color: transparent !important;
     }
  .recipe-page-ingredient-list-title {
    font-size: 14px;
    font-family: "Roboto", sans-serif;
    line-height: 1.5;
    font-weight: 400;
    color: #a79674;
    padding-left: 15px;
    padding-top: 15px;
    margin: 0;
  }
    height: 100%;
    width: 100%;
    td:last-child {
      width: 10px !important;
    }
}
</style>
