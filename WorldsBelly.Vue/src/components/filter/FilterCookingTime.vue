<template>
    <BaseExpandContent class="side-filter" :label="$t('Cooking/Prep')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div>
            <FilterInputDropdown
              :options="[{id: 0, label: `${$t('lessThan')} (${$t('minutes')})`}, {id: 1, label:  `${$t('moreThan')} (${$t('minutes')})`}]"
              :placeholder="$t('any')"
              :selectedOption="time.option"
              @selected="time.option = $event.id"
              v-model="time.amount"
              type="number"
            >
            </FilterInputDropdown>
        </div>
    </BaseExpandContent>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import _ from "lodash"

export default {
    name: "FilterCookingTime",
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
        FilterInputDropdown: () => import("@/components/filter/FilterInputDropdown.vue"),
    },
    props: {
      expanded: Boolean,
      queries: Object
    },
    data() {
        return {
            time: {
              amount: null,
              option: 0,
            },
            clonedTime: {
              amount: null,
              option: 0,
            }
        };
    },
    computed: {
       ...mapGetters('measurement/', ['get_measurements']),
        ...mapGetters("nutrient/", ["get_nutrients"]),
    },
    mounted() {
        if(!_.isEmpty(this.queries.time)) {
          this.time.amount = this.queries.time.amount;
          this.time.option = this.queries.time.option;
          this.$emit('update:expanded', true)
        }
    },
    watch: {
      time: {
        handler() {
          if(this.time.amount) {
            this.$emit('input', this.time)
          }
          else {
            this.$emit('input', {})
          }
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedTime = _.cloneDeep(this.time);
            this.time = {
              amount: null,
              option: 0,
            };
          }
          else if(this.clonedTime.amount) {
            this.time = _.cloneDeep(this.clonedTime);
            this.clonedTime = {
              amount: null,
              option: 0,
            };
          }
        }
      }
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
