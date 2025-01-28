<template>
    <div class="ingredient-list-inputs flex flex-col" :class="{'cursor-pointer': placeholder}"
          @mouseover="hover = true"
          @mouseleave="hover = false">
      <div class="flex">
        <div class="ingredient-list-inputs__name pr-2">
          <baseSelect
            ref="select-ingredient"
            :value="value.ingredientId"
            :items="get_ingredients"
            :excludeItems="defaultIngredients"
            :defaultItem="selectedItem"
            @input="selectIngredient($event)"
            @search="getSuggestedIngredients($event)"
            @searchMore="getSuggestedIngredientsMore($event)"
            :moreLimit="50"
            @selectedItem="value.measurementId = $event.defaultMeasurementId"
            :loading.sync="loadingIngredientsList"
            itemText="name"
            itemValue="id"
            :placeholder="`${$t('type')} ${$t('ingredient')}`"
            noFilter
            input
            solo
            flat
            small
            v-if="!placeholder"
            shadow
            returnAll
          />
          <baseInput v-else :value="`${$t('add')} ${$t('ingredient')}`" solo flat small class="placeholder" shadow :class="{'placeholder-active': hover && placeholder}"/>
        </div>
        <div class="ingredient-list-inputs__amount pr-2">
          <baseInput
            type="number"
            v-model="value.amount"
            :placeholder="$t('amount')"
            :disabled="value.ingredientId ? false : true"
            :shadow="value.ingredientId ? true : false"
            solo
            flat
            small
            v-if="!placeholder" />
          <baseInput v-else solo flat small class="placeholder" shadow :class="{'placeholder-active': hover && placeholder}" />
        </div>
        <div class="ingredient-list-inputs__measure">
          <baseSelect
            v-model="value.measurementId"
            :items="sortMeasurements(get_measurements, value.measurementId)"
            multifilter
            itemText="unit"
            itemText2="name"
            :disabled="value.ingredientId ? false : true"
            :shadow="value.ingredientId ? true : false"
            solo
            flat
            small
            v-if="!placeholder"
            costumized
            >
            <template v-slot:item="{ item }">{{ item.name }}</template>
            </baseSelect>
          <baseInput v-else solo flat small class="placeholder" shadow :class="{'placeholder-active': hover && placeholder}" />
        </div>
      </div>
      <ContentEditable v-if="value" v-model="value.description" hideIcon nullable class="w-full" :placeholder="`${$t('addDescription')} (${$t('optional')})`" :limit="45" top="3">
        <span class="ingredient-list-inputs__description p-1" />
      </ContentEditable>
    </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import _ from "lodash";

export default {
    name: "IngredientListInputs",
    props: {
      value: Object,
      editable: Boolean,
      placeholder: Boolean,
      measurements: Array,
      defaultIngredients: Array
    },
    components: {
      ContentEditable: () => import('@/components/administration/editor/ContentEditable.vue'),
    },
    data() {
      return {
        isPageReady: false,
        hover: false,
        suggestedIngredients: [],
        loadingIngredientsList: false,
        focus: false,
        selectedItem: null
      }
    },
    computed: {
		...mapGetters('ingredient/', ['get_ingredients']),
		...mapGetters('measurement/', ['get_measurements']),
    },
    methods: {
		...mapActions('ingredient/', ['fetch_ingredients', 'append_ingredients']),
		...mapActions('measurement/', ['fetch_measurements']),
      getSuggestedIngredients(val) {
          this.loadingIngredientsList = true;
          this.fetch_ingredients(val)
          .finally(() => (this.loadingIngredientsList = false));
      },
      getSuggestedIngredientsMore(val) {
          this.loadingIngredientsList = true;
          this.append_ingredients(val)
          .finally(() => (this.loadingIngredientsList = false));
      },
      sortMeasurements(measurements, defaultMeasurementId) {
          if(this.selectedItem) {
            var typeId = measurements.find(_ => _.id == defaultMeasurementId)?.typeId;
            var types = [typeId];
            if(this.selectedItem.oneCentimeterInGram != null) {
              types.push(2);
              types.push(3);
              types.push(4);
            }
            if(this.selectedItem.oneCentimeterInMilliliter != null) {
              types.push(3);
              types.push(4);
            }
            if(this.selectedItem.oneMilliliterInGram != null) {
              types.push(2);
            }
            if(this.selectedItem.onePieceInCentimeter != null) {
              types.push(4);
              types.push(5);
            }
            if(this.selectedItem.onePieceInGram != null) {
              types.push(2);
              types.push(5);
            }
            if(this.selectedItem.onePieceInMilliliter != null) {
              types.push(3);
              types.push(5);
            }
            const valueTypes = _.uniq(types);
            return measurements?.filter(_ => valueTypes.includes(_.typeId))
          }
          return [];
        },
      //   (measurements, defaultMeasurementId) {
      //   console.log('test', this.selectedItem);
      //   if(!measurements || !defaultMeasurementId) return [];
      //   var typeId = measurements.find(_ => _.id == defaultMeasurementId)?.typeId;
      //   if(typeId) return measurements.filter(_ => _.typeId == typeId);
      // },
      selectIngredient(ingredient) {
        this.value.ingredientId = ingredient.id;
        this.selectedItem = _.cloneDeep(ingredient);
        // {
        //   id: ingredient.id,
        //   name: ingredient.name,
        //   namePlural: ingredient.namePlural
        // }
        this.$emit('rememberIngredient', this.selectedItem);
      },
      getDefaultIngredient(id) {
        return this.defaultIngredients.find(_ => _.id == id);
      }
    },
    created() {
          if(this.value?.ingredientId) {
            // this.selectedItem = _.cloneDeep(this.getDefaultIngredient(this.value.ingredientId));
            this.selectedItem = {
              ...this.getDefaultIngredient(this.value.ingredientId),
              id: this.value.ingredientId,
              name: this.value.name || this.getDefaultIngredient(this.value.ingredientId)?.name,
              namePlural: this.value.namePlural || this.getDefaultIngredient(this.value.ingredientId)?.name
            }
          }
          if(this.get_ingredients.length > 1 || this.get_measurements > 0) {
            this.isPageReady = true;
            return;
          }
          let fetch_measurements = this.fetch_measurements()
          let fetch_ingredients = this.fetch_ingredients()
          Promise.all([fetch_measurements, fetch_ingredients])
          .finally(() => {
              this.isPageReady = true;
          });
    },
};
</script>

<style lang="scss" scoped>
.ingredient-list-inputs {
  width: 100%;
  padding: 2px 0px;
  display: flex;
  align-items: center;
  .placeholder {
    cursor: pointer;
    pointer-events: none;
    opacity: .5 !important;
  }
  .placeholder-active {
      transition: all 300ms;
      opacity: 1 !important;
  }
  &__name {
    width: 60%;
  }
  &__amount {
    width: 25%;
  }
  &__measure {
    width: 15%;
  }
  &__description {
    font-size: 12px;
  }
}
</style>

<style lang="scss">
// overwrite
.ingredient-list-inputs {
  // .v-input__slot {
  //   box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.1) !important;
  // -webkit-box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.1) !important;
  // -moz-box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.1) !important;
  // border-radius: 0px !important;
  // }
}
</style>
