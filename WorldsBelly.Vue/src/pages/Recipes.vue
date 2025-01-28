<template>
    <RecipesContent
        ref="RecipesContent"
        class="recipes-page"
        @isRecipesFetched="isRecipesFetched = $event"
    />
</template>

<script>

export default {
    name: "Recipes",
    components: {
        RecipesContent: () => import("@/components/recipes/RecipesContent.vue"),
    },
    data() {
        return {
        };
    },
    computed: {
    },
    methods: {
    },
};
</script>

<style lang="scss" >
.recipes-page {
}
</style>
