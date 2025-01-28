<template>
<div class="w-full relative p-3">
    <div class="w-full flex flex-wrap" :class="{'justify-center': isMobile}">
        <RecipeCard
            class="p-3"
            v-for="(recipe, i) in recipes"
            :key="i"
            @click.prevent.native="$emit('onClick', recipe)"
            :value="recipe"
            :servings="servings"
        />
    </div>
      <div class="w-full flex flex-wrap" v-if="!hideLoading" :class="{'justify-center': isMobile}">
          <RecipeCardSkeleton class="p-3" v-for="index in skeletons" :key="index+'b'" />
      </div>
    <BaseIntersectionObserver v-if="recipes && recipes.length > 1 && !noMore" @intersecting="intersecting($event)" />
    <div class="p-4 w-full text-center" v-if="loadingMore">
      <baseLoading :value="true" />
    </div>
    <div v-if="noMore" class="p-4 w-full text-center"> {{ $t('noMoreRecipes') }} </div>
    <div v-else-if="recipes.length == 0 && !loadingMore && !loading" class="p-4 w-full text-center"> {{ $t('noRecipesFound') }} </div>
</div>
</template>

<script>
export default {
    name: "RecipeCards",
    components: {
        RecipeCard: () => import("@/components/base/RecipeCard.vue"),
        RecipeCardSkeleton: () => import("@/components/base/RecipeCardSkeleton.vue"),
    },
    props: {
      recipes: {
          type: Array,
          default: () => [],
        },
        loading: {
          type: Boolean,
          default: false
        },
        skeletons: {
          type: Number,
          default: 10
        },
        noMore: {
          type: Boolean,
          default: false
        },
        loadingMore: {
          type: Boolean,
          default: false
        },
        servings: [String, Number]
    },
    data() {
        return {
            hideLoading: true,
        };
    },
    computed: {
      isMobile() {
          return window.innerWidth <= 800;
        },
    },
    methods: {
      intersecting(intersecting) {
          if (
              !this.loading &&
              !this.loadingMore &&
              this.hideLoading &&
              intersecting &&
              this.recipes &&
              this.recipes.length > 0
          ) {
              if(!this.noMore) {
                this.$emit("loadMore");
              }
          }
          return;
      },
    },
    mounted() {
    },
  watch: {
    loading: {
      handler() {
        if(!this.loading) {
          setTimeout(() => {
              this.hideLoading = true;
          }, 400);
        }
        else {
          this.hideLoading = false;
        }
      }, immediate: true
    }
  }
};
</script>
<style lang="scss">
</style>
