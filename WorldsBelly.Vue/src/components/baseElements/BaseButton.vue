<template>
    <v-btn
      :style="{ 'height': round ? '' : height, width: round ? '' : 'auto', padding: round ? '' : '0 5px', 'min-width': round ? '' : 'auto'}"
      class="base-button"
      :loading="loading"
      :disabled="disabled"
      :class="classObject"
      :outlined="outlined"
      :color="light ? 'white' : ''"
      :fab="round"
      :depressed="noshadow"
      :x-small="xsmall"
      :small="small"
      :large="large"
      :x-large="xlarge"
    >
    <div class="flex justify-center items-center" :class="{'flex-col': iconTop, 'p-2': !round}">
      <v-icon v-if="icon" :size="size" :class="{'pb-2': iconTop, 'pr-2': !iconTop && !round}">
        {{ icon }}
      </v-icon>
      <p v-if="!value" :style="'font-size: '+ size+'px'"><slot></slot></p>
      <template v-else></template>
    </div>
    </v-btn>
</template>

<script>
  export default {
      name: "baseButton",
      props: {
        value: [String, Number],
        icon: [String],
        loading: [Boolean],
        disabled: [Boolean],
        outlined: [Boolean],
        light: [Boolean],
        primary: [Boolean],
        secondary: [Boolean],
        tertiary: [Boolean],
        delete: [Boolean],
        floaty: [Boolean],
        round: [Boolean],
        noshadow: [Boolean],
        xsmall: [Boolean],
        height: {
          type: [String],
          default: "auto"
        },
        small: [Boolean],
        size: [String, Number],
        large: [Boolean],
        xlarge: [Boolean],
        iconTop: [Boolean],
        dashed: [Boolean],
        thick: [Boolean],
        green: [Boolean],
        black: [Boolean],
        orange: [Boolean],
        red: [Boolean],
        outred: [Boolean],
        blue: [Boolean],
      },
    data() {
      return {
      }
    },
    computed: {
        classObject() {
          return {
            thick: this.thick,
            primary: this.primary,
            secondary: this.secondary,
            tertiary: this.tertiary,
            delete: this.delete,
            dashed: this.dashed,
            floaty: this.floaty,
            bgreen: this.green,
            bblack: this.black,
            borange: this.orange,
            bred: this.red,
            bblue: this.blue,
            outred: this.outred,
          }
        },
    }
  };
</script>
<style lang="scss" scoped>
.base-button {
    font-family: 'raleway';
    font-weight: 400;
    text-transform: none;
  &.primary {
    font-size: 12px;
    border-radius: 1rem;
    color: #ffffff;
    background: #1e1f1e;
  }
  &.secondary {
    font-size: 12px;
    border-radius: 1rem;
    color: #1e1f1e;
    background: transparent;
  }
  &.tertiary {
    color: #000 ;
    background: #f1f1f1;
    box-shadow: none ;
    border: 1px solid #d4d4d4;
    // min-height: 2.5rem;
    padding: 0 15px !important;
    &:active,
    &:hover,
    &:focus {
      outline: 0;
    }


    &:hover {
      transition: all 300ms !important;
    }

    &:active:hover,
    &:focus:hover,
    &:focus {
      box-shadow: rgba(0, 0, 0, 0.12) 0 1px 3px 0 inset !important;
      color: #FFFFFF !important;
    }
  }
  &.thick {
      font-size: 12px !important;
      background-color: #111111 !important;
      border-radius: 2rem;
      color: #fff !important;
      min-height: 2.5rem;
      padding: 0 15px !important;
  }
}
</style>

