<template>
  <div class="recipe-create-choice-selector">
    <div class="recipe-create-choice-selector__title">
      <h1>
        {{ value.title }}
      </h1>
      <p>{{ value.text }}</p>
    </div>
    <div class="recipe-create-choice-selector__options" v-if="!isUpdating">
          <RecipeCreateChoiceOption
            :id="`choice-option-${option.id}`"
            class="m-2"
            v-for="(option, i) in sortedOptions"
            :key="i+'e'"
            v-bind="option.tag"
            :selected="option.selected"
            :icon="option.icon"
            :yellow="option.dontAddTag"
            @click.prevent.native="handleSelect(option)"
          />
    </div>
  </div>
</template>

<script>
import _ from "lodash";

export default {
	name: 'RecipeCreateChoice',
  props: {
    value: Object,
    orderIndex: Number
  },
	data() {
		return {
      isUpdating: false
      // country: 20,
		}
	},
	components: {
		RecipeCreateChoiceOption: () => import('@/components/administration/recipeCreate/RecipeCreateChoiceOption.vue'),
	},
  computed: {
    sortedOptions() {
      return this.value?.tagSelectables;
    }
  },
	methods: {
    handleSelect(selected) {
      this.$emit('selected', selected);
      this.value.tagSelectables.map((e) => {
        if(selected.id == e.id) {
          e.selected = true;
        }
        else {
          e.selected = false;
        }
        return e;
      })
      this.isUpdating = true;
      this.$nextTick(() => {
        this.isUpdating = false;
      })
    }
	},
}
</script>

<style lang="scss" scoped>
  // .recipe-create-choice-selector-active {
  //   pointer-events: auto !important;
  // }
  // .recipe-create-choice-selector-inactive {
  //   opacity: .1 !important;
  // }
  .recipe-create-choice-selector {
    // pointer-events: none;
    &__title {
      display: grid;
      h1 {
        color: #1d1d1f;
        font-style: normal;
        -webkit-font-smoothing: antialiased;
        direction: ltr;
        text-align: center;
        margin: 0;
        padding: 0;
        font-size: 34px;
        font-weight: 600;
        margin-top: 40px;
      }
      p {
        color: #1d1d1f;
        font-style: normal;
        -webkit-font-smoothing: antialiased;
        direction: ltr;
        text-align: center;
        font-size: 18px;
        font-weight: 400;
        font-family: 'Open Sans';
        letter-spacing: .011em;
        display: inline;
        padding-top: 10px;

      }
    }
    &__options {
      display: flex;
      align-items: center;
      justify-content: center;
      flex-wrap: wrap;
      padding: 20px;
      margin-top: 40px;
    }
  }
</style>
