<template>
    <li v-if="!isDeleted">
        <div class="comment" :class="{'is-deleting': isDeletingReply}">
            <div class="comment-img">
              <v-avatar
                color="#f7f7f7"
                size="30"
                style="border: 1px solid #d6d6d6"
                class="mr-2"
                :class="{'cursor-pointer': value.createdByUser.id != 0 }"
                @click="value.createdByUser.id == 0 ? '' :$localeRouter.push('/profile/'+value.createdByUser.username+'/recipes')"
              >
                <img v-if="value.createdByUser.image && value.createdByUser.id != 0" :src="'https://worldsbellystorage.blob.core.windows.net'+value.createdByUser.image" />
                <baseIcon v-else class="pt-2" :size="26" color="#253a54" value="icofont-user" />
              </v-avatar>
              <div class="is-owner" v-if="owner == value.createdByUser.id && value.createdByUser.id != 0">
                  <baseIcon v-tooltip="$t('ownerOfRecipe')" color="#3b84b8" :value="'icofont-chef'" :size="18"></baseIcon>
              </div>
            </div>
            <div class="comment-content">
                <div class="comment-details">
                    <h4 class="comment-name cursor-pointer" @click="value.createdByUser.id == 0 ? '' :$localeRouter.push('/profile/'+value.createdByUser.username+'/recipes')">
                    {{ value.createdByUser.username }}
                    </h4>
                    <span class="comment-log">{{ commentDate }}</span>
                    <baseIcon class="cursor-pointer" :value="show ? 'bi-eye-fill' : 'bi-eye-slash-fill'" :size="18" @click.prevent.native="show = !show"></baseIcon>
                    <template v-if="value.createdByUser.id != 0">
                      <div class="comment-reply" @click.prevent="toggleReply()" :class="{active: get_reply == value.id}" v-if="value.createdByUser.id != activeuser">
                          {{ $t('reply') }}
                      </div>
                      <div class="comment-delete" @click.prevent="deleteReply()" v-if="value.createdByUser.id == activeuser">
                          {{ $t('delete') }}
                      </div>
                    </template>
                </div>
                <div class="comment-desc">
                    <p v-if="show">{{ value.translatedText.text }}</p>
                    <p v-else>...</p>
                </div>
                <div class="comment-data" v-if="show">
                    <!-- <div class="comment-likes">
                        <div class="comment-likes-up cursor-pointer">
                            <baseIcon :size="18" color="#253a54" value="icofont-rounded-up" />
                        </div>
                        <div class="comment-likes-rating">
                            <span>2</span>
                        </div>
                        <div class="comment-likes-down cursor-pointer">
                            <baseIcon :size="18" color="#253a54" value="icofont-rounded-down" />
                        </div>
                    </div> -->
                    <!-- <div class="comment-reply" @click="toggleReply()" :class="{active: get_reply == value.id}">
                        Reply
                    </div> -->
                    <!-- <div class="comment-report">
                        Report
                    </div> -->
                </div>
                <div class="comment-module-reply" v-show="get_reply == value.id">
                  <p v-if="error" style="color: red">{{error}}</p>
                  <v-textarea
                      :placeholder="$msal.isAuthenticated ? $t('writeReply') : $t('unSignedInReply')"
                      :disabled="!$msal.isAuthenticated"
                      outlined
                      :rows="3"
                      auto-grow
                      v-model="reply"
                      type="textarea"
                  >
                    <template v-slot:prepend-inner>
                      <BaseUiButton
                          type="btn2"
                          :disabled="!reply || reply.length < 3 || isCreatingReply"
                          style="padding: 10px 15px"
                          :size="14"
                          @click="addReply()"
                      >
                          {{ $t('send') }}
                      </BaseUiButton>
                    </template>
                  </v-textarea>
                </div>
            </div>
        </div>
        <template v-if="show && value.replies && value.replies.length > 0">
          <ul v-if="level < 4">
            <Comment
              v-for="(reply, i) in value.replies"
              :key="reply.id"
              v-model="value.replies[i]"
              :recipeId="recipeId"
              :level="level+1"
            />
          </ul>
          <div class="w-full pt-6" v-else>
            <Comment
              v-for="(reply, i) in value.replies"
              :key="reply.id"
              v-model="value.replies[i]"
              :recipeId="recipeId"
              :level="level+1"
            />
          </div>
        </template>
    </li>
</template>



<script>
import { mapGetters, mapActions, mapMutations } from "vuex";
import _ from "lodash";

export default {
    name: "Comment",
    components: {
    },
    props: {
        value: Object,
        recipeId: Number,
        level: Number,
        owner: Number
    },
    data() {
        return {
          reply: null,
          isCreatingReply: false,
          isDeletingReply: false,
          showReply: false,
          error: null,
          show: true,
          isDeleted: false,
        }
    },
    computed: {
      ...mapGetters("comment/", ["get_reply"]),
      ...mapGetters("user/", ["get_active_user"]),
      commentDate() {
        return this.$moment.getTimeSinceText(this.value.createdAt);
      },
      activeuser() {
        return this.get_active_user?.id;
      }
    },
    destroyed() {
      this.set_reply(null);
    },
    methods: {
      ...mapMutations('comment/', ['set_reply']),
      ...mapActions('comment/', ['create_reply', 'delete_reply']),
      toggleReply() {
        if(this.get_reply == this.value.id) {
          this.set_reply(null);
        }
        else {
          this.set_reply(this.value.id);
        }
      },
      addReply() {
        this.error = null;
        this.isCreatingReply = true;
        this.create_reply(
          {
            recipeId: this.recipeId,
            comment: {
              parentCommentId: this.value.id,
              text: this.reply
            }
        }).then((comment) => {
          this.reply = null;
          var clonedVal = this.value;
          clonedVal.replies.push(comment);
          this.$emit('input', clonedVal);
          this.set_reply(null);
        }).catch((e) => {
          this.error = e;
        }).finally(() => {
          this.isCreatingReply = false;
        })
      },
      deleteReply() {
        this.isDeletingReply = true;
        this.delete_reply(this.value.id).then(() => {
          this.reply = null;
          this.set_reply(null);
          if(this.value.replies && this.value.replies.length > 0) {
            var clonedVal = this.value;
            clonedVal.deletedAt = 'deleted';
            clonedVal.createdByUser = {id: 0, username: 'Deleted Comment'};
            clonedVal.translatedText.text = null;
            this.$emit('input', clonedVal);
          }
          else {
            this.isDeleted = true;
          }
        }).finally(() => {
          this.isDeletingReply = false;
        })
      }
    },
    created() {
      if(this.value.createdByUser.id == 0) {
        this.show = false;
      }
    }
}

</script>
<style lang="scss" scoped>
.comment {
  .comment-img {
    position: relative;
    .is-owner {
      position: absolute;
      top: -20px;
      left: 7px;
    }
  }
}
</style>
