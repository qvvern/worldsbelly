<template>
  <div class="base-box" :class="padding">
    <div class="base-box__title" :class="paddingTitle">{{ title }} <slot name="header"></slot></div>
    <div class="base-box__text" :class="{'base-box-scrollable': scrollable, 'scrollbar-one': scrollable}"><slot></slot></div>
  </div>
</template>

<script>
  export default {
      name: "baseBox",
      props: {
        title: [String],
        scrollable: [Boolean],
        padding: {
          type: String,
          default: 'p-4'
        },
        paddingTitle: {
          type: String,
          default: ''
        }
      },
    data() {
      return {
      }
    },
  };
</script>
<style lang="scss" scoped>
.base-box-scrollable {
  overflow-y: auto;
}
.base-box {
  position: relative;
  background: white;
  border-radius: 10px;
  box-shadow: #9e9e9e24 0px 0px 6px 0px;
    // box-shadow: rgba(0, 23, 55, 0.2) 0px 24px 60px 0px;
    transition: box-shadow 300ms ease-in-out 0s;
    border: 1px solid #ececec;
  display: flex;
  flex-flow: column;
  &__title {
      font-weight: 500;
      color: #091736;
      text-transform: uppercase;
      font-size: 14px;
  }
  &__text {
    flex: 1;
  }
}
</style>
