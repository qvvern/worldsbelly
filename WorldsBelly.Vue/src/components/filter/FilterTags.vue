<template>
    <BaseExpandContent class="side-filter" :label="$t('popularTags')" :expanded.sync="expanded" @onClick="$emit('update:expanded', $event)">
        <div class="flex flex-wrap w-full">
          <baseCheckbox class="mr-3 mb-1" v-for="(tag, i) in tags" :key="i" v-model="tags[i].value" :label="tag.label" />
        </div>
    </BaseExpandContent>
</template>

<script>
import { mapGetters } from "vuex";
import _ from "lodash";

export default {
    name: "FilterTags",
    props: {
      expanded: Boolean,
      queries: Object
    },
    components: {
        BaseExpandContent: () => import("@/components/base/BaseExpandContent.vue"),
    },
    data() {
        return {
          tags: [
            {id: 2221, label: this.$t('tags.vegan'), value: false},
            {id: 2349, label: this.$t('tags.vegetarian'), value: false},
            {id: 470, label: this.$t('tags.glutenFree'), value: false},
          ],
          clonedTags: []
        };
    },
    computed: {
		...mapGetters('recipe/', [ 'get_recipe_difficulty' ]),
    },
    methods: {
    },
    mounted() {
      if(this.queries.tags?.length > 0) {
        for (let i = 0; i < this.tags.length; i++) {
          this.tags[i].value =  this.queries.tags.some(q => q == this.tags[i].id) ? true : false;
        }
      }
    },
    watch: {
      tags: {
        handler() {
          var queriesTags = this.queries.tags?.length > 0 ? this.queries.tags.filter(id => !this.tags.some(_ => _.id == Number(id))) : [];
          var selected = this.tags.filter(_ => _.value == true).map(_ => _.id);
          this.$emit('input', queriesTags.concat(selected));
        }, deep: true
      },
      expanded: {
        handler(show) {
          if(!show) {
            this.clonedTags = _.cloneDeep(this.tags);
            this.tags = this.tags.map((tag) => {tag.value = false; return tag});
          }
          else if(this.clonedTags.length > 0) {
            this.tags = _.cloneDeep(this.clonedTags);
            this.clonedTags = [];
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
