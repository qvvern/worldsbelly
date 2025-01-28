<template>
<div class="w-full flex-col">
  <template>
    <input ref="fileUpload" @change="onFileChange" type="file" hidden>
  </template>
  <div class="step-preview flex w-full items-center justify-center p-2">
    <div class="step-preview-image" ref="step-preview-image" v-if="value || wrongFile" :class="video ? 'mr-2' : ''">
        <div v-if="value">
          <img class="image" v-if="value && value.startsWith('data')" :src="value" />
          <img class="image" v-else-if="value" :src="'https://worldsbellystorage.blob.core.windows.net'+value+'/main'" />
          <div style="position: absolute; top: 10px; left: 10px; z-index: 2">
              <baseButton icon="icofont-close" red delete xsmall :size="12" @click.prevent.native="removeImage" :disabled="isUploading">Remove Image</baseButton>
          </div>
        </div>
        <div v-else class="upload-area-drop__text py-4">
            <v-alert
              prominent
              type="error"
            >
              <v-row align="center">
                <v-col class="grow">
                  Wrong file type
                </v-col>
                <v-col class="shrink">
                  <baseButton icon="icofont-cloud-upload" thick @click.prevent.native="$refs.fileUpload.click()">Try another image</baseButton>
                </v-col>
              </v-row>
            </v-alert>
        </div>
      </div>
    <div class="step-preview-video" v-if="video">
      <div style="position: absolute; top: 10px; left: 10px; z-index: 2">
          <baseButton icon="icofont-close" red delete xsmall :size="12" @click.prevent.native="$emit('removeVideo')">Remove Video</baseButton>
      </div>
      <iframe class="video" width="100%" :height="getHeight" :src="video" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
    </div>
  </div>
  <div>
    <slot v-bind:uploadImage="uploadImage" />
  </div>
</div>
</template>

<script>
  export default {
      name: "RecipeDraftsEditImageUploadStep",
      props: {
        value: String,
        video: String,
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
        getHeight() {
          return this.$refs['step-preview-image'] ? this.$refs['step-preview-image'].clientHeight : '315'
        }
      },
      methods: {
        uploadImage() {
          this.$refs.fileUpload.click();
        },
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
.step-preview {
  &-image {
    position: relative;
    overflow: hidden;
    width: 100%;
    max-width: 75%;
    height: auto;
    border: 1px dashed rgba(0,0,0,.25);
    border-radius: 15px;
    .image {
      width: 100%;
    }
  }
  &-video {
    position: relative;
    overflow: hidden;
    width: 100%;
    max-width: 75%;
    height: fit-content;
    border: 1px dashed rgba(0,0,0,.25);
    border-radius: 15px;
    .video {
      width: 100%;
    }
  }
}
</style>
