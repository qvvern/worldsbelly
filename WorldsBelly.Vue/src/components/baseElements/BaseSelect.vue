<template>
    <v-autocomplete
      ref="el"
      :data-id="id"
      class="base-select"
      :value="inputValue"
      :label="label"
      :items="sortedItems || []"
      @input="handleInput($event)"
      :loading="loading"
      :search-input.sync="search"
      @keydown="reset()"
      @blur="handleBlur()"
      :color="color"
      :item-text="itemText"
      :item-value="itemValue"
      :placeholder="placeholder"
      :append-icon="input ? '' : '$dropdown'"
      hide-no-data
      :disabled="disabled"
      :spellcheck="false"
      :class="classObject"
      :style="styleObject"
      :outlined="outlined"
      :dense="small"
      :solo="solo"
      :flat="flat"
      :autofocus="autofocus"
      return-object
      :filter="filterObject"
      :no-filter="noFilter"
      :dark="dark"
      :clearable="clearable"
      :single-line="singleLine"
      :multiple="multiple"
      :chips="chips"
    >
    <template v-slot:selection="data" v-if="costumizedSelection">
        <slot name="selection" :item="data.item"></slot>
    </template>
    <template v-slot:item="data" v-if="costumized">
        <slot name="item" :item="data.item"></slot>
    </template>
  <template v-slot:append-item>
    <div v-intersect="endIntersect" />
  </template>
    </v-autocomplete>
</template>

