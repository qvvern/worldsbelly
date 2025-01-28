<template>
  <div class="base-sidebar-mb-wrapper" :style="{top: isHome ? '60px' : '0px'}">
      <div class="base-sidebar-mb-opener">
        <v-btn
          :elevation="0"
          small
          dense
          color="#fff"
          @click="$parent.isOpen = !$parent.isOpen"
        >
          <v-icon>
            {{ 'bi-sort-down' }}
          </v-icon>
          <p>Filters</p>
        </v-btn>
      </div>
  </div>
  </template>

  <script>
  export default {
      name: "RecipesSortBy",
      props: {
        isHome: {
          type: Boolean,
          default: false
        },
      },
      components: {
      },
      data() {
          return {
            isOpen: false,
          };
      },
    methods: {
    },
  };
  </script>

  <style lang="scss">
  .base-sidebar-mb-wrapper {
      width: 100%;
      position: sticky;
      left: 30px;
      z-index: 8;
      /* float: right; */
      display: flex;
      height: 0px;
      justify-content: flex-start;
    &-opener {
      button {
        margin: 3px;
        height: 35px !important;
        p {
          font-size: 11px;
          font-weight: 400;
          padding-right: 5px;
          text-transform: lowercase;
        }
        .v-icon {
          font-size: 16px !important;
        }
      }
    }
  }
  </style>
