<template>
<div class="recipe-card-wrapper">
            <v-skeleton-loader
              class="recipe-card-skeleton cursor-pointer p-0"
               @mouseenter="hover = true" @mouseleave="hover = false" :class="{hovered: hover}"
              type="image, list-item-avatar, sentences, table-tfoot"
              min-width="290"
            />
</div>
</template>

<script>
export default {
    name: "RecipeCardSkeleton",
    data() {
        return {
            hover: false,
        };
    },
};
</script>

<style lang="scss">
.recipe-card-skeleton {
    overflow: hidden;
    position: relative;
    background: #f9f9f9 !important;
    background: -webkit-linear-gradient(top, #f8fafc, #f5f5f5 100%) !important;
    border-radius: 10px !important;
    height: fit-content;
    max-width: 300px;
    -webkit-box-shadow: 0px 0px 4px 0px rgba(0,0,0,0.15) !important;
    box-shadow: 0px 0px 4px 0px rgba(0,0,0,0.15) !important;
    &.hovered {
      transition: box-shadow .4s;
      -webkit-box-shadow: 0px 0px 16px 0px rgba(0,0,0,0.2) !important;
      box-shadow: 0px 0px 16px 0px rgba(0,0,0,0.2) !important;
    }
    .v-skeleton-loader__table-tfoot, .v-skeleton-loader__list-item-avatar, .v-skeleton-loader__sentences{
      background: #f6f6f6 !important;
    }
    .v-skeleton-loader__list-item-avatar {
      display: flex !important;
      flex-direction: row-reverse !important;
      .v-skeleton-loader__text {
        margin-right: 100px !important;
      }
      .v-skeleton-loader__avatar {
        margin-right: 0px !important;
        margin-left: 15px !important;
        height: 30px !important;
        width: 30px !important;
      }
    }
    .v-skeleton-loader__sentences {
      margin-top: -5px !important;
      padding: 0px 15px !important;
      display: flex !important;
      flex-direction: column-reverse !important;
       margin-bottom: 25px !important;
      .v-skeleton-loader__text {
        height: 8px !important;
      }
      .v-skeleton-loader__text:nth-child(2) {
       margin-bottom: 10px !important;
      }
    }
    .v-skeleton-loader__table-tfoot {
      display: flex !important;
      justify-content: space-between !important;
      .v-skeleton-loader__avatar {
        display: none !important;
      }
      .v-skeleton-loader__text {
        height: 6px !important;
        margin-left: 0px !important;
        max-width: 64px !important;
      }
    }
}
</style>
