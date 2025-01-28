<template>
<div>
  <div class="flex flex-col w-full h-full items-center upload-area-container" v-if="isSameUser && $route.name == 'ProfileSettings'"
              @mouseover="hoverImage = true"
              @mouseleave="hoverImage = false">
    <div class="upload-area mb-4" @dragover.prevent="dragOver" @drop.prevent="drop($event)">
        <div class="upload-area-drop flex w-full h-full justify-center items-center">
            <div v-if="isDragging" class="drag-overlay"  v-on:dragleave.self="dragLeave" @mouseleave.prevent="dragLeave" />
            <div class="text-center">
                <img class="image" v-if="image && image.startsWith('data')" :src="image" />
                <img class="image" v-else-if="image" :src="'https://worldsbellystorage.blob.core.windows.net'+image" />
                <div v-if="wrongFile" class="upload-area-drop__text py-4">
                  <v-alert
                    prominent
                    type="error"
                  >
                    <v-row align="center">
                      <v-col class="grow">
                        {{ $t('wrongFileType') }}
                      </v-col>
                      <v-col class="shrink">
                        <baseButton icon="icofont-cloud-upload" thick @click.prevent.native="$refs.fileUpload.click()">{{$t('tryAnotherImage')}}</baseButton>
                      </v-col>
                    </v-row>
                  </v-alert>
                </div>
                <template v-if="!image">
                  <input ref="fileUpload" @change="onFileChange" type="file" hidden>
                </template>
                <template v-if="!image && !wrongFile">
                  <baseButton icon="icofont-cloud-upload" thick @click.prevent.native="$refs.fileUpload.click()">{{$t('upload')}} {{$t('image')}}</baseButton>
                </template>
            </div>
        </div>
    </div>
    <img v-if="!image" class="image" :style="styleObject" src="@/assets/images/profile-placeholder.png"/>
    <div v-else  style="position: absolute; top: 5px; left: 5px; z-index: 2">
        <baseButton icon="icofont-close" red delete xsmall :size="12" @click.prevent.native="removeImage" :disabled="isUploading">{{$t('remove')}} {{$t('image')}}</baseButton>
    </div>
  </div>
  <div v-else class="upload-area-container">
    <img class="image" v-if="image" :src="'https://worldsbellystorage.blob.core.windows.net'+image" />
    <img v-else class="image" :style="styleObject" src="@/assets/images/profile-placeholder.png"/>
  </div>
</div>
</template>

<script>
import { mapGetters } from "vuex";
import { EventBus } from '@/event-bus'
import _ from "lodash";
  export default {
      name: "ProfileEditImageUpload",
      props: {
        value: String,
      },
      data() {
        return {
          image: null,
          isUploading: false,
          isDragging: false,
          wrongFile: false,
          hoverImage: false,
        }
      },
      computed: {
      ...mapGetters("user/", ["get_active_user"]),
      ...mapGetters("user/", ["get_user"]),
        styleObject() {
          return {
            opacity: this.hoverImage ? '.9' : '.85'
          }
        },
        isSameUser() {
          return this.get_user?.adObjectId == this.get_active_user?.adObjectId
        }
      },
      methods: {
        onFileChange(e) {
          var files = e.target.files || e.dataTransfer.files;
          if (!files.length)
            return;
          this.createImage(files[0]);
        },
        createImage(file) {
          this.wrongFile = false;
          this.isDragging = false;
          if (file.type.indexOf("image/") >= 0) {
            var reader = new FileReader();
            var vm = this;
            reader.onload = (e) => {
              this.image = e.target.result;
            };
            reader.readAsDataURL(file);
          }
          else {
              this.wrongFile = true;
              this.image = _.clone(this.value);
          }
        },
        drop(e) {
            let files = e.dataTransfer.files;
            // allows only 1 file
            if (files.length === 1) {
                let file = files[0];
                this.createImage(file);
            }
        },
        removeImage() {
            this.image = null;
        },
        dragOver(e) {
            e.preventDefault();
            this.isDragging = true;
        },
        dragLeave(e) {
            e.preventDefault();
            this.$nextTick(() => {
              this.isDragging = false;
            })
        },
        resetImage() {
           this.image = _.clone(this.value);
        }
      },
      mounted() {
        if (!EventBus._events.resetProfileImage) {
          EventBus.$on('resetProfileImage', this.resetImage)
        }
      },
      watch: {
        value: {
          handler() {
           this.image = _.clone(this.value);
          }, immediate: true
        },
        // '$route': {
        //   handler() {
        //    this.image = _.clone(this.value);
        //   }, deep: true
        // },
        image: {
          handler() {
            this.$emit("onImageChange", this.image);
          }
        }
      }
  };
</script>

<style lang="scss" scoped>
.upload-area-container {
  .upload-area {
    position: absolute;
    width: 100%;
    height: 100%;
    z-index: 1;
    top:0;
    left: 0;
    &-drop {
        border: 1px solid var(--color-grey-400);
        border-radius: 3px;
        overflow: hidden;
        width: 100%;
        height: 100%;
      &__text {
        font-size: 15px;
        font-weight: 600;
      }
    }
    .drag-overlay {
        position: absolute;
        width: 100%;
        height: 100%;
        border-radius: 3px;
        background-color: rgba(85, 143, 209, 0.5);
    }
  }
  .image {
    top: 50%;
    left: 0;
    transform: translate(0, -50%);
    position: absolute;
    z-index: 0;
    transition: all 400ms;
    width: auto;
    height: 100%;
  }
}
</style>
