<template>
    <div class="recipe-comments">
            <div class="recipe-comments-title max-w-screen-lg flex pt-6" style="margin: 0 auto">
                 {{ $t('comments') }}
            </div>
              <section class="comment-module-wrapper max-w-screen-lg flex pt-6" style="margin: 0 auto" v-if="isPageReady">
                <p v-if="noFoundComments" style="padding: 0px 20px;font-style: italic;font-size: 13px;">{{ this.noFoundComments }}</p>
                <p v-else-if="get_comments && get_comments.length == 0" style="padding: 0px 20px;font-style: italic;font-size: 13px;">{{ $t('noComments') }}</p>
                <div class="comment-module">
                  <ul>
                    <Comment
                      v-for="(comment, i) in get_comments"
                      :key="comment.id"
                      v-model="get_comments[i]"
                      :recipeId="recipeId"
                      :owner="recipeByUserId"
                      :level="0"
                    />
                  </ul>
                </div>
                  <div class="comment-module-add" v-show="!get_reply">
                    <p v-if="error" style="color: red">{{error}}</p>
                    <v-textarea
                        :placeholder="$msal.isAuthenticated ? 'Write a comment' : 'Sign in to comment'"
                        :disabled="!$msal.isAuthenticated"
                        outlined
                        :rows="3"
                        auto-grow
                        v-model="comment"
                        type="textarea"
                    >
                      <template v-slot:prepend-inner>
                        <BaseUiButton
                            type="btn2"
                            :disabled="!comment || comment.length < 3 || isCreatingComment"
                            style="padding: 10px 15px"
                            :size="14"
                            @click="addComment()"
                        >
                            {{ $t('send') }}
                        </BaseUiButton>
                      </template>
                    </v-textarea>
                  </div>
              </section>
              <section class="comment-module-wrapper max-w-screen-lg flex flex-col items-center" style="margin: 0 auto; min-height: 200px" v-else>

                  <baseLoading :value="!isPageReady" :progressSentences="[`${$t('preparing')} ${$t('comments')}`]" />
              </section>
    </div>
</template>


<script>
// eslint-disable vue/no-parsing-error
import { mapGetters, mapActions } from "vuex";
export default {
    name: "RecipeContentComments",
    components: {
        Comment: () => import("@/components/base/Comment.vue"),
    },
    props: {
        recipeId: Number,
        recipeByUserId: Number
    },
    data() {
        return {
          isPageReady: false,
          isCreatingComment: false,
          comment: null,
          error: null,
          noFoundComments: null,
        }
    },
    computed: {
      ...mapGetters("comment/", ["get_comments", "get_reply"]),

    },
    methods: {
      ...mapActions('comment/', ['fetch_comments', 'create_comment']),
      addComment() {
        this.error = null;
        this.isCreatingComment = true;
        this.create_comment(
          {
            recipeId: this.recipeId,
            comment: {
              parentCommentId: null,
              text: this.comment
            }
        }).then(() => {
          this.comment = null;
        }).catch((e) => {
          this.error = e;
        }).finally(() => {
          this.isCreatingComment = false;
        })
      }
    },
    async created() {
      await this.fetch_comments(this.recipeId)
        .catch((e) => {
          this.noFoundComments = e;
        }).finally(() => {
          this.isPageReady = true;
        })
    }
}

</script>
<style lang="scss">
    .comment-module-add, .comment-module-reply {
      .fieldset, .v-input__slot {
        background: #ffffff !important;
      }
      .v-input__slot {
        display: flex;
        flex-direction: row-reverse;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 0px !important;
      }
      .v-text-field__details {
        display: none;
      }
      .v-input__prepend-inner {
            margin: 8px 0px;
            padding-left: 5px;
      }
      .v-text-field--outlined fieldset {
          border-right: 0px !important;
          border-bottom: 0px !important;
          border-left: 0px !important;
          color: #e5e5e5 !important;
          border-radius: 0px !important;
      }
    }
