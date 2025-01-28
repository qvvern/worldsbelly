<template>
  <div class="recipe-create-option-selectors w-full">
    <baseLoading :value="!value || !value[0].options.length > 0" overlay parent :progressSentences="[`${$t('preparing')} ${$t('options')}`]" />
    <div id="line-container"></div>
    <BaseLine
      container="#line-container"
      v-for="(line, i) in lines"
      :key="i"
      :to="line.to"
      :from="line.from"
    />
    <template v-if="value && value[0].options.length > 0">
      <template  v-for="(step, i) in value">
        <RecipeCreateOptionSelector
          class="recipe-create-option-selector"
          :id="'RecipeCreateOptionSelector-'+step.id"
          :key="'l'+i"
          :title="step.title"
          :subtitle="step.subtitle"
          :options="step.options"
          :step="step"
          :parentStep.sync="value[i - 1]"
          :active="step.active"
          @selected="select($event)"
          @createRecipeDraft="$emit('createRecipeDraft', $event)"
          :selectedTags.sync="selectedTags"
          @selectSingleOption="select($event)"
        />
      </template>
    </template>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import VueScrollTo  from 'vue-scrollto';
import _ from "lodash";
export default {
	name: 'RecipeCreateSteps',
	components: {
		RecipeCreateOptionSelector: () => import('@/components/administration/recipeCreate/RecipeCreateOptionSelector.vue'),
		BaseLine: () => import('@/components/base/BaseLine.vue'),
	},
  props: {
    value: Array,
    selectedTags: Array,
  },
	data() {
		return {
      isFetchingTags: false,
      lines: [],
		}
	},
  created() {
    this.$nextTick(() => {
        var linksContainer = document.querySelector('#line-container');
        var rectWrapper = linksContainer?.parentElement.getBoundingClientRect();
        linksContainer.style.transform = `translate(${-rectWrapper.left}px, ${-rectWrapper.top}px)`;
    })
  },
  computed: {
        ...mapGetters("tagSelectable/", ["get_tag_selectables"]),
  },
	methods: {
    resetFirst() {
        this.lines = []
        var activeStep = this.value[0];
        activeStep.active = true;
        this.$emit('deselectedId', _.clone(this.get_tag_selectables.find(_ => _.id == activeStep.selectedId)?.tagId));
        activeStep.selectedId = null;
        activeStep.selectedName = null;
    },
    prevStep() {
        var activeStepIndex = this.value.findIndex(_ => _.active == true);
        var prevStep = this.value[activeStepIndex-1];
        var deselectStep = this.value[activeStepIndex-1];
        var activeStep = this.value[activeStepIndex];
        activeStep.active = false;
        deselectStep.active = true;
        this.$emit('deselectedId', _.clone(this.get_tag_selectables.find(_ => _.id == deselectStep.selectedId)?.tagId));
        deselectStep.selectedId = null;
        deselectStep.selectedName = null;
        var lineIndex = this.lines.findIndex(_ => _.id == deselectStep.id);
        this.lines.splice(lineIndex, 1);
        this.scrollTo(`RecipeCreateOptionSelector-${prevStep.id}`);
    },
    reselect(step, option) {
        var lineIndex = this.lines.findIndex(_ => _.id == step.id);
        this.lines.splice(lineIndex);
        step.selectedId = null;
        step.active = true;
        this.$nextTick(() => {
          this.select({step: step, option:option})
        })
    },
    select(payload) {
        if(payload.step.selectedId) {
          this.reselect(payload.step, payload.option);
          return;
        }
          var activeStepIndex = this.value.findIndex(_ => _.active == true);
          var activeStep = this.value[activeStepIndex];
          var nextStep = this.value[activeStepIndex+1];
          this.$emit('selectedId', _.clone(payload.option.tagId));
          activeStep.selectedId = payload.option.id;
          activeStep.selectedName = payload.option.name;
          activeStep.active = false;
          if(!nextStep) {
            this.$emit('nextStepCategory');
            return;
          }
          nextStep.active = true;
          this.lines.push({id: activeStep.id, to: `[data-tag='${payload.step.id}-${payload.option.id}']`, from: `#RecipeCreateOptionSelector-${activeStep.id+1}`});
          this.scrollTo(`RecipeCreateOptionSelector-${activeStep.id+1}`);
        },
      scrollTo(id) {
        var element = document.getElementById(id);
        VueScrollTo.scrollTo(element, 800, {container: '.recipe-create .base-layout-flex-row__content-inner__content'})
      },
    },
}
</script>

<style lang="scss" scoped>
.recipe-create-option-selector {
  width: 100%;
  min-height: 75vh;
}
</style>
