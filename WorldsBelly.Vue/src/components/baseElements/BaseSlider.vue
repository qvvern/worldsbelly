<template>
      <v-slider
        :value="value"
         @input="$emit('input', $event)"
        :min="min"
        :max="max"
        :label="label"
        :thumb-label="labels && labelsFollow ? 'always' : false"
        ticks
        class="settings-slider"
        :tick-labels="labelsFollow ? [] : labels"
        :color="lineColor"
        :thumb-color="thumbColor ? thumbColor : lineColor"
        :track-color="rightLineColor"
        :class="{'pointer-events-none': disabled}"
      >
        <div slot="label" :style="styleLabelObj" class="base-slider-label">
           {{label}}
        </div>
          <template v-if="labelsFollow && labels && labels.length == max" v-slot:thumb-label="{ value }">
            <div class="settings-slider__label" :style="styleLabelItemObj">{{ labels[value-1] }}</div>
          </template>
      </v-slider>
</template>

<script>
  export default {
      name: "BaseSlider",
      props: {
        value: [String, Number],
        label: [String],
        labels: {
          type: [Array],
        },
        max: {
          type: [Number, String],
          default: 5
        },
        min: {
          type: [Number, String],
          default: 0
        },
        labelWidth: {
          type: [String],
          default: 'auto'
        },
        labelAlign: {
          type: [String],
          default: 'left'
        },
        labelsFollow: {
          type: [Boolean],
          default: false
        },
        lineColor: {
          type: [String],
          default: '#e1b514'
        },
        thumbColor:  {
          type: [String],
          default: '#fff'
        },
        rightLineColor: {
          type: [String],
          default: '#c6c6c6'
        },
        fontSize: {
          type: [String],
          default: '12px'
        },
        fontColor: {
          type: [String],
          default: '#00133e'
        },
        disabled: Boolean
      },
      computed: {
        styleLabelObj() {
          return {
            width: this.labelWidth,
            'text-align': this.labelAlign,
            'font-size': this.fontSize,
            'color': this.fontColor,
          }
        },
        styleLabelItemObj() {
          return {
            marginRight: this.value == this.max ? '50px' : 0,
          }
        }
      }
  };
</script>
<style lang="scss">
.settings-slider {
  .base-slider-label {
      font-family: 'Signika', sans-serif;
      font-weight: 500;
  }
  .v-slider__thumb {
    border: 1px solid #7f8a94 !important;
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
    height: 6px !important;
    * {
      border-radius: 10px !important;
    }
  }
  .v-slider__thumb:before {
    display: none;
  }
}
</style>
