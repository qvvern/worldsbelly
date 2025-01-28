<template>
    <BaseExpandContent class="side-filter" :label="$t('cuisines')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div>
          <baseCheckbox v-model="showInclude" :label="$t('included')" />
            <div v-if="showInclude">
                <div class="py-1">
                    <v-chip
                        v-for="includedCountry in includedCountries"
                        :key="includedCountry.id"
                        small
                        @click:close="remove(includedCountry)"
                        close
                        >{{ includedCountry.name }}</v-chip
                    >
                </div>
                <FilterSelect
                    :items.sync="sortedIncluded"
                    :excludeItems="includedCountries.concat(excludedCountries)"
                    @selected="selectCountry($event)"
                    @search="searchInclude($event)"
                    @close="resetSearch()"
                    :moreLimit="50"
                    itemText="name"
                    :placeholder="$t('select') +' '+$t('country')"
                    :intersect="false"
                    show
                />
            </div>
        </div>
        <div class="pt-4">
          <baseCheckbox v-model="showExclude" :label="$t('excluded')" />
            <div v-if="showExclude">
                <div class="py-1">
                    <v-chip
                        v-for="country in excludedCountries"
                        :key="country.id"
                        small
                        @click:close="removeExcluded(country)"
                        close
                        >{{ country.name }}</v-chip
                    >
                </div>
                <FilterSelect
                    :items.sync="sortedExcluded"
                    :excludeItems="includedCountries.concat(excludedCountries)"
                    @selected="selectExcludeCountry($event)"
                    @search="searchExclude($event)"
                    @close="resetSearch()"
                    :moreLimit="50"
                    itemText="name"
                    :placeholder="$t('select') +' '+$t('country')"
                    :intersect="false"
                    show
                />
            </div>
        </div>
    </BaseExpandContent>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import _ from "lodash"

export default {
    name: "FilterCountries",
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
        FilterSelect: () => import("@/components/filter/FilterSelect.vue"),
    },
    props: {
      expanded: Boolean,
      queries: Object
    },
    data() {
        return {
            showInclude: true,
            showExclude: false,
            includedCountries: [],
            excludedCountries: [],
            loadingCountriesList: false,
            sortedIncluded: [],
            sortedExcluded: [],
            clonedIncludedCountries: [],
            clonedExcludedCountries: []
        };
    },
    computed: {
        ...mapGetters("country/", ["get_countries"]),
    },
    mounted() {
      this.resetSearch();
      if(this.queries?.includecuisines?.length > 0) {
        this.$emit('update:expanded', true)
        this.showInclude = true;
        this.includedCountries = this.queries.includecuisines.map(id => {
          return this.get_countries.find(c => c.id == id);
        })
      }
      if(this.queries?.excludecuisines?.length > 0) {
        this.$emit('update:expanded', true)
        this.showExclude = true;
        this.excludedCountries = this.queries.excludecuisines.map(id => {
          return this.get_countries.find(c => c.id == id);
        })
      }
    },
    methods: {
      resetSearch() {
        this.$nextTick(() => {
          this.sortedExcluded = _.cloneDeep(this.get_countries);
          this.sortedIncluded = _.cloneDeep(this.get_countries);
        })
      },
      searchExclude(search) {
        if(!search) {
          this.sortedExcluded = _.cloneDeep(this.get_countries);
          return;
        }
        this.sortedExcluded = _.cloneDeep(this.get_countries).filter(_ => _.name?.toLowerCase().includes(search?.toLowerCase()))
      },
      searchInclude(search) {
        if(!search) {
          this.sortedIncluded = _.cloneDeep(this.get_countries);
          return;
        }
          this.sortedIncluded = _.cloneDeep(this.get_countries).filter(_ => _.name?.toLowerCase().includes(search?.toLowerCase()))
      },
        selectCountry(country) {
            this.includedCountries.push(country);
        },
        selectExcludeCountry(country) {
            this.excludedCountries.push(country);
        },
        remove(i) {
           this.includedCountries = this.includedCountries.filter(_ => _.id != i.id);
        },
        removeExcluded(i) {
           this.excludedCountries = this.excludedCountries.filter(_ => _.id != i.id);
        },
    },
    watch: {
      includedCountries: {
        handler() {
          var selected = this.includedCountries?.map(_ => _.id) || [];
          this.$emit('includedCuisines', selected);
        }, deep: true
      },
      excludedCountries: {
        handler() {
          var selected = this.excludedCountries?.map(_ => _.id) || [];
          this.$emit('excludedCuisines', selected);
        }, deep: true
      },
      showInclude: {
        handler(show) {
          if(!show) {
            this.clonedIncludedCountries = _.cloneDeep(this.includedCountries);
            this.includedCountries = [];
          }
          else if(this.clonedIncludedCountries.length > 0) {
            this.includedCountries = _.cloneDeep(this.clonedIncludedCountries).filter(({id}) => !this.excludedCountries.some(x => x.id == id));
            this.clonedIncludedCountries = [];
          }
        }
      },
      showExclude: {
        handler(show) {
          if(!show) {
            this.clonedExcludedCountries = _.cloneDeep(this.excludedCountries);
            this.excludedCountries = [];
          }
          else if(this.clonedExcludedCountries.length > 0) {
            this.excludedCountries = _.cloneDeep(this.clonedExcludedCountries).filter(({id}) => !this.includedCountries.some(x => x.id == id));
            this.clonedExcludedCountries = [];
          }
        }
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedExcludedCountries = _.cloneDeep(this.excludedCountries);
            this.excludedCountries = [];
            this.clonedIncludedCountries = _.cloneDeep(this.includedCountries);
            this.includedCountries = [];
          }
          else if(this.clonedExcludedCountries.length > 0 || this.clonedIncludedCountries.length > 0) {
            this.excludedCountries = _.cloneDeep(this.clonedExcludedCountries);
            this.clonedExcludedCountries = [];
            this.includedCountries = _.cloneDeep(this.clonedIncludedCountries);
            this.clonedIncludedCountries = [];
          }
        }
      }
    }
};
</script>

<style lang="scss">
.side-filter {
    // border-bottom: 1px solid #f7f9fb;
    .v-input--checkbox {
        label {
            -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
            -webkit-font-smoothing: antialiased;
            font-family: "Google Sans", Roboto, Arial, sans-serif !important;
            color: #111;
            background-repeat: no-repeat;
            box-sizing: inherit;
            font-size: 12px;
            line-height: normal;
            font-weight: 400;
        }
    }
    .v-chip {
      // box-shadow: 0px 0px 2px 0px rgba(0, 0, 0, 0.4) !important;
      background: #15132b !important;
      color: #f5f5f5 !important;
      border-radius: 4px !important;
      margin: 3px;
      // height: auto !important;
      .v-icon {
        font-size: 13px !important;
        color: rgba(254, 254, 255, 0.65) !important;
      }
    }
}
</style>
