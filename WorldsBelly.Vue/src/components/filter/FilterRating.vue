<template>
    <BaseExpandContent class="side-filter" :label="$t('rating')" :labelContext="!expanded ? '' : `${range[0]} - ${range[1]}`" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
          <FilterRange class="side-filter-rating" v-model="range" :min="min" :max="max" lineColor="#fdd835" thumbColor="#efc408" />
    </BaseExpandContent>
</template>

<script>
import _ from "lodash";

export default {
    name: "FilterRating",
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
        FilterRange: () => import("@/components/filter/FilterRange.vue"),
    },
    props: {
      expanded: Boolean,
      queries: Object
    },
    data() {
        return {
            range: [0, 10],
            min: 0,
            max: 10,
            clonedRange: []
        };
    },
    mounted() {
      if(this.queries.fromrating) {
          this.range[0] = Number(this.queries.fromrating);
        this.$emit('update:expanded', true)
      }
      if(this.queries.torating) {
          this.range[1] = Number(this.queries.torating);
        this.$emit('update:expanded', true)
      }
    },
    watch: {
      range: {
        handler() {
          if(this.min != this.range[0] || this.max != this.range[1]) {
            this.$emit('input', {from: this.range[0], to: this.range[1]});
          }
          else {
            this.$emit('input', null);
          }
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedRange = _.cloneDeep(this.range);
            this.range = [0, 10];
          }
          else if(this.clonedRange.length > 0) {
            this.range = _.cloneDeep(this.clonedRange);
            this.clonedRange = [];
          }
        }
      }
    }
};
</script>

<style lang="scss">
.side-filter {
  &-rating {
    .v-slider__thumb:after {
        content: "✭";
        display: block;
        width: 16px;
        margin-left: auto;
        margin-right: auto;
        left: 8px;
        font-size: 18px;
        background: white;
        cursor: move; /* fallback if grab cursor is unsupported */
        cursor: grab;
        cursor: -moz-grab;
        cursor: -webkit-grab;
        top: 13px;
    }
  }
}
</style>
