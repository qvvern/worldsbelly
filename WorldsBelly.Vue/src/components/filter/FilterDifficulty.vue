<template>
    <BaseExpandContent class="side-filter" :label="$t('difficulty')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div class="flex flex-wrap w-full">
          <baseCheckbox class="mr-3 mb-1" v-for="(d, i) in difficulty" :key="i" v-model="difficulty[i].value" :label="d.label" />
        </div>
    </BaseExpandContent>
</template>

<script>
import { mapGetters } from "vuex";
import _ from "lodash";

export default {
    name: "FilterDifficulty",
    props: {
      expanded: Boolean,
      queries: Object
    },
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
    },
    data() {
        return {
          difficulty: [],
          clonedDifficulty: []
        };
    },
    computed: {
		...mapGetters('recipe/', [ 'get_recipe_difficulty' ]),
    },
    mounted() {
      this.difficulty = this.get_recipe_difficulty.map(_ => {
        return {
          id: _.id,
          label: _.label,
          value: this.queries.difficulty.some(id => id == _.id)
        };
      });
      if(this.difficulty.some(t => t.value == true)) {
        this.$emit('update:expanded', true)
      }
    },
    watch: {
      difficulty: {
        handler() {
          var queriesTags = this.queries.difficulty?.length > 0 ? this.queries.difficulty.filter(id => !this.difficulty.some(_ => _.id == Number(id))) : [];
          var selected = this.difficulty.filter(_ => _.value == true).map(_ => _.id);
          this.$emit('input', queriesTags.concat(selected));
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedDifficulty = _.cloneDeep(this.difficulty);
            this.difficulty = this.difficulty.map((tag) => {tag.value = false; return tag});
          }
          else if(this.clonedDifficulty.length > 0) {
            this.difficulty = _.cloneDeep(this.clonedDifficulty);
            this.clonedDifficulty = [];
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
