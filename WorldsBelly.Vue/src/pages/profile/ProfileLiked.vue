<template>
    <div class="page-profile">
      <div v-if="isSearching || (get_recipes && get_recipes.length > 0)" class="w-full">
        <RecipeCards
          :recipes="isSearching ? [] : get_recipes ? get_recipes : []"
          :loading="isSearching"
          :loadingMore="isLoadingMore"
          @loadMore="appendRecipes"
          :noMore="noMore"
          @onClick="goToRecipe($event)"
          :skeletons="1"
        />
      </div>
      <div v-else>
        {{ $t('noLikesFromUser') }}
      </div>
    </div>
</template>

<script>
import { mapGetters, mapMutations, mapActions } from 'vuex'
    export default {
      name: "ProfileLiked",
      components: {
          RecipeCards: () => import("@/components/base/RecipeCards.vue"),
      },
      data() {
          return {
            collection: "liked",
            isSearching: false,
            isLoadingMore: false,
            noMore: false
          }
      },
      computed: {
        ...mapGetters("user/", ["get_recipes_filter_collection", "get_recipes_collection", "get_user"]),
        get_recipes_filter() {
          return this.get_recipes_filter_collection(this.collection);
        },
        get_recipes() {
          return this.get_recipes_collection(this.collection);
        },
      },
      methods: {
          ...mapActions('user/', ['fetch_recipes_related_to_user', 'fetch_append_recipes_related_to_user']),
          ...mapMutations('user/', ['set_recipes_filter_collection']),
        appendRecipes() {
            this.isLoadingMore = true;

            this.fetch_append_recipes_related_to_user(this.collection)
                .then((response) => {
                    if (response.pageSize > response.recipes.length) {
                        this.noMore = true;
                    }
                })
                .finally(() => {
                    this.isLoadingMore = false;
                });
        },
        goToRecipe(recipe) {
            this.$localeRouter.push({
                name: "Recipe",
                params: { id: recipe.id },
            });
        },
      },
      mounted() {
        if(!this.get_recipes_filter) {
          this.set_recipes_filter_collection({
            collection: this.collection,
            filter: {
              page: 1,
              likedBy: this.get_user.id
            }
          })
        }
        if(!this.get_recipes) {
          this.isSearching = true;
          this.fetch_recipes_related_to_user(this.collection).finally(() => {
            this.isSearching = false;
          });
        }
      }
    }
</script>

<style lang="scss" scoped>
.page-profile {
    &-tabs {
        // border-bottom: 1px solid red;
    }
}
</style>
