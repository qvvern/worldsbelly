<template>
  <baseLayout class="recipe-create">
    <div v-if="isPageReady" slot="header">
      <v-progress-linear
        height="6"
        :value="progress"
        color="amber"
      ></v-progress-linear>
    </div>
    <div v-if="isPageReady" slot="content" class="recipe-create__body">
      <RecipeCreateChoices
        v-model="choices"
        @createRecipeDraft="handleCreate($event)"
        @progressComplete="progress = 100"
        @progressUpdate="progress = ($event * 10) * 1.7"
      />
    </div>
    <template slot="loader">
        <baseLoading v-if="!isPageReady" :value="true" overlay parent fade :progressSentences="[`${$t('preparing')} ${$t('choices')}`]" />
        <baseLoading v-if="creatingRecipe" :value="true" overlay parent fade :progressSentences="[`${$t('creating')} ${$t('recipe')}`]" />
    </template>
  </baseLayout>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import _ from "lodash";
import { administrationMixin } from "@/mixins/AdministrationMixin";

export default {
	name: 'RecipeCreate',
  mixins: [administrationMixin],
	components: {
		RecipeCreateChoices: () => import('@/components/administration/recipeCreate/RecipeCreateChoices.vue'),
	},
	data() {
		return {
      isPageReady: false,
      creatingRecipe: false,
      choices: [],
      progress: 10
		}
	},
  computed: {
		...mapGetters('tagSelectable/', ['get_tagSelectable_choices']),
		...mapGetters('recipe/', ['get_recipe_draft']),
  },
  methods: {
		...mapActions('tagSelectable/', ['fetch_tagSelectable_choices']),
		...mapActions('recipe/', ['create_recipe_draft']),
        handleCreate(selectedChoices) {
          this.creatingRecipe = true;
          var selectedTags = [];
          selectedChoices.forEach(c => {
            var category = c.tagSelectableCategory;
            var selected = category.tagSelectables.find(_ => _.selected == true);
            if(!selected.dontAddTag) {
              selectedTags.push(selected.tagId);
            }
          });
            let newRecipe = {
              tagIds: selectedTags,
              languageId: 20
            };
            this.create_recipe_draft(newRecipe).then((e) => {
              window.location =  `${window.location.origin}/${this.$language.code}/administration/drafts/${this.get_recipe_draft.id}`;
            }).finally(() => {
              this.creatingRecipe = false;
            });
        },
        mapItems() {
            this.isPageReady = false;
            this.fetch_tagSelectable_choices().then(() => {
                var firstChoices = this.get_tagSelectable_choices?.filter(_ => _.orderIndex == 1) || [];
                this.choices = firstChoices.map(choice => {
                    return this.mapChoices(choice, 2, [])
                })
                this.isPageReady = true;
            })
        },
        mapChoices(choiceParam, orderIndex, tagIds) {
            var Vue = this;
            var choice = choiceParam;
            var availableChoices = this.get_tagSelectable_choices?.filter(_ => _.orderIndex == orderIndex && _.relatedChoiceId == choice.id) || [];
            if(!availableChoices || availableChoices?.length == 0) return choice;

            var selectables = [];
            choice.tagSelectableCategory?.tagSelectables?.forEach(selectable => {
                var foundCategory = availableChoices.find(_ => _.tagId == selectable.tag.id);
                var nextOrderIndex = orderIndex+1;
                var newSelectable = selectable;
                if(foundCategory) {
                    var prevTagIds = _.cloneDeep(tagIds);
                    prevTagIds.push(foundCategory.tagId)
                    newSelectable.choice = Vue.mapChoices(foundCategory, nextOrderIndex, prevTagIds);
                }
                else {
                    var foundAnotherCategory = availableChoices.find(_ => _.tagId == null);
                    if(foundAnotherCategory) {
                        var prevTagIds2 = _.cloneDeep(tagIds);
                        prevTagIds2.push(foundAnotherCategory.tagId)
                        newSelectable.choice = Vue.mapChoices(foundAnotherCategory, nextOrderIndex, prevTagIds2);
                    }
                }
                if(newSelectable?.choice?.tagSelectableCategory?.tagSelectables) {
                    newSelectable.choice.tagSelectableCategory.tagSelectables = newSelectable.choice.tagSelectableCategory.tagSelectables.filter(s => {
                        var excludedTags = Vue.excludedTags(s.excludedTags);
                        if(excludedTags.some(id => tagIds.some(_ => _ == id))) return;
                        return s
                    })
                }
                selectables.push(newSelectable);
            })
            choice.tagSelectableCategory.tagSelectables = selectables;
            return choice;
        },
        excludedTags(tagIds) {
            if(!tagIds) return [];
            var tags = tagIds.split(',');
            return tags.map(tag => Number(tag))
        },
  },
  created() {
      this.fetch_tagSelectable_choices().then(() => {
        this.mapItems();
      })
  },
}
</script>

<style lang="scss" scoped>
.recipe-create {
  &__body {
    position: relative;
    height: 100%;
    width: 100%;
    // display: flex;
  }
}
</style>
