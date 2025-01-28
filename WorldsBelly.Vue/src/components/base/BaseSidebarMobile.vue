<template>
  <div class="base-sidebar-mb">
      <transition name="base-sidebar-mb-toggle">
          <div class="base-sidebar-mb__menu" v-if="show">
            <slot></slot>
          </div>
      </transition>
      <transition name="base-sidebar-mb-fade">
          <div class="base-sidebar-mb__fade" v-if="show" @click="onClose"></div>
      </transition>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: 'AppSidebar',
  components: {
  },
  data() {
      return {
          show: false,
      };
  },
  computed: {
  },
  methods: {
      onClose() {
          this.show = false;
          setTimeout(() => {
              this.$emit('close');
          }, 450);
      },
  },
  mounted() {
      this.show = true;
  },
  watch: {
  },
};
</script>

<style lang="scss" scoped>
.base-sidebar-mb {
  position: fixed;
  top: 0;
  left: 0;
  height: 100%;
  width: 100%;
  z-index: 999999999999;
  cursor: auto;
  &__menu {
      position: absolute;
      top: 0;
      left: 0;
      z-index: 1;
      background: #fff;
      display: flex;
      flex-direction: column;
      width: 300px;
      height: 100%;
      overflow-x: hidden;
      overflow-y: auto;
      box-shadow: 0px 2px 20px 0px rgba(0,0,0,0.3);
      &-header {
          &__title {
              position: sticky;
              background: #fff;
              top: 0;
              width: 100%;
              color: #222222;
              font-family: 'Open Sans', 'Inter', sans-serif;
              font-weight: 600;
              font-size: 16px;
              padding: 20px;
              padding-top: 50px;
              padding-bottom: 0px;
          }
          &__close {
            background: #F5F5F5;
            border-radius: 100%;
            width: 30px;
            height: 30px;
            display: flex;
            justify-content: center;
            align-items: center;
            overflow: hidden;
              position: absolute;
              right: 10px;
              top: 10px;
              cursor: pointer;
              &:hover {
                  background: rgba(0,0,0,.1)
              }
          }
      }
      &-content {
          flex: 1;
          display: flex;
          flex-direction: column;
      }
      &-apps {
          padding-top: 10px;
          flex:1;
          &__app {
              padding: 5px 20px;
              color: #000;
              font-family: 'Open Sans', 'Inter', sans-serif;
              font-weight: 400;
              font-size: 16px;
              cursor: pointer;
              display: flex;
              align-items: center;
              &-box {
                  display: flex;
                  justify-content: center;
                  align-items: center;
                  width: 30px;
                  height: 30px;
                  background: #ccc;
                  -webkit-transition: all 0.3s;
                  transition: all 0.3s;
                  h1 {
                      padding: 0px;
                      text-align: center;
                      font-family: 'Open Sans', 'Inter', sans-serif;
                      font-weight: 600;
                      font-size: 14px;
                  }
              }
              p {
                  padding: 0px;
                  line-height: 1px;
                  padding-left: 12px;
                  text-align: center;
                  font-family: 'Open Sans', 'Inter', sans-serif;
                  font-weight: 400;
                  font-size: 14px;
              }
              &:hover {
                  background: rgba(0,0,0,.1)
              }
              &.active {
                  background: rgba(0,0,0,.1);
                  opacity: 0.4;
                  pointer-events: none;
                  cursor: auto;
              }
          }
      }
      &-recents {
          ul {
              margin: 0;
              padding: 0 20px;
              list-style: none;
          }
          li {
              margin: 12px 0 0 0;
              padding: 0 0 12px;
              border-bottom: 1px solid #eee;
              cursor: pointer;
              .headline {
                  display: flex;
                  align-items: center;
                  padding-bottom: 5px;
                  img {
                      height: 14px;
                      padding-right: 10px;
                  }
              }
              &:last-child {
                  border-bottom: none;
              }
              p {
                  color: #000;
                  font-size: 12px;
                  line-height: 16px;
                  word-break: break-all;
                  &.headline {
                      text-transform: uppercase;
                  }
                  &.subheadline {
                      color:#666;
                  }
              }
          }
      }
      &-footer {
          color: #aaa;
          font-size: 10px;
          padding: 20px;
      }
  }
  &__fade {
      position: absolute;
      top: 0;
      left: 0;
      background: rgba(0,0,0,.2);
      height: 100%;
      width: 100%;
  }
}
.base-sidebar-mb-toggle-enter-active,
.base-sidebar-mb-toggle-leave-active {
  transform: translateX(0);
  transition: transform 0.5s;
}

.base-sidebar-mb-toggle-enter,
.base-sidebar-mb-toggle-leave-to {
  transform: translateX(-100%);
}

.base-sidebar-mb-fade-enter-active,
.base-sidebar-mb-fade-leave-active {
transition: opacity 0.4s;
}
.base-sidebar-mb-fade-enter,
.base-sidebar-mb-fade-leave-to {
opacity: 0;
}
</style>
