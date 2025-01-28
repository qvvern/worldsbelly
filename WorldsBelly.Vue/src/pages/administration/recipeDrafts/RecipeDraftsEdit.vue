<template>
<baseLayout>
  <template slot="content" v-if="isPageReady && get_recipe_draft">
  <div class="recipe-draft-page">
    <div class="recipe-draft-page__header py-6">
      <div class="max-w-screen-xl	px-4 m-auto flex flex-col">
        <div class="flex flex-wrap justify-between w-full mb-2">
          <div class="max-w-2xl" style="margin-right: -100px;">
            <ContentEditable v-model="get_recipe_draft.title" placeholder="Name of recipe" :limit="200"  >
              <h1 class="recipe-title max-w-2xl" />
            </ContentEditable>
            <ContentEditable v-model="get_recipe_draft.summary" placeholder="Describe the taste of your recipe" :limit="250">
              <p class="recipe-summary max-w-2xl" />
            </ContentEditable>
          </div>
          <div class="pt-4">
            <div class="flex">
                <RecipeDraftsEditSelectCountry
                  class="pt-2"
                  v-model="get_recipe_draft.originCountryId"
                />
                <BaseSelect
                  labelTop
                  class="pt-2"
                  :label="$t('bestServed')"
                  :placeholder="$t('select')"
                  v-model="get_recipe_draft.bestServedId"
                  :items="get_recipe_best_served"
                  itemText="label"
                  itemValue="id"
                  outlined
                  height="55px"
                  style="width: 150px"
                  right
                />
                <BaseSelect
                  labelTop
                  class="pt-2"
                  :label="$t('difficulty')"
                  :placeholder="$t('select')"
                  v-model="get_recipe_draft.difficultyId"
                  :items="get_recipe_difficulty"
                  itemText="label"
                  itemValue="id"
                  outlined
                  height="55px"
                  style="width: 120px"
                  right
                />
                <RecipeDraftsEditRating :value="0" :createdByUser="createdByUserId"/>
            </div>
          </div>
        </div>
        <div class="flex w-full justify-between">
          <div class="image-container mr-3">
            <div class="image-container__main">
              <RecipeDraftsEditImageUpload v-model="get_recipe_draft.imageUrl" />
            </div>
            <div class="image-container__sub flex flex-col">
              <!-- <div class="image-container__sub-image mb-1">
                <baseIcon value="icofont-image" />
                <p>Add Image</p>
              </div> -->
              <RecipeDraftsAddVideo v-model="get_recipe_draft.videoUrl">
                <div class="image-container__sub-image">
                  <baseIcon value="icofont-ui-video-chat" />
                  <p>{{get_recipe_draft.videoUrl ? `${$t('change')} ${$t('video')}` : `${$t('add')} ${$t('video')}`}}</p>
                </div>
              </RecipeDraftsAddVideo>
            </div>
          </div>
            <baseBox title="Nutrients per serving" style="opacity: .75; min-width: 300px;">
                {{$t('nutrientsNotCalculated')}}
            </baseBox>
        </div>
        <div class="flex justify-between w-full my-4">
            <div class="cell">
                <div class="flex">
                  <template v-for="(tag, i) in get_recipe_draft.tags">
                    <v-chip :key="i" class="mr-2"  small v-tooltip="tag.description">{{tag.name}}</v-chip>
                  </template>
                  <!-- <RecipeDraftsCustomTags v-model="get_recipe_draft.userTags" /> -->
                </div>
                <p class="pt-3" style="font-size: 12px">{{ $t('recipeBy') }} <br><b>{{get_recipe_draft.createdByUser.username}}</b></p>
            </div>
          <div>
            <div class="flex">
              <baseBox :title="$t('totalTime')" class="mr-4" style="opacity: .75" v-if="get_recipe_draft.tags.every(t => t.id != 943)">
                  {{ Number(get_recipe_draft.totalCookingTime) + Number(get_recipe_draft.totalPrepTime) }} {{$t('mins')}}
              </baseBox>
              <baseBox :title="$t('cookingTime')" class="mr-4" v-if="get_recipe_draft.tags.every(t => t.id != 943)">
                <ContentEditable v-model="get_recipe_draft.totalCookingTime" placeholder="0" number :limit="10">
                  <span class="p-1 pl-0" /><b>{{$t('mins')}}</b>
                </ContentEditable>
              </baseBox>
              <baseBox :title="$t('prepTime')">
                <ContentEditable v-model="get_recipe_draft.totalPrepTime" placeholder="0" number :limit="10">
                  <span class="p-1 pl-0" /><b>{{$t('mins')}}</b>
                </ContentEditable>
              </baseBox>
            </div>
          </div>
        </div>
        <div class="flex flex-wrap w-full my-4">
          <RecipeDraftsEditTastebuds
              v-bind="get_recipe_draft"
              draft
              @update="get_recipe_draft[$event.property] = $event.value" />
        </div>
      </div>
    </div>
    <div class="recipe-draft-page__content">
        <div class="recipe-draft-page__body flex w-full h-full">
          <div class="recipe-ingredients">
            <IngredientList
              :recipeServings="get_recipe_draft.servingAmount"
               @setRecipeServings="get_recipe_draft.servingAmount = $event"
               v-model="get_recipe_draft.ingredientLists"
               @rememberIngredients="rememberIngredients = $event"
               >
               </IngredientList>
          </div>
          <div class="recipe-content">
            <baseLoading v-if="isSaving" :value="true" :progressSentences="[`${$t('saving')} ${$t('recipe')}`]" />
            <div v-else class="recipe-content-step-wrapper" v-for="(step, i) in get_recipe_draft.steps" :key="i">
              <div class="recipe-content-step">
                <ContentEditable v-model="get_recipe_draft.steps[i].title" :placeholder="`${$t('step')} 1`" emitPlaceholder :limit="20" top="18">
                  <h1 class="mt-2 ml-3 p-2" style="line-height: normal" />
                </ContentEditable>
                <Editor
                  ref="Editor"
                  v-model="get_recipe_draft.steps[i].content"
                  :ingredientLists.sync="get_recipe_draft.ingredientLists"
                  :rememberIngredients.sync="rememberIngredients"
                  :placeholder="$t('addStepText')"
                  :showBar="showBar == i ? true : true"
                  @onAllIngredientsAdded="allIngredientsAdded = $event"
                  />
              </div>
              <RecipeDraftsEditImageUploadStep v-model="get_recipe_draft.steps[i].imageUrl" :video.sync="get_recipe_draft.steps[i].videoUrl" v-slot="{ uploadImage }" @removeVideo="get_recipe_draft.steps[i].videoUrl = null">
                <div class="recipe-content-step-actions flex justify-center mt-2">
                  <div class="recipe-content-step-actions__btn mr-2" @click="uploadImage" :class="{disabled: get_recipe_draft.steps[i].imageUrl}">
                    <baseIcon value="icofont-image" />
                    <p>{{$t('add')}} {{$t('image')}}</p>
                  </div>
                  <RecipeDraftsAddVideo v-model="get_recipe_draft.steps[i].videoUrl" :class="{'pointer-events-none': get_recipe_draft.steps[i].videoUrl}">
                    <div class="recipe-content-step-actions__btn mr-2" :class="{disabled: get_recipe_draft.steps[i].videoUrl}">
                      <baseIcon value="icofont-ui-video-chat" />
                      <p>{{$t('add')}} {{$t('video')}}</p>
                    </div>
                  </RecipeDraftsAddVideo>
                  <div class="recipe-content-step-actions__btn" @click="removeStep(i)" :class="{disabled: get_recipe_draft.steps.length == 1}">
                    <baseIcon value="icofont-close" />
                    <p>{{$t('remove')}}</p>
                  </div>
                </div>
              </RecipeDraftsEditImageUploadStep>
            </div>
            <div class="recipe-content-actions flex justify-center">
                <baseButton dashed @click.prevent.native="addStep(get_recipe_draft.steps.length)" :size="14" icon="icofont-plus" >{{$t('add')}} {{$t('newStep')}}</baseButton>
            </div>
          </div>
        </div>
    </div>
      <RecipeDraftsEditActions
      :allIngredientsAdded="allIngredientsAdded"
      @onSave="isSaving = $event"
      @onDelete="isDeleting = $event"
      @onPublish="isPublishing = $event"
      :recipe.sync="get_recipe_draft"
      />
  </div>
  </template>
  <template v-else slot="content">
    <template v-if="isPublished">
      <div class="flex items-center justify-center py-8">
        <h3>{{$t('cannotEditRecipeAlreadyPublished')}}</h3>
      </div>
    </template>
    <BaseProgressLinear v-else></BaseProgressLinear>
  </template>
  <template slot="loader">
      <baseLoading v-if="isDeleting || isPublishing" :value="true" overlay parent fade :progressSentences="[(isPublishing ? `${$t('publishing')} ${$t('recipe')}}` : `${$t('deleting')} ${$t('recipe')}}`)]" />
    </template>
