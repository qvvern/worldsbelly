<template>
    <div class="base-quanity-input flex" :style="outerstylings" :class="{focused: pickerVisible}">
      <baseIcon :size="14" @click.prevent.native="remove()" :class="{disabled: min && min >= input }">icofont-minus</baseIcon>
      <label v-if="label" :style="{right: labelRight ? '5px' : '', left: !labelRight ? '5px' : ''}" >{{label}}</label>
      <input :placeholder="placeholder" v-model="input" type="number" :style="stylings"  @focus="pickerVisible = true" @blur="pickerVisible = false">
      <baseIcon :size="14" @click.prevent.native="add()" :class="{disabled: max && max <= input }">icofont-plus</baseIcon>
    </div>
</template>

<script>
import _ from "lodash";
  export default {
      name: "baseQuantityInput",
      props: {
        label: String,
        value: [String, Number],
        placeholder: [String, Number],
        width: [String, Number],
        left: [Boolean],
        right: [Boolean],
        labelRight: [Boolean],
        center: {
          type: [Boolean],
          default: true
        },
        height: {
          type: [String, Number],
          default: '30px'
        },
        min: [Number],
        max: [Number],
      incrementor: {
        type: Number,
        default: 1,
      },
      },
    data() {
      return {
        input: 0,
        pickerVisible: false
      }
    },
    computed: {
      outerstylings() {
        return {
          'max-width': this.width,
          'min-width': this.width
        }
      },
      stylings() {
        return {
          'height': this.height,
          'text-align': this.left ? 'left' : this.right ? 'right' : this.center ? 'center' : '',
        }
      }
    },
    mounted() {
    },
    methods: {
		updateInput: _.debounce(function () {
			if ((this.min == null || this.min <= this.input) && (this.max == null || this.max >= this.input)) {
				this.$emit('input', Number(this.input));
			} else {
				this.input = Number(this.min ?? this.max);
			}
		}, 100),
		add() {
			const newVal = Number(this.input) + this.incrementor;
			if (this.max == null || this.max >= newVal) {
				this.input = Number(newVal);
			}
		},
		remove() {
			const newVal = Number(this.input) - this.incrementor;
			if (this.min == null || this.min <= newVal) {
				this.input = Number(newVal);
			}
		},
    },
    watch: {
      value: {
        handler() {
          if(!this.value && this.value != 0) {
            this.input = null;
          }
          if(Number(this.value) != Number(this.input)) {
            this.input = this.value
          }
        }, immediate: true
      },
      input: {
        handler() {
            this.updateInput();
        }
      }
    }
  };
</script>
<style lang="scss">
.base-quanity-input {
    box-shadow: inset 0 2px 12px -4px #c5d1d9;
    border-radius: 15px;
    position: relative;
    label {
      display: block;
      position: absolute;
      top: -20px;
      z-index: 1;
      color: #000;
      font-size: 0.7em
    }
  input {
    width: 100%;
    height: 100%;
    padding: 0 .5rem;
    padding-top: 2px;
    font-size: 16px;
    border: 0;
    background: transparent;
    color: #5a5a5a;
    text-align: center;
    box-sizing: border-box;
      &::placeholder {
        font-size: 0.7em
      }
  }
  input:focus {
    outline: none;
    // box-shadow: 0 5px 55px -10px rgba(0, 0, 0, 0.2), 0 0 4px #3fb0ff; /* Allows border radius on focus */
  }
  .v-icon  {
    padding: 10px;
    margin: 0;
    border: 0;
    border-radius: 1.4rem;
    cursor: pointer;
    transition: opacity 0.15s;
    opacity: 0.5;
    color: #000000;
    outline: none;
    &.disabled {
      pointer-events: none;
      color: #696969 !important;
    }
  }
  .v-icon:hover  {
    background-position-y: 4px;
    box-shadow: inset 0 2px 12px -4px #c5d1d9;
    opacity: 1;
  }


    /* Chrome, Safari, Edge, Opera */
  input::-webkit-outer-spin-button,
  input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }

  /* Firefox */
  input[type=number] {
    -moz-appearance: textfield;
  }
}
</style>
