<template>
    <div class="recipe-mb-nutrients w-full">
      <div class="recipe-nutrients">
            <div class="nutrients-list">
                <div class="nutrients-list__title pb-2 flex justify-between w-full items-center px-4">
                  <div class="flex ">
                    <h3>{{ $t('nutrients') }}</h3>
                  </div>
                    <baseQuantityInput
                        class="mr-2"
                        :min="1"
                        width="110px"
                        labelRight
                        :value="nutrientServingAmount"
                        @input="nutrientServingAmount = Number($event)"
                    />
                    <span class="recipe-serving-amount">{{ Number(nutrientServingAmount) == 1 ? $t('serving') : $t('servings') }}</span>
                </div>
                <div class="w-full">
                  <RecipeNutrientTable
                      :nutrients="value.nutrients"
                      :servings="Number(nutrientServingAmount)"
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
    name: "RecipeContentMobileNutrients",
    props: {
      value: Object
    },
    components: {
        RecipeNutrientTable: () => import("@/components/recipe/RecipeNutrientTable.vue"),
    },
    data() {
        return {
          nutrientServingAmount: 1,
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
.recipe-mb-nutrients {

  .recipe-nutrients {
                width: 100%;
                .recipe-serving-amount {
                  font-weight: 500;
                  font-size: 14px;
                  text-transform: uppercase;
                  color: #091736;
                }
                .nutrients-list__title {
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