</baseLayout>
</template>

<script>
import { EventBus } from '@/event-bus'
import { mapActions, mapGetters } from 'vuex'
import _ from "lodash";
export default {
	name: 'RecipeDraftsEdit',
	components: {
		IngredientList: () => import('@/components/ingredientList/IngredientList.vue'),
		Editor: () => import('@/components/administration/editor/Editor.vue'),
		ContentEditable: () => import('@/components/administration/editor/ContentEditable.vue'),
    RecipeDraftsEditSelectCountry: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsEditSelectCountry.vue'),
		RecipeDraftsEditRating: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsEditRating.vue'),
		RecipeDraftsEditTastebuds: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsEditTastebuds.vue'),
		RecipeDraftsEditActions: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsEditActions.vue'),
		// RecipeDraftsCustomTags: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsCustomTags.vue'),
		RecipeDraftsEditImageUpload: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsEditImageUpload.vue'),
		RecipeDraftsAddVideo: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsAddVideo.vue'),
		RecipeDraftsEditImageUploadStep: () => import('@/components/administration/recipeDraftsEdit/RecipeDraftsEditImageUploadStep.vue')
	},
	data() {
		return {
        rememberIngredients: [],
        isDeleting: false,
        isPublishing: false,
        isPublished: false,
        isSaving: false,
        isPageReady: false,
        hoverImage: false,
        showBar: null,
        allIngredientsAdded: false
		}
	},
  computed: {
		...mapGetters('recipe/', ['get_recipe_draft', 'get_recipe_difficulty', 'get_recipe_best_served']),
    createdByUserId() {
      return this.get_recipe_draft?.createdByUser?.id;
    }
  },
  methods: {
		...mapActions('recipe/', ['fetch_recipe_draft', 'delete_recipe_draft']),
    addStep(index) {
      this.get_recipe_draft.steps.push({
        orderIndex : (index+1),
        title: 'Step '+ (index+1),
        content: "Description here",
      });
    },
    removeStep(index) {
      this.get_recipe_draft.steps.splice(index, 1);
      var cloneSteps = _.cloneDeep(this.get_recipe_draft.steps)
      this.get_recipe_draft.steps = [];
      this.$nextTick(() => {
        this.get_recipe_draft.steps = cloneSteps
      })
    },
    syncIngredientsSelector() {
        for (let index = 0; index < this.get_recipe_draft?.steps.length; index++) {
          try {
            this.$refs.Editor[index].syncIngredientsSelector();
          }
          catch {
            setTimeout(() => {
              this.$nextTick(() => {
                this.$refs.Editor[index]?.syncIngredientsSelector();
              })
            }, 200);
          }
        }
    },
    syncIngredientsEditor() {
        for (let index = 0; index < this.get_recipe_draft?.steps.length; index++) {
          try {
            this.$refs.Editor[index].syncIngredientsEditor();
          }
          catch {
            setTimeout(() => {
              this.$nextTick(() => {
                this.$refs.Editor[index]?.syncIngredientsEditor();
              })
            }, 200);
          }
        }
    },
  },
  mounted() {
      if (!EventBus._events.onSyncIngredientsSelector) {
        EventBus.$on('onSyncIngredientsSelector', this.syncIngredientsSelector)
      }
      if (!EventBus._events.onSyncIngredientsEditor) {
        EventBus.$on('onSyncIngredientsEditor', this.syncIngredientsEditor)
      }
  },
  created() {
      this.fetch_recipe_draft(this.$route.params.id).then(() => {
        if(!this.get_recipe_draft.isPublished) this.isPageReady = true;
        else this.isPublished = true;
      });
  }
}
</script>

