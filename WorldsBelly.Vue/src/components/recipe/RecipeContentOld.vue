<template>
    <baseLayout class="recipe-content">
        <div class="recipe-page" slot="content">
            <template v-if="clonedRecipe">
                <div class="recipe-page__header py-6">
                    <div class="max-w-screen-xl px-4 m-auto flex flex-col">
                        <div class="flex flex-wrap justify-between w-full mb-2">
                            <div>
                                <h1 class="recipe-title max-w-2xl">{{ clonedRecipe.title }}</h1>
                                <p class="recipe-summary max-w-2xl">{{ clonedRecipe.summary }}</p>
                            </div>
                            <div class="pt-4">
                                <div class="flex">
                                    <RecipeTopBox
                                        title="Cuisines (Origin)"
                                        :text="get_countries.find((_) => _.id == clonedRecipe.originCountryId).name"
                                        :flag="get_countries.find((_) => _.id == clonedRecipe.originCountryId).code"
                                    />
                                    <RecipeTopBox
                                        title="Best Served"
                                        :text="
                                            get_recipe_best_served.find((_) => _.id == clonedRecipe.bestServedId).label
                                        "
                                    />
                                    <RecipeTopBox
                                        title="Diffculty"
                                        :text="
                                            get_recipe_difficulty.find((_) => _.id == clonedRecipe.difficultyId).label
                                        "
                                    />
                                    <RecipeDraftsEditRating
                                        v-model="clonedRecipe.rating"
                                        :createdByUser="clonedRecipe.createdByUser.id"
                                        :userRating="recipeUserRating"
                                        @rating="recipeUserRating.rating = $event"
                                    />
                                </div>
                            </div>
                        </div>
                        <div class="flex w-full justify-between" style="height: 400px; position: relative">
                            <div class="image-container mr-1" style="max-width: calc(100% - 410px)">
                                <BaseCarousel
                                    ref="mainslide"
                                    height="100%"
                                    width="100%"
                                    class="mr-1"
                                    :pagination="recipeSlides.length > 1"
                                    @activeSlide="setActiveSlide($event)"
                                >
                                    <BaseCarouselSlide
                                        width="100%"
                                        v-for="(item, i) in recipeSlides"
                                        :key="i"
                                        style="background: #d4d4d4; text-align: center"
                                    >
                                        <div style="height: 400px; position: relative">
                                            <img
                                                v-if="item.type == 'image'"
                                                :src="item.url"
                                                width="100%"
                                                height="auto"
                                                style="
                                                    transform: translate(0px, -50%);
                                                    position: absolute;
                                                    top: 50%;
                                                    left: 0;
                                                "
                                            />
                                            <iframe
                                                v-else
                                                class="video"
                                                width="100%"
                                                height="400"
                                                :src="item.videoUrl"
                                                title="YouTube video player"
                                                frameborder="0"
                                                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                                allowfullscreen
                                            ></iframe>
                                        </div>
                                    </BaseCarouselSlide>
                                </BaseCarousel>
                                <div class="image-container-thumbs scrollbar-one" v-if="recipeSlides.length > 1">
                                    <div
                                        @click="$refs.mainslide.toSlide(i)"
                                        v-for="(item, i) in recipeSlides"
                                        :key="i"
                                        class="cursor-pointer mb-1 image-container-thumbs-thumb"
                                        :class="{ active: i == activeSlide }"
                                        style="
                                            height: 65px;
                                            width: 100px;
                                            overflow: hidden;
                                            position: relative;
                                            background-size: cover;
                                            border-radius: 5px;
                                        "
                                        :style="{ 'background-image': `url('${item.url}')` }"
                                    >
                                        <template v-if="item.type == 'video'">
                                            <div
                                                style="
                                                    position: absolute;
                                                    top: 0;
                                                    left: 0;
                                                    width: 100%;
                                                    height: 100%;
                                                    background: rgba(0, 0, 0, 0.4);
                                                    backdrop-filter: blur(1px);
                                                "
                                            ></div>
                                            <baseIcon color="#fff" class="position-center">icofont-play-alt-2</baseIcon>
                                        </template>
                                    </div>
                                </div>
                            </div>
                            <baseBox padding="pt-2" paddingTitle="px-4" style="opacity: 0.75; min-width: 400px" scrollable>
                                <template slot="header">
                                    <div class="flex flex-row items-center justify-between">
                                        <div style="width: 100%">Nutrients by</div>
                                        <div class="flex flex-row items-center">
                                            <baseQuantityInput
                                                class="mr-3"
                                                :min="1"
                                                width="110px"
                                                labelRight
                                                :value="nutrientServingAmount"
                                                @input="nutrientServingAmount = Number($event)"
                                            />
                                            {{ Number(nutrientServingAmount) == 1 ? "Serving" : "Servings" }}
                                        </div>
                                    </div>
                                </template>
                                <RecipeNutrientTable
                                    :nutrients="clonedRecipe.nutrients"
                                    :servings="Number(nutrientServingAmount)"
                                />
                            </baseBox>
                        </div>
                        <div class="flex justify-between w-full my-4">
                            <div class="cell">
                                <div class="flex">
                                    <template v-for="(tag, i) in [...clonedRecipe.tags, ...clonedRecipe.userTags]">
                                        <v-chip :key="i" class="mr-2" small>{{ tag.name }}</v-chip>
                                    </template>
                                </div>
                                <p class="pt-3" style="font-size: 12px">
                                    Recipe by <br /><b>{{ clonedRecipe.createdByUser.username }}</b>
                                </p>
                            </div>
                            <div>
                                <div class="flex">
                                    <baseBox
                                        title="total time"
                                        style="opacity: 0.75"
                                        v-if="clonedRecipe.totalCookingTime > 0 && clonedRecipe.totalPrepTime > 0"
                                    >
                                        <b>{{
                                            Number(clonedRecipe.totalCookingTime) + Number(clonedRecipe.totalPrepTime)
                                        }}</b>
                                        mins
                                    </baseBox>
                                    <baseBox
                                        title="total cooking time"
                                        class="ml-4"
                                        v-if="clonedRecipe.totalCookingTime > 0"
                                    >
                                        <span class="p-1 pl-0"
                                            ><b>{{ clonedRecipe.totalCookingTime }}</b></span
                                        >mins
                                    </baseBox>
                                    <baseBox title="total prep time" class="ml-4" v-if="clonedRecipe.totalPrepTime > 0">
                                        <span class="p-1 pl-0"
                                            ><b>{{ clonedRecipe.totalPrepTime }}</b></span
                                        >mins
                                    </baseBox>
                                </div>
                            </div>
                        </div>
                        <div class="flex flex-wrap w-full my-4">
                            <RecipeDraftsEditTastebuds
                                v-bind="clonedRecipe"
                                :userRating="recipeUserRating"
                                @rating="recipeUserRating = $event"
                            />
                        </div>
                    </div>
                </div>
                <div class="recipe-page__content">
                    <div class="recipe-page__body flex w-full h-full">
                        <BaseSidebar>
                          <div class="recipe-ingredients">
                              <div class="ingredients-list">
                                  <div class="ingredients-list__title pb-2 flex justify-between w-full items-center px-4">
                                      <h3 class="p-2">Ingredients</h3>
                                      <baseQuantityInput
                                          class="mr-3 mt-2"
                                          label="Amount of servings"
                                          :min="1"
                                          width="110px"
                                          labelRight
                                          :value="clonedRecipe.servingAmount"
                                          @input="newRecipeServings = Number($event)"
                                      />
                                  </div>
                                  <RecipeIngredientList
                                      v-for="(ingredientList, i) in clonedRecipe.ingredientLists"
                                      :key="i"
                                      :value="ingredientList"
                                      :recipeServings="clonedRecipe.servingAmount"
                                      :newRecipeServings.sync="newRecipeServings"
                                      @updatedIngredientMeasurement="mapStepContent(null, $event)"
                                      @updatedIngredientServing="mapStepContent($event, null)"
                                  />
                              </div>
                        </div>
                        </BaseSidebar>
                        <div class="recipe-content">
                            <div class="recipe-content-step-wrapper" v-for="(step, i) in clonedRecipe.steps" :key="i">
                                <div class="recipe-content-step">
                                    <BaseCollapsible :label="step.title">
                                        <div v-html="step.content"></div>
                                        <div class="recipe-content-step-media w-full flex flex-wrap items-center">
                                            <div
                                                class="recipe-content-step-media-container pt-6 mr-3"
                                                style="width: 100%; max-width: 400px"
                                                v-if="step.videoUrl"
                                            >
                                                <BaseIframe
                                                    v-if="step.videoUrl"
                                                    :aspectRatio="2"
                                                    class="video"
                                                    width="100%"
                                                    :src="step.videoUrl"
                                                    title="YouTube video player"
                                                    frameborder="0"
                                                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                                    allowfullscreen
                                                ></BaseIframe>
                                            </div>
                                            <div
                                                class="recipe-content-step-media-container pt-6"
                                                style="width: 100%; max-width: 355px"
                                                v-if="step.imageUrl"
                                            >
                                                <base-image
                                                    :value="'https://worldsbellystorage.blob.core.windows.net' + step.imageUrl"
                                                />
                                                <!-- <v-img contain :aspect-ratio="16/9" v-if="step.imageUrl" width="100%" :src="'https://worldsbellystorage.blob.core.windows.net'+step.imageUrl" /> -->
                                            </div>
                                        </div>
                                    </BaseCollapsible>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
            <template v-else>
                <div class="flex items-center justify-center py-8">
                    <h3>Cannot find recipe.</h3>
                </div>
            </template>
        </div>
    </baseLayout>
