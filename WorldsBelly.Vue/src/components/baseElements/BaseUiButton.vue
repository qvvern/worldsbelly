<template>
  <button
    class="wb-button"
    :class="{[type]: type, outlined: outlined}"
    :style="{background: background}"
    v-bind="$attrs"
    v-on="inputListeners"
    :disabled="disabled"
  >
    <div class="disabled-overlay" v-if="disabled"></div>
    <v-progress-circular :width="1" class="wb-button__loader" :size="size / 1.2" v-if="loading" indeterminate :color="color"></v-progress-circular>
    <span class="wb-button__content" :class="{ loading }" :style="{'font-size': size+'px', color: color}">
      <v-icon v-if="icon" :size="size * 1.2" :color="color"> {{ icon }} </v-icon>
      <template v-if="label">{{ label }}</template>
      <template v-else> <slot></slot> </template>
    </span>
  </button>
</template>

<script>

export default {
  name: "BaseUiButton",
  components: {
  },
  props: {
    label: String,
    type: {
      type: String,
    },
    icon: String,
    loading: {
      type: Boolean,
      default: false,
    },
    outlined: {
      type: Boolean,
      default: false,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    size: {
      type: Number,
      default: 16,
    },
    background: String,
    color: String,
  },
  computed: {
    inputListeners() {
      if (!this.disabled) {
        return Object.assign({}, this.$listeners);
      }
      return false;
    },
  },
};
</script>
<style lang="scss" scoped>
.wb-button:focus {
  outline: none;
}
.wb-button:not(:disabled) {
  cursor: pointer;
}
[type="button"]::-moz-focus-inner,
button::-moz-focus-inner {
  padding: 0;
  border-style: none;
}
.wb-button {
  overflow: hidden;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  position: relative;
  user-select: none;
  border-radius: 0;
  // overflow: visible;
  text-transform: none;
  word-wrap: normal;
  -webkit-appearance: button;
  background-color: #fff;
  padding: 8px 12px;
  &:active:not(:disabled) {
    transform: translateY(1px);
  }
  &:disabled {
    cursor: default;
    // cursor: not-allowed;
    .wb-button__content {
      opacity: .6 !important;
    }
  }
  .disabled-overlay {
    background: rgba(133, 133, 133, 0.4);
    z-index: 9;
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
  }
  &__loader {
    position: absolute;
  }
  &__content {
    border: none;
    color: #000;
    font-family: 'Roboto', Helvetica, sans-serif;
    cursor: pointer;
    display: inline-flex;
    align-items: center;
    &.loading {
      visibility: hidden;
    }
  }
  &:hover {
    transition: all 150ms !important;
  }
}

.outlined.wb-button {
  background-color: transparent !important;
  .wb-button__content {
  }
  &:hover {
    background-color: transparent !important;
  }
}

.btn1.wb-button {
    background: #080808;
    border-radius: 15px;
  .wb-button__content, .v-icon, .wb-button__loader {
    color: #ffffff;
  }
  &:hover:not(:disabled) {
    background: #1e1f1e;
  }
}

.btn2.wb-button {
  background-color: #f5f5f5;
  border-radius: 8px;
  border-width: 0;
  color: #333333;
  cursor: pointer;
  font-family: "Haas Grot Text R Web", "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-size: 14px;
  font-weight: 500;
  line-height: 20px;
  list-style: none;
  // padding: 10px 12px;
  border: 1px solid #e6e6e6;
  text-align: center;
  .wb-button__content, .v-icon, .wb-button__loader {
    // color: #ffffff;
  }
  &:hover:not(:disabled) {
    // background: #1e1f1e;
  }
}
.btn3.wb-button {
  background-color: transparent;
  border-radius: 8px;
  border-width: 0;
  color: #333333;
  cursor: pointer;
  font-family: "Haas Grot Text R Web", "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-size: 14px;
  font-weight: 400;
  line-height: 14px;
  list-style: none;
  // padding: 10px 12px;
  border: 1px solid #e6e6e6;
  text-align: center;
  &:hover {
    background-color: rgba(0,0,0,.1);
  }
  .wb-button__content, .v-icon, .wb-button__loader {
    // color: #ffffff;
  }
  &:hover:not(:disabled) {
    // background: #1e1f1e;
  }
}
.btn4.wb-button {
  color: #fff;
  background-color: #1f1c24;
  padding: 0 5px;
  border: 0;
  border-radius: 10px;
  // width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  .v-icon {
    color: #fff;
    padding-right: 8px;
  }
  span {
    color: #fff;
    padding-left: 8px;
    padding-right: 8px;
    font-size: 12px;
    font-family: 'DM Sans', sans-serif, "Haas Grot Text R Web", "Helvetica Neue", Helvetica, Arial, sans-serif;
  }
  .wb-button__content, .v-icon, .wb-button__loader {
    color: #ffffff;
  }
  &:hover:not(:disabled) {
    background-color: #28262c;
  }
}
</style>
