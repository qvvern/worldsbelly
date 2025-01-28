<template>
    <baseLayout class="recipe-page">
        <div slot="content" class="recipe-page__content" v-if="clonedRecipe">
            <div class="recipe-page__content-header py-6">
                <div class="max-w-screen-xl px-4 m-auto flex flex-col">
                    <div class="flex flex-wrap justify-between w-full mb-2">
                        <div>
                            <h1 class="recipe-title max-w-2xl">{{ clonedRecipe.title }}</h1>
                            <p class="recipe-summary max-w-2xl">{{ clonedRecipe.summary }}</p>
                        </div>
                        <div class="pt-4 flex items-end">
                            <div class="flex justify-end">
                                <RecipeTopBox
                                    class="mx-3"
                                    :title="$t('cuisine')"
                                    :text="get_countries.find((_) => _.id == clonedRecipe.originCountryId).name"
                                    :flag="get_countries.find((_) => _.id == clonedRecipe.originCountryId).code"
                                />
                                <RecipeTopBox
                                    class="mx-3"
                                    :title="$t('bestServed')"
                                    :text="get_recipe_best_served.find((_) => _.id == clonedRecipe.bestServedId).label"
                                />
                                <RecipeTopBox
                                    class="mx-3"
                                    :title="$t('difficulty')"
                                    :text="get_recipe_difficulty.find((_) => _.id == clonedRecipe.difficultyId).label"
                                />
                                <RecipeTopBox :title="$t('share')" class="mx-3">
                                  <SocialBtn>
                                    <baseIcon :size="18" :title="clonedRecipe.title" :text="clonedRecipe.summary" color="#253a54" value="bi-share-fill" class="cursor-pointer" style="padding-right: 4px"/>
                                  </SocialBtn>
                                </RecipeTopBox>
                                <RecipeTopBox :title="like ? $t('liked') : $t('like')" class="mx-3">
                                  <HeartBtn :recipeId="get_recipe.recipeId" v-model="like" :style="{'padding-right': like ? '3px' : '0px'}"></HeartBtn>
                                </RecipeTopBox>
                                <RecipeDraftsEditRating
                                    v-model="clonedRecipe.rating"
                                    :createdByUser="clonedRecipe.createdByUser.id"
                                    :userRating="recipeUserRating"
                                    :draft="false"
                                    @rating="submitRating($event, true)"
                                    :class="{'is-rating': isRating}"
                                    class="ml-3"
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
                                            v-else-if="item.videoUrl"
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
                                                backdrop-filter: blur(3px);
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
                                        {{ Number(nutrientServingAmount) == 1 ? $t('serving') : $t('servings') }}
                                    </div>
                                </div>
                            </template>
                            <RecipeNutrientTable
                                :nutrients="clonedRecipe.nutrients"
                                :servings="Number(nutrientServingAmount)"
                            />
                        </baseBox>
                    </div>
                    <div class="flex justify-between w-full mt-3">
                        <div class="cell">
                            <div class="flex">
                                <template v-for="(tag, i) in clonedRecipe.tags">
                                    <v-chip :key="i" class="mr-2 recipe-tag" small @click="searchForTag(tag)">{{ tag.name }}</v-chip>
                                </template>
                                <!-- <template v-for="(tag, i) in clonedRecipe.userTags">
                                    <v-chip :key="i" class="mr-2 recipe-tag" small @click="searchForTag(tag, true)">{{ tag.name }}</v-chip>
                                </template> -->
                            </div>
                            <div class="pt-6 w-full flex flex-wrap">
                              <div class="recipe-details-item cursor-pointer" @click="$localeRouter.push('/profile/'+clonedRecipe.createdByUser.username+'/recipes')">
                                <v-avatar
                                  color="#f7f7f7"
                                  size="30"
                                  style="border: 1px solid #d6d6d6"
                                  class="mr-2">
                                  <img v-if="clonedRecipe.createdByUser.image" :src="'https://worldsbellystorage.blob.core.windows.net'+clonedRecipe.createdByUser.image" />
                                  <baseIcon v-else class="pt-2" :size="26" color="#253a54" value="icofont-user" />
                                </v-avatar>
                                <div style="font-size: 12px">
                                    {{ $t('recipeBy') }} <br /><b>{{ clonedRecipe.createdByUser.username }}</b>
                                </div>
                              </div>
                              <div class="recipe-details-item cursor-pointer">
                                <baseIcon class="mr-2 pb-3" :size="18" color="#253a54" value="bi-chat-fill" />
                                <div style="font-size: 12px" @click.prevent="scrollToComments()">
                                   {{ $t('comments') }} <br /><b>{{clonedRecipe.totalComments}}</b>
                                </div>
                              </div>
                              <div class="recipe-details-item">
                                <baseIcon class="mr-2 pb-3" :size="18" color="#253a54" value="bi-heart-fill" />
                                <div style="font-size: 12px">
                                   {{ $t('likes') }} <br /><b>{{clonedRecipe.totaliked}}</b>
                                </div>
                              </div>
                              <div class="recipe-details-item">
                                <baseIcon class="mr-2 pb-3" :size="18" color="#253a54" value="icofont-people" />
                                <div style="font-size: 12px">
                                   {{ $t('views') }} <br /><b>{{clonedRecipe.totalViews + 1}}</b>
                                </div>
                              </div>
                              <div class="recipe-details-item">
                                <baseIcon class="mr-2 pb-3" :size="18" color="#253a54" value="bi-star-fill" />
                                <div style="font-size: 12px">
                                   {{ $t('ratings') }} <br /><b>{{ clonedRecipe.totalRatings }}</b>
                                </div>
                              </div>
                            </div>
                        </div>
                        <div>
                            <div class="flex">
                                <baseBox
                                    :title="$t('totalTime')"
                                    class="ml-4 total-recipe-box"
                                    v-if="clonedRecipe.totalCookingTime > 0 && clonedRecipe.totalPrepTime > 0"
                                >
                                  <div class="total-recipe-time">
                                      <div class="pr-1" v-if="$calculate.mins(Number(clonedRecipe.totalCookingTime) + Number(clonedRecipe.totalPrepTime)).h != 0">
                                        <span class="total-recipe-time-number">{{ $calculate.mins(Number(clonedRecipe.totalCookingTime) + Number(clonedRecipe.totalPrepTime)).h }}</span>{{ $t('hours') }}
                                      </div>
                                      <div>
                                        <span class="total-recipe-time-number">{{ $calculate.mins(Number(clonedRecipe.totalCookingTime) + Number(clonedRecipe.totalPrepTime)).m }}</span>{{ $t('mins') }}
                                      </div>
                                  </div>
                                </baseBox>

                                <baseBox
                                    :title="$t('cookingTime')"
                                    class="ml-4 total-recipe-box"
                                    v-if="clonedRecipe.totalCookingTime > 0"
                                >
                                  <div class="total-recipe-time">
                                      <div class="pr-1" v-if="$calculate.mins(clonedRecipe.totalCookingTime).h != 0">
                                        <span class="total-recipe-time-number">{{ $calculate.mins(clonedRecipe.totalCookingTime).h }}</span>{{ $t('hours') }}
                                      </div>
                                      <div>
                                        <span class="total-recipe-time-number">{{ $calculate.mins(clonedRecipe.totalCookingTime).m }}</span>{{ $t('mins') }}
                                      </div>
                                  </div>
                                </baseBox>

                                <baseBox
                                    :title="$t('prepTime')"
                                    class="ml-4 total-recipe-box"
                                    v-if="clonedRecipe.totalPrepTime > 0"
                                >
                                  <div class="total-recipe-time">
                                      <div class="pr-1" v-if="$calculate.mins(clonedRecipe.totalPrepTime).h != 0">
                                        <span class="total-recipe-time-number">{{ $calculate.mins(clonedRecipe.totalPrepTime).h }}</span>{{ $t('hours') }}
                                      </div>
                                      <div>
                                        <span class="total-recipe-time-number">{{ $calculate.mins(clonedRecipe.totalPrepTime).m }}</span>{{ $t('mins') }}
                                      </div>
                                  </div>
                                </baseBox>
                            </div>
                        </div>
                    </div>
                    <div class="flex flex-wrap w-full my-4">
                        <RecipeDraftsEditTastebuds
                            v-bind="clonedRecipe"
                            :draft="false"
                        />
                    </div>
                </div>
            </div>
            <div class="recipe-page__content-body max-w-screen-xl flex">
                <BaseSidebar style="min-width: 400px" height="100%" class="p-4 pb-0"> <!-- :style="{marginTop: `-35px`}" -->
                    <div class="recipe-ingredients scrollbar-two pb-2" style="overflow-y: auto">
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
                                    :value="clonedRecipe.servingAmount"
                                    @input="newRecipeServings = Number($event)"
                                />
                                <span class="recipe-serving-amount">{{(newRecipeServings == 0 ? clonedRecipe.servingAmount : newRecipeServings) == 1 ? $t('serving') : $t('servings') }}</span>
                            </div>
                            <div class="w-full" style="overflow-y: auto;">
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
                    </div>
                </BaseSidebar>
                <div class="w-full">
                  <div class="recipe-steps w-full">
                      <div class="recipe-steps-dots"></div>
                      <div class="recipe-content-step-wrapper" v-for="(step, i) in clonedRecipe.steps" :key="i">
                          <div class="recipe-content-step pb-6">
                              <BaseCollapsible :label="step.title" :index="i+1">
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
                  <div class="w-full ready-to-serve">
                    <baseIcon value="icofont-restaurant" color="#c3ab7c" style="margin-left: -8px" size="33" />
                    <div class="ready-to-serve-text">{{ $t('readyToServe') }}</div>
                  </div>
                </div>
            </div>
            <RecipeContentComments :recipeId="get_recipe.recipeId" v-if="get_recipe" :recipeByUserId="get_recipe.createdByUser.id"/>
        </div>
        <div slot="content" class="flex items-center justify-center py-8" v-else>
            <h3>{{$t('cannotFindRecipe')}}.</h3>
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
        HeartBtn: () => import("@/components/base/HeartBtn.vue"),
        SocialBtn: () => import("@/components/base/SocialBtn.vue"),
        RecipeDraftsEditRating: () => import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditRating.vue"),
        RecipeDraftsEditTastebuds: () =>
            import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditTastebuds.vue"),
        RecipeTopBox: () => import("@/components/recipe/RecipeTopBox.vue"),
        RecipeNutrientTable: () => import("@/components/recipe/RecipeNutrientTable.vue"),
        RecipeIngredientList: () => import("@/components/recipe/RecipeIngredientList.vue"),
        RecipeContentComments: () => import("@/components/recipe/RecipeContentComments.vue"),
    },
    props: {
        inDialog: Boolean,
    },
    data() {
        return {
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
                VueScrollTo.scrollTo(activeEl, 400, { container: '.recipe-page__content' });
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
.recipe-page {
  background: white;
  .is-rating {
    opacity: .5;
    pointer-events: none;
  }
    &__content {
      width: 100%;
      overflow-x: hidden;
      height: 100%;
        &-header {
            width: 100%;
            background: -webkit-linear-gradient(top, #fdfdfd, #f6f7f7 75%);
            // box-shadow: 0 2px 12px 1px rgba(0,0,0,.05);
            .recipe-tag {
              border: 1px solid #ebebeb;
              background: #ececef;
              span {
                color: #3a414a;
                font-weight: 600;
              }
            }
            .recipe-details-item {
              display: flex;
              align-items: center;
              margin-right: 30px;
            }
            .total-recipe-box {
              .base-box__title {
                font-weight: 500;
                color: #707d95;
                text-transform: none;
                font-size: 16px;
              }
              .total-recipe-time {
                display: flex;
                .total-recipe-time-number {
                    font-family: 'Signika', sans-serif;
                    font-weight: 600;
                    font-size: 24px;
                    padding-right: 3px;
                }
              }
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
        &-body {
            margin: 0 auto;
            .recipe-ingredients {
                -webkit-box-shadow: -3px -2px 10px rgb(234, 234, 234);
                -moz-box-shadow: -3px -2px 10px rgb(234, 234, 234);
                box-shadow: -3px -2px 10px rgb(234, 234, 234);
                background: white;
                min-width: 350px;
                height: 100%;
                border: 1px solid var(--border-color);
                border-radius: 30px;
                .recipe-serving-amount {
                  font-weight: 500;
                  font-size: 14px;
                  text-transform: uppercase;
                  color: #091736;
                }
                .ingredients-list__title {
                  padding: 15px 0px;
                  width: 100%;
                  position: sticky;
                  background: white;
                  top: 0px;
                }
            }
            .ready-to-serve {
              padding-top: 5px;
              padding-left: 7px;
              display: flex;
              align-items: flex-start;
              .ready-to-serve-text {
                padding: 8px 10px;
                font-size: 18px;
                font-family: 'Signika', sans-serif;
                color: #5c5126;
              }
            }
            .recipe-steps {
              padding-top: 40px;
              padding-left: 40px;
              padding-right: 10px;
              padding-bottom: 40px;
              position: relative;
              &-dots {
                background: radial-gradient(ellipse at center, #c3ab7c 20%, #c3ab7c 20%, transparent 40%);
                background-size: 10px 10px;
                background-repeat: repeat-y;
                background-position: 0px center;
                width: 100%;
                position: absolute;
                top: 0;
                left: 10px;
                height: 100%;
              }
                [data-ingredient] {
                    width: fit-content;
                    font-size: 14px !important;
                    color: rgba(0, 0, 0, 0.87) !important;
                    font-family: "Roboto", sans-serif !important;
                    font-weight: 400;
                    margin-right: 0px;
                    margin-top: 5px;
                    padding: 2px;
                    padding-left: 5px;
                    padding-right: 5px;
                    background: #f3e1be;
                    position: relative;
                    display: inline-flex;
                    .i-name {
                        padding-left: 5px;
                        font-weight: 500;
                    }
                }
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
}
</style>