</template>

<script>
import VueScrollTo from "vue-scrollto";
import { mapGetters, mapMutations, mapActions } from "vuex";
import _ from "lodash";

export default {
    name: "RecipeContent",
    components: {
        BaseSidebar: () => import("@/components/base/BaseSidebar.vue"),
        BaseCollapsible: () => import("@/components/base/BaseCollapsible.vue"),
        BaseCarousel: () => import("@/components/base/BaseCarousel.vue"),
        BaseCarouselSlide: () => import("@/components/base/BaseCarouselSlide.vue"),
        RecipeDraftsEditRating: () => import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditRating.vue"),
        RecipeDraftsEditTastebuds: () =>
            import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditTastebuds.vue"),
        RecipeTopBox: () => import("@/components/recipe/RecipeTopBox.vue"),
        RecipeNutrientTable: () => import("@/components/recipe/RecipeNutrientTable.vue"),
        RecipeIngredientList: () => import("@/components/recipe/RecipeIngredientList.vue"),
    },
    props: {
        inDialog: Boolean,
    },
    data() {
        return {
            clonedRecipe: null,
            recipeUserRating: null,
            isFetchingRecipeSteps: false,
            newRecipeServings: 0,
            nutrientServingAmount: 1,
            activeSlide: 0,
            isMappingStepContent: true,
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
            "get_recipe_steps"
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
                        videoUrl: this.clonedRecipe.videoUrl,
                        url: "https://worldsbellystorage.blob.core.windows.net" + step.imageUrl,
                    });
                }
            });
            return slides;
        },
    },
    methods: {
        ...mapMutations("recipe/", ["set_recipe", "set_recipe_user_rating", "set_recipe_steps"]),
        ...mapActions("recipe/", ["fetch_recipe_user_rating", "submit_recipe_user_rating", "fetch_recipe_steps"]),
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
                        spanAmount
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
            handler() {
                if (!this.get_recipe) {
                    this.clonedRecipe = null;
                } else {
                  this.recipeUserRating = {
                        recipeId: this.get_recipe.recipeId,
                        sweet: null,
                        sour: null,
                        salty: null,
                        bitter: null,
                        spices: null,
                        flavor: null,
                        rating: null,
                    };
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
                    }
                }
            },
            deep: true,
            immediate: true,
        },
    },
};
</script>

