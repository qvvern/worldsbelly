<template>
    <v-progress-linear
      indeterminate
      :color="color"
    ></v-progress-linear>
</template>

<script>
    export default {
        name: "baseProgressLinear",
        props: {
          color: {
            type: [String],
            default: "#62ba2e"
          },
        },
    };
</script>

<style lang="scss" scoped>
</style>
