<template>
  <div class="recipe-data-table" ref="recipeDataTable">
    <v-data-table
      :headers="headers"
      :items="items"
      :items-per-page="-1"
      hide-default-footer
      fixed-header
      :mobile-breakpoint="0"
    >
      <template v-slot:[`header.amount`]>
        <span>Amount</span>
      </template>
      <template v-slot:[`item.amount`]="{ item }">
          <b v-tooltip="''+item.amount">{{ parseFloat((item.amount).toFixed(2)) }}</b>
      </template>
      <template v-slot:[`item.unit`]="{ value, index }">
        <RecipeDataTableDropdown :value="value" :index="index" @selectMeasurement="selectMeasurement($event, index)" />
      </template>
    </v-data-table>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import _ from "lodash";

export default {
	name: 'RecipeNutrientTable',
	components: {
		RecipeDataTableDropdown: () => import('@/components/recipe/RecipeDataTableDropdown.vue'),
  },
	props: {
    servings: Number,
    nutrients: Array
	},
	data() {
		return {
      height: null,
      currentServings: null,
      hover: null,
		}
	},
  computed: {
		...mapGetters('measurement/', ['get_measurements']),
        ...mapGetters("nutrient/", ["get_nutrients"]),
        headers() {
            return [
                {
                    text: "#",
                    align: "start",
                    sortable: false, // will cause bug when selecting new meaurement type.
                    value: "orderindex",
                },
                {
                    text: "Name",
                    align: "start",
                    sortable: false,
                    value: "name",
                },
                {
                    text: "Amount",
                    align: "end",
                    sortable: false,
                    value: "amount",
                },
                {
                    text: "Unit",
                    align: "end",
                    sortable: false,
                    value: "unit",
                },
            ];
        },
        items() {
            var index = 1;
            return this.nutrients.map((n) => {
                var nutrient = this.get_nutrients.find((_) => _.id == n.id);
                return {
                    orderindex: index++,
                    name: nutrient.name,
                    amount: n.amount,
                    unit: this.get_measurements.find((_) => _.id == nutrient.measurementId),
                };
            });
        },
  },
	methods: {
    selectMeasurement(newMeasurement, index) {
      const currentItem = this.items[index];
      const baseAmount = currentItem.amount * currentItem.unit.conversionAmount;
      this.items[index].unit = newMeasurement;
      this.items[index].amount = (baseAmount / newMeasurement.conversionAmount);
    }
	},
  mounted() {
    this.currentServings = this.servings;
  },
  watch: {
    servings: {
      handler() {
        if(this.servings == 0) return;
        this.items.map(item => {
          var singleAmount = item.amount / this.currentServings;
          item.amount = (singleAmount * this.servings);
        })
        this.currentServings = this.servings;
      }
    },
  }
}
</script>

<style lang="scss">
.recipe-data-table {
  th:first-child {
    i {
      display: none !important
    }
  }

  th:nth-child(3) {
    min-width: 100px;
  }
  min-height: 100%;
}
.recipe-data-table-list {
  background: white;
  .v-list-item {
    cursor: pointer;
    min-height: 20px;
    // border-bottom: 1px solid #ececec;
    * {
      font-size: 12px;
    }
    background: white;
    &.active {
      background: #ececec;
    }
    &.hover {
      background: #e8f2f3;
    }
  }
}
</style>
