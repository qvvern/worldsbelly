<template>
    <baseLayout class="recipe-mb" v-if="clonedRecipe">
        <div slot="content" class="recipe-mb__content">
            <v-tabs-items v-model="tab">
              <v-tab-item>
                <RecipeContentMobileDetails v-model="clonedRecipe" />
              </v-tab-item>
              <v-tab-item>
                <RecipeContentMobileNutrients v-model="clonedRecipe" />
              </v-tab-item>
              <v-tab-item>
                <RecipeContentMobileIngredients
                  v-model="clonedRecipe"
                  @updatedIngredientMeasurement="mapStepContent(null, $event)"
                  @updatedIngredientServing="mapStepContent($event, null)"
                />
              </v-tab-item>
              <v-tab-item>
                <RecipeContentMobileInstructions v-model="clonedRecipe" />
              </v-tab-item>
              <v-tab-item>
                <RecipeContentMobileComments :recipeId="get_recipe.recipeId" v-if="get_recipe" :recipeByUserId="get_recipe.createdByUser.id" />
              </v-tab-item>
            </v-tabs-items>
        </div>
        <div slot="footer" class="recipe-mb__footer">
          <v-tabs
            v-model="tab"
            bg-color="primary"
          >
            <v-tab>
              <div>
                <baseIcon value="icofont-restaurant-menu" :size="22" />
                <p style="font-size: 11px">Details</p>
              </div>
            </v-tab>
            <v-tab>
              <div>
                <baseIcon value="icofont-tablets" :size="22" />
                <p style="font-size: 11px">Nutrients</p>
              </div>
            </v-tab>
            <v-tab>
              <div>
                <baseIcon value="icofont-salt-and-pepper" :size="22" />
                <p style="font-size: 11px">Ingredients</p>
              </div>
            </v-tab>
            <v-tab>
              <div>
                <baseIcon value="icofont-spoon-and-fork" :size="22" />
                <p style="font-size: 11px">Instructions</p>
              </div>
            </v-tab>
            <v-tab>
              <div>
                <baseIcon value="icofont-comment" :size="19" style="padding: 2px"/>
                <p style="font-size: 11px">Comments</p>
              </div>
            </v-tab>
          </v-tabs>
        </div>
    </baseLayout>
    <baseLayout v-else>
        <div slot="content" class="flex items-center justify-center py-8">
            <h3>{{$t('cannotFindRecipe')}}.</h3>
        </div>
    </baseLayout>
</template>

<script>
import VueScrollTo from "vue-scrollto";
import { mapGetters, mapMutations, mapActions } from "vuex";
import _ from "lodash";