<style lang="scss" scoped>
.recipe-draft-page {
  position: relative;
  height: 100%;
  width: 100%;
  &__header {
    width: 100%;
    // background: #f8fafc;
    background: -webkit-linear-gradient(top, #f8fafc, #fafafa 100%);
    // -webkit-box-shadow: inset 0 20px 20px -20px rgba(0,0,0,0.2);
    // -moz-box-shadow: inset 0 20px 20px -20px rgba(0,0,0,0.2);
    // box-shadow: inset 0 20px 20px -20px rgba(0,0,0,0.2);
    // border-bottom: 1px solid var(--border-color);
    &-bar {
      // border-top: 1px solid var(--border-color);
      // border-bottom: 1px solid var(--border-color);
    }
    .image-container {
      width: 100%;
      display: flex;
        // background: #f2f2f2;
      // background: #e5e7e9;
      // padding: 20px;
      &__main {
        position: relative;
        box-shadow: #9e9e9e24 0px 0px 6px 0px;
        background: #f2f2f2a1; //#f2f2f2;
        width: 100%;
        height: 350px;
        display: flex;
        align-items: center;
        justify-content: center;
        overflow: hidden;
        flex-direction: column;
        position: relative;
      }
      &__sub {
        &-image {
          display: flex;
          align-items: center;
          justify-content: center;
          flex-direction: column;
          border: 2px dotted var(--border-color);
          border-left: 0px;
          border-top-right-radius: 10px;
          border-bottom-right-radius: 10px;
          background: #fff;
          width: 75px;
          height: 75px;
          p {
            padding-top: 2px;
            font-size: 10px;
          }
          &:hover {
            cursor: pointer;
            border: 2px solid rgb(31, 29, 29);
            border-left: 0px;
            transition: all 400ms;
            background: rgb(63, 184, 15);
            * {
              color: white;
            }
          }
        }
      }
    }
  }
  &__content {
    overflow: hidden;
    .recipe-ingredients {
      position: sticky;
      margin-left: 20px;
      margin-top: 20px;
      border-top: 3px solid var(--border-color);
      border-left: 3px solid var(--border-color);
      border-right: 3px solid var(--border-color);
      border-top-left-radius: 50px;
      border-top-right-radius: 50px;
      // box-shadow: 1px -1px 22px -2px rgba(37,64,92,0.18);
      // -webkit-box-shadow: 1px -1px 22px -2px rgba(37,64,92,0.18);
      // -moz-box-shadow: 1px -1px 22px -2px rgba(37,64,92,0.18);
      padding: 15px 0px;
      max-width: 475px;
      min-width: 475px;
    }
    .recipe-content {
      padding: 40px;
      width: 56%;
      &-step-wrapper {
        width: 100%;
        border: 1px dashed rgba(0,0,0,.25);
        margin-bottom: 20px;
        border-radius: 15px;
        &:hover {
          border: 1px dashed rgba(0,0,0,1);
          transition: all 400ms;
        }
      }
      &-step {
        width: 100%;
        padding: 0px;
        overflow: hidden;
        word-break: break-all;
      }
      &-step-actions {
        margin-bottom: 20px;
        // padding: 10px;
        &__btn {
          display: flex;
          align-items: center;
          justify-content: center;
          flex-direction: column;
          border: 1px dashed rgba(0,0,0,.25);
          border-radius: 15px;
          width: 65px;
          height: 65px;
          &.disabled {
            pointer-events: none;
            opacity: .75;
            background: rgb(228, 228, 228);
          }
          p {
            padding-top: 2px;
            font-size: 10px;
          }
          &:hover {
            cursor: pointer;
            border: 1px dashed rgba(0,0,0,1);
            transition: all 400ms;
            background: #fff;
          }
        }
      }
    }
  }
}
</style>
