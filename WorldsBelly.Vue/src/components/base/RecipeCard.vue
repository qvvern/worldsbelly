<template>
<div class="recipe-card-wrapper">
    <!-- <div class="recipe-card-user">
        <div class="recipe-card-user__icon"><baseIcon size="16" color="#15132b">icofont-ui-user</baseIcon></div>
        <p class="recipe-card-user__name">User</p>
    </div> -->
    <v-card class="recipe-card cursor-pointer p-0" @mouseenter="hover = true" @mouseleave="hover = false" :class="{hovered: hover}" width="290">
        <template slot="progress">
            <v-progress-linear color="deep-purple" height="10" indeterminate></v-progress-linear>
        </template>
        <div class="recipe-card__image">
          <div v-if="value.isPublished && !value.isApproved" class="status-label">
            {{ $t('awaitingApproval') }}
          </div>
          <!-- <div class="recipe-card-user">
              <div class="recipe-card-user__icon"><baseIcon size="16">icofont-ui-user</baseIcon></div>
              <p class="recipe-card-user__name">User</p>
          </div> -->
            <div class="recipe-card__image-tags" v-if="value.tags && value.tags.length > 0">
              <div v-for="(tag, i) in filteredTags" :key="i">{{tag.name}}</div>
            </div>
            <div class="recipe-card__image-overlay"></div>
            <v-img
                :height="200"
                v-if="value.imageUrl"
                :src="'https://worldsbellystorage.blob.core.windows.net' + value.imageUrl +'mainThumb'"

            ></v-img>
            <v-img :height="200" v-else src="@/assets/images/no-image-bg.png" lazy-src="@/assets/images/no-image-bg.png"></v-img>
        </div>

        <div class="flex flex-col w-full recipe-card-info">
            <div class="recipe-card__top">
                <div class="recipe-card-rating flex justify-between w-full">
                  <div class="flex flex-col">
                    <div class="flex items-start">
                      <baseIcon size="16" class="pr-1">icofont-clock-time</baseIcon>
                      <div class="recipe-card-time__number">{{value.totalTime}}</div>
                      <div class="recipe-card-time__text">{{ $t('mins') }}</div>
                    </div>
                    <div class="flex items-start">
                      <div class="recipe-card-time__number">{{servings || value.servingAmount}}</div>
                      <div class="recipe-card-time__text" style="font-family: arial; text-transform: none">{{ $t('servings') }}</div>
                    </div>
                  </div>
                  <div class="flex flex-col justify-end">
                    <div class="flex justify-end">
                      <div class="recipe-card-time__number">{{value.rating}}/10</div>
                      <baseIcon size="16" class="pl-1" color="#ffc416">icofont-ui-rating</baseIcon>
                    </div>
                    <div class="flex justify-end">
                      <div class="recipe-card-time__number">{{ value.totalRatings }}</div>
                      <div class="recipe-card-time__text" style="font-family: arial; text-transform: none">{{ $t('votes') }}</div>
                    </div>
                  </div>
                </div>
                <div class="recipe-card-text">
                  <div class="py-2">
                    <h4 class="recipe-card-title">{{ value.title || $t('titleHere') }}</h4>
                    <p class="recipe-card-summary">{{ value.summary | maxLength(100) }}</p>
                  </div>
                </div>
            </div>


            <div class="recipe-card__ingredients px-0 py-0 pb-2">
              <div class="recipe-card__ingredients flex flex-wrap py-0 px-0">
              <p v-for="(ingredient, i) in allIngredients" :key="i">
                <span>{{getAmount(ingredient.amount)}}{{getMeasurementById(ingredient.measurementId).unit}}</span> {{ingredient.name}}
              </p>
              </div>
            </div>
            <div class="pt-3 pb-0 flex w-full justify-between py-0 px-0" style="font-size: 11px">
                <p class="flex items-center py-0 px-0 justify-start"><baseIcon size="16" class="pr-1">icofont-ui-calendar</baseIcon>{{ cardDate }} </p>
                <p class="flex items-center py-0 px-0 justify-end">{{value.totalComments }} {{value.totalComments == 1 ? $t('comment') : $t('comments')}} <baseIcon size="16" class="pl-1">icofont-speech-comments</baseIcon></p>
            </div>
        </div>
    </v-card>
