<template>
    <div class="filter-input-dropdown" ref="filterInputDropdown" v-click-outside="close">
      <div class="search-label" v-if="label">
          <v-chip
              small
              close
              @click:close="$emit('remove', label)"
              >{{ label }}</v-chip
          >
      </div>
        <div class="search-input" ref="selectInput">
            <v-text-field
              :value="value"
              @input="$emit('input', $event)"
              :label="options.find(_ => _.id == selectedOption).label"
              :placeholder="placeholder"
              hide-details
              dense
              persistent-placeholder
              filled
              :type="type"
            >
            <template v-slot:append>
              <div class="flex items-end justify-bottom">
                <slot name="append"></slot>
              </div>
            </template>
            </v-text-field>
            <v-menu
            bottom
            left
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
              class="search-input-btn"
                icon
                v-bind="attrs"
                v-on="on"
              >
                <baseIcon @click.prevent.native="active = !active" :value="active ? 'icofont-caret-up' : 'icofont-caret-down'" />
              </v-btn>
            </template>

            <v-list>
              <v-list-item
                v-for="(item, i) in sortedItems"
                :key="i"
                @click="$emit('selected', item)"
              >
                <v-list-item-title >{{ item.label }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>

        </div>
    </div>
</template>

<script>
import _ from "lodash";

export default {
    name: "FilterInputDropdown",
    props: {
        value: [Number, String],
        label: String,
        placeholder: String,
        options: Array,
        selectedOption: [Object, Number, String],
        type: String
    },
    data() {
        return {
            searchValue: null,
            active: false,
            inputWidth: null
        };
    },
    computed: {
      sortedItems() {
        return this.options.filter(({id}) => id != this.selectedOption)
      }
    },
    methods: {
      close() {
            this.active = false;
            this.$emit('close');
      },
    },
    mounted() {
      this.$nextTick(() => {
        this.inputWidth = this.$refs.filterInputDropdown.offsetWidth + 'px';
      })
    },
    watch: {
        searchValue: {
            handler() {
                this.search(this.searchValue);
            },
            deep: true,
        },
    },
};
</script>

<style lang="scss">
.filter-input-dropdown {
    justify-content: flex-start;
    display: flex;
    align-items: center;
    flex-wrap: wrap;
    background: #f0f0f0;
    border-bottom: 1px solid #000;
    margin-bottom: 5px;
    border-radius: 5px;
    overflow: hidden;
    .v-input__slot {
      padding: 0px 5px !important;
      background: transparent !important;
    }
    .search-label {
      padding: 2px;
    }
    .search-input {
      flex-grow: 1;
      width: 80px;
      position: relative;
        background: transparent;
        input {
            width: 100%;
            outline: none;
            &::placeholder {
                color: #7b7b7b;
                font-size: 14px;
            }
        }
        &-btn {
          position: absolute;
          right: -2px;
          top: -3px;
          .v-icon {
            color: #15132b;
          }
        }
    }
    .search-select-list {
            padding: 0;
            margin: 0;
            position: relative;
            background: #fff;
            z-index: 999999;
            max-height: 200px;
            overflow-x: hidden;
            overflow-y: auto;
            box-shadow: inset 0 2px 12px -4px #c5d1d9;
            position: absolute;
            border-bottom-left-radius: 15px;
                list-style-type: none;
            li {
                padding: 5px 15px;
                font-size: 14px;
                cursor: pointer;
                &:first-child {
                    padding-top: 10px;
                }
                &:hover {
                    background: rgb(228, 228, 228);
                }
            }
    }
}
</style>