export default {
    name: "RecipeContentMobile",
    components: {
        RecipeContentMobileDetails: () => import("@/components/recipe/mobile/RecipeContentMobileDetails.vue"),
        RecipeContentMobileInstructions: () => import("@/components/recipe/mobile/RecipeContentMobileInstructions.vue"),
        RecipeContentMobileIngredients: () => import("@/components/recipe/mobile/RecipeContentMobileIngredients.vue"),
        RecipeContentMobileNutrients: () => import("@/components/recipe/mobile/RecipeContentMobileNutrients.vue"),
        RecipeContentMobileComments: () => import("@/components/recipe/mobile/RecipeContentMobileComments.vue"),
        // BaseSidebar: () => import("@/components/base/BaseSidebar.vue"),
        // BaseCollapsible: () => import("@/components/base/BaseCollapsible.vue"),
        // BaseCarousel: () => import("@/components/base/BaseCarousel.vue"),
        // HeartBtn: () => import("@/components/base/HeartBtn.vue"),
        // SocialBtn: () => import("@/components/base/SocialBtn.vue"),
        // BaseCarouselSlide: () => import("@/components/base/BaseCarouselSlide.vue"),
        // RecipeDraftsEditRating: () => import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditRating.vue"),
        // RecipeDraftsEditTastebuds: () =>
        //     import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditTastebuds.vue"),
        // RecipeTopBox: () => import("@/components/recipe/RecipeTopBox.vue"),
        // RecipeNutrientTable: () => import("@/components/recipe/RecipeNutrientTable.vue"),
        // RecipeIngredientList: () => import("@/components/recipe/RecipeIngredientList.vue"),
        // RecipeContentComments: () => import("@/components/recipe/RecipeContentComments.vue"),
    },
    props: {
        inDialog: Boolean,
    },
    data() {
        return {
            tab: 0,
            clonedRecipe: null,
            recipeUserRating: {
              recipeId: null,
              rating: null
            },
            isFetchingRecipeSteps: false,
            newRecipeServings: 0,
            nutrientServingAmount: 1,
            activeSlide: 0,
            isMappingStepContent: true,
            like: false,
            isRating: false
        };
    },
    computed: {
        ...mapGetters("measurement/", ["get_measurements"]),
        ...mapGetters("country/", ["get_countries"]),
        ...mapGetters("recipe/", [
            "get_recipe",
            "get_recipe_user_rating",
            "get_recipe_difficulty",
            "get_recipe_best_served",
            "get_recipe_steps",
        ]),
        recipeSlides() {
            var slides = [];
            slides.push({
                type: "image",
                url: "https://worldsbellystorage.blob.core.windows.net" + this.clonedRecipe.imageUrl + "main",
            });
            if (this.clonedRecipe.videoUrl) {
                slides.push({
                    type: "video",
                    videoUrl: this.clonedRecipe.videoUrl,
                    url: "https://worldsbellystorage.blob.core.windows.net" + this.clonedRecipe.imageUrl + "main",
                });
            }
            this.clonedRecipe.steps.forEach((step) => {
                if (step.imageUrl) {
                    slides.push({
                        type: "image",
                        url: "https://worldsbellystorage.blob.core.windows.net" + step.imageUrl,
                    });
                }
                if (step.videoUrl) {
                    slides.push({
                        type: "video",
                        videoUrl: step.videoUrl,
                        url: step.imageUrl ? "https://worldsbellystorage.blob.core.windows.net" + step.imageUrl : "https://worldsbellystorage.blob.core.windows.net" + this.clonedRecipe.imageUrl + "main",
                    });
                }
            });
            return slides;
        },
    },
    methods: {
        ...mapMutations("recipe/", ["set_recipe", "set_recipe_user_rating", "set_recipe_steps"]),
        ...mapActions("recipe/", ["fetch_recipe_user_rating", "submit_recipe_user_rating", "fetch_recipe_steps"]),
        ...mapActions("user/", ["fetch_recipe_like"]),
        searchForTag(tag, isUserTag) {
          if(!isUserTag) {
            location = location.origin+`/recipes?tags=${tag.id}`
          }
          else {
            location = location.origin+`/recipes?utags=${tag.id}`
          }
        },
        submitRating(rating) {
            this.recipeUserRating.rating = rating;
            this.isRating = true;
            this.submit_recipe_user_rating(this.recipeUserRating).finally(() => {
                this.isRating = false;
            })
        },
        scrollToComments() {
                var activeEl = document.querySelector(".recipe-comments");
            this.$nextTick(() => {
                VueScrollTo.scrollTo(activeEl, 400, { container: '.recipe-mb__content' });
            });
        },
        setActiveSlide(index) {
            this.activeSlide = index;
            this.$nextTick(() => {
                var activeEl = document.querySelector(".image-container-thumbs-thumb.active");
                VueScrollTo.scrollTo(activeEl, 400, { container: ".image-container-thumbs" });
            });
        },
        mapHtmlContent(htmlString, newServing, newMeasurement) {
            var htmlObject = document.createElement("div");
            htmlObject.innerHTML = htmlString;
            const ingredients = _.flatMap(this.clonedRecipe.ingredientLists, "ingredients");
            const Vue = this;
            if (newMeasurement) {
                var htmlIngredientsSpecific = htmlObject.querySelectorAll(
                    `[data-ingredient][data-id='${newMeasurement.ingredientId}']`
                );
                [].forEach.call(htmlIngredientsSpecific, function (span) {
                    var spanAmount = Number(span.getAttribute("data-amount"));
                    var finalAmount = Vue.$calculate.measurement(
                        newMeasurement.oldMeasurement,
                        newMeasurement.newMeasurement,
                        spanAmount,
                        newMeasurement.ingredient
                    );
                    span.querySelector(".i-amount").innerHTML = parseFloat(finalAmount.toFixed(8));
                    span.querySelector(".i-amount").title = finalAmount;
                    span.querySelector(".i-measurement").title = newMeasurement.newMeasurement.name;
                    span.setAttribute("data-amount", finalAmount);
                    span.querySelector(".i-measurement").innerHTML = newMeasurement.newMeasurement.unit;
                });
            } else {
                var htmlIngredients = htmlObject.querySelectorAll(`[data-ingredient]`);
                [].forEach.call(htmlIngredients, function (span) {
                    var spanAmount = Number(span.getAttribute("data-amount"));
                    if (newServing != null) {
                        var singleAmount = spanAmount / newServing.currentServings;
                        var finalAmount = singleAmount * newServing.newRecipeServings;
                        span.querySelector(".i-amount").title = finalAmount;
                        span.querySelector(".i-amount").innerHTML = parseFloat(finalAmount.toFixed(8));
                        span.setAttribute("data-amount", finalAmount);
                    } else {
                        var spanIngredient = Number(span.getAttribute("data-id"));
                        var ingredient = ingredients.find((_) => _.id == spanIngredient);
                        var m = Vue.get_measurements.find((_) => _.id == ingredient.measurementId);
                        span.innerHTML = `
              <span class="i-amount">${spanAmount}</span>
              <span class="i-measurement">${m.unit} </span>
              <span class="i-name">${ingredient.name}</span>`;
                        span.querySelector(".i-measurement").title = m.name;
                    }
                });
            }
            return htmlObject.innerHTML;
        },
        mapStepContent: _.debounce(function (newServing, newMeasurement) {
            this.clonedRecipe.steps = this.clonedRecipe.steps.map((step) => {
                step.content = this.mapHtmlContent(step.content, newServing, newMeasurement);
                return step;
            });
            this.isMappingStepContent = true;
            this.$nextTick(() => {
                this.isMappingStepContent = false;
            });
        }, 100),
    },
    destroyed() {
        this.clonedRecipe = null;
        this.set_recipe(null);
        this.set_recipe_steps([]);
        this.set_recipe_user_rating(null);
    },
    watch: {
        get_recipe: {
            async handler() {
                if (!this.get_recipe) {
                    this.clonedRecipe = null;
                } else {
                    this.clonedRecipe = _.cloneDeep(this.get_recipe);
                    this.isFetchingRecipeSteps = true;
                    this.fetch_recipe_steps(this.get_recipe.id).then(() => {
                        this.clonedRecipe.steps = this.get_recipe_steps;
                        this.mapStepContent();
                        this.isFetchingRecipeSteps = false;
                    });
                    if (this.$msal.isAuthenticated) {
                        this.fetch_recipe_user_rating(this.get_recipe.recipeId).then(() => {
                            this.recipeUserRating = _.cloneDeep(this.get_recipe_user_rating);
                        });
                        this.like = await this.fetch_recipe_like(this.get_recipe.recipeId);
                    }
                }
            },
            deep: true,
            immediate: true,
        },
    },
};
</script>

<style lang="scss">
.recipe-mb {
  background: white;
  &__content, .v-window, .v-window-item {
      width: 100%;
      overflow-x: hidden;
      height: 100%;
  }
  &__footer {
    width: 100%;
      background: #fafafa !important;
    .v-tabs-bar {
      box-shadow: 0px 0px 2px 2px rgb(0 0 0 / 3%) !important;
      border-top: 1px solid #f3f0ea !important;
      background: transparent !important;
      height: 48px;
      .v-tabs-bar__content {
        width: 100%;
        display: flex;
        justify-content: space-between;
        .v-tabs-slider {
          background: #d6b36c !important;
        }
        .v-tab {
                width: 100%;
                white-space: normal;
                font-size: 12px !important;
                padding: 0px !important;
                text-transform: none !important;
                font-weight: 400 !important;
                letter-spacing: normal !important;
                line-height: normal !important;
                min-width: auto !important;
                max-width: auto !important;
                outline: none !important;
                &--active {
                  color: #c3ab7c !important;
                }
          }
      }
    }
  }
}
</style>
