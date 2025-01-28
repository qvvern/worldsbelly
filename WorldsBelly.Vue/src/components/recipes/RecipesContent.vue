<template>
<div class="recipes-content-wrapper w-full h-full">
      <RecipesSortBy :sortBy.sync="queries.sortBy" :sortDescending.sync="queries.sortDescending" :isHome="isHome" />
      <BaseSidebarMobileActivator v-if="isMobile" :isHome="isHome"/>
    <div class="recipes-content w-full flex" :style="{paddingTop: paddingTop}">
      <component  v-if="!isMobile || isOpen" @close="isOpen = false" :is="sidebarComponent" :style="{marginTop: `-${paddingTop}`}" :sidebarTop="sidebarTop">
      <div class="recipes-content-sidebar__header px-2 flex items-center justify-end" style="height: 30px">
            <!-- <h2>Filters</h2> -->
            <BaseUiButton
                v-if="hasFilters"
                type="btn2"
                class="ml-3 mt-2"
                style="height: 20px; border: none; border-radius: 0px; font-weight: normal;"
                @click.prevent.native="clearFilters()"
                :size="11"
                icon="icofont-close-line"
            >
                Reset
            </BaseUiButton>
      </div>
      <template v-if="!isClearingFilters">
        <FilterTypes
          v-if="!sidebarTop"
          :queries.sync="queries"
          @input="queries.tags = $event"
          :expanded.sync="expandTypes"
        />
        <!-- food, etc. -->
          <!-- pre-create tags -->
        <!-- season -->
        <FilterTags
          :queries.sync="queries"
          @input="queries.tags = $event"
          :expanded.sync="expandTags"
        />
        <FilterServings
          :queries.sync="queries"
          @input="queries.servings = $event"
          :expanded.sync="expandServings"
        />
        <FilterIngredients
          :expanded.sync="expandIngredients"
          :queries.sync="queries"
          @includedIngredients="queries.includeingredients = $event"
          @excludedIngredients="queries.excludeingredients = $event"
        />
        <FilterNutrients
          :queries.sync="queries"
          @input="queries.nutrients = $event"
          :expanded.sync="expandNutrients"
        />
        <FilterBestServed
          :queries.sync="queries"
          @input="queries.bestServed = $event"
          :expanded.sync="expandBestServed"
        />
        <FilterCountries
          :queries.sync="queries"
          @includedCuisines="queries.includecuisines = $event"
          @excludedCuisines="queries.excludecuisines = $event"
          :expanded.sync="expandCountries"
        />
        <FilterCookingTime
          :queries.sync="queries"
          @input="queries.time = $event"
          :expanded.sync="expandCookingTime"
        />
        <FilterRating
          :queries.sync="queries"
          @input="$event ? (queries.fromrating = $event.from, queries.torating = $event.to) : (queries.fromrating = '', queries.torating = '')"
          :expanded.sync="expandRating"
        />
        <FilterFlavors
          :queries.sync="queries"
          @sweet="$event ? (queries.fromsweet = $event.from, queries.tosweet = $event.to) :  (queries.fromsweet = '', queries.tosweet = '')"
          @sour="$event ? (queries.fromsour = $event.from, queries.tosour = $event.to) :  (queries.fromsour = '', queries.tosour = '')"
          @salty="$event ? (queries.fromsalty = $event.from, queries.tosalty = $event.to) :  (queries.fromsalty = '', queries.tosalty = '')"
          @bitter="$event ? (queries.frombitter = $event.from, queries.tobitter = $event.to) :  (queries.frombitter = '', queries.tobitter = '')"
          @spices="$event ? (queries.fromspices = $event.from, queries.tospices = $event.to) :  (queries.fromspices = '', queries.tospices = '')"
          @flavor="$event ? (queries.fromflavor = $event.from, queries.toflavor = $event.to) :  (queries.fromflavor = '', queries.toflavor = '')"
          :expanded.sync="expandFlavors"
          />
        <FilterDifficulty
          :queries.sync="queries"
           @input="queries.difficulty = $event"
          :expanded.sync="expandDifficulty"
          />
      </template>
      </component>
      <RecipeCards
        :recipes="isSearching ? [] : get_recipes"
        :loading="!isRecipesFetched || isSearching"
        :loadingMore="isLoadingMore"
        @loadMore="appendRecipes"
        :servings="queries.servings"
        :noMore="noMore"
        @onClick="goToRecipe($event)"
      />
    </div>
</div>
</template>