</div>
</template>

<script>
import { mapGetters } from 'vuex'
import _ from "lodash";
export default {
    name: "RecipeCard",
    props: {
        value: Object,
        servings: [String, Number]
    },
    data() {
        return {
            hover: false,
            selection: 1,
        };
    },
    computed: {
		...mapGetters('measurement/', ['get_measurements']),
      filteredTags() {
        return this.value.tags?.filter(_ => _.hideInCard != true &&  _.hidden != true)?.slice(0, 5);
      },
      cardDate() {
        return this.$moment.getTimeSinceText(this.value.publishedAt);
      },
      allIngredients() {
        var Vue = this;
        var result = _.flatMap(this.value.ingredientLists, ({ title, ingredients }) =>
          _.map(ingredients, item => ({ title, ...item }))
        );
        var grouped = [];
        result.forEach(function (hash) {
            return function (o) {
                if (!hash[o.ingredientId]) {
                    hash[o.ingredientId] = {
                      ingredientId: o.ingredientId,
                      amount: 0,
                      measurementId: o.measurementId,
                      name: o.name
                    };
                    grouped.push(hash[o.ingredientId]);
                }
                var actualAmount = o.amount;
                if(hash[o.ingredientId].measurementId != o.measurementId) {
                    var baseMeasurement = Vue.get_measurements.find(_ => _.id == o.measurementId);
                    var convertToBase = o.amount * baseMeasurement.conversionAmount;
                    actualAmount = convertToBase;
                }
                hash[o.ingredientId].amount += +actualAmount;
            };
        }(Object.create(null)));

        return grouped
      },
    },
    methods: {
      getMeasurementById(id) {
        if(!id || !this.get_measurements || this.get_measurements.length == 0) return;
        return this.get_measurements.find(_ => _.id == id);
      },
      getAmount(amount) {
        if(this.servings && Number(this.servings) != this.value.servingAmount) {
          var newAmount = amount / this.value.servingAmount;
          return  parseFloat((newAmount * Number(this.servings)).toFixed(8));
        }
        return amount;
      }
    },
};
</script>

<style lang="scss">
    .recipe-card-user {
        display: flex;
        align-items: center;
        border-radius: 20px;
        padding-right: 6px;
      &__icon {
          // background: rgb(179, 179, 179);
          // border-radius: 100%;
          // height: 20px;
          // width: 20px;
          padding-left: 4px;
          padding-top: 0px;
          .v-icon {
            padding-left: 2px;
            margin-bottom: -5px;
            font-size: 18px !important;
          }
      }
      &__name {
        padding-top: 6px;
        padding-left: 4px;
        font-size: 11px;
      }
    }
