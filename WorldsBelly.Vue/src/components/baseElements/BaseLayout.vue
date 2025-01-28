<template>
    <div fluid class="pa-0 base-layout" :class="classes" :style="'overflow-y:' + stickyHeader ? 'hidden' : 'auto'">
        <div class="base-layout-flex">
            <slot name="loader"></slot>
            <div class="base-layout-flex-row__header">
              <slot name="header"></slot>
            </div>
            <div class="base-layout-flex-row__content">
                <div class="base-layout-flex-row__content-inner">
                  <div class="base-layout-flex-row__content-inner__side" v-if="$slots['sidecontent']">
                      <slot name="sidecontent"></slot>
                  </div>
                  <div ref="content" class="base-layout-flex-row__content-inner__content">
                    <slot name="content" v-if="$slots['content']"></slot>
                    <div name="content" v-else>
                        <baseProgressLinear />
                    </div>
                  </div>
                </div>
            </div>
            <div class="base-layout-flex-row__footer" v-if="$slots['footer']">
                <v-layout>
                    <v-flex xs12>
                        <slot name="footer"></slot>
                    </v-flex>
                </v-layout>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "baseLayout",
    props: {
        classes: String,
        stickyHeader: {
            type: Boolean,
            default: true,
        },
        scrollListener: Boolean
    },
    methods: {
      handleScroll(e) {
        this.$emit('scroll', e);
      },
    },
    created () {
      if(this.scrollListener) {
        this.$nextTick(() => {
          this.$refs.content.addEventListener("scroll", this.handleScroll);
        })
      }
    },
    destroyed () {
      if(this.scrollListener && this.$refs.content) {
        this.$refs.content.removeEventListener("scroll", this.handleScroll);
      }
    }
};
</script>

<style lang="scss" scoped>
.base-layout {
    height: 100%;
    max-width: 100%;
    position: relative;
    overflow-x: hidden;
    position: relative;
    display: flex;
    flex-flow: column;
}
.base-layout-flex {
    position: relative;
    display: flex;
    overflow: hidden;
    flex-flow: column;
    width: 100%;
    height: 100%;
    &-row__header {
        flex-grow: 0;
    }
    &-row__content {
        display: flex;
        flex-direction: column;
        flex-grow: 1;
        height: 100%;
        position: relative;
        &-inner {
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            display: flex;
            &__content {
              height: 100%;
              width: 100%;
              overflow-y: auto;
            }
            &__side {
              height: 100%;
            }
        }
    }
    &-row__footer {
        flex: 1 1 auto;
    }
}
</style>
