<template>
    <div class="filter-select" ref="filterSelect" v-click-outside="close">
        <div class="search-input">
            <input ref="selectInput" type="text" v-model="searchValue" @focus="active = true" :placeholder="placeholder" />
        </div>
            <ul class="search-select-list scrollbar-one" ref="selectList" v-if="(active || (active && loading)) && (searchValue || show)" :style="{'max-height': maxHeight}">
                <li v-for="(item, i) in sortedItems" :key="i" @click="$emit('selected', item), $refs.selectInput.focus()">
                    {{ getOptionText(item) }}
                </li>
                <li v-if="loading" class="flex justify-center items-center">
                    <v-progress-circular indeterminate :size="16"></v-progress-circular>
                </li>
                <li v-else-if="!loading && localItems.length == 0" class="flex justify-center items-center">
                    {{ $t('noResults') }}
                </li>
                <li v-else-if="!loading && sortedItems.length == 0" class="flex justify-center items-center">
                    {{ $t('noMoreResults') }}
                </li>
                  <div v-if="intersect" v-intersect="endIntersect" />
            </ul>
    </div>
</template>

<script>
import _ from "lodash";

export default {
    name: "FilterSelect",
    props: {
        value: String,
        items: Array,
        excludeItems: Array,
        itemText: String,
        placeholder: String,
        multiple: Boolean,
        show: Boolean,
        moreLimit: {
          type: Number,
          default: 50
        },
        intersect: {
          type: Boolean,
          default: true
        },
        itemValue: {
          type: String,
          default: "id"
        },
        maxHeight: {
          type: String,
          default: "200px"
        }
    },
    data() {
        return {
            searchValue: null,
            active: false,
            isIntersecting: true,
            localItems: [],
            noMore: false,
            loading: true,
        };
    },
    computed: {
      sortedItems() {
        return this.localItems.filter((_) => !this.excludeItems.some(x => x[this.itemValue] == _[this.itemValue]))
      }
    },
    methods: {
      close() {
            this.localItems = [];
            this.noMore = false;
            this.active = false;
            this.isIntersecting = true;
            this.searchValue = null;
            this.loading = !this.show;
            this.$emit('close');
      },
        getOptionText(e) {
            return _.get(e, this.itemText);
        },
        search: _.debounce(function (val) {
            this.localItems = [];
            this.noMore = false;
            if(val?.trim() || this.show) {
              this.loading = true;
              this.$emit("search", val);
            }
        }, 300),
        endIntersect() {
          if(this.localItems.length > 0 && !this.noMore && !this.loading && !this.isIntersecting) {
            this.isIntersecting = true;
            this.$emit('searchMore', this.searchValue)
            this.loading = true;
          }
        }
    },
    watch: {
        searchValue: {
            handler() {
                this.search(this.searchValue);
            },
            deep: true,
        },
        items: {
          handler(items) {
            if(this.intersect && (items.length < this.moreLimit || items.length < this.localItems.length + this.moreLimit)) {
              this.noMore = true;
            }
            this.localItems = _.cloneDeep(items);
            setTimeout(() => {
              this.loading = false;
              this.isIntersecting = false;
            }, 100);
          }, deep: true, immediate: true
        }
    },
};
</script>

<style lang="scss" scoped>
.filter-select {
  box-shadow: 0px 0px 2px 0px rgba(0, 0, 0, 0.2) !important;
  width: 215px;
    .search-input {
        width: 100%;
        padding: 5px 10px;
        background: #f4f4f4;
        input {
            width: 100%;
            outline: none;
            &::placeholder {
                color: #7b7b7b;
                font-size: 14px;
            }
        }
    }
    .search-select-list {
            padding: 0;
            margin: 0;
            position: relative;
            background: #fff;
            z-index: 999999;
            // max-height: 200px;
            overflow-x: hidden;
            overflow-y: auto;
            box-shadow: inset 0 2px 12px -4px #c5d1d9;
            position: absolute;
            border-bottom-left-radius: 15px;
            list-style-type: none;
            width: 215px;
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
