<template>
  <div class="recipe-create-choices w-full h-full flex flex-col">
      <baseButton
        v-if="choices.length > 1"
        :disabled="selectingPrevious || choices.length <= 1"
        dashed
        noshadow
        small
        class="recipe-create-choices-previous-btn pr-2"
        @click.prevent.native="selectPrevious()">
      <baseIcon>icofont-hand-drawn-left</baseIcon>
        {{ $t('previous') }}
      </baseButton>
    <div class="recipe-create-choices__inner w-full">
      <div id="line-container"></div>
        <template v-if="!forceUpdate">
          <BaseLine
            container="#line-container"
            v-for="(line, l) in lines"
            :key="l+'l'"
            :to="line.to"
            :from="line.from"
          />
      </template>
      <div
        v-for="(choice, i) in choices"
        :key="i"
        class="recipe-create-choices__inner-choice"
        :style="{'min-height': pageHeight+'px', 'padding-top': '75px'}">
          <RecipeCreateChoice
            :id="'choice-'+choice.id"
            class="h-full w-full px-2"
            v-model="choices[i].tagSelectableCategory"
            @selected="addChoice($event, choices[i].orderIndex)"
            v-if="!forceUpdate"
          />
      </div>
      <template v-if="isDone">
        <div class="h-full w-full flex flex-col justify-center items-center p-4 text-center" :style="{'min-height': pageHeight+'px'}">
          <h1 class="text-center">{{ $t('createRecipeTitle') }}.</h1>
          <baseButton class="mr-2" noshadow size="15" primary @click.prevent.native="$emit('createRecipeDraft', choices)">{{ $t('create') }} {{ $t('recipe') }}</baseButton>
        </div>
        <div id="create-recipe-choice"></div>
      </template>
    </div>
  </div>
</template>

<script>
import _ from "lodash";
import VueScrollTo  from 'vue-scrollto';
export default {
	name: 'RecipeCreateChoices',
	components: {
		RecipeCreateChoice: () => import('@/components/administration/recipeCreate/RecipeCreateChoice.vue'),
		BaseLine: () => import('@/components/base/BaseLine.vue'),
	},
  props: {
    value: [Object, Array],
  },
	data() {
		return {
      choices: [],
      lines: [],
      isDone: false,
      selectingPrevious: false,
      forceUpdate: false
		}
	},
  computed: {
    pageHeight() {
      var height = document.querySelector('.recipe-create')?.offsetHeight - 8;
      return height > 500 ? height : 500;
    }
  },
	methods: {
    selectPrevious() {
      this.selectingPrevious = true;
        var currentChouce = this.choices[this.choices.length-2];
        this.$emit('progressUpdate', currentChouce.orderIndex-1);
          this.scrollTo(`choice-${currentChouce.id}`, 400);
    },
    updatePrevious() {
       this.choices.length=(this.choices.length-1);
       this.lines.length=(this.lines.length-1);
        this.choices[this.choices.length-1].tagSelectableCategory.tagSelectables.map(_ => { _.selected = false; return _ })
        this.selectingPrevious = false;
        if(this.isDone) {
          this.isDone = false;
        }
        this.forceUpdate = true;
        this.$nextTick(() => {
          this.forceUpdate = false;
        })

    },
    addChoice(e, orderIndex) {
      this.isDone = false;
      this.$emit('progressUpdate', orderIndex);
      if(this.choices.length > orderIndex) {
       this.choices.length=orderIndex;
       this.lines.length=orderIndex-1;
      }
      if(e?.choice) {
        this.choices.push(_.cloneDeep(e.choice));
        this.$nextTick(() => {
          this.lines.push({id: orderIndex, to: `#choice-${e.choice.id}`, from: `#choice-option-${e.id}`});
          this.scrollTo(`choice-${e.choice.id}`);
        })
      }
      else {
        this.$emit('progressComplete');
        this.isDone = true;
        this.$nextTick(() => {
          this.scrollTo('create-recipe-choice');
        })
      }
    },
    scrollTo(el, speed) {
      var element = document.getElementById(el);
      var Vue = this;
      VueScrollTo.scrollTo(element, speed || 1000, {container: '.recipe-create .base-layout-flex-row__content-inner__content',
      onDone: function(e) {
          if(Vue.selectingPrevious) {
            Vue.updatePrevious();
          }
      },
      onCancel: function(e) {
        element.scrollIntoView();
          if(Vue.selectingPrevious) {
            Vue.updatePrevious();
          }
      },})
    },
  },
  mounted() {
    this.choices.push(_.cloneDeep(this.value[0]));
  },
  created() {
    this.$nextTick(() => {
        var linksContainer = document.querySelector('#line-container');
        var rectWrapper = linksContainer?.parentElement.getBoundingClientRect();
        linksContainer.style.transform = `translate(${-rectWrapper.left}px, ${-rectWrapper.top}px)`;
    })
  },
}
</script>

<style lang="scss" scoped>
.recipe-create-choices-previous-btn {
  position: absolute;
  top: 5px;
  left: 5px;
}
.recipe-create-choices {
  &__inner {
    &-choice {

    }
  }
}
</style>