<style lang="scss">
.base-button {

/* CSS */
&.floaty {
  align-items: center;
  appearance: none;
  background-color: #fff !important;
  border-radius: 24px !important;
  border-style: none;
  box-shadow: rgba(0, 0, 0, .2) 0 3px 5px -1px,rgba(0, 0, 0, .14) 0 6px 10px 0,rgba(0, 0, 0, .12) 0 1px 18px 0 !important;
  box-sizing: border-box !important;
  color: #3c4043 !important;
  cursor: pointer;
  display: inline-flex;
  fill: currentcolor;
  font-family: "Google Sans",Roboto,Arial,sans-serif !important;
  // font-size: 14px !important;
  font-weight: 500 !important;
  // height: 48px !important;
  justify-content: center !important;
  letter-spacing: .25px !important;
  line-height: normal;
  max-width: 100%;
  overflow: visible;
  padding: 4px 20px!important;
  position: relative;
  text-align: center;
  text-transform: none;
  transition: box-shadow 280ms cubic-bezier(.4, 0, .2, 1),opacity 15ms linear 30ms,transform 270ms cubic-bezier(0, 0, .2, 1) 0ms;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  will-change: transform,opacity;
  z-index: 0;
  .v-icon {
    font-size: 16px !important;
    margin-left: -10px !important;
  }
}

&.floaty:hover {
  background: #F6F9FE !important;
  color: #174ea6 !important;
}

&.floaty:active {
  box-shadow: 0 4px 4px 0 rgb(60 64 67 / 30%), 0 8px 12px 6px rgb(60 64 67 / 15%);
  outline: none;
}

&.floaty:focus {
  outline: none;
}

&.floaty:not(:disabled) {
  box-shadow: rgba(60, 64, 67, .3) 0 1px 3px 0, rgba(60, 64, 67, .15) 0 4px 8px 3px;
}

&.floaty:not(:disabled):hover {
  box-shadow: rgba(60, 64, 67, .3) 0 2px 3px 0, rgba(60, 64, 67, .15) 0 6px 10px 4px;
}

&.floaty:not(:disabled):focus {
  box-shadow: rgba(60, 64, 67, .3) 0 1px 3px 0, rgba(60, 64, 67, .15) 0 4px 8px 3px;
}

&.floaty:not(:disabled):active {
  box-shadow: rgba(60, 64, 67, .3) 0 4px 4px 0, rgba(60, 64, 67, .15) 0 8px 12px 6px;
}

&.floaty:disabled {
  box-shadow: rgba(60, 64, 67, .3) 0 1px 3px 0, rgba(60, 64, 67, .15) 0 4px 8px 3px;
}

/* CSS */
&.delete {
  // align-items: center;
  // background-color: #FFE7E7 !important;
  // background-position: 0 0;
  // border: 1px solid #FEE0E0 !important;
  // box-sizing: border-box;
  // color: #D33A2C !important;
  // cursor: pointer;
  // display: flex;
  // font-weight: 700 !important;
}

&.delete:active,
&.delete:hover,
&.delete:focus {
  outline: 0;
}


&.delete:active {
  background-color: #D33A2C !important;
  box-shadow: rgba(0, 0, 0, 0.12) 0 1px 3px 0 inset !important;
  color: #FFFFFF !important;
}

&.delete:hover {
  color: #D33A2C !important;
  background-color: #FFE3E3 !important;
  border-color: #FAA4A4 !important;
}

&.delete:active:hover,
&.delete:focus:hover,
&.delete:focus {
  box-shadow: rgba(0, 0, 0, 0.12) 0 1px 3px 0 inset !important;
  color: #FFFFFF !important;
}

/* CSS */
&.dashed {
  appearance: none;
  background-color: #FAFBFC !important;
  border: 1px dashed rgba(27, 31, 35, 0.15);
  border-radius: 16px !important;
  box-shadow: rgba(27, 31, 35, 0.04) 0 1px 0, rgba(255, 255, 255, 0.25) 0 1px 0 inset;
  box-sizing: border-box;
  color: #24292E;
  cursor: pointer;
  display: inline-block;
  font-family: -apple-system, system-ui, "Segoe UI", Helvetica, Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji" !important;
  font-size: 14px;
  font-weight: 500;
  line-height: 20px;
  list-style: none;
  position: relative;
  transition: background-color 0.2s cubic-bezier(0.3, 0, 0.5, 1);
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  vertical-align: middle;
  white-space: nowrap;
  word-wrap: break-word;
}

&.dashed:hover {
  background-color: #F3F4F6 !important;
  text-decoration: none;
  transition-duration: 0.1s;
}

&.dashed:disabled {
  background-color: #FAFBFC !important;
  border-color: rgba(27, 31, 35, 0.15);
  color: #959DA5;
  cursor: default;
}

&.dashed:active {
  background-color: #EDEFF2 !important;
  box-shadow: rgba(225, 228, 232, 0.2) 0 1px 0 inset !important;
  transition: none 0s;
}

&.dashed:focus {
  outline: 1px transparent;
}

&.dashed:before {
  display: none;
}

&.dashed:-webkit-details-marker {
  display: none;
}

&.bgreen {
  background: #0dad26 !important;
  color: white !important;
  &:hover {
    background: #16c25e !important;
    color: white !important;
  }
}
&.bblack {
  background: #1e1f1e !important;
  color: white !important;
  &:hover {
    background: #282928 !important;
    color: white !important;
  }
}
&.borange {
  background: #ad6b0d !important;
  color: white !important;
  &:hover {
    color: white !important;
    background: #f1a90d !important;
  }
}
&.bblue {
  background: #0d91ad !important;
  color: white !important;
  &:hover {
  background: #10a1c2 !important;
    color: white !important;
  }
}
&.bred {
  background: #ad130d !important;
  color: white !important;
  &:hover {
    color: white !important;
  background: #c01610 !important;
  }
}

&.outred {
  border-radius: 30px;
  color: white !important;
  background: rgba(252, 6, 6, .1) !important;
  border: 2px solid #c01610 !important;
  .v-icon {
    color: #c01610 !important;
  }
  &:hover {
    color: white !important;
    background: #c01610 !important;
    .v-icon {
      color: black !important;
    }
  }
}

}
</style>
