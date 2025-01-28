<template>
  <baseLayout class="recipe-draft">
    <div slot="content" class="recipe-draft__body" v-if="isPageReady">
        <v-toolbar
                class="recipe-draft-toolbar"
                dense
                color="#080825"
                dark
                extended
                extension-height="300"
                flat
        >
            <!-- <img class="image" src="@/assets/images/background.jpg"/> -->
            <v-toolbar-title class="mx-auto" slot="extension">
                <span>{{ $t('YouHaveTotalOf') }} {{recipeDrafts.length}}</span>
                <h2>{{ $t('recipesInDraft') }}</h2>
                <v-layout row class="p-3 mt-1 flex items-center">
                    <v-flex wrap>
                            <baseIcon value="icofont-food-basket" size="20" color="#b3b3b3" />
                    </v-flex>
                    <v-flex wrap>
                            <baseIcon value="icofont-bbq" size="25" color="#b3b3b3" />
                    </v-flex>
                    <v-flex wrap>
                            <baseIcon value="icofont-dining-table" size="35" color="#b3b3b3" />
                    </v-flex>
                </v-layout>
            </v-toolbar-title>
        </v-toolbar>
        <template  v-if="recipeDrafts.length > 0">
          <div class="w-full flex flex-wrap p-3 justify-center" style="margin-top: -50px">
              <RecipeCard class="p-3" v-for="(draft, i) in recipeDrafts" :key="i" @click.prevent.native="openDraft(draft)" :value="draft" />
          </div>
        </template>
        <template v-else>
          <div class="w-full flex flex-wrap p-3 justify-center">
            <div class="drawing">
              <div class="chef-hat">
                <div class="chef-hat__ruffle"></div>
                <div class="chef-hat__ruffle"></div>
                <div class="chef-hat__ruffle"></div>
              </div>
            </div>
              <div class="flex w-full flex-col flex-wrap p-3 justify-center items-center">
                  <template v-if="recipePublished.length > 0">
                    <baseButton thick @click.prevent.native="$localeRouter.push({name: 'RecipePublished'})">{{ $t('SeeYourPublishedRecipes') }}</baseButton>
                    <h4 class="pb-2 my-2">{{$t('youHave')}} {{recipePublished.length == 1 ? $t('recipe') : $t('recipes') }} {{$t('published')}}!</h4>
                    <baseButton tertiary @click.prevent.native="$localeRouter.push({name: 'RecipeCreate'})">{{ $t('makeAnotherRecipe') }}</baseButton>
                  </template>
                  <template v-else>
                    <baseButton thick @click.prevent.native="$localeRouter.push({name: 'RecipeCreate'})">{{ $t('getStarted') }}</baseButton>
                    <h4 class="pb-2 my-2">{{ $t('YouHaveMadeNoRecipes') }}</h4>
                  </template>
              </div>
          </div>
        </template>
    </div>
    <template slot="loader">
        <baseLoading v-if="isloading" :value="true" overlay parent fade :progressSentences="[`${$t('preparing')} ${$t('draft')}`]" />
        <baseLoading v-else-if="!isPageReady" :value="true" overlay parent fade :progressSentences="[`${$t('preparing')} ${$t('recipes')}`]" />
    </template>
  </baseLayout>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
export default {
	name: 'RecipeDrafts',
	components: {
		RecipeCard: () => import('@/components/base/RecipeCard.vue'),
	},
	data() {
		return {
        isPageReady: false,
        isloading: false
		}
	},
  computed: {
    recipeDrafts() {
      return this.get_recipe_drafts && this.get_recipe_drafts.length > 0 && this.get_recipe_drafts.some(_ => _.isPublished != true) ?
      this.get_recipe_drafts.filter(_=> _.isPublished != true) : [];
    },
    recipePublished() {
      return this.get_recipe_drafts && this.get_recipe_drafts.length > 0 && this.get_recipe_drafts.some(_ => _.isPublished == true) ?
      this.get_recipe_drafts.filter(_=> _.isPublished == true) : [];
    },
		...mapGetters('recipe/', ['get_recipe_drafts']),
  },
  methods: {
		...mapActions('recipe/', ['fetch_recipe_drafts']),
    openDraft(draft) {
      this.isloading = true;
      setTimeout(() => {
        this.$nextTick(() => {
          window.location =  `${window.location.origin}/${this.$language.code}/administration/drafts/${draft.id}`;
        })
      }, 100);
    }
  },
  created() {
    if(this.get_recipe_drafts.length > 0) {
      this.isPageReady = true;
      return;
    }
    this.fetch_recipe_drafts().finally(() => {
      this.isPageReady = true;
    })
  }
}
</script>
<style lang="scss" scoped>
.drawing {

  .chef-hat {
    position: relative;
    background: #EFEFEF;
    height: 60px;
    width: 100px;
    box-shadow: -8px 0 0 0 #AAA inset, -48px 0 0 0 #CCC inset;
    border-radius: 0 0 8px 8px;
    // transform: rotate(-22.5deg);
  }

  $i: 0;
  $ruffles: ((60px, 60px, 0, -20px, translate(0, -75%)), (80px, 80px, 0, 50%, translate(-50%, -75%)), (60px, 60px, 0, 100%, translate(-40px, -75%)));
  @each $height,
  $width,
  $x,
  $y,
  $transform in $ruffles {
    $i: $i + 1;
    .chef-hat__ruffle:nth-child(#{$i}) {
      position: absolute;
      height: $height;
      width: $width;
      background: #EFEFEF;
      top: $x;
      left: $y;
      transform: $transform;
      border-radius: 60px;
      box-shadow: -4px -8px 0 0 #AAA inset, -8px -16px 0 0 #CCC inset;
    }
  }

}
</style>
<style scoped>
    .recipe-draft-toolbar {
           background: -webkit-linear-gradient(top, #d7621d, #8b0e0e 100%) !important;
    }
    .outine-2 {
        border: 2px solid white;
    }
    .card--flex-toolbar {
        margin-top: -124px;
    }
    .learn-more-btn {
        text-transform: initial;
        text-decoration: underline;
    }
</style>
