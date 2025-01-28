<template>
    <v-dialog
      v-model="isOpen"
      :width="width"
      :fullscreen="$vuetify.breakpoint.xsOnly"
    >
      <v-card>
        <v-card-title class="header text-center">
            <span class="text-center w-full"> <baseIcon value="icofont-info-circle" color="#eb3800" class="m-3" size="28" /></span>
            <span class="text-center w-full" v-html="title || $t('actionCannotBeUndone')"></span>
        </v-card-title>
        <v-card-text class="body text-center">
          <template v-if="$slots.default">
            <slot></slot>
          </template>
          <template v-else>
            <span v-html="description || $t('areYouSureYouWantToContinue')"></span>
          </template>
        </v-card-text>
        <v-card-actions class="footer text-center">
            <span class="text-center w-full mb-4">
                <baseButton tertiary large @click.prevent.native="cancel" class="mr-3">{{ cancelBtn || $t('no') }}</baseButton>
                <baseButton tertiary black large @click.prevent.native="confirm">{{ confirmBtn || $t('yes')}}</baseButton>
            </span>
        </v-card-actions>
      </v-card>
    </v-dialog>
</template>

<script>
  export default {
      name: "BaseConfirmDialog",
      props: {
        value: [Boolean],
        width: {
          type: String,
          default: "400",
        },
        title: {
          type: String,
          default: null,
        },
        cancelBtn: {
          type: String,
          default: null,
        },
        confirmBtn: {
          type: String,
          default: null,
        },
        description: {
          type: String,
          default: null,
        },
      },
    data() {
      return {
        isOpen: false
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
    open() {
      this.isOpen = true;
    },
    close() {
      this.isOpen = false;
    },
    confirm() {
        this.$emit('confirm', true);
        this.close();
    },
    cancel() {
        this.$emit('cancel', true);
        this.close();
    }
  },
  watch: {
    value: {
      immediate: true,
      handler(oldVal, newVal) {
        if(oldVal == newVal) return;
        if(this.value != null) this.isOpen = this.value;
      },
    },
  }
  };
</script>
<style lang="scss" scoped>
</style>
