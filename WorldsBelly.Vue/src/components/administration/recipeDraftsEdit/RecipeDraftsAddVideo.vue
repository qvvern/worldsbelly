<template>
  <div>
      <v-menu
        v-model="menu"
        transition="slide-x-transition"
        bottom
        :close-on-content-click="false"
      >
        <template v-slot:activator="{ on, attrs }">
          <div class="flex items-center cursor-pointer"
            v-bind="attrs"
            v-on="on">
             <slot></slot>
          </div>
        </template>
      <v-card>

      <v-list-item three-line>
        <v-list-item-content class="my-0">
          <div class="text-overline mb-2">
            {{ $t('add') }} {{ $t('video') }}
          </div>
          <v-list-item class="py-0 px-1">
            <baseInput
              :placeholder="`${$t('add')} youtube ${$t('link')}`"
              innerIcon="icofont-brand-youtube"
              v-model="clonedValue"
              outlined
              style="height: 60px"
              clearable
              :error="isValid || !clonedValue ? '' : 'youtube '+$t('urlIsNotValid')"
            />
          </v-list-item>
        </v-list-item-content>
      </v-list-item>
        <v-card-actions class="flex justify-end mt-0 pt-0 mr-3 pb-3">
          <v-btn
            outlined
            text
            @click="close()"
          >
            {{ $t('cancel') }}
          </v-btn>
          <v-btn
            v-if="!clonedValue && value"
            outlined
            text
            @click="removeVideo()"
          >
            {{ $t('update') }}
          </v-btn>
          <v-btn
            v-else
            outlined
            text
            @click="addVideo()"
            :disabled="!isValid"
          >
            {{value ? $t('update') : $t('add')}}
          </v-btn>
        </v-card-actions>
      </v-card>
      </v-menu>
  </div>
</template>

<script>
import {validateUrl} from 'youtube-validate'
import _ from "lodash";
  export default {
      name: "RecipeDraftsAddVideo",
      components: {
      },
      props: {
        value: String,
      },
      data() {
        return {
          clonedValue: null,
          menu: false,
          isValid: true
        }
      },
      methods: {
        getYoutubeLink(url) {
          var youtubeId = null;
          if(url.includes('watch')) {
            var test = url.split('=')[1];
            youtubeId = test;
            if(youtubeId.includes('&')) {
              youtubeId = youtubeId.split('&')[0];
            }
          }
          else if(url.includes('youtu.be')) {
              youtubeId = url.split('youtu.be/')[1];
          }
          else if(url.includes('embed')) {
            youtubeId = url.split('embed/')[1];
          }
          if(youtubeId?.length > 5) {
            return `https://www.youtube.com/embed/${youtubeId}`;
          }
          return;
        },
        addVideo() {
          if(this.validUrl(this.clonedValue)) {
            this.$emit('input', _.cloneDeep(this.getYoutubeLink(this.clonedValue)));
          }
          this.close();
        },
        removeVideo() {
          this.$emit('input', null);
          this.close();
        },
        validUrl(url) {
          if(url?.startsWith('https://www.youtube.com/') || url?.startsWith('https://youtu.be/')) return true;
          return false;
        },
        close() {
          this.clonedValue = null;
          this.isValid = false;
          this.menu = false;
        }
      },
    watch: {
        clonedValue: {
          immediate: true,
          handler() {
            if(this.validUrl(this.clonedValue)) {
                this.isValid = true;
            }
            else {
                this.isValid = false;
            }
          },
        },
        menu() {
          this.clonedValue = _.cloneDeep(this.value);
          if(this.menu != true) return;
        }
    }
  };
</script>

<style lang="scss" scoped>
.recipe-drafts-edit-actions {
}
</style>
