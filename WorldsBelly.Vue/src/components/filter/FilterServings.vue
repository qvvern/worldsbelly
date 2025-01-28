<template>
    <BaseExpandContent class="side-filter" :label="$t('servings')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
              <baseQuantityInput
                :placeholder="$t('intendedAmount')"
                :min="0"
                v-model="input"
              />
    </BaseExpandContent>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import _ from "lodash";

export default {
    name: "FilterServings",
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
    },
    props: {
      expanded: Boolean,
      queries: Object
    },
    data() {
        return {
            input: null,
        };
    },
    computed: {
    },
    methods: {
    },
    mounted() {
      if(this.queries.servings) {
          this.input = Number(this.queries.servings);
      }
    },
    watch: {
      input: {
        handler() {
          if(this.input <= 0) this.input = null;
          this.$emit('input', this.input);
        }
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedInput = _.cloneDeep(this.input);
            this.input = null;
          }
          else if(this.clonedInput) {
            this.input = _.cloneDeep(this.clonedInput);
            this.clonedInput = null;
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
