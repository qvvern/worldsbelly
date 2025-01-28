<template>
    <BaseExpandContent class="side-filter" :label="$t('flavors.label')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div class="side-filter-flavors" >
            <FilterRange :min="min" :max="max" :label="`${$t('flavors.sweet.label')} (${sweetLabels[sweet[0]-1] || $t('any')} - ${sweetLabels[sweet[1]-1]})`" v-model="sweet" />
            <FilterRange :min="min" :max="max" :label="`${$t('flavors.sour.label')} (${sourLabels[sour[0]-1] || $t('any')} - ${sourLabels[sour[1]-1]})`" v-model="sour" />
            <FilterRange :min="min" :max="max" :label="`${$t('flavors.salty.label')} (${saltyLabels[salty[0]-1] || $t('any')} - ${saltyLabels[salty[1]-1]})`" v-model="salty" />
            <FilterRange :min="min" :max="max" :label="`${$t('flavors.bitter.label')} (${bitterLabels[bitter[0]-1] || $t('any')} - ${bitterLabels[bitter[1]-1]})`" v-model="bitter" />
            <FilterRange :min="min" :max="max" :label="`${$t('flavors.spices.label')} (${spicesLabels[spices[0]-1] || $t('any')} - ${spicesLabels[spices[1]-1]})`" v-model="spices" />
            <FilterRange :min="min" :max="max" :label="`${$t('flavors.flavor.label')} (${flavorLabels[flavor[0]-1] || $t('any')} - ${flavorLabels[flavor[1]-1]})`" v-model="flavor" />
        </div>
    </BaseExpandContent>
</template>

<script>
import _ from "lodash";
import { mapActions, mapGetters } from "vuex";

