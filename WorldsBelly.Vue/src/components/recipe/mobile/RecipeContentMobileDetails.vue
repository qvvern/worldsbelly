<template>
    <div class="recipe-mb-details w-full flex flex-col">
      <div class="recipe-mb-details-top">
        <div class="recipe-mb-details-like">
          <HeartBtn :recipeId="value.recipeId" v-model="like" :style="{'padding-right': like ? '3px' : '0px'}"></HeartBtn>
        </div>
        <div class="recipe-mb-details-images">
              <BaseCarousel
                  ref="mainslide"
                  :height="carouselHeight+'px'"
                  width="100%"
                  direction="vertical"
                  class="mr-1"
                  :pagination="recipeSlides.length > 1"
                  @activeSlide="setActiveSlide($event)"
              >
                  <BaseCarouselSlide
                      width="100%"
                      v-for="(item, i) in recipeSlides"
                      :key="i"
                      style="text-align: center"
                  >
                          <img
                              v-if="item.type == 'image'"
                              :src="item.url"
                              width="100%"
                              height="auto"
                          />
                          <iframe
                              v-else-if="item.videoUrl"
                              class="video"
                              width="100%"
                              :height="carouselHeight"
                              :src="item.videoUrl"
                              title="YouTube video player"
                              frameborder="0"
                              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                              allowfullscreen
                          ></iframe>
                  </BaseCarouselSlide>
              </BaseCarousel>
                <div class="image-container-mb-thumbs scrollbar-one pt-1" v-if="recipeSlides.length > 1">
                    <div
                        @click="$refs.mainslide.toSlide(i)"
                        v-for="(item, i) in recipeSlides"
                        :key="i"
                        class="cursor-pointer mb-1 image-container-mb-thumbs-thumb"
                        :class="{ active: i == activeSlide }"
                        style="
                            height: 35px;
                            min-width: 60px;
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
                            <baseIcon color="#fff" class="position-center" style="position: absolute">icofont-play-alt-2</baseIcon>
                        </template>
                    </div>
                </div>
        </div>
      </div>
      <div class="recipe-mb-details-bottom">
        <div class="recipe-mb-details-bottom__inner">
          <div class="recipe-mb-details-details p-2">
              <h1 class="recipe-mb-details-details__title">{{ value.title }}</h1>
              <div class="flex w-full items-center justify-center">
                <v-rating
                  style="margin-top: -5px;"
                  hover
                  length="5"
                  size="18"
                  v-model="value.rating"
                  color="yellow darken-1"
                  background-color="grey darken-1"
                />
              </div>
              <p class="recipe-mb-details-details__summary">{{ value.summary }}</p>
          </div>
          <div class="recipe-mb-details-tags px-2">
              <template v-for="(tag, i) in value.tags">
                  <v-chip :key="i" class="mr-2 recipe-mb-details-tags__tag" small @click="searchForTag(tag)">{{ tag.name }}</v-chip>
              </template>
          </div>
            <div class="flex flex-wrap w-full my-4">
                <RecipeDraftsEditTastebuds
                    v-bind="value"
                    mobile
                    :draft="false"
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
    name: "RecipeContentMobileDetails",
    props: {
      value: Object
    },
    components: {
        BaseCarousel: () => import("@/components/base/BaseCarousel.vue"),
        BaseCarouselSlide: () => import("@/components/base/BaseCarouselSlide.vue"),
        RecipeDraftsEditTastebuds: () => import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditTastebuds.vue"),
        RecipeDraftsEditRating: () => import("@/components/administration/recipeDraftsEdit/RecipeDraftsEditRating.vue"),
        HeartBtn: () => import("@/components/base/HeartBtn.vue"),
        RecipeTopBox: () => import("@/components/recipe/RecipeTopBox.vue"),
    },
    data() {
        return {
            activeSlide: 0,
            like: false
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
        carouselHeight() {
          return window.innerWidth / 1.5;
        },
        recipeSlides() {
            var slides = [];
            slides.push({
                type: "image",
                url: "https://worldsbellystorage.blob.core.windows.net" + this.value.imageUrl + "main",
            });
            if (this.value.videoUrl) {
                slides.push({
                    type: "video",
                    videoUrl: this.value.videoUrl,
                    url: "https://worldsbellystorage.blob.core.windows.net" + this.value.imageUrl + "main",
                });
            }
            this.value.steps.forEach((step) => {
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
                        url: step.imageUrl ? "https://worldsbellystorage.blob.core.windows.net" + step.imageUrl : "https://worldsbellystorage.blob.core.windows.net" + this.value.imageUrl + "main",
                    });
                }
            });
            return slides;
        },
    },
    methods: {
        setActiveSlide(index) {
            this.activeSlide = index;
            this.$nextTick(() => {
                var activeEl = document.querySelector(".image-container-mb-thumbs-thumb.active");
                VueScrollTo.scrollTo(activeEl, 400, { container: ".image-container-mb-thumbs" });
            });
        },
    },
    destroyed() {
    },
    watch: {
    },
};
</script>

<style lang="scss">
.recipe-mb-details {
    .v-rating .v-icon {
      padding: 0 !important;
    }
    text-align: center;
    &-top {
      width: 100%;
    }
    &-bottom {
      position: relative;
      width: 100%;
      &__inner {
        position: absolute;
        z-index: 1;
        margin-top: -25px;
        background: white;
        width: 100%;
        border-top-left-radius: 25px;
        border-top-right-radius: 25px;

      }
    }
    &-like {
      z-index: 9;
      position: absolute;
      top: 5px;
      right: 5px;
    }
  &-images {
      position: relative;
      .image-container-mb-thumbs {
          z-index: 1;
          position: absolute;
          margin-top: -70px;
          display: flex;
          justify-content: center;
          overflow-x: auto;
          height: 100%;
          width: 100%;
          overflow-y: hidden;
          &-thumb {
              margin-right: 5px;
              border: 1px solid #000000;
              &.active {
                  border: 1px solid rgb(111, 245, 111);
              }
          }
      }
  }
  &-details {
    text-align: center;
    &__title {
      font-size: 20px;
    }
    &__summary {
      font-size: 14px;
    }
  }
  &-tags {
      &__tag {
          border-radius: 0px !important;
          border: 0px !important;
          background: #ededed !important;
          height: auto !important;
        span {
          font-size: 11px;
          color: #3a414a;
          font-weight: 500 !important;
        }
      }
  }
}
</style>
