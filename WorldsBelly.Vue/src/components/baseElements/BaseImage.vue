<template>
  <v-responsive class="base-image" :aspect-ratio="16/9">
    <v-icon @click="overlay = !overlay" class="media-expand m-1 p-1" :size="18" color="#fff">icofont-expand</v-icon>
    <v-img class="cursor-pointer" @click="overlay = !overlay" :aspect-ratio="16/9" width="100%" :src="value" />
    <v-overlay
      :z-index="zIndex"
      :value="overlay"
      @click="overlay = false"
      class="w-full"
    >
    <div class="flex w-full h-full p-6">
      <v-img contain :aspect-ratio="16/9" height="100%" :src="value" />
    </div>
    </v-overlay>
  </v-responsive>
</template>

<script>
  export default {
      name: "baseIcon",
      props: {
        value: String,
        zIndex: {
          type: [String, Number],
          default: 99999
        }
      },
    data() {
      return {
        overlay: false,
      }
    },
    computed: {
    }
  };
</script>
<style lang="scss">
.base-image {
  position: relative;
  background: #dcdcdc;
  .v-overlay__content {
    width: 100%;
    height: 100%;
  }
  .v-icon {
    text-shadow: 0px 0px 2px rgb(4, 4, 4);
  }
  .media-expand {
    position: absolute;
    top: 0;
    right: 0;
    z-index: 999;
  }
}
</style>
