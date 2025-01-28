<template>
  <div class="recipe-drafts-edit-actions p-3">
    <BaseConfirmDialog ref="confirmDelete" @confirm="deleteRecipe()" />
    <BaseConfirmDialog
      ref="confirmPublish"
      @confirm="publishRecipe()"
      :title="$t('pleaseConfirm')"
      width="800px"
      :description="$t('beforePublishMessage')"
      :confirmBtn="$t('confirmPublishBtn')"
      :cancelBtn="$t('cancelPublishBtn')"
    />
    <baseButton floaty delete @click.prevent.native="$refs.confirmDelete.open()" class="mr-5" icon="icofont-close-line">{{$t('delete')}}</baseButton>
    <baseButton
      floaty
      class="mr-5"
      black
      large
      @click.prevent.native="saveRecipe()"
      :loading="isSaving"
      :disabled="isSaving"
    >
      {{ isSaving ? $t('saving') : $t('save') }}
    </baseButton>
    <span v-tooltip="publishMessage">
      <baseButton floaty :disabled="publishMessage != ''" @click.prevent.native="$refs.confirmPublish.open()">{{$t('publish')}}</baseButton>
    </span>
  </div>
</template>

<script>
import { EventBus } from '@/event-bus'
import _ from "lodash";
import { mapActions, mapGetters, mapMutations } from 'vuex'
  export default {
      name: "RecipeDraftsEditActions",
      props: {
        recipeRating: Object,
        recipe: Object,
        allIngredientsAdded: Boolean
      },
      data() {
        return {
          isSaving: false,
          isDeleting: false,
          invalid: {}
        }
      },
      computed: {
        ...mapGetters('recipe/', ['get_recipe_draft']),
        publishMessage() {
          var messages = [];
          if(this.recipe.title == null || this.recipe.title.length < 2) {
            messages.push(`- ${this.$t('title')}.`);
          }
          if(this.recipe.summary == null || this.recipe.summary.length < 2) {
            messages.push(`- ${this.$t('summary')}.`);
          }
          if(this.recipe.originCountryId == null) {
            messages.push(`- ${this.$t('cuisine')}.`);
          }
          if(this.recipe.difficultyId == null) {
            messages.push(`- ${this.$t('difficulty')}.`);
          }
          if(this.recipe.bestServedId == null) {
            messages.push(`- ${this.$t('bestServed')}.`);
          }
          if(this.recipe.imageUrl == null) {
            messages.push(`- ${this.$t('pleaseUploadImage')}.`);
          }
          if(this.recipe.totalCookingTime == 0 && this.recipe.totalPrepTime == 0) {
            messages.push(`- ${this.$t('Cooking/Prep')}.`);
          }
          if(this.recipe.servingAmount == 0) {
            messages.push(`- ${this.$t('noServingAmount')}`);
          }
          if(this.recipe.ingredientLists.some(_ => _.ingredients.length < 1) || this.recipe.ingredientLists.some(_ => _.ingredients.some(i => i.ingredientId == null))) {
            messages.push(`- ${this.$t('noIngredients')}.`);
          }
          if(!this.allIngredientsAdded) {
            messages.push(`- ${this.$t('noIngredientsInSteps')}.`);
          }
          if(!Object.values(this.invalid).every(value => value === false)) {
            messages.push(`- ${this.$t('noAmountInIngredients')}.`);
          }
          if(this.recipe.steps.some(_ => _.title == null) || this.recipe.steps.some(_ => _.content == null) || this.recipe.steps.some(_ => _.content == `Description here`)) {
            messages.push(`- ${this.$t('noDescriptioOfRecipe')}.`);
          }
          if(this.recipe.salty == 0 && this.recipe.sour == 0 && this.recipe.spices == 0 && this.recipe.flavor == 0 && this.recipe.bitter == 0 && this.recipe.sweet == 0) {
             messages.push(`- ${this.$t('NoFlavorsPicked')}..`);
          }

          if(messages.length > 0) {
            var msgString = messages.join("<br>")
            return this.$t('cannotPublish')+'<br><br>' + msgString
          }
          return "";
        }
      },
      methods: {
        ...mapActions('recipe/', ['update_recipe_draft', 'delete_recipe_draft', 'publish_recipe_draft']),
        ...mapMutations('recipe/', ['set_recipe_draft']),
        saveRecipe() {
          this.isSaving = true;
          this.$emit('onSave', true);
            this.update_recipe_draft(this.get_recipe_draft).then(() => {
              this.$emit('onSave', false);
              this.isSaving = false;
            });
        },
        deleteRecipe() {
          this.$emit('onDelete', true);
          this.delete_recipe_draft(this.get_recipe_draft.id).then(() => {
            this.$localeRouter.push({ name: 'RecipeDraftsOverview' });
            this.set_recipe_draft(null);
          }).finally(() => {
            this.$emit('onDelete', false);
          });
        },
        publishRecipe() {
          this.$emit('onPublish', true);
          this.publish_recipe_draft(this.get_recipe_draft).then(() => {
              this.$localeRouter.push({ name: 'RecipePublished' });
              this.set_recipe_draft(null);
          }).finally(() => {
            this.$emit('onPublish', false);
          });
        },
        isSomeInvalid(editor) {
          this.invalid[editor.id] = editor.invalid
        }
      },
      mounted() {
        if (!EventBus._events.onIsSomeInvalid) {
          EventBus.$on('onIsSomeInvalid', this.isSomeInvalid)
        }
      }
  };
</script>

<style lang="scss" scoped>
.recipe-drafts-edit-actions {
  position: fixed;
  bottom: 15px;
  right: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
