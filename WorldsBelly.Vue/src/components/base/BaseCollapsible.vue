<template>
  <div class="collapsible-wrapper">
    <div class="collapsible-dot-wrapper">
      <div class="collapsible-dot"><span>{{ index }}</span></div>
    </div>
    <button type="button" class="collapsible" :style="{paddingTop: paddingTop}">
      <div class="flex justify-between items-center">
        <h1>{{ label }}</h1>
        <baseIcon
         @click.prevent.native="expanded = !expanded"
          :value="expanded ? 'icofont-simple-up' : 'icofont-simple-down'"
        ></baseIcon>
      </div>
    </button>
    <div class="collapsible-content" ref="content">
      <template v-if="expanded">
        <slot />
      </template>
      <template v-else>
      ...
      </template>
    </div>
  </div>
</template>

<script>

export default {
  name: "BaseCollapsible",
  components: {
  },
  props: {
      label: String,
      index: Number,
      paddingTop: {
        type: String,
        default: "0px"
      }
  },
  data() {
    return {
      expanded: true,
    }
  },
};
</script>

<style lang="scss" scoped>
.collapsible-wrapper {
  position: relative;
  .collapsible-dot-wrapper {
      position: absolute;
      width: 40px;
      height: 40px;
      left: -45px;
      top: 9px;
      background: #fff;
      display: flex;
      justify-content: center;
      align-items: center;
      border-radius: 100%;
    .collapsible-dot {
      width: 24px;
      height: 24px;
      background: #c3ab7c;
      border-radius: 100%;
      display: flex;
      justify-content: center;
      align-items: center;
      span {
        // padding-left: 1px;
        font-size: 12px;
        font-weight: 500;
        color: #fff;
      }
    }
  }
}
.collapsible {
  background-color: white;
  color: black;
  cursor: auto;
  width: 100%;
  // border-bottom: 0.1px solid #ccc;
  border-collapse: collapse;
  text-align: left;
  outline: 0 !important;
  font-size: 13px;
  .v-icon {
    cursor: pointer;
    color: rgb(195 171 124);
  }
}

// .active,
// .collapsible:hover {
//   background-color: rgb(247, 247, 247);
// }

.collapsible {
  padding: 10px 5px;
  padding-bottom: 0px;
}
.collapsible-content {
  padding: 10px 5px;
  padding-top: 0px;
}
</style>
