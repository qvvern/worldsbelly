<template>
    <BaseExpandContent class="side-filter" :label="$t('nutrients')+' ('+$t('perServing')+')'" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div>
                <div class="py-1">
                  <template v-for="(includedNutrient, i) in nutrients">
                  <FilterInputDropdown
                    :label="includedNutrient.name + ` (${get_measurements.find(_ => _.id == includedNutrient.measurementId).unit})`"
                    :options="[{id: 0, label: $t('lessThan')}, {id: 1, label: $t('moreThan')}]"
                    placeholder="0"
                    :selectedOption="nutrients[i].option"
                    @selected="nutrients[i].option = $event.id"
                    @remove="remove(nutrients[i])"
                    :key="includedNutrient.id"
                    v-model="nutrients[i].amount"
                    type="number"
                  >
                  </FilterInputDropdown>
                  </template>
                </div>
                <FilterSelect
                    :items.sync="sortedIncluded"
                    :excludeItems="nutrients"
                    @selected="selectNutrient($event)"
                    @search="searchInclude($event)"
                    @close="resetSearch()"
                    :moreLimit="50"
                    itemText="name"
                    :placeholder="$t('select') +' '+$t('nutrient')"
                    :intersect="false"
                    show
                />
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
    props: {
      expanded: Boolean,
      queries: Object
    },
    data() {
        return {
            nutrients: [],
            loadingNutrientsList: false,
            sortedIncluded: [],
            sortedExcluded: [],
            clonedNutrients: []
        };
    },
    computed: {
       ...mapGetters('measurement/', ['get_measurements']),
        ...mapGetters("nutrient/", ["get_nutrients"]),
    },
    mounted() {
      this.resetSearch();
      if(this.queries?.nutrients?.length > 0) {
        this.nutrients = this.queries.nutrients.map(_ => {
          var nutrient = this.get_nutrients.find(n => n.id == _.id);
          return {
            ...nutrient,
            amount: _.amount,
            option: _.option
          }
        })
      }
    },
    methods: {
      resetSearch() {
        this.$nextTick(() => {
          this.sortedIncluded = _.cloneDeep(this.get_nutrients);
        })
      },
      searchInclude(search) {
        if(!search) {
          this.sortedIncluded = _.cloneDeep(this.get_nutrients);
          return;
        }
          this.sortedIncluded = _.cloneDeep(this.get_nutrients).filter(_ => _.name?.toLowerCase().includes(search?.toLowerCase()))
      },
        selectNutrient(nutrient) {
            var clonedNutrient = _.cloneDeep(nutrient);
            clonedNutrient.amount = 0;
            clonedNutrient.option = 0;
            this.nutrients.push(clonedNutrient);
        },
        remove(i) {
           this.nutrients = this.nutrients.filter(_ => _.id != i.id);
        },
    },
    watch: {
      nutrients: {
        handler() {
          // {id: 1, a: 200, o: 4}
          var queryNutrients = this.nutrients.map(n => {
            return {
              id: n.id,
              amount: n.amount,
              option: n.option
            }
          })
          this.$emit('input', queryNutrients)
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedNutrients = _.cloneDeep(this.nutrients);
            this.nutrients = [];
          }
          else if(this.clonedNutrients.length > 0) {
            this.nutrients = _.cloneDeep(this.clonedNutrients);
            this.clonedNutrients = [];
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
