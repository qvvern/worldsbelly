<template>
<div class="recipes-sort-by" ref="recipes-sort-by" :style="{top: isHome ? '60px' : '0px'}">
    <div class="recipes-sort-by-opener">
      <v-btn
        :elevation="0"
        small
        dense
        color="#fff"
        @click="isOpen = !isOpen"
      >
        <p>{{options.find(_ => _.key == sortBy).label}}</p>
        <v-icon>
          {{ sortDescending ? 'bi-sort-up' : 'bi-sort-down' }}
        </v-icon>
      </v-btn>
    </div>
    <div class="recipes-sort-by-dropdown" v-if="isOpen">
      <div class="flex justify-end w-full">
        <v-icon :size="18" class="cursor-pointer" @click="$emit('update:sortDescending', !sortDescending)">
          {{ sortDescending ? 'bi-sort-up' : 'bi-sort-down' }}
        </v-icon>
      </div>
      <ul>
        <li v-for="option in options" :key="option.key" :class="{active: option.key == sortBy}" @click="$emit('update:sortBy', option.key), isOpen = false">
          {{option.label}}
        </li>
      </ul>
    </div>
  <!-- {{ options.find(_ => _.key == sortBy) }} -->
</div>
</template>

<script>
export default {
    name: "RecipesSortBy",
    props: {
      sortBy: {
        type: String,
        default: ""
      },
      sortDescending: Boolean,
      isHome: {
        type: Boolean,
        default: false
      },
    },
    data() {
        return {
          isOpen: false,
          options: [
            {
              key: "",
              label: this.$t('popularity')
            },
            {
              key: "Rating",
              label: this.$t('rating')
            },
            {
              key: "Newest",
              label: this.$t('newest')
            },
            {
              key: "TotalTime",
              label: this.$t('time')
            },
            {
              key: "TotalViews",
              label: this.$t('views')
            },
            {
              key: "TotalLikes",
              label: this.$t('likes')
            },
            {
              key: "TotalRating",
              label: this.$t('totalRating')
            },
          ]
        };
    },
  methods: {
    // cancel() {
    //   this.isOpen = false;
    // },
    // apply() {
    //   this.isOpen = false;
    // },
    documentClick(e) {
      if (this.isOpen) {
        const el = this.$refs["recipes-sort-by"];
        const { target } = e;
        if (el !== target && !el.contains(target)) {
          this.isOpen = false;
        }
      }
    },
  },
  created() {
    document.addEventListener("mousedown", this.documentClick);
  },
  destroyed() {
    document.removeEventListener("mousedown", this.documentClick);
  },
};
</script>

<style lang="scss">
.recipes-sort-by {
    width: 100%;
    position: sticky;
    right: 30px;
    z-index: 8;
    /* float: right; */
    display: flex;
    height: 0px;
    justify-content: flex-end;
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
  &-dropdown {
    position: absolute;
    width: 125px;
    right: 10px;
    top: 25px;
    background: white;
    color: #000;
    box-shadow: 0 2px 6px 0 rgba(0, 0, 0, 0.2);
    z-index: 999;
    border-radius: 4px;
    border: 1px solid #ddd;
    .v-icon {
      position: absolute;
      margin: 5px;
    }
    ul {
      list-style: none;
      cursor: pointer;
      padding-left: 0px !important;
      li {
        &:first-child {
          padding-top: 5px;
        }
        padding: 3px 10px;
        font-size: 14px;
        font-weight: 400;
        &:hover {
          background: rgba(0, 0, 0, 0.1);
        }
        &.active {
          font-weight: 500;
          background: rgba(161, 205, 226, 0.1);
        }
      }
    }
  }
}
</style>
