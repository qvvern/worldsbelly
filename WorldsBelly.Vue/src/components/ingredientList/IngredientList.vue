<template>
    <div class="ingredients-list">
          <template>
            <div class="ingredients-list__title pb-2">
              <h3 class="p-2">{{$t('ingredients')}}</h3>
                <baseQuantityInput class="mr-3" :label="$t('amountOfServings')" :min="0" width="110px" labelRight :value="recipeServings" @input="$emit('setRecipeServings', Number($event));" />
                <!-- <baseInput label="Amount of servings" :value="recipeServings" @input="$emit('setRecipeServings', Number($event));" type="number" right outlined small borderless width="160px"></baseInput> -->
            </div>
            <div class="ingredients-list__recipe">
              <template v-if="!loadingIngredientList">
                <div class="ingredients-list__recipe-list pl-1 pr-2" v-for="(list, i) in value" :key="i"
                      @mouseover="hover = i"
                      @mouseleave="hover = -1">
                  <div class="flex items-center" v-if="value.length > 1">
                    <baseButton class="mr-2" icon="mdi-close" round size="18" xsmall noshadow dashed @click.prevent.native="removeList(i)"></baseButton>
                    <ContentEditable v-model="list.title" placeholder="list" :limit="30" top="3" emitPlaceholder>
                      <span class="one p-1" />
                    </ContentEditable>
                  </div>
                  <div class="flex mt-2" v-for="(ingredient, u) in list.ingredients" :key="u">
                    <baseButton v-show="list.ingredients.length > 1" class="mt-2 mr-2" icon="mdi-minus" round size="18" xsmall noshadow @click.prevent.native="removeIngredientFromList(i, u)"></baseButton>
                    <IngredientListInputs ref="listinput" v-model="value[i].ingredients[u]" @rememberIngredient="addIn(i, u, $event)" :defaultIngredients="rememberIngredients[i] ? rememberIngredients[i] : []" />
                  </div>
                  <div class="flex items-center mt-2 input-placeholder" v-show="list.ingredients.length == 0 || hover == i" @click="addEmptyIngredient(i)">
                      <baseButton v-show="list.ingredients.length > 1" class="mr-2" icon="mdi-plus" round size="18" xsmall noshadow secondary></baseButton>
                      <IngredientListInputs placeholder />
                  </div>
                </div>
              </template>
            </div>
            <div class="ingredients-list__action">
              <baseButton dashed @click.prevent.native="addEmptyIngredientList()"  noshadow class="mt-2">{{$t('createNewList')}}</baseButton>
            </div>
          </template>
          <!-- <template v-else>
            <BaseProgressLinear></BaseProgressLinear>
          </template> -->
    </div>
</template>

<script>
import { cloneDeep } from "lodash";
import { mapActions } from "vuex";

export default {
    name: "IngredientList",
    components: {
      IngredientListInputs: () => import('@/components/ingredientList/IngredientListInputs.vue'),
      ContentEditable: () => import('@/components/administration/editor/ContentEditable.vue'),
    },
    props: {
      editable: Boolean,
      recipeServings: Number,
      value: Array,
    },
    data() {
      return {
        rememberIngredients: {},
        loadingIngredientList: false,
        hover: false,
      }
    },
    computed: {
    },
    methods: {
      ...mapActions("ingredient/", ["fetch_ingredients_by_id"]),
      addIn(index, ingredientIndex, ev) {
        if(!this.rememberIngredients[index]) this.rememberIngredients[index] = [];
        if(this.rememberIngredients[index][ingredientIndex]) {
          this.rememberIngredients[index][ingredientIndex] = ev;
        }
        else {
          this.rememberIngredients[index].push(ev);
        }
        this.$emit('rememberIngredients', cloneDeep(this.rememberIngredients));
      },
      removeList(listIndex) {
        this.value.splice(listIndex, 1);
        this.rememberIngredients.splice(listIndex, 1);
        this.loadingIngredientList = true;
        this.$nextTick(() => {
          this.loadingIngredientList = false;
          this.$emit('rememberIngredients', cloneDeep(this.rememberIngredients));
        })
      },
      removeIngredientFromList(listIndex, ingredientIndex) {
        this.value[listIndex].ingredients.splice(ingredientIndex, 1);
        this.rememberIngredients[listIndex].splice(ingredientIndex, 1);
        this.loadingIngredientList = true;
        this.$nextTick(() => {
          this.loadingIngredientList = false;
          this.$emit('rememberIngredients', cloneDeep(this.rememberIngredients));
        })
      },
      loadRememberIngredients() {
         this.value.forEach((item, index) => {
           this.rememberIngredients[index] = cloneDeep(item.ingredients).map(_ => {
             return {
               id: _.ingredientId,
               name:_.name,
               namePlural: _.namePlural
             }
           })

            const ingredientIds = item.ingredients.map(i => i.ingredientId);
            this.fetch_ingredients_by_id(ingredientIds).then((ingredients) => {
              this.rememberIngredients[index] = this.rememberIngredients[index].map(i => {
                var ingredient = ingredients.find(_ => _.id == i.id);
                if(ingredient != null) {
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

         })
        this.$emit('rememberIngredients', cloneDeep(this.rememberIngredients));
      },
      addEmptyIngredient(index) {
        this.value[index].ingredients.push({
          ingredientId: null,
          amount: null,
          measurementId: null,
          title: null,
          description: null,
          recipeIngredientListId: this.value[index].id,
          tempId: this.$guid.uuidv4()
        });
      },
      addEmptyIngredientList() {
        this.value.push({
          orderIndex: this.value.length,
          title: this.$t('list') + " " + this.value.length,
          ingredients: []
        });
      }
    },
    created() {
      this.loadRememberIngredients();
    }
};
</script>

<style lang="scss" scoped>
.ingredients-list {
  padding: 20px 5px;
  .input-placeholder {
    position: absolute;
    padding-right: 12px;
  }
  &__title {
    display: flex;
    align-items: flex-start;
    justify-content: space-between;
    h3 {
      color: rgb(0, 0, 0);
      -webkit-box-direction: normal;
      box-sizing: border-box;
      font-family: 'Montserrat',sans-serif;
      font-weight: 300;
      max-width: 500px;
      padding-top: 5px;
      padding-bottom: 5px;
      padding-left: 10px;
      font-size: 18px;
      margin: 0px;
      line-height: 1;
    }
  }
  &__recipe {
    &-list {
      padding-bottom: 60px;
    }
  }
  &__action {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
  }
}
</style>
