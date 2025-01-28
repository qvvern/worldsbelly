<template>
    <div class="recipe-page w-full h-full">
        <template v-if="isPageReady">
            <div v-if="(get_recipe && get_recipe.id) || isFound" class="recipe-page-content w-full h-full">
                <RecipeContent v-if="!isMobile" :recipe="get_recipe" @isFound="isFound = true"/>
                <RecipeContentMobile v-else :recipe="get_recipe" @isFound="isFound = true"/>
            </div>
            <div v-else class="flex justify-center w-full text-center p-5 mt-5">{{ $t('notFound') }}</div>
        </template>
        <template v-else>
            <BaseProgressLinear></BaseProgressLinear>
        </template>
    </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";

export default {
    name: "recipe",
    components: {
        RecipeContent: () => import("@/components/recipe/RecipeContent.vue"),
        RecipeContentMobile: () => import("@/components/recipe/RecipeContentMobile.vue"),
    },
    data() {
        return {
            isPageReady: false,
            isFound: false
        };
    },
    computed: {
        ...mapGetters("recipe/", ["get_recipe"]),
        isMobile() {
          return window.innerWidth <= 800;
        }
    },
    methods: {
        ...mapActions("recipe/", ["fetch_recipe"]),
    },
    created() {
      if(this.$route.params?.id) {
        this.fetch_recipe(this.$route.params.id).finally(() => {
            this.isPageReady = true;
        });
      }
    },
};
</script>

<style lang="scss" >
.recipe-page {
    overflow: hidden;
    position: relative;
}
</style>
