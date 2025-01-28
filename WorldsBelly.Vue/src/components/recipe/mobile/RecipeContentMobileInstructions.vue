<template>
    <div class="recipe-mb-instructions w-full flex flex-col">
                  <div class="recipe-steps w-full">
                      <div class="recipe-steps-dots"></div>
                      <div class="recipe-content-step-wrapper" v-for="(step, i) in value.steps" :key="i">
                          <div class="recipe-content-step pb-6">
                              <BaseCollapsible :label="step.title" :index="i+1" paddingTop="16px">
                                  <div v-html="step.content"></div>
                                  <div class="recipe-content-step-media w-full flex flex-wrap items-center">
                                      <div
                                          class="recipe-content-step-media-container pt-6 mr-3"
                                          style="width: 100%; max-width: 400px"
                                          v-if="step.videoUrl"
                                      >
                                          <BaseIframe
                                              v-if="step.videoUrl"
                                              :aspectRatio="2"
                                              class="video"
                                              width="100%"
                                              :src="step.videoUrl"
                                              title="YouTube video player"
                                              frameborder="0"
                                              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                              allowfullscreen
                                          ></BaseIframe>
                                      </div>
                                      <div
                                          class="recipe-content-step-media-container pt-6"
                                          style="width: 100%; max-width: 355px"
                                          v-if="step.imageUrl"
                                      >
                                          <base-image
                                              :value="'https://worldsbellystorage.blob.core.windows.net' + step.imageUrl"
                                          />
                                          <!-- <v-img contain :aspect-ratio="16/9" v-if="step.imageUrl" width="100%" :src="'https://worldsbellystorage.blob.core.windows.net'+step.imageUrl" /> -->
                                      </div>
                                  </div>
                              </BaseCollapsible>
                          </div>
                      </div>
                  </div>
                  <div class="w-full ready-to-serve">
                    <baseIcon value="icofont-restaurant" color="#c3ab7c" style="margin-left: -8px" size="33" />
                    <div class="ready-to-serve-text">{{ $t('readyToServe') }}</div>
                  </div>
    </div>
</template>

<script>
/* eslint-disable */
import VueScrollTo from "vue-scrollto";
import { mapGetters, mapMutations, mapActions } from "vuex";
// import _ from "lodash";

export default {
    name: "RecipeContentMobileInstructions",
    props: {
      value: Object
    },
    components: {
        BaseCollapsible: () => import("@/components/base/BaseCollapsible.vue"),
    },
    data() {
        return {
        };
    },
    computed: {
    },
    methods: {
    },
    destroyed() {
    },
    watch: {
    },
};
</script>

<style lang="scss">
.recipe-mb-instructions {
  padding-left: 5px;
  h1 {
    font-size: 18px;
  }
            .ready-to-serve {
              padding-top: 5px;
              padding-left: 7px;
              display: flex;
              align-items: flex-start;
              .ready-to-serve-text {
                padding: 8px 10px;
                font-size: 18px;
                font-family: 'Signika', sans-serif;
                color: #5c5126;
              }
            }
            .recipe-steps {
              padding-top: 10px;
              padding-left: 40px;
              padding-right: 10px;
              padding-bottom: 10px;
              position: relative;
              &-dots {
                background: radial-gradient(ellipse at center, #c3ab7c 20%, #c3ab7c 20%, transparent 40%);
                background-size: 10px 10px;
                background-repeat: repeat-y;
                background-position: 0px center;
                width: 100%;
                position: absolute;
                top: 0;
                left: 10px;
                height: 100%;
              }
                [data-ingredient] {
                    width: fit-content;
                    font-size: 14px !important;
                    color: rgba(0, 0, 0, 0.87) !important;
                    font-family: "Roboto", sans-serif !important;
                    font-weight: 400;
                    margin-right: 0px;
                    margin-top: 5px;
                    padding: 2px;
                    padding-left: 5px;
                    padding-right: 5px;
                    background: #f3e1be;
                    position: relative;
                    display: inline-flex;
                    .i-name {
                        padding-left: 5px;
                        font-weight: 500;
                    }
                }
                &-step-wrapper {
                    width: 100%;
                    margin-bottom: 20px;
                    border-radius: 15px;
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
                        border: 1px dashed rgba(0, 0, 0, 0.25);
                        border-radius: 15px;
                        width: 65px;
                        height: 65px;
                        &.disabled {
                            pointer-events: none;
                            opacity: 0.75;
                            background: rgb(228, 228, 228);
                        }
                        p {
                            padding-top: 2px;
                            font-size: 10px;
                        }
                        &:hover {
                            cursor: pointer;
                            border: 1px dashed rgba(0, 0, 0, 1);
                            transition: all 400ms;
                            background: #fff;
                        }
                    }
                }
            }
}
</style>
