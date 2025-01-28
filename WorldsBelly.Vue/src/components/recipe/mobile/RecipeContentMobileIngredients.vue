<template>
    <div class="recipe-mb-ingredients w-full">

      <div class="recipe-ingredients">
            <div class="ingredients-list">
                <div class="ingredients-list__title pb-2 flex justify-between w-full items-center px-4">
                  <div class="flex ">
                    <baseIcon value="icofont-basket" color="#c3ab7c" :size="30" style="padding-bottom: 4px;"/>
                    <h3 class="p-2">{{ $t('ingredients') }}</h3>
                  </div>
                    <baseQuantityInput
                        class="mr-2"
                        :min="1"
                        width="110px"
                        labelRight
                        :value="value.servingAmount"
                        @input="newRecipeServings = Number($event)"
                    />
                    <span class="recipe-serving-amount">{{(newRecipeServings == 0 ? value.servingAmount : newRecipeServings) == 1 ? $t('serving') : $t('servings') }}</span>
                </div>
                <div class="w-full">
                  <RecipeIngredientList
                      v-for="(ingredientList, i) in value.ingredientLists"
                      :key="i"
                      :value="ingredientList"
                      :recipeServings="value.servingAmount"
                      :newRecipeServings.sync="newRecipeServings"
                      @updatedIngredientMeasurement="$emit('updatedIngredientMeasurement', $event)"
                      @updatedIngredientServing="$emit('updatedIngredientServing', $event)"
                  />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
/* eslint-disable */
import VueScrollTo from "vue-scrollto";
import { mapGetters, mapMutations, mapActions } from "vuex";
// import _ from "lodash";

export default {
    name: "RecipeContentMobileIngredients",
    props: {
      value: Object
    },
    components: {
        RecipeIngredientList: () => import("@/components/recipe/RecipeIngredientList.vue"),
    },
    data() {
        return {
            newRecipeServings: 0,
        };
    },
    computed: {
    },
    methods: {
    },
    destroyed() {
    },
    watch: {
    },
};
</script>

<style lang="scss">
.recipe-mb-ingredients {

  .recipe-ingredients {
                width: 100%;
                .recipe-serving-amount {
                  font-weight: 500;
                  font-size: 14px;
                  text-transform: uppercase;
                  color: #091736;
                }
                .ingredients-list__title {
                  padding: 10px 0px;
                  width: 100%;
                  position: sticky;
                  background: white;
                  top: -1px;
                  z-index: 9;
                }
            }
}
</style>
