<template>
    <BaseExpandContent class="side-filter" :label="$t('ingredients')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div>
          <baseCheckbox v-model="showInclude" :label="$t('included')" />
            <div v-if="showInclude">
                <div class="py-1">
                    <v-chip
                        v-for="includedIngredient in includedIngredients"
                        :key="includedIngredient.id"
                        small
                        @click:close="remove(includedIngredient)"
                        close
                        >{{ includedIngredient.name }}</v-chip
                    >
                </div>
                <FilterSelect
                    :items="get_ingredients"
                    :excludeItems="includedIngredients.concat(excludedIngredients)"
                    @selected="selectIngredient($event)"
                    @search="getSuggestedIngredients($event)"
                    @searchMore="getSuggestedIngredientsMore($event)"
                    :moreLimit="50"
                    :loading.sync="loadingIngredientsList"
                    itemText="name"
                    :placeholder="$t('type') +' '+$t('ingredient')"
                />
            </div>
        </div>
        <div class="pt-4">
          <baseCheckbox v-model="showExclude" :label="$t('excluded')" />
            <div v-if="showExclude">
                <div class="py-1">
                    <v-chip
                        v-for="ingredient in excludedIngredients"
                        :key="ingredient.id"
                        small
                        @click:close="removeExcluded(ingredient)"
                        close
                        >{{ ingredient.name }}</v-chip
                    >
                </div>
                <FilterSelect
                    :items="get_ingredients"
                    :excludeItems="includedIngredients.concat(excludedIngredients)"
                    @selected="selectExcludeIngredient($event)"
                    @search="getSuggestedIngredients($event)"
                    @searchMore="getSuggestedIngredientsMore($event)"
                    :moreLimit="50"
                    :loading.sync="loadingIngredientsList"
                    itemText="name"
                    :placeholder="$t('type') +' '+$t('ingredient')"
                />
            </div>
        </div>
    </BaseExpandContent>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import _ from "lodash";

export default {
    name: "FilterIngredients",
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
        FilterSelect: () => import("@/components/filter/FilterSelect.vue"),
    },
    props: {
      expanded: Boolean,
      queries: Object
    },
    data() {
        return {
            showInclude: true,
            showExclude: false,
            includedIngredients: [],
            excludedIngredients: [],
            loadingIngredientsList: false,
            ingredientsFromQuery: [],
            isQueryLoaded: false,
            clonedIncludedIngredients: [],
            clonedExcludedIngredients: []
        };
    },
    computed: {
        ...mapGetters("ingredient/", ["get_ingredients"]),
    },
    methods: {
        ...mapActions("ingredient/", ["fetch_ingredients", "append_ingredients", "fetch_ingredients_by_id"]),
        selectIngredient(ingredient) {
            this.includedIngredients.push(ingredient);
        },
        selectExcludeIngredient(ingredient) {
            this.excludedIngredients.push(ingredient);
        },
        remove(i) {
           this.includedIngredients = this.includedIngredients.filter(_ => _.id != i.id);
        },
        removeExcluded(i) {
           this.excludedIngredients = this.excludedIngredients.filter(_ => _.id != i.id);
        },
        getSuggestedIngredients(val) {
            this.fetch_ingredients(val);
        },
        getSuggestedIngredientsMore(val) {
            this.append_ingredients(val);
        },
    },
    mounted() {
      var a = this.queries.excludeingredients || [];
      var b = this.queries.includeingredients ||[ ];
      var queryIds = a.concat(b);
      if(queryIds.length > 0) {
        this.fetch_ingredients_by_id(queryIds).then((ingredients) => {
          this.ingredientsFromQuery = ingredients;
        })
      }
      else {
        this.isQueryLoaded = true;
      }
    },
    watch: {
      ingredientsFromQuery: {
        handler() {
          if(this.queries.includeingredients?.length > 0) {
            this.queries.includeingredients.forEach(id => {
              this.includedIngredients.push(this.ingredientsFromQuery.find(_ => _.id == id));
            });
          }
          if(this.queries.excludeingredients?.length > 0) {
            this.showExclude = true;
            this.queries.excludeingredients.forEach(id => {
              this.excludedIngredients.push(this.ingredientsFromQuery.find(_ => _.id == id));
            });
          }
          this.$nextTick(() => {
            this.isQueryLoaded = true;
          })
        },
      },
      includedIngredients: {
        handler() {
          var selected = this.includedIngredients?.map(_ => _.id) || [];
          this.$emit('includedIngredients', selected);
        }, deep: true
      },
      excludedIngredients: {
        handler() {
          var selected = this.excludedIngredients?.map(_ => _.id) || [];
          this.$emit('excludedIngredients', selected);
        }, deep: true
      },
      showInclude: {
        handler(show) {
          if(!show) {
            this.clonedIncludedIngredients = _.cloneDeep(this.includedIngredients);
            this.includedIngredients = [];
          }
          else if(this.clonedIncludedIngredients.length > 0) {
            this.includedIngredients = _.cloneDeep(this.clonedIncludedIngredients).filter(({id}) => !this.excludedIngredients.some(x => x.id == id));
            this.clonedIncludedIngredients = [];
          }
        }
      },
      showExclude: {
        handler(show) {
          if(!show) {
            this.clonedExcludedIngredients = _.cloneDeep(this.excludedIngredients);
            this.excludedIngredients = [];
          }
          else if(this.clonedExcludedIngredients.length > 0) {
            this.excludedIngredients = _.cloneDeep(this.clonedExcludedIngredients).filter(({id}) => !this.includedIngredients.some(x => x.id == id));
            this.clonedExcludedIngredients = [];
          }
        }
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedExcludedIngredients = _.cloneDeep(this.excludedIngredients);
            this.excludedIngredients = [];
            this.clonedIncludedIngredients = _.cloneDeep(this.includedIngredients);
            this.includedIngredients = [];
          }
          else if(this.clonedExcludedIngredients.length > 0 || this.clonedIncludedIngredients.length > 0) {
            this.excludedIngredients = _.cloneDeep(this.clonedExcludedIngredients);
            this.clonedExcludedIngredients = [];
            this.includedIngredients = _.cloneDeep(this.clonedIncludedIngredients);
            this.clonedIncludedIngredients = [];
          }
        }
      }
    }
};
</script>

<style lang="scss">
.side-filter {
    // border-bottom: 1px solid #f7f9fb;
    .v-input--checkbox {
        label {
            -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
            -webkit-font-smoothing: antialiased;
            font-family: "Google Sans", Roboto, Arial, sans-serif !important;
            color: #111;
            background-repeat: no-repeat;
            box-sizing: inherit;
            font-size: 12px;
            line-height: normal;
            font-weight: 400;
        }
    }
    .v-chip {
      // box-shadow: 0px 0px 2px 0px rgba(0, 0, 0, 0.4) !important;
      background: #15132b !important;
      color: #f5f5f5 !important;
      border-radius: 4px !important;
      margin: 3px;
      // height: auto !important;
      .v-icon {
        font-size: 13px !important;
        color: rgba(254, 254, 255, 0.65) !important;
      }
    }
}
</style>
