<template>
    <v-menu
        v-model="menu"
        transition="slide-y-transition"
        open-on-click
        bottom
        left
        :close-on-content-click="!canRate"
        :z-index="999999"
        >
        <template v-slot:activator="{ on, attrs }">
            <div
                style="text-align: right; padding-right: 22px"
                class="pb-0 cursor-pointer"
                v-bind="attrs"
                v-on="on"
            >
                <p style="font-size: 12px; color: rgba(0, 0, 0, 0.6)">{{ $t('rating') }}</p>
                <div class="flex items-center">
                    <baseIcon value="icofont-ui-rating" :size="16" color="#ffc416" />
                    <p class="pl-2">
                        <b>{{ localRating ? localRating : value }}</b> / 10
                    </p>
                </div>
            </div>
        </template>

        <v-card>
            <v-list-item three-line class="recipe-rating-popup" :class="{'py-4': canRate}">
                <v-list-item-content>
                    <v-list-item>
                        <template v-if="!$msal.isAuthenticated">
                            <p>{{ $t('ratingForm.unSignedIn') }}</p>
                        </template>
                        <template v-else-if="get_active_user && get_active_user.id == createdByUser">
                            <p>{{ $t('ratingForm.ownRecipe') }}</p>
                        </template>
                        <div v-else-if="userRating" class="flex flex-col text-center">
                            <h4 v-if="!userRating.rating">{{ $t('ratingForm.rateRecipe') }}</h4>
                            <h4 v-else>{{ $t('ratingForm.changeRating') }}</h4>
                            <v-rating
                              class="my-4"
                              hover
                              length="10"
                              size="22"
                              v-model="userVal"
                              color="yellow darken-1"
                              background-color="grey darken-1"
                            />
                            <div class="w-full flex justify-center">
                            <baseButton
                                tertiary
                                class="mr-2"
                                size="12"
                                height="30px"
                                @click.prevent.native="menu = false"
                            >
                                {{ $t('close') }}
                            </baseButton>
                            <baseButton
                                tertiary
                                v-if="userRating.rating"
                                class="mr-2"
                                size="12"
                                height="30px"
                                style="background: #e6e6e6"
                                icon="icofont-close"
                                @click.prevent.native="handleRating(null)"
                            >
                                {{ $t('withdraw') }} {{ $t('rating') }}
                            </baseButton>
                            <baseButton
                                tertiary
                                white
                                class="mr-2"
                                size="12"
                                height="30px"
                                @click.prevent.native="handleRating(userVal)"
                            >
                                {{ $t('submit') }} {{ $t('rating') }}
                            </baseButton>
                            </div>
                        </div>
                    </v-list-item>
                </v-list-item-content>
            </v-list-item>
        </v-card>
    </v-menu>
</template>

<script>
import { mapGetters } from "vuex";
import _ from "lodash";
export default {
    name: "RecipeDraftsEditRating",
    props: {
        value: [Number, String],
        createdByUser: Number,
        userRating: Object,
    },
    data() {
        return {
            userVal: 0,
            menu: false,
            localRating: null
        };
    },
    mounted() {
        this.inputValue = _.cloneDeep(this.value);
    },
    computed: {
        ...mapGetters("user/", ["get_active_user"]),
        canRate() {
            return this.get_active_user && this.get_active_user.id != this.createdByUser ? true : false;
        },
    },
    methods: {
        handleRating(rating) {
            this.localRating = rating;
            this.$emit('rating', rating);
            this.menu = false;
        },
    },
    watch: {
      userRating: {
        handler() {
          if(this.userRating?.rating != null && this.userRating?.rating != this.userVal) {
            this.userVal = _.clone(this.userRating.rating);
          }
          else if(this.userRating?.rating == null) {
            this.userVal = 0;
          }
        }, immediate: true, deep: true
      }
    }
};
</script>
<style lang="scss">
.recipe-rating-popup {
    background: #fafafa;
    h4 {
        color: #111;
        font-family: "Helvetica Neue", "Open Sans", sans-serif;
        font-size: 18px;
        font-weight: bold;
        letter-spacing: -1px;
        line-height: 1;
        text-align: center;
    }
    p {
        color: #000000;
        font-family: "Helvetica Neue", sans-serif;
        font-size: 14px;
        line-height: 24px;
        margin: 0 0 24px;
        text-align: justify;
        text-justify: inter-word;
    }
}
</style>
