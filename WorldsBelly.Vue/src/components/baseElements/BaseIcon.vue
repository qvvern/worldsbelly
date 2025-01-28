<template>
    <v-icon class="base-icon" :size="size" :disabled="disabled" :class="classObject" :color="color">
      <template v-if="value">
        {{ value }}
      </template>
      <template v-else>
        <slot></slot>
      </template>
    </v-icon>
</template>

<script>
  export default {
      name: "baseIcon",
      props: {
        value: [String, Number],
        disabled: [Boolean],
        size: [String, Number],
        color: [String]
      },
    data() {
      return {
      }
    },
    computed: {
        classObject() {
          return {
            // primary: this.primary,
            // secondary: this.secondary,
          }
        },
    }
  };
</script>
<style lang="scss" scoped>
.base-icon {
}
</style>
