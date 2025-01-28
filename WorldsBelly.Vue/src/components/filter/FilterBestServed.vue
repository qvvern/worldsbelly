<template>
    <BaseExpandContent class="side-filter" :label="$t('bestServed')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div class="flex flex-wrap w-full">
          <baseCheckbox class="mr-3 mb-1" v-for="(d, i) in bestServed" :key="i" v-model="bestServed[i].value" :label="d.label" />
        </div>
    </BaseExpandContent>
</template>

<script>
import { mapGetters } from "vuex";
import _ from "lodash";

export default {
    name: "FilterBestServed",
    props: {
      expanded: Boolean,
      queries: Object
    },
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
    },
    data() {
        return {
          bestServed: [],
          clonedBestServed: []
        };
    },
    computed: {
		...mapGetters('recipe/', [ 'get_recipe_best_served']),
    },
    mounted() {
      this.bestServed = this.get_recipe_best_served.map(_ => {
        return {
          id: _.id,
          label: _.label,
          value: this.queries.bestServed.some(id => id == _.id)
        };
      })
    },
    watch: {
      bestServed: {
        handler() {
          var queriesTags = this.queries.bestServed?.length > 0 ? this.queries.bestServed.filter(id => !this.bestServed.some(_ => _.id == Number(id))) : [];
          var selected = this.bestServed.filter(_ => _.value == true).map(_ => _.id);
          this.$emit('input', queriesTags.concat(selected));
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedBestServed = _.cloneDeep(this.bestServed);
            this.bestServed = this.bestServed.map((tag) => {tag.value = false; return tag});
          }
          else if(this.clonedBestServed.length > 0) {
            this.bestServed = _.cloneDeep(this.clonedBestServed);
            this.clonedBestServed = [];
          }
        }
      }
    }
};
</script>

<style lang="scss">
.side-filter {
}
</style>