<style lang="scss" scoped>
.recipe-content {
    display: flex;
    flex-direction: column;
    position: relative;
    height: 100%;
    width: 100%;
    background: #fff !important;
}
</style>

<style lang="scss">
.recipe-page {
    [data-ingredient] {
        width: fit-content;
        font-size: 14px !important;
        color: rgba(0, 0, 0, 0.87) !important;
        font-family: "Roboto", sans-serif !important;
        font-weight: 400;
        margin-right: 5px;
        margin-top: 5px;
        padding: 2px;
        padding-left: 5px;
        padding-right: 5px;
        background: #cee7ff;
        position: relative;
        .i-name {
            font-weight: 500;
        }
    }
    position: relative;
    // height: 100%;
    width: 100%;
    &__header {
        width: 100%;
        // background: #f8fafc;
        background: -webkit-linear-gradient(top, #f8fafc, #fafafa 100%);
        // -webkit-box-shadow: inset 0 20px 20px -20px rgba(0,0,0,0.2);
        // -moz-box-shadow: inset 0 20px 20px -20px rgba(0,0,0,0.2);
        // box-shadow: inset 0 20px 20px -20px rgba(0,0,0,0.2);
        // border-bottom: 1px solid var(--border-color);
        &-bar {
            // border-top: 1px solid var(--border-color);
            // border-bottom: 1px solid var(--border-color);
        }
        .image-container-thumbs {
            overflow-y: auto;
            height: 100%;
            width: 124px;
            overflow-x: hidden;
            &-thumb {
                border: 2px solid #f9fafa;
                &.active {
                    border: 2px solid rgb(111, 245, 111);
                }
            }
        }
        .image-container {
            width: 100%;
            display: flex;
            // background: #f2f2f2;
            // background: #e5e7e9;
            // padding: 20px;
            &__main {
                position: relative;
                box-shadow: #9e9e9e24 0px 0px 6px 0px;
                background: #f2f2f2a1; //#f2f2f2;
                width: 100%;
                height: 350px;
                display: flex;
                align-items: center;
                justify-content: center;
                overflow: hidden;
                flex-direction: column;
                position: relative;
            }
            &__sub {
                &-image {
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-direction: column;
                    border: 2px dotted var(--border-color);
                    border-left: 0px;
                    border-top-right-radius: 10px;
                    border-bottom-right-radius: 10px;
                    background: #fff;
                    width: 75px;
                    height: 75px;
                    p {
                        padding-top: 2px;
                        font-size: 10px;
                    }
                    &:hover {
                        cursor: pointer;
                        border: 2px solid rgb(31, 29, 29);
                        border-left: 0px;
                        transition: all 400ms;
                        background: rgb(63, 184, 15);
                        * {
                            color: white;
                        }
                    }
                }
            }
        }
    }
    &__content {
        overflow: hidden;
        .recipe-ingredients {
            position: sticky;
            margin-left: 20px;
            margin-top: 20px;
            border-top: 3px solid var(--border-color);
            border-left: 3px solid var(--border-color);
            border-right: 3px solid var(--border-color);
            border-top-left-radius: 50px;
            border-top-right-radius: 50px;
            // box-shadow: 1px -1px 22px -2px rgba(37,64,92,0.18);
            // -webkit-box-shadow: 1px -1px 22px -2px rgba(37,64,92,0.18);
            // -moz-box-shadow: 1px -1px 22px -2px rgba(37,64,92,0.18);
            padding: 15px 0px;
            max-width: 475px;
            min-width: 475px;
        }
        .recipe-content {
            padding: 40px;
            width: 56%;
            &-step-wrapper {
                width: 100%;
                margin-bottom: 20px;
                border-radius: 15px;
            }
            &-step {
                width: 100%;
                padding: 0px;
                overflow: hidden;
                word-break: break-all;
            }
            &-step-actions {
                margin-bottom: 20px;
                // padding: 10px;
                &__btn {
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-direction: column;
                    border: 1px dashed rgba(0, 0, 0, 0.25);
                    border-radius: 15px;
                    width: 65px;
                    height: 65px;
                    &.disabled {
                        pointer-events: none;
                        opacity: 0.75;
                        background: rgb(228, 228, 228);
                    }
                    p {
                        padding-top: 2px;
                        font-size: 10px;
                    }
                    &:hover {
                        cursor: pointer;
                        border: 1px dashed rgba(0, 0, 0, 1);
                        transition: all 400ms;
                        background: #fff;
                    }
                }
            }
        }
    }
}
</style>
