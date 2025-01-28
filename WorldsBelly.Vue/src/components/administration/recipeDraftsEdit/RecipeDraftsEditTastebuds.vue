<template>
  <div class="w-full" title="">
      <!-- <h5>Define taste of recipe</h5> -->
      <div class="grid grid-rows-2 grid-flow-col gap-3" v-if="!isRefreshing" :class="{'recipe-flavors': !draft}">
          <BaseSlider
            title=""
            labelsFollow
            :value="sweet"
            @input="updateUserRating('sweet', $event)"
            :label="$t('flavors.sweet.label')"
            :labels="mobile ? null : sweetLabels"
            :disabled="!$msal.isAuthenticated || !draft"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="sour"
            @input="updateUserRating('sour', $event)"
            :label="$t('flavors.sour.label')"
            :labels="mobile ? null : sourLabels"
            :disabled="!$msal.isAuthenticated || !draft"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="salty"
            @input="updateUserRating('salty', $event)"
            :label="$t('flavors.salty.label')"
            :labels="mobile ? null : saltyLabels"
            :disabled="!$msal.isAuthenticated || !draft"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="bitter"
            @input="updateUserRating('bitter', $event)"
            :label="$t('flavors.bitter.label')"
            :labels="mobile ? null : bitterLabels"
            :disabled="!$msal.isAuthenticated || !draft"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="spices"
            @input="updateUserRating('spices', $event)"
            :label="$t('flavors.spices.label')"
            :labels="mobile ? null : spicesLabels"
            :disabled="!$msal.isAuthenticated || !draft"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="flavor"
            @input="updateUserRating('flavor', $event)"
            :label="$t('flavors.flavor.label')"
            :labels="mobile ? null : flavorLabels"
            :disabled="!$msal.isAuthenticated || !draft"
          />
      </div>
  </div>
</template>

<script>
  export default {
    name: "RecipeDraftsEditTastebuds",
    props: {
      salty: [Number, String],
      sour: [Number, String],
      spices: [Number, String],
      flavor: [Number, String],
      bitter: [Number, String],
      sweet: [Number, String],
      draft: Boolean,
      mobile: Boolean
    },
    data() {
      return {
        isRefreshing: false,
        sweetLabels: [this.$t('flavors.sweet.0'), this.$t('flavors.sweet.1'), this.$t('flavors.sweet.2'), this.$t('flavors.sweet.3'), this.$t('flavors.sweet.4')],
        sourLabels: [this.$t('flavors.sour.0'), this.$t('flavors.sour.1'), this.$t('flavors.sour.2'), this.$t('flavors.sour.3'), this.$t('flavors.sour.4')],
        saltyLabels: [this.$t('flavors.salty.0'), this.$t('flavors.salty.1'), this.$t('flavors.salty.2'), this.$t('flavors.salty.3'), this.$t('flavors.salty.4')],
        bitterLabels: [this.$t('flavors.bitter.0'), this.$t('flavors.bitter.1'), this.$t('flavors.bitter.2'), this.$t('flavors.bitter.3'), this.$t('flavors.bitter.4')],
        spicesLabels: [this.$t('flavors.spices.0'), this.$t('flavors.spices.1'), this.$t('flavors.spices.2'), this.$t('flavors.spices.3'), this.$t('flavors.spices.4')],
        flavorLabels: [this.$t('flavors.flavor.0'), this.$t('flavors.flavor.1'), this.$t('flavors.flavor.2'), this.$t('flavors.flavor.3'), this.$t('flavors.flavor.4')],
      }
    },
    methods: {
      updateUserRating(property, value) {
        this.$emit(`update`, {property: property, value: value})
      },
    },
  };
</script>
<style lang="scss">
.recipe-flavors {
  .v-slider__thumb {
    height: 6px !important;
    background: #e1b514 !important;
    width: 10px !important;
    border: 0px !important;
  }
}
.settings-slider {
    padding-top: 10px;
    // overflow: hidden;
    height: 35px;
  .v-slider__thumb-label {
    background: transparent !important;
    color: #000
  }
  .settings-slider__label {
    pointer-events: none;
    color: #091e32;
    font-size: 12px;
    padding-top: 30px;
    display: flex;
    align-items: center;
    width: 100px;
    justify-content: center;
  }
  .v-slider__thumb-label {
    pointer-events: none;
  }

}
</style>
