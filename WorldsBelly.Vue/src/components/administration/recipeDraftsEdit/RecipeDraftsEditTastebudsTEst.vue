<template>
  <div class="w-full" title="">
      <!-- <h5>Define taste of recipe</h5> -->
      <div class="grid grid-rows-2 grid-flow-col gap-3" v-if="!isRefreshing">
          <BaseSlider
            title=""
            labelsFollow
            :value="seeRating ? userVal.sweet : sweet"
            @input="updateUserRating('sweet', $event)"
            label="Sweet"
            :labels="['neutral', 'mild', 'semi-sweet', 'sweet', 'very sweet']"
            :disabled="!$msal.isAuthenticated || (!draft && get_active_user && get_active_user.id == createdByUser)"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="seeRating ? userVal.sour : sour"
            @input="updateUserRating('sour', $event)"
            label="Sour"
            :labels="['neutral', 'mild', 'semi-sour', 'sour', 'very sour']"
            :disabled="!$msal.isAuthenticated || (!draft && get_active_user && get_active_user.id == createdByUser)"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="seeRating ? userVal.salty : salty"
            @input="updateUserRating('salty', $event)"
            label="Salty"
            :labels="['neutral', 'mild', 'semi-salty', 'salty', 'very salty']"
            :disabled="!$msal.isAuthenticated || (!draft && get_active_user && get_active_user.id == createdByUser)"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="seeRating ? userVal.bitter : bitter"
            @input="updateUserRating('bitter', $event)"
            label="Bitter"
            :labels="['neutral', 'mild', 'semi-bitter', 'bitter', 'very bitter']"
            :disabled="!$msal.isAuthenticated || (!draft && get_active_user && get_active_user.id == createdByUser)"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="seeRating ? userVal.spices : spices"
            @input="updateUserRating('spices', $event)"
            label="Herbs & Spices"
            :labels="['neutral', 'mild', 'spiceful', 'Spicy', 'very spicy']"
            :disabled="!$msal.isAuthenticated || (!draft && get_active_user && get_active_user.id == createdByUser)"
          />
          <BaseSlider
            title=""
            labelsFollow
            :value="seeRating ? userVal.flavor : flavor"
            @input="updateUserRating('flavor', $event)"
            label="Flavor"
            :labels="['neutral', 'mild', 'moderate', 'flavorful', 'umami']"
            :disabled="!$msal.isAuthenticated || (!draft && get_active_user && get_active_user.id == createdByUser)"
          />
      </div>
      <div class="flex justify-end w-full pt-3" v-if="!draft && get_active_user && get_active_user.id != createdByUser">
          <baseButton
              v-if="hasRatedBefore && !isSubmitted"
              class="mr-2"
              size="12"
              height="30px"
              :icon="seeRating ? 'bi-eye-slash' : 'bi-eye'"
              tertiary
              @click.prevent.native="seeRating = !seeRating"
          >
              {{seeRating ? 'Hide your rating' : 'See your rating'}}
          </baseButton>
          <baseButton
            title=""
              v-if="hasRatedBefore && seeRating"
              class="mr-2"
              size="12"
              height="30px"
              icon="icofont-close"
              tertiary
              @click.prevent.native="handleRating(true)"
          >
              Withdraw rating
          </baseButton>
          <baseButton
              title=""
              v-if="isAnyUpdatedTrue && ((!this.seeRating && !this.hasRatedBefore) || (this.seeRating && this.hasRatedBefore) || draft)"
              white
              class="mr-2"
              size="12"
              height="30px"
              outlined
              blue
              @click.prevent.native="handleRating()"
          >
              Submit rating
          </baseButton>
      </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import _ from "lodash";
  export default {
    name: "RecipeDraftsEditTastebuds",
    props: {
      salty: [Number, String],
      sour: [Number, String],
      spices: [Number, String],
      flavor: [Number, String],
      bitter: [Number, String],
      sweet: [Number, String],
      rating: [Number, String],
      recipeId: [Number, String],
      createdByUser: Number,
      userRating: Object,
      draft: Boolean
    },
    data() {
      return {
        seeRating: false,
        userVal: null,
        isUpdated: {
          salty: false,
          sour: false,
          spices: false,
          flavor: false,
          bitter: false,
          sweet: false,
        },
        isRefreshing: false,
        isSubmitted: false
      }
    },
    computed: {
      ...mapGetters("user/", ["get_active_user"]),
      hasRatedBefore() {
        if(this.userRating.salty != null || this.userRating.sour != null || this.userRating.spices != null || this.userRating.flavor != null || this.userRating.bitter != null || this.userRating.sweet != null ) {
          return true;
        }
        return false;
      },
      isAnyUpdatedTrue() {
        const areTrue = Object.values(this.isUpdated).some(
          value => value == true
        );
       return areTrue
      }
    },
    methods: {
      updateUserRating(property, value) {
        if(!this.userVal || (!this.seeRating && this.hasRatedBefore)) return;
        this.userVal[property] = value;
        if((value != this.userRating[property]) && value != 1) {
          this.isUpdated[property] = true;
        }
        else {
          this.isUpdated[property] = false;
        }
      },
      handleRating(widthdrawRating) {
        this.isSubmitted = true;
        var newRating = {
          rating: this.rating,
          recipeId: this.recipeId
        }
        if(widthdrawRating) {
          this.seeRating = false;
          newRating.salty = null;
          newRating.sour = null;
          newRating.spices = null;
          newRating.flavor = null;
          newRating.bitter = null;
          newRating.sweet = null;
          this.$emit('rating', newRating);
          this.isRefreshing = true;
          this.$nextTick(() => {
            this.isRefreshing = false;
          })
        }
        else {
          newRating.salty = this.userVal.salty;
          newRating.sour = this.userVal.sour;
          newRating.spices = this.userVal.spices;
          newRating.flavor = this.userVal.flavor;
          newRating.bitter = this.userVal.bitter;
          newRating.sweet = this.userVal.sweet;
          this.$emit('rating', newRating)
        }
        if(this.draft) {
          this.$emit('rating', newRating)
        }
        else {
          this.isUpdated = {};
        }
      }
    },
    created() {
      this.userVal = _.clone(this.userRating);
    },
    watch: {
      userVal: {
        handler() {
          if(this.draft) {
            this.handleRating();
          }
        }, deep: true
      },
      userRating: {
        handler() {
          if(!this.draft) {
            this.userVal = _.clone(this.userRating);
          }
        }, deep: true
      }
    }
  };
</script>
<style lang="scss">
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
