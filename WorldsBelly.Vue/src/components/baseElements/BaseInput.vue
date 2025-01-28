<template>
    <v-text-field
      class="base-input"
      :placeholder="placeholder"
      :clearable="clearable"
      :prepend-inner-icon="innerIcon"
      :value="value"
      @input="$emit('input', $event)"
      :disabled="disabled"
      :class="classObject"
      :style="styleObject"
      :outlined="outlined"
      :dense="small"
      :solo="solo"
      :flat="flat"
      :type="type"
      :color="color"
      :label="label"
      :hint="hint"
      :error-messages="error"
      @keyup.enter="$emit('enter', $event)"
    >
     <template v-slot:prepend-inner>
        <slot name="prepend"></slot>
     </template>
     <template v-slot:append-inner>
        <slot name="append"></slot>
     </template>
    </v-text-field>
</template>

<script>
  export default {
      name: "baseInput",
      props: {
        hint: String,
        label: String,
        value: [String, Number],
        error: [String, Number],
        placeholder: String,
        color: {
            type: String,
            default: 'teal',
        },
        disabled: Boolean,
        radius: {
            type: String,
            default: '0px',
        },
        innerIcon: {
            type: String,
            default: '',
        },
        type: {
            type: String,
            default: 'text',
        },
        width: {
            type: String,
            default: 'auto',
        },
        outlined: Boolean,
        small: Boolean,
        solo: Boolean,
        flat: Boolean,
        clearable: Boolean,
        shadow: Boolean,
        right: Boolean,
        borderless: Boolean
      },
    data() {
      return {
      }
    },
    computed: {
        classObject() {
          return {
            disabled: this.disabled,
            right: this.right,
            borderless: this.borderless
          }
        },
        styleObject() {
          return {
            'border-radius': this.radius,
            'max-width': this.width,
            'padding-top': this.small && this.label ? '4px' : 'auto',
            'height': this.small ? this.label ? '44px' : '40px' : 'auto',
            'overflow': 'hidden',
            'box-shadow': this.shadow ? '0px 1px 5px 0px #adbec8 !important' : ''
          }
        },
    }
  };
</script>
<style lang="scss" scoped>
.base-input {
  &.disabled {
      box-shadow: 0 !important;
    opacity: .9 !important;
  }

}
</style>
<style lang="scss">
.base-input {
    &.disabled {
      .v-input__slot, .v-input__control {
        background: #e9eaeb !important;
      }
    }
    &.right {
      [type="number"] {
        text-align: right;
        direction: rtl;
      }
      .v-text-field__slot {
        label {
          // left: auto !important;
          // right: 0 !important;
          transition: all 400ms;
          padding-right: 5px !important;
        }
        // .v-label--active {
        //   transition: all 400ms;
        //   right: -35px !important
        // }
        input {
          text-align: right;
          padding-right: 5px !important;
        }
      }
    }
    &.borderless {
        .v-label--active {
          left: auto !important;
          right: -35px !important
        }
      fieldset {
          // box-shadow: inset 0 -1px 2px rgba(0,0,0,.1);
          border: 0px !important;
        }
    }
    // border-radius: 20px !important;
    .v-input__icon--prepend-inner .v-icon {
      padding-right: 5px;
      font-size: 18px !important;
      color: var(--highlight) !important;
    }
    .v-text-field--outlined .v-text-field__slot {
      padding-top: 5px !important;
      font-size: 14px;
    }
  fieldset {
      // box-shadow: inset 0 -1px 2px rgba(0,0,0,.1);
      border: 1px solid #ccc !important;
    }
}
</style>
