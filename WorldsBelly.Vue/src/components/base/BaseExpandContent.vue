<template>
  <div class="base-expand-content" :class="{'is-expanded': expanded}">
    <button type="button" class="expand-content-btn" @click="onExpanded">
      <span class="flex justify-between text-center" style="line-height: auto; clear: both;">
        <p><span>{{ label }}</span><span v-if="labelContext" class="pl-1">({{labelContext}})</span></p>
        <baseIcon
          v-if="expandable"
          class="expand-icon"
          :value="expanded ? 'icofont-simple-up' : 'icofont-simple-down'"
          :size="16"
          color="#808080"
        ></baseIcon>
      </span>
    </button>
    <div class="content pt-1" ref="content" v-show="expanded">
      <slot />
    </div>
  </div>
</template>

<script>
export default {
  name: "BaseExpandContent",
  components: {
  },
  props: {
      label: String,
      labelContext: String,
      expanded: Boolean,
      clearable: Boolean,
      expandable: {
        type: Boolean,
        default: true
      }
  },
  data() {
    return {
      // isExpanded: false
    }
  },
  methods: {
    onExpanded() {
        this.$emit('onClick', !this.expanded);
    }
  },
  created() {
  },
};
</script>

<style lang="scss" scoped>
.base-expand-content {
  // background-color: white;
  // border-bottom: 1px solid #f7f9fb;
    padding-bottom: 10px;
    // border-top: 1px solid #f5f5f5;
  width: 100%;
  &.is-expanded {
    padding-bottom: 20px;
    // background-color: #f7f9fb;
    // border-bottom: 1px solid #fff;
    .expand-icon {
      color: #62ba2e !important;
    }
  }
}
.expand-content-btn {
  cursor: pointer;
  width: 100%;
  // height: 45px;
  text-align: left;
  outline: 0 !important;
  font-size: 13px;
  padding: 0px 20px;
  p {
    font-family: 'Roboto', "Haas Grot Text R Web", "Helvetica Neue", Helvetica, Arial, sans-serif !important;
    font-style: normal;
    text-decoration-skip-ink: auto;
    font-size: 12px;
    font-weight: 600;
    line-height: 1.45455;
    text-transform: uppercase;
    color: #161e2d;
    white-space: nowrap;
  }
}

.expand-content-btn:hover {
  // background-color: rgb(247, 247, 247);
}

.content {
  display: block;
  overflow: hidden;
  padding: 0px 20px;
}
</style>