<script>
import debounce from "lodash/debounce";
import _ from "lodash";

    export default {
        name: "baseSelect",
        props: {
          value: [String, Number, Object],
          label: String,
          placeholder: String,
          color: {
              type: String,
              default: 'teal',
          },
          items: Array,
          excludeItems: Array,
          loading: Boolean,
          itemText: {
              type: String,
              default: 'name',
          },
          itemText2: {
              type: String,
              default: 'namePlural',
          },
          itemValue: {
              type: String,
              default: 'id',
          },
          input: Boolean,
          disabled: Boolean,
          radius: {
              type: String,
              default: '0px',
          },
          outlined: Boolean,
          small: Boolean,
          solo: Boolean,
          flat: Boolean,
          shadow: Boolean,
          autofocus: Boolean,
          returnAll: {
              type: Boolean,
              default: false,
          },
          moreLimit: Number,
          costumized: Boolean,
          costumizedSelection: Boolean,
          multifilter: Boolean,
          defaultItem: Object,
          dark: Boolean,
          clearable: Boolean,
          multiple: Boolean,
          chips: Boolean,
          noFilter: {
            type: Boolean,
            default: true
          },
          autoWith: [Number, Boolean],
          clearOnSearch: [Boolean],
          minWidth: {
            type: [String],
            default: 'auto'
          },
          maxWidth: {
            type: [String],
            default: '100%'
          },
          fallbackItem: Object,
          height: {
            type: [String],
            default: '40px'
          },
          singleLine: Boolean,
          right: Boolean,
          labelTop: Boolean,
        },
      data() {
        return {
          id: null,
          search: null,
          inputValue: null,
          activeItem: null,
          moreCounter: 1,
        }
      },
      computed: {
          classObject() {
            return {
              disabled: this.disabled,
              'align-right': this.right,
              'label-top': this.labelTop,
            }
          },
          styleObject() {
            return {
              'border-radius': this.radius,
              'height': this.small ? '40px' : this.height,
              'overflow': 'hidden',
              'box-shadow': this.shadow ? '0px 1px 5px 0px #adbec8 !important' : '',
              'min-width': this.minWidth,
              'max-width': this.maxWidth
            }
          },
          sortedItems() {
            var items = [];
            if(this.items) items = [...this.items];
            if(this.excludeItems && this.excludeItems.length > 0) items = items.filter(_ => !this.excludeItems.some(x => x[this.itemValue] == _[this.itemValue]));
            if(this.activeItem) items.push(this.activeItem);
            if(this.fallbackItem) items.push(this.fallbackItem);
            return items.filter(n => n);
          }
      },
      mounted() {
        this.id = this._uid;
        if(this.defaultItem) this.activeItem = this.defaultItem;
        this.$nextTick(() => {
          var foundItem = this.sortedItems.find(_ => _[this.itemValue] == this.inputValue);
          var el = document.querySelector(`input[data-id="${this.id}"]`);
          if(el) {
              if(this.autoWith && foundItem) {
                  el.style.width = 0;
                  el.style.minWidth = 0;
                  var closestParent = el.parentNode.closest('.base-select');
                  closestParent.style.width = ((foundItem[this.itemText]?.length + (isNaN(this.autoWith) ? 1 : this.autoWith)) * 8) + 'px';
              }
              if(this.labelTop && this.placeholder) {
                setTimeout(() => {
                  el.placeholder = this.placeholder;
                }, 50);
              }
          }
        })
      },
      methods: {
        handleBlur() {
          if(!this.value && this.fallbackItem) {
            if(!this.returnAll) {
              this.$emit('input', this.fallbackItem[this.itemValue] || 0);
            }
            else {
              this.$emit('input', this.fallbackItem);
            }
          }
        },
        reset() {
            if(this.clearOnSearch) this.$emit('input', null);
        },
        filterObject(item, queryText) {
          if(!this.multifilter) {
            return item[this.itemText].toLocaleLowerCase().indexOf(queryText.toLocaleLowerCase()) > -1;
          }
          return (
            item[this.itemText].toLocaleLowerCase().indexOf(queryText.toLocaleLowerCase()) >
              -1 ||
            item[this.itemText2].toLocaleLowerCase().indexOf(queryText.toLocaleLowerCase()) > -1
          )
        },
        handleInput(e) {
          if(!e) {
            this.$emit('input', e); return;
          }
          if(this.autoWith) {
            this.inputValue = null;
            var el = document.querySelector(`input[data-id="${this.id}"]`);
            if(el) {
              el.style.width = 0;
              el.style.minWidth = 0;
              var closestParent = el.parentNode.closest('.base-select');
              closestParent.style.width = ((e[this.itemText]?.length + (isNaN(this.autoWith) ? 1 : this.autoWith)) * 8) + 'px';
            }
          }
          this.activeItem = _.cloneDeep(e);
          if(!this.returnAll) {
            this.$emit('input', e[this.itemValue] || e);
          }
          else {
            this.$emit('input', e);
          }
          this.$emit('selectedItem', e);
        },
        endIntersect(entries, observer, isIntersecting) {
          if (isIntersecting && this.items.length >= this.moreLimit * this.moreCounter) {
            this.moreCounter += 1;
            this.$emit('searchMore', this.search)
          }
        }
      },
      watch: {
        value: {
          handler() {
            if(this.inputValue == this.value) return;
              this.inputValue = _.cloneDeep(this.value);
          }, immediate: true, deep: true
        },
        search: debounce(function(val) {
          // Items have already been loaded
          // if (this.items.length > 0) return
          this.moreCounter = 1;

          // Items have already been requested
          if (this.loading) return

          // Lazily load input items
          this.$emit('search', val);

          //scroll top
          document.querySelector('.v-autocomplete__content')?.scrollTo({top: 0});

        }, 300),
        sortedItems: {
          handler() {
             this.$refs.el.updateMenuDimensions()
          }
        }
      },
    };
</script>

<style lang="scss" scoped>
.base-select {
  &.disabled {
    opacity: .75 !important;
  }
}
</style>

<style lang="scss">
.base-select {
    &.label-top {
      label {
        transform: translateY(-24px) scale(0.75);
        max-width: 133%;
      }
    }
    &.disabled {
      .v-input__slot, .v-input__control {
        background: #e9eaeb !important;
      }
    }
    .v-input__control fieldset {
      border: 0 !important;
    }
    .v-input__append-inner {
      padding-left: 0px;
      right: 0px;
      position: absolute;
      .v-input__icon {
        min-width: 14px;
        width: 14px;
      }
    }
    &.align-right {
      label {
        left: auto !important;
        right: 0px !important;
        padding-right: 28px;
      }
      .v-label {
        transform-origin: top right !important;
      }
      input {
        padding-right: 24px;
        text-align: right !important;
      }
    }
}
</style>
