<template>
    <v-dialog
      v-model="value"
      :width="width"
      :fullscreen="$vuetify.breakpoint.xsOnly"
      @click:outside="closeDialog"
    >
      <v-card>
        <v-card-title class="text-h5">
          <slot name="header"></slot>
        </v-card-title>

        <v-card-text class="mt-4">
          <slot name="body"></slot>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <slot name="footer"></slot>
        </v-card-actions>
      </v-card>
    </v-dialog>
</template>

<script>
  export default {
      name: "baseDialog",
      props: {
        value: [Boolean],
        width: [String],
      },
    data() {
      return {
        active: false,
      }
    },
    computed: {
        classObject() {
          return {
            // primary: this.primary,
            // secondary: this.secondary,
          }
        },
    },
    methods: {
      closeDialog() {
        this.$emit('input', false);
      },
    }
  };
</script>
<style lang="scss" scoped>
.base-icon {
}
</style>
