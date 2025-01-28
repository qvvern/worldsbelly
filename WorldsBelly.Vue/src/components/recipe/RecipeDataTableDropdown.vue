<template>
    <v-menu offset-y :close-on-click="true" v-model="showMenu">
      <template v-slot:activator="{ on, attrs }">
        <div
          style="width: 100%; display: flex; justify-content: flex-end"
          color="primary"
          dark
          v-bind="attrs"
          v-on="on"
          @click="hover = null"
        >
        <span style="font-size: 14px"><slot></slot></span> <p style="min-width: 25px;font-size: 14px">{{ value.unit }}</p>
        <baseIcon :size="12" style="margin-top: -5px; margin-right: -8px; opacity: .5">{{showMenu ? 'icofont-rounded-up' : 'icofont-rounded-down'}}</baseIcon>
        </div>
      </template>
      <v-list class="recipe-data-table-list" max-height="200px" flat>
              <v-list-item
                @click="$emit('selectMeasurement', measurement)"
                @mouseenter="hover = measurement.id"
                :class="{active: measurement.unit == value.unit, hover: hover == measurement.id}"
                v-for="(measurement, i) in filteredMeasurements()"
                :key="i"
              >
                <v-list-item-title>{{ measurement.unit }}</v-list-item-title>
              </v-list-item>
      </v-list>
    </v-menu>
</template>

<script>
import { mapGetters } from 'vuex'
import _ from "lodash";

export default {
	name: 'RecipeDataTableDropdown',
	components: {
  },
	props: {
    value: Object,
    item: Object
	},
	data() {
		return {
      showMenu: false,
      hover: null,
		}
	},
  computed: {
		...mapGetters('measurement/', ['get_measurements']),
  },
	methods: {
    filteredMeasurements() {
      var types = [this.value?.typeId];
      if(this.item?.oneCentimeterInGram != null) {
        types.push(2);
        types.push(4);
      }
      if(this.item?.oneCentimeterInMilliliter != null) {
        types.push(3);
        types.push(4);
      }
      if(this.item?.oneMilliliterInGram != null) {
        types.push(2);
        types.push(3);
      }
      if(this.item?.onePieceInCentimeter != null) {
        types.push(4);
        types.push(5);
      }
      if(this.item?.onePieceInGram != null) {
        types.push(2);
        types.push(5);
      }
      if(this.item?.onePieceInMilliliter != null) {
        types.push(3);
        types.push(5);
      }
      const valueTypes = _.uniq(types);
      return _.cloneDeep(this.get_measurements)?.filter(_ => valueTypes?.includes(_.typeId));
    }
	},
  mounted() {
  },
  watch: {
  }
}
</script>

<style lang="scss">
.recipe-data-table {
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