.recipe-card {
    overflow: hidden;
    position: relative;
    background: #f9f9f9 !important;
    background: -webkit-linear-gradient(top, #f8fafc, #f5f5f5 100%) !important;
    border-radius: 10px !important;
    height: fit-content;
    max-width: 300px;
    -webkit-box-shadow: 0px 0px 4px 0px rgba(0,0,0,0.15) !important;
    box-shadow: 0px 0px 4px 0px rgba(0,0,0,0.15) !important;
    &.hovered {
      transition: box-shadow .4s;
      -webkit-box-shadow: 0px 0px 16px 0px rgba(0,0,0,0.2) !important;
      box-shadow: 0px 0px 16px 0px rgba(0,0,0,0.2) !important;
    }
    .status-label {
      background: rgb(194 132 10 / 71%);
      z-index: 9;
      position: absolute;
      top: 10px;
      right: 10px;
      border-radius: 2px;
      color: #f5f5f5;
      display: inline-block;
      font-size: 11px;
      line-height: normal;
      padding: 5px 10px;
      text-transform: uppercase;
    }
    &-time {
      // color: #FFFFFF;
      // background: #e7843c;
      color: #161e2d;
      background: #ffffff;
      box-shadow: inset 0 0 2px rgba(0,0,0,.3);
      position: absolute;
      top: 10px;
      right: 10px;
      z-index: 1;
      width: 45px;
      height: 45px;
      padding: 6px 0!important;
      -webkit-border-radius: 100%!important;
      -moz-border-radius: 100%!important;
      border-radius: 100%!important;
      font-weight: 700;
      text-align: center;
      -webkti-box-sizing: border-box;
      -moz-box-sizing: border-box;
      box-sizing: border-box;
      &__number {
        font-size: 13px;
        padding: 0 !important;
      }
      &__text {
        padding: 0 !important;
        text-transform: uppercase;
        font-size: 10px;
        margin-top: 3px;
        margin-left: 3px;
        font-weight: 400;
      }
    }
    &-info {
        padding: 8px !important;
        border-top: 0px !important;
    }
    &__image {
        position: relative;
    }
    &__image-tags {
        position: absolute;
        width: 100%;
        z-index: 7;
        bottom: 0;
        display: flex;
        justify-content: flex-end;
        div {
          background: #e7843c;
          color: white;
          // border-top-left-radius: 5px;
          // border-top-right-radius: 5px;
          // margin-right: 2px;
          margin-left: 2px;
          padding: 0px 6px;
          font-size: 12px;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }
    }
    &__image-overlay {
        position: absolute;
        width: 100%;
        z-index: 9;
        height: 100%;
        bottom: 0;
        -moz-box-shadow: inset 0 -10px 10px -10px rgba(46, 46, 46, 0.2) !important;
        -webkit-box-shadow: inset 0 -10px 10px -10px rgba(46, 46, 46, 0.2) !important;
        box-shadow: inset 0 -15px 15px -15px rgba(46, 46, 46, 0.2) !important;
    }
    &__top {
      position: relative;
        font-size: 12px;
        word-wrap: break-word;
        &-left {
            width: 100%;
            float: left;
            color: #0f0f0f;
        }
        .recipe-card-rating {
          // float: left;
        }
        .recipe-card-rating {
        }
    }
    &-title {
      padding-top: 2px;
      line-height: 22px;
      font-size: 16px !important;
      color: #15132b !important;
      font-family: "Roboto", sans-serif !important;
    }
    &__summary {
        font-size: 13px !important;
        color: rgba(46, 46, 46, 0.87) !important;
        font-family: "Roboto", sans-serif !important;
    }
    &__ingredients {
        p {
            width: fit-content;
            font-size: 12px !important;
            // color: rgba(0, 0, 0, 0.87) !important;
            font-family: "Roboto", sans-serif !important;
            font-weight: 500;
            margin-right: 5px;
            margin-top: 5px;
            padding: 2px;
            padding-left: 5px;
            padding-right: 5px;
            // background: #cee7ff;
            position: relative;
            color: rgb(21, 19, 43) !important;
            background: #ebedef;
            border: 1px solid #dde2e6;
            span {
                // display: none;
                padding-right: 2px;
                font-weight: 400;
                font-size: 11px;
                // color: #505050;
                color: #414754;
            }
        }
        &-red p {
            background: #f5ddd6;
            color: #ca1411 !important;
            border: 1px solid #f0c1c0;
        }
        &-green p {
            background: #dcf0d5;
            color: #0a5738 !important;
            border: 1px solid #c6e6d9;
        }
        &-purple p {
            background: #f8ddf2;
            color: #cc43aa !important;
            border: 1px solid #f5bfe7;
        }
        &-orange p {
            background: #fcf3d8;
            color: #aa7306 !important;
            border: 1px solid #fae3b5;
        }
    }

    .icon {
        display: inline-block;
        vertical-align: middle;
        margin: -2px 0 0 2px;
        font-size: 18px;

        & + & {
            padding-left: 10px;
        }
    }
}
</style>
