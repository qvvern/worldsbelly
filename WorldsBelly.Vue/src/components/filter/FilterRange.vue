<template>
    <div class="filter-range" ref="filterRange">
      <p v-if="label">{{label}}</p>
          <v-range-slider
            :value="value"
            @input="$emit('input', $event)"
            :max="max"
            :min="min"
            hide-details
            class="align-center"
            :color="lineColor"
            :thumb-color="thumbColor ? thumbColor : lineColor"
            :track-color="rightLineColor"
          >
          </v-range-slider>
    </div>
</template>

<script>
import _ from "lodash";

export default {
    name: "FilterRange",
    props: {
        value: Array,
        min: Number,
        max: Number,
        label: String,
        lineColor: {
          type: [String],
          default: '#f18055'
        },
        thumbColor:  {
          type: [String],
          default: '#f18055'
        },
        rightLineColor: {
          type: [String],
          default: '#ccc'
        },
    },
    data() {
        return {
          range: [this.min, this.max],
        };
    },
    methods: {
    },
    mounted() {
    },
    watch: {
      range: {
        handler() {
          this.$emit('input', this.range);
        }, deep: true
      }
    }
};
</script>

<style lang="scss" >
.filter-range {
  // box-shadow: 0px 0px 2px 0px rgba(0, 0, 0, 0.2) !important;
  width: 100%;
  p {
    font-size: 12px;
  }
  .v-slider {
    min-height: auto !important;
    height: 15px !important;
  }
  .v-slider__thumb-label {
    background: transparent !important;
    color: #000
  }
  .settings-slider__label {
    display: flex;
    align-items: center;
    width: 100px;
    justify-content: center;
  }
  .v-slider__track-container {
    height: 2px !important;
    * {
      border-radius: 10px !important;
    }
  }
  .v-slider__thumb:before {
    display: none;
  }
}
</style>