<script>
import { EventBus } from '@/event-bus'
import { mapActions, mapGetters, mapMutations } from "vuex";
import { QueryMixin } from "@/mixins/QueryMixin";
import _ from "lodash"
export default {
    name: "RecipesContent",
    mixins: [QueryMixin],
    components: {
        RecipeCards: () => import("@/components/base/RecipeCards.vue"),
        BaseSidebar: () => import("@/components/base/BaseSidebar.vue"),
        BaseSidebarMobileActivator: () => import("@/components/base/BaseSidebarMobileActivator.vue"),
        BaseSidebarMobile: () => import('@/components/base/BaseSidebarMobile.vue'),
        FilterServings: () => import("@/components/filter/FilterServings.vue"),
        FilterIngredients: () => import("@/components/filter/FilterIngredients.vue"),
        FilterFlavors: () => import("@/components/filter/FilterFlavors.vue"),
        FilterRating: () => import("@/components/filter/FilterRating.vue"),
        FilterNutrients: () => import("@/components/filter/FilterNutrients.vue"),
        FilterCountries: () => import("@/components/filter/FilterCountries.vue"),
        FilterDifficulty: () => import("@/components/filter/FilterDifficulty.vue"),
        FilterBestServed: () => import("@/components/filter/FilterBestServed.vue"),
        FilterTags: () => import("@/components/filter/FilterTags.vue"),
        FilterTypes: () => import("@/components/filter/FilterTypes.vue"),
        FilterCookingTime: () => import("@/components/filter/FilterCookingTime.vue"),
        RecipesSortBy: () => import("@/components/recipes/RecipesSortBy.vue"),
    },
    props: {
      paddingTop: {
        type: String,
        default: null
      },
      sidebarTop: {
        type: String,
        default: null
      },
      isHome: {
        type: Boolean,
        default: false
      },
    },
    data() {
        return {
            isOpen: false,
            expandTypes: true,
            expandTags: true,
            expandServings: true,
            expandNutrients: true,
            expandBestServed: true,
            expandIngredients: true,
            expandDifficulty: false,
            expandFlavors: false,
            expandRating: false,
            expandCookingTime: false,
            expandCountries: false,
            isFirstLoad: false,
            isRecipesFetched: false,
            isSearching: false,
            noMore: false,
            isLoadingMore: false,
            queries: {
              search: "",
              tags: [],
              servings: "",
              includeingredients: [],
              excludeingredients: [],
              nutrients: [],
              includecuisines: [],
              excludecuisines: [],
              time: {},
              bestServed: [],
              difficulty: [],
              fromrating: "",
              torating: "",
              fromsweet: "",
              tosweet: "",
              fromsour: "",
              tosour: "",
              fromsalty: "",
              tosalty: "",
              frombitter: "",
              tobitter: "",
              fromspices: "",
              tospices: "",
              fromflavor: "",
              toflavor: "",
              sortDescending: false,
              sortBy: "",
            },
            hasFilters: false,
            isClearingFilters: true,
        };
    },
    computed: {
        ...mapGetters("recipe/", ["get_recipes", "get_recipes_filter"]),
        isMobile() {
          return window.innerWidth <= 800;
        },
        sidebarComponent() {
          if(this.isMobile) return 'BaseSidebarMobile';
          return 'BaseSidebar';
        }
    },
    methods: {
        ...mapActions("recipe/", ["fetch_recipes", "fetch_append_recipes"]),
        ...mapMutations("recipe/", ["set_recipes_filter"]),
        goToRecipe(recipe) {
            this.$localeRouter.push({
                name: "Recipe",
                params: { id: recipe.id },
            });
        },
        search: _.debounce(function() {
            this.$emit("isRecipesFetched", false);
            this.isFirstLoad = true;
            this.isSearching = true;
            this.noMore = false;
            this.fetch_recipes().finally(() => {
                this.isSearching = false;
                this.isRecipesFetched = true;
                this.$emit("isRecipesFetched", true);
            });
        }, 400),
        appendRecipes() {
            this.isLoadingMore = true;

            this.fetch_append_recipes()
                .then((response) => {
                    if (response.pageSize > response.recipes.length) {
                        this.noMore = true;
                    }
                })
                .finally(() => {
                    this.isLoadingMore = false;
                });
        },
        clearFilters() {
          this.queries = {
              search: "",
              tags: [],
              servings: "",
              includeingredients: [],
              excludeingredients: [],
              nutrients: [],
              includecuisines: [],
              excludecuisines: [],
              time: {},
              bestServed: [],
              difficulty: [],
              fromrating: "",
              torating: "",
              fromsweet: "",
              tosweet: "",
              fromsour: "",
              tosour: "",
              fromsalty: "",
              tosalty: "",
              frombitter: "",
              tobitter: "",
              fromspices: "",
              tospices: "",
              fromflavor: "",
              toflavor: "",
            };
            this.hasFilters = false;
            this.isClearingFilters = true;
            this.$nextTick(() => {
              this.isClearingFilters = false;
            })
        },
        canClearFilters: _.debounce(function() {
          var newFilter = _.cloneDeep(this.queries);
          Object.keys(newFilter).forEach(key => {
            if (newFilter[key] == null || newFilter[key] == "" || (_.isObject(newFilter[key]) && _.isEmpty(newFilter[key]))) {
              delete newFilter[key];
            }
          });
          this.hasFilters = !_.isEmpty(newFilter);
        }, 200),
        searchText(text) {
          this.$set(this.queries, 'search', text);
        }
    },
    mounted() {
        EventBus.$on('onSearch', this.searchText)
        if (this.get_recipes?.length > 0) {
            this.isRecipesFetched = true;
            this.$emit("isRecipesFetched", true);
        } else {
          if(_.isEmpty(this.$route.query)) {
            this.search();
          }
        }
        this.$nextTick(() => {
          this.isClearingFilters = false;
        })
    },
  watch: {
    queries: {
      handler() {
        this.canClearFilters();
        if(!this.isFirstLoad) {
          this.$emit('queriesLoaded', this.queries);
        }
        var filterA = _.cloneDeep(this.get_recipes_filter);
        var filterB = _.cloneDeep(this.queries);
        this.set_recipes_filter(_.cloneDeep(this.queries));
        filterA.servings = null;
        filterB.servings = null;
        if(!_.isEqual(filterA, filterB)) {
          this.search();
        }
      },
      deep: true,
    }
  }
};
</script>
<style lang="scss">
.recipes-content-wrapper  {
  position: relative;
}
.recipes-content {
  &-sidebar__header {
    // position: sticky;
    // top: 0;
    // z-index: 9;
    // background: white;
     h2 {
      font-size: .875rem;
      color: rgba(0, 0, 0, 1);
      -webkit-box-sizing: border-box;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      font-weight: 400;
      padding: 10px;
     }
  }
}
</style>