.recipe-comments {
  margin-top: 40px;
  &-title {
    padding-left: 35px !important;
    font-size: 18px !important;
    font-family: 'Roboto' !important;
    color: #976b41 !important;
    font-weight: 600 !important;

  }
  width: 100%;
  * {
      margin: 0;
      padding: 0;
      box-sizing: border-box;
      ul {
          list-style: none;
      }
      a {
          text-decoration: none;
      }
  }
      section.comment-module-wrapper {
          width: 100%;
          height: auto;
          // background: #fff;
          // border-radius: 5px;
          border-top: 1px solid #e5e5e5;
          border-left: 1px solid #e5e5e5;
          border-right: 1px solid #e5e5e5;
          border-top-left-radius: 20px;
          border-top-right-radius: 20px;
          background: #fdfdfd;
          margin: 0px auto;
          display: flex;
          flex-direction: column;
          align-items: flex-start;
          .comment-module {
              width: 100%;
              padding: 0px 25px;
              padding-bottom: 20px;
          }
          .comment-module-add {
            display: flex;
            flex-direction: column;
            position: sticky;
            bottom: 0px;
            width: 100%;
            // background: #fdfdfd;
            // border-top: 1px solid #e5e5e5;
            // height: 100px;
            // padding: 10px 50px;
          }
          .comment-module-reply  {
            display: flex;
            flex-direction: column;
            width: 100%;
            border: 1px solid #e5e5e5;
            border-top: 0px;
          }

          ul {
              width: 100%;
              height: 100%;
              display: flex;
              flex-direction: column;
              align-items: flex-start;
              row-gap: 20px;
              margin-left: 0px;
              li {
                  width: 100%;
                  position: relative;
                  .comment {
                      width: 100%;
                      display: flex;
                      column-gap: 10px;
                      &.is-deleting {
                        opacity: 0.5;
                        pointer-events: none;
                      }
                      .comment-content {
                          width: 93%;
                          display: flex;
                          flex-direction: column;
                          row-gap: 6px;
                          .comment-details {
                              width: 100%;
                              display: flex;
                              column-gap: 15px;
                              align-items: center;
                              justify-content: flex-start;
                              .comment-name {
                                  text-transform: capitalize;
                              }
                              .comment-log {
                                  color: #7a7a7a;
                                  font-size: 14px;
                              }
                              .comment-reply, .comment-report, .comment-delete {
                                  color: #272727;
                                  font-size: 13px;
                                  font-weight: 400;
                                  cursor: pointer;
                                  &.active {
                                    font-weight: 600;
                                  }
                              }
                              .comment-delete {
                                  font-size: 12px;
                                  color: #555555;
                              }
                          }
                          .comment-data {
                              width: 100%;
                              display: flex;
                              justify-content: flex-start;
                              align-items: center;
                              column-gap: 20px;
                              * {
                                font-size: 12px;
                              }
                              .comment-likes {
                                  display: flex;
                                  align-items: center;
                                  column-gap: 12px;
                                  > div {
                                      display: flex;
                                      column-gap: 4px;
                                      align-items: center;
                                      img {
                                          cursor: pointer;
                                      }
                                      span {
                                          font-weight: 600;
                                      }
                                  }
                              }
                              .comment-reply, .comment-report, .comment-delete {
                                  color: #272727;
                                  font-weight: 400;
                                  cursor: pointer;
                                  &.active {
                                    font-weight: 600;
                                  }
                              }
                          }
                      }
                  }

                  &::before {
                      content: "";
                      position: absolute;
                      top: 40px;
                      left: 40px;
                      transform: translateX(-25px);
                      width: 2px;
                      height: calc(100% - 40px);
                      background: #c3ab7c;
                  }

                  ul {
                      margin-top: 15px;
                      margin-left: 3%;
                      width: calc(100% - 3%);
                  }
              }
          }
      }

  @media screen and (max-width: 768px) {
      main {
          section.comment-module-wrapper {
              ul {
                  li {
                      .comment {
                          column-gap: 12px;
                          .comment-content {
                              width: 85%;
                              .comment-details {
                                  flex-direction: column;
                                  align-items: flex-start;
                              }
                              .comment-data {
                                  column-gap: 12px;
                              }
                          }
                      }

                      &::before {
                          top: 50px;
                          left: 50px;
                          transform: translateX(-30px);
                          height: calc(100% - 60px);
                      }

                      ul {
                          margin-top: 25px;
                          margin-left: 50px;
                          width: calc(100% - 50px);
                      }
                  }
              }
          }
      }
  }
}
</style>
