<template>
    <BaseExpandContent class="side-filter" label="Nutrients" :expanded="true">
        <div>
          <baseCheckbox v-model="showInclude" label="Included" />
            <div v-if="showInclude">
                <div class="py-1">
                  <FilterInputDropdown
                    :label="includedNutrient.name + ` (${get_measurements.find(_ => _.id == includedNutrient.measurementId).unit})`"
                    :options="[{id: 0, label: 'less than'}, {id: 1, label: 'more than'}]"
                    placeholder="anything"
                    :selectedOption="0"
                     v-for="(includedNutrient, i) in includedNutrients"
                    :key="includedNutrient.id"
                    v-model="includedNutrients[i].amount"
                  >
                  </FilterInputDropdown>
                </div>
                <FilterSelect
                    :items.sync="sortedIncluded"
                    :excludeItems="includedNutrients.concat(excludedNutrients)"
                    @selected="selectNutrient($event)"
                    @search="searchInclude($event)"
                    @close="resetSearch()"
                    :moreLimit="50"
                    itemText="name"
                    placeholder="Select nutrient"
                    :intersect="false"
                    show
                />
            </div>
        </div>
        <div class="pt-4">
          <baseCheckbox v-model="showExclude" label="Excluded" />
            <div v-if="showExclude">
                <div class="py-1">
                    <v-chip
                        v-for="nutrient in excludedNutrients"
                        :key="nutrient.id"
                        small
                        @click:close="removeExcluded(nutrient)"
                        close
                        >{{ nutrient.name }}</v-chip
                    >
                </div>
                <FilterSelect
                    :items.sync="sortedExcluded"
                    :excludeItems="includedNutrients.concat(excludedNutrients)"
                    @selected="selectExcludeNutrient($event)"
                    @search="searchExclude($event)"
                    @close="resetSearch()"
                    :moreLimit="50"
                    itemText="name"
                    placeholder="Select nutrient"
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
    name: "FilterNutrients",
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
        FilterSelect: () => import("@/components/filter/FilterSelect.vue"),
        FilterInputDropdown: () => import("@/components/filter/FilterInputDropdown.vue"),
    },
    data() {
        return {
            expanded: true,
            showInclude: true,
            showExclude: false,
            includedNutrients: [],
            excludedNutrients: [],
            loadingNutrientsList: false,
            sortedIncluded: [],
            sortedExcluded: [],
        };
    },
    computed: {
       ...mapGetters('measurement/', ['get_measurements']),
        ...mapGetters("nutrient/", ["get_nutrients"]),
    },
    mounted() {
      this.resetSearch();
    },
    methods: {
      resetSearch() {
        this.$nextTick(() => {
          this.sortedExcluded = _.cloneDeep(this.get_nutrients);
          this.sortedIncluded = _.cloneDeep(this.get_nutrients);
        })
      },
      searchExclude(search) {
        if(!search) {
          this.sortedExcluded = _.cloneDeep(this.get_nutrients);
          return;
        }
        this.sortedExcluded = _.cloneDeep(this.get_nutrients).filter(_ => _.name?.toLowerCase().includes(search?.toLowerCase()))
      },
      searchInclude(search) {
        if(!search) {
          this.sortedIncluded = _.cloneDeep(this.get_nutrients);
          return;
        }
          this.sortedIncluded = _.cloneDeep(this.get_nutrients).filter(_ => _.name?.toLowerCase().includes(search?.toLowerCase()))
      },
        selectNutrient(nutrient) {
            this.includedNutrients.push(nutrient);
        },
        selectExcludeNutrient(nutrient) {
            this.excludedNutrients.push(nutrient);
        },
        remove(i) {
           this.includedNutrients = this.includedNutrients.filter(_ => _.id != i.id);
        },
        removeExcluded(i) {
           this.excludedNutrients = this.excludedNutrients.filter(_ => _.id != i.id);
        },
    },
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
