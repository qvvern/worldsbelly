<template>
  <div class="flex flex-col w-full h-full items-center upload-area-container"
              @mouseover="hoverImage = true"
              @mouseleave="hoverImage = false">
    <div class="upload-area mb-4" @dragover.prevent="dragOver" @drop.prevent="drop($event)">
        <div class="upload-area-drop flex w-full h-full justify-center items-center">
            <div v-if="isDragging" class="drag-overlay"  v-on:dragleave.self="dragLeave" @mouseleave.prevent="dragLeave" />
            <div class="text-center">
                <img class="image" v-if="value && value.startsWith('data')" :src="value" />
                <img class="image" v-else-if="value" :src="'https://worldsbellystorage.blob.core.windows.net'+value+'main'" />
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
                <template v-if="!value">
                  <input ref="fileUpload" @change="onFileChange" type="file" hidden>
                </template>
                <template v-if="!value && !wrongFile">
                  <baseButton icon="icofont-cloud-upload" thick @click.prevent.native="$refs.fileUpload.click()">{{$t('upload')}} {{$t('image')}}</baseButton>
                </template>
            </div>
        </div>
    </div>
    <img v-if="!value" class="image" :style="styleObject" src="@/assets/images/no-image-bg.png"/>
    <div v-else  style="position: absolute; top: 5px; left: 5px; z-index: 2">
        <baseButton icon="icofont-close" red delete xsmall :size="12" @click.prevent.native="removeImage" :disabled="isUploading">{{$t('remove')}} {{$t('image')}}</baseButton>
    </div>
  </div>
</template>

<script>
  export default {
      name: "RecipeDraftsEditImageUpload",
      props: {
        value: String,
      },
      data() {
        return {
          isUploading: false,
          isDragging: false,
          wrongFile: false,
          hoverImage: false,
        }
      },
      computed: {
        styleObject() {
          return {
            opacity: this.hoverImage ? '.9' : '.85'
          }
        },
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
              vm.$emit('input', e.target.result);
            };
            reader.readAsDataURL(file);
          }
          else {
              this.wrongFile = true;
              this.$emit('input', null);
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
          this.$emit('input', null);
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
    width: 100%;
    height: auto;
  }
}
</style>
