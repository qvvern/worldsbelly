<template>
  <div class="base-sidebar scrollbar-one" :style="{top: sidebarTop, width: width}" :class="{'scroll-right': scrollRight, 'hide-scroll': !scrollable}">
    <div class="base-sidebar__content" :style="{height: height}">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: "BaseSidebar",
  props: {
    sidebarTop: String,
    width: {
      type: String,
      default: "300px"
    },
    height: {
      type: String,
      default: "auto"
    },
    scrollRight: Boolean,
    scrollable: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      minimized: false,
    };
  },
  computed: {
  },
  methods: {
  },
  mounted() {
  },
};
</script>

<style lang="scss" scoped>
.base-sidebar {
  &.hide-scroll {
    overflow-y: hidden !important;
  }
  &.scroll-right {
      direction:rtl;
  }
  height: 100%;
    max-height: var(--page-height);
    position: sticky;
    top: 0px;
    overflow-x: hidden;
    overflow-y: auto;
    &__content {
      width: 100%;
      direction:ltr
    }
}
</style>
