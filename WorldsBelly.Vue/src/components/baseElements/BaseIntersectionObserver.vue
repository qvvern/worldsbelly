<template>
  <div class="base-observer">
    <slot />
  </div>
</template>

<script>
export default {
  name: "BaseIntersectionObserver",
  props: {
    options: Object,
  },
  mounted() {
    const options = this.options || {};
    this.observer = new IntersectionObserver(([entry]) => {
      if (entry.rootBounds == null) return;

      const difference = entry.rootBounds.height - entry.intersectionRect.top;

      if (difference < 100) {
        this.$emit("intersecting", entry.isIntersecting);
      }
    }, options);

    this.observer.observe(this.$el);
  },
  destroyed() {
    this.observer.disconnect();
  },
};
</script>