export default {
    name: "FilterFlavors",
    props: {
        expanded: Boolean,
        queries: Object,
    },
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
        FilterRange: () => import("@/components/filter/FilterRange.vue"),
    },
    data() {
        return {
            sweet: [0, 5],
            sour: [0, 5],
            salty: [0, 5],
            bitter: [0, 5],
            spices: [0, 5],
            flavor: [0, 5],
            min: 0,
            max: 5,
            clonedsweet: [],
            clonedsour: [],
            clonedsalty: [],
            clonedbitter: [],
            clonedspices: [],
            clonedflavor: [],
            sweetLabels: [this.$t('flavors.sweet.0'), this.$t('flavors.sweet.1'), this.$t('flavors.sweet.2'), this.$t('flavors.sweet.3'), this.$t('flavors.sweet.4')],
            sourLabels: [this.$t('flavors.sour.0'), this.$t('flavors.sour.1'), this.$t('flavors.sour.2'), this.$t('flavors.sour.3'), this.$t('flavors.sour.4')],
            saltyLabels: [this.$t('flavors.salty.0'), this.$t('flavors.salty.1'), this.$t('flavors.salty.2'), this.$t('flavors.salty.3'), this.$t('flavors.salty.4')],
            bitterLabels: [this.$t('flavors.bitter.0'), this.$t('flavors.bitter.1'), this.$t('flavors.bitter.2'), this.$t('flavors.bitter.3'), this.$t('flavors.bitter.4')],
            spicesLabels: [this.$t('flavors.spices.0'), this.$t('flavors.spices.1'), this.$t('flavors.spices.2'), this.$t('flavors.spices.3'), this.$t('flavors.spices.4')],
            flavorLabels: [this.$t('flavors.flavor.0'), this.$t('flavors.flavor.1'), this.$t('flavors.flavor.2'), this.$t('flavors.flavor.3'), this.$t('flavors.flavor.4')],
        };
    },
    computed: {
        ...mapGetters("ingredient/", ["get_ingredients"]),
    },
    mounted() {
        if (this.queries.fromsweet) {
            this.sweet[0] = Number(this.queries.fromsweet);
            this.$emit("update:expanded", true);
        }
        if (this.queries.tosweet) {
            this.sweet[1] = Number(this.queries.tosweet);
            this.$emit("update:expanded", true);
        }

        if (this.queries.fromsour) {
            this.sour[0] = Number(this.queries.fromsour);
            this.$emit("update:expanded", true);
        }
        if (this.queries.tosour) {
            this.sour[1] = Number(this.queries.tosour);
            this.$emit("update:expanded", true);
        }

        if (this.queries.fromsalty) {
            this.salty[0] = Number(this.queries.fromsalty);
            this.$emit("update:expanded", true);
        }
        if (this.queries.tosalty) {
            this.salty[1] = Number(this.queries.tosalty);
            this.$emit("update:expanded", true);
        }

        if (this.queries.frombitter) {
            this.bitter[0] = Number(this.queries.frombitter);
            this.$emit("update:expanded", true);
        }
        if (this.queries.tobitter) {
            this.bitter[1] = Number(this.queries.tobitter);
            this.$emit("update:expanded", true);
        }

        if (this.queries.fromspices) {
            this.spices[0] = Number(this.queries.fromspices);
            this.$emit("update:expanded", true);
        }
        if (this.queries.tospices) {
            this.spices[1] = Number(this.queries.tospices);
            this.$emit("update:expanded", true);
        }

        if (this.queries.fromflavor) {
            this.flavor[0] = Number(this.queries.fromflavor);
            this.$emit("update:expanded", true);
        }
        if (this.queries.toflavor) {
            this.flavor[1] = Number(this.queries.toflavor);
            this.$emit("update:expanded", true);
        }
    },
    watch: {
      sweet: {
        handler() {
          if(this.min != this.sweet[0] || this.max != this.sweet[1]) {
            this.$emit('sweet', {from: this.sweet[0], to: this.sweet[1]});
          }
          else {
            this.$emit('sweet', null);
          }
        }, deep: true
      },
      sour: {
        handler() {
          if(this.min != this.sour[0] || this.max != this.sour[1]) {
            this.$emit('sour', {from: this.sour[0], to: this.sour[1]});
          }
          else {
            this.$emit('sour', null);
          }
        }, deep: true
      },
      salty: {
        handler() {
          if(this.min != this.salty[0] || this.max != this.salty[1]) {
            this.$emit('salty', {from: this.salty[0], to: this.salty[1]});
          }
          else {
            this.$emit('salty', null);
          }
        }, deep: true
      },
      bitter: {
        handler() {
          if(this.min != this.bitter[0] || this.max != this.bitter[1]) {
            this.$emit('bitter', {from: this.bitter[0], to: this.bitter[1]});
          }
          else {
            this.$emit('bitter', null);
          }
        }, deep: true
      },
      spices: {
        handler() {
          if(this.min != this.spices[0] || this.max != this.spices[1]) {
            this.$emit('spices', {from: this.spices[0], to: this.spices[1]});
          }
          else {
            this.$emit('spices', null);
          }
        }, deep: true
      },
      flavor: {
        handler() {
          if(this.min != this.flavor[0] || this.max != this.flavor[1]) {
            this.$emit('flavor', {from: this.flavor[0], to: this.flavor[1]});
          }
          else {
            this.$emit('flavor', null);
          }
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedsweet = _.cloneDeep(this.sweet);
            this.sweet = [0, 5];
            this.clonedsour = _.cloneDeep(this.sour);
            this.sour = [0, 5];
            this.clonedsalty = _.cloneDeep(this.salty);
            this.salty = [0, 5];
            this.clonedbitter = _.cloneDeep(this.bitter);
            this.bitter = [0, 5];
            this.clonedspices = _.cloneDeep(this.spices);
            this.spices = [0, 5];
            this.clonedflavor = _.cloneDeep(this.flavor);
            this.flavor = [0, 5];
          }
          else if(this.clonedsweet.length > 0 ||
          this.clonedsour.length > 0 ||
          this.clonedsalty.length > 0 ||
          this.clonedbitter.length > 0 ||
          this.clonedspices.length > 0 ||
          this.clonedflavor.length > 0) {
            this.sweet = _.cloneDeep(this.clonedsweet);
            this.clonedsweet = [];
            this.sour = _.cloneDeep(this.clonedsour);
            this.clonedsour = [];
            this.salty = _.cloneDeep(this.clonedsalty);
            this.clonedsalty = [];
            this.bitter = _.cloneDeep(this.clonedbitter);
            this.clonedbitter = [];
            this.spices = _.cloneDeep(this.clonedspices);
            this.clonedspices = [];
            this.flavor = _.cloneDeep(this.clonedflavor);
            this.clonedflavor = [];
          }
        }
      }
    }
};
</script>

<style lang="scss">
.side-filter {
  &-flavors {
    .v-slider__thumb:after {
        cursor: move; /* fallback if grab cursor is unsupported */
        cursor: grab;
        cursor: -moz-grab;
        cursor: -webkit-grab;
    }
  }
}
</style>
